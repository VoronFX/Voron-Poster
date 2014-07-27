namespace Voron_Poster
{
    partial class PostTaskGUI
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.GTName = new System.Windows.Forms.Label();
            this.GTStatus = new System.Windows.Forms.Label();
            this.GTSelected = new System.Windows.Forms.CheckBox();
            this.GTProgress = new System.Windows.Forms.ProgressBar();
            this.GTStatusIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.GTStatusIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // GTName
            // 
            this.GTName.BackColor = System.Drawing.SystemColors.Control;
            this.GTName.Location = new System.Drawing.Point(24, 0);
            this.GTName.Margin = new System.Windows.Forms.Padding(0);
            this.GTName.MaximumSize = new System.Drawing.Size(0, 24);
            this.GTName.MinimumSize = new System.Drawing.Size(0, 24);
            this.GTName.Name = "GTName";
            this.GTName.Padding = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.GTName.Size = new System.Drawing.Size(0, 24);
            this.GTName.TabIndex = 18;
            this.GTName.Text = "Тема/Раздел";
            this.GTName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // GTStatus
            // 
            this.GTStatus.BackColor = System.Drawing.SystemColors.Control;
            this.GTStatus.Location = new System.Drawing.Point(300, 5);
            this.GTStatus.Margin = new System.Windows.Forms.Padding(0);
            this.GTStatus.MaximumSize = new System.Drawing.Size(0, 24);
            this.GTStatus.MinimumSize = new System.Drawing.Size(0, 24);
            this.GTStatus.Name = "GTStatus";
            this.GTStatus.Padding = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.GTStatus.Size = new System.Drawing.Size(0, 24);
            this.GTStatus.TabIndex = 17;
            this.GTStatus.Text = "Состояние";
            this.GTStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // GTSelected
            // 
            this.GTSelected.BackColor = System.Drawing.SystemColors.Control;
            this.GTSelected.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.GTSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.GTSelected.Location = new System.Drawing.Point(0, 0);
            this.GTSelected.Margin = new System.Windows.Forms.Padding(0);
            this.GTSelected.MaximumSize = new System.Drawing.Size(24, 24);
            this.GTSelected.MinimumSize = new System.Drawing.Size(24, 24);
            this.GTSelected.Name = "GTSelected";
            this.GTSelected.Size = new System.Drawing.Size(24, 24);
            this.GTSelected.TabIndex = 14;
            this.GTSelected.ThreeState = true;
            this.GTSelected.UseVisualStyleBackColor = false;
            // 
            // GTProgress
            // 
            this.GTProgress.BackColor = System.Drawing.SystemColors.Control;
            this.GTProgress.Location = new System.Drawing.Point(474, 11);
            this.GTProgress.Maximum = 561;
            this.GTProgress.MaximumSize = new System.Drawing.Size(0, 18);
            this.GTProgress.MinimumSize = new System.Drawing.Size(0, 18);
            this.GTProgress.Name = "GTProgress";
            this.GTProgress.Size = new System.Drawing.Size(0, 18);
            this.GTProgress.TabIndex = 15;
            // 
            // GTStatusIcon
            // 
            this.GTStatusIcon.BackColor = System.Drawing.Color.Transparent;
            this.GTStatusIcon.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG;
            this.GTStatusIcon.Location = new System.Drawing.Point(336, 0);
            this.GTStatusIcon.Margin = new System.Windows.Forms.Padding(0);
            this.GTStatusIcon.MaximumSize = new System.Drawing.Size(24, 24);
            this.GTStatusIcon.MinimumSize = new System.Drawing.Size(24, 24);
            this.GTStatusIcon.Name = "GTStatusIcon";
            this.GTStatusIcon.Size = new System.Drawing.Size(24, 24);
            this.GTStatusIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.GTStatusIcon.TabIndex = 16;
            this.GTStatusIcon.TabStop = false;
            // 
            // PostTaskGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.Controls.Add(this.GTName);
            this.Controls.Add(this.GTStatus);
            this.Controls.Add(this.GTSelected);
            this.Controls.Add(this.GTProgress);
            this.Controls.Add(this.GTStatusIcon);
            this.Name = "PostTaskGUI";
            this.Size = new System.Drawing.Size(727, 79);
            ((System.ComponentModel.ISupportInitialize)(this.GTStatusIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label GTName;
        private System.Windows.Forms.Label GTStatus;
        private System.Windows.Forms.CheckBox GTSelected;
        private System.Windows.Forms.ProgressBar GTProgress;
        private System.Windows.Forms.PictureBox GTStatusIcon;
    }
}
