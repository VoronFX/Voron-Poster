using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Imaging;

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

        public static MainForm MainForm;
        public enum InfoIcons
        {
            Complete, Running, Stopped, Waiting, Cancelled, Error, Run, Restart, Cancel, Clear,
            Gear, Activity, Login, Question, Save, Test, None
        }
        public static Bitmap[] InfoIconsBitmaps = InitInfoIconsBitmapsInit();
        public static string[] InfoIconsTooltips = InitInfoIconsTooltipsInit();
        private static Bitmap[] InitInfoIconsBitmapsInit()
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
        private static string[] InitInfoIconsTooltipsInit()
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

        private InfoIcons status = InfoIcons.Stopped;
        public InfoIcons Status
        {
            get { return status; }
            set
            {
                if (value != status)
                {
                    status = value;

                    MainForm.BeginInvoke((Action)(() =>
                    {
                        Ctrls.StatusIcon.Image = InfoIconsBitmaps[(int)status];

                        switch (status)
                        {
                            case InfoIcons.Error: Ctrls.Status.LinkColor = Color.Red; break;
                            case InfoIcons.Complete: Ctrls.Status.LinkColor = Color.Green; break;
                            default: Ctrls.Status.LinkColor = Color.Black; break;
                        }

                        switch (status)
                        {
                            case InfoIcons.Waiting:
                            case InfoIcons.Stopped: Ctrls.Status.LinkBehavior = LinkBehavior.NeverUnderline; break;
                            default: Ctrls.Status.LinkBehavior = LinkBehavior.HoverUnderline; break;
                        }

                        switch (status)
                        {
                            case InfoIcons.Cancelled:
                            case InfoIcons.Error: Ctrls.Progress.SetState(2); break;
                            case InfoIcons.Waiting: Ctrls.Progress.SetState(3); break;
                            default: Ctrls.Progress.SetState(1); break;
                        }
                    }));
                }
            }
        }
        public InfoIcons Action
        {
            get
            {
                switch (status)
                {
                    case InfoIcons.Complete:
                    case InfoIcons.Stopped:
                    case InfoIcons.Cancelled: return InfoIcons.Run;
                    case InfoIcons.Error: return InfoIcons.Restart;
                    default: return InfoIcons.Cancel;
                }
            }
        }

        #endregion

        public bool Selected { get; set; }
        public string TargetUrl { get; set; }
        public string StatusText { get; set; }
        public int Progress { get; set; }

        #region DataGridView

        public class DataGridViewStatusIconCell : DataGridViewImageCell
        {
            public DataGridViewStatusIconCell() : base() { }

            protected override object GetFormattedValue(object value,
            int rowIndex, ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context)
            {
                cellStyle.Alignment =
                   DataGridViewContentAlignment.MiddleCenter;
                if (value is InfoIcons || value is int)
                {

                    var StatusTextCell = OwningRow.Cells[ColumnIndex + 1] as DataGridViewLinkCell;
                    var ProgressCell = OwningRow.Cells[ColumnIndex + 2] as DataGridViewProgressCell;

                    switch ((InfoIcons)value)
                    {
                        case InfoIcons.Error: StatusTextCell.LinkColor = Color.Red; break;
                        case InfoIcons.Complete: StatusTextCell.LinkColor = Color.Green; break;
                        default: StatusTextCell.LinkColor = Color.Black; break;
                    }

                    switch ((InfoIcons)value)
                    {
                        case InfoIcons.Waiting:
                        case InfoIcons.Stopped: StatusTextCell.LinkBehavior = LinkBehavior.NeverUnderline; break;
                        default: StatusTextCell.LinkBehavior = LinkBehavior.HoverUnderline; break;
                    }

                    switch ((InfoIcons)value)
                    {
                        case InfoIcons.Cancelled:
                        case InfoIcons.Error: ProgressCell.ProgressBar.SetState(2); break;
                        case InfoIcons.Waiting: ProgressCell.ProgressBar.SetState(3); break;
                        default: ProgressCell.ProgressBar.SetState(1); break;
                    }

                    return InfoIconsBitmaps[(int)value];
                }
                else return null;
            }
        }

        public class DataGridViewStatusIconColumn : DataGridViewColumn
        {
            public DataGridViewStatusIconColumn() : base(new DataGridViewStatusIconCell()) { }
        }

        public class DataGridViewProgressCell : DataGridViewCell
        {
            public DataGridViewProgressCell() : base() { }


            protected override object GetFormattedValue(object value,
            int rowIndex, ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context)
            {
                if (value is int)
                    ProgressBar.Value = (int)value;
                return null;
            }

            public ProgressBar ProgressBar = new ProgressBar()
            {
                Maximum = 561
            };

            protected override void Paint(Graphics graphics, Rectangle clipBounds,
                Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
                object value, object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                Rectangle Borders = this.BorderWidths(advancedBorderStyle);
                var Rectangle = new Rectangle(0, 0, cellBounds.Width - cellStyle.Padding.Horizontal - Borders.Width,
                    cellBounds.Height - cellStyle.Padding.Vertical - Borders.Height);
                Bitmap b = new Bitmap(Rectangle.Width, Rectangle.Height);
                ProgressBar.Size = Rectangle.Size;
                ProgressBar.DrawToBitmap(b, Rectangle);
                Rectangle.Offset(cellBounds.Location);
                Rectangle.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);

                // Draw the cell background, if specified. 
                if ((paintParts & DataGridViewPaintParts.Background) ==
                    DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground =
                        new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified. 
                if ((paintParts & DataGridViewPaintParts.Border) ==
                    DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }

                graphics.DrawImage(b, Rectangle);
            }

            ~DataGridViewProgressCell()
            {
                if (ProgressBar.IsHandleCreated)
                    ProgressBar.Invoke((Action)(() => ProgressBar.Dispose()));
            }
        }

        public class DataGridViewProgressColumn : DataGridViewColumn
        {
            public DataGridViewProgressColumn()
                : base(new DataGridViewProgressCell()) { }
        }

        public class DataGridViewImageButtonCell : DataGridViewButtonCell
        {
            public DataGridViewImageButtonCell()
                : base()
            {
                if (OwningColumn != null)
                    Icon = (OwningColumn as DataGridViewImageButtonColumn).Icon;
            }

            protected bool enabled = true;
            public bool Enabled
            {
                get { return enabled; }
                set
                {
                    if (value != enabled)
                    {
                        enabled = value;
                        DataGridView.InvalidateCell(this);
                    }
                }
            }
            public Bitmap Icon;

            protected override object GetFormattedValue(object value,
            int rowIndex, ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context)
            {
                if (OwningColumn != null)
                    Icon = (OwningColumn as DataGridViewImageButtonColumn).Icon;
                if (value is InfoIcons)
                    switch ((InfoIcons)value)
                    {
                        case InfoIcons.Running:
                        case InfoIcons.Waiting: Icon = InfoIconsBitmaps[(int)InfoIcons.Cancel]; break;
                        case InfoIcons.Stopped:
                        case InfoIcons.Cancelled:
                        case InfoIcons.Complete: Icon = InfoIconsBitmaps[(int)InfoIcons.Run]; break;
                        case InfoIcons.Error: Icon = InfoIconsBitmaps[(int)InfoIcons.Restart]; break;
                    }
                return null;
            }

            protected override void Paint(Graphics graphics, Rectangle clipBounds,
                Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
                object value, object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                if (Icon == null) return;
                // Calculate the area in which to draw the button.
                Rectangle buttonArea = cellBounds;
                Rectangle buttonAdjustment =
                    this.BorderWidths(advancedBorderStyle);
                buttonArea.X += buttonAdjustment.X;
                buttonArea.Y += buttonAdjustment.Y;
                buttonArea.Height -= buttonAdjustment.Height;
                buttonArea.Width -= buttonAdjustment.Width;

                var Rectangle = new Rectangle((int)Math.Round((buttonArea.Width - Icon.Width) / 2F),
                    (int)Math.Round((buttonArea.Height - Icon.Height) / 2F), Icon.Width, Icon.Height);
                Rectangle.Offset(buttonArea.Location);

                ImageAttributes attributes = new ImageAttributes();
                ColorMatrix matrix = new ColorMatrix();
                float value2;

                if (this.Enabled == false)
                    value2 = 0.4f;
                else
                    value2 = 1.0f;

                matrix.Matrix33 = value2;
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                if (!Enabled)
                {
                    // Draw the cell background, if specified. 
                    if ((paintParts & DataGridViewPaintParts.Background) ==
                        DataGridViewPaintParts.Background)
                    {
                        SolidBrush cellBackground =
                            new SolidBrush(cellStyle.BackColor);
                        graphics.FillRectangle(cellBackground, cellBounds);
                        cellBackground.Dispose();
                    }

                    // Draw the cell borders, if specified. 
                    if ((paintParts & DataGridViewPaintParts.Border) ==
                        DataGridViewPaintParts.Border)
                    {
                        PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                            advancedBorderStyle);
                    }

                    // Draw the disabled button.                
                    ButtonRenderer.DrawButton(graphics, buttonArea, PushButtonState.Disabled);

                }
                else
                {

                    // The button cell is enabled, so let the base class  
                    // handle the painting. 
                    base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                        cellState, value, formattedValue, errorText,
                        cellStyle, advancedBorderStyle, paintParts);

                }
                graphics.DrawImage(Icon, Rectangle, 0, 0, Icon.Width, Icon.Height, GraphicsUnit.Pixel, attributes);
            }

        }

        public class DataGridViewImageButtonColumn : DataGridViewColumn
        {
            public DataGridViewImageButtonColumn()
                : base(new DataGridViewImageButtonCell()) { }

            public Bitmap Icon { get; set; }

            public override object Clone()
            {
                DataGridViewImageButtonColumn col = base.Clone() as DataGridViewImageButtonColumn;
                col.Icon = Icon;
                return col;
            }

        }

        #endregion

        public PostTask(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        public bool New = true;

        protected Forum forum;
        public Forum Forum
        {
            get { return forum; }
            set
            {
                forum = value;
                if (forum != null)
                {
                    forum.StatusMessageUpdate = StatusUpdate;
                    forum.Progress.ProgressUpdate = x => Progress = x;
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
        }


        static int ActiveTasks = 0;
        static ConcurrentQueue<PostTask> PostTaskQueue = new ConcurrentQueue<PostTask>();
        public static void StartNext()
        {
            while ((MainForm.Settings.MaxActiveTasks == 0 || ActiveTasks < MainForm.Settings.MaxActiveTasks)
                && !PostTaskQueue.IsEmpty)
            {
                PostTask T;
                if (PostTaskQueue.TryDequeue(out T))
                {
                    ActiveTasks++;
                    T.Status = InfoIcons.Running;
                    T.Forum.Activity.Start();
                }
            }
        }

        public void QueueTask()
        {
            Forum.AccountToUse = MainForm.Settings.Account;
            Forum.CreateActivity(async () =>
               await Forum.LoginRunScritsAndPost(new Uri(TargetUrl), MainForm.messageSubject.Text, MainForm.messageText.Text));
            Forum.Activity.ContinueWith((prevtask) =>
            {
                ActiveTasks--;
                StartNext();
            });
            Status = InfoIcons.Waiting;
            Forum.StatusMessage = "В очереди";
            PostTaskQueue.Enqueue(this);
        }

        public async Task AwaitForComplete()
        {
            await Forum.Activity;
            if (Forum.Error is Forum.UserCancelledException)
                Status = InfoIcons.Cancelled;
            else if (Forum.Error != null)
                Status = InfoIcons.Error;
            else
                Status = InfoIcons.Complete;
        }

    }


}
