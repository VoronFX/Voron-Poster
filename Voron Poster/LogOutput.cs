using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Net;

namespace Voron_Poster
{
    public partial class LogOutput : Form
    {
        [Serializable]
        struct LogData
        {
            public string[] LogUrl;
            public string[] LogHtml;
            public string[] LogResponse;
            public string Variables;
            public string Comments;
            public string Email;
        }
        private LogData Log;

        public List<KeyValuePair<object, string>> HttpLog;
        public Scintilla HtmlBox = new Scintilla();
        public Scintilla HttpBox = new Scintilla();
        public Scintilla VariablesBox = new Scintilla();

        public LogOutput(List<KeyValuePair<object, string>> NewHttpLog, string NewVariables)
        {
            InitializeComponent();
            Browser.Navigate("about:blank");
            HttpLog = NewHttpLog;

            Log.LogUrl = new string[HttpLog.Count];
            Log.LogHtml = new string[HttpLog.Count];
            Log.LogResponse = new string[HttpLog.Count];
            Log.Variables = NewVariables;
            for (int i = 0; i < HttpLog.Count; i++)
            {
                Log.LogUrl[i] = GetUrl(HttpLog[i]);
                Log.LogHtml[i] = GetHtml(HttpLog[i]);
                Log.LogResponse[i] = FormatHttpResponse(HttpLog[i]);
            }

            HtmlBox.Dock = System.Windows.Forms.DockStyle.Fill;
            HtmlBox.LineWrapping.VisualFlags = ScintillaNET.LineWrappingVisualFlags.End;
            HtmlBox.Location = new System.Drawing.Point(0, 0);
            HtmlBox.Margins.Margin1.Type = MarginType.Number;
            HtmlBox.Margins.Margin1.Width = 27;
            HtmlBox.Margins.Margin2.Width = 16;
            HtmlBox.Name = "HtmlBox";
            HtmlBox.TabIndex = 0;
            HtmlBox.ConfigurationManager.Language = "html";
            HtmlBox.Indentation.SmartIndentType = SmartIndent.Simple;
            HtmlTab.Controls.Add(HtmlBox);

            HttpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            HttpBox.LineWrapping.VisualFlags = ScintillaNET.LineWrappingVisualFlags.End;
            HttpBox.Location = new System.Drawing.Point(0, 0);
            HttpBox.Margins.Margin1.Type = MarginType.Number;
            HttpBox.Margins.Margin1.Width = 27;
            HttpBox.Margins.Margin2.Width = 16;
            HttpBox.Name = "ResponseBox";
            HttpBox.TabIndex = 0;
            HttpBox.ConfigurationManager.Language = "js";
            HttpBox.Indentation.SmartIndentType = SmartIndent.Simple;
            ResponseTab.Controls.Add(HttpBox);

            VariablesBox.Dock = System.Windows.Forms.DockStyle.Fill;
            VariablesBox.LineWrapping.VisualFlags = ScintillaNET.LineWrappingVisualFlags.End;
            VariablesBox.Location = new System.Drawing.Point(0, 0);
            VariablesBox.Margins.Margin1.Type = MarginType.Number;
            VariablesBox.Margins.Margin1.Width = 27;
            VariablesBox.Margins.Margin2.Width = 16;
            VariablesBox.Name = "VariablesBox";
            VariablesBox.TabIndex = 0;
            VariablesBox.ConfigurationManager.Language = "xml";
            VariablesBox.Indentation.SmartIndentType = SmartIndent.Simple;

            VariablesTab.Controls.Add(VariablesBox);
            LoadLog();
        }

        private void LoadLog()
        {
            ResponseList.Items.Clear();

            for (int i = 0; i < Log.LogUrl.Length; i++)
            {
                ResponseList.Items.Add(Log.LogUrl[i]);
            }
            if (ResponseList.Items.Count > 0)
                ResponseList.SelectedIndex = ResponseList.Items.Count - 1;

            VariablesBox.IsReadOnly = false;
            VariablesBox.Text = Log.Variables;
            VariablesBox.IsReadOnly = true;
            reportComments.Text = Log.Comments;
            reportEmail.Text = Log.Email;
        }

        private string GetUrl(KeyValuePair<object, string> LogItem)
        {
            if (LogItem.Key is HttpResponseMessage)
                return (LogItem.Key as HttpResponseMessage).RequestMessage.RequestUri.AbsoluteUri;
            else return LogItem.Value;
        }

        private string GetHtml(KeyValuePair<object, string> LogItem)
        {
            if (LogItem.Key is HttpResponseMessage)
            {
                Task<string> T = (LogItem.Key as HttpResponseMessage).Content.ReadAsStringAsync();
                T.Wait();
                return T.Result;
            }
            return LogItem.Key as string;
        }

        private string FormatHttpResponse(KeyValuePair<object, string> LogItem)
        {
            string Text = String.Empty;
            if (LogItem.Key is HttpResponseMessage)
            {
                var Response = (LogItem.Key as HttpResponseMessage);
                Text = Response.RequestMessage.ToString() + "\n\n";
                Text += "Request POST content: \n{\n" + LogItem.Value + "\n}\n\n";
                Text += Response.ToString();
            }
            else
            {
                Text = "RequestUrl: " + LogItem.Value + "\n\n";
            }
            return Text;
        }

