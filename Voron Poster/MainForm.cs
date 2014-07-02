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
using System.Web;
using System.Xml;
using Roslyn.Scripting.CSharp;

namespace Voron_Poster
{
    public partial class MainForm : Form
    {
        List<TaskGui> Tasks = new List<TaskGui>();

        public MainForm()
        {
            InitializeComponent();
            var pos = this.PointToScreen(label1.Location);
            pos = progressBar1.PointToClient(pos);
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
            progressBar1.Maximum = 15;


            //f = new ForumSMF();
            //f.RequestTimeout = 3000;
            //f.MainPage = new Uri("http://www.simplemachines.org/community/");
            //System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            //t.Interval = 100;
            //t.Tick += new EventHandler((a, b) => { progressBar1.Value = f.Progress; });
            //t.Start();
            //await f.Login("Voron", "LEVEL2index");
            //textBox1.Lines = f.Log.ToArray();
            //textBox2.Text = f.h;
            //  RenderHtml(f.h);
            // this.Text = hashLoginPassword(textBox1.Lines[0], textBox1.Lines[1], textBox1.Lines[2]);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.Cancel.Cancel();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            //await f.PostMessage(new Uri("http://www.simplemachines.org/community/index.php?topic=524612.0"), "test3", textBox2.Text);
            //textBox1.Lines = f.Log.ToArray();
            //textBox2.Text += f.h;
            //RenderHtml(f.h);
        }



        #region Tasks Page

