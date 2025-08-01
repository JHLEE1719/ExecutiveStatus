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
            string name = txtName.Text.Trim();
            string status = rdoPresent.Checked ? "���" : "����";
            string reason = rdoPresent.Checked ? "" : cmbReason.Text;
            string memo = txtMemo.Text.Trim();

            // ��ȿ�� �˻�
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("�̸��� �Է��ϼ���.", "�Է� ����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (!rdoPresent.Checked && string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("���� ������ �����ϼ���.", "�Է� ����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbReason.Focus();
                return;
            }

            // �޽��� �ڽ� Ȯ��
            string message = $"�̸�: {name}\n����: {status}";
            if (status == "����")
                message += $"\n����: {reason}";
            message += $"\n�޸�: {memo}";

            MessageBox.Show(message, "�Է°� Ȯ��");

            if (listViewStatus.SelectedItems.Count > 0)
            {
                // ���õ� �׸� ����
                ListViewItem selected = listViewStatus.SelectedItems[0];
                selected.SubItems[0].Text = name;
                selected.SubItems[1].Text = status;
                selected.SubItems[2].Text = reason;
                selected.SubItems[3].Text = memo;
            }
            else
            {
                // �� �׸� �߰�
                string[] row = { name, status, reason, memo };
                ListViewItem item = new ListViewItem(row);
                listViewStatus.Items.Add(item);
            }

            // �Է� �ʵ� �ʱ�ȭ
            txtName.Text = "";
            rdoPresent.Checked = true; // �⺻���� '���'�� ����
            cmbReason.SelectedIndex = -1;
            cmbReason.Enabled = false;
            txtMemo.Text = "";
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // �ʿ� �� ����
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

            if (status == "���")
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
                MessageBox.Show("������ �׸��� �����ϼ���.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewStatus.SelectedItems.Count > 0)
            {
                ListViewItem item = listViewStatus.SelectedItems[0];

                // ���� �� �Է� �ʵ�� �ҷ�����
                txtName.Text = item.SubItems[0].Text;
                string status = item.SubItems[1].Text;
                rdoPresent.Checked = (status == "���");
                rdoAbsent.Checked = (status == "����");
                cmbReason.Text = item.SubItems[2].Text;
                txtMemo.Text = item.SubItems[3].Text;

                // �׸� ���� (���� �� ���� �� �ٽ� �߰��ǵ���)
                listViewStatus.Items.Remove(item);
            }
            else
            {
                MessageBox.Show("������ �׸��� �����ϼ���.");
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