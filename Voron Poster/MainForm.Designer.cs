﻿namespace Voron_Poster
{
    partial class MainForm
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.TasksPage = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.NewUrlComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.AddTaskButton = new System.Windows.Forms.Button();
            this.NewUrlTextBox = new System.Windows.Forms.TextBox();
            this.TaskPropertiesPage = new System.Windows.Forms.TabPage();
            this.DetectEngineButton = new System.Windows.Forms.Button();
            this.ForumEngineComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ScriptMoveDownButton = new System.Windows.Forms.Button();
            this.ScriptMoveUpButton = new System.Windows.Forms.Button();
            this.ScriptAddButton = new System.Windows.Forms.Button();
            this.RemoveScriptButton = new System.Windows.Forms.Button();
            this.ScriptListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TryLoginButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.LocalAccountCheckbox = new System.Windows.Forms.RadioButton();
            this.GlobalAccountCheckbox = new System.Windows.Forms.RadioButton();
            this.ShowPassword = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BrowseProfileButton = new System.Windows.Forms.Button();
            this.DeleteProfileButton = new System.Windows.Forms.Button();
            this.NewProfileButton = new System.Windows.Forms.Button();
            this.SaveProfileButton = new System.Windows.Forms.Button();
            this.LoadProfileButton = new System.Windows.Forms.Button();
            this.ProfileComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MainPageBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TargetUrlBox = new System.Windows.Forms.TextBox();
            this.TaskPropCancel = new System.Windows.Forms.Button();
            this.TaskPropApply = new System.Windows.Forms.Button();
            this.TasksUpdater = new System.Windows.Forms.Timer(this.components);
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.TasksGuiTable = new Voron_Poster.DBTableLayoutPanel();
            this.GTName = new System.Windows.Forms.Label();
            this.GTStatus = new System.Windows.Forms.Label();
            this.GTStart = new System.Windows.Forms.Button();
            this.GTSelected = new System.Windows.Forms.CheckBox();
            this.GTProgress = new System.Windows.Forms.ProgressBar();
            this.GTStatusIcon = new System.Windows.Forms.PictureBox();
            this.GTStop = new System.Windows.Forms.Button();
            this.GTDelete = new System.Windows.Forms.Button();
            this.Tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.TasksPage.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.TaskPropertiesPage.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.TasksGuiTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GTStatusIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.tabPage1);
            this.Tabs.Controls.Add(this.tabPage4);
            this.Tabs.Controls.Add(this.TasksPage);
            this.Tabs.Controls.Add(this.TaskPropertiesPage);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(3, 3);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(681, 494);
            this.Tabs.TabIndex = 8;
            this.Tabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.Tabs_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.treeView1);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(673, 468);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(678, 468);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 15;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(215, 421);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(63, 458);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "label1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 450);
            this.progressBar1.Maximum = 3;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(254, 22);
            this.progressBar1.TabIndex = 12;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(334, 377);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(228, 217);
            this.treeView1.TabIndex = 11;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(568, 13);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(563, 369);
            this.textBox2.TabIndex = 10;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(-1, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(563, 369);
            this.textBox1.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(215, 392);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(673, 468);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Сообщение";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // TasksPage
            // 
            this.TasksPage.Controls.Add(this.groupBox4);
            this.TasksPage.Controls.Add(this.label8);
            this.TasksPage.Controls.Add(this.AddTaskButton);
            this.TasksPage.Controls.Add(this.NewUrlTextBox);
            this.TasksPage.Controls.Add(this.TasksGuiTable);
            this.TasksPage.Location = new System.Drawing.Point(4, 22);
            this.TasksPage.Name = "TasksPage";
            this.TasksPage.Padding = new System.Windows.Forms.Padding(20);
            this.TasksPage.Size = new System.Drawing.Size(673, 468);
            this.TasksPage.TabIndex = 2;
            this.TasksPage.Text = "Задачи";
            this.TasksPage.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Controls.Add(this.button5);
            this.groupBox4.Controls.Add(this.button6);
            this.groupBox4.Controls.Add(this.button7);
            this.groupBox4.Controls.Add(this.button8);
            this.groupBox4.Controls.Add(this.NewUrlComboBox);
            this.groupBox4.Location = new System.Drawing.Point(23, 23);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(10, 3, 3, 5);
            this.groupBox4.Size = new System.Drawing.Size(628, 79);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Готовые профили";
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Image = global::Voron_Poster.Properties.Resources.folder_Open_16xLG;
            this.button4.Location = new System.Drawing.Point(515, 17);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(96, 24);
            this.button4.TabIndex = 5;
            this.button4.Text = "... Обзор";
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.Image = global::Voron_Poster.Properties.Resources.action_Cancel_16xLG;
            this.button5.Location = new System.Drawing.Point(247, 47);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(97, 24);
            this.button5.TabIndex = 4;
            this.button5.Text = "Удалить";
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button6.Image = global::Voron_Poster.Properties.Resources.action_add_16xLG;
            this.button6.Location = new System.Drawing.Point(13, 47);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(125, 24);
            this.button6.TabIndex = 3;
            this.button6.Text = "Создать новый";
            this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button7.Image = global::Voron_Poster.Properties.Resources.save_16xLG;
            this.button7.Location = new System.Drawing.Point(144, 47);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(97, 24);
            this.button7.TabIndex = 2;
            this.button7.Text = "Сохранить";
            this.button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Image = global::Voron_Poster.Properties.Resources.Open_6529;
            this.button8.Location = new System.Drawing.Point(464, 47);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(147, 24);
            this.button8.TabIndex = 1;
            this.button8.Text = "Открыть (Применить)";
            this.button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button8.UseVisualStyleBackColor = true;
            // 
            // NewUrlComboBox
            // 
            this.NewUrlComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewUrlComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.NewUrlComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.NewUrlComboBox.FormattingEnabled = true;
            this.NewUrlComboBox.Location = new System.Drawing.Point(13, 19);
            this.NewUrlComboBox.Name = "NewUrlComboBox";
            this.NewUrlComboBox.Size = new System.Drawing.Size(496, 21);
            this.NewUrlComboBox.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 113);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(212, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Ссылка на тему/раздел для публикации";
            // 
            // AddTaskButton
            // 
            this.AddTaskButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTaskButton.Image = global::Voron_Poster.Properties.Resources.action_add_16xLG;
            this.AddTaskButton.Location = new System.Drawing.Point(561, 127);
            this.AddTaskButton.Name = "AddTaskButton";
            this.AddTaskButton.Size = new System.Drawing.Size(90, 24);
            this.AddTaskButton.TabIndex = 2;
            this.AddTaskButton.Text = "Добавить";
            this.AddTaskButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.AddTaskButton.UseVisualStyleBackColor = true;
            this.AddTaskButton.Click += new System.EventHandler(this.AddTaskButton_Click);
            // 
            // NewUrlTextBox
            // 
            this.NewUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewUrlTextBox.Location = new System.Drawing.Point(23, 129);
            this.NewUrlTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
            this.NewUrlTextBox.Name = "NewUrlTextBox";
            this.NewUrlTextBox.Size = new System.Drawing.Size(522, 20);
            this.NewUrlTextBox.TabIndex = 1;
            // 
            // TaskPropertiesPage
            // 
            this.TaskPropertiesPage.Controls.Add(this.DetectEngineButton);
            this.TaskPropertiesPage.Controls.Add(this.ForumEngineComboBox);
            this.TaskPropertiesPage.Controls.Add(this.label7);
            this.TaskPropertiesPage.Controls.Add(this.groupBox3);
            this.TaskPropertiesPage.Controls.Add(this.groupBox2);
            this.TaskPropertiesPage.Controls.Add(this.groupBox1);
            this.TaskPropertiesPage.Controls.Add(this.label3);
            this.TaskPropertiesPage.Controls.Add(this.MainPageBox);
            this.TaskPropertiesPage.Controls.Add(this.label2);
            this.TaskPropertiesPage.Controls.Add(this.TargetUrlBox);
            this.TaskPropertiesPage.Controls.Add(this.TaskPropCancel);
            this.TaskPropertiesPage.Controls.Add(this.TaskPropApply);
            this.TaskPropertiesPage.Location = new System.Drawing.Point(4, 22);
            this.TaskPropertiesPage.Name = "TaskPropertiesPage";
            this.TaskPropertiesPage.Padding = new System.Windows.Forms.Padding(20);
            this.TaskPropertiesPage.Size = new System.Drawing.Size(673, 468);
            this.TaskPropertiesPage.TabIndex = 4;
            this.TaskPropertiesPage.Text = "Параметры задачи";
            this.TaskPropertiesPage.UseVisualStyleBackColor = true;
            // 
            // DetectEngineButton
            // 
            this.DetectEngineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DetectEngineButton.Enabled = false;
            this.DetectEngineButton.Image = global::Voron_Poster.Properties.Resources.gear_16xLG;
            this.DetectEngineButton.Location = new System.Drawing.Point(308, 194);
            this.DetectEngineButton.Name = "DetectEngineButton";
            this.DetectEngineButton.Size = new System.Drawing.Size(99, 25);
            this.DetectEngineButton.TabIndex = 15;
            this.DetectEngineButton.Text = "Определить";
            this.DetectEngineButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.DetectEngineButton.UseVisualStyleBackColor = true;
            this.DetectEngineButton.Click += new System.EventHandler(this.DetectEngineButton_Click);
            // 
            // ForumEngineComboBox
            // 
            this.ForumEngineComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForumEngineComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ForumEngineComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ForumEngineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ForumEngineComboBox.FormattingEnabled = true;
            this.ForumEngineComboBox.Location = new System.Drawing.Point(122, 196);
            this.ForumEngineComboBox.Name = "ForumEngineComboBox";
            this.ForumEngineComboBox.Size = new System.Drawing.Size(180, 21);
            this.ForumEngineComboBox.TabIndex = 12;
            this.ForumEngineComboBox.SelectedIndexChanged += new System.EventHandler(this.ForumEngineComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Движок форума:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.ScriptMoveDownButton);
            this.groupBox3.Controls.Add(this.ScriptMoveUpButton);
            this.groupBox3.Controls.Add(this.ScriptAddButton);
            this.groupBox3.Controls.Add(this.RemoveScriptButton);
            this.groupBox3.Controls.Add(this.ScriptListBox);
            this.groupBox3.Location = new System.Drawing.Point(23, 225);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(10, 3, 10, 5);
            this.groupBox3.Size = new System.Drawing.Size(384, 220);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Скрипты предобработки";
            // 
            // ScriptMoveDownButton
            // 
            this.ScriptMoveDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScriptMoveDownButton.Image = global::Voron_Poster.Properties.Resources.arrow_Down_16xLG;
            this.ScriptMoveDownButton.Location = new System.Drawing.Point(345, 51);
            this.ScriptMoveDownButton.Name = "ScriptMoveDownButton";
            this.ScriptMoveDownButton.Size = new System.Drawing.Size(26, 26);
            this.ScriptMoveDownButton.TabIndex = 14;
            this.ScriptMoveDownButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ScriptMoveDownButton.UseVisualStyleBackColor = true;
            // 
            // ScriptMoveUpButton
            // 
            this.ScriptMoveUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScriptMoveUpButton.Image = global::Voron_Poster.Properties.Resources.arrow_Up_16xLG;
            this.ScriptMoveUpButton.Location = new System.Drawing.Point(345, 19);
            this.ScriptMoveUpButton.Name = "ScriptMoveUpButton";
            this.ScriptMoveUpButton.Size = new System.Drawing.Size(26, 26);
            this.ScriptMoveUpButton.TabIndex = 13;
            this.ScriptMoveUpButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ScriptMoveUpButton.UseVisualStyleBackColor = true;
            // 
            // ScriptAddButton
            // 
            this.ScriptAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScriptAddButton.Image = global::Voron_Poster.Properties.Resources.action_add_16xLG;
            this.ScriptAddButton.Location = new System.Drawing.Point(345, 146);
            this.ScriptAddButton.Name = "ScriptAddButton";
            this.ScriptAddButton.Size = new System.Drawing.Size(26, 26);
            this.ScriptAddButton.TabIndex = 12;
            this.ScriptAddButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ScriptAddButton.UseVisualStyleBackColor = true;
            // 
            // RemoveScriptButton
            // 
            this.RemoveScriptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveScriptButton.Image = global::Voron_Poster.Properties.Resources.Remove_16xLG;
            this.RemoveScriptButton.Location = new System.Drawing.Point(345, 177);
            this.RemoveScriptButton.Name = "RemoveScriptButton";
            this.RemoveScriptButton.Size = new System.Drawing.Size(26, 26);
            this.RemoveScriptButton.TabIndex = 11;
            this.RemoveScriptButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.RemoveScriptButton.UseVisualStyleBackColor = true;
            // 
            // ScriptListBox
            // 
            this.ScriptListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScriptListBox.Location = new System.Drawing.Point(13, 19);
            this.ScriptListBox.Name = "ScriptListBox";
            this.ScriptListBox.Size = new System.Drawing.Size(326, 160);
            this.ScriptListBox.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.TryLoginButton);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox8);
            this.groupBox2.Controls.Add(this.LocalAccountCheckbox);
            this.groupBox2.Controls.Add(this.GlobalAccountCheckbox);
            this.groupBox2.Controls.Add(this.ShowPassword);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.PasswordBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.UsernameBox);
            this.groupBox2.Location = new System.Drawing.Point(413, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 247);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Авторизация";
            // 
            // TryLoginButton
            // 
            this.TryLoginButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TryLoginButton.Enabled = false;
            this.TryLoginButton.Image = global::Voron_Poster.Properties.Resources.user_16xLG;
            this.TryLoginButton.Location = new System.Drawing.Point(11, 208);
            this.TryLoginButton.Name = "TryLoginButton";
            this.TryLoginButton.Size = new System.Drawing.Size(209, 24);
            this.TryLoginButton.TabIndex = 14;
            this.TryLoginButton.Text = "Пробная авторизация";
            this.TryLoginButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.TryLoginButton.UseVisualStyleBackColor = true;
            this.TryLoginButton.Click += new System.EventHandler(this.TryLoginButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(173, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Для авторизации использовать:";
            // 
            // textBox8
            // 
            this.textBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox8.Location = new System.Drawing.Point(11, 58);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(209, 20);
            this.textBox8.TabIndex = 12;
            // 
            // LocalAccountCheckbox
            // 
            this.LocalAccountCheckbox.AutoSize = true;
            this.LocalAccountCheckbox.Location = new System.Drawing.Point(11, 84);
            this.LocalAccountCheckbox.Name = "LocalAccountCheckbox";
            this.LocalAccountCheckbox.Size = new System.Drawing.Size(166, 17);
            this.LocalAccountCheckbox.TabIndex = 11;
            this.LocalAccountCheckbox.Text = "Отдельную учетную запись:";
            this.LocalAccountCheckbox.UseVisualStyleBackColor = true;
            this.LocalAccountCheckbox.CheckedChanged += new System.EventHandler(this.GlobalAccountCheckbox_CheckedChanged);
            // 
            // GlobalAccountCheckbox
            // 
            this.GlobalAccountCheckbox.AutoSize = true;
            this.GlobalAccountCheckbox.Checked = true;
            this.GlobalAccountCheckbox.Location = new System.Drawing.Point(11, 35);
            this.GlobalAccountCheckbox.Name = "GlobalAccountCheckbox";
            this.GlobalAccountCheckbox.Size = new System.Drawing.Size(171, 17);
            this.GlobalAccountCheckbox.TabIndex = 10;
            this.GlobalAccountCheckbox.TabStop = true;
            this.GlobalAccountCheckbox.Text = "Глобальную учетную запись:";
            this.GlobalAccountCheckbox.UseVisualStyleBackColor = true;
            this.GlobalAccountCheckbox.CheckedChanged += new System.EventHandler(this.GlobalAccountCheckbox_CheckedChanged);
            // 
            // ShowPassword
            // 
            this.ShowPassword.AutoSize = true;
            this.ShowPassword.Enabled = false;
            this.ShowPassword.Location = new System.Drawing.Point(11, 185);
            this.ShowPassword.Name = "ShowPassword";
            this.ShowPassword.Size = new System.Drawing.Size(128, 17);
            this.ShowPassword.TabIndex = 9;
            this.ShowPassword.Tag = "";
            this.ShowPassword.Text = "Показывать пароль";
            this.ShowPassword.UseVisualStyleBackColor = true;
            this.ShowPassword.CheckedChanged += new System.EventHandler(this.ShowPassword_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Пароль:";
            // 
            // PasswordBox
            // 
            this.PasswordBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PasswordBox.Enabled = false;
            this.PasswordBox.Location = new System.Drawing.Point(11, 159);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(209, 20);
            this.PasswordBox.TabIndex = 7;
            this.PasswordBox.UseSystemPasswordChar = true;
            this.PasswordBox.TextChanged += new System.EventHandler(this.PasswordBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Имя пользователя:";
            // 
            // UsernameBox
            // 
            this.UsernameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UsernameBox.Enabled = false;
            this.UsernameBox.Location = new System.Drawing.Point(11, 120);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(209, 20);
            this.UsernameBox.TabIndex = 5;
            this.UsernameBox.TextChanged += new System.EventHandler(this.UsernameBox_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.BrowseProfileButton);
            this.groupBox1.Controls.Add(this.DeleteProfileButton);
            this.groupBox1.Controls.Add(this.NewProfileButton);
            this.groupBox1.Controls.Add(this.SaveProfileButton);
            this.groupBox1.Controls.Add(this.LoadProfileButton);
            this.groupBox1.Controls.Add(this.ProfileComboBox);
            this.groupBox1.Location = new System.Drawing.Point(23, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10, 3, 3, 5);
            this.groupBox1.Size = new System.Drawing.Size(627, 79);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Готовые профили";
            // 
            // BrowseProfileButton
            // 
            this.BrowseProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseProfileButton.Image = global::Voron_Poster.Properties.Resources.folder_Open_16xLG;
            this.BrowseProfileButton.Location = new System.Drawing.Point(514, 17);
            this.BrowseProfileButton.Name = "BrowseProfileButton";
            this.BrowseProfileButton.Size = new System.Drawing.Size(96, 24);
            this.BrowseProfileButton.TabIndex = 5;
            this.BrowseProfileButton.Text = "... Обзор";
            this.BrowseProfileButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BrowseProfileButton.UseVisualStyleBackColor = true;
            // 
            // DeleteProfileButton
            // 
            this.DeleteProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteProfileButton.Image = global::Voron_Poster.Properties.Resources.action_Cancel_16xLG;
            this.DeleteProfileButton.Location = new System.Drawing.Point(247, 47);
            this.DeleteProfileButton.Name = "DeleteProfileButton";
            this.DeleteProfileButton.Size = new System.Drawing.Size(97, 24);
            this.DeleteProfileButton.TabIndex = 4;
            this.DeleteProfileButton.Text = "Удалить";
            this.DeleteProfileButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.DeleteProfileButton.UseVisualStyleBackColor = true;
            this.DeleteProfileButton.Click += new System.EventHandler(this.DeleteProfileButton_Click);
            // 
            // NewProfileButton
            // 
            this.NewProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewProfileButton.Image = global::Voron_Poster.Properties.Resources.action_add_16xLG;
            this.NewProfileButton.Location = new System.Drawing.Point(13, 47);
            this.NewProfileButton.Name = "NewProfileButton";
            this.NewProfileButton.Size = new System.Drawing.Size(125, 24);
            this.NewProfileButton.TabIndex = 3;
            this.NewProfileButton.Text = "Создать новый";
            this.NewProfileButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.NewProfileButton.UseVisualStyleBackColor = true;
            this.NewProfileButton.Click += new System.EventHandler(this.NewProfileButton_Click);
            // 
            // SaveProfileButton
            // 
            this.SaveProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveProfileButton.Image = global::Voron_Poster.Properties.Resources.save_16xLG;
            this.SaveProfileButton.Location = new System.Drawing.Point(144, 47);
            this.SaveProfileButton.Name = "SaveProfileButton";
            this.SaveProfileButton.Size = new System.Drawing.Size(97, 24);
            this.SaveProfileButton.TabIndex = 2;
            this.SaveProfileButton.Text = "Сохранить";
            this.SaveProfileButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.SaveProfileButton.UseVisualStyleBackColor = true;
            this.SaveProfileButton.Click += new System.EventHandler(this.SaveProfileButton_Click);
            // 
            // LoadProfileButton
            // 
            this.LoadProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadProfileButton.Image = global::Voron_Poster.Properties.Resources.Open_6529;
            this.LoadProfileButton.Location = new System.Drawing.Point(463, 47);
            this.LoadProfileButton.Name = "LoadProfileButton";
            this.LoadProfileButton.Size = new System.Drawing.Size(147, 24);
            this.LoadProfileButton.TabIndex = 1;
            this.LoadProfileButton.Text = "Открыть (Применить)";
            this.LoadProfileButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.LoadProfileButton.UseVisualStyleBackColor = true;
            this.LoadProfileButton.Click += new System.EventHandler(this.LoadProfileButton_Click);
            // 
            // ProfileComboBox
            // 
            this.ProfileComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProfileComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ProfileComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ProfileComboBox.FormattingEnabled = true;
            this.ProfileComboBox.Location = new System.Drawing.Point(13, 19);
            this.ProfileComboBox.Name = "ProfileComboBox";
            this.ProfileComboBox.Size = new System.Drawing.Size(495, 21);
            this.ProfileComboBox.TabIndex = 0;
            this.ProfileComboBox.TextChanged += new System.EventHandler(this.ProfileComboBox_TextChanged);
            this.ProfileComboBox.Enter += new System.EventHandler(this.ProfileComboBox_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ссылка на главную страницу форума";
            // 
            // MainPageBox
            // 
            this.MainPageBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPageBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.MainPageBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.MainPageBox.Location = new System.Drawing.Point(23, 170);
            this.MainPageBox.Name = "MainPageBox";
            this.MainPageBox.Size = new System.Drawing.Size(384, 20);
            this.MainPageBox.TabIndex = 3;
            this.MainPageBox.Text = "http://";
            this.MainPageBox.TextChanged += new System.EventHandler(this.UrlBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ссылка на тему/раздел для публикации";
            // 
            // TargetUrlBox
            // 
            this.TargetUrlBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetUrlBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.TargetUrlBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.TargetUrlBox.Location = new System.Drawing.Point(23, 36);
            this.TargetUrlBox.Name = "TargetUrlBox";
            this.TargetUrlBox.Size = new System.Drawing.Size(627, 20);
            this.TargetUrlBox.TabIndex = 0;
            this.TargetUrlBox.Text = "http://";
            this.TargetUrlBox.TextChanged += new System.EventHandler(this.UrlBox_TextChanged);
            // 
            // TaskPropCancel
            // 
            this.TaskPropCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskPropCancel.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Critical_32xMD_color;
            this.TaskPropCancel.Location = new System.Drawing.Point(534, 407);
            this.TaskPropCancel.Name = "TaskPropCancel";
            this.TaskPropCancel.Size = new System.Drawing.Size(116, 38);
            this.TaskPropCancel.TabIndex = 7;
            this.TaskPropCancel.Text = "Отмена";
            this.TaskPropCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.TaskPropCancel.UseVisualStyleBackColor = true;
            this.TaskPropCancel.Click += new System.EventHandler(this.ClosePropertiesPage);
            // 
            // TaskPropApply
            // 
            this.TaskPropApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskPropApply.Enabled = false;
            this.TaskPropApply.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Complete_and_ok_32xMD_color;
            this.TaskPropApply.Location = new System.Drawing.Point(413, 407);
            this.TaskPropApply.Name = "TaskPropApply";
            this.TaskPropApply.Size = new System.Drawing.Size(115, 38);
            this.TaskPropApply.TabIndex = 6;
            this.TaskPropApply.Text = "Применить";
            this.TaskPropApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.TaskPropApply.UseVisualStyleBackColor = true;
            this.TaskPropApply.Click += new System.EventHandler(this.TaskPropApply_Click);
            // 
            // TasksUpdater
            // 
            this.TasksUpdater.Tick += new System.EventHandler(this.TasksUpdater_Tick);
            // 
            // ToolTip
            // 
            this.ToolTip.AutoPopDelay = 5000;
            this.ToolTip.InitialDelay = 250;
            this.ToolTip.ReshowDelay = 100;
            // 
            // TasksGuiTable
            // 
            this.TasksGuiTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TasksGuiTable.AutoScroll = true;
            this.TasksGuiTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TasksGuiTable.BackColor = System.Drawing.Color.White;
            this.TasksGuiTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.TasksGuiTable.ColumnCount = 8;
            this.TasksGuiTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.TasksGuiTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TasksGuiTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.TasksGuiTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.TasksGuiTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.TasksGuiTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.TasksGuiTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.TasksGuiTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TasksGuiTable.Controls.Add(this.GTName, 0, 0);
            this.TasksGuiTable.Controls.Add(this.GTStatus, 2, 0);
            this.TasksGuiTable.Controls.Add(this.GTStart, 5, 0);
            this.TasksGuiTable.Controls.Add(this.GTSelected, 0, 0);
            this.TasksGuiTable.Controls.Add(this.GTProgress, 4, 0);
            this.TasksGuiTable.Controls.Add(this.GTStatusIcon, 3, 0);
            this.TasksGuiTable.Controls.Add(this.GTStop, 6, 0);
            this.TasksGuiTable.Controls.Add(this.GTDelete, 7, 0);
            this.TasksGuiTable.Location = new System.Drawing.Point(23, 175);
            this.TasksGuiTable.Margin = new System.Windows.Forms.Padding(3, 23, 3, 3);
            this.TasksGuiTable.Name = "TasksGuiTable";
            this.TasksGuiTable.RowCount = 2;
            this.TasksGuiTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.TasksGuiTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TasksGuiTable.Size = new System.Drawing.Size(627, 270);
            this.TasksGuiTable.TabIndex = 0;
            this.TasksGuiTable.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TasksGuiTable_CellPaint);
            // 
            // GTName
            // 
            this.GTName.BackColor = System.Drawing.SystemColors.Control;
            this.GTName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTName.Location = new System.Drawing.Point(26, 1);
            this.GTName.Margin = new System.Windows.Forms.Padding(0);
            this.GTName.MaximumSize = new System.Drawing.Size(0, 24);
            this.GTName.MinimumSize = new System.Drawing.Size(0, 24);
            this.GTName.Name = "GTName";
            this.GTName.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.GTName.Size = new System.Drawing.Size(308, 24);
            this.GTName.TabIndex = 13;
            this.GTName.Text = "Тема/Раздел";
            this.GTName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GTStatus
            // 
            this.GTStatus.BackColor = System.Drawing.SystemColors.Control;
            this.GTStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTStatus.Location = new System.Drawing.Point(335, 1);
            this.GTStatus.Margin = new System.Windows.Forms.Padding(0);
            this.GTStatus.MaximumSize = new System.Drawing.Size(0, 24);
            this.GTStatus.MinimumSize = new System.Drawing.Size(0, 24);
            this.GTStatus.Name = "GTStatus";
            this.GTStatus.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.GTStatus.Size = new System.Drawing.Size(120, 24);
            this.GTStatus.TabIndex = 12;
            this.GTStatus.Text = "Состояние";
            this.GTStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GTStart
            // 
            this.GTStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTStart.Enabled = false;
            this.GTStart.Image = global::Voron_Poster.Properties.Resources.arrow_run_16xLG;
            this.GTStart.Location = new System.Drawing.Point(552, 1);
            this.GTStart.Margin = new System.Windows.Forms.Padding(0);
            this.GTStart.MaximumSize = new System.Drawing.Size(24, 24);
            this.GTStart.MinimumSize = new System.Drawing.Size(24, 24);
            this.GTStart.Name = "GTStart";
            this.GTStart.Size = new System.Drawing.Size(24, 24);
            this.GTStart.TabIndex = 8;
            this.GTStart.UseVisualStyleBackColor = true;
            this.GTStart.Click += new System.EventHandler(this.GTStartStop_Click);
            // 
            // GTSelected
            // 
            this.GTSelected.BackColor = System.Drawing.SystemColors.Control;
            this.GTSelected.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.GTSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.GTSelected.Location = new System.Drawing.Point(1, 1);
            this.GTSelected.Margin = new System.Windows.Forms.Padding(0);
            this.GTSelected.MaximumSize = new System.Drawing.Size(24, 24);
            this.GTSelected.MinimumSize = new System.Drawing.Size(24, 24);
            this.GTSelected.Name = "GTSelected";
            this.GTSelected.Size = new System.Drawing.Size(24, 24);
            this.GTSelected.TabIndex = 0;
            this.GTSelected.ThreeState = true;
            this.GTSelected.UseVisualStyleBackColor = false;
            this.GTSelected.Click += new System.EventHandler(this.GTSelected_Click);
            // 
            // GTProgress
            // 
            this.GTProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTProgress.Location = new System.Drawing.Point(484, 4);
            this.GTProgress.MaximumSize = new System.Drawing.Size(0, 18);
            this.GTProgress.MinimumSize = new System.Drawing.Size(0, 18);
            this.GTProgress.Name = "GTProgress";
            this.GTProgress.Size = new System.Drawing.Size(64, 18);
            this.GTProgress.TabIndex = 2;
            // 
            // GTStatusIcon
            // 
            this.GTStatusIcon.BackColor = System.Drawing.SystemColors.Control;
            this.GTStatusIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTStatusIcon.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG;
            this.GTStatusIcon.Location = new System.Drawing.Point(456, 1);
            this.GTStatusIcon.Margin = new System.Windows.Forms.Padding(0);
            this.GTStatusIcon.MaximumSize = new System.Drawing.Size(24, 24);
            this.GTStatusIcon.MinimumSize = new System.Drawing.Size(24, 24);
            this.GTStatusIcon.Name = "GTStatusIcon";
            this.GTStatusIcon.Size = new System.Drawing.Size(24, 24);
            this.GTStatusIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.GTStatusIcon.TabIndex = 9;
            this.GTStatusIcon.TabStop = false;
            // 
            // GTStop
            // 
            this.GTStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTStop.Enabled = false;
            this.GTStop.Image = global::Voron_Poster.Properties.Resources.Symbols_Stop_16xLG;
            this.GTStop.Location = new System.Drawing.Point(577, 1);
            this.GTStop.Margin = new System.Windows.Forms.Padding(0);
            this.GTStop.MaximumSize = new System.Drawing.Size(24, 24);
            this.GTStop.MinimumSize = new System.Drawing.Size(24, 24);
            this.GTStop.Name = "GTStop";
            this.GTStop.Size = new System.Drawing.Size(24, 24);
            this.GTStop.TabIndex = 7;
            this.GTStop.UseVisualStyleBackColor = true;
            this.GTStop.Click += new System.EventHandler(this.GTStartStop_Click);
            // 
            // GTDelete
            // 
            this.GTDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTDelete.Enabled = false;
            this.GTDelete.Image = global::Voron_Poster.Properties.Resources.action_Cancel_16xLG;
            this.GTDelete.Location = new System.Drawing.Point(602, 1);
            this.GTDelete.Margin = new System.Windows.Forms.Padding(0);
            this.GTDelete.MaximumSize = new System.Drawing.Size(24, 24);
            this.GTDelete.MinimumSize = new System.Drawing.Size(24, 24);
            this.GTDelete.Name = "GTDelete";
            this.GTDelete.Size = new System.Drawing.Size(24, 24);
            this.GTDelete.TabIndex = 10;
            this.GTDelete.UseVisualStyleBackColor = true;
            this.GTDelete.Click += new System.EventHandler(this.GTDelete_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 500);
            this.Controls.Add(this.Tabs);
            this.MinimumSize = new System.Drawing.Size(590, 539);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Voron Poster";
            this.Tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.TasksPage.ResumeLayout(false);
            this.TasksPage.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.TaskPropertiesPage.ResumeLayout(false);
            this.TaskPropertiesPage.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.TasksGuiTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GTStatusIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage TasksPage;

        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage TaskPropertiesPage;
        private System.Windows.Forms.CheckBox GTSelected;
        private System.Windows.Forms.PictureBox GTStatusIcon;
        private System.Windows.Forms.Button GTDelete;
        private System.Windows.Forms.TextBox NewUrlTextBox;
        private System.Windows.Forms.Button AddTaskButton;
        private System.Windows.Forms.Label GTStatus;
        private System.Windows.Forms.Label GTName;
        private System.Windows.Forms.TextBox TargetUrlBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button NewProfileButton;
        private System.Windows.Forms.Button SaveProfileButton;
        private System.Windows.Forms.Button LoadProfileButton;
        private System.Windows.Forms.ComboBox ProfileComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MainPageBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button TaskPropCancel;
        private System.Windows.Forms.Button TaskPropApply;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button ScriptAddButton;
        private System.Windows.Forms.Button RemoveScriptButton;
        private System.Windows.Forms.ListBox ScriptListBox;
        private System.Windows.Forms.Button DeleteProfileButton;
        private System.Windows.Forms.Button ScriptMoveDownButton;
        private System.Windows.Forms.Button ScriptMoveUpButton;
        private System.Windows.Forms.Button BrowseProfileButton;
        private System.Windows.Forms.RadioButton LocalAccountCheckbox;
        private System.Windows.Forms.RadioButton GlobalAccountCheckbox;
        public System.Windows.Forms.CheckBox ShowPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Button TryLoginButton;
        private System.Windows.Forms.Button DetectEngineButton;
        private System.Windows.Forms.ComboBox ForumEngineComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button GTStart;
        private System.Windows.Forms.ProgressBar GTProgress;
        private System.Windows.Forms.Button GTStop;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ComboBox NewUrlComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer TasksUpdater;
        public DBTableLayoutPanel TasksGuiTable;
        public System.Windows.Forms.ToolTip ToolTip;

    }
}

