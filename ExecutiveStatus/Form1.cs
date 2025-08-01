using System;
using System.Windows.Forms;

namespace ExecutiveStatus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbReason.Items.AddRange(new string[] { "회의", "출장", "외근", "휴가", "교육", "기타" });
            cmbReason.Enabled = false;
        }

        private void rdoPresent_CheckedChanged(object sender, EventArgs e)
        {
            cmbReason.Enabled = !rdoPresent.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string status = rdoPresent.Checked ? "재실" : "부재";
            string reason = rdoPresent.Checked ? "" : cmbReason.Text;
            string memo = txtMemo.Text.Trim();

            // 유효성 검사
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("이름을 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (!rdoPresent.Checked && string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("부재 사유를 선택하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbReason.Focus();
                return;
            }

            // 메시지 박스 확인
            string message = $"이름: {name}\n상태: {status}";
            if (status == "부재")
                message += $"\n사유: {reason}";
            message += $"\n메모: {memo}";

            MessageBox.Show(message, "입력값 확인");

            if (listViewStatus.SelectedItems.Count > 0)
            {
                // 선택된 항목 수정
                ListViewItem selected = listViewStatus.SelectedItems[0];
                selected.SubItems[0].Text = name;
                selected.SubItems[1].Text = status;
                selected.SubItems[2].Text = reason;
                selected.SubItems[3].Text = memo;
            }
            else
            {
                // 새 항목 추가
                string[] row = { name, status, reason, memo };
                ListViewItem item = new ListViewItem(row);
                listViewStatus.Items.Add(item);
            }

            // 입력 필드 초기화
            txtName.Text = "";
            rdoPresent.Checked = true; // 기본값을 '재실'로 설정
            cmbReason.SelectedIndex = -1;
            cmbReason.Enabled = false;
            txtMemo.Text = "";
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // 필요 시 구현
        }
        private void listViewStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewStatus.SelectedItems.Count == 0)
                return;

            ListViewItem selected = listViewStatus.SelectedItems[0];
            txtName.Text = selected.SubItems[0].Text;
            string status = selected.SubItems[1].Text;
            cmbReason.Text = selected.SubItems[2].Text;
            txtMemo.Text = selected.SubItems[3].Text;

            if (status == "재실")
                rdoPresent.Checked = true;
            else
                rdoAbsent.Checked = true;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewStatus.SelectedItems.Count > 0)
            {
                listViewStatus.Items.Remove(listViewStatus.SelectedItems[0]);
            }
            else
            {
                MessageBox.Show("삭제할 항목을 선택하세요.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewStatus.SelectedItems.Count > 0)
            {
                ListViewItem item = listViewStatus.SelectedItems[0];

                // 기존 값 입력 필드로 불러오기
                txtName.Text = item.SubItems[0].Text;
                string status = item.SubItems[1].Text;
                rdoPresent.Checked = (status == "재실");
                rdoAbsent.Checked = (status == "부재");
                cmbReason.Text = item.SubItems[2].Text;
                txtMemo.Text = item.SubItems[3].Text;

                // 항목 삭제 (수정 후 저장 시 다시 추가되도록)
                listViewStatus.Items.Remove(item);
            }
            else
            {
                MessageBox.Show("수정할 항목을 선택하세요.");
            }
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {

        }
    }
}