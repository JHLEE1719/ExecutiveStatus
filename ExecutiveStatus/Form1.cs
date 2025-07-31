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
            string name = txtName.Text;
            string status = rdoPresent.Checked ? "재실" : "부재";
            string reason = rdoPresent.Checked ? "" : cmbReason.Text;
            string memo = txtMemo.Text;

            // 메시지 박스 확인
            string message = $"이름: {name}\n상태: {status}";
            if (status == "부재")
                message += $"\n사유: {reason}";
            message += $"\n메모: {memo}";

            MessageBox.Show(message, "입력값 확인");

            // 리스트에 추가
            string[] row = { name, status, reason, memo };
            ListViewItem item = new ListViewItem(row);
            listViewStatus.Items.Add(item);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // 필요 시 구현
        }
        private void listViewStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 선택된 항목이 있을 경우 값을 텍스트박스로 채워넣을 수도 있습니다.
            if (listViewStatus.SelectedItems.Count > 0)
            {
                var item = listViewStatus.SelectedItems[0];
                txtName.Text = item.SubItems[0].Text;
                string status = item.SubItems[1].Text;
                rdoPresent.Checked = status == "재실";
                rdoAbsent.Checked = status == "부재";
                cmbReason.Text = item.SubItems[2].Text;
                txtMemo.Text = item.SubItems[3].Text;
            }
        }
    }
}