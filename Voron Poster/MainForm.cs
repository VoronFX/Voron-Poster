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
using ScintillaNET;

namespace Voron_Poster
{

    public partial class MainForm : Form
    {

        public List<TaskGui> Tasks = new List<TaskGui>();
        public TaskGui CurrTask;
        public Scintilla CodeEditor = new Scintilla();

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

            CodeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            CodeEditor.LineWrapping.VisualFlags = ScintillaNET.LineWrappingVisualFlags.End;
            CodeEditor.Location = new System.Drawing.Point(0, 0);
            CodeEditor.Margins.Margin1.AutoToggleMarkerNumber = 0;
            CodeEditor.Margins.Margin1.IsClickable = true;
            CodeEditor.Margins.Margin2.Width = 16;
            CodeEditor.Name = "_scintilla";
            CodeEditor.TabIndex = 0;
            CodeEditor.ConfigurationManager.Language = "cs";
            CodeEditor.Indentation.SmartIndentType = SmartIndent.CPP;
            CodeEditor.Size = CodeBox.Size;
            CodeEditor.Location = CodeBox.Location;
            CodeEditor.Anchor = CodeBox.Anchor;
            CodeBox.Dispose();
            CodeTab.Controls.Add(CodeEditor);
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
        bool DetectMainPage;
        bool RecheckBlock;
        bool PropertiesActivity;
        bool PropertiesLoginActivity;
        Task PropertiesActivityTask;
        Forum TempForum;
        Forum.TaskBaseProperties TempProperties = new Forum.TaskBaseProperties();
        CancellationTokenSource StopProperties;

        private string GetDomain(string Url)
        {
            string Domain = new String(Url.Replace("http://", String.Empty)
                .Replace("https://", String.Empty).TakeWhile(c => c != '/').ToArray());
            if (Domain.IndexOf('.') > 0 && Domain.IndexOf('.') < Domain.Length-1)
                return Domain;
            return String.Empty;
        }

