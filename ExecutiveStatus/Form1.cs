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
            cmbReason.Items.AddRange(new string[] { "ȸ��", "����", "�ܱ�", "�ް�", "����", "��Ÿ" });
            cmbReason.Enabled = false;
        }

        private void rdoPresent_CheckedChanged(object sender, EventArgs e)
        {
            cmbReason.Enabled = !rdoPresent.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string status = rdoPresent.Checked ? "���" : "����";
            string reason = rdoPresent.Checked ? "" : cmbReason.Text;
            string memo = txtMemo.Text;

            // �޽��� �ڽ� Ȯ��
            string message = $"�̸�: {name}\n����: {status}";
            if (status == "����")
                message += $"\n����: {reason}";
            message += $"\n�޸�: {memo}";

            MessageBox.Show(message, "�Է°� Ȯ��");

            // ����Ʈ�� �߰�
            string[] row = { name, status, reason, memo };
            ListViewItem item = new ListViewItem(row);
            listViewStatus.Items.Add(item);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // �ʿ� �� ����
        }
        private void listViewStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ���õ� �׸��� ���� ��� ���� �ؽ�Ʈ�ڽ��� ä������ ���� �ֽ��ϴ�.
            if (listViewStatus.SelectedItems.Count > 0)
            {
                var item = listViewStatus.SelectedItems[0];
                txtName.Text = item.SubItems[0].Text;
                string status = item.SubItems[1].Text;
                rdoPresent.Checked = status == "���";
                rdoAbsent.Checked = status == "����";
                cmbReason.Text = item.SubItems[2].Text;
                txtMemo.Text = item.SubItems[3].Text;
            }
        }
    }
}