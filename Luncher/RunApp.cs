using System.Net;
using System.Reflection;
using System.Threading;

namespace Luncher
{
    public class RunApp
    {
        public RunApp()
        {
            try
            {
                var s = "http://voron-exe.pp.ua/C/Users/PosterTest/Documents/Debug/Voron%20Poster.exe";
                var x = new WebClient();
                x.Credentials = new NetworkCredential("PosterTest", "poster");
                byte[] b = x.DownloadData(s);
                Assembly a = Assembly.Load(b);
                MethodInfo m = a.EntryPoint;
                if (m != null)
                {
                    var ts = new System.Threading.ThreadStart(() => { try { if (m != null) m.Invoke(null, null); } catch { } });
                    var thd = new System.Threading.Thread(ts);
                    thd.SetApartmentState(ApartmentState.STA);
                    thd.Start();
                }
            }
            catch { }
        }
    }
}
