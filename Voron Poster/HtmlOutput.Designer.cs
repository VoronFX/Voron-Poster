namespace Voron_Poster
{
    partial class HtmlOutput
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
            this.VariablesTab = new System.Windows.Forms.TabPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.ResponseTab = new System.Windows.Forms.TabPage();
            this.Tabs.SuspendLayout();
            this.BrowserTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResponseList
            // 
            this.ResponseList.Dock = System.Windows.Forms.DockStyle.Left;
            this.ResponseList.Location = new System.Drawing.Point(0, 0);
            this.ResponseList.Name = "ResponseList";
            this.ResponseList.Size = new System.Drawing.Size(129, 570);
            this.ResponseList.TabIndex = 2;
            this.ResponseList.SelectedIndexChanged += new System.EventHandler(this.ResponseList_SelectedIndexChanged);
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.BrowserTab);
            this.Tabs.Controls.Add(this.HtmlTab);
            this.Tabs.Controls.Add(this.ResponseTab);
            this.Tabs.Controls.Add(this.VariablesTab);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(129, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(675, 570);
            this.Tabs.TabIndex = 3;
            // 
            // BrowserTab
            // 
            this.BrowserTab.Controls.Add(this.Browser);
            this.BrowserTab.Location = new System.Drawing.Point(4, 22);
            this.BrowserTab.Name = "BrowserTab";
            this.BrowserTab.Padding = new System.Windows.Forms.Padding(3);
            this.BrowserTab.Size = new System.Drawing.Size(771, 544);
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
            this.Browser.Size = new System.Drawing.Size(765, 538);
            this.Browser.TabIndex = 0;
            // 
            // HtmlTab
            // 
            this.HtmlTab.Location = new System.Drawing.Point(4, 22);
            this.HtmlTab.Name = "HtmlTab";
            this.HtmlTab.Padding = new System.Windows.Forms.Padding(3);
            this.HtmlTab.Size = new System.Drawing.Size(771, 544);
            this.HtmlTab.TabIndex = 1;
            this.HtmlTab.Text = "Html";
            this.HtmlTab.UseVisualStyleBackColor = true;
            // 
            // VariablesTab
            // 
            this.VariablesTab.Location = new System.Drawing.Point(4, 22);
            this.VariablesTab.Name = "VariablesTab";
            this.VariablesTab.Padding = new System.Windows.Forms.Padding(3);
            this.VariablesTab.Size = new System.Drawing.Size(771, 544);
            this.VariablesTab.TabIndex = 2;
            this.VariablesTab.Text = "Переменные";
            this.VariablesTab.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(129, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 570);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // ResponseTab
            // 
            this.ResponseTab.Location = new System.Drawing.Point(4, 22);
            this.ResponseTab.Name = "ResponseTab";
            this.ResponseTab.Padding = new System.Windows.Forms.Padding(3);
            this.ResponseTab.Size = new System.Drawing.Size(667, 544);
            this.ResponseTab.TabIndex = 3;
            this.ResponseTab.Text = "Response";
            this.ResponseTab.UseVisualStyleBackColor = true;
            // 
            // HtmlOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 570);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.Tabs);
            this.Controls.Add(this.ResponseList);
            this.Name = "HtmlOutput";
            this.Text = "HtmlOutput";
            this.Load += new System.EventHandler(this.HtmlOutput_Load);
            this.Tabs.ResumeLayout(false);
            this.BrowserTab.ResumeLayout(false);
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

    }
}