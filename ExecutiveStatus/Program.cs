using System;
using System.Threading;
using System.Windows.Forms;

namespace ExecutiveStatus
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // ���� �ν��Ͻ� ���� (Ʈ���� ������Ʈ �浹 ����)
            using var mutex = new Mutex(true, "ExecutiveStatusAgent.SingleInstance", out bool isNew);
            if (!isNew)
            {
                MessageBox.Show("�̹� ���� ���Դϴ�. Ʈ���� �������� Ȯ���ϼ���.",
                    "ExecutiveStatus", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // WinForms ǥ�� �ʱ�ȭ (.NET Framework ȣȯ ���)
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ���� �� ����
            Application.Run(new Form1());
        }
    }
}
