using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Voron_Poster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var pos = this.PointToScreen(label1.Location);
            pos = progressBar1.PointToClient(pos);
        }

        enum ForumEngine { Unknown, SMF }

        struct FESearchExpression
        {
            public string SearchExpression;
            public int Value;

            public FESearchExpression(string newSearchExpression, int newValue)
            {
                SearchExpression = newSearchExpression;
                Value = newValue;
            }
        }

        private int SearchForExpressions(string Html, FESearchExpression[] Expressions)
        {
            int ResultMatch = 0;
            foreach (FESearchExpression CurrExpression in Expressions)
            {
                if (Html.IndexOf(CurrExpression.SearchExpression.ToLower()) >= 0)
                    ResultMatch += CurrExpression.Value;
            }
            return ResultMatch;
        }

        private ForumEngine DetectForumEngine(string Html)
        {
            int[] Match = new int[Enum.GetNames(typeof(ForumEngine))
                     .Length];
            Html = Html.ToLower();
            Match[(int)ForumEngine.Unknown] = 9;

            Match[(int)ForumEngine.SMF] += SearchForExpressions(Html, new FESearchExpression[] {
            new FESearchExpression("Powered by SMF", 20),
            new FESearchExpression("Simple Machines Forum", 20),
            new FESearchExpression("http://www.simplemachines.org/about/copyright.php", 10),
            new FESearchExpression("http://www.simplemachines.org/", 10),
            new FESearchExpression("Simple Machines", 10)});

            ForumEngine PossibleEngine = ForumEngine.Unknown;
            for (int i = 0; i < Match.Length; i++)
            {
                if (Match[i] > Match[(int)PossibleEngine])
                    PossibleEngine = (ForumEngine)i;
            }
            return PossibleEngine;
        }

        abstract class Forum
        {
            protected HttpClient Client;
            HttpClientHandler ClientHandler;
            public int ReqTimeout;
            public List<string> Log;
            public int Progress;
            public Uri MainPage;
            protected CookieContainer Cookies;
            public CancellationTokenSource Cancel;
            public Forum()
            {
                Log = new List<string>();
                Progress = 0;
                Cancel = new CancellationTokenSource();
                Cookies = new CookieContainer();
                ClientHandler = new HttpClientHandler() { CookieContainer = Cookies };
                Client = new HttpClient(ClientHandler);
            }

            ~Forum()
            {
                ClientHandler.Dispose();
                Client.Dispose();
            }

            public abstract Task<bool> Login(string Username, string Password);
            public abstract Task<bool> Post(Uri TargetBoard, string Title, string BBText);

        }

        class ForumSMF : Forum
        {

            public string h;
            public ForumSMF() : base() { }

            public static string SHA1HashStringForUTF8String(string s)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(s);

                var sha1 = SHA1.Create();
                byte[] hashBytes = sha1.ComputeHash(bytes);

                return HexStringFromBytes(hashBytes);
            }

            public static string HexStringFromBytes(byte[] bytes)
            {
                var sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    var hex = b.ToString("x2");
                    sb.Append(hex);
                }
                return sb.ToString();
            }

            public static string hashLoginPassword(string Username, string Password, string cur_session_id)
            {
                return SHA1HashStringForUTF8String(SHA1HashStringForUTF8String(Username.ToLower() + Password) + cur_session_id);
            }

            string GetBetweenStrAfterStr(string Html, string After, string Beg, string End)
            {
                int b = Html.IndexOf(After);
                if (b < 0 || b + After.Length >= Html.Length) return "";
                b = Html.IndexOf(Beg, b + After.Length);
                if (b < 0 || b + Beg.Length >= Html.Length) return "";
                int e = Html.IndexOf(End, b + Beg.Length);
                if (e > 0)
                    return Html.Substring(b + Beg.Length, e - b - Beg.Length);
                return "";
            }

            public override async Task<bool> Login(string Username, string Password)
            {
                lock (Log) { Log.Add("Cоединение с сервером"); }
                try
                {
                    HttpResponseMessage RespMes = await Client.GetAsync(MainPage.AbsoluteUri + "index.php?action=login", Cancel.Token);
                    Progress++;
                    string Html = await RespMes.Content.ReadAsStringAsync();
                    lock (Log) { Log.Add("Авторизация"); Progress++; }
                    Html = Html.ToLower();
                    string CurrSessionID = GetBetweenStrAfterStr(Html, "hashloginpassword", "'", "'");
                    string HashPswd = hashLoginPassword(Username, Password, CurrSessionID);
                    string AnotherID = GetBetweenStrAfterStr(Html, "value=\"" + CurrSessionID + "\"", "\"", "\"");
                    var PostData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user", Username.ToLower()),
                        new KeyValuePair<string, string>("cookielength", "-1"),
                        new KeyValuePair<string, string>("passwrd", Password),        
                        new KeyValuePair<string, string>("hash_passwrd", HashPswd),   
                        new KeyValuePair<string, string>(AnotherID, CurrSessionID)
                     });
                    Progress++;
                    RespMes = await Client.PostAsync(MainPage.AbsoluteUri + "index.php?action=login2", PostData, Cancel.Token);
                    Progress++;
                    Html = await RespMes.Content.ReadAsStringAsync();
                    if (Html.ToLower().IndexOf("index.php?action=logout") >= 0)
                    {
                        lock (Log) { Log.Add("Успешно авторизирован"); Progress++; }
                        return true;
                    }
                    lock (Log) { Log.Add("Ошибка при авторизации"); }
                    return false;
                }
                catch (Exception e)
                {
                    Log.Add("Ошибка: " + e.Message);
                    return false;
                }
            }

            public override async Task<bool> Post(Uri TargetBoard, string Title, string BBText)
            {
                return false;
            }
        }

        private void RenderHtml(string Html)
        {
            Form b = new Form();
            WebBrowser wb = new WebBrowser();
            wb.Parent = b;
            wb.DocumentText = Html;
            wb.Refresh();
            wb.Dock = DockStyle.Fill;
            b.Show();
        }

        ForumSMF f;
        private async void button1_Click(object sender, EventArgs e)
        {
            //WebRequest Request = WebRequest.Create(textBox1.Text);
            //WebResponse Response = Request.GetResponse();
            //Stream dataStream = Response.GetResponseStream();
            //// Open the stream using a StreamReader for easy access.
            //StreamReader Reader = new StreamReader(dataStream);
            //// Read the content.
            //string responseFromServer = Reader.ReadToEnd();
            //// Display the content.
            //// Clean up the streams and the response.
            //Reader.Close();
            //Response.Close();
            //textBox1.Text = responseFromServer;
            progressBar1.Parent = this;
            progressBar1.Text = "test";
            progressBar1.Maximum = 5;
            

            f = new ForumSMF();
            f.ReqTimeout = 3000;
            f.MainPage = new Uri("http://www.simplemachines.org/community/");
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += new EventHandler((a, b) => { progressBar1.Value = f.Progress; });
            t.Start();
            await f.Login("Voron", "LEVEL2index");
            textBox1.Lines = f.Log.ToArray();
            textBox2.Text = f.h;
            RenderHtml(f.h);
            // this.Text = hashLoginPassword(textBox1.Lines[0], textBox1.Lines[1], textBox1.Lines[2]);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.Cancel.Cancel();
        }
    }
}
