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
using Roslyn.Scripting;

using System.Reflection;
using ScintillaNET;
using Microsoft.Win32;

namespace Voron_Poster
{
    public partial class MainForm : Form
    {
        public List<TaskGui> Tasks = new List<TaskGui>();
        public TaskGui CurrTask;
        public Scintilla scriptsEditor = new Scintilla();

        public MainForm()
        {
            InitializeComponent();
            Forum.CaptchaForm.Owner = this;
            for (int i = 0; i < Tabs.TabPages.Count; i++)
            {
                typeof(TabPage).InvokeMember("DoubleBuffered",
        BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
        null, Tabs.TabPages[i], new object[] { true });

            }
            Tabs.TabPages.Remove(propTab);
            Tabs.TabPages.Remove(scriptsTab);
            propEngine.Items.AddRange(Enum.GetNames(typeof(Forum.Engine)));
            propEngine.Items.RemoveAt(0);

            GTStatusIcon.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Stopped);

            // Use latest installed IE
            //try
            //{
            //    using (RegistryKey RegKey = Registry.CurrentUser.OpenSubKey(
            // @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION\", true))
            //        RegKey.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe",
            //            9000, RegistryValueKind.DWord);
            //}
            //catch { }

            //        typeof(TabControl).InvokeMember("DoubleBuffered",
            //BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            //null, Tabs, new object[] { true });
            scriptsEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            scriptsEditor.LineWrapping.VisualFlags = ScintillaNET.LineWrappingVisualFlags.End;
            scriptsEditor.Location = new System.Drawing.Point(0, 0);
            scriptsEditor.Margins.Margin1.Type = MarginType.Number;
            scriptsEditor.Margins.Margin1.Width = 27;
            scriptsEditor.Margins.Margin2.Width = 16;
            scriptsEditor.Name = "scriptsEditor";
            scriptsEditor.TabIndex = 0;
            scriptsEditor.ConfigurationManager.Language = "cs";
            scriptsEditor.Indentation.SmartIndentType = SmartIndent.CPP;
            scriptsEditor.Size = scriptsCodeBox.Size;
            scriptsEditor.Location = scriptsCodeBox.Location;
            scriptsEditor.Anchor = scriptsCodeBox.Anchor;
            scriptsEditor.TextChanged += scriptsEditor_TextChanged;
            scriptsCodeBox.Dispose();
            scriptsCodeTab.Controls.Add(scriptsEditor);
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

        #region Tasks Page

        private void TasksGuiTable_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.CellBounds;
            if (e.Row == 0)
                g.FillRectangle(SystemBrushes.Control, r);
            //else // Color table rows by status, but I dislike the result
            //{
            //    var Status = (PictureBox)tasksTable.GetControlFromPosition(3, e.Row);
            //    if (Status != null)
            //    {
            //        switch (TaskGui.GetInfo((Bitmap)Status.Image))
            //        {
            //            case TaskGui.InfoIcons.Running:
            //            case TaskGui.InfoIcons.Waiting:
            //                g.FillRectangle(Brushes.LightBlue, r); break;
            //            case TaskGui.InfoIcons.Complete:
            //                g.FillRectangle(Brushes.LightGreen, r); break;
            //            case TaskGui.InfoIcons.Error:
            //                g.FillRectangle(Brushes.LightPink, r); break;
            //            default:
            //                g.FillRectangle(Brushes.White, r); break;
            //        }
            //    }
            //}
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            TaskGui New = new TaskGui(this);
            New.TargetUrl = tasksUrl.Text;
            New.Properties(sender, e);
            propEngine.SelectedIndex = -1;
            Tasks.Add(New);


            //TaskPropertiesPage.Enabled = false;
            // Tabs.TabIndex = Tabs.TabPages.IndexOf(TaskPropertiesPage);
            // Tabs.SelectTab(TaskPropertiesPage);
            // TaskPropertiesPage.Select();
            //  Tabs.TabPages.Remove(TaskPropertiesPage);
        }

        TabPage PreviousTab = null;
        private void Tabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (PreviousTab == propTab && e.TabPage != scriptsTab)
            {
                DialogResult Ask = System.Windows.Forms.DialogResult.No;
                if (propApply.Enabled)
                    Ask = MessageBox.Show("Сохранить изменнения?",
                        this.Text, MessageBoxButtons.YesNoCancel);
                switch (Ask)
                {
                    case System.Windows.Forms.DialogResult.Yes: propApply.PerformClick(); break;
                    case System.Windows.Forms.DialogResult.No: propCancel.PerformClick(); break;
                }
            }
            if (e.TabPage.Enabled)
            {
                PreviousTab = e.TabPage;
                e.Cancel = false;
            }
            else
                e.Cancel = true;

