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

namespace Voron_Poster
{
    public partial class LogOutput : Form
    {
        public List<KeyValuePair<HttpResponseMessage, string>> HttpLog;
        public Scintilla HtmlBox = new Scintilla();
        public Scintilla HttpBox = new Scintilla();
        public Scintilla VariablesBox = new Scintilla();
        public LogOutput(List<KeyValuePair<HttpResponseMessage, string>> NewHttpLog)
        {
            InitializeComponent();
            Browser.Navigate("about:blank");
            HttpLog = NewHttpLog;
            for (int i = 0; i < HttpLog.Count; i++)
            {
                ResponseList.Items.Add(HttpLog[i].Key.RequestMessage.RequestUri);
            }
            ResponseList.SelectedIndex = ResponseList.Items.Count - 1;
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

        }

        private void HtmlOutput_Load(object sender, EventArgs e)
        {

        }

        private async void ResponseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ResponseList.SelectedIndex >= 0)
            {
                try
                {
                    string Html = await HttpLog[ResponseList.SelectedIndex].Key.Content.ReadAsStringAsync();
                    Browser.Document.Write(Html);
                    Browser.Refresh();

                    HttpBox.IsReadOnly = false;
                    HttpBox.Text = HttpLog[ResponseList.SelectedIndex].Key.RequestMessage.ToString()+"\n\n";
                    HttpBox.Text += "Request POST content: \n{\n"+HttpLog[ResponseList.SelectedIndex].Value + "\n}\n\n";
                    HttpBox.Text += HttpLog[ResponseList.SelectedIndex].Key.ToString();
     
                    HttpBox.IsReadOnly = true;

                    HtmlBox.IsReadOnly = false;
                    HtmlBox.Text = Html;
                    HtmlBox.IsReadOnly = true;
                }
                catch (Exception Error) { 
                    MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
    }
}
