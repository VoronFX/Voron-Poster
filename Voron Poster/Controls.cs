using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voron_Poster
{
        public struct ScriptsControls
        {
            string OpenedScript;
            // global::Voron_Poster.MainForm MainForm;
            // public ScriptsControls(global::Voron_Poster.MainForm ParentForm) : this() { MainForm = ParentForm; }
            public ScintillaNET.Scintilla CodeEditor;
            public System.Windows.Forms.TextBox CodeBox;
            public System.Windows.Forms.TextBox NameBox;
            public System.Windows.Forms.Button AcceptButton;
            public System.Windows.Forms.ListBox ListBox;
            public System.Windows.Forms.Button DeleteButton;
            public System.Windows.Forms.Button NewButton;
            public System.Windows.Forms.Button SaveButton;

            public System.Windows.Forms.Button RunButton;

            public System.Windows.Forms.Splitter ListSplitter;
            public System.Windows.Forms.Panel ListPanel;
            public System.Windows.Forms.Splitter TestSplitter;
            public System.Windows.Forms.Panel TestPanel1;
            public System.Windows.Forms.Panel TestPanel2;

            public System.Windows.Forms.TabPage Tab;
            public System.Windows.Forms.TabControl Tabs;
            public System.Windows.Forms.TabPage CodeTab;
            //public CodeControls Test;
            //public partial struct CodeControls
            //{ 

            //}

            //public TestControls Test;
            //public partial struct TestControls
            //{
            public System.Windows.Forms.TextBox ResultBox;
            public System.Windows.Forms.TextBox SubjectBox;
            public System.Windows.Forms.TabPage TestTab;
            public System.Windows.Forms.Label StatusLabel;
            public System.Windows.Forms.TextBox MessageBox;
            void TestBox_Enter(object sender, EventArgs e);
            //}
        }
}
