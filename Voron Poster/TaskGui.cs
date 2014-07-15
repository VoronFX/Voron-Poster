using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voron_Poster
{

    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }

    public class TaskGui
    {

        #region Gui

        private MainForm MainForm;
        public enum InfoIcons
        {
            Complete, Running, Stopped, Waiting, Cancelled, Error, Run, Restart, Cancel, Clear,
            Gear, Activity, Login, Question, Save, Test, None
        }
        private static Bitmap GetIcon(InfoIcons Info)
        {

            switch (Info)
            {   //Statuses
                case InfoIcons.Complete: return global::Voron_Poster.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
                case InfoIcons.Running: return global::Voron_Poster.Properties.Resources.StatusAnnotations_Play_16xLG;
                case InfoIcons.Stopped: return global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG;
                case InfoIcons.Waiting: return global::Voron_Poster.Properties.Resources.StatusAnnotations_Pause_16xLG;
                case InfoIcons.Cancelled: return global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG_color;
                case InfoIcons.Error: return global::Voron_Poster.Properties.Resources.StatusAnnotations_Critical_16xLG_color;
                //Actions
                case InfoIcons.Run: return global::Voron_Poster.Properties.Resources.arrow_run_16xLG;
                case InfoIcons.Restart: return global::Voron_Poster.Properties.Resources.Restart_6322;
                case InfoIcons.Cancel: return global::Voron_Poster.Properties.Resources.Symbols_Stop_16xLG;
                case InfoIcons.Clear: return GetTaggedIcon(InfoIcons.Stopped);
                //OtherStuff
                case InfoIcons.Gear: return global::Voron_Poster.Properties.Resources.gear_16xLG;
                case InfoIcons.Activity: return global::Voron_Poster.Properties.Resources.Activity_16xLG;
                case InfoIcons.Login: return global::Voron_Poster.Properties.Resources.user_16xLG;
                case InfoIcons.Question: return global::Voron_Poster.Properties.Resources.StatusAnnotations_Help_and_inconclusive_16xLG;
                case InfoIcons.Save: return global::Voron_Poster.Properties.Resources.save_16xLG;
                case InfoIcons.Test: return global::Voron_Poster.Properties.Resources.test_32x_SMcuted;
                default: return null;
            }
        }
        public static Bitmap GetTaggedIcon(InfoIcons Info)
        {
            Bitmap Icon = GetIcon(Info);
            Icon.Tag = Enum.GetName(typeof(InfoIcons), Info);
            return Icon;
        }
        public static InfoIcons GetInfo(Bitmap Icon)
        {
            return (InfoIcons)(Enum.Parse(typeof(InfoIcons), (string)Icon.Tag));
        }
        public static string GetTooltip(InfoIcons Info)
        {
            switch (Info)
            {   //Statuses
                case InfoIcons.Complete: return "Выполнено";
                case InfoIcons.Running: return "Выполянется...";
                case InfoIcons.Stopped: return "Остановлено";
                case InfoIcons.Waiting: return "В очереди...";
                case InfoIcons.Cancelled: return "Отменено";
                case InfoIcons.Error: return "Ошибка";
                //Actions
                case InfoIcons.Run: return "Старт";
                case InfoIcons.Restart: return "Повторить";
                case InfoIcons.Cancel: return "Отменить";
                case InfoIcons.Clear: return "Сбросить статус";
                default: return null;
            }
        }
        public InfoIcons Status = InfoIcons.Stopped, Action = InfoIcons.Run;
        public void SetStatusIcon()
        {
            if (Forum != null)
                lock (Forum.Log)
                {
                     string StatusText = Forum.Log.Last<string>();
                     Ctrls.Status.Text = new string(StatusText.Skip(Math.Max(0, StatusText.IndexOf(":")+1)).ToArray());
                }
            MainForm.ToolTip.SetToolTip(Ctrls.Status, Ctrls.Status.Text);
            if (Forum == null || Forum.Activity == null)
            {
                Ctrls.StatusIcon.Image = GetTaggedIcon(InfoIcons.Stopped);
                Status = InfoIcons.Stopped;
                Action = InfoIcons.Run;
            }
            else
            {
                switch (Forum.Activity.Status)
                {
                    case TaskStatus.Running:
                    case TaskStatus.WaitingForActivation:
                        if (Forum.WaitingForQueue)
                        {
                            Status = InfoIcons.Waiting;
                            Action = InfoIcons.Cancel;
                        }
                        else
                        {
                            Status = InfoIcons.Running;
                            Action = InfoIcons.Cancel;
                        }
                        break;
                    case TaskStatus.RanToCompletion:
                        if (Forum.Error == null)
                        {
                            Status = InfoIcons.Complete;
                            Action = InfoIcons.Run;
                        }
                        else if (Forum.Error is OperationCanceledException)
                        {
                            Status = InfoIcons.Cancelled;
                            Action = InfoIcons.Run;
                        }
                        else
                        {
                            Status = InfoIcons.Error;
                            Action = InfoIcons.Restart;
                        }
                        break;
                    case TaskStatus.Created:
                    case TaskStatus.WaitingToRun:
                        Status = InfoIcons.Waiting;
                        Action = InfoIcons.Cancel;
                        break;
                    case TaskStatus.Canceled:
                        Status = InfoIcons.Cancelled;
                        Action = InfoIcons.Run;
                        break;
                    default:
                        Status = InfoIcons.Error;
                        Action = InfoIcons.Restart;
                        break;
                }
            }
            if (Status == InfoIcons.Cancelled || Status == InfoIcons.Stopped) Ctrls.Progress.Value = 0;
            else
                Ctrls.Progress.Value = Math.Min(765, Forum.Progress[0] + Forum.Progress[1] + Forum.Progress[2]);
            Ctrls.StatusIcon.Image = GetTaggedIcon(Status);
            Ctrls.StartStop.Image = GetTaggedIcon(Action);

            if (Status == InfoIcons.Error)
            {
                ModifyProgressBarColor.SetState(Ctrls.Progress, 2);
            }
            else ModifyProgressBarColor.SetState(Ctrls.Progress, 1);

            if (Status == InfoIcons.Error)
            {
                Ctrls.Status.LinkColor = Color.Red;
                Ctrls.Status.LinkBehavior = LinkBehavior.HoverUnderline;
            }
            else if (Status == InfoIcons.Complete)
            {
                Ctrls.Status.LinkColor = Color.Green;
                Ctrls.Status.LinkBehavior = LinkBehavior.HoverUnderline;
            }
            else
            {
                Ctrls.Status.LinkColor = Color.Black;
                Ctrls.Status.LinkBehavior = LinkBehavior.NeverUnderline;
            }

            MainForm.ToolTip.SetToolTip(Ctrls.StatusIcon, GetTooltip(Status));
            MainForm.ToolTip.SetToolTip(Ctrls.StartStop, GetTooltip(Action));
        }

        public struct TaskGuiControls
        {
            public CheckBox Selected;
            public LinkLabel Name;
            public LinkLabel Status;
            public PictureBox StatusIcon;
            public ProgressBar Progress;
            public Button StartStop;
            public Button Properties;
            public Button Delete;
            public Control[] AsArray;
            public void InitializeControls()
            {
                this.Selected = new System.Windows.Forms.CheckBox();
                this.Name = new System.Windows.Forms.LinkLabel();
                this.Status = new System.Windows.Forms.LinkLabel();
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
                Selected.BackColor = Color.Transparent;
                Selected.Checked = true;
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
                Name.Padding = new Padding(3, 6, 3, 0);
                Name.TextAlign = System.Drawing.ContentAlignment.TopLeft;
                Name.BackColor = Color.Transparent;
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
                Status.Padding = new Padding(3, 6, 3, 0);
                Status.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                Status.LinkColor = Color.Black;
                Status.LinkBehavior = LinkBehavior.NeverUnderline;
                Status.ActiveLinkColor = Color.Black;
                Status.BackColor = Color.Transparent;
                // 
                // GTStatusIcon
                // 
                StatusIcon.Dock = System.Windows.Forms.DockStyle.Fill;
                StatusIcon.Image = GetTaggedIcon(InfoIcons.Stopped);
                StatusIcon.Location = new System.Drawing.Point(569, 1);
                StatusIcon.Margin = new System.Windows.Forms.Padding(0);
                StatusIcon.MaximumSize = new System.Drawing.Size(24, 24);
                StatusIcon.MinimumSize = new System.Drawing.Size(24, 24);
                StatusIcon.Name = "GTStatusIcon";
                StatusIcon.Size = new System.Drawing.Size(24, 24);
                StatusIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                StatusIcon.TabIndex = 9;
                StatusIcon.TabStop = false;
                StatusIcon.BackColor = Color.Transparent;
                // 
                // GTProgress
                // 
                Progress.Dock = System.Windows.Forms.DockStyle.Fill;
                Progress.Location = new System.Drawing.Point(597, 4);
                Progress.Name = "GTProgress";
                Progress.Size = new System.Drawing.Size(69, 18);
                Progress.MaximumSize = new System.Drawing.Size(0, 18);
                Progress.MinimumSize = new System.Drawing.Size(0, 18);
                Progress.Maximum = 765;
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
                StartStop.BackColor = Color.Transparent;
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
                Properties.BackColor = Color.Transparent;
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
                AsArray = new Control[] { Selected, Name, Status, StatusIcon, Progress, StartStop, Properties, Delete };
            }
        }

        public TaskGuiControls Ctrls;

        private void AddToGuiTable()
        {
            MainForm.tasksTable.RowCount = MainForm.tasksTable.RowCount + 1;
            for (int i = 0; i < Ctrls.AsArray.Length; i++)
            {
                MainForm.tasksTable.Controls.Add(Ctrls.AsArray[i], i, MainForm.tasksTable.RowCount - 2);
                MainForm.tasksTable.RowStyles[MainForm.tasksTable.RowCount - 2].SizeType = SizeType.Absolute;
                MainForm.tasksTable.RowStyles[MainForm.tasksTable.RowCount - 2].Height = 24F;
                MainForm.tasksTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
        }

        #endregion

        public TaskGui(MainForm Parent)
        {
            MainForm = Parent;
            Ctrls.InitializeControls();
            MainForm.ToolTip.SetToolTip(Ctrls.Delete, "Удалить");
            MainForm.ToolTip.SetToolTip(Ctrls.Properties, "Опции");
            Ctrls.StartStop.Click += StartStop;
            Ctrls.Delete.Click += Delete;
            Ctrls.Properties.Click += Properties;
            Ctrls.Name.LinkClicked += Name_LinkClicked;
            Ctrls.Status.LinkClicked += Status_LinkClicked;
            AddToGuiTable();
        }

        public bool New = true;

        public string TargetUrl;
        public Forum Forum;

        public void Properties(object sender, EventArgs e)
        {
            MainForm.CurrTask = this;
            MainForm.propShow();
        }

        private void Name_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start((sender as LinkLabel).Text);
        }

        private void Status_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Ctrls.Status.LinkColor != Color.Black && Forum != null)
                Forum.ShowData(TargetUrl);
        }

        public void Delete(object sender, EventArgs e)
        {
            for (int c = 0; c < Ctrls.AsArray.Length; c++)
            {
                int r = MainForm.tasksTable.GetRow(Ctrls.AsArray[c]);
                MainForm.tasksTable.Controls.Remove(Ctrls.AsArray[c]);
                Ctrls.AsArray[c].Dispose();
                for (r = r + 1; r < MainForm.tasksTable.RowCount; r++)
                {
                    Control ControlUnder = MainForm.tasksTable.GetControlFromPosition(c, r);
                    if (ControlUnder != null)
                        MainForm.tasksTable.SetRow(ControlUnder, r - 1);
                }
            }
            lock (MainForm.Tasks) MainForm.Tasks.Remove(this);
            MainForm.tasksTable.RowCount -= 1;
            MainForm.tasksTable.RowStyles[MainForm.tasksTable.RowCount - 1].SizeType = SizeType.AutoSize;
        }
        
        public async void StartStop(object sender, EventArgs e)
        {
            Ctrls.StartStop.Enabled = false;
            Ctrls.Delete.Enabled = false;
            Ctrls.Properties.Enabled = false;
            if (Action == InfoIcons.Cancel)
            {
                Ctrls.StartStop.Enabled = false;
                Forum.Cancel.Cancel();
            }
            else
            {
                try
                {
                    Forum.Reset();
                    Forum.Activity =
                        Forum.Run(new Uri(TargetUrl), MainForm.messageSubject.Text, MainForm.messageText.Text);
                    SetStatusIcon();
                    Ctrls.StartStop.Enabled = true;
                    Forum.Error = await Forum.Activity;
                }
                catch (Exception Error)
                {
                    if (!(Error is OperationCanceledException))
                        throw;
                    Forum.Error = Error;
                }
                if (Forum.Error != null)
                {
                    if (Forum.Error is OperationCanceledException)
                        lock (Forum.Log) Forum.Log.Add("Отменено");
                    else
                        lock (Forum.Log) Forum.Log.Add("Ошибка\n" + Forum.Error.Message);
                }
                SetStatusIcon();
                Ctrls.Delete.Enabled = true;
                Ctrls.Properties.Enabled = true;
                Ctrls.StartStop.Enabled = true;
            }
        }

    }


}