        private void GlobalAccountCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            TempProperties.UseLocalAccount = LocalAccountCheckbox.Checked;
            ResetIcon(sender, e);
            ValidateProperties();
        }

        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            PasswordBox.UseSystemPasswordChar = !ShowPassword.Checked;
        }

        private void UrlBox_TextChanged(object sender, EventArgs e)
        {
            string Text = (sender as TextBox).Text;
            int CaretPos = (sender as TextBox).SelectionStart;
            if (!RecheckBlock)
            {
                RecheckBlock = true;
                Text = new String(Text.Skip(Math.Max(Text.LastIndexOf("https://"), Text.LastIndexOf("http://"))).ToArray());
                if (CaretPos > 0 && CaretPos < 9 && CaretPos < Text.Length &&
                    !(CaretPos == 5 && Text.ToLower()[4] == 's')) Text = Text.Remove(CaretPos - 1, 1);
                int i = 0;
                int https = 0;
                while (i < Text.Length && i < 4 && Text.ToLower()[i] == "http"[i]) i++;
                if (i == 4 && Text.Length > 4 && Text.ToLower()[4] == 's') { i = 5; https = 1; }
                if (i == 4 + https) while (i < Text.Length && i < 7 + https && Text.ToLower()[i] == "://"[i - 4 - https]) i++;
                // Text = Text.Remove(0, i).Replace("http://",String.Empty).Replace("https://", String.Empty);
                if (https == 1)
                    Text = "https://" + Text.Remove(0, i);
                else Text = "http://" + Text.Remove(0, i);
                (sender as TextBox).Text = Text;
                CaretPos += 7 + https;
                RecheckBlock = false;
            }
            //if (!(sender as TextBox).Text.StartsWith("http://".Substring(0,Math.Min(, StringComparison.OrdinalIgnoreCase)
            //    && !(sender as TextBox).Text.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            //{
            //    CaretPos += 7;
            //    (sender as TextBox).Text = "http://" + (sender as TextBox).Text;
            //}
            Uri Url;
            if (Uri.TryCreate((sender as TextBox).Text, UriKind.Absolute, out Url)
                && (Url.Scheme == Uri.UriSchemeHttp || Url.Scheme == Uri.UriSchemeHttps))
            {
                (sender as TextBox).ForeColor = Color.Black;
                if (sender == TargetUrlBox)
                {
                    TempTargetUrl = (sender as TextBox).Text;
                    if (DetectMainPage)
                    {
                        string Domain = GetDomain(TargetUrlBox.Text).ToLower();
                        if (MainPageBox.Text.IndexOf("https://") >= 0)
                        MainPageBox.Text = "https://" + Domain;
                        else MainPageBox.Text = "http://" + Domain;
                            if (Domain != String.Empty) //suggest profile
                                foreach (Object s in ProfileComboBox.Items)
                                {
                                    if ((s as String).ToLower().IndexOf(Domain) >= 0)
                                    {
                                        ProfileComboBox.Text = (s as String);
                                        break;
                                    }
                                }
                    }
                }
                else
                    TempProperties.ForumMainPage = (sender as TextBox).Text;
            }
            else (sender as TextBox).ForeColor = Color.Red;
            (sender as TextBox).SelectionStart = CaretPos;
            if (sender == MainPageBox && MainPageBox.Focused) DetectMainPage = false;
            if (MainPageBox.Text == "http://" || MainPageBox.Text == "https://") DetectMainPage = true;
            ResetIcon(sender, e);
            ValidateProperties();
        }

        private void UsernameBox_TextChanged(object sender, EventArgs e)
        {
            TempProperties.Username = UsernameBox.Text;
            ResetIcon(sender, e);
        }

        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            TempProperties.Password = PasswordBox.Text;
            ResetIcon(sender, e);
        }

        private void ForumEngineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ForumEngineComboBox.SelectedIndex >= 0)
                TempProperties.Engine = (Forum.ForumEngine)Enum.Parse(typeof(Forum.ForumEngine),
            (string)ForumEngineComboBox.SelectedItem);
            ValidateProperties();
            ResetIcon(sender, e);
        }

        //private void ForumEngineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    TryLoginButton.Enabled = MainPageBox.ForeColor == Color.Black;
        //}

        private async void DetectEngineButton_Click(object sender, EventArgs e)
        {
            PropertiesActivity = true;
            ValidateProperties();
            DetectEngineButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Activity);
            Forum.ForumEngine Detected = Forum.ForumEngine.Unknown;
            HttpClient Client = new HttpClient();
            StopProperties = new CancellationTokenSource();
            Client.Timeout = new TimeSpan(0, 0, 10);
            bool Error = false;
            try
            {
                Task<Forum.ForumEngine> DetectTask = Forum.DetectForumEngine(TempProperties.ForumMainPage, Client, StopProperties.Token);
                PropertiesActivityTask = DetectTask;
                Detected = await DetectTask;
            }
            catch
            {
                Error = true;
                DetectEngineButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Error);
            }
            finally
            {
                Client.Dispose();
            }
            if (!Error)
            {
                if (Detected == Forum.ForumEngine.Unknown)
                    DetectEngineButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Question);
                else
                {
                    TempProperties.Engine = Detected;
                    ForumEngineComboBox.SelectedIndex =
                        ForumEngineComboBox.Items.IndexOf(Enum.GetName(typeof(Forum.ForumEngine), Detected));
                    DetectEngineButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Complete);
                }
            }
            else DetectEngineButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Error);
            PropertiesActivity = false;
            ValidateProperties();
        }

        private async void TryLoginButton_Click(object sender, EventArgs e)
        {
            PropertiesActivity = true;
            PropertiesLoginActivity = true;
            ValidateProperties();
            TryLoginButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Activity);
            TempForum = Forum.New(TempProperties.Engine);
            StopProperties = new CancellationTokenSource();
            TempForum.Properties = TempProperties;
            TempForum.Cancel = StopProperties;
            TempForum.RequestTimeout = new TimeSpan(0, 0, 10);
            try
            {
                Task<bool> LoginTask = TempForum.Login();
                PropertiesActivityTask = LoginTask;
                if (await LoginTask)
                    TryLoginButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Complete);
                else
                    TryLoginButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Error);
            }
            catch { TryLoginButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Error); }
            TempForum = null;
            PropertiesActivity = false;
            PropertiesLoginActivity = false;
            ValidateProperties();
        }

        private void ResetIcon(object sender, EventArgs e)
        {
            if (!PropertiesActivity)
            {
                TryLoginButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Login);
                if (sender == MainPageBox || sender == ForumEngineComboBox)
                    DetectEngineButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Gear);
            }
        }

        private void ValidateProperties()
        {
            //if (PropertiesActivity == null)
            //{
            //    if (File.Exists(GetProfilePath(ProfileComboBox.Text)))
            //    {
            //        DeleteProfileButton.Enabled = false;
            //        LoadProfileButton.Enabled = false;
            //    }
            //    //   MainPageBox.Enabled
            //    //       ForumEngineComboBox.Enabled
            //}
            GlobalAccountCheckbox.Enabled = !PropertiesLoginActivity;
            LocalAccountCheckbox.Enabled = !PropertiesLoginActivity;
            UsernameBox.Enabled = LocalAccountCheckbox.Checked &&
                LocalAccountCheckbox.Enabled && !PropertiesLoginActivity;
            PasswordBox.Enabled = UsernameBox.Enabled;
            ShowPassword.Enabled = UsernameBox.Enabled;

            DeleteProfileButton.Enabled =
                File.Exists(GetProfilePath(ProfileComboBox.Text)) &&
                !PropertiesActivity;
            LoadProfileButton.Enabled = DeleteProfileButton.Enabled;

            MainPageBox.Enabled = !PropertiesActivity;
            ForumEngineComboBox.Enabled = !PropertiesActivity;
            DetectEngineButton.Enabled =
                MainPageBox.ForeColor == Color.Black &&
                !PropertiesActivity;
            TryLoginButton.Enabled =
                MainPageBox.ForeColor == Color.Black &&
                ForumEngineComboBox.SelectedIndex >= 0 &&
                !PropertiesActivity;

            TaskPropApply.Enabled =
                TargetUrlBox.ForeColor == Color.Black &&
                MainPageBox.ForeColor == Color.Black &&
                ForumEngineComboBox.SelectedIndex >= 0 &&
                !PropertiesActivity;
            SaveProfileButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Save);
            //TaskPropCancel.Enabled = DetectEngineButton.Enabled &&
            //    TryLoginButton.Enabled;
        }

        private void TaskPropApply_Click(object sender, EventArgs e)
        {
            TaskPropApply.Enabled = false;
            TempForum = Forum.New(TempProperties.Engine);
            TempForum.Properties = TempProperties;
            CurrTask.Forum = TempForum;
            CurrTask.TargetUrl = TempTargetUrl;
            TempForum = null;
            CurrTask.New = false;
            ClosePropertiesPage(sender, e);
            TaskPropApply.Enabled = true;
        }

        public void ShowPropertiesPage()
        {
            //
            Tabs.TabPages.Add(TaskPropertiesPage);
            //Tabs.SelectedIndex = Tabs.TabPages.IndexOf(TaskPropertiesPage);
            Tabs.SelectTab(TaskPropertiesPage);
            for (int i = 0; i < Tabs.TabPages.Count; i++)
                Tabs.TabPages[i].Enabled = Tabs.TabPages[i] == TaskPropertiesPage;
            //  TaskPropertiesPage.Enabled = true;
            // this.ResumeLayout();
            ProfileComboBox_Enter(ProfileComboBox, EventArgs.Empty);
            PropertiesActivity = false;
            PropertiesLoginActivity = false;
            if (CurrTask.Forum != null)
                TempProperties = new Forum.TaskBaseProperties(CurrTask.Forum.Properties);
            else
                TempProperties = new Forum.TaskBaseProperties();
            LoadTaskBaseProperties(TempProperties);   
            TargetUrlBox.Text = CurrTask.TargetUrl;
            TargetUrlBox.Focus();
            //this.AcceptButton = TaskPropApply;
            //this.CancelButton = TaskPropCancel;
        }

        private void LoadTaskBaseProperties(Forum.TaskBaseProperties Properties)
        {
            if (TempProperties.UseLocalAccount)
                LocalAccountCheckbox.Select();
            else GlobalAccountCheckbox.Select();
            MainPageBox.Text = TempProperties.ForumMainPage;
            DetectMainPage = TempProperties.ForumMainPage == null
                || TempProperties.ForumMainPage == "https://"
                || TempProperties.ForumMainPage == "http://"
                || TempProperties.ForumMainPage == String.Empty;
            ForumEngineComboBox.SelectedIndex = ForumEngineComboBox.Items.IndexOf(
                Enum.GetName(typeof(Forum.ForumEngine), TempProperties.Engine));
            UsernameBox.Text = TempProperties.Username;
            PasswordBox.Text = TempProperties.Password;
        }

        private async void ClosePropertiesPage(object sender, EventArgs e)
        {
            TaskPropCancel.Enabled = false;
            TaskPropertiesPage.Enabled = false;

            if (PropertiesActivityTask != null && StopProperties != null)
            {
                StopProperties.Cancel();
                try
                {
                    await PropertiesActivityTask;
                }
                catch (Exception)
                {
                }
            }

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

        #region PropertiesProfiles

        bool ProfilesLocked;

        private string GetProfilePath(string Path)
        {
            if (!Path.EndsWith(".xml")) Path += ".xml";
            if (Path.IndexOf('\\') < 0) Path = "Profiles\\" + Path;
            return Path;
        }

        private void ProfileComboBox_TextChanged(object sender, EventArgs e)
        {
            ValidateProperties();
        }

        private void ProfileComboBox_Enter(object sender, EventArgs e)
        {
            if (!ProfilesLocked)
            {
                ProfilesLocked = true;
                lock (ProfileComboBox.Items)
                {
                    ProfileComboBox.Items.Clear();
                    Directory.CreateDirectory(".\\Profiles\\");
                    string[] Paths = Directory.GetFiles(".\\Profiles\\", "*.xml");
                    //trying complicated constructions =D
                    Array.ForEach<string>(Paths, s => ProfileComboBox.Items.Add(
                        s.Replace(".\\Profiles\\", String.Empty).Replace(".xml", String.Empty)));
                }
                ProfilesLocked = false;
            }
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
                if (ProfileComboBox.Text == String.Empty) NewProfileButton_Click(sender, e);
                var Xml = new System.Xml.Serialization.XmlSerializer(typeof(Forum.TaskBaseProperties));
                using (FileStream F = File.Create(GetProfilePath(ProfileComboBox.Text)))
                    Xml.Serialize(F, TempProperties);
                ValidateProperties();
                SaveProfileButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Complete);
            }
            catch (Exception Error)
            {
                SaveProfileButton.Image = TaskGui.GetIcon(TaskGui.InfoIcons.Error);
                MessageBox.Show(Error.Message);
            }
            finally
            {
                SaveProfileButton.Enabled = true;
            }
        }

        private void LoadProfileButton_Click(object sender, EventArgs e)
        {
            SaveProfileButton.Enabled = false;
            LoadProfileButton.Enabled = false;
            try
            {
                var Xml = new System.Xml.Serialization.XmlSerializer(typeof(Forum.TaskBaseProperties));
                using (FileStream F = File.OpenRead(GetProfilePath(ProfileComboBox.Text)))
                    TempProperties = (Forum.TaskBaseProperties)Xml.Deserialize(F);
                LoadTaskBaseProperties(TempProperties);
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message);
            }
            finally
            {
                LoadProfileButton.Enabled = true;
                SaveProfileButton.Enabled = true;
            }
        }

        private void NewProfileButton_Click(object sender, EventArgs e)
        {
            string NameBase = GetDomain(MainPageBox.Text);
            if (NameBase == String.Empty) NameBase = GetDomain(TargetUrlBox.Text);
            if (NameBase == String.Empty) NameBase = "Профиль";
            int i = 2;
            if (File.Exists(GetProfilePath(NameBase)))
            {
                while (File.Exists(GetProfilePath(NameBase + "(" + i.ToString() + ")"))) i++;
                NameBase += "(" + i.ToString() + ")";
            }
            ProfileComboBox.Text = NameBase;
        }

        #endregion

        #endregion

        #region TasksPage

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            TaskGui New = new TaskGui(this);
            New.TargetUrl = NewUrlTextBox.Text;
            New.Properties(sender, e);
            ForumEngineComboBox.SelectedIndex = -1;
            Tasks.Add(New);


            //TaskPropertiesPage.Enabled = false;
            // Tabs.TabIndex = Tabs.TabPages.IndexOf(TaskPropertiesPage);
            // Tabs.SelectTab(TaskPropertiesPage);
            // TaskPropertiesPage.Select();
            //  Tabs.TabPages.Remove(TaskPropertiesPage);
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
            TaskGui.InfoIcons Action = TaskGui.GetInfo((Bitmap)((sender as Button).Image));
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

        #endregion

        private void textBox4_Enter(object sender, EventArgs e)
        {
          //  if ()
        }

    }
}