        private void TasksGuiTable_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Row == 0)
            {
                Graphics g = e.Graphics;
                Rectangle r = e.CellBounds;
                g.FillRectangle(SystemBrushes.Control, r);
            }
        }

        #endregion

        #region Task Properties Page

        Forum.TaskBaseProperties CurrentProperties = new Forum.TaskBaseProperties();



        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            PasswordBox.UseSystemPasswordChar = !ShowPassword.Checked;
        }

        private void TargetURlBox_TextChanged(object sender, EventArgs e)
        {
            Uri TargetUrl;
            if (Uri.TryCreate(TargetURlBox.Text, UriKind.Absolute, out TargetUrl)
                && TargetUrl.Scheme == Uri.UriSchemeHttp)
            {
                if (MainPageBox.Enabled && MainPageBox.Text == String.Empty)
                {
                    MainPageBox.Text = TargetUrl.AbsolutePath;
                }
                TargetURlBox.ForeColor = Color.Black;
                if (MainPageBox.Enabled & MainPageBox.ForeColor == Color.Black)
                    TaskPropApply.Enabled = true; ;
            }
            else
            {
                TaskPropApply.Enabled = false;
                TargetURlBox.ForeColor = Color.Red;
            }
        }

        private void MainPageBox_TextChanged(object sender, EventArgs e)
        {
            Uri MainPageUrl;
            if (Uri.TryCreate(MainPageBox.Text, UriKind.Absolute, out MainPageUrl)
                && MainPageUrl.Scheme == Uri.UriSchemeHttp)
            {
                CurrentProperties.ForumMainPage = MainPageBox.Text;
                MainPageBox.ForeColor = Color.Black;
                DetectEngineButton.Enabled = true;
                if (TargetURlBox.ForeColor == Color.Black)
                    TaskPropApply.Enabled = true; ;
            }
            else
            {
                DetectEngineButton.Enabled = false;
                TaskPropApply.Enabled = false;
                MainPageBox.ForeColor = Color.Red;
            }
        }

        private string GetProfilePath(string Path)
        {
            if (!Path.EndsWith(".xml")) Path += ".xml";
            if (Path.IndexOf('\\') < 0) Path = "Profiles\\" + Path;
            return Path;
        }

        private void ProfileComboBox_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(GetProfilePath(ProfileComboBox.Text)))
            {
                DeleteProfileButton.Enabled = true;
                LoadProfileButton.Enabled = true;
            }
            else
            {
                DeleteProfileButton.Enabled = false;
                LoadProfileButton.Enabled = false;
            }
        }

        private void ProfileComboBox_Enter(object sender, EventArgs e)
        {
            ProfileComboBox.Items.Clear();
            Directory.CreateDirectory(".\\Profiles\\");
            string[] Paths = Directory.GetFiles(".\\Profiles\\", "*.xml");
            //trying complicated constructions =D
            Array.ForEach<string>(Paths, s => ProfileComboBox.Items.Add(
                s.Replace(".\\Profiles\\", String.Empty).Replace(".xml", String.Empty)));
        }

        private void DeleteProfileButton_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(GetProfilePath(ProfileComboBox.Text));
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message);
            }
            finally
            {
                DeleteProfileButton.Enabled = false;
                LoadProfileButton.Enabled = false;
            }
        }

        private void SaveProfileButton_Click(object sender, EventArgs e)
        {
            SaveProfileButton.Enabled = false;
            try
            {
                System.Xml.Serialization.XmlSerializer Xml =
                    new System.Xml.Serialization.XmlSerializer(typeof(Forum.TaskBaseProperties));
                using (FileStream F = File.Create(GetProfilePath(ProfileComboBox.Text)))
                    Xml.Serialize(F, CurrentProperties);
                LoadProfileButton.Enabled = true;
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message);
            }
            finally
            {
                SaveProfileButton.Enabled = true;
            }
        }

        #endregion

        private void NewProfileButton_Click(object sender, EventArgs e)
        {
            int i = 1;
            while (File.Exists(GetProfilePath("Шаблон#" + i.ToString()))) i++;
            ProfileComboBox.Text = "Шаблон#" + i.ToString();
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            TaskPropertiesPage.Enabled = false;
            // Tabs.TabIndex = Tabs.TabPages.IndexOf(TaskPropertiesPage);
            // Tabs.SelectTab(TaskPropertiesPage);
            // TaskPropertiesPage.Select();
            //  Tabs.TabPages.Remove(TaskPropertiesPage);
        }

        private void Tabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // TaskPropApply.
            e.Cancel = !e.TabPage.Enabled;
            TasksUpdater.Enabled = e.TabPage == TasksPage;
        }

        private void TasksUpdater_Tick(object sender, EventArgs e)
        {
            bool IconRunning = false, IconCritical = false, IconCancelled = false, IconComplete = false;
            bool IconStart = false, IconRestart = false, IconStop = false;
            bool Selected = true;
            for (int i = 0; i < Tasks.Count; i++)
            {
                Tasks[i].SetStatusIcon();
                if (Tasks[i].Ctrls.Selected.Checked)
                {
                    if (!IconRunning) IconRunning = Tasks[i].Ctrls.StatusIcon.Image ==
                        global::Voron_Poster.Properties.Resources.StatusAnnotations_Play_16xLG
                        || Tasks[i].Ctrls.StatusIcon.Image ==
                        global::Voron_Poster.Properties.Resources.StatusAnnotations_Pause_16xLG;
                    if (!IconCritical) IconCritical = Tasks[i].Ctrls.StatusIcon.Image ==
                        global::Voron_Poster.Properties.Resources.StatusAnnotations_Critical_16xLG_color;
                    if (!IconCancelled) IconCancelled = Tasks[i].Ctrls.StatusIcon.Image ==
                        global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG_color;
                    if (!IconComplete) IconComplete = Tasks[i].Ctrls.StatusIcon.Image ==
                        global::Voron_Poster.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
                    if (Tasks[i].Ctrls.StartStop.Enabled)
                    {
                        if (!IconStart) IconStart = Tasks[i].Ctrls.StartStop.Image ==
                            global::Voron_Poster.Properties.Resources.arrow_run_16xLG;
                        if (!IconRestart) IconRestart = Tasks[i].Ctrls.StartStop.Image ==
                            global::Voron_Poster.Properties.Resources.Restart_6322;
                        if (!IconStop) IconStop = Tasks[i].Ctrls.StartStop.Image ==
                            global::Voron_Poster.Properties.Resources.Symbols_Stop_16xLG;
                    }
                }
                else Selected = false;
            }
            if (IconRunning)
                GTStatusIcon.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Play_16xLG;
            else if (IconCritical)
                GTStatusIcon.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Critical_16xLG_color;
            else if (IconCancelled)
                GTStatusIcon.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG_color;
            else if (IconComplete)
                GTStatusIcon.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
            else GTStatusIcon.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG;
            GTSelected.Checked = Selected;
            GTStart.Enabled = IconRestart || IconStart;
            if (IconRestart) GTStart.Image = global::Voron_Poster.Properties.Resources.Restart_6322;
            else GTStart.Image = global::Voron_Poster.Properties.Resources.arrow_run_16xLG;
            GTStop.Enabled = IconRestart || IconStop;
            if (IconRestart) GTStop.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG;
            else GTStop.Image = global::Voron_Poster.Properties.Resources.Symbols_Stop_16xLG;
        }

        private void ToolTip_Popup(object sender, PopupEventArgs e)
        {
        }

    }
}
