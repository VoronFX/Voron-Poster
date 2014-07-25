using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luncher2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            var a = Assembly.Load(global::Luncher2.Properties.Resources.Voron_Poster);
            var m = a.EntryPoint;
            var ts = new System.Threading.ThreadStart(() => { try { if (m != null) m.Invoke(null, null); } catch { } });
            var thd = new System.Threading.Thread(ts);
            thd.SetApartmentState(ApartmentState.STA);
            thd.Start();
        }
    }
}
