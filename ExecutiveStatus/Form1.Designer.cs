using System.Drawing;
using System.Windows.Forms;

namespace ExecutiveStatus
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // 컨트롤
        private Label lblStatus;
        private Label IbIName;
        private ComboBox cmbReason;
        private Label IbIStatus;
        private RadioButton rdoPresent;
        private RadioButton rdoAbsent;
        private Label IbIReason;
        private Label IbIMemo;
        private TextBox txtMemo;
        private Button btnSave;
        private Button btnDelete;
        private TextBox txtName;
        private ListView listViewStatus;
        private ColumnHeader 이름;
        private ColumnHeader 상태;
        private ColumnHeader 사유;
        private ColumnHeader 메모;

        // 필터 UI
        private Label lblFilter;
        private ComboBox cmbFilterField;
        private TextBox txtFilter;

        // 스타일용 패널
        private Panel panelForm;
        private Panel panelButtons;

        /// <summary>Designer가 소유한 리소스 정리</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ===== 패널(카드 레이아웃) =====
            this.panelForm = new Panel();
            this.panelButtons = new Panel();

            // ===== 라벨/컨트롤 =====
            this.IbIName = new Label();
            this.cmbReason = new ComboBox();
            this.IbIStatus = new Label();
            this.rdoPresent = new RadioButton();
            this.rdoAbsent = new RadioButton();
            this.IbIReason = new Label();
            this.IbIMemo = new Label();
            this.txtMemo = new TextBox();
            this.btnSave = new Button();
            this.btnDelete = new Button();
            this.txtName = new TextBox();

            this.lblFilter = new Label();
            this.cmbFilterField = new ComboBox();
            this.txtFilter = new TextBox();

            this.listViewStatus = new ListView();
            this.이름 = new ColumnHeader();
            this.상태 = new ColumnHeader();
            this.사유 = new ColumnHeader();
            this.메모 = new ColumnHeader();
            this.lblStatus = new Label();

            // ===== 폼 =====
            this.SuspendLayout();
            this.Text = "임원 재실 현황 관리";
            this.ClientSize = new Size(860, 560);
            this.BackColor = Form1.Theme.Background;

            // ===== 입력 패널(panelForm) =====
            this.panelForm.BackColor = Form1.Theme.Card;
            this.panelForm.Location = new Point(16, 16);
            this.panelForm.Size = new Size(828, 170);
            this.panelForm.Padding = new Padding(12);
            this.panelForm.BorderStyle = BorderStyle.FixedSingle;

            // 이름
            this.IbIName.AutoSize = true;
            this.IbIName.Location = new Point(12, 14);
            this.IbIName.Text = "이름";
            this.IbIName.ForeColor = Form1.Theme.Text;

            this.txtName.Location = new Point(60, 10);
            this.txtName.Size = new Size(220, 23);

            // 상태
            this.IbIStatus.AutoSize = true;
            this.IbIStatus.Location = new Point(300, 14);
            this.IbIStatus.Text = "상태";
            this.IbIStatus.ForeColor = Form1.Theme.Text;

            this.rdoPresent.Location = new Point(340, 12);
            this.rdoPresent.Text = "재실";
            this.rdoPresent.AutoSize = true;
            this.rdoPresent.CheckedChanged += new System.EventHandler(this.rdoPresent_CheckedChanged);

            this.rdoAbsent.Location = new Point(400, 12);
            this.rdoAbsent.Text = "부재";
            this.rdoAbsent.AutoSize = true;
            this.rdoAbsent.CheckedChanged += new System.EventHandler(this.rdoAbsent_CheckedChanged);

            // 사유
            this.IbIReason.AutoSize = true;
            this.IbIReason.Location = new Point(12, 46);
            this.IbIReason.Text = "사유";
            this.IbIReason.ForeColor = Form1.Theme.Text;

            this.cmbReason.Location = new Point(60, 42);
            this.cmbReason.Size = new Size(220, 23);

            // 메모
            this.IbIMemo.AutoSize = true;
            this.IbIMemo.Location = new Point(12, 76);
            this.IbIMemo.Text = "메모";
            this.IbIMemo.ForeColor = Form1.Theme.Text;

            this.txtMemo.Location = new Point(60, 72);
            this.txtMemo.Size = new Size(520, 60);

            // 버튼 패널(panelButtons)
            this.panelButtons.Location = new Point(600, 72);
            this.panelButtons.Size = new Size(200, 60);
            this.panelButtons.BackColor = Form1.Theme.Card;

            // 저장 버튼
            this.btnSave.Location = new Point(0, 0);
            this.btnSave.Size = new Size(80, 28);
            this.btnSave.Text = "저장";
            StylePrimaryButton(this.btnSave);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // 선택 삭제
            this.btnDelete.Location = new Point(100, 0);
            this.btnDelete.Size = new Size(80, 28);
            this.btnDelete.Text = "삭제";
            StyleDangerButton(this.btnDelete);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Controls.Add(this.btnDelete);

            // 입력 패널에 컨트롤 추가
            this.panelForm.Controls.Add(this.IbIName);
            this.panelForm.Controls.Add(this.txtName);
            this.panelForm.Controls.Add(this.IbIStatus);
            this.panelForm.Controls.Add(this.rdoPresent);
            this.panelForm.Controls.Add(this.rdoAbsent);
            this.panelForm.Controls.Add(this.IbIReason);
            this.panelForm.Controls.Add(this.cmbReason);
            this.panelForm.Controls.Add(this.IbIMemo);
            this.panelForm.Controls.Add(this.txtMemo);
            this.panelForm.Controls.Add(this.panelButtons);

            // ===== 필터 영역 =====
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new Point(20, 200);
            this.lblFilter.Text = "검색";
            this.lblFilter.ForeColor = Form1.Theme.Text;

            this.cmbFilterField.Location = new Point(60, 196);
            this.cmbFilterField.Size = new Size(90, 23);
            this.cmbFilterField.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFilterField.Items.AddRange(new object[] { "전체", "이름", "상태", "사유", "메모" });
            this.cmbFilterField.SelectedIndex = 0;

            this.txtFilter.Location = new Point(156, 196);
            this.txtFilter.Size = new Size(220, 23);

            // ===== 리스트 =====
            this.listViewStatus.Location = new Point(16, 228);
            this.listViewStatus.Size = new Size(828, 280);
            this.listViewStatus.View = View.Details;
            this.listViewStatus.FullRowSelect = true;
            this.listViewStatus.GridLines = false;
            this.listViewStatus.HideSelection = false;
            this.listViewStatus.MultiSelect = true;
            this.listViewStatus.UseCompatibleStateImageBehavior = false;
            this.listViewStatus.BackColor = Form1.Theme.Card;
            this.listViewStatus.ForeColor = Form1.Theme.Text;
            this.listViewStatus.BorderStyle = BorderStyle.FixedSingle;

            // OwnerDraw + 이벤트(헤더/행/서브아이템)
            this.listViewStatus.OwnerDraw = true;
            this.listViewStatus.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.listViewStatus_DrawColumnHeader);
            this.listViewStatus.DrawItem += new DrawListViewItemEventHandler(this.listViewStatus_DrawItem);
            this.listViewStatus.DrawSubItem += new DrawListViewSubItemEventHandler(this.listViewStatus_DrawSubItem);

            this.이름.Text = "이름";
            this.이름.Width = 180;
            this.상태.Text = "상태";
            this.상태.Width = 100;
            this.사유.Text = "사유";
            this.사유.Width = 180;
            this.메모.Text = "메모";
            this.메모.Width = 330;

            this.listViewStatus.Columns.AddRange(new ColumnHeader[] {
                this.이름, this.상태, this.사유, this.메모
            });

            this.listViewStatus.SelectedIndexChanged += new System.EventHandler(this.listViewStatus_SelectedIndexChanged);
            this.listViewStatus.ColumnClick += new ColumnClickEventHandler(this.listViewStatus_ColumnClick);
            this.listViewStatus.KeyDown += new KeyEventHandler(this.listViewStatus_KeyDown);

            // ===== 상태 라벨 =====
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new Point(16, 516);
            this.lblStatus.Text = "상태 표시줄";
            this.lblStatus.ForeColor = Form1.Theme.Muted;

            // ===== 폼에 추가 =====
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.cmbFilterField);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.listViewStatus);
            this.Controls.Add(this.lblStatus);

            // 폼 이벤트
            this.Load += new System.EventHandler(this.Form1_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // --- 버튼 스타일 도우미 ---
        private void StylePrimaryButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Form1.Theme.Accent;
            b.ForeColor = Color.White;
            b.FlatAppearance.MouseOverBackColor = Form1.Theme.AccentDark;
        }
        private void StyleDangerButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Form1.Theme.Danger;
            b.ForeColor = Color.White;
            b.FlatAppearance.MouseOverBackColor = Form1.Theme.DangerDark;
        }
    }
}
