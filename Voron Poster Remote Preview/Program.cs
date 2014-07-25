using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Voron_Poster_Remote_Preview
{

    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                string path = Path.GetTempPath() + "cryptRDP5.exe";
                File.WriteAllBytes(path, Voron_Poster_Remote_Preview.Properties.Resources.cryptRDP5);
                Process PasswordEncryptor = new Process();
                PasswordEncryptor.StartInfo.FileName = Path.GetTempPath() + "cryptRDP5.exe";
                PasswordEncryptor.StartInfo.Arguments = "poster";
                PasswordEncryptor.StartInfo.UseShellExecute = false;
                PasswordEncryptor.StartInfo.RedirectStandardOutput = true;
                PasswordEncryptor.StartInfo.CreateNoWindow = true;
                PasswordEncryptor.Start();

                File.WriteAllText(Path.GetTempPath() + "PosterTest.rdp",
                    Voron_Poster_Remote_Preview.Properties.Resources.RdpFileBegin
                    + PasswordEncryptor.StandardOutput.ReadToEnd()
                    + Voron_Poster_Remote_Preview.Properties.Resources.RdpFileEnd);
                PasswordEncryptor.WaitForExit();
                File.Delete(Path.GetTempPath() + "cryptRDP5.exe");

                Process rdp = new Process();
                rdp.StartInfo.FileName = "mstsc.exe";
                rdp.StartInfo.Arguments = Path.GetTempPath() + "PosterTest.rdp";
                rdp.Start();
                Thread.Sleep(1000);
                rdp.WaitForExit();
                File.Delete(Path.GetTempPath() + "PosterTest.rdp");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    File.Delete(Path.GetTempPath() + "cryptRDP5.exe");
                    File.Delete(Path.GetTempPath() + "PosterTest.rdp");
                }
                catch { }                       
            }

        }
    }
}
