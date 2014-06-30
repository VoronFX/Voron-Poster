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

            public int ReqTimeout;
            public List<string> Log;
            public int Progress;
            Thread Thread;
            public Uri MainPage;
            protected CookieContainer Cookies;
            public Forum() { 
                Log = new List<string>();
                Progress = 0;
            }
            public abstract bool Login(string Username, string Password);

            protected bool TryRequestGet(string Uri, out string Result)
            {
                Result = String.Empty;
                try
                {
                    HttpWebRequest Request = HttpWebRequest.CreateHttp(Uri);
                    Request.CookieContainer = Cookies;
                    Request.Timeout = ReqTimeout;
                    HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                    if (Response.StatusCode != HttpStatusCode.OK)
                    {
                        lock (Log) { Log.Add("Ошибка: " + Response.StatusDescription); }
                        return false;
                    }
                    Stream dataStream = Response.GetResponseStream();
                    using (StreamReader Reader = new StreamReader(dataStream))
                    {
                        Result = Reader.ReadToEnd();
                        Response.Close();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Log.Add("Ошибка: " + e.Message);
                    return false;
                }
            }

            protected bool TryRequestPost(string Uri, string PostData, out string Result)
            {
                Result = String.Empty;
                try
                {
                    HttpWebRequest Request = HttpWebRequest.CreateHttp(Uri);
                    Request.CookieContainer = Cookies;
                    Request.Timeout = ReqTimeout;
                    Request.Method = "POST";
                    Request.ContentType = "application/x-www-form-urlencoded";

                    byte[] sentData = Encoding.UTF8.GetBytes(PostData);
                    Request.ContentLength = sentData.Length;
                    using (Stream sendStream = Request.GetRequestStream())
                    {
                        sendStream.Write(sentData, 0, sentData.Length);
                    }
                    HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                    if (Response.StatusCode != HttpStatusCode.OK)
                    {
                        lock (Log) { Log.Add("Ошибка: " + Response.StatusDescription); }
                        return false;
                    }
                    using (Stream ReceiveStream = Response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8))
                        {
                            //Кодировка указывается в зависимости от кодировки ответа сервера
                            Char[] read = new Char[256];
                            int count = sr.Read(read, 0, 256);
                            while (count > 0)
                            {
                                String str = new String(read, 0, count);
                                Result += str;
                                count = sr.Read(read, 0, 256);
                            }
                        }
                    }
                    Response.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Log.Add("Ошибка: " + e.Message);
                    return false;
                }
            }

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

            public override bool Login(string Username, string Password)
            {
                lock (Log) { Log.Add("Cоединение с сервером"); }
                try
                {
                    Cookies = new CookieContainer();
                    string Html = String.Empty;
                    if (!TryRequestGet(MainPage.AbsoluteUri + "index.php?action=login", out Html)) return false;
                    lock (Log) { Log.Add("Авторизация"); Progress++; }
                    Html = Html.ToLower();
                    string CurrSessionID = GetBetweenStrAfterStr(Html, "hashloginpassword", "'", "'");
                    string PostData =
                    "user=" + Uri.EscapeDataString(Username.ToLower()) +
                    "&cookielength=-1" +
                    "&passwrd=" + Uri.EscapeDataString(Password);
                    string HashPswd = hashLoginPassword(Username, Password, CurrSessionID);
                    if (HashPswd.Length > 0) PostData += "&hash_passwrd=" + HashPswd;
                    string AnotherID = GetBetweenStrAfterStr(Html, "value=\"" + CurrSessionID + "\"", "\"", "\"");
                    if (AnotherID.Length > 0) PostData += "&" + AnotherID + "=" + CurrSessionID;
                    if (!TryRequestPost(MainPage.AbsoluteUri + "index.php?action=login2", PostData, out Html)) return false;
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


        private void button1_Click(object sender, EventArgs e)
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
            ForumSMF f = new ForumSMF();
            f.ReqTimeout = 3000;
            f.MainPage = new Uri("http://www.simplemachines.org/community/");

            f.Login("Voron", "LEVEL2index");
            textBox1.Lines = f.Log.ToArray();
            textBox2.Text = f.h;
            RenderHtml(f.h);
            // this.Text = hashLoginPassword(textBox1.Lines[0], textBox1.Lines[1], textBox1.Lines[2]);

        }
    }
}
