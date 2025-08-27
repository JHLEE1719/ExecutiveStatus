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
            // 단일 인스턴스 보장 (트레이 에이전트 충돌 방지)
            using var mutex = new Mutex(true, "ExecutiveStatusAgent.SingleInstance", out bool isNew);
            if (!isNew)
            {
                MessageBox.Show("이미 실행 중입니다. 트레이 아이콘을 확인하세요.",
                    "ExecutiveStatus", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // WinForms 표준 초기화 (.NET Framework 호환 방식)
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 메인 폼 실행
            Application.Run(new Form1());
        }
    }
}