            TasksUpdater.Enabled = e.TabPage == tasksTab;
        }

        private void TasksUpdater_Tick(object sender, EventArgs e)
        {
            bool[] SelInfo = new bool[Enum.GetNames(typeof(TaskGui.InfoIcons)).Length];
            bool Checked = false, Unchecked = false;
            int ProgressSum = 0;
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
                    ProgressSum += Tasks[i].Ctrls.Progress.Value;
                }
            }
            // Set global status icon
            if (SelInfo[(int)TaskGui.InfoIcons.Running] || SelInfo[(int)TaskGui.InfoIcons.Waiting])
                GTStatusIcon.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Running);
            else if (SelInfo[(int)TaskGui.InfoIcons.Error])
                GTStatusIcon.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Error);
            else if (SelInfo[(int)TaskGui.InfoIcons.Cancelled])
                GTStatusIcon.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Cancelled);
            else if (SelInfo[(int)TaskGui.InfoIcons.Complete])
                GTStatusIcon.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Complete);
            else GTStatusIcon.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Stopped);

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
            GTStop.Enabled = SelInfo[(int)TaskGui.InfoIcons.Restart] ||
                SelInfo[(int)TaskGui.InfoIcons.Cancel] || SelInfo[(int)TaskGui.InfoIcons.Cancelled];
            if (SelInfo[(int)TaskGui.InfoIcons.Cancel]) GActionStop = TaskGui.InfoIcons.Cancel;
            else GActionStop = TaskGui.InfoIcons.Clear;
            GTStart.Image = TaskGui.GetTaggedIcon(GActionStart);
            GTStop.Image = TaskGui.GetTaggedIcon(GActionStop);
            ToolTip.SetToolTip(GTStart, TaskGui.GetTooltip(GActionStart));
            ToolTip.SetToolTip(GTStop, TaskGui.GetTooltip(GActionStop));

            if (Tasks.Count == 0 || !Checked) GTProgress.Value = 0;
            else
                GTProgress.Value = ProgressSum / Tasks.Count;
            if ((SelInfo[(int)TaskGui.InfoIcons.Error] || SelInfo[(int)TaskGui.InfoIcons.Cancelled]) &&
               !(SelInfo[(int)TaskGui.InfoIcons.Running] || SelInfo[(int)TaskGui.InfoIcons.Waiting]))
                ModifyProgressBarColor.SetState(GTProgress, 2);
            else ModifyProgressBarColor.SetState(GTProgress, 1);
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
            GTStart.Enabled = false;
            GTStop.Enabled = false;
            GTDelete.Enabled = false;
            TasksUpdater.Enabled = false;
            TaskGui.InfoIcons Action = TaskGui.GetInfo((Bitmap)((sender as Button).Image));
            if (Action == TaskGui.InfoIcons.Clear)
                for (int i = 0; i < Tasks.Count; i++)
                {
                    if (Tasks[i].Ctrls.Selected.Checked &&
                         Tasks[i].Ctrls.StartStop.Enabled && (
                    Tasks[i].Status == TaskGui.InfoIcons.Cancelled ||
                    Tasks[i].Status == TaskGui.InfoIcons.Error))
                    {
                        Tasks[i].Forum.Reset();
                        Tasks[i].SetStatusIcon();
                    }
                }
            else
                for (int i = 0; i < Tasks.Count; i++)
                {
                    if (Tasks[i].Ctrls.Selected.Checked
                        && Tasks[i].Ctrls.StartStop.Enabled
                           && Tasks[i].Action == Action)
                        //  Tasks[i].Ctrls.StartStop.PerformClick();
                        Tasks[i].StartStop(sender, e);
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
                    Task.Delete(sender, e);
                }
            }
            Remove.Clear();
        }

        public struct TaskList
        {
            public Forum.TaskBaseProperties[] Properties;
            public string[] TargetUrls;
        }

        private void tasksSave_Click(object sender, EventArgs e)
        {
            tasksSave.Enabled = false;
            try
            {
                Directory.CreateDirectory(@".\TaskLists\");
                var SaveFileDialog = new SaveFileDialog();
                SaveFileDialog.DefaultExt = "xml";
                SaveFileDialog.Filter = "Список задач (*.xml)|*.xml|Все файлы (*.*)|*.*";
                SaveFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, @"TaskLists");
                if (SaveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    TaskList TaskList;
                    TaskList.Properties = new Forum.TaskBaseProperties[Tasks.Count];
                    TaskList.TargetUrls = new string[Tasks.Count];
                    for (int i = 0; i < Tasks.Count; i++)
                    {
                        TaskList.Properties[i] = Tasks[i].Forum.Properties;
                        TaskList.TargetUrls[i] = Tasks[i].TargetUrl;
                    }
                    var Xml = new System.Xml.Serialization.XmlSerializer(typeof(TaskList));
                    using (FileStream F = File.Create(SaveFileDialog.FileName))
                        Xml.Serialize(F, TaskList);
                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            tasksSave.Enabled = true;
        }

        private void tasksLoad_Click(object sender, EventArgs e)
        {
            tasksLoad.Enabled = false;
            try
            {
                Directory.CreateDirectory(@".\TaskLists\");
                var OpenFileDialog = new OpenFileDialog();
                OpenFileDialog.DefaultExt = ".xml";
                OpenFileDialog.Filter = "Список задач (*.xml)|*.xml|Все файлы (*.*)|*.*";
                OpenFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, @"TaskLists");
                if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    TaskList TaskList;
                    var Xml = new System.Xml.Serialization.XmlSerializer(typeof(TaskList));
                    using (FileStream F = File.OpenRead(OpenFileDialog.FileName))
                        TaskList = (TaskList)Xml.Deserialize(F);
                    for (int i = 0; i < TaskList.Properties.Length; i++)
                    {
                        TaskGui NewTask = new TaskGui(this);
                        NewTask.New = false;
                        NewTask.TargetUrl = TaskList.TargetUrls[i];
                        NewTask.Forum = Forum.New(TaskList.Properties[i].Engine);
                        NewTask.Forum.Properties = TaskList.Properties[i];
                        NewTask.Ctrls.Name.Text = NewTask.TargetUrl;
                        Tasks.Add(NewTask);
                    }
                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            tasksLoad.Enabled = true;
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

        private void propAuthGlobal_CheckedChanged(object sender, EventArgs e)
        {
            TempProperties.UseLocalAccount = propAuthLocal.Checked;
            ResetIcon(sender, e);
            ValidateProperties();
        }

        private void propAuthShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            propPassword.UseSystemPasswordChar = !propAuthShowPassword.Checked;
        }

        private void propUrl_TextChanged(object sender, EventArgs e)
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
            Uri Url;
            if (Uri.TryCreate((sender as TextBox).Text, UriKind.Absolute, out Url)
                && (Url.Scheme == Uri.UriSchemeHttp || Url.Scheme == Uri.UriSchemeHttps))
            {
                (sender as TextBox).ForeColor = Color.Black;
                if (sender == propTargetUrl)
                {
                    TempTargetUrl = (sender as TextBox).Text;
                    if (DetectMainPage)
                    {
                        string Domain = Forum.GetDomain(propTargetUrl.Text).ToLower();
                        if (propTargetUrl.Text.LastIndexOf("/") > 7)
                            propMainUrl.Text = new String(propTargetUrl.Text.Take(propTargetUrl.Text.LastIndexOf("/") + 1).ToArray());
                        else
                            propMainUrl.Text = propTargetUrl.Text + "/";
                        if (Domain != String.Empty) //suggest profile
                            foreach (Object s in propProfiles.Items)
                            {
                                if ((s as String).ToLower().IndexOf(Domain) >= 0)
                                {
                                    propProfiles.Text = (s as String);
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
            if (sender == propMainUrl && propMainUrl.Focused) DetectMainPage = false;
            if (propMainUrl.Text == "http://" || propMainUrl.Text == "https://") DetectMainPage = true;
            ResetIcon(sender, e);
            ValidateProperties();
        }

        private void propAuthUsername_TextChanged(object sender, EventArgs e)
        {
            TempProperties.Username = propUsername.Text;
            ResetIcon(sender, e);
        }

        private void propAuthPassword_TextChanged(object sender, EventArgs e)
        {
            TempProperties.Password = propPassword.Text;
            ResetIcon(sender, e);
        }

        private void propEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (propEngine.SelectedIndex >= 0)
                TempProperties.Engine = (Forum.Engine)Enum.Parse(typeof(Forum.Engine),
            (string)propEngine.SelectedItem);
            ValidateProperties();
            ResetIcon(sender, e);
        }

        //private void ForumEngineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    TryLoginButton.Enabled = MainPageBox.ForeColor == Color.Black;
        //}

        private async void propEngineDetect_Click(object sender, EventArgs e)
        {
            PropertiesActivity = true;
            ValidateProperties();
            propEngineDetect.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Activity);
            Forum.Engine Detected = Forum.Engine.Unknown;
            HttpClient Client = new HttpClient();
            StopProperties = new CancellationTokenSource();
            Client.Timeout = new TimeSpan(0, 0, 10);
            bool Error = false;
            try
            {
                Task<Forum.Engine> DetectTask = Forum.DetectEngine(TempProperties.ForumMainPage, Client, StopProperties.Token);
                PropertiesActivityTask = DetectTask;
                Detected = await DetectTask;
            }
            catch
            {
                Error = true;
                propEngineDetect.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Error);
            }
            finally
            {
                Client.Dispose();
            }
            if (!Error)
            {
                if (Detected == Forum.Engine.Unknown)
                    propEngineDetect.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Question);
                else
                {
                    TempProperties.Engine = Detected;
                    propEngine.SelectedIndex =
                        propEngine.Items.IndexOf(Enum.GetName(typeof(Forum.Engine), Detected));
                    propEngineDetect.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Complete);
                }
            }
            else propEngineDetect.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Error);
            PropertiesActivity = false;
            ValidateProperties();
        }

        private async void propAuthTryLogin_Click(object sender, EventArgs e)
        {
            PropertiesActivity = true;
            PropertiesLoginActivity = true;
            ValidateProperties();
            propAuthTryLogin.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Activity);
            TempForum = Forum.New(TempProperties.Engine);
            StopProperties = new CancellationTokenSource();
            TempForum.Properties = TempProperties;
            TempForum.Reset();
            TempForum.Cancel = StopProperties;
            TempForum.RequestTimeout = new TimeSpan(0, 0, 10);
            Task<Exception> LoginTask = TempForum.Login();
            PropertiesActivityTask = LoginTask;
            Exception Error;
            try
            {
                Error = await LoginTask;
            }
            catch (Exception CatchedError)
            {
                Error = CatchedError;
            }
            if (Error == null) propAuthTryLogin.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Complete);
            else
            {
                propAuthTryLogin.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Error);
                ToolTip.SetToolTip(propAuthTryLogin, "Ошибка: " + Error.Message);
            }
            PropertiesActivity = false;
            PropertiesLoginActivity = false;
            ValidateProperties();
        }

        private void propAuthLog_Click(object sender, EventArgs e)
        {
            if (TempForum != null)
            {
                TempForum.ShowData(TempForum.Properties.ForumMainPage);
            }
        }

        private void ResetIcon(object sender, EventArgs e)
        {
            if (!PropertiesActivity)
            {
                propAuthTryLogin.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Login);
                ToolTip.SetToolTip(propAuthTryLogin, String.Empty);
                if (sender == propMainUrl || sender == propEngine)
                    propEngineDetect.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Gear);
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
            propAuthGlobal.Enabled = !PropertiesLoginActivity;
            propAuthLocal.Enabled = !PropertiesLoginActivity;
            propUsername.Enabled = propAuthLocal.Checked &&
                propAuthLocal.Enabled && !PropertiesLoginActivity;
            propPassword.Enabled = propUsername.Enabled;
            propAuthShowPassword.Enabled = propUsername.Enabled;

            propProfileDelete.Enabled =
                File.Exists(GetProfilePath(propProfiles.Text)) &&
                !PropertiesActivity;
            propProfileLoad.Enabled = propProfileDelete.Enabled;

            propMainUrl.Enabled = !PropertiesActivity;
            propEngine.Enabled = !PropertiesActivity;
            propEngineDetect.Enabled =
                propMainUrl.ForeColor == Color.Black &&
                !PropertiesActivity;
            propAuthTryLogin.Enabled =
                propMainUrl.ForeColor == Color.Black &&
                propEngine.SelectedIndex >= 0 &&
                !PropertiesActivity;

            propScriptsEdit.Enabled =
                propScriptsList.SelectedIndex > -1;
            propScriptsRemove.Enabled =
                propScriptsList.SelectedIndex > -1;
            propScriptsUp.Enabled = propScriptsList.SelectedIndex > 0;
            propScriptsDown.Enabled = propScriptsList.SelectedIndex < propScriptsList.Items.Count - 1;

            propApply.Enabled =
                propTargetUrl.ForeColor == Color.Black &&
                propMainUrl.ForeColor == Color.Black &&
                propEngine.SelectedIndex >= 0 &&
                propScriptsList.Items.Count > 0 &&
                !PropertiesActivity;
            propProfileSave.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Save);
            //TaskPropCancel.Enabled = DetectEngineButton.Enabled &&
            //    TryLoginButton.Enabled;
        }

        private void propApply_Click(object sender, EventArgs e)
        {
            propApply.Enabled = false;
            TempProperties.PreProcessingScripts = propScriptsList.Items.Cast<string>().ToList<string>();
            TempForum = Forum.New(TempProperties.Engine);
            TempForum.Properties = TempProperties;
            CurrTask.Forum = TempForum;
            CurrTask.TargetUrl = TempTargetUrl;
            CurrTask.Ctrls.Name.Text = CurrTask.TargetUrl;
            TempForum = null;
            CurrTask.New = false;
            TasksUpdater_Tick(sender, e);
            ClosePropertiesPage(sender, e);
            propApply.Enabled = true;
        }

        public void ShowPropertiesPage()
        {
            //
            Tabs.TabPages.Add(propTab);
            //Tabs.SelectedIndex = Tabs.TabPages.IndexOf(TaskPropertiesPage);
            Tabs.SelectTab(propTab);
            for (int i = 0; i < Tabs.TabPages.Count; i++)
                Tabs.TabPages[i].Enabled = Tabs.TabPages[i] == propTab;
            //  TaskPropertiesPage.Enabled = true;
            // this.ResumeLayout();
            ProfileComboBox_Enter(propProfiles, EventArgs.Empty);
            PropertiesActivity = false;
            PropertiesLoginActivity = false;
            if (CurrTask.Forum != null)
                TempProperties = new Forum.TaskBaseProperties(CurrTask.Forum.Properties);
            else
                TempProperties = new Forum.TaskBaseProperties();
            LoadTaskBaseProperties(TempProperties);
            propTargetUrl.Text = CurrTask.TargetUrl;
            ValidateProperties();
            propTargetUrl.Focus();
            //this.AcceptButton = TaskPropApply;
            //this.CancelButton = TaskPropCancel;
        }

        private void LoadTaskBaseProperties(Forum.TaskBaseProperties Properties)
        {
            if (TempProperties.UseLocalAccount)
                propAuthLocal.Select();
            else propAuthGlobal.Select();
            propMainUrl.Text = TempProperties.ForumMainPage;
            DetectMainPage = TempProperties.ForumMainPage == null
                || TempProperties.ForumMainPage == "https://"
                || TempProperties.ForumMainPage == "http://"
                || TempProperties.ForumMainPage == String.Empty;
            propEngine.SelectedIndex = propEngine.Items.IndexOf(
                Enum.GetName(typeof(Forum.Engine), TempProperties.Engine));
            propUsername.Text = TempProperties.Username;
            propPassword.Text = TempProperties.Password;
            propScriptsList.Items.Clear();
            propScriptsList.Items.AddRange(TempProperties.PreProcessingScripts.ToArray());
        }

        private async void ClosePropertiesPage(object sender, EventArgs e)
        {
            propCancel.Enabled = false;
            propTab.Enabled = false;

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

            tasksTab.Enabled = true;
            PreviousTab = null;
            Tabs.SelectTab(tasksTab);
            for (int i = 0; i < Tabs.TabPages.Count; i++) Tabs.TabPages[i].Enabled = true;
            Tabs.TabPages.Remove(propTab);
            if (CurrTask.New)
            {
                CurrTask.Delete(sender, e);
            }
            TempForum = null;
            propCancel.Enabled = true;
        }

        private void propScriptsRemove_Click(object sender, EventArgs e)
        {
            propScriptsList.SelectedIndex -= 1;
            propScriptsList.Items.RemoveAt(propScriptsList.SelectedIndex + 1);
            ValidateProperties();
        }

        private void propScriptsUp_Click(object sender, EventArgs e)
        {
            propScriptsList.Items.Insert(propScriptsList.SelectedIndex - 1, propScriptsList.SelectedItem);
            propScriptsList.SelectedIndex = propScriptsList.SelectedIndex - 2;
            propScriptsList.Items.RemoveAt(propScriptsList.SelectedIndex + 2);
            ValidateProperties();
        }

        private void propScriptsDown_Click(object sender, EventArgs e)
        {
            propScriptsList.Items.Insert(propScriptsList.SelectedIndex + 2, propScriptsList.SelectedItem);
            propScriptsList.SelectedIndex = propScriptsList.SelectedIndex + 2;
            propScriptsList.Items.RemoveAt(propScriptsList.SelectedIndex - 2);
            ValidateProperties();
        }

        private void propScriptsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateProperties();
        }

        private void propScriptsEdit_Click(object sender, EventArgs e)
        {
            scriptsName.Text = (String)propScriptsList.SelectedItem;
            propScriptsRemove_Click(sender, e);
            propScriptsAdd_Click(sender, e);
            scriptsSave.Enabled = false;
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
                lock (propProfiles.Items)
                {
                    propProfiles.Items.Clear();
                    Directory.CreateDirectory(".\\Profiles\\");
                    string[] Paths = Directory.GetFiles(".\\Profiles\\", "*.xml");
                    //trying complicated constructions =D
                    Array.ForEach<string>(Paths, s => propProfiles.Items.Add(
                        s.Replace(".\\Profiles\\", String.Empty).Replace(".xml", String.Empty)));
                }
                ProfilesLocked = false;
            }
        }

        private void DeleteProfileButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить профиль " + propProfiles.Text + "?",
                "Удалить профиль?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                try
                {
                    File.Delete(GetProfilePath(propProfiles.Text));
                }
                catch (Exception Error)
                {
                    MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    propProfileDelete.Enabled = false;
                    propProfileLoad.Enabled = false;
                }
        }

        private void SaveProfileButton_Click(object sender, EventArgs e)
        {
            propProfileSave.Enabled = false;
            try
            {
                TempProperties.PreProcessingScripts = propScriptsList.Items.Cast<string>().ToList<string>();
                if (propProfiles.Text == String.Empty) NewProfileButton_Click(sender, e);
                var Xml = new System.Xml.Serialization.XmlSerializer(typeof(Forum.TaskBaseProperties));
                using (FileStream F = File.Create(GetProfilePath(propProfiles.Text)))
                    Xml.Serialize(F, TempProperties);
                ValidateProperties();
                propProfileSave.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Complete);
            }
            catch (Exception Error)
            {
                propProfileSave.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Error);
                MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                propProfileSave.Enabled = true;
            }
        }

        private void LoadProfileButton_Click(object sender, EventArgs e)
        {
            propProfileSave.Enabled = false;
            propProfileLoad.Enabled = false;
            try
            {
                var Xml = new System.Xml.Serialization.XmlSerializer(typeof(Forum.TaskBaseProperties));
                using (FileStream F = File.OpenRead(GetProfilePath(propProfiles.Text)))
                    TempProperties = (Forum.TaskBaseProperties)Xml.Deserialize(F);
                LoadTaskBaseProperties(TempProperties);
                ValidateProperties();
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                propProfileLoad.Enabled = true;
                propProfileSave.Enabled = true;
            }
        }

        private void NewProfileButton_Click(object sender, EventArgs e)
        {
            string NameBase = Forum.GetDomain(propMainUrl.Text);
            if (NameBase == String.Empty) NameBase = Forum.GetDomain(propTargetUrl.Text);
            if (NameBase == String.Empty) NameBase = "Профиль";
            int i = 2;
            if (File.Exists(GetProfilePath(NameBase)))
            {
                while (File.Exists(GetProfilePath(NameBase + "(" + i.ToString() + ")"))) i++;
                NameBase += "(" + i.ToString() + ")";
            }
            propProfiles.Text = NameBase;
        }

        #endregion

        #endregion

        #region Scripts

        string OpenedScript;
        bool IgnoreEvents;

        private void scriptsTab_Enter(object sender, EventArgs e)
        {
            IgnoreEvents = true;
            scriptsList.Items.Clear();
            scriptsName.AutoCompleteCustomSource.Clear();
            Directory.CreateDirectory(".\\Scripts\\");
            string[] Paths = Directory.GetFiles(".\\Scripts\\", "*.cs");
            //trying complicated constructions =D
            Array.ForEach<string>(Paths, s =>
            {
                s = s.Replace(".\\Scripts\\", String.Empty).Replace(".cs", String.Empty);
                scriptsList.Items.Add(s);
                scriptsName.AutoCompleteCustomSource.Add(s);
            }); ;
            SelectScriptInList();
            IgnoreEvents = false;
        }

        public static string GetScriptPath(string Path)
        {
            if (!Path.EndsWith(".cs")) Path += ".cs";
            if (Path.IndexOf('\\') < 0) Path = "Scripts\\" + Path;
            return Path;
        }

        private bool AskSave()
        {
            switch (MessageBox.Show("Сохранить изменения в файле \""
                + scriptsName.Text + "\"?", "Изменения", MessageBoxButtons.YesNoCancel))
            {
                case System.Windows.Forms.DialogResult.Yes:
                    scriptsSave_Click(scriptsSave, EventArgs.Empty);
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    IgnoreEvents = true;
                    if (OpenedScript == null)
                        scriptsList.SelectedIndex = -1;
                    else
                    {
                        scriptsName.Text = OpenedScript;
                        SelectScriptInList();
                    }
                    IgnoreEvents = false;
                    return false;
            }
            return true;
        }

        private void SelectScriptInList()
        {
            IgnoreEvents = true;
            Console.WriteLine("select begins");
            int i;
            for (i = 0; i < scriptsList.Items.Count; i++)
            {
                if (scriptsName.Text.Equals((string)scriptsList.Items[i],
                    StringComparison.OrdinalIgnoreCase))
                {
                    scriptsList.SelectedItem = scriptsList.Items[i];
                    break;
                }
            }
            if (i >= scriptsList.Items.Count)
            {
                scriptsList.SelectedIndex = -1;
                if (scriptsEditor.Text != String.Empty)
                {
                    OpenedScript = (string)scriptsList.SelectedItem;
                    if (scriptsName.Text != String.Empty)
                    {
                        scriptsSave.Enabled = true;
                        scriptsAccept.Enabled = true;
                    }
                }
                else scriptsAccept.Enabled = false;
            }
            else
                scriptsAccept.Enabled = true;
            IgnoreEvents = false;
            Console.WriteLine("select ends");
            //}
        }

        private void scriptsName_TextChanged(object sender, EventArgs e)
        {
            if (!IgnoreEvents)
            {
                Console.WriteLine("namechange begins");
                scriptsSave.Enabled = scriptsEditor.Text != String.Empty && scriptsName.Text != String.Empty;
                SelectScriptInList();
                Console.WriteLine("namechange ends");
            }
        }

        private void scriptsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IgnoreEvents)
            {
                Console.WriteLine("indexchange begins");
                IgnoreEvents = true;
                if (OpenedScript != (string)scriptsList.SelectedItem && scriptsList.SelectedIndex != -1)
                {
                    if (scriptsSave.Enabled && !AskSave()) return;
                    scriptsEditor.Text = String.Empty;
                    try
                    {
                        scriptsEditor.Text = System.IO.File.ReadAllText(GetScriptPath((string)scriptsList.SelectedItem));
                        OpenedScript = (string)scriptsList.SelectedItem;
                        scriptsRun.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Test);
                    }
                    catch (Exception Error)
                    {
                        MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    scriptsSave.Enabled = false;
                }
                scriptsDelete.Enabled = scriptsList.SelectedIndex != -1;
                scriptsAccept.Enabled = scriptsDelete.Enabled;
                scriptsName.Text = (string)scriptsList.SelectedItem;
                IgnoreEvents = false;
                Console.WriteLine("indexchange ends");
            }
        }

        private void scriptsEditor_TextChanged(object sender, EventArgs e)
        {
            if (!IgnoreEvents)
            {
                scriptsSave.Enabled = true;
                scriptsRun.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Test);
            }
        }

        private void scriptsSave_Click(object sender, EventArgs e)
        {
            scriptsSave.Enabled = false;
            IgnoreEvents = true;
            try
            {
                if (scriptsName.Text == String.Empty) scriptsNew_Click(sender, e);
                System.IO.File.WriteAllText(GetScriptPath(scriptsName.Text), scriptsEditor.Text);
                OpenedScript = scriptsName.Text;
                scriptsTab_Enter(sender, e);
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                scriptsSave.Enabled = true;
            }
            IgnoreEvents = false;
        }

        private void scriptsNew_Click(object sender, EventArgs e)
        {
            if (scriptsSave.Enabled && !AskSave()) return;
            scriptsEditor.Text = String.Empty;
            OpenedScript = null;
            int i = 1;
            while (File.Exists(GetScriptPath("Script #" + i.ToString()))) i++;
            scriptsName.Text = "Script #" + i.ToString();
        }

        private void scriptsDelete_Click(object sender, EventArgs e)
        {
            scriptsDelete.Enabled = false;
            if (MessageBox.Show(this, "Удалить скрипт " + OpenedScript + "?",
                "Удалить скрипт?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                try
                {
                    File.Delete(GetScriptPath(OpenedScript));
                    scriptsTab_Enter(sender, e);
                    scriptsSave.Enabled = false;
                    scriptsList.SelectedIndex = scriptsList.Items.Count - 1;

                }
                catch (Exception Error)
                {
                    scriptsDelete.Enabled = true;
                    MessageBox.Show(Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void scriptsCancel_Click(object sender, EventArgs e)
        {
            if (scriptsSave.Enabled && !AskSave()) return;
            propTab.Enabled = true;
            Tabs.TabPages.Remove(scriptsTab);
            Tabs.SelectedTab = propTab;
        }

        private void scriptsAccept_Click(object sender, EventArgs e)
        {
            scriptsCancel_Click(sender, e);
            propScriptsList.Items.Insert(propScriptsList.SelectedIndex + 1, scriptsName.Text);
            propScriptsList.SelectedIndex = propScriptsList.SelectedIndex + 1;
        }

        #region TestTab

        Thread TestThread;

        private async void scriptsRun_Click(object sender, EventArgs e)
        {
            scriptsRun.Enabled = false;
            scriptsListPanel.Enabled = false;
            bool SaveState = scriptsSave.Enabled, DeleteState = scriptsDelete.Enabled;
            scriptsSave.Enabled = false;
            scriptsDelete.Enabled = false;
            scriptsEditor.IsReadOnly = true;
            scriptsSubject.TextChanged -= scriptsSubject_TextChanged;
            scriptsMessage.TextChanged -= scriptsSubject_TextChanged;
            scriptsRun.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Activity);
            var Result = new List<string>();
            Exception Error = null;
            ToolTip.SetToolTip(scriptsStatusLabel, String.Empty);
            string Code = scriptsEditor.Text;
            scriptsEditor.Styles.LineNumber.IsVisible = true;
            TestThread = new Thread(() =>
            {
                try
                {
                    var ScriptData = new Forum.ScriptData(new Forum.ScriptData.PostMessage(scriptsSubject.Text, scriptsMessage.Text));
                    var Session = Forum.InitScriptEngine(ScriptData);
                    Session.Execute(Code);
                    for (int i = 0; i < ScriptData.Output.Count; i++)
                    {
                        Result.Add("Сообщение " + i.ToString());
                        Result.Add("Тема: " + ScriptData.Output[i].Subject);
                        Result.Add("Текст:\n" + ScriptData.Output[i].Message);
                        Result.Add(String.Empty);
                    }
                }
                catch (Exception sError)
                {
                    Error = sError;
                }
            });
            scriptsTestAbortTimer.Enabled = true;
            TestThread.Start();
            Task WaitScript = new Task(() =>
            {
                Thread.Sleep(100);
                TestThread.Join();
            });
            WaitScript.Start();
            await WaitScript;
            scriptsTestAbortTimer.Enabled = false;
            scriptsRun.Click -= scriptsRun_Click; // in case it wasn't removed
            scriptsRun.Click += scriptsRun_Click;
            scriptsRun.Click -= scriptsAbort_Click;
            scriptsRun.Text = "Тест";
            if (Error != null)
            {
                scriptsStatusLabel.Text = Error.Message;
                scriptsRun.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Error);
                scriptsStatusLabel.ForeColor = Color.Red;
            }
            else
            {
                scriptsResult.Clear();
                scriptsResult.Lines = Result.ToArray();
                scriptsResult.Focus();
                scriptsRun.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Complete);
                scriptsStatusLabel.ForeColor = Color.Black;
                scriptsStatusLabel.Text = "Скрипт выполнен";
            }
            scriptsSubject.TextChanged += scriptsSubject_TextChanged;
            scriptsMessage.TextChanged += scriptsSubject_TextChanged;
            scriptsRun.Enabled = true;
            scriptsListPanel.Enabled = true;
            scriptsSave.Enabled = SaveState;
            scriptsDelete.Enabled = DeleteState;
            scriptsEditor.IsReadOnly = false;
        }

        private void scriptsAbort_Click(object sender, EventArgs e)
        {
            scriptsRun.Enabled = false;
            scriptsRun.Click += scriptsRun_Click;
            scriptsRun.Click -= scriptsAbort_Click;
            ToolTip.SetToolTip(scriptsRun, String.Empty);
            TestThread.Abort();
        }

        private void scriptsTestAbortTimer_Tick(object sender, EventArgs e)
        {
            scriptsTestAbortTimer.Enabled = false;
            scriptsRun.Click -= scriptsRun_Click;
            scriptsRun.Click += scriptsAbort_Click;
            scriptsRun.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Cancelled);
            scriptsRun.Text = "Прервать";
            scriptsRun.Enabled = true;
            ToolTip.SetToolTip(scriptsRun, "Завершить процесс\n"
                + "Процесс не отвечает, возможно в коде есть бесконечный цикл.\n"
                + "Не используйте этот скрипт пока не исправите проблему!\n"
                + "Завершить процесс при выполнении основных задачь будет не возможно!");
        }

        private void scriptsSubject_TextChanged(object sender, EventArgs e)
        {
            scriptsRun.Image = TaskGui.GetTaggedIcon(TaskGui.InfoIcons.Test);
        }

        private void scriptsTestBox_Enter(object sender, EventArgs e)
        {
            if ((sender as TextBox).ForeColor == SystemColors.GrayText)
            {
                (sender as TextBox).Text = String.Empty;
                (sender as TextBox).ForeColor = SystemColors.WindowText;
                (sender as TextBox).Font = new Font((sender as TextBox).Font, FontStyle.Regular);
            }
        }

        private void scriptsSubject_Leave(object sender, EventArgs e)
        {
            if (scriptsSubject.Text == String.Empty)
            {
                scriptsSubject.Text = "Тема сообщения";
                scriptsSubject.ForeColor = SystemColors.GrayText;
                scriptsSubject.Font = new Font(scriptsSubject.Font, FontStyle.Italic);
            }
        }

        private void scriptsMessage_Leave(object sender, EventArgs e)
        {
            if (scriptsMessage.Text == String.Empty)
            {
                scriptsMessage.Text = "[b]Тестовое сообщение[b]\nСегодня [color=red]" +
                    "хорошая[/color] погода.\nМы пойдем [color=#12830a]купаться[/color] на речку.";
                scriptsMessage.ForeColor = SystemColors.GrayText;
                scriptsMessage.Font = new Font(scriptsMessage.Font, FontStyle.Italic);
            }
        }

        #endregion

        private void propScriptsAdd_Click(object sender, EventArgs e)
        {
            Tabs.TabPages.Add(scriptsTab);
            Tabs.SelectedTab = scriptsTab;
            propTab.Enabled = false;
            ValidateProperties();
        }

        #endregion

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }


       
        private void button1_Click(object sender, EventArgs e)
        {
            //CefSharp.BrowserSettings b = new CefSharp.BrowserSettings();
            //WebView w = new WebView("http://google.com", b);
            //w.Width = 200;
            //w.Height = 200;
            //w.Visible = true;
            //w.Enabled = true;
            //this.Controls.Add(w);
        }





    }
}
