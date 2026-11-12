using System;
using System.Windows.Forms;

namespace HeicConverter
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序入口
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
