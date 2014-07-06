using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voron_Poster
{
    public partial class StructuredControlsForm
    {
        public partial struct ScriptsControls
        {

            public void TestBox_Enter(object sender, EventArgs e)
            {
                if ((sender as TextBox).ForeColor == SystemColors.GrayText)
                {
                    (sender as TextBox).Text = String.Empty;
                    (sender as TextBox).ForeColor = SystemColors.WindowText;
                    (sender as TextBox).Font = new Font((sender as TextBox).Font, FontStyle.Regular);
                }
            }

            public void SubjectBox_Leave(object sender, EventArgs e)
            {
                if (SubjectBox.Text == String.Empty)
                {
                    SubjectBox.Text = "Тема сообщения";
                    SubjectBox.ForeColor = SystemColors.GrayText;
                    SubjectBox.Font = new Font(SubjectBox.Font, FontStyle.Italic);
                }
            }

            public void MessageBox_Leave(object sender, EventArgs e)
            {
                if (MessageBox.Text == String.Empty)
                {
                    MessageBox.Text = "[b]Тестовое сообщение[b]\nСегодня [color=red]" +
                        "хорошая[/color] погода.\nМы пойдем [color=#12830a]купаться[/color] на речку.";
                    MessageBox.ForeColor = SystemColors.GrayText;
                    MessageBox.Font = new Font(MessageBox.Font, FontStyle.Italic);
                }
            }

            public void ScriptsPage_Enter(object sender, EventArgs e)
            {
                ListBox.Items.Clear();
                NameBox.AutoCompleteCustomSource.Clear();
                Directory.CreateDirectory(".\\Scripts\\");
                string[] Paths = Directory.GetFiles(".\\Scripts\\", "*.cs");
                for (int i = 0; i < Paths.Length; i++)
                {
                    Paths[i] = Paths[i].Replace(".\\Scripts\\", String.Empty).Replace(".cs", String.Empty);
                }
                ListBox.Items.AddRange(Paths);
                NameBox.AutoCompleteCustomSource.AddRange(Paths);
                ListBox.SelectedIndex = ListBox.Items.Count - 1;
            }

            public void SelectScriptInList()
            {
                //if (!NameBox.Text.Equals((string)ListBox.SelectedItem,
                //        StringComparison.OrdinalIgnoreCase) ^ SaveButton.Enabled)
                //{
                int i;
                //if () i = int.MaxValue;
                //else
                if (SaveButton.Enabled) ListBox.SelectedIndexChanged -= ListBox_SelectedIndexChanged;
                for (i = 0; i < ListBox.Items.Count; i++)
                {
                    if (NameBox.Text.Equals((string)ListBox.Items[i],
                        StringComparison.OrdinalIgnoreCase))
                    {
                        ListBox.SelectedItem = ListBox.Items[i];
                        break;
                    }
                }
                if (i >= ListBox.Items.Count)
                {
                    ListBox.SelectedIndexChanged -= ListBox_SelectedIndexChanged;
                    ListBox.SelectedIndex = -1;
                    if (CodeEditor.Text != String.Empty)
                    {
                        OpenedScript = (string)ListBox.SelectedItem;
                        if (NameBox.Text != String.Empty)
                            SaveButton.Enabled = true;
                    }
                }
                ListBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
                //}

            }

            public void NameBox_TextChanged(object sender, EventArgs e)
            {
                Console.WriteLine(NameBox.Text);
                if (!NameBox.Text.Equals((string)ListBox.SelectedItem,
                       StringComparison.OrdinalIgnoreCase) && CodeEditor.Text != String.Empty && NameBox.Text != String.Empty)
                    SaveButton.Enabled = true;
                SelectScriptInList();
            }

            public bool AskSave()
            {
                switch (System.Windows.Forms.MessageBox.Show("Сохранить изменения?", "Изменения", MessageBoxButtons.YesNoCancel))
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        SaveButton_Click(SaveButton, EventArgs.Empty);
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                        if (OpenedScript == null)
                        {
                            ListBox.SelectedIndexChanged -= ListBox_SelectedIndexChanged;
                            ListBox.SelectedIndex = -1;
                            ListBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
                        }
                        else
                        {
                            NameBox.Text = OpenedScript;
                            SelectScriptInList();
                        }
                        return false;
                }
                return true;
            }

            public void ListBox_SelectedIndexChanged(object sender, EventArgs e)
            {
                Console.WriteLine(ListBox.SelectedIndex);
                if (OpenedScript != (string)ListBox.SelectedItem)
                {
                    if (SaveButton.Enabled && !AskSave()) return;
                    CodeEditor.Text = String.Empty;
                    try
                    {
                        CodeEditor.Text = System.IO.File.ReadAllText(GetScriptPath((string)ListBox.SelectedItem));
                        OpenedScript = (string)ListBox.SelectedItem;
                    }
                    catch (Exception Error)
                    {
                        System.Windows.Forms.MessageBox.Show(Error.Message);
                    }
                }
                DeleteButton.Enabled = ListBox.SelectedIndex != -1;
                NameBox.TextChanged -= NameBox_TextChanged;
                NameBox.Text = (string)ListBox.SelectedItem;
                NameBox.TextChanged += NameBox_TextChanged;
                CodeEditor.TextChanged -= CodeEditor_TextChanged;
                SaveButton.Enabled = false;
                CodeEditor.TextChanged += CodeEditor_TextChanged;
            }

            public void CodeEditor_TextChanged(object sender, EventArgs e)
            {
                SaveButton.Enabled = true;
            }
            public void SaveButton_Click(object sender, EventArgs e)
            {
                SaveButton.Enabled = false;
                try
                {
                    if (NameBox.Text == String.Empty) NewScriptButton_Click(sender, e);
                    System.IO.File.WriteAllText(GetScriptPath(NameBox.Text), CodeEditor.Text);
                    OpenedScript = NameBox.Text;
                    ScriptsPage_Enter(sender, e);
                }
                catch (Exception Error)
                {
                    System.Windows.Forms.MessageBox.Show(Error.Message);
                    SaveButton.Enabled = true;
                }
            }

            public string GetScriptPath(string Path)
            {
                if (!Path.EndsWith(".cs")) Path += ".cs";
                if (Path.IndexOf('\\') < 0) Path = "Scripts\\" + Path;
                return Path;
            }

            public void NewScriptButton_Click(object sender, EventArgs e)
            {
                if (SaveButton.Enabled && !AskSave()) return;
                CodeEditor.Text = String.Empty;
                int i = 1;
                while (File.Exists(GetScriptPath("Script #" + i.ToString()))) i++;
                NameBox.Text = "Script #" + i.ToString();
            }

            public void DeleteButton_Click(object sender, EventArgs e)
            {
                DeleteButton.Enabled = false;
                if (System.Windows.Forms.MessageBox.Show("Удалить скрипт " + OpenedScript + "?",
                    "Удалить скрипт?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    try
                    {
                        File.Delete(GetScriptPath(OpenedScript));
                        ScriptsPage_Enter(sender, e);
                        ListBox.SelectedIndex = ListBox.Items.Count - 1;
                    }
                    catch (Exception Error)
                    {
                        DeleteButton.Enabled = true;
                        System.Windows.Forms.MessageBox.Show(Error.Message);
                    }
            }
        }
    }
}
