using System;
using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace ICPLib
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //EnvUtil.SetPathBefore(Environment.CurrentDirectory + "\\vtk\\bin");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ICPTestForm());
        }
    }
}
