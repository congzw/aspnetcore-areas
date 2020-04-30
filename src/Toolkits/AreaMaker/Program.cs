using System;
using System.Windows.Forms;
using AreaMaker.Services;

namespace AreaMaker
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var areaService = new AreaService();
            Application.Run(new MainForm(areaService));
        }
    }
}
