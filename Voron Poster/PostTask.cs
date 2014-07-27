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

    public class PostTask
    {

        #region Gui

        private MainForm MainForm;
        public enum InfoIcons
        {
            Complete, Running, Stopped, Waiting, Cancelled, Error, Run, Restart, Cancel, Clear,
            Gear, Activity, Login, Question, Save, Test, None
        }
        public static Bitmap[] InfoIconsBitmaps = InitInfoIconsBitmap();
        public static string[] InfoIconsTooltips = InitInfoIconsTooltip();
        private static Bitmap[] InitInfoIconsBitmap()
        {
            var Bitmaps = new Bitmap[Enum.GetValues(typeof(InfoIcons)).Length];
            // Statuses
            Bitmaps[(int)InfoIcons.Complete] = global::Voron_Poster.Properties.Resources.StatusAnnotations_Complete_and_ok_16xLG_color;
            Bitmaps[(int)InfoIcons.Running] = global::Voron_Poster.Properties.Resources.StatusAnnotations_Play_16xLG;
            Bitmaps[(int)InfoIcons.Stopped] = global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG;
            Bitmaps[(int)InfoIcons.Waiting] = global::Voron_Poster.Properties.Resources.StatusAnnotations_Pause_16xLG;
            Bitmaps[(int)InfoIcons.Cancelled] = global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG_color;
            Bitmaps[(int)InfoIcons.Error] = global::Voron_Poster.Properties.Resources.StatusAnnotations_Critical_16xLG_color;
            // Actions
            Bitmaps[(int)InfoIcons.Run] = global::Voron_Poster.Properties.Resources.arrow_run_16xLG;
            Bitmaps[(int)InfoIcons.Restart] = global::Voron_Poster.Properties.Resources.Restart_6322;
            Bitmaps[(int)InfoIcons.Cancel] = global::Voron_Poster.Properties.Resources.Symbols_Stop_16xLG;
            Bitmaps[(int)InfoIcons.Clear] = Bitmaps[(int)InfoIcons.Stopped];
            // OtherStuff
            Bitmaps[(int)InfoIcons.Gear] = global::Voron_Poster.Properties.Resources.gear_16xLG;
            Bitmaps[(int)InfoIcons.Activity] = global::Voron_Poster.Properties.Resources.Activity_16xLG;
            Bitmaps[(int)InfoIcons.Login] = global::Voron_Poster.Properties.Resources.user_16xLG;
            Bitmaps[(int)InfoIcons.Question] = global::Voron_Poster.Properties.Resources.StatusAnnotations_Help_and_inconclusive_16xLG;
            Bitmaps[(int)InfoIcons.Save] = global::Voron_Poster.Properties.Resources.save_16xLG;
            Bitmaps[(int)InfoIcons.Test] = global::Voron_Poster.Properties.Resources.test_32x_SMcuted;
            for (int i = 0; i < Bitmaps.Length; i++)
            {
                if (Bitmaps[i] != null)
                    Bitmaps[i].Tag = Enum.GetName(typeof(InfoIcons), (InfoIcons)i);
            }
            return Bitmaps;
        }
        private static string[] InitInfoIconsTooltip()
        {
            var Tooltips = new string[Enum.GetValues(typeof(InfoIcons)).Length];
            // Statuses
            Tooltips[(int)InfoIcons.Complete] = "Выполнено";
            Tooltips[(int)InfoIcons.Running] = "Выполянется...";
            Tooltips[(int)InfoIcons.Stopped] = "Остановлено";
            Tooltips[(int)InfoIcons.Waiting] = "В очереди...";
            Tooltips[(int)InfoIcons.Cancelled] = "Отменено";
            Tooltips[(int)InfoIcons.Error] = "Ошибка";
            // Actions
            Tooltips[(int)InfoIcons.Run] = "Старт";
            Tooltips[(int)InfoIcons.Restart] = "Повторить";
            Tooltips[(int)InfoIcons.Cancel] = "Отменить";
            Tooltips[(int)InfoIcons.Clear] = "Сбросить статус";
            return Tooltips;
        }

        private InfoIcons status = InfoIcons.Stopped, action = InfoIcons.Run;
        public InfoIcons Status
        {
            get { return status; }
            set
            {
                if (value != status)
                {
                    status = value;

                    switch (status)
                    {
                        case InfoIcons.Error: Action = InfoIcons.Restart; break;
                        case InfoIcons.Complete:
                        case InfoIcons.Stopped:
                        case InfoIcons.Cancelled: Action = InfoIcons.Run; break;
                        default: Action = InfoIcons.Cancel; break;
                    }

                    MainForm.BeginInvoke((Action)(() =>
                    {
                        Ctrls.StatusIcon.Image = InfoIconsBitmaps[(int)status];

                        if (status == InfoIcons.Error)
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

                        switch (status)
                        {
                            case InfoIcons.Error: ModifyProgressBarColor.SetState(Ctrls.Progress, 2); break;
                            case InfoIcons.Waiting: ModifyProgressBarColor.SetState(Ctrls.Progress, 3); break;
                            default: ModifyProgressBarColor.SetState(Ctrls.Progress, 1); break;
                        }
                    }));
                }
            }
        }
        public InfoIcons Action
        {
            get { return action; }
            private set
            {
                action = value;
                Ctrls.StartStop.BeginInvoke((Action)(() =>
                {
                    Ctrls.StartStop.Image = InfoIconsBitmaps[(int)action];
                }));
            }
        }
        /// <summary>
        /// Deprecated
        /// </summary>
        public void SetStatusIcon()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            // if (Forum != null && Monitor.TryEnter(Forum.Log, TimeSpan.Zero))
            // {
            //     // lock (Forum.Log)
            //     // {
            //     string StatusText = Forum.Log.Last<string>();
            //     Ctrls.Status.Text = StatusText;
            //     //     //Ctrls.Status.Text = new string(StatusText.Skip(Math.Max(0, StatusText.IndexOf(":")+1)).ToArray());
            //     // }
            //     Monitor.Exit(Forum.Log);
            // }
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 10)
                Console.WriteLine("# 1 - {0}", stopwatch.ElapsedMilliseconds);
            stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            if (Forum == null || Forum.Activity == null)
            {
                Ctrls.StatusIcon.Image = InfoIconsBitmaps[(int)InfoIcons.Stopped];
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

            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 10)
                Console.WriteLine("# 2 - {0}", stopwatch.ElapsedMilliseconds);
            stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            //   ModifyProgressBarColor.SetState(Ctrls.Progress, 1); // Not-green colors sometimes ignores progress changes
            //if (Status == InfoIcons.Cancelled || Status == InfoIcons.Stopped) Ctrls.Progress.Value = 0;
            //else
            //    Ctrls.Progress.Value = Math.Min(561, Forum.progress.Login + Forum.Progress[1] / 5 + Forum.progress.Post);
            //Ctrls.StatusIcon.Image = InfoIconsBitmaps[(int)Status];
            //Ctrls.StartStop.Image = InfoIconsBitmaps[(int)Action];

            switch (Status)
            {
                case InfoIcons.Error: ModifyProgressBarColor.SetState(Ctrls.Progress, 2); break;
                case InfoIcons.Waiting: ModifyProgressBarColor.SetState(Ctrls.Progress, 3); break;
                default: ModifyProgressBarColor.SetState(Ctrls.Progress, 1); break;
            }

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

            // MainForm.ToolTip.SetToolTip(Ctrls.StatusIcon, GetTooltip(Status));
            //MainForm.ToolTip.SetToolTip(Ctrls.StartStop, GetTooltip(Action));
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 10)
                Console.WriteLine("# 3 - {0}", stopwatch.ElapsedMilliseconds);
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
                Name.Location = new System.Drawing.Point(0, 0);
                Name.Name = "GTName";
                Name.Size = new System.Drawing.Size(377, 24);
               // Name.MaximumSize = new System.Drawing.Size(0, 24);
            //    Name.MinimumSize = new System.Drawing.Size(0, 24);
                Name.TabIndex = 3;
                Name.Text = "Тема/Раздел";
                Name.Padding = new Padding(3, 6, 3, 0);
                Name.TextAlign = System.Drawing.ContentAlignment.TopLeft;
                Name.BackColor = Color.Transparent;
                // 
                // GTStatus
                // 
                Status.AutoSize = false;
                Status.Dock = DockStyle.Fill;
                Status.Location = new System.Drawing.Point(0, 0);
                Status.Name = "GTStatus";
                Status.Size = new System.Drawing.Size(153, 24);
                // Status.MaximumSize = new System.Drawing.Size(0, 24);
                // Status.MinimumSize = new System.Drawing.Size(0, 24);
                Status.TabIndex = 4;
                Status.Text = "Остановлено";
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
                StatusIcon.Image = InfoIconsBitmaps[(int)InfoIcons.Stopped];
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
                Progress.Maximum = 561;
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

        public static void ResizeEnd(Control control)
        {
                Size s = control.ClientSize;
                control.Dock = DockStyle.None;
                control.Size = s;
        }

        #endregion

        public PostTask(MainForm Parent)
        {
            MainForm = Parent;
            Ctrls.InitializeControls();
            MainForm.ToolTip.SetToolTip(Ctrls.Delete, "Удалить");
            MainForm.ToolTip.SetToolTip(Ctrls.Properties, "Опции");
            Ctrls.StartStop.Click += StartStop_Clicked;
            Ctrls.Delete.Click += Delete_Clicked;
            Ctrls.Properties.Click += Properties_Clicked;
            Ctrls.Name.LinkClicked += Name_LinkClicked;
            Ctrls.Status.LinkClicked += Status_LinkClicked;
            AddToGuiTable();
        }

        public bool New = true;

        public string TargetUrl;
        protected Forum forum;
        public Forum Forum
        {
            get { return forum; }
            set
            {
                forum = value;
                if (forum != null)
                {
                    forum.Progress.ProgressChanged += Progress_ProgressChanged;
                    forum.StatusUpdate = StatusUpdate;
                }
            }
        }

        private void StatusUpdate(string status)
        {
            if (Status == InfoIcons.Running || Status == InfoIcons.Waiting)
            {
                if (Forum.WaitingForQueue) Status = InfoIcons.Waiting;
                else Status = InfoIcons.Running;
            }
            Ctrls.Status.BeginInvoke((Action)(() =>
            {
                Ctrls.Status.Text = status;
                MainForm.ToolTip.SetToolTip(Ctrls.Status, status);
            }));
        }

        private void Progress_ProgressChanged(object sender, int e)
        {
            Ctrls.Progress.Value = e;
        }

        public void Properties_Clicked(object sender, EventArgs e)
        {
            Ctrls.Properties.Enabled = false;
            Ctrls.StartStop.Enabled = false;
            Ctrls.Delete.Enabled = false;
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

        public void Delete_Clicked(object sender, EventArgs e)
        {
            MainForm.tasksTable.SuspendLayout();
            for (int c = 0; c < Ctrls.AsArray.Length; c++)
            {
                int r = MainForm.tasksTable.GetRow(Ctrls.AsArray[c]);
                MainForm.tasksTable.Controls.Remove(Ctrls.AsArray[c]);
                Ctrls.AsArray[c].Dispose();
                for (r = r + 1; r < MainForm.tasksTable.RowCount; r++)
                {
                    Control ControlBelow = MainForm.tasksTable.GetControlFromPosition(c, r);
                    if (ControlBelow != null)
                        MainForm.tasksTable.SetRow(ControlBelow, r - 1);
                }
            }
            lock (MainForm.Tasks) MainForm.Tasks.Remove(this);
            MainForm.tasksTable.RowCount -= 1;
            MainForm.tasksTable.RowStyles[MainForm.tasksTable.RowCount - 1].SizeType = SizeType.AutoSize;
            MainForm.tasksTable.ResumeLayout();
        }

        public async void StartStop_Clicked(object sender, EventArgs e)
        {
            Ctrls.StartStop.Enabled = false;
            Ctrls.Delete.Enabled = false;
            Ctrls.Properties.Enabled = false;
            if (Action == InfoIcons.Cancel)
                Forum.Cancel.Cancel();
            else
            {
                try
                {
                    var Settings = MainForm.Settings;
                    var GlobalAccount = Settings.Account;
                    Forum.AccountToUse = GlobalAccount;
                    Forum.Activity =
                        Forum.Run(new Uri(TargetUrl), MainForm.messageSubject.Text, MainForm.messageText.Text);
                    Status = InfoIcons.Running;
                    Ctrls.StartStop.Enabled = true;
                    Forum.Error = await Forum.Activity;
                }
                catch (Exception Error)
                {
                    Forum.Error = Error;
                    //if (!(Error is OperationCanceledException))
                    //    throw;
                }
                if (Forum.Error != null)
                {
                    if (Forum.Cancel.IsCancellationRequested)
                    {
                        Forum.StatusMessage = "Отменено";
                        Status = InfoIcons.Cancelled;
                    }
                    else
                    {
                        Status = InfoIcons.Error;
                        if (Forum.Error is OperationCanceledException)
                            Forum.StatusMessage = "Ошибка: Время ожидания истекло";
                        else
                            Forum.StatusMessage = "Ошибка: " + Forum.Error.Message;
                    }
                }
                else Status = InfoIcons.Complete;
                Ctrls.Delete.Enabled = true;
                Ctrls.Properties.Enabled = true;
                Ctrls.StartStop.Enabled = true;
            }
        }

    }


}