        private void ResponseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ResponseList.SelectedIndex >= 0)
            {
                try
                {
                    Browser.Document.Write(Log.LogHtml[ResponseList.SelectedIndex]);
                    Browser.Refresh();

                    HttpBox.IsReadOnly = false;
                    HttpBox.Text = Log.LogResponse[ResponseList.SelectedIndex];
                    HttpBox.IsReadOnly = true;

                    HtmlBox.IsReadOnly = false;
                    HtmlBox.Text = Log.LogHtml[ResponseList.SelectedIndex];
                    HtmlBox.IsReadOnly = true;
                }
                catch (Exception Error)
                {
                    MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private byte[] MakeReport()
        {
            var Dumper = new BinaryFormatter();
            LogData LogCopy = Log;
            LogCopy.Email = reportEmail.Text;
            LogCopy.Comments = reportComments.Text;
            // Delete authorization info
            if (!reportAccountInclude.Checked)
            {
                List<string> Lines = Regex.Split(LogCopy.Variables, "\r\n|\r|\n").ToList<string>();
                int beg = -1, end = -1;
                for (int i = Lines.Count - 1; i >= 0; i--)
                {
                    if (Lines[i].IndexOf("Username: ", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        Lines[i].IndexOf("Password: ", StringComparison.OrdinalIgnoreCase) >= 0) Lines.RemoveAt(i);
                    else if (Lines[i].IndexOf("<Account>", StringComparison.OrdinalIgnoreCase) >= 0) beg = i;
                    else if (Lines[i].IndexOf("</Account>", StringComparison.OrdinalIgnoreCase) >= 0) end = i;
                }
                if (beg >= 0 && end >= 0)
                {
                    Lines.RemoveRange(beg, end + 1 - beg);
                }
                LogCopy.Variables = String.Join("\n", Lines);
                for (int i = 0; i < LogCopy.LogResponse.Length; i++)
                {
                    beg = LogCopy.LogResponse[i].IndexOf("Request POST content", StringComparison.OrdinalIgnoreCase);
                    if (beg != -1)
                    {
                        beg = LogCopy.LogResponse[i].IndexOf("{", beg, StringComparison.OrdinalIgnoreCase);
                        end = beg + 1;
                        if (beg >= 0 && end < LogCopy.LogResponse[i].Length)
                        {
                            int Braket = 1;
                            while (Braket > 0 && end < LogCopy.LogResponse[i].Length)
                            {
                                if (LogCopy.LogResponse[i][end] == '{') Braket++;
                                else if (LogCopy.LogResponse[i][end] == '}') Braket--;
                                end++;
                            }
                            LogCopy.LogResponse[i] = LogCopy.LogResponse[i].Substring(0, beg)
                                + LogCopy.LogResponse[i].Substring(end, LogCopy.LogResponse[i].Length - end);
                        }
                    }
                }
            }
            using (var Stream = new System.IO.MemoryStream())
            {
                Dumper.Serialize(Stream, LogCopy);
                return Stream.ToArray();
            }
        }

        private void reportSend_Click(object sender, EventArgs e)
        {
            reportSend.Enabled = false;
            try
            {
                using (var WebClient = new WebClient())
                {
                    //   var HttpClient = new HttpClient();
                    //   var Form = new MultipartFormDataContent();

                    //  Form.Add(new StreamContent(Stream), "filMyFile");

                    //  var x = await HttpClient.PostAsync(new Uri("http://voron-exe.pp.ua/Projects/Voron%20Poster/Report.aspx", true), new StreamContent(Stream));

                    WebClient.UploadDataAsync(new Uri("http://voron-exe.pp.ua/Projects/Voron%20Poster/Report.aspx",
                        true), "POST", MakeReport());

                }
                MessageBox.Show("Отчет отправлен", this.Text, MessageBoxButtons.OK);
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                reportSend.Enabled = true;
            }
        }

        private void reportIKnowAboutPrivateInfo_CheckedChanged(object sender, EventArgs e)
        {
            reportSend.Enabled = reportIKnowAboutPrivateInfo.Checked;
        }

        private void propProfileLoad_Click(object sender, EventArgs e)
        {
            try
            {
                var OpenFileDialog = new OpenFileDialog();
                OpenFileDialog.DefaultExt = ".dump";
                OpenFileDialog.Filter = "Отчет (*.dump)|*.dump|Все файлы (*.*)|*.*";
                if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var F = new System.IO.FileStream(OpenFileDialog.FileName, System.IO.FileMode.Open))
                    {
                        var Dumper = new BinaryFormatter();
                        Log = (LogData)Dumper.Deserialize(F);
                        LoadLog();
                    }
                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void propProfileSave_Click(object sender, EventArgs e)
        {
            try
            {
                var SaveFileDialog = new SaveFileDialog();
                SaveFileDialog.DefaultExt = ".dump";
                SaveFileDialog.Filter = "Отчет (*.dump)|*.dump|Все файлы (*.*)|*.*";
                if (SaveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var F = new System.IO.FileStream(SaveFileDialog.FileName, System.IO.FileMode.Create))
                    {
                        byte[] Buffer = MakeReport();
                        F.Write(Buffer, 0, Buffer.Length);
                    }
                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
