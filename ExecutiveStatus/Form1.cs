using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExecutiveStatus
{
    public partial class Form1 : Form
    {
        // --- 내부 데이터 모델 ---
        private class Record
        {
            public long Id;
            public string Name = "";
            public string Status = "";
            public string Reason = "";
            public string Memo = "";
        }

        private List<Record> records = new List<Record>();   // 전체 데이터 캐시
        private long? selectedRowId = null;                  // 현재 선택 레코드
        private int sortColumn = 0;                          // 0:이름, 1:상태, 2:사유, 3:메모
        private bool sortAsc = true;                         // 정렬 방향

        public Form1()
        {
            InitializeComponent();

            // 메모 박스 UI
            txtMemo.Multiline = true;
            txtMemo.ScrollBars = ScrollBars.Vertical;
            txtMemo.WordWrap = true;

            // 라디오/콤보 기본
            rdoPresent.Checked = true;
            rdoAbsent.Checked = false;
            cmbReason.DropDownStyle = ComboBoxStyle.DropDownList;

            // 필터 기본
            cmbFilterField.SelectedIndex = 0; // "전체"
            txtFilter.TextChanged += (_, __) => ApplyFilterSortAndRender();
            cmbFilterField.SelectedIndexChanged += (_, __) => ApplyFilterSortAndRender();

            UpdateReasonUI();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 사유 목록
            cmbReason.Items.Clear();
            cmbReason.Items.AddRange(new string[] { "회의", "출장", "외근", "휴가", "교육", "기타" });

            // DB 파일 준비 및 스키마 보정
            string dbPath = GetDbPath();
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                using var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conn.Open();
                using var cmd = new SQLiteCommand(@"
                    CREATE TABLE IF NOT EXISTS Status (
                        Id     INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name   TEXT,
                        Status TEXT,
                        Reason TEXT,
                        Memo   TEXT
                    );", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("DB 파일 executive_status.db 가 생성되었습니다.");
            }
            else
            {
                // 기존에 Name PRIMARY KEY였던 경우 자동 마이그레이션
                EnsureSchema();
            }

            LoadDataFromDB();
            ApplyFilterSortAndRender();
        }

        private string GetDbPath() =>
            Path.Combine(Application.StartupPath, "executive_status.db");

        /// <summary>
        /// 기존 테이블에 Name PK같은 제약이 있으면
        /// Id INTEGER PRIMARY KEY AUTOINCREMENT 구조로 마이그레이션
        /// </summary>
        private void EnsureSchema()
        {
            try
            {
                using var conn = new SQLiteConnection($"Data Source={GetDbPath()};Version=3;");
                conn.Open();

                // 현재 스키마 점검
                var cols = new List<(string name, bool pk)>();
                using (var cmd = new SQLiteCommand("PRAGMA table_info(Status);", conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        string name = rd["name"]?.ToString() ?? "";
                        int pk = Convert.ToInt32(rd["pk"]);
                        cols.Add((name, pk == 1));
                    }
                }

                bool hasId = cols.Any(c => c.name.Equals("Id", StringComparison.OrdinalIgnoreCase));
                bool nameIsPk = cols.Any(c => c.name.Equals("Name", StringComparison.OrdinalIgnoreCase) && c.pk);

                if (!hasId || nameIsPk)
                {
                    using var tx = conn.BeginTransaction();

                    // 새 테이블 생성
                    using (var create = new SQLiteCommand(@"
                        CREATE TABLE IF NOT EXISTS Status_new(
                            Id     INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name   TEXT,
                            Status TEXT,
                            Reason TEXT,
                            Memo   TEXT
                        );", conn, tx))
                        create.ExecuteNonQuery();

                    // 데이터 이관
                    using (var copy = new SQLiteCommand(
                        "INSERT INTO Status_new (Name, Status, Reason, Memo) SELECT Name, Status, Reason, Memo FROM Status;", conn, tx))
                        copy.ExecuteNonQuery();

                    // 교체
                    using (var drop = new SQLiteCommand("DROP TABLE Status;", conn, tx))
                        drop.ExecuteNonQuery();
                    using (var rename = new SQLiteCommand("ALTER TABLE Status_new RENAME TO Status;", conn, tx))
                        rename.ExecuteNonQuery();

                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("스키마 보정 중 오류: " + ex.Message);
            }
        }

        // --- DB 로드 ---
        private void LoadDataFromDB()
        {
            try
            {
                using var conn = new SQLiteConnection($"Data Source={GetDbPath()};Version=3;");
                conn.Open();

                // Id가 있으면 그걸 쓰고, 없어도 ROWID 별칭으로 Id를 얻을 수 있음
                string sql = "SELECT COALESCE(Id, rowid) AS Id, Name, Status, Reason, Memo FROM Status ORDER BY COALESCE(Id, rowid)";
                using var cmd = new SQLiteCommand(sql, conn);
                using SQLiteDataReader reader = cmd.ExecuteReader();

                records.Clear();
                while (reader.Read())
                {
                    records.Add(new Record
                    {
                        Id = (long)reader["Id"],
                        Name = reader["Name"]?.ToString() ?? "",
                        Status = reader["Status"]?.ToString() ?? "",
                        Reason = reader["Reason"]?.ToString() ?? "",
                        Memo = reader["Memo"]?.ToString() ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB 로드 중 오류 발생: " + ex.Message);
            }
        }

        // --- UI 상태 ---
        private void UpdateReasonUI()
        {
            bool absent = rdoAbsent.Checked;
            cmbReason.Enabled = absent;
            if (!absent)
            {
                cmbReason.SelectedIndex = -1;
                cmbReason.Text = string.Empty;
            }
        }

        private void rdoPresent_CheckedChanged(object sender, EventArgs e) => UpdateReasonUI();
        private void rdoAbsent_CheckedChanged(object sender, EventArgs e) => UpdateReasonUI();

        private void listViewStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewStatus.SelectedItems.Count == 0)
            {
                selectedRowId = null;
                return;
            }

            var selected = listViewStatus.SelectedItems[0];
            txtName.Text = selected.SubItems[0].Text;

            rdoPresent.Checked = (selected.SubItems[1].Text == "재실");
            rdoAbsent.Checked = !rdoPresent.Checked;

            cmbReason.Text = selected.SubItems[2].Text;
            txtMemo.Text = selected.SubItems[3].Text;

            selectedRowId = (long?)selected.Tag;
            UpdateReasonUI();
        }

        // --- 저장(추가/수정) ---
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string status = rdoPresent.Checked ? "재실" : "부재";
            string reason = rdoPresent.Checked ? "" : cmbReason.Text;
            string memo = txtMemo.Text.Trim();

            // 유효성
            bool ok = true;
            if (string.IsNullOrWhiteSpace(name))
            {
                txtName.BackColor = Color.MistyRose; ok = false;
            }
            else txtName.BackColor = Theme.Window;

            if (!rdoPresent.Checked && string.IsNullOrWhiteSpace(reason))
            {
                cmbReason.BackColor = Color.MistyRose; ok = false;
            }
            else cmbReason.BackColor = Theme.Window;

            if (!ok)
            {
                MessageBox.Show("필수 입력 항목을 확인하세요.");
                return;
            }

            try
            {
                using var conn = new SQLiteConnection($"Data Source={GetDbPath()};Version=3;");
                conn.Open();

                if (selectedRowId.HasValue)
                {
                    string usql = @"UPDATE Status
                                    SET Name=@Name, Status=@Status, Reason=@Reason, Memo=@Memo
                                    WHERE COALESCE(Id, rowid)=@Id";
                    using var ucmd = new SQLiteCommand(usql, conn);
                    ucmd.Parameters.AddWithValue("@Name", name);
                    ucmd.Parameters.AddWithValue("@Status", status);
                    ucmd.Parameters.AddWithValue("@Reason", reason);
                    ucmd.Parameters.AddWithValue("@Memo", memo);
                    ucmd.Parameters.AddWithValue("@Id", selectedRowId.Value);
                    ucmd.ExecuteNonQuery();
                }
                else
                {
                    string isql = "INSERT INTO Status (Name, Status, Reason, Memo) VALUES (@Name,@Status,@Reason,@Memo)";
                    using var icmd = new SQLiteCommand(isql, conn);
                    icmd.Parameters.AddWithValue("@Name", name);
                    icmd.Parameters.AddWithValue("@Status", status);
                    icmd.Parameters.AddWithValue("@Reason", reason);
                    icmd.Parameters.AddWithValue("@Memo", memo);
                    icmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB 저장 중 오류: " + ex.Message);
                return;
            }

            lblStatus.Text = $"저장 완료 - {DateTime.Now:HH:mm:ss}";
            ClearInputs();
            LoadDataFromDB();
            ApplyFilterSortAndRender();
        }

        // --- 선택행 삭제 & Del 키 ---
        private void btnDelete_Click(object sender, EventArgs e) => DeleteSelectedRows();
        private void listViewStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedRows();
                e.Handled = true;
            }
        }
        private void DeleteSelectedRows()
        {
            if (listViewStatus.SelectedItems.Count == 0)
            {
                MessageBox.Show("삭제할 항목을 선택하세요.");
                return;
            }

            int n = listViewStatus.SelectedItems.Count;
            if (MessageBox.Show($"{n}건을 삭제하시겠습니까?", "확인",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                using var conn = new SQLiteConnection($"Data Source={GetDbPath()};Version=3;");
                conn.Open();

                using var tx = conn.BeginTransaction();
                string dsql = "DELETE FROM Status WHERE COALESCE(Id, rowid)=@Id";
                using var dcmd = new SQLiteCommand(dsql, conn, tx);

                foreach (ListViewItem it in listViewStatus.SelectedItems)
                {
                    if (it.Tag is long id)
                    {
                        dcmd.Parameters.Clear();
                        dcmd.Parameters.AddWithValue("@Id", id);
                        dcmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("삭제 중 오류: " + ex.Message);
                return;
            }

            lblStatus.Text = $"삭제 완료 - {DateTime.Now:HH:mm:ss}";
            ClearInputs();
            LoadDataFromDB();
            ApplyFilterSortAndRender();
        }

        // --- 정렬/필터/렌더 ---
        private void listViewStatus_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (sortColumn == e.Column) sortAsc = !sortAsc;
            else { sortColumn = e.Column; sortAsc = true; }
            ApplyFilterSortAndRender();
        }

        private bool MatchFilter(Record r)
        {
            string key = (txtFilter.Text ?? "").Trim();
            if (key.Length == 0) return true;

            StringComparison cmp = StringComparison.OrdinalIgnoreCase;
            switch (cmbFilterField.SelectedItem?.ToString())
            {
                case "이름": return r.Name.IndexOf(key, cmp) >= 0;
                case "상태": return r.Status.IndexOf(key, cmp) >= 0;
                case "사유": return r.Reason.IndexOf(key, cmp) >= 0;
                case "메모": return r.Memo.IndexOf(key, cmp) >= 0;
                default:      // 전체
                    return r.Name.IndexOf(key, cmp) >= 0
                        || r.Status.IndexOf(key, cmp) >= 0
                        || r.Reason.IndexOf(key, cmp) >= 0
                        || r.Memo.IndexOf(key, cmp) >= 0;
            }
        }

        private IEnumerable<Record> SortRecords(IEnumerable<Record> seq)
        {
            Func<Record, object> key = sortColumn switch
            {
                0 => r => r.Name,
                1 => r => r.Status,
                2 => r => r.Reason,
                3 => r => r.Memo,
                _ => r => r.Id
            };
            return sortAsc ? seq.OrderBy(key) : seq.OrderByDescending(key);
        }

        private void ApplyFilterSortAndRender()
        {
            var filtered = records.Where(MatchFilter);
            var ordered = SortRecords(filtered);

            listViewStatus.BeginUpdate();
            listViewStatus.Items.Clear();

            foreach (var r in ordered)
            {
                var item = new ListViewItem(new[] { r.Name, r.Status, r.Reason, r.Memo })
                {
                    Tag = r.Id
                };
                listViewStatus.Items.Add(item);
            }
            listViewStatus.EndUpdate();

            UpdateSortGlyph();
            listViewStatus.Invalidate(); // OwnerDraw 재그리기
        }

        // 헤더 텍스트에 정렬 방향 화살표 표시
        private void UpdateSortGlyph()
        {
            string[] baseTexts = { "이름", "상태", "사유", "메모" };
            for (int i = 0; i < baseTexts.Length; i++)
            {
                string arrow = (i == sortColumn) ? (sortAsc ? " ▲" : " ▼") : "";
                listViewStatus.Columns[i].Text = baseTexts[i] + arrow;
            }
        }

        private void ClearInputs()
        {
            txtName.Text = "";
            rdoPresent.Checked = true;
            rdoAbsent.Checked = false;
            cmbReason.SelectedIndex = -1;
            txtMemo.Text = "";
            selectedRowId = null;
            listViewStatus.SelectedItems.Clear();
            txtName.Focus();
            UpdateReasonUI();
        }

        // =========================
        // OwnerDraw: 헤더/행/서브아이템 (상태 배지 포함)
        // =========================
        private void listViewStatus_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using var headerBg = new SolidBrush(Theme.Header);
            using var headerBorder = new Pen(Theme.HeaderBorder);
            e.Graphics.FillRectangle(headerBg, e.Bounds);
            e.Graphics.DrawRectangle(headerBorder, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);

            TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            var textRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 16, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, e.Header.Text, listViewStatus.Font, textRect, Theme.Text, flags);
        }

        private void listViewStatus_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            bool selected = (e.State & ListViewItemStates.Selected) != 0;
            Color back = selected ? Theme.RowSelected : ((e.ItemIndex % 2 == 0) ? Theme.RowLight : Theme.RowAlt);
            using var bg = new SolidBrush(back);
            e.Graphics.FillRectangle(bg, e.Bounds);

            if ((e.State & ListViewItemStates.Focused) != 0)
                e.DrawFocusRectangle();
        }

        private void listViewStatus_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            bool selected = e.Item.Selected;
            Color fore = selected ? Theme.RowSelectedText : Theme.Text;

            if (e.ColumnIndex == 1) // 상태 배지
            {
                string status = e.SubItem.Text.Trim();
                bool present = status == "재실";

                Rectangle r = e.Bounds;
                SizeF textSize = e.Graphics.MeasureString(status, listViewStatus.Font);
                int padH = 8, padV = 4;
                int badgeW = (int)Math.Min(r.Width - 10, textSize.Width + padH * 2);
                int badgeH = (int)Math.Min(r.Height - 6, textSize.Height + padV * 2);
                Rectangle badge = new Rectangle(r.X + 8, r.Y + (r.Height - badgeH) / 2, badgeW, badgeH);

                Color badgeBg = present ? Theme.BadgePresent : Theme.BadgeAbsent;
                using var path = new System.Drawing.Drawing2D.GraphicsPath();
                int radius = 8, d = radius * 2;
                path.AddArc(badge.X, badge.Y, d, d, 180, 90);
                path.AddArc(badge.Right - d, badge.Y, d, d, 270, 90);
                path.AddArc(badge.Right - d, badge.Bottom - d, d, d, 0, 90);
                path.AddArc(badge.X, badge.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var sb = new SolidBrush(badgeBg)) e.Graphics.FillPath(sb, path);
                TextRenderer.DrawText(e.Graphics, status, listViewStatus.Font,
                    badge, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
            else
            {
                Rectangle r = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 12, e.Bounds.Height);
                TextRenderer.DrawText(e.Graphics, e.SubItem.Text, listViewStatus.Font, r, fore,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            }
        }

        // --- 테마 팔레트 ---
        private static class Theme
        {
            public static readonly Color Background = Color.FromArgb(246, 248, 250); // 폼 배경
            public static readonly Color Card = Color.White;
            public static readonly Color Accent = Color.FromArgb(52, 152, 219);      // 저장 버튼
            public static readonly Color AccentDark = Color.FromArgb(41, 128, 185);
            public static readonly Color Danger = Color.FromArgb(231, 76, 60);       // 삭제
            public static readonly Color DangerDark = Color.FromArgb(192, 57, 43);
            public static readonly Color Muted = Color.FromArgb(108, 117, 125);

            public static readonly Color Header = Color.FromArgb(242, 244, 246);
            public static readonly Color HeaderBorder = Color.FromArgb(220, 224, 228);
            public static readonly Color RowLight = Color.White;
            public static readonly Color RowAlt = Color.FromArgb(245, 247, 249);
            public static readonly Color RowSelected = Color.FromArgb(225, 236, 250);
            public static readonly Color RowSelectedText = Color.FromArgb(25, 35, 45);
            public static readonly Color Text = Color.FromArgb(45, 52, 54);

            // 배지 색
            public static readonly Color BadgePresent = Color.FromArgb(46, 204, 113); // 재실
            public static readonly Color BadgeAbsent = Color.FromArgb(189, 62, 55);  // 부재

            public static readonly Color Window = SystemColors.Window;
        }
    }
}