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
using System.Reflection;

namespace Voron_Poster
{

    public partial class MainForm : Form
    {

        public List<TaskGui> Tasks = new List<TaskGui>();
        public TaskGui CurrTask;

        public MainForm()
        {
            InitializeComponent();

            for (int i = 0; i < Tabs.TabPages.Count; i++)
            {
                typeof(TabPage).InvokeMember("DoubleBuffered",
        BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
        null, Tabs.TabPages[i], new object[] { true });

            }
            Tabs.TabPages.Remove(TaskPropertiesPage);
            ForumEngineComboBox.Items.AddRange(Enum.GetNames(typeof(Forum.ForumEngine)));
            ForumEngineComboBox.Items.RemoveAt(0);
            //        typeof(TabControl).InvokeMember("DoubleBuffered",
            //BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            //null, Tabs, new object[] { true });
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

        string TempTargetUrl;
        Forum TempForum;
        Forum.TaskBaseProperties TempProperties = new Forum.TaskBaseProperties();



        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            PasswordBox.UseSystemPasswordChar = !ShowPassword.Checked;
        }

        private void TargetURlBox_TextChanged(object sender, EventArgs e)
        {
            Uri TargetUrl;
            if (Uri.TryCreate(TargetUrlBox.Text, UriKind.Absolute, out TargetUrl)
                && TargetUrl.Scheme == Uri.UriSchemeHttp)
            {
                //TempTargetUrl = TargetUrlBox.Text;
                //if (MainPageBox.Enabled && MainPageBox.Text == String.Empty)
                //{
                //    MainPageBox.Text = TargetUrl.AbsolutePath;
                //}
                TargetUrlBox.ForeColor = Color.Black;
                if (MainPageBox.Enabled & MainPageBox.ForeColor == Color.Black)
                    TaskPropApply.Enabled = true;
            }
            else
            {
                TaskPropApply.Enabled = false;
                TargetUrlBox.ForeColor = Color.Red;
            }
        }

        private void MainPageBox_TextChanged(object sender, EventArgs e)
        {
            Uri MainPageUrl;
            if (Uri.TryCreate(MainPageBox.Text, UriKind.Absolute, out MainPageUrl)
                && MainPageUrl.Scheme == Uri.UriSchemeHttp)
            {
                // TempProperties.ForumMainPage = MainPageBox.Text;
                MainPageBox.ForeColor = Color.Black;
                DetectEngineButton.Enabled = true;
                if (ForumEngineComboBox.SelectedIndex >= 0)
                    TryLoginButton.Enabled = true;
                if (TargetUrlBox.ForeColor == Color.Black)
                    TaskPropApply.Enabled = true;
            }
            else
            {
                DetectEngineButton.Enabled = false;
                TaskPropApply.Enabled = false;
                TryLoginButton.Enabled = false;
                MainPageBox.ForeColor = Color.Red;
            }
        }

        #region PropertiesProfiles

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
                    Xml.Serialize(F, TempProperties);
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

        private void NewProfileButton_Click(object sender, EventArgs e)
        {
            int i = 1;
            while (File.Exists(GetProfilePath("Шаблон#" + i.ToString()))) i++;
            ProfileComboBox.Text = "Шаблон#" + i.ToString();
        }

        #endregion

        #endregion



        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            Tasks.Add(new TaskGui(this));


            //TaskPropertiesPage.Enabled = false;
            // Tabs.TabIndex = Tabs.TabPages.IndexOf(TaskPropertiesPage);
            // Tabs.SelectTab(TaskPropertiesPage);
            // TaskPropertiesPage.Select();
            //  Tabs.TabPages.Remove(TaskPropertiesPage);
        }

        public void ShowPropertiesPage()
        {
            //
            Tabs.TabPages.Add(TaskPropertiesPage);
            //Tabs.SelectedIndex = Tabs.TabPages.IndexOf(TaskPropertiesPage);
            Tabs.SelectTab(TaskPropertiesPage);
            for (int i = 0; i < Tabs.TabPages.Count; i++)
                if (Tabs.TabPages[i] != TaskPropertiesPage)
                    Tabs.TabPages[i].Enabled = false;
            //  TaskPropertiesPage.Enabled = true;
            // this.ResumeLayout();
        }

        private void ClosePropertiesPage(object sender, EventArgs e)
        {
            TaskPropCancel.Enabled = false;
            TasksPage.Enabled = true;
            Tabs.SelectTab(TasksPage);
            for (int i = 0; i < Tabs.TabPages.Count; i++) Tabs.TabPages[i].Enabled = true;
            Tabs.TabPages.Remove(TaskPropertiesPage);
            if (CurrTask.New)
            {
                CurrTask.Delete(sender, e);
            }
            TaskPropCancel.Enabled = true;
        }

        private void TaskPropApply_Click(object sender, EventArgs e)
        {
            TaskPropApply.Enabled = false;
            CurrTask.New = false;
            // CurrTask.Forum.Properties = CurrTa;
            ClosePropertiesPage(sender, e);
            TaskPropApply.Enabled = true;
        }

        private void Tabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //if (Tabs.SelectedIndex == Tabs.TabPages.IndexOf(TaskPropertiesPage))
            e.Cancel = !e.TabPage.Enabled;
            TasksUpdater.Enabled = e.TabPage == TasksPage;
        }

        private void TasksUpdater_Tick(object sender, EventArgs e)
        {
            bool[] SelInfo = new bool[Enum.GetNames(typeof(TaskGui.InfoIcons)).Length];
            bool Checked = false, Unchecked = false;
            lock (Tasks)
            {
                for (int i = 0; i < Tasks.Count; i++)
                {
                    Tasks[i].SetStatusIcon();
                    if (Tasks[i].Ctrls.Selected.Checked)
                    {
                        SelInfo[(int)Tasks[i].Status] = true;
                        SelInfo[(int)Tasks[i].Action] = true;
                        Checked = true;
                    }
                    else Unchecked = true;
                }
            }
            // Set global status icon
            if (SelInfo[(int)TaskGui.InfoIcons.Running] || SelInfo[(int)TaskGui.InfoIcons.Waiting])
                GTStatusIcon.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Running);
            else if (SelInfo[(int)TaskGui.InfoIcons.Error])
                GTStatusIcon.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Error);
            else if (SelInfo[(int)TaskGui.InfoIcons.Cancelled])
                GTStatusIcon.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Cancelled);
            else if (SelInfo[(int)TaskGui.InfoIcons.Complete])
                GTStatusIcon.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Complete);
            else GTStatusIcon.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Stopped);

            if (Checked && Unchecked) GTSelected.CheckState = CheckState.Indeterminate;
            else if (Checked) GTSelected.CheckState = CheckState.Checked;
            else GTSelected.CheckState = CheckState.Unchecked;

            // Set global start icon 
            TaskGui.InfoIcons GActionStart, GActionStop;
            GTStart.Enabled = SelInfo[(int)TaskGui.InfoIcons.Restart] || SelInfo[(int)TaskGui.InfoIcons.Run];
            GTDelete.Enabled = GTStart.Enabled;
            if (SelInfo[(int)TaskGui.InfoIcons.Restart]) GActionStart = TaskGui.InfoIcons.Restart;
            else GActionStart = TaskGui.InfoIcons.Run;
            // Set global stop icon 
            GTStop.Enabled = SelInfo[(int)TaskGui.InfoIcons.Restart] || SelInfo[(int)TaskGui.InfoIcons.Cancel];
            if (SelInfo[(int)TaskGui.InfoIcons.Restart]) GActionStop = TaskGui.InfoIcons.Clear;
            else GActionStop = TaskGui.InfoIcons.Cancel;
            GTStart.Image = TaskGui.GetIcon(GActionStart);
            GTStop.Image = TaskGui.GetIcon(GActionStop);
            ToolTip.SetToolTip(GTStart, TaskGui.GetTooltip(GActionStart));
            ToolTip.SetToolTip(GTStop, TaskGui.GetTooltip(GActionStop));
        }

        private void GTSelected_Click(object sender, EventArgs e)
        {
            if (GTSelected.CheckState == CheckState.Indeterminate)
                GTSelected.CheckState = CheckState.Unchecked;
            for (int i = 0; i < Tasks.Count; i++)
                Tasks[i].Ctrls.Selected.Checked = GTSelected.Checked;

        }

        private void GTStartStop_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;
            TasksUpdater.Enabled = false;
            TaskGui.InfoIcons Action = TaskGui.GetInfo(new Bitmap((sender as Button).Image));
            for (int i = 0; i < Tasks.Count; i++)
            {
                if (Tasks[i].Ctrls.Selected.Checked && Tasks[i].Action == Action)
                    Tasks[i].Ctrls.StartStop.PerformClick();
            }
            TasksUpdater.Enabled = true;
        }

        private void GTDelete_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;
            List<TaskGui> Remove = new List<TaskGui>();
            lock (Tasks)
            {
                for (int i = 0; i < Tasks.Count; i++)
                {
                    if (Tasks[i].Ctrls.Selected.Checked && Tasks[i].Ctrls.Delete.Enabled)
                    {
                        Remove.Add(Tasks[i]);
                    }
                }
                foreach (TaskGui Task in Remove)
                {
                    Task.Ctrls.Delete.PerformClick();
                }
            }
            Remove.Clear();
        }

        private void ForumEngineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TempForum = Forum.New((Forum.ForumEngine)Enum.Parse(typeof(Forum.ForumEngine),
                (string)ForumEngineComboBox.SelectedItem));
            TryLoginButton.Enabled = MainPageBox.ForeColor == Color.Black;
        }

        private async void DetectEngineButton_Click(object sender, EventArgs e)
        {
            DetectEngineButton.Enabled = false;
            TryLoginButton.Enabled = false;
            ForumEngineComboBox.Enabled = false;
            MainPageBox.Enabled = false;
            TaskPropApply.Enabled = false;
            LoadProfileButton.Enabled = false;
            DetectEngineButton.Image
            Forum.ForumEngine Detected = Forum.ForumEngine.Unknown;
            HttpClient Client = new HttpClient();
            try
            {
                TempForum.DetectForumEngine(await
                    (await Client.GetAsync(TempProperties.ForumMainPage)).Content.ReadAsStringAsync());
            }
            catch
            {

            }
            finally
            {
                Client.Dispose();
            }



        }

        private void ValidateProperties()
        {
            TaskPropApply.Enabled =
                TargetUrlBox.ForeColor == Color.Black &&
                MainPageBox.ForeColor == Color.Black &&
                ForumEngineComboBox.SelectedIndex >= 0 &&
                DetectEngineButton.Enabled &&
                TryLoginButton.Enabled;

            //TaskPropCancel.Enabled = DetectEngineButton.Enabled &&
            //    TryLoginButton.Enabled;

        }

    }
}
