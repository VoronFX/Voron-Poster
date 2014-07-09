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
    public partial class HtmlOutput : Form
    {
        public List<HttpResponseMessage> ResponseLog;
        public Scintilla HtmlBox = new Scintilla();
        public Scintilla ResponseBox = new Scintilla();
        public Scintilla VariablesBox = new Scintilla();
        public HtmlOutput(List<HttpResponseMessage> NewResponseLog)
        {
            InitializeComponent();
            Browser.Navigate("about:blank");
            ResponseLog = NewResponseLog;
            for (int i = 0; i < ResponseLog.Count; i++)
            {
                ResponseList.Items.Add(ResponseLog[i].RequestMessage.RequestUri);
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

            ResponseBox.Dock = System.Windows.Forms.DockStyle.Fill;
            ResponseBox.LineWrapping.VisualFlags = ScintillaNET.LineWrappingVisualFlags.End;
            ResponseBox.Location = new System.Drawing.Point(0, 0);
            ResponseBox.Margins.Margin1.Type = MarginType.Number;
            ResponseBox.Margins.Margin1.Width = 27;
            ResponseBox.Margins.Margin2.Width = 16;
            ResponseBox.Name = "ResponseBox";
            ResponseBox.TabIndex = 0;
            ResponseBox.ConfigurationManager.Language = "js";
            ResponseBox.Indentation.SmartIndentType = SmartIndent.Simple;
            ResponseTab.Controls.Add(ResponseBox);

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
                    string Html = await ResponseLog[ResponseList.SelectedIndex].Content.ReadAsStringAsync();
                    Browser.Document.Write(Html);
                    Browser.Refresh();

                    ResponseBox.IsReadOnly = false;
                    ResponseBox.Text = ResponseLog[ResponseList.SelectedIndex].RequestMessage.ToString()+"\n\n";
                    ResponseBox.Text += ResponseLog[ResponseList.SelectedIndex].ToString();
     
                    ResponseBox.IsReadOnly = true;

                    HtmlBox.IsReadOnly = false;
                    HtmlBox.Text = Html;
                    HtmlBox.IsReadOnly = true;
                }
                catch (Exception Error) { MessageBox.Show(Error.Message); }
            }
        }
    }
}
