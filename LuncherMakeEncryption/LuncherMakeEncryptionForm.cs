using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuncherMakeEncryption
{
    public partial class LuncherMakeEncryptionForm : Form
    {
        public LuncherMakeEncryptionForm()
        {
            InitializeComponent();
        }

        static Random r = new Random();
        static string[] Useless = new string[] { " Thread.Sleep(0);", " ", "  ", "   "};

        private static string Fill(string Input, int Length, bool ForceAppend)
        {
            StringBuilder Res = new StringBuilder(Input);
            while (Res.Length != Length)
            {
                string AddS;
                if (r.Next(2) == 0) AddS = Useless[r.Next(Useless.Length)];
                else AddS = " Useless = " + r.Next(10000).ToString() + ";";
                if (Res.Length + AddS.Length > Length) Res.Append(new string(' ', Length - Res.Length));
                else { if (ForceAppend || r.Next(2) == 0 ) Res.Append(AddS); else Res.Insert(0, AddS); }
            }
            Res.Replace("\n", " ");
            return Res.ToString();
        }

        List<string> ss;
        private void button1_Click(object sender, EventArgs e)
        {
            ss = new List<string>();
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String000);
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String001);
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String002);
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String003);
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String004);
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String005);
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String006);
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String007);
            for (int i = 0; i < 10; i++)
            ss.Add(" w++; ");
            FormStrings(250);
        }

        private void FormStrings(int Count)
        {
            int MLen = 0;
            for (int i = 0; i < ss.Count; i++)
                MLen = Math.Max(MLen, ss[i].Length);
                ss[0] = Fill(ss[0], MLen, true);
            for (int i = 1; i < ss.Count; i++)
                ss[i] = Fill(ss[i], MLen, false);
            ss.Add(Fill("", MLen, false));
            ss.Add(Fill("", MLen, false));
            for (int i = ss.Count; i < Count; i++)
                ss.Insert(r.Next(ss.Count-1)+1, Fill("", MLen, false));
            textBox1.Lines = ss.ToArray();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var AES = new AesCryptoServiceProvider();
            AES.Key = new byte[256 / 8] { 240, 119, 82, 224, 93, 215, 250, 43, 78, 192, 95, 229, 166, 27, 4, 105, 40, 251, 211, 19, 190, 77, 207,
                    34, 116, 39, 244, 211, 54, 212, 5, 205 };
            AES.IV = new byte[128 / 8] { 58, 41, 152, 142, 81, 7, 185, 181, 153, 139, 240, 86, 160, 125, 97, 233 };
            var c = AES.CreateEncryptor();
            List<string> sss = new List<string>();
            for (int i = 0; i < ss.Count; i++)
            {
                byte[] b = Encoding.UTF8.GetBytes(ss[i]);
                b = c.TransformFinalBlock(b, 0, b.Length);
                
                // Generate AES key
                //b = new byte[256/8];
                //for (int i3 = 0; i3 < b.Length; i3++)
                //{
                //    b[i3] = (byte)r.Next(256);
                //}

              //  sss.Add("a.Insert(r.Next(a.Count-1)+1," + string.Join(" ", GetFormatedByteArray(b))+ ");");
                if ( i == 0)
                    sss.Add("a.Add(");
                else
                sss.Add("a.Insert(r.Next(a.Count-1)+1,");
                sss.AddRange(GetFormatedByteArray(b));
                sss.Add(");");
            }
            textBox1.Lines = sss.ToArray();
        }

        private string[] GetFormatedByteArray(byte[] b)
        {
            List<string> ls = new List<string>();
                StringBuilder s = new StringBuilder("new byte[] {");
                for (int i2 = 0; i2 < b.Length - 1; i2++)
                {
                    s.Append("" + b[i2].ToString() + ",");
                    if (s.Length > 80)
                    {
                        ls.Add(s.ToString());
                        s.Clear();
                    }
                }
                ls.Add(s.ToString());
                s.Clear();
                ls[ls.Count-1]=(ls.Last()+"" + b[b.Length-1].ToString() + "}");
                return ls.ToArray();
        }

        private void fff(int Len)
        {
            ss = new List<string>();
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String000+ " byte[] bbb = new byte["+Len.ToString()+"]; ");
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String100);
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String101);
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String102);
            ss.Add(global::LuncherMakeEncryption.Properties.Resources.String007);
            FormStrings(50);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] b = System.IO.File.ReadAllBytes(textBox2.Text);
           // return;
            var AES = new AesCryptoServiceProvider();
            AES.Key = new byte[256 / 8] { 240, 119, 82, 224, 93, 205, 250, 43, 78, 192, 95, 229, 166, 27, 4, 104, 40, 251, 211, 20, 190, 77, 207,
                    34, 116, 39, 244, 211, 54, 212, 5, 205 };
            AES.IV = new byte[128 / 8] { 58, 41, 152, 142, 81, 7, 185, 181, 153, 139, 240, 86, 160, 125, 97, 233 };
            var c = AES.CreateEncryptor();
            b = c.TransformFinalBlock(b, 0, b.Length);
            ss = new List<string>();
            int dd = 1000;
            int blocksize = (b.Length / dd);
            for (int i = 0; i < dd; i++)
            {

                byte[] subb = new byte[blocksize];
                for (int i2 = i * blocksize; i2 < (i + 1) * blocksize; i2++)
                    subb[i2 - (i * blocksize)] = b[i2];
                //var s = new StringBuilder("xxx.TryAdd(" + i.ToString() + ",new byte[]={");
                //int i2 = 0;
                //for (i2 = i*(b.Length/dd); i2 < Math.Min(b.Length, (i+1)*(b.Length/dd))-1 ; i2++)
                //{
                //    s.Append(b[i2].ToString());
                //    s.Append(",");
                //}
                //s.Append(b[i2].ToString());
                //s.Append();
                ss.Add("xxx.Add(" + i.ToString() + ",");
                    ss.AddRange(GetFormatedByteArray(subb));
                        ss.Add(");");
            }
            byte[] subbl = new byte[b.Length-blocksize*dd];
            for (int i2 = dd * blocksize; i2 < b.Length; i2++)
                subbl[i2 - (dd * blocksize)] = b[i2];
            ss.Add("xxx.Add(" + dd.ToString() + ",");
            ss.AddRange(GetFormatedByteArray(subbl));
            ss.Add(");");
            textBox1.Lines = ss.ToArray();
            //FormStrings(1001);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] b = System.IO.File.ReadAllBytes(textBox2.Text);
            var AES = new AesCryptoServiceProvider();
            AES.Key = new byte[256 / 8] { 240, 119, 82, 224, 93, 205, 250, 43, 78, 192, 95, 229, 166, 27, 4, 104, 40, 251, 211, 20, 190, 77, 207,
                    34, 116, 39, 244, 211, 54, 212, 5, 205 };
            AES.IV = new byte[128 / 8] { 58, 41, 152, 142, 81, 7, 185, 181, 153, 139, 240, 86, 160, 125, 97, 233 };
            var c = AES.CreateEncryptor();
            b = c.TransformFinalBlock(b, 0, b.Length);
            fff(b.Length);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string s = global::LuncherMakeEncryption.Properties.Resources.StringBeg;
            button4_Click(sender, e);
            button3_Click(sender, e);
            s += textBox1.Text;
            button2_Click(sender, e);
            s += textBox1.Text;
               s+=global::LuncherMakeEncryption.Properties.Resources.StringEnd;
               textBox1.Text = s;
               System.IO.File.WriteAllText(textBox3.Text, s);
        }
    }
}
