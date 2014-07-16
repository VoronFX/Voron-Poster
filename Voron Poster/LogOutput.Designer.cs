namespace Voron_Poster
{
    partial class LogOutput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ResponseList = new System.Windows.Forms.ListBox();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.BrowserTab = new System.Windows.Forms.TabPage();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.HtmlTab = new System.Windows.Forms.TabPage();
            this.ResponseTab = new System.Windows.Forms.TabPage();
            this.VariablesTab = new System.Windows.Forms.TabPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.reportTab = new System.Windows.Forms.TabPage();
            this.reportSend = new System.Windows.Forms.Button();
            this.reportAccountInclude = new System.Windows.Forms.CheckBox();
            this.propProfileSave = new System.Windows.Forms.Button();
            this.propProfileLoad = new System.Windows.Forms.Button();
            this.reportComments = new System.Windows.Forms.TextBox();
            this.reportSendGroup = new System.Windows.Forms.GroupBox();
            this.reportCommentsLabel = new System.Windows.Forms.Label();
            this.reportSaveLoad = new System.Windows.Forms.GroupBox();
            this.reportIKnowAboutPrivateInfo = new System.Windows.Forms.CheckBox();
            this.reportEmail = new System.Windows.Forms.TextBox();
            this.reportEmailLabel = new System.Windows.Forms.Label();
            this.reportInfoLabel = new System.Windows.Forms.Label();
            this.Tabs.SuspendLayout();
            this.BrowserTab.SuspendLayout();
            this.reportTab.SuspendLayout();
            this.reportSendGroup.SuspendLayout();
            this.reportSaveLoad.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResponseList
            // 
            this.ResponseList.Dock = System.Windows.Forms.DockStyle.Left;
            this.ResponseList.Location = new System.Drawing.Point(0, 0);
            this.ResponseList.Name = "ResponseList";
            this.ResponseList.Size = new System.Drawing.Size(129, 459);
            this.ResponseList.TabIndex = 2;
            this.ResponseList.SelectedIndexChanged += new System.EventHandler(this.ResponseList_SelectedIndexChanged);
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.BrowserTab);
            this.Tabs.Controls.Add(this.HtmlTab);
            this.Tabs.Controls.Add(this.ResponseTab);
            this.Tabs.Controls.Add(this.VariablesTab);
            this.Tabs.Controls.Add(this.reportTab);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(129, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(545, 459);
            this.Tabs.TabIndex = 3;
            // 
            // BrowserTab
            // 
            this.BrowserTab.Controls.Add(this.Browser);
            this.BrowserTab.Location = new System.Drawing.Point(4, 22);
            this.BrowserTab.Name = "BrowserTab";
            this.BrowserTab.Padding = new System.Windows.Forms.Padding(3);
            this.BrowserTab.Size = new System.Drawing.Size(667, 544);
            this.BrowserTab.TabIndex = 0;
            this.BrowserTab.Text = "Браузер";
            this.BrowserTab.UseVisualStyleBackColor = true;
            // 
            // Browser
            // 
            this.Browser.AllowNavigation = false;
            this.Browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Browser.Location = new System.Drawing.Point(3, 3);
            this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.Browser.Name = "Browser";
            this.Browser.ScriptErrorsSuppressed = true;
            this.Browser.Size = new System.Drawing.Size(661, 538);
            this.Browser.TabIndex = 0;
            // 
            // HtmlTab
            // 
            this.HtmlTab.Location = new System.Drawing.Point(4, 22);
            this.HtmlTab.Name = "HtmlTab";
            this.HtmlTab.Padding = new System.Windows.Forms.Padding(3);
            this.HtmlTab.Size = new System.Drawing.Size(667, 544);
            this.HtmlTab.TabIndex = 1;
            this.HtmlTab.Text = "Код страницы";
            this.HtmlTab.UseVisualStyleBackColor = true;
            // 
            // ResponseTab
            // 
            this.ResponseTab.Location = new System.Drawing.Point(4, 22);
            this.ResponseTab.Name = "ResponseTab";
            this.ResponseTab.Padding = new System.Windows.Forms.Padding(3);
            this.ResponseTab.Size = new System.Drawing.Size(667, 544);
            this.ResponseTab.TabIndex = 3;
            this.ResponseTab.Text = "Запрос \\ Ответ";
            this.ResponseTab.UseVisualStyleBackColor = true;
            // 
            // VariablesTab
            // 
            this.VariablesTab.Location = new System.Drawing.Point(4, 22);
            this.VariablesTab.Name = "VariablesTab";
            this.VariablesTab.Padding = new System.Windows.Forms.Padding(3);
            this.VariablesTab.Size = new System.Drawing.Size(667, 544);
            this.VariablesTab.TabIndex = 2;
            this.VariablesTab.Text = "Переменные";
            this.VariablesTab.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(129, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 459);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // reportTab
            // 
            this.reportTab.Controls.Add(this.reportSendGroup);
            this.reportTab.Location = new System.Drawing.Point(4, 22);
            this.reportTab.Name = "reportTab";
            this.reportTab.Padding = new System.Windows.Forms.Padding(13);
            this.reportTab.Size = new System.Drawing.Size(537, 433);
            this.reportTab.TabIndex = 4;
            this.reportTab.Text = "Отчет";
            this.reportTab.UseVisualStyleBackColor = true;
            // 
            // reportSend
            // 
            this.reportSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reportSend.Enabled = false;
            this.reportSend.Image = global::Voron_Poster.Properties.Resources.Bug_16xLG;
            this.reportSend.Location = new System.Drawing.Point(374, 126);
            this.reportSend.Name = "reportSend";
            this.reportSend.Size = new System.Drawing.Size(120, 23);
            this.reportSend.TabIndex = 0;
            this.reportSend.Text = "Отправить отчет";
            this.reportSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.reportSend.UseVisualStyleBackColor = true;
            this.reportSend.Click += new System.EventHandler(this.reportSend_Click);
            // 
            // reportAccountInclude
            // 
            this.reportAccountInclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportAccountInclude.Location = new System.Drawing.Point(14, 65);
            this.reportAccountInclude.Name = "reportAccountInclude";
            this.reportAccountInclude.Size = new System.Drawing.Size(354, 17);
            this.reportAccountInclude.TabIndex = 1;
            this.reportAccountInclude.Text = "Включить в отчет данные авторизации";
            this.reportAccountInclude.UseVisualStyleBackColor = true;
            // 
            // propProfileSave
            // 
            this.propProfileSave.Image = global::Voron_Poster.Properties.Resources.save_16xLG;
            this.propProfileSave.Location = new System.Drawing.Point(11, 19);
            this.propProfileSave.Name = "propProfileSave";
            this.propProfileSave.Size = new System.Drawing.Size(97, 24);
            this.propProfileSave.TabIndex = 5;
            this.propProfileSave.Text = "Сохранить";
            this.propProfileSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propProfileSave.UseVisualStyleBackColor = true;
            this.propProfileSave.Click += new System.EventHandler(this.propProfileSave_Click);
            // 
            // propProfileLoad
            // 
            this.propProfileLoad.Image = global::Voron_Poster.Properties.Resources.Open_6529;
            this.propProfileLoad.Location = new System.Drawing.Point(11, 49);
            this.propProfileLoad.Name = "propProfileLoad";
            this.propProfileLoad.Size = new System.Drawing.Size(97, 24);
            this.propProfileLoad.TabIndex = 4;
            this.propProfileLoad.Text = "Открыть";
            this.propProfileLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propProfileLoad.UseVisualStyleBackColor = true;
            this.propProfileLoad.Click += new System.EventHandler(this.propProfileLoad_Click);
            // 
            // reportComments
            // 
            this.reportComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportComments.Location = new System.Drawing.Point(11, 167);
            this.reportComments.Multiline = true;
            this.reportComments.Name = "reportComments";
            this.reportComments.Size = new System.Drawing.Size(483, 173);
            this.reportComments.TabIndex = 6;
            // 
            // reportSendGroup
            // 
            this.reportSendGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportSendGroup.Controls.Add(this.reportSaveLoad);
            this.reportSendGroup.Controls.Add(this.reportInfoLabel);
            this.reportSendGroup.Controls.Add(this.reportEmailLabel);
            this.reportSendGroup.Controls.Add(this.reportEmail);
            this.reportSendGroup.Controls.Add(this.reportIKnowAboutPrivateInfo);
            this.reportSendGroup.Controls.Add(this.reportCommentsLabel);
            this.reportSendGroup.Controls.Add(this.reportComments);
            this.reportSendGroup.Controls.Add(this.reportAccountInclude);
            this.reportSendGroup.Controls.Add(this.reportSend);
            this.reportSendGroup.Location = new System.Drawing.Point(16, 16);
            this.reportSendGroup.Name = "reportSendGroup";
            this.reportSendGroup.Padding = new System.Windows.Forms.Padding(8, 3, 8, 8);
            this.reportSendGroup.Size = new System.Drawing.Size(505, 351);
            this.reportSendGroup.TabIndex = 7;
            this.reportSendGroup.TabStop = false;
            this.reportSendGroup.Text = "Отправить отчет";
            // 
            // reportCommentsLabel
            // 
            this.reportCommentsLabel.AutoSize = true;
            this.reportCommentsLabel.Location = new System.Drawing.Point(11, 151);
            this.reportCommentsLabel.Name = "reportCommentsLabel";
            this.reportCommentsLabel.Size = new System.Drawing.Size(80, 13);
            this.reportCommentsLabel.TabIndex = 8;
            this.reportCommentsLabel.Text = "Комментарии:";
            // 
            // reportSaveLoad
            // 
            this.reportSaveLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reportSaveLoad.Controls.Add(this.propProfileLoad);
            this.reportSaveLoad.Controls.Add(this.propProfileSave);
            this.reportSaveLoad.Location = new System.Drawing.Point(374, 19);
            this.reportSaveLoad.Name = "reportSaveLoad";
            this.reportSaveLoad.Padding = new System.Windows.Forms.Padding(8, 3, 8, 8);
            this.reportSaveLoad.Size = new System.Drawing.Size(120, 84);
            this.reportSaveLoad.TabIndex = 8;
            this.reportSaveLoad.TabStop = false;
            this.reportSaveLoad.Text = "Отчет";
            // 
            // reportIKnowAboutPrivateInfo
            // 
            this.reportIKnowAboutPrivateInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportIKnowAboutPrivateInfo.Location = new System.Drawing.Point(14, 88);
            this.reportIKnowAboutPrivateInfo.Name = "reportIKnowAboutPrivateInfo";
            this.reportIKnowAboutPrivateInfo.Size = new System.Drawing.Size(354, 21);
            this.reportIKnowAboutPrivateInfo.TabIndex = 9;
            this.reportIKnowAboutPrivateInfo.Text = "Я знаю что в отчет может попасть личная информация";
            this.reportIKnowAboutPrivateInfo.UseVisualStyleBackColor = true;
            this.reportIKnowAboutPrivateInfo.CheckedChanged += new System.EventHandler(this.reportIKnowAboutPrivateInfo_CheckedChanged);
            // 
            // reportEmail
            // 
            this.reportEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportEmail.Location = new System.Drawing.Point(11, 128);
            this.reportEmail.Name = "reportEmail";
            this.reportEmail.Size = new System.Drawing.Size(357, 20);
            this.reportEmail.TabIndex = 9;
            // 
            // reportEmailLabel
            // 
            this.reportEmailLabel.AutoSize = true;
            this.reportEmailLabel.Location = new System.Drawing.Point(11, 112);
            this.reportEmailLabel.Name = "reportEmailLabel";
            this.reportEmailLabel.Size = new System.Drawing.Size(102, 13);
            this.reportEmailLabel.TabIndex = 10;
            this.reportEmailLabel.Text = "Контактный E-mail:";
            // 
            // reportInfoLabel
            // 
            this.reportInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.reportInfoLabel.Location = new System.Drawing.Point(11, 25);
            this.reportInfoLabel.Name = "reportInfoLabel";
            this.reportInfoLabel.Size = new System.Drawing.Size(357, 37);
            this.reportInfoLabel.TabIndex = 11;
            this.reportInfoLabel.Text = "Если у вас возникли проблемы с этим сайтом вы можете отправить отчет здесь";
            // 
            // LogOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 459);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.Tabs);
            this.Controls.Add(this.ResponseList);
            this.MinimumSize = new System.Drawing.Size(630, 445);
            this.Name = "LogOutput";
            this.Text = "Лог";
            this.Tabs.ResumeLayout(false);
            this.BrowserTab.ResumeLayout(false);
            this.reportTab.ResumeLayout(false);
            this.reportSendGroup.ResumeLayout(false);
            this.reportSendGroup.PerformLayout();
            this.reportSaveLoad.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ResponseList;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage BrowserTab;
        public System.Windows.Forms.WebBrowser Browser;
        private System.Windows.Forms.TabPage HtmlTab;
        private System.Windows.Forms.TabPage VariablesTab;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TabPage ResponseTab;
        private System.Windows.Forms.TabPage reportTab;
        private System.Windows.Forms.Button reportSend;
        private System.Windows.Forms.CheckBox reportAccountInclude;
        private System.Windows.Forms.Button propProfileSave;
        private System.Windows.Forms.Button propProfileLoad;
        private System.Windows.Forms.GroupBox reportSaveLoad;
        private System.Windows.Forms.GroupBox reportSendGroup;
        private System.Windows.Forms.Label reportCommentsLabel;
        private System.Windows.Forms.TextBox reportComments;
        private System.Windows.Forms.Label reportEmailLabel;
        private System.Windows.Forms.TextBox reportEmail;
        private System.Windows.Forms.CheckBox reportIKnowAboutPrivateInfo;
        private System.Windows.Forms.Label reportInfoLabel;

    }
}