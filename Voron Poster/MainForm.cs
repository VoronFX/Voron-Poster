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


            f = new ForumSMF();
            f.ReqTimeout = 3000;
            f.MainPage = new Uri("http://www.simplemachines.org/community/");
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += new EventHandler((a, b) => { progressBar1.Value = f.Progress; });
            t.Start();
            await f.Login("Voron", "LEVEL2index");
            textBox1.Lines = f.Log.ToArray();
            textBox2.Text = f.h;
            //  RenderHtml(f.h);
            // this.Text = hashLoginPassword(textBox1.Lines[0], textBox1.Lines[1], textBox1.Lines[2]);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.Cancel.Cancel();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await f.PostMessage(new Uri("http://www.simplemachines.org/community/index.php?topic=524612.0"), "test3", textBox2.Text);
            textBox1.Lines = f.Log.ToArray();
            textBox2.Text += f.h;
            RenderHtml(f.h);
        }



        #region Tasks Page
        class TaskGui
        {
            TableLayoutPanel Parent;
            CheckBox Selected;
            LinkLabel Name;
            Label Status;
            PictureBox StatusIcon;
            ProgressBar Progress;
            Button StartStop;
            Button Properties;
            Button Delete;

            private void InitializeComponent()
            {
                this.Selected = new System.Windows.Forms.CheckBox();
                this.Name = new System.Windows.Forms.LinkLabel();
                this.Status = new System.Windows.Forms.Label();
                this.StatusIcon = new System.Windows.Forms.PictureBox();
                this.Progress = new System.Windows.Forms.ProgressBar();
                this.StartStop = new System.Windows.Forms.Button();
                this.Properties = new System.Windows.Forms.Button();
                this.Delete = new System.Windows.Forms.Button();
                // 
                // GTSelected
                // 
                Selected.AutoSize = false;
                Selected.Dock = System.Windows.Forms.DockStyle.Fill;
                Selected.Anchor = System.Windows.Forms.AnchorStyles.Top;
                Selected.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
                Selected.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
                Selected.Location = new System.Drawing.Point(1, 1);
                Selected.Margin = new System.Windows.Forms.Padding(0);
                Selected.MaximumSize = new System.Drawing.Size(24, 24);
                Selected.MinimumSize = new System.Drawing.Size(24, 24);
                Selected.Name = "GTSelected";
                Selected.Size = new System.Drawing.Size(24, 24);
                Selected.TabIndex = 0;
                Selected.UseVisualStyleBackColor = true;
                // 
                // GTName
                // 
                Name.AutoSize = false;
                Name.Dock = System.Windows.Forms.DockStyle.Fill;
                Name.Location = new System.Drawing.Point(28, 1);
                Name.Name = "GTName";
                Name.Size = new System.Drawing.Size(377, 24);
                Name.MaximumSize = new System.Drawing.Size(0, 24);
                Name.MinimumSize = new System.Drawing.Size(0, 24);
                Name.TabIndex = 3;
                Name.Text = "Тема/Раздел";
                Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                // 
                // GTStatus
                // 
                Status.AutoSize = false;
                Status.Dock = System.Windows.Forms.DockStyle.Fill;
                Status.Location = new System.Drawing.Point(412, 1);
                Status.Name = "GTStatus";
                Status.Size = new System.Drawing.Size(153, 24);
                Status.MaximumSize = new System.Drawing.Size(0, 24);
                Status.MinimumSize = new System.Drawing.Size(0, 24);
                Status.TabIndex = 4;
                Status.Text = "Состояние";
                Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                // 
                // GTStatusIcon
                // 
                StatusIcon.Dock = System.Windows.Forms.DockStyle.Fill;
                StatusIcon.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG;
                StatusIcon.Location = new System.Drawing.Point(569, 1);
                StatusIcon.Margin = new System.Windows.Forms.Padding(0);
                StatusIcon.MaximumSize = new System.Drawing.Size(24, 24);
                StatusIcon.MinimumSize = new System.Drawing.Size(24, 24);
                StatusIcon.Name = "GTStatusIcon";
                StatusIcon.Size = new System.Drawing.Size(24, 24);
                StatusIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                StatusIcon.TabIndex = 9;
                StatusIcon.TabStop = false;
                // 
                // GTProgress
                // 
                Progress.Dock = System.Windows.Forms.DockStyle.Fill;
                Progress.Location = new System.Drawing.Point(597, 4);
                Progress.Name = "GTProgress";
                Progress.Size = new System.Drawing.Size(69, 18);
                Progress.MaximumSize = new System.Drawing.Size(0, 18);
                Progress.MinimumSize = new System.Drawing.Size(0, 18);
                Progress.TabIndex = 2;
                // 
                // GTStartStop
                // 
                StartStop.AutoSize = false;
                StartStop.Dock = System.Windows.Forms.DockStyle.Fill;
                StartStop.Image = global::Voron_Poster.Properties.Resources.arrow_run_16xLG;
                StartStop.Location = new System.Drawing.Point(670, 1);
                StartStop.Margin = new System.Windows.Forms.Padding(0);
                StartStop.MaximumSize = new System.Drawing.Size(24, 24);
                StartStop.MinimumSize = new System.Drawing.Size(24, 24);
                StartStop.Name = "GTStartStop";
                StartStop.Size = new System.Drawing.Size(24, 24);
                StartStop.TabIndex = 8;
                StartStop.UseVisualStyleBackColor = true;
                // 
                // GTPropeties
                // 
                Properties.AutoSize = false;
                Properties.Dock = System.Windows.Forms.DockStyle.Fill;
                Properties.Image = global::Voron_Poster.Properties.Resources.gear_16xLG;
                Properties.Location = new System.Drawing.Point(695, 1);
                Properties.Margin = new System.Windows.Forms.Padding(0);
                Properties.MaximumSize = new System.Drawing.Size(24, 24);
                Properties.MinimumSize = new System.Drawing.Size(24, 24);
                Properties.Name = "GTPropeties";
                Properties.Size = new System.Drawing.Size(24, 24);
                Properties.TabIndex = 7;
                Properties.UseVisualStyleBackColor = true;
                // 
                // GTDelete
                // 
                Delete.AutoSize = false;
                Delete.Dock = System.Windows.Forms.DockStyle.Fill;
                Delete.Image = global::Voron_Poster.Properties.Resources.action_Cancel_16xLG;
                Delete.Location = new System.Drawing.Point(720, 1);
                Delete.Margin = new System.Windows.Forms.Padding(0);
                Delete.MaximumSize = new System.Drawing.Size(24, 24);
                Delete.MinimumSize = new System.Drawing.Size(24, 24);
                Delete.Name = "GTDelete";
                Delete.Size = new System.Drawing.Size(24, 24);
                Delete.TabIndex = 10;
                Delete.UseVisualStyleBackColor = true;
            }

            public TaskGui(TableLayoutPanel ParentPanel)
            {
                Parent = ParentPanel;
                InitializeComponent();
                Control[] Ctrls = new Control[] { Selected, Name, Status, StatusIcon, Progress, StartStop, Properties, Delete };
                Parent.RowCount = Parent.RowCount + 1;
                for (int i = 0; i < Ctrls.Length; i++)
                {
                    Parent.Controls.Add(Ctrls[i], i, Parent.RowCount - 1);
                    Parent.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
                }
            }
        }

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

        TaskProperties CurrentProperties = new TaskProperties();

        public class TaskProperties
        {
            public string ForumMainPage;
            public bool UseGlobalAccount;
            public string Username;
            public string Password;
            public List<String> PreProcessingScripts;
            public TaskProperties()
            {
                PreProcessingScripts = new List<string>();
            }
        }

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
                    new System.Xml.Serialization.XmlSerializer(typeof(TaskProperties));
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

    }
}
