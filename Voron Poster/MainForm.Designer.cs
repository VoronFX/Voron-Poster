namespace Voron_Poster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Tabs = new System.Windows.Forms.TabControl();
            this.messageTab = new System.Windows.Forms.TabPage();
            this.messageTextLabel = new System.Windows.Forms.Label();
            this.messageSubjectLabel = new System.Windows.Forms.Label();
            this.messageNext = new System.Windows.Forms.Button();
            this.messagePreview = new System.Windows.Forms.Button();
            this.messageText = new System.Windows.Forms.TextBox();
            this.messageSubject = new System.Windows.Forms.TextBox();
            this.previewTab = new System.Windows.Forms.TabPage();
            this.previewPanel = new System.Windows.Forms.Panel();
            this.previewWBPanel = new System.Windows.Forms.Panel();
            this.previewWB = new System.Windows.Forms.WebBrowser();
            this.previewNext = new System.Windows.Forms.Button();
            this.previewDockUndock = new System.Windows.Forms.Button();
            this.tasksTab = new System.Windows.Forms.TabPage();
            this.tasksGroup = new System.Windows.Forms.GroupBox();
            this.tasksSave = new System.Windows.Forms.Button();
            this.tasksLoad = new System.Windows.Forms.Button();
            this.tasksAdd = new System.Windows.Forms.Button();
            this.tasksUrl = new System.Windows.Forms.TextBox();
            this.tasksUrlLabel = new System.Windows.Forms.Label();
            this.propTab = new System.Windows.Forms.TabPage();
            this.propEngineDetect = new System.Windows.Forms.Button();
            this.propEngine = new System.Windows.Forms.ComboBox();
            this.propEngineLabel = new System.Windows.Forms.Label();
            this.propScriptsGroup = new System.Windows.Forms.GroupBox();
            this.propScriptsEdit = new System.Windows.Forms.Button();
            this.propScriptsDown = new System.Windows.Forms.Button();
            this.propScriptsUp = new System.Windows.Forms.Button();
            this.propScriptsAdd = new System.Windows.Forms.Button();
            this.propScriptsRemove = new System.Windows.Forms.Button();
            this.propScriptsList = new System.Windows.Forms.ListBox();
            this.propAuthGroup = new System.Windows.Forms.GroupBox();
            this.propAuthLog = new System.Windows.Forms.Button();
            this.propAuthTryLogin = new System.Windows.Forms.Button();
            this.propAuthGlobalLabel = new System.Windows.Forms.Label();
            this.propAuthGlobalUsername = new System.Windows.Forms.TextBox();
            this.propAuthLocal = new System.Windows.Forms.RadioButton();
            this.propAuthGlobal = new System.Windows.Forms.RadioButton();
            this.propAuthShowPassword = new System.Windows.Forms.CheckBox();
            this.propPasswordLabel = new System.Windows.Forms.Label();
            this.propPassword = new System.Windows.Forms.TextBox();
            this.propUsernameLabel = new System.Windows.Forms.Label();
            this.propUsername = new System.Windows.Forms.TextBox();
            this.propProfilesGroup = new System.Windows.Forms.GroupBox();
            this.propProfileBrowse = new System.Windows.Forms.Button();
            this.propProfileDelete = new System.Windows.Forms.Button();
            this.propProfileNew = new System.Windows.Forms.Button();
            this.propProfileSave = new System.Windows.Forms.Button();
            this.propProfileLoad = new System.Windows.Forms.Button();
            this.propProfiles = new System.Windows.Forms.ComboBox();
            this.propMainUrlLabel = new System.Windows.Forms.Label();
            this.propMainUrl = new System.Windows.Forms.TextBox();
            this.propTargetLabel = new System.Windows.Forms.Label();
            this.propTargetUrl = new System.Windows.Forms.TextBox();
            this.propApply = new System.Windows.Forms.Button();
            this.propCancel = new System.Windows.Forms.Button();
            this.scriptsTab = new System.Windows.Forms.TabPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.scriptsTabs = new System.Windows.Forms.TabControl();
            this.scriptsCodeTab = new System.Windows.Forms.TabPage();
            this.scriptsDelete = new System.Windows.Forms.Button();
            this.scriptsNew = new System.Windows.Forms.Button();
            this.scriptsSave = new System.Windows.Forms.Button();
            this.scriptsCodeBox = new System.Windows.Forms.TextBox();
            this.scriptsTestTab = new System.Windows.Forms.TabPage();
            this.scriptsResult = new System.Windows.Forms.TextBox();
            this.scriptsRunPanel = new System.Windows.Forms.Panel();
            this.scriptsStatusLabel = new System.Windows.Forms.Label();
            this.scriptsRun = new System.Windows.Forms.Button();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.scriptsMessage = new System.Windows.Forms.TextBox();
            this.scriptsSpacePanel = new System.Windows.Forms.Panel();
            this.scriptsSubject = new System.Windows.Forms.TextBox();
            this.scriptsListPanel = new System.Windows.Forms.Panel();
            this.scriptsAccept = new System.Windows.Forms.Button();
            this.scriptsCancel = new System.Windows.Forms.Button();
            this.scriptsName = new System.Windows.Forms.TextBox();
            this.scriptsList = new System.Windows.Forms.ListBox();
            this.settingsTab = new System.Windows.Forms.TabPage();
            this.settingsApplySuggestedProfile = new System.Windows.Forms.CheckBox();
            this.settingsLoadLastTasklist = new System.Windows.Forms.CheckBox();
            this.settingsGAuthGroup = new System.Windows.Forms.GroupBox();
            this.settingsGAuthShowPassword = new System.Windows.Forms.CheckBox();
            this.settingsGAuthPasswordLabel = new System.Windows.Forms.Label();
            this.settingsGAuthPassword = new System.Windows.Forms.TextBox();
            this.settingsGAuthUsenameLabel = new System.Windows.Forms.Label();
            this.settingsGAuthUsername = new System.Windows.Forms.TextBox();
            this.settingsSave = new System.Windows.Forms.Button();
            this.settingsCancel = new System.Windows.Forms.Button();
            this.aboutTab = new System.Windows.Forms.TabPage();
            this.aboutLogo = new System.Windows.Forms.PictureBox();
            this.aboutAuthorGroup = new System.Windows.Forms.GroupBox();
            this.aboutAuthorSkype = new System.Windows.Forms.LinkLabel();
            this.aboutAurhorVK = new System.Windows.Forms.LinkLabel();
            this.aboutAuthorEmail = new System.Windows.Forms.LinkLabel();
            this.aboutAuthorName = new System.Windows.Forms.Label();
            this.aboutAuthorAvatar = new System.Windows.Forms.PictureBox();
            this.aboutLicenseLabel = new System.Windows.Forms.Label();
            this.aboutAboutInfo = new System.Windows.Forms.Label();
            this.aboutProgramName = new System.Windows.Forms.Label();
            this.aboutLicenseBox = new System.Windows.Forms.TextBox();
            this.aboutLicenseList = new System.Windows.Forms.ListBox();
            this.aboutSupportLabel = new System.Windows.Forms.Label();
            this.TasksUpdater = new System.Windows.Forms.Timer(this.components);
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.scriptsTestAbortTimer = new System.Windows.Forms.Timer(this.components);
            this.tasksTable = new Voron_Poster.DBTableLayoutPanel();
            this.GTName = new System.Windows.Forms.Label();
            this.GTStatus = new System.Windows.Forms.Label();
            this.GTStart = new System.Windows.Forms.Button();
            this.GTSelected = new System.Windows.Forms.CheckBox();
            this.GTProgress = new System.Windows.Forms.ProgressBar();
            this.GTStatusIcon = new System.Windows.Forms.PictureBox();
            this.GTStop = new System.Windows.Forms.Button();
            this.GTDelete = new System.Windows.Forms.Button();
            this.Tabs.SuspendLayout();
            this.messageTab.SuspendLayout();
            this.previewTab.SuspendLayout();
            this.previewPanel.SuspendLayout();
            this.previewWBPanel.SuspendLayout();
            this.tasksTab.SuspendLayout();
            this.tasksGroup.SuspendLayout();
            this.propTab.SuspendLayout();
            this.propScriptsGroup.SuspendLayout();
            this.propAuthGroup.SuspendLayout();
            this.propProfilesGroup.SuspendLayout();
            this.scriptsTab.SuspendLayout();
            this.scriptsTabs.SuspendLayout();
            this.scriptsCodeTab.SuspendLayout();
            this.scriptsTestTab.SuspendLayout();
            this.scriptsRunPanel.SuspendLayout();
            this.scriptsListPanel.SuspendLayout();
            this.settingsTab.SuspendLayout();
            this.settingsGAuthGroup.SuspendLayout();
            this.aboutTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aboutLogo)).BeginInit();
            this.aboutAuthorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aboutAuthorAvatar)).BeginInit();
            this.tasksTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GTStatusIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.messageTab);
            this.Tabs.Controls.Add(this.previewTab);
            this.Tabs.Controls.Add(this.tasksTab);
            this.Tabs.Controls.Add(this.propTab);
            this.Tabs.Controls.Add(this.scriptsTab);
            this.Tabs.Controls.Add(this.settingsTab);
            this.Tabs.Controls.Add(this.aboutTab);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(3, 3);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(870, 647);
            this.Tabs.TabIndex = 8;
            this.Tabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.Tabs_Selecting);
            // 
            // messageTab
            // 
            this.messageTab.Controls.Add(this.messageTextLabel);
            this.messageTab.Controls.Add(this.messageSubjectLabel);
            this.messageTab.Controls.Add(this.messageNext);
            this.messageTab.Controls.Add(this.messagePreview);
            this.messageTab.Controls.Add(this.messageText);
            this.messageTab.Controls.Add(this.messageSubject);
            this.messageTab.Location = new System.Drawing.Point(4, 22);
            this.messageTab.Name = "messageTab";
            this.messageTab.Padding = new System.Windows.Forms.Padding(20);
            this.messageTab.Size = new System.Drawing.Size(862, 621);
            this.messageTab.TabIndex = 3;
            this.messageTab.Text = "Сообщение";
            this.messageTab.UseVisualStyleBackColor = true;
            // 
            // messageTextLabel
            // 
            this.messageTextLabel.AutoSize = true;
            this.messageTextLabel.Location = new System.Drawing.Point(23, 76);
            this.messageTextLabel.Name = "messageTextLabel";
            this.messageTextLabel.Size = new System.Drawing.Size(100, 13);
            this.messageTextLabel.TabIndex = 49;
            this.messageTextLabel.Text = "Текст сообщения:";
            // 
            // messageSubjectLabel
            // 
            this.messageSubjectLabel.AutoSize = true;
            this.messageSubjectLabel.Location = new System.Drawing.Point(23, 37);
            this.messageSubjectLabel.Name = "messageSubjectLabel";
            this.messageSubjectLabel.Size = new System.Drawing.Size(97, 13);
            this.messageSubjectLabel.TabIndex = 48;
            this.messageSubjectLabel.Text = "Тема сообщения:";
            // 
            // messageNext
            // 
            this.messageNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.messageNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.messageNext.Image = global::Voron_Poster.Properties.Resources.arrow_run_16xLG;
            this.messageNext.Location = new System.Drawing.Point(757, 23);
            this.messageNext.Name = "messageNext";
            this.messageNext.Size = new System.Drawing.Size(82, 24);
            this.messageNext.TabIndex = 47;
            this.messageNext.Text = "Далее";
            this.messageNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.messageNext.UseVisualStyleBackColor = true;
            this.messageNext.Click += new System.EventHandler(this.messageNext_Click);
            // 
            // messagePreview
            // 
            this.messagePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.messagePreview.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.messagePreview.Image = ((System.Drawing.Image)(resources.GetObject("messagePreview.Image")));
            this.messagePreview.Location = new System.Drawing.Point(624, 23);
            this.messagePreview.Name = "messagePreview";
            this.messagePreview.Size = new System.Drawing.Size(127, 24);
            this.messagePreview.TabIndex = 46;
            this.messagePreview.Text = "Предв. просмотр";
            this.messagePreview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.messagePreview.UseVisualStyleBackColor = true;
            this.messagePreview.Click += new System.EventHandler(this.messagePreview_Click);
            // 
            // messageText
            // 
            this.messageText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.messageText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.messageText.Location = new System.Drawing.Point(23, 92);
            this.messageText.Multiline = true;
            this.messageText.Name = "messageText";
            this.messageText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.messageText.Size = new System.Drawing.Size(816, 506);
            this.messageText.TabIndex = 1;
            this.messageText.WordWrap = false;
            this.messageText.TextChanged += new System.EventHandler(this.messageText_TextChanged);
            this.messageText.Enter += new System.EventHandler(this.messageText_Enter);
            // 
            // messageSubject
            // 
            this.messageSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.messageSubject.ForeColor = System.Drawing.SystemColors.ControlText;
            this.messageSubject.Location = new System.Drawing.Point(23, 53);
            this.messageSubject.Name = "messageSubject";
            this.messageSubject.Size = new System.Drawing.Size(816, 20);
            this.messageSubject.TabIndex = 0;
            this.messageSubject.Enter += new System.EventHandler(this.messageText_Enter);
            // 
            // previewTab
            // 
            this.previewTab.Controls.Add(this.previewPanel);
            this.previewTab.Location = new System.Drawing.Point(4, 22);
            this.previewTab.Name = "previewTab";
            this.previewTab.Padding = new System.Windows.Forms.Padding(20);
            this.previewTab.Size = new System.Drawing.Size(862, 621);
            this.previewTab.TabIndex = 8;
            this.previewTab.Text = "Предв. просмотр";
            this.previewTab.UseVisualStyleBackColor = true;
            this.previewTab.Enter += new System.EventHandler(this.previewTab_Enter);
            // 
            // previewPanel
            // 
            this.previewPanel.Controls.Add(this.previewWBPanel);
            this.previewPanel.Controls.Add(this.previewNext);
            this.previewPanel.Controls.Add(this.previewDockUndock);
            this.previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPanel.Location = new System.Drawing.Point(20, 20);
            this.previewPanel.Name = "previewPanel";
            this.previewPanel.Size = new System.Drawing.Size(822, 581);
            this.previewPanel.TabIndex = 0;
            // 
            // previewWBPanel
            // 
            this.previewWBPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewWBPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewWBPanel.Controls.Add(this.previewWB);
            this.previewWBPanel.Location = new System.Drawing.Point(4, 30);
            this.previewWBPanel.Margin = new System.Windows.Forms.Padding(4);
            this.previewWBPanel.Name = "previewWBPanel";
            this.previewWBPanel.Size = new System.Drawing.Size(814, 547);
            this.previewWBPanel.TabIndex = 0;
            // 
            // previewWB
            // 
            this.previewWB.AllowNavigation = false;
            this.previewWB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewWB.Location = new System.Drawing.Point(0, 0);
            this.previewWB.MinimumSize = new System.Drawing.Size(20, 20);
            this.previewWB.Name = "previewWB";
            this.previewWB.ScriptErrorsSuppressed = true;
            this.previewWB.Size = new System.Drawing.Size(812, 545);
            this.previewWB.TabIndex = 0;
            // 
            // previewNext
            // 
            this.previewNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.previewNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.previewNext.Image = global::Voron_Poster.Properties.Resources.arrow_run_16xLG;
            this.previewNext.Location = new System.Drawing.Point(737, 3);
            this.previewNext.Name = "previewNext";
            this.previewNext.Size = new System.Drawing.Size(82, 24);
            this.previewNext.TabIndex = 48;
            this.previewNext.Text = "Далее";
            this.previewNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.previewNext.UseVisualStyleBackColor = true;
            this.previewNext.Click += new System.EventHandler(this.previewNext_Click);
            // 
            // previewDockUndock
            // 
            this.previewDockUndock.Image = global::Voron_Poster.Properties.Resources.frame_16xLG;
            this.previewDockUndock.Location = new System.Drawing.Point(3, 3);
            this.previewDockUndock.Name = "previewDockUndock";
            this.previewDockUndock.Size = new System.Drawing.Size(140, 24);
            this.previewDockUndock.TabIndex = 0;
            this.previewDockUndock.Text = "В отдельном окне";
            this.previewDockUndock.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.previewDockUndock.UseVisualStyleBackColor = true;
            this.previewDockUndock.Click += new System.EventHandler(this.previewDockUndock_Click);
            // 
            // tasksTab
            // 
            this.tasksTab.Controls.Add(this.tasksGroup);
            this.tasksTab.Controls.Add(this.tasksTable);
            this.tasksTab.Location = new System.Drawing.Point(4, 22);
            this.tasksTab.Name = "tasksTab";
            this.tasksTab.Padding = new System.Windows.Forms.Padding(20, 15, 20, 20);
            this.tasksTab.Size = new System.Drawing.Size(862, 621);
            this.tasksTab.TabIndex = 2;
            this.tasksTab.Text = "Публикация";
            this.tasksTab.UseVisualStyleBackColor = true;
            // 
            // tasksGroup
            // 
            this.tasksGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksGroup.Controls.Add(this.tasksSave);
            this.tasksGroup.Controls.Add(this.tasksLoad);
            this.tasksGroup.Controls.Add(this.tasksAdd);
            this.tasksGroup.Controls.Add(this.tasksUrl);
            this.tasksGroup.Controls.Add(this.tasksUrlLabel);
            this.tasksGroup.Location = new System.Drawing.Point(23, 18);
            this.tasksGroup.Name = "tasksGroup";
            this.tasksGroup.Padding = new System.Windows.Forms.Padding(3, 0, 4, 3);
            this.tasksGroup.Size = new System.Drawing.Size(816, 71);
            this.tasksGroup.TabIndex = 12;
            this.tasksGroup.TabStop = false;
            this.tasksGroup.Text = "Список тем";
            // 
            // tasksSave
            // 
            this.tasksSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksSave.Image = global::Voron_Poster.Properties.Resources.save_16xLG;
            this.tasksSave.Location = new System.Drawing.Point(633, 13);
            this.tasksSave.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tasksSave.Name = "tasksSave";
            this.tasksSave.Size = new System.Drawing.Size(86, 24);
            this.tasksSave.TabIndex = 11;
            this.tasksSave.Text = "Сохранить";
            this.tasksSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.tasksSave.UseVisualStyleBackColor = true;
            this.tasksSave.Click += new System.EventHandler(this.tasksSave_Click);
            // 
            // tasksLoad
            // 
            this.tasksLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksLoad.Image = global::Voron_Poster.Properties.Resources.Open_6529;
            this.tasksLoad.Location = new System.Drawing.Point(723, 13);
            this.tasksLoad.Margin = new System.Windows.Forms.Padding(10, 0, 1, 0);
            this.tasksLoad.Name = "tasksLoad";
            this.tasksLoad.Size = new System.Drawing.Size(86, 24);
            this.tasksLoad.TabIndex = 11;
            this.tasksLoad.Text = "...Открыть";
            this.tasksLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.tasksLoad.UseVisualStyleBackColor = true;
            this.tasksLoad.Click += new System.EventHandler(this.tasksLoad_Click);
            // 
            // tasksAdd
            // 
            this.tasksAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksAdd.Image = global::Voron_Poster.Properties.Resources.action_add_16xLG;
            this.tasksAdd.Location = new System.Drawing.Point(723, 40);
            this.tasksAdd.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.tasksAdd.Name = "tasksAdd";
            this.tasksAdd.Size = new System.Drawing.Size(86, 24);
            this.tasksAdd.TabIndex = 2;
            this.tasksAdd.Text = "Добавить";
            this.tasksAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.tasksAdd.UseVisualStyleBackColor = true;
            this.tasksAdd.Click += new System.EventHandler(this.AddTaskButton_Click);
            // 
            // tasksUrl
            // 
            this.tasksUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksUrl.Location = new System.Drawing.Point(10, 42);
            this.tasksUrl.Margin = new System.Windows.Forms.Padding(13, 3, 13, 3);
            this.tasksUrl.Name = "tasksUrl";
            this.tasksUrl.Size = new System.Drawing.Size(708, 20);
            this.tasksUrl.TabIndex = 1;
            // 
            // tasksUrlLabel
            // 
            this.tasksUrlLabel.AutoSize = true;
            this.tasksUrlLabel.Location = new System.Drawing.Point(10, 26);
            this.tasksUrlLabel.Name = "tasksUrlLabel";
            this.tasksUrlLabel.Size = new System.Drawing.Size(212, 13);
            this.tasksUrlLabel.TabIndex = 7;
            this.tasksUrlLabel.Text = "Ссылка на тему/раздел для публикации";
            // 
            // propTab
            // 
            this.propTab.Controls.Add(this.propEngineDetect);
            this.propTab.Controls.Add(this.propEngine);
            this.propTab.Controls.Add(this.propEngineLabel);
            this.propTab.Controls.Add(this.propScriptsGroup);
            this.propTab.Controls.Add(this.propAuthGroup);
            this.propTab.Controls.Add(this.propProfilesGroup);
            this.propTab.Controls.Add(this.propMainUrlLabel);
            this.propTab.Controls.Add(this.propMainUrl);
            this.propTab.Controls.Add(this.propTargetLabel);
            this.propTab.Controls.Add(this.propTargetUrl);
            this.propTab.Controls.Add(this.propApply);
            this.propTab.Controls.Add(this.propCancel);
            this.propTab.Location = new System.Drawing.Point(4, 22);
            this.propTab.Name = "propTab";
            this.propTab.Padding = new System.Windows.Forms.Padding(20, 30, 20, 20);
            this.propTab.Size = new System.Drawing.Size(862, 621);
            this.propTab.TabIndex = 4;
            this.propTab.Text = "Параметры задачи";
            this.propTab.UseVisualStyleBackColor = true;
            // 
            // propEngineDetect
            // 
            this.propEngineDetect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propEngineDetect.Enabled = false;
            this.propEngineDetect.Image = global::Voron_Poster.Properties.Resources.gear_16xLG;
            this.propEngineDetect.Location = new System.Drawing.Point(497, 191);
            this.propEngineDetect.Name = "propEngineDetect";
            this.propEngineDetect.Size = new System.Drawing.Size(99, 25);
            this.propEngineDetect.TabIndex = 8;
            this.propEngineDetect.Text = "Определить";
            this.propEngineDetect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propEngineDetect.UseVisualStyleBackColor = true;
            this.propEngineDetect.Click += new System.EventHandler(this.propEngineDetect_Click);
            // 
            // propEngine
            // 
            this.propEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propEngine.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.propEngine.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.propEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.propEngine.FormattingEnabled = true;
            this.propEngine.Location = new System.Drawing.Point(122, 193);
            this.propEngine.Name = "propEngine";
            this.propEngine.Size = new System.Drawing.Size(369, 21);
            this.propEngine.TabIndex = 7;
            this.propEngine.SelectedIndexChanged += new System.EventHandler(this.propEngine_SelectedIndexChanged);
            // 
            // propEngineLabel
            // 
            this.propEngineLabel.AutoSize = true;
            this.propEngineLabel.Location = new System.Drawing.Point(23, 198);
            this.propEngineLabel.Name = "propEngineLabel";
            this.propEngineLabel.Size = new System.Drawing.Size(93, 13);
            this.propEngineLabel.TabIndex = 6;
            this.propEngineLabel.Text = "Движок форума:";
            // 
            // propScriptsGroup
            // 
            this.propScriptsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propScriptsGroup.Controls.Add(this.propScriptsEdit);
            this.propScriptsGroup.Controls.Add(this.propScriptsDown);
            this.propScriptsGroup.Controls.Add(this.propScriptsUp);
            this.propScriptsGroup.Controls.Add(this.propScriptsAdd);
            this.propScriptsGroup.Controls.Add(this.propScriptsRemove);
            this.propScriptsGroup.Controls.Add(this.propScriptsList);
            this.propScriptsGroup.Location = new System.Drawing.Point(23, 222);
            this.propScriptsGroup.Name = "propScriptsGroup";
            this.propScriptsGroup.Padding = new System.Windows.Forms.Padding(10, 3, 10, 5);
            this.propScriptsGroup.Size = new System.Drawing.Size(573, 376);
            this.propScriptsGroup.TabIndex = 9;
            this.propScriptsGroup.TabStop = false;
            this.propScriptsGroup.Text = "Скрипты предобработки";
            // 
            // propScriptsEdit
            // 
            this.propScriptsEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.propScriptsEdit.Enabled = false;
            this.propScriptsEdit.Image = global::Voron_Poster.Properties.Resources.gear_16xLG;
            this.propScriptsEdit.Location = new System.Drawing.Point(534, 310);
            this.propScriptsEdit.Name = "propScriptsEdit";
            this.propScriptsEdit.Size = new System.Drawing.Size(26, 26);
            this.propScriptsEdit.TabIndex = 6;
            this.propScriptsEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propScriptsEdit.UseVisualStyleBackColor = false;
            this.propScriptsEdit.Click += new System.EventHandler(this.propScriptsEdit_Click);
            // 
            // propScriptsDown
            // 
            this.propScriptsDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propScriptsDown.Enabled = false;
            this.propScriptsDown.Image = global::Voron_Poster.Properties.Resources.arrow_Down_16xLG;
            this.propScriptsDown.Location = new System.Drawing.Point(534, 51);
            this.propScriptsDown.Name = "propScriptsDown";
            this.propScriptsDown.Size = new System.Drawing.Size(26, 26);
            this.propScriptsDown.TabIndex = 5;
            this.propScriptsDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propScriptsDown.UseVisualStyleBackColor = true;
            this.propScriptsDown.Click += new System.EventHandler(this.propScriptsDown_Click);
            // 
            // propScriptsUp
            // 
            this.propScriptsUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propScriptsUp.Enabled = false;
            this.propScriptsUp.Image = global::Voron_Poster.Properties.Resources.arrow_Up_16xLG;
            this.propScriptsUp.Location = new System.Drawing.Point(534, 19);
            this.propScriptsUp.Name = "propScriptsUp";
            this.propScriptsUp.Size = new System.Drawing.Size(26, 26);
            this.propScriptsUp.TabIndex = 4;
            this.propScriptsUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propScriptsUp.UseVisualStyleBackColor = true;
            this.propScriptsUp.Click += new System.EventHandler(this.propScriptsUp_Click);
            // 
            // propScriptsAdd
            // 
            this.propScriptsAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.propScriptsAdd.Image = global::Voron_Poster.Properties.Resources.action_add_16xLG;
            this.propScriptsAdd.Location = new System.Drawing.Point(534, 278);
            this.propScriptsAdd.Name = "propScriptsAdd";
            this.propScriptsAdd.Size = new System.Drawing.Size(26, 26);
            this.propScriptsAdd.TabIndex = 0;
            this.propScriptsAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propScriptsAdd.UseVisualStyleBackColor = true;
            this.propScriptsAdd.Click += new System.EventHandler(this.propScriptsAdd_Click);
            // 
            // propScriptsRemove
            // 
            this.propScriptsRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.propScriptsRemove.Enabled = false;
            this.propScriptsRemove.Image = global::Voron_Poster.Properties.Resources.Remove_16xLG;
            this.propScriptsRemove.Location = new System.Drawing.Point(534, 342);
            this.propScriptsRemove.Name = "propScriptsRemove";
            this.propScriptsRemove.Size = new System.Drawing.Size(26, 26);
            this.propScriptsRemove.TabIndex = 1;
            this.propScriptsRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propScriptsRemove.UseVisualStyleBackColor = true;
            this.propScriptsRemove.Click += new System.EventHandler(this.propScriptsRemove_Click);
            // 
            // propScriptsList
            // 
            this.propScriptsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propScriptsList.Location = new System.Drawing.Point(13, 19);
            this.propScriptsList.Name = "propScriptsList";
            this.propScriptsList.Size = new System.Drawing.Size(515, 329);
            this.propScriptsList.TabIndex = 3;
            this.propScriptsList.SelectedIndexChanged += new System.EventHandler(this.propScriptsList_SelectedIndexChanged);
            this.propScriptsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.propScriptsList_MouseDoubleClick);
            // 
            // propAuthGroup
            // 
            this.propAuthGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propAuthGroup.Controls.Add(this.propAuthLog);
            this.propAuthGroup.Controls.Add(this.propAuthTryLogin);
            this.propAuthGroup.Controls.Add(this.propAuthGlobalLabel);
            this.propAuthGroup.Controls.Add(this.propAuthGlobalUsername);
            this.propAuthGroup.Controls.Add(this.propAuthLocal);
            this.propAuthGroup.Controls.Add(this.propAuthGlobal);
            this.propAuthGroup.Controls.Add(this.propAuthShowPassword);
            this.propAuthGroup.Controls.Add(this.propPasswordLabel);
            this.propAuthGroup.Controls.Add(this.propPassword);
            this.propAuthGroup.Controls.Add(this.propUsernameLabel);
            this.propAuthGroup.Controls.Add(this.propUsername);
            this.propAuthGroup.Location = new System.Drawing.Point(602, 151);
            this.propAuthGroup.Name = "propAuthGroup";
            this.propAuthGroup.Size = new System.Drawing.Size(237, 403);
            this.propAuthGroup.TabIndex = 10;
            this.propAuthGroup.TabStop = false;
            this.propAuthGroup.Text = "Авторизация";
            // 
            // propAuthLog
            // 
            this.propAuthLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propAuthLog.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Help_and_inconclusive_16xLG;
            this.propAuthLog.Location = new System.Drawing.Point(196, 208);
            this.propAuthLog.Name = "propAuthLog";
            this.propAuthLog.Size = new System.Drawing.Size(24, 24);
            this.propAuthLog.TabIndex = 10;
            this.propAuthLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propAuthLog.UseVisualStyleBackColor = true;
            this.propAuthLog.Click += new System.EventHandler(this.propAuthLog_Click);
            // 
            // propAuthTryLogin
            // 
            this.propAuthTryLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propAuthTryLogin.Enabled = false;
            this.propAuthTryLogin.Image = global::Voron_Poster.Properties.Resources.user_16xLG;
            this.propAuthTryLogin.Location = new System.Drawing.Point(11, 208);
            this.propAuthTryLogin.Name = "propAuthTryLogin";
            this.propAuthTryLogin.Size = new System.Drawing.Size(179, 24);
            this.propAuthTryLogin.TabIndex = 9;
            this.propAuthTryLogin.Text = "Пробная авторизация";
            this.propAuthTryLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propAuthTryLogin.UseVisualStyleBackColor = true;
            this.propAuthTryLogin.Click += new System.EventHandler(this.propAuthTryLogin_Click);
            // 
            // propAuthGlobalLabel
            // 
            this.propAuthGlobalLabel.AutoSize = true;
            this.propAuthGlobalLabel.Location = new System.Drawing.Point(6, 19);
            this.propAuthGlobalLabel.Name = "propAuthGlobalLabel";
            this.propAuthGlobalLabel.Size = new System.Drawing.Size(173, 13);
            this.propAuthGlobalLabel.TabIndex = 0;
            this.propAuthGlobalLabel.Text = "Для авторизации использовать:";
            // 
            // propAuthGlobalUsername
            // 
            this.propAuthGlobalUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propAuthGlobalUsername.Location = new System.Drawing.Point(11, 58);
            this.propAuthGlobalUsername.Name = "propAuthGlobalUsername";
            this.propAuthGlobalUsername.ReadOnly = true;
            this.propAuthGlobalUsername.Size = new System.Drawing.Size(209, 20);
            this.propAuthGlobalUsername.TabIndex = 2;
            this.propAuthGlobalUsername.TabStop = false;
            // 
            // propAuthLocal
            // 
            this.propAuthLocal.AutoSize = true;
            this.propAuthLocal.Location = new System.Drawing.Point(11, 84);
            this.propAuthLocal.Name = "propAuthLocal";
            this.propAuthLocal.Size = new System.Drawing.Size(166, 17);
            this.propAuthLocal.TabIndex = 3;
            this.propAuthLocal.Text = "Отдельную учетную запись:";
            this.propAuthLocal.UseVisualStyleBackColor = true;
            this.propAuthLocal.CheckedChanged += new System.EventHandler(this.propAuthGlobal_CheckedChanged);
            // 
            // propAuthGlobal
            // 
            this.propAuthGlobal.AutoSize = true;
            this.propAuthGlobal.Checked = true;
            this.propAuthGlobal.Location = new System.Drawing.Point(11, 35);
            this.propAuthGlobal.Name = "propAuthGlobal";
            this.propAuthGlobal.Size = new System.Drawing.Size(171, 17);
            this.propAuthGlobal.TabIndex = 1;
            this.propAuthGlobal.TabStop = true;
            this.propAuthGlobal.Text = "Глобальную учетную запись:";
            this.propAuthGlobal.UseVisualStyleBackColor = true;
            this.propAuthGlobal.CheckedChanged += new System.EventHandler(this.propAuthGlobal_CheckedChanged);
            // 
            // propAuthShowPassword
            // 
            this.propAuthShowPassword.AutoSize = true;
            this.propAuthShowPassword.Enabled = false;
            this.propAuthShowPassword.Location = new System.Drawing.Point(11, 185);
            this.propAuthShowPassword.Name = "propAuthShowPassword";
            this.propAuthShowPassword.Size = new System.Drawing.Size(128, 17);
            this.propAuthShowPassword.TabIndex = 8;
            this.propAuthShowPassword.Tag = "";
            this.propAuthShowPassword.Text = "Показывать пароль";
            this.propAuthShowPassword.UseVisualStyleBackColor = true;
            this.propAuthShowPassword.CheckedChanged += new System.EventHandler(this.propAuthShowPassword_CheckedChanged);
            // 
            // propPasswordLabel
            // 
            this.propPasswordLabel.AutoSize = true;
            this.propPasswordLabel.Location = new System.Drawing.Point(8, 143);
            this.propPasswordLabel.Name = "propPasswordLabel";
            this.propPasswordLabel.Size = new System.Drawing.Size(48, 13);
            this.propPasswordLabel.TabIndex = 6;
            this.propPasswordLabel.Text = "Пароль:";
            // 
            // propPassword
            // 
            this.propPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propPassword.Enabled = false;
            this.propPassword.Location = new System.Drawing.Point(11, 159);
            this.propPassword.Name = "propPassword";
            this.propPassword.Size = new System.Drawing.Size(209, 20);
            this.propPassword.TabIndex = 7;
            this.propPassword.UseSystemPasswordChar = true;
            this.propPassword.TextChanged += new System.EventHandler(this.propAuthPassword_TextChanged);
            // 
            // propUsernameLabel
            // 
            this.propUsernameLabel.AutoSize = true;
            this.propUsernameLabel.Location = new System.Drawing.Point(8, 104);
            this.propUsernameLabel.Name = "propUsernameLabel";
            this.propUsernameLabel.Size = new System.Drawing.Size(106, 13);
            this.propUsernameLabel.TabIndex = 4;
            this.propUsernameLabel.Text = "Имя пользователя:";
            // 
            // propUsername
            // 
            this.propUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propUsername.Enabled = false;
            this.propUsername.Location = new System.Drawing.Point(11, 120);
            this.propUsername.Name = "propUsername";
            this.propUsername.Size = new System.Drawing.Size(209, 20);
            this.propUsername.TabIndex = 5;
            this.propUsername.TextChanged += new System.EventHandler(this.propAuthUsername_TextChanged);
            // 
            // propProfilesGroup
            // 
            this.propProfilesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propProfilesGroup.Controls.Add(this.propProfileBrowse);
            this.propProfilesGroup.Controls.Add(this.propProfileDelete);
            this.propProfilesGroup.Controls.Add(this.propProfileNew);
            this.propProfilesGroup.Controls.Add(this.propProfileSave);
            this.propProfilesGroup.Controls.Add(this.propProfileLoad);
            this.propProfilesGroup.Controls.Add(this.propProfiles);
            this.propProfilesGroup.Location = new System.Drawing.Point(23, 59);
            this.propProfilesGroup.Name = "propProfilesGroup";
            this.propProfilesGroup.Padding = new System.Windows.Forms.Padding(10, 3, 3, 5);
            this.propProfilesGroup.Size = new System.Drawing.Size(816, 79);
            this.propProfilesGroup.TabIndex = 2;
            this.propProfilesGroup.TabStop = false;
            this.propProfilesGroup.Text = "Готовые профили";
            // 
            // propProfileBrowse
            // 
            this.propProfileBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propProfileBrowse.Image = global::Voron_Poster.Properties.Resources.folder_Open_16xLG;
            this.propProfileBrowse.Location = new System.Drawing.Point(703, 17);
            this.propProfileBrowse.Name = "propProfileBrowse";
            this.propProfileBrowse.Size = new System.Drawing.Size(96, 24);
            this.propProfileBrowse.TabIndex = 5;
            this.propProfileBrowse.Text = "... Обзор";
            this.propProfileBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propProfileBrowse.UseVisualStyleBackColor = true;
            this.propProfileBrowse.Click += new System.EventHandler(this.propProfileBrowse_Click);
            // 
            // propProfileDelete
            // 
            this.propProfileDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.propProfileDelete.Image = global::Voron_Poster.Properties.Resources.action_Cancel_16xLG;
            this.propProfileDelete.Location = new System.Drawing.Point(247, 47);
            this.propProfileDelete.Name = "propProfileDelete";
            this.propProfileDelete.Size = new System.Drawing.Size(97, 24);
            this.propProfileDelete.TabIndex = 2;
            this.propProfileDelete.Text = "Удалить";
            this.propProfileDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propProfileDelete.UseVisualStyleBackColor = true;
            this.propProfileDelete.Click += new System.EventHandler(this.DeleteProfileButton_Click);
            // 
            // propProfileNew
            // 
            this.propProfileNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.propProfileNew.Image = global::Voron_Poster.Properties.Resources.action_add_16xLG;
            this.propProfileNew.Location = new System.Drawing.Point(13, 47);
            this.propProfileNew.Name = "propProfileNew";
            this.propProfileNew.Size = new System.Drawing.Size(125, 24);
            this.propProfileNew.TabIndex = 4;
            this.propProfileNew.Text = "Создать новый";
            this.propProfileNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propProfileNew.UseVisualStyleBackColor = true;
            this.propProfileNew.Click += new System.EventHandler(this.NewProfileButton_Click);
            // 
            // propProfileSave
            // 
            this.propProfileSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.propProfileSave.Image = global::Voron_Poster.Properties.Resources.save_16xLG;
            this.propProfileSave.Location = new System.Drawing.Point(144, 47);
            this.propProfileSave.Name = "propProfileSave";
            this.propProfileSave.Size = new System.Drawing.Size(97, 24);
            this.propProfileSave.TabIndex = 3;
            this.propProfileSave.Text = "Сохранить";
            this.propProfileSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propProfileSave.UseVisualStyleBackColor = true;
            this.propProfileSave.Click += new System.EventHandler(this.SaveProfileButton_Click);
            // 
            // propProfileLoad
            // 
            this.propProfileLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.propProfileLoad.Image = global::Voron_Poster.Properties.Resources.Open_6529;
            this.propProfileLoad.Location = new System.Drawing.Point(652, 47);
            this.propProfileLoad.Name = "propProfileLoad";
            this.propProfileLoad.Size = new System.Drawing.Size(147, 24);
            this.propProfileLoad.TabIndex = 1;
            this.propProfileLoad.Text = "Открыть (Применить)";
            this.propProfileLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propProfileLoad.UseVisualStyleBackColor = true;
            this.propProfileLoad.Click += new System.EventHandler(this.LoadProfileButton_Click);
            // 
            // propProfiles
            // 
            this.propProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propProfiles.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.propProfiles.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.propProfiles.FormattingEnabled = true;
            this.propProfiles.Location = new System.Drawing.Point(13, 19);
            this.propProfiles.Name = "propProfiles";
            this.propProfiles.Size = new System.Drawing.Size(684, 21);
            this.propProfiles.TabIndex = 0;
            this.propProfiles.TextChanged += new System.EventHandler(this.ProfileComboBox_TextChanged);
            this.propProfiles.Enter += new System.EventHandler(this.ProfileComboBox_Enter);
            // 
            // propMainUrlLabel
            // 
            this.propMainUrlLabel.AutoSize = true;
            this.propMainUrlLabel.Location = new System.Drawing.Point(23, 151);
            this.propMainUrlLabel.Name = "propMainUrlLabel";
            this.propMainUrlLabel.Size = new System.Drawing.Size(197, 13);
            this.propMainUrlLabel.TabIndex = 4;
            this.propMainUrlLabel.Text = "Ссылка на главную страницу форума";
            // 
            // propMainUrl
            // 
            this.propMainUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propMainUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.propMainUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.propMainUrl.Location = new System.Drawing.Point(23, 167);
            this.propMainUrl.Name = "propMainUrl";
            this.propMainUrl.Size = new System.Drawing.Size(573, 20);
            this.propMainUrl.TabIndex = 5;
            this.propMainUrl.Text = "http://";
            this.propMainUrl.TextChanged += new System.EventHandler(this.propUrl_TextChanged);
            // 
            // propTargetLabel
            // 
            this.propTargetLabel.AutoSize = true;
            this.propTargetLabel.Location = new System.Drawing.Point(23, 17);
            this.propTargetLabel.Name = "propTargetLabel";
            this.propTargetLabel.Size = new System.Drawing.Size(212, 13);
            this.propTargetLabel.TabIndex = 0;
            this.propTargetLabel.Text = "Ссылка на тему/раздел для публикации";
            // 
            // propTargetUrl
            // 
            this.propTargetUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propTargetUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.propTargetUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.propTargetUrl.Location = new System.Drawing.Point(23, 33);
            this.propTargetUrl.Name = "propTargetUrl";
            this.propTargetUrl.Size = new System.Drawing.Size(816, 20);
            this.propTargetUrl.TabIndex = 1;
            this.propTargetUrl.Text = "http://";
            this.propTargetUrl.TextChanged += new System.EventHandler(this.propUrl_TextChanged);
            // 
            // propApply
            // 
            this.propApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.propApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.propApply.Enabled = false;
            this.propApply.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Complete_and_ok_32xMD_color;
            this.propApply.Location = new System.Drawing.Point(602, 560);
            this.propApply.Name = "propApply";
            this.propApply.Size = new System.Drawing.Size(115, 38);
            this.propApply.TabIndex = 11;
            this.propApply.Text = "Применить";
            this.propApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propApply.UseVisualStyleBackColor = true;
            this.propApply.Click += new System.EventHandler(this.propApply_Click);
            // 
            // propCancel
            // 
            this.propCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.propCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.propCancel.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Critical_32xMD_color;
            this.propCancel.Location = new System.Drawing.Point(723, 560);
            this.propCancel.Name = "propCancel";
            this.propCancel.Size = new System.Drawing.Size(116, 38);
            this.propCancel.TabIndex = 12;
            this.propCancel.Text = "Отмена";
            this.propCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propCancel.UseVisualStyleBackColor = true;
            this.propCancel.Click += new System.EventHandler(this.propClose);
            // 
            // scriptsTab
            // 
            this.scriptsTab.Controls.Add(this.splitter1);
            this.scriptsTab.Controls.Add(this.scriptsTabs);
            this.scriptsTab.Controls.Add(this.scriptsListPanel);
            this.scriptsTab.Location = new System.Drawing.Point(4, 22);
            this.scriptsTab.Name = "scriptsTab";
            this.scriptsTab.Padding = new System.Windows.Forms.Padding(3);
            this.scriptsTab.Size = new System.Drawing.Size(862, 621);
            this.scriptsTab.TabIndex = 5;
            this.scriptsTab.Text = "Скрипты";
            this.scriptsTab.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(152, 3);
            this.splitter1.MinExtra = 351;
            this.splitter1.MinSize = 131;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 615);
            this.splitter1.TabIndex = 16;
            this.splitter1.TabStop = false;
            // 
            // scriptsTabs
            // 
            this.scriptsTabs.Controls.Add(this.scriptsCodeTab);
            this.scriptsTabs.Controls.Add(this.scriptsTestTab);
            this.scriptsTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptsTabs.Location = new System.Drawing.Point(152, 3);
            this.scriptsTabs.MinimumSize = new System.Drawing.Size(350, 0);
            this.scriptsTabs.Name = "scriptsTabs";
            this.scriptsTabs.SelectedIndex = 0;
            this.scriptsTabs.Size = new System.Drawing.Size(707, 615);
            this.scriptsTabs.TabIndex = 9;
            // 
            // scriptsCodeTab
            // 
            this.scriptsCodeTab.Controls.Add(this.scriptsDelete);
            this.scriptsCodeTab.Controls.Add(this.scriptsNew);
            this.scriptsCodeTab.Controls.Add(this.scriptsSave);
            this.scriptsCodeTab.Controls.Add(this.scriptsCodeBox);
            this.scriptsCodeTab.Location = new System.Drawing.Point(4, 22);
            this.scriptsCodeTab.Name = "scriptsCodeTab";
            this.scriptsCodeTab.Padding = new System.Windows.Forms.Padding(3);
            this.scriptsCodeTab.Size = new System.Drawing.Size(699, 589);
            this.scriptsCodeTab.TabIndex = 0;
            this.scriptsCodeTab.Text = "Код";
            this.scriptsCodeTab.UseVisualStyleBackColor = true;
            // 
            // scriptsDelete
            // 
            this.scriptsDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scriptsDelete.Image = global::Voron_Poster.Properties.Resources.action_Cancel_16xLG;
            this.scriptsDelete.Location = new System.Drawing.Point(137, 559);
            this.scriptsDelete.Name = "scriptsDelete";
            this.scriptsDelete.Size = new System.Drawing.Size(97, 24);
            this.scriptsDelete.TabIndex = 7;
            this.scriptsDelete.Text = "Удалить";
            this.scriptsDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.scriptsDelete.UseVisualStyleBackColor = true;
            this.scriptsDelete.Click += new System.EventHandler(this.scriptsDelete_Click);
            // 
            // scriptsNew
            // 
            this.scriptsNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scriptsNew.Image = global::Voron_Poster.Properties.Resources.action_add_16xLG;
            this.scriptsNew.Location = new System.Drawing.Point(6, 559);
            this.scriptsNew.Name = "scriptsNew";
            this.scriptsNew.Size = new System.Drawing.Size(125, 24);
            this.scriptsNew.TabIndex = 6;
            this.scriptsNew.Text = "Создать новый";
            this.scriptsNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.scriptsNew.UseVisualStyleBackColor = true;
            this.scriptsNew.Click += new System.EventHandler(this.scriptsNew_Click);
            // 
            // scriptsSave
            // 
            this.scriptsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptsSave.Enabled = false;
            this.scriptsSave.Image = global::Voron_Poster.Properties.Resources.save_16xLG;
            this.scriptsSave.Location = new System.Drawing.Point(596, 559);
            this.scriptsSave.Name = "scriptsSave";
            this.scriptsSave.Size = new System.Drawing.Size(97, 24);
            this.scriptsSave.TabIndex = 5;
            this.scriptsSave.Text = "Сохранить";
            this.scriptsSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.scriptsSave.UseVisualStyleBackColor = true;
            this.scriptsSave.Click += new System.EventHandler(this.scriptsSave_Click);
            // 
            // scriptsCodeBox
            // 
            this.scriptsCodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptsCodeBox.Location = new System.Drawing.Point(6, 6);
            this.scriptsCodeBox.Multiline = true;
            this.scriptsCodeBox.Name = "scriptsCodeBox";
            this.scriptsCodeBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptsCodeBox.Size = new System.Drawing.Size(687, 547);
            this.scriptsCodeBox.TabIndex = 1;
            this.scriptsCodeBox.TextChanged += new System.EventHandler(this.scriptsEditor_TextChanged);
            // 
            // scriptsTestTab
            // 
            this.scriptsTestTab.Controls.Add(this.scriptsResult);
            this.scriptsTestTab.Controls.Add(this.scriptsRunPanel);
            this.scriptsTestTab.Controls.Add(this.splitter3);
            this.scriptsTestTab.Controls.Add(this.scriptsMessage);
            this.scriptsTestTab.Controls.Add(this.scriptsSpacePanel);
            this.scriptsTestTab.Controls.Add(this.scriptsSubject);
            this.scriptsTestTab.Location = new System.Drawing.Point(4, 22);
            this.scriptsTestTab.Name = "scriptsTestTab";
            this.scriptsTestTab.Padding = new System.Windows.Forms.Padding(10);
            this.scriptsTestTab.Size = new System.Drawing.Size(699, 589);
            this.scriptsTestTab.TabIndex = 1;
            this.scriptsTestTab.Text = "Проверка";
            this.scriptsTestTab.UseVisualStyleBackColor = true;
            // 
            // scriptsResult
            // 
            this.scriptsResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptsResult.Location = new System.Drawing.Point(10, 181);
            this.scriptsResult.Multiline = true;
            this.scriptsResult.Name = "scriptsResult";
            this.scriptsResult.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.scriptsResult.Size = new System.Drawing.Size(679, 398);
            this.scriptsResult.TabIndex = 42;
            this.scriptsResult.WordWrap = false;
            // 
            // scriptsRunPanel
            // 
            this.scriptsRunPanel.Controls.Add(this.scriptsStatusLabel);
            this.scriptsRunPanel.Controls.Add(this.scriptsRun);
            this.scriptsRunPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.scriptsRunPanel.Location = new System.Drawing.Point(10, 145);
            this.scriptsRunPanel.Name = "scriptsRunPanel";
            this.scriptsRunPanel.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.scriptsRunPanel.Size = new System.Drawing.Size(679, 36);
            this.scriptsRunPanel.TabIndex = 37;
            // 
            // scriptsStatusLabel
            // 
            this.scriptsStatusLabel.Location = new System.Drawing.Point(105, 5);
            this.scriptsStatusLabel.Name = "scriptsStatusLabel";
            this.scriptsStatusLabel.Size = new System.Drawing.Size(225, 31);
            this.scriptsStatusLabel.TabIndex = 33;
            this.scriptsStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scriptsRun
            // 
            this.scriptsRun.Dock = System.Windows.Forms.DockStyle.Left;
            this.scriptsRun.Image = global::Voron_Poster.Properties.Resources.test_32x_SMcuted;
            this.scriptsRun.Location = new System.Drawing.Point(0, 5);
            this.scriptsRun.MaximumSize = new System.Drawing.Size(99, 25);
            this.scriptsRun.Name = "scriptsRun";
            this.scriptsRun.Size = new System.Drawing.Size(99, 25);
            this.scriptsRun.TabIndex = 32;
            this.scriptsRun.Text = "Тест";
            this.scriptsRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.scriptsRun.UseVisualStyleBackColor = true;
            this.scriptsRun.Click += new System.EventHandler(this.scriptsRun_Click);
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(10, 142);
            this.splitter3.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.splitter3.MinSize = 37;
            this.splitter3.Name = "splitter3";
            this.splitter3.Padding = new System.Windows.Forms.Padding(0, 10, 10, 10);
            this.splitter3.Size = new System.Drawing.Size(679, 3);
            this.splitter3.TabIndex = 35;
            this.splitter3.TabStop = false;
            // 
            // scriptsMessage
            // 
            this.scriptsMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.scriptsMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.scriptsMessage.ForeColor = System.Drawing.SystemColors.GrayText;
            this.scriptsMessage.Location = new System.Drawing.Point(10, 40);
            this.scriptsMessage.Multiline = true;
            this.scriptsMessage.Name = "scriptsMessage";
            this.scriptsMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptsMessage.Size = new System.Drawing.Size(679, 102);
            this.scriptsMessage.TabIndex = 40;
            this.scriptsMessage.Text = "[b]Тестовое сообщение[b]\r\nСегодня [color=red]хорошая[/color] погода.\r\nМы пойдем [" +
    "color=#12830a]купаться[/color] на речку.";
            this.scriptsMessage.WordWrap = false;
            this.scriptsMessage.TextChanged += new System.EventHandler(this.scriptsSubject_TextChanged);
            this.scriptsMessage.Enter += new System.EventHandler(this.scriptsTestBox_Enter);
            this.scriptsMessage.Leave += new System.EventHandler(this.scriptsMessage_Leave);
            // 
            // scriptsSpacePanel
            // 
            this.scriptsSpacePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.scriptsSpacePanel.Location = new System.Drawing.Point(10, 30);
            this.scriptsSpacePanel.MaximumSize = new System.Drawing.Size(0, 10);
            this.scriptsSpacePanel.MinimumSize = new System.Drawing.Size(0, 10);
            this.scriptsSpacePanel.Name = "scriptsSpacePanel";
            this.scriptsSpacePanel.Size = new System.Drawing.Size(0, 10);
            this.scriptsSpacePanel.TabIndex = 39;
            // 
            // scriptsSubject
            // 
            this.scriptsSubject.Dock = System.Windows.Forms.DockStyle.Top;
            this.scriptsSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.scriptsSubject.ForeColor = System.Drawing.SystemColors.GrayText;
            this.scriptsSubject.Location = new System.Drawing.Point(10, 10);
            this.scriptsSubject.Name = "scriptsSubject";
            this.scriptsSubject.Size = new System.Drawing.Size(679, 20);
            this.scriptsSubject.TabIndex = 33;
            this.scriptsSubject.Text = "Тема сообщения";
            this.scriptsSubject.TextChanged += new System.EventHandler(this.scriptsSubject_TextChanged);
            this.scriptsSubject.Enter += new System.EventHandler(this.scriptsTestBox_Enter);
            this.scriptsSubject.Leave += new System.EventHandler(this.scriptsSubject_Leave);
            // 
            // scriptsListPanel
            // 
            this.scriptsListPanel.Controls.Add(this.scriptsAccept);
            this.scriptsListPanel.Controls.Add(this.scriptsCancel);
            this.scriptsListPanel.Controls.Add(this.scriptsName);
            this.scriptsListPanel.Controls.Add(this.scriptsList);
            this.scriptsListPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.scriptsListPanel.Location = new System.Drawing.Point(3, 3);
            this.scriptsListPanel.MinimumSize = new System.Drawing.Size(149, 0);
            this.scriptsListPanel.Name = "scriptsListPanel";
            this.scriptsListPanel.Size = new System.Drawing.Size(149, 615);
            this.scriptsListPanel.TabIndex = 7;
            // 
            // scriptsAccept
            // 
            this.scriptsAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptsAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.scriptsAccept.Enabled = false;
            this.scriptsAccept.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Complete_and_ok_32xMD_color;
            this.scriptsAccept.Location = new System.Drawing.Point(3, 573);
            this.scriptsAccept.Name = "scriptsAccept";
            this.scriptsAccept.Size = new System.Drawing.Size(96, 38);
            this.scriptsAccept.TabIndex = 16;
            this.scriptsAccept.Text = "Выбрать";
            this.scriptsAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.scriptsAccept.UseVisualStyleBackColor = true;
            this.scriptsAccept.Click += new System.EventHandler(this.scriptsAccept_Click);
            // 
            // scriptsCancel
            // 
            this.scriptsCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptsCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.scriptsCancel.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Critical_32xMD_color;
            this.scriptsCancel.Location = new System.Drawing.Point(105, 573);
            this.scriptsCancel.Name = "scriptsCancel";
            this.scriptsCancel.Size = new System.Drawing.Size(38, 38);
            this.scriptsCancel.TabIndex = 14;
            this.scriptsCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.scriptsCancel.UseVisualStyleBackColor = true;
            this.scriptsCancel.Click += new System.EventHandler(this.scriptsCancel_Click);
            // 
            // scriptsName
            // 
            this.scriptsName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptsName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.scriptsName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.scriptsName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.scriptsName.Location = new System.Drawing.Point(3, 6);
            this.scriptsName.Name = "scriptsName";
            this.scriptsName.Size = new System.Drawing.Size(140, 20);
            this.scriptsName.TabIndex = 0;
            this.scriptsName.TextChanged += new System.EventHandler(this.scriptsName_TextChanged);
            // 
            // scriptsList
            // 
            this.scriptsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptsList.Location = new System.Drawing.Point(3, 29);
            this.scriptsList.Name = "scriptsList";
            this.scriptsList.Size = new System.Drawing.Size(140, 524);
            this.scriptsList.Sorted = true;
            this.scriptsList.TabIndex = 2;
            this.scriptsList.SelectedIndexChanged += new System.EventHandler(this.scriptsList_SelectedIndexChanged);
            this.scriptsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.scriptsList_MouseDoubleClick);
            // 
            // settingsTab
            // 
            this.settingsTab.Controls.Add(this.settingsApplySuggestedProfile);
            this.settingsTab.Controls.Add(this.settingsLoadLastTasklist);
            this.settingsTab.Controls.Add(this.settingsGAuthGroup);
            this.settingsTab.Controls.Add(this.settingsSave);
            this.settingsTab.Controls.Add(this.settingsCancel);
            this.settingsTab.Location = new System.Drawing.Point(4, 22);
            this.settingsTab.Name = "settingsTab";
            this.settingsTab.Padding = new System.Windows.Forms.Padding(20, 30, 20, 20);
            this.settingsTab.Size = new System.Drawing.Size(862, 621);
            this.settingsTab.TabIndex = 7;
            this.settingsTab.Text = "Настройки";
            this.settingsTab.UseVisualStyleBackColor = true;
            // 
            // settingsApplySuggestedProfile
            // 
            this.settingsApplySuggestedProfile.AutoSize = true;
            this.settingsApplySuggestedProfile.Enabled = false;
            this.settingsApplySuggestedProfile.Location = new System.Drawing.Point(247, 73);
            this.settingsApplySuggestedProfile.Name = "settingsApplySuggestedProfile";
            this.settingsApplySuggestedProfile.Size = new System.Drawing.Size(268, 17);
            this.settingsApplySuggestedProfile.TabIndex = 17;
            this.settingsApplySuggestedProfile.Text = "Автоматически применять найденный профиль";
            this.settingsApplySuggestedProfile.UseVisualStyleBackColor = true;
            this.settingsApplySuggestedProfile.CheckedChanged += new System.EventHandler(this.settings_Changed);
            // 
            // settingsLoadLastTasklist
            // 
            this.settingsLoadLastTasklist.AutoSize = true;
            this.settingsLoadLastTasklist.Checked = true;
            this.settingsLoadLastTasklist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.settingsLoadLastTasklist.Location = new System.Drawing.Point(247, 50);
            this.settingsLoadLastTasklist.Name = "settingsLoadLastTasklist";
            this.settingsLoadLastTasklist.Size = new System.Drawing.Size(256, 17);
            this.settingsLoadLastTasklist.TabIndex = 16;
            this.settingsLoadLastTasklist.Text = "Загружать последний список тем при старте";
            this.settingsLoadLastTasklist.UseVisualStyleBackColor = true;
            this.settingsLoadLastTasklist.CheckedChanged += new System.EventHandler(this.settings_Changed);
            // 
            // settingsGAuthGroup
            // 
            this.settingsGAuthGroup.Controls.Add(this.settingsGAuthShowPassword);
            this.settingsGAuthGroup.Controls.Add(this.settingsGAuthPasswordLabel);
            this.settingsGAuthGroup.Controls.Add(this.settingsGAuthPassword);
            this.settingsGAuthGroup.Controls.Add(this.settingsGAuthUsenameLabel);
            this.settingsGAuthGroup.Controls.Add(this.settingsGAuthUsername);
            this.settingsGAuthGroup.Location = new System.Drawing.Point(23, 33);
            this.settingsGAuthGroup.Name = "settingsGAuthGroup";
            this.settingsGAuthGroup.Padding = new System.Windows.Forms.Padding(5, 8, 3, 3);
            this.settingsGAuthGroup.Size = new System.Drawing.Size(199, 136);
            this.settingsGAuthGroup.TabIndex = 13;
            this.settingsGAuthGroup.TabStop = false;
            this.settingsGAuthGroup.Text = "Глобальная учетная запись";
            // 
            // settingsGAuthShowPassword
            // 
            this.settingsGAuthShowPassword.AutoSize = true;
            this.settingsGAuthShowPassword.Location = new System.Drawing.Point(11, 102);
            this.settingsGAuthShowPassword.Name = "settingsGAuthShowPassword";
            this.settingsGAuthShowPassword.Size = new System.Drawing.Size(128, 17);
            this.settingsGAuthShowPassword.TabIndex = 8;
            this.settingsGAuthShowPassword.Tag = "";
            this.settingsGAuthShowPassword.Text = "Показывать пароль";
            this.settingsGAuthShowPassword.UseVisualStyleBackColor = true;
            this.settingsGAuthShowPassword.CheckedChanged += new System.EventHandler(this.settingsGAuthShowPassword_CheckedChanged);
            // 
            // settingsGAuthPasswordLabel
            // 
            this.settingsGAuthPasswordLabel.AutoSize = true;
            this.settingsGAuthPasswordLabel.Location = new System.Drawing.Point(8, 60);
            this.settingsGAuthPasswordLabel.Name = "settingsGAuthPasswordLabel";
            this.settingsGAuthPasswordLabel.Size = new System.Drawing.Size(48, 13);
            this.settingsGAuthPasswordLabel.TabIndex = 6;
            this.settingsGAuthPasswordLabel.Text = "Пароль:";
            // 
            // settingsGAuthPassword
            // 
            this.settingsGAuthPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsGAuthPassword.Location = new System.Drawing.Point(11, 76);
            this.settingsGAuthPassword.Name = "settingsGAuthPassword";
            this.settingsGAuthPassword.Size = new System.Drawing.Size(169, 20);
            this.settingsGAuthPassword.TabIndex = 7;
            this.settingsGAuthPassword.UseSystemPasswordChar = true;
            this.settingsGAuthPassword.TextChanged += new System.EventHandler(this.settings_Changed);
            // 
            // settingsGAuthUsenameLabel
            // 
            this.settingsGAuthUsenameLabel.AutoSize = true;
            this.settingsGAuthUsenameLabel.Location = new System.Drawing.Point(8, 21);
            this.settingsGAuthUsenameLabel.Name = "settingsGAuthUsenameLabel";
            this.settingsGAuthUsenameLabel.Size = new System.Drawing.Size(106, 13);
            this.settingsGAuthUsenameLabel.TabIndex = 4;
            this.settingsGAuthUsenameLabel.Text = "Имя пользователя:";
            // 
            // settingsGAuthUsername
            // 
            this.settingsGAuthUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsGAuthUsername.Location = new System.Drawing.Point(11, 37);
            this.settingsGAuthUsername.Name = "settingsGAuthUsername";
            this.settingsGAuthUsername.Size = new System.Drawing.Size(169, 20);
            this.settingsGAuthUsername.TabIndex = 5;
            this.settingsGAuthUsername.TextChanged += new System.EventHandler(this.settings_Changed);
            // 
            // settingsSave
            // 
            this.settingsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.settingsSave.Enabled = false;
            this.settingsSave.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Complete_and_ok_32xMD_color;
            this.settingsSave.Location = new System.Drawing.Point(602, 560);
            this.settingsSave.Name = "settingsSave";
            this.settingsSave.Size = new System.Drawing.Size(115, 38);
            this.settingsSave.TabIndex = 14;
            this.settingsSave.Text = "Сохранить";
            this.settingsSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.settingsSave.UseVisualStyleBackColor = true;
            this.settingsSave.Click += new System.EventHandler(this.settingsSave_Click);
            // 
            // settingsCancel
            // 
            this.settingsCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.settingsCancel.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Critical_32xMD_color;
            this.settingsCancel.Location = new System.Drawing.Point(723, 560);
            this.settingsCancel.Name = "settingsCancel";
            this.settingsCancel.Size = new System.Drawing.Size(116, 38);
            this.settingsCancel.TabIndex = 15;
            this.settingsCancel.Text = "Отмена";
            this.settingsCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.settingsCancel.UseVisualStyleBackColor = true;
            this.settingsCancel.Click += new System.EventHandler(this.settingsCancel_Click);
            // 
            // aboutTab
            // 
            this.aboutTab.Controls.Add(this.aboutLogo);
            this.aboutTab.Controls.Add(this.aboutAuthorGroup);
            this.aboutTab.Controls.Add(this.aboutLicenseLabel);
            this.aboutTab.Controls.Add(this.aboutAboutInfo);
            this.aboutTab.Controls.Add(this.aboutProgramName);
            this.aboutTab.Controls.Add(this.aboutLicenseBox);
            this.aboutTab.Controls.Add(this.aboutLicenseList);
            this.aboutTab.Controls.Add(this.aboutSupportLabel);
            this.aboutTab.Location = new System.Drawing.Point(4, 22);
            this.aboutTab.Name = "aboutTab";
            this.aboutTab.Padding = new System.Windows.Forms.Padding(20);
            this.aboutTab.Size = new System.Drawing.Size(862, 621);
            this.aboutTab.TabIndex = 6;
            this.aboutTab.Text = "О программе";
            this.aboutTab.UseVisualStyleBackColor = true;
            // 
            // aboutLogo
            // 
            this.aboutLogo.Image = global::Voron_Poster.Properties.Resources.VoronPosterIcon;
            this.aboutLogo.Location = new System.Drawing.Point(23, 20);
            this.aboutLogo.Name = "aboutLogo";
            this.aboutLogo.Size = new System.Drawing.Size(100, 100);
            this.aboutLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.aboutLogo.TabIndex = 29;
            this.aboutLogo.TabStop = false;
            // 
            // aboutAuthorGroup
            // 
            this.aboutAuthorGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutAuthorGroup.Controls.Add(this.aboutAuthorSkype);
            this.aboutAuthorGroup.Controls.Add(this.aboutAurhorVK);
            this.aboutAuthorGroup.Controls.Add(this.aboutAuthorEmail);
            this.aboutAuthorGroup.Controls.Add(this.aboutAuthorName);
            this.aboutAuthorGroup.Controls.Add(this.aboutAuthorAvatar);
            this.aboutAuthorGroup.Location = new System.Drawing.Point(587, 110);
            this.aboutAuthorGroup.Name = "aboutAuthorGroup";
            this.aboutAuthorGroup.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.aboutAuthorGroup.Size = new System.Drawing.Size(252, 112);
            this.aboutAuthorGroup.TabIndex = 24;
            this.aboutAuthorGroup.TabStop = false;
            this.aboutAuthorGroup.Text = "Автор программы";
            // 
            // aboutAuthorSkype
            // 
            this.aboutAuthorSkype.AutoSize = true;
            this.aboutAuthorSkype.Location = new System.Drawing.Point(97, 76);
            this.aboutAuthorSkype.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.aboutAuthorSkype.Name = "aboutAuthorSkype";
            this.aboutAuthorSkype.Size = new System.Drawing.Size(91, 13);
            this.aboutAuthorSkype.TabIndex = 7;
            this.aboutAuthorSkype.TabStop = true;
            this.aboutAuthorSkype.Text = "Skype: Voron.exe";
            this.aboutAuthorSkype.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.aboutAuthorSkype_LinkClicked);
            // 
            // aboutAurhorVK
            // 
            this.aboutAurhorVK.AutoSize = true;
            this.aboutAurhorVK.Location = new System.Drawing.Point(97, 60);
            this.aboutAurhorVK.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.aboutAurhorVK.Name = "aboutAurhorVK";
            this.aboutAurhorVK.Size = new System.Drawing.Size(145, 13);
            this.aboutAurhorVK.TabIndex = 6;
            this.aboutAurhorVK.TabStop = true;
            this.aboutAurhorVK.Text = "https://vk.com/id100633452";
            this.aboutAurhorVK.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.aboutAurhorVK_LinkClicked);
            // 
            // aboutAuthorEmail
            // 
            this.aboutAuthorEmail.AutoSize = true;
            this.aboutAuthorEmail.Location = new System.Drawing.Point(97, 44);
            this.aboutAuthorEmail.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.aboutAuthorEmail.Name = "aboutAuthorEmail";
            this.aboutAuthorEmail.Size = new System.Drawing.Size(113, 13);
            this.aboutAuthorEmail.TabIndex = 5;
            this.aboutAuthorEmail.TabStop = true;
            this.aboutAuthorEmail.Text = "Voron.exe@gmail.com";
            this.aboutAuthorEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.aboutAuthorEmail_LinkClicked);
            // 
            // aboutAuthorName
            // 
            this.aboutAuthorName.AutoSize = true;
            this.aboutAuthorName.Location = new System.Drawing.Point(97, 28);
            this.aboutAuthorName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.aboutAuthorName.Name = "aboutAuthorName";
            this.aboutAuthorName.Size = new System.Drawing.Size(141, 13);
            this.aboutAuthorName.TabIndex = 3;
            this.aboutAuthorName.Text = "Воронин Игорь Борисович";
            // 
            // aboutAuthorAvatar
            // 
            this.aboutAuthorAvatar.Image = global::Voron_Poster.Properties.Resources.YAAvatar__2_;
            this.aboutAuthorAvatar.Location = new System.Drawing.Point(11, 19);
            this.aboutAuthorAvatar.Name = "aboutAuthorAvatar";
            this.aboutAuthorAvatar.Size = new System.Drawing.Size(80, 80);
            this.aboutAuthorAvatar.TabIndex = 0;
            this.aboutAuthorAvatar.TabStop = false;
            // 
            // aboutLicenseLabel
            // 
            this.aboutLicenseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutLicenseLabel.Location = new System.Drawing.Point(23, 120);
            this.aboutLicenseLabel.Name = "aboutLicenseLabel";
            this.aboutLicenseLabel.Size = new System.Drawing.Size(558, 91);
            this.aboutLicenseLabel.TabIndex = 28;
            this.aboutLicenseLabel.Text = resources.GetString("aboutLicenseLabel.Text");
            // 
            // aboutAboutInfo
            // 
            this.aboutAboutInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutAboutInfo.Location = new System.Drawing.Point(129, 48);
            this.aboutAboutInfo.Name = "aboutAboutInfo";
            this.aboutAboutInfo.Size = new System.Drawing.Size(710, 16);
            this.aboutAboutInfo.TabIndex = 27;
            this.aboutAboutInfo.Text = "Программа для автоматической публикации сообщений на форумы. Написана по заказу.";
            // 
            // aboutProgramName
            // 
            this.aboutProgramName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutProgramName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.aboutProgramName.Location = new System.Drawing.Point(129, 20);
            this.aboutProgramName.Name = "aboutProgramName";
            this.aboutProgramName.Size = new System.Drawing.Size(710, 28);
            this.aboutProgramName.TabIndex = 26;
            this.aboutProgramName.Text = "Voron Poster";
            this.aboutProgramName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // aboutLicenseBox
            // 
            this.aboutLicenseBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutLicenseBox.Location = new System.Drawing.Point(23, 340);
            this.aboutLicenseBox.Multiline = true;
            this.aboutLicenseBox.Name = "aboutLicenseBox";
            this.aboutLicenseBox.ReadOnly = true;
            this.aboutLicenseBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.aboutLicenseBox.Size = new System.Drawing.Size(816, 258);
            this.aboutLicenseBox.TabIndex = 23;
            // 
            // aboutLicenseList
            // 
            this.aboutLicenseList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutLicenseList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.aboutLicenseList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.aboutLicenseList.Items.AddRange(new object[] {
            "Voron Poster © Copyright 2014 by Voronin Igor (Voron.exe)",
            "Codekicker.BBCode © Copyright by http://codekicker.de/",
            "ScintillaNET © Copyright 2002-2006 by Garrett Serack <gserack@gmail.com>",
            "Scintilla © Copyright 1998-2006 by Neil Hodgson <neilh@scintilla.org>",
            "Common.Logging © Copyright Aleksandr Seovic, Mark Pollack, Erich Eichinger, Steph" +
                "en Bohlen",
            "Roslyn © Copyright Microsoft",
            "ScryptCs © Copyright 2013, 2014 Glenn Block, Justin Rusbatch, Filip Wojcieszyn",
            "Html Agility Pack © Somin Mourrier, Jeff Klawiter"});
            this.aboutLicenseList.Location = new System.Drawing.Point(23, 228);
            this.aboutLicenseList.Name = "aboutLicenseList";
            this.aboutLicenseList.Size = new System.Drawing.Size(816, 106);
            this.aboutLicenseList.TabIndex = 22;
            this.aboutLicenseList.SelectedIndexChanged += new System.EventHandler(this.aboutLicenseList_SelectedIndexChanged);
            // 
            // aboutSupportLabel
            // 
            this.aboutSupportLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutSupportLabel.Location = new System.Drawing.Point(129, 64);
            this.aboutSupportLabel.Name = "aboutSupportLabel";
            this.aboutSupportLabel.Size = new System.Drawing.Size(710, 47);
            this.aboutSupportLabel.TabIndex = 25;
            this.aboutSupportLabel.Text = resources.GetString("aboutSupportLabel.Text");
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
            // scriptsTestAbortTimer
            // 
            this.scriptsTestAbortTimer.Interval = 5000;
            this.scriptsTestAbortTimer.Tick += new System.EventHandler(this.scriptsTestAbortTimer_Tick);
            // 
            // tasksTable
            // 
            this.tasksTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tasksTable.AutoScroll = true;
            this.tasksTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tasksTable.BackColor = System.Drawing.Color.White;
            this.tasksTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tasksTable.ColumnCount = 8;
            this.tasksTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tasksTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tasksTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tasksTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tasksTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tasksTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tasksTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tasksTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tasksTable.Controls.Add(this.GTName, 0, 0);
            this.tasksTable.Controls.Add(this.GTStatus, 2, 0);
            this.tasksTable.Controls.Add(this.GTStart, 5, 0);
            this.tasksTable.Controls.Add(this.GTSelected, 0, 0);
            this.tasksTable.Controls.Add(this.GTProgress, 4, 0);
            this.tasksTable.Controls.Add(this.GTStatusIcon, 3, 0);
            this.tasksTable.Controls.Add(this.GTStop, 6, 0);
            this.tasksTable.Controls.Add(this.GTDelete, 7, 0);
            this.tasksTable.Location = new System.Drawing.Point(23, 106);
            this.tasksTable.Margin = new System.Windows.Forms.Padding(3, 23, 3, 3);
            this.tasksTable.Name = "tasksTable";
            this.tasksTable.RowCount = 2;
            this.tasksTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tasksTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tasksTable.Size = new System.Drawing.Size(816, 492);
            this.tasksTable.TabIndex = 0;
            this.tasksTable.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TasksGuiTable_CellPaint);
            this.tasksTable.SizeChanged += new System.EventHandler(this.tasksTable_SizeChanged);
            // 
            // GTName
            // 
            this.GTName.BackColor = System.Drawing.Color.Transparent;
            this.GTName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTName.Location = new System.Drawing.Point(26, 1);
            this.GTName.Margin = new System.Windows.Forms.Padding(0);
            this.GTName.MaximumSize = new System.Drawing.Size(0, 24);
            this.GTName.MinimumSize = new System.Drawing.Size(0, 24);
            this.GTName.Name = "GTName";
            this.GTName.Padding = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.GTName.Size = new System.Drawing.Size(412, 24);
            this.GTName.TabIndex = 13;
            this.GTName.Text = "Тема/Раздел";
            this.GTName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // GTStatus
            // 
            this.GTStatus.BackColor = System.Drawing.Color.Transparent;
            this.GTStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTStatus.Location = new System.Drawing.Point(439, 1);
            this.GTStatus.Margin = new System.Windows.Forms.Padding(0);
            this.GTStatus.MaximumSize = new System.Drawing.Size(0, 24);
            this.GTStatus.MinimumSize = new System.Drawing.Size(0, 24);
            this.GTStatus.Name = "GTStatus";
            this.GTStatus.Padding = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.GTStatus.Size = new System.Drawing.Size(171, 24);
            this.GTStatus.TabIndex = 12;
            this.GTStatus.Text = "Состояние";
            this.GTStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // GTStart
            // 
            this.GTStart.BackColor = System.Drawing.Color.Transparent;
            this.GTStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTStart.Enabled = false;
            this.GTStart.Image = global::Voron_Poster.Properties.Resources.arrow_run_16xLG;
            this.GTStart.Location = new System.Drawing.Point(740, 1);
            this.GTStart.Margin = new System.Windows.Forms.Padding(0);
            this.GTStart.MaximumSize = new System.Drawing.Size(24, 24);
            this.GTStart.MinimumSize = new System.Drawing.Size(24, 24);
            this.GTStart.Name = "GTStart";
            this.GTStart.Size = new System.Drawing.Size(24, 24);
            this.GTStart.TabIndex = 8;
            this.GTStart.UseVisualStyleBackColor = false;
            this.GTStart.Click += new System.EventHandler(this.GTStartStop_Click);
            // 
            // GTSelected
            // 
            this.GTSelected.BackColor = System.Drawing.Color.Transparent;
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
            this.GTProgress.Location = new System.Drawing.Point(639, 4);
            this.GTProgress.Maximum = 561;
            this.GTProgress.MaximumSize = new System.Drawing.Size(0, 18);
            this.GTProgress.MinimumSize = new System.Drawing.Size(0, 18);
            this.GTProgress.Name = "GTProgress";
            this.GTProgress.Size = new System.Drawing.Size(97, 18);
            this.GTProgress.TabIndex = 2;
            // 
            // GTStatusIcon
            // 
            this.GTStatusIcon.BackColor = System.Drawing.Color.Transparent;
            this.GTStatusIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTStatusIcon.Image = global::Voron_Poster.Properties.Resources.StatusAnnotations_Stop_16xLG;
            this.GTStatusIcon.Location = new System.Drawing.Point(611, 1);
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
            this.GTStop.BackColor = System.Drawing.Color.Transparent;
            this.GTStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTStop.Enabled = false;
            this.GTStop.Image = global::Voron_Poster.Properties.Resources.Symbols_Stop_16xLG;
            this.GTStop.Location = new System.Drawing.Point(765, 1);
            this.GTStop.Margin = new System.Windows.Forms.Padding(0);
            this.GTStop.MaximumSize = new System.Drawing.Size(24, 24);
            this.GTStop.MinimumSize = new System.Drawing.Size(24, 24);
            this.GTStop.Name = "GTStop";
            this.GTStop.Size = new System.Drawing.Size(24, 24);
            this.GTStop.TabIndex = 7;
            this.GTStop.UseVisualStyleBackColor = false;
            this.GTStop.Click += new System.EventHandler(this.GTStartStop_Click);
            // 
            // GTDelete
            // 
            this.GTDelete.BackColor = System.Drawing.Color.Transparent;
            this.GTDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GTDelete.Enabled = false;
            this.GTDelete.Image = global::Voron_Poster.Properties.Resources.action_Cancel_16xLG;
            this.GTDelete.Location = new System.Drawing.Point(790, 1);
            this.GTDelete.Margin = new System.Windows.Forms.Padding(0);
            this.GTDelete.MaximumSize = new System.Drawing.Size(24, 24);
            this.GTDelete.MinimumSize = new System.Drawing.Size(24, 24);
            this.GTDelete.Name = "GTDelete";
            this.GTDelete.Size = new System.Drawing.Size(24, 24);
            this.GTDelete.TabIndex = 10;
            this.GTDelete.UseVisualStyleBackColor = false;
            this.GTDelete.Click += new System.EventHandler(this.GTDelete_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 653);
            this.Controls.Add(this.Tabs);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(590, 539);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Voron Poster";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Tabs.ResumeLayout(false);
            this.messageTab.ResumeLayout(false);
            this.messageTab.PerformLayout();
            this.previewTab.ResumeLayout(false);
            this.previewPanel.ResumeLayout(false);
            this.previewWBPanel.ResumeLayout(false);
            this.tasksTab.ResumeLayout(false);
            this.tasksGroup.ResumeLayout(false);
            this.tasksGroup.PerformLayout();
            this.propTab.ResumeLayout(false);
            this.propTab.PerformLayout();
            this.propScriptsGroup.ResumeLayout(false);
            this.propAuthGroup.ResumeLayout(false);
            this.propAuthGroup.PerformLayout();
            this.propProfilesGroup.ResumeLayout(false);
            this.scriptsTab.ResumeLayout(false);
            this.scriptsTabs.ResumeLayout(false);
            this.scriptsCodeTab.ResumeLayout(false);
            this.scriptsCodeTab.PerformLayout();
            this.scriptsTestTab.ResumeLayout(false);
            this.scriptsTestTab.PerformLayout();
            this.scriptsRunPanel.ResumeLayout(false);
            this.scriptsListPanel.ResumeLayout(false);
            this.scriptsListPanel.PerformLayout();
            this.settingsTab.ResumeLayout(false);
            this.settingsTab.PerformLayout();
            this.settingsGAuthGroup.ResumeLayout(false);
            this.settingsGAuthGroup.PerformLayout();
            this.aboutTab.ResumeLayout(false);
            this.aboutTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aboutLogo)).EndInit();
            this.aboutAuthorGroup.ResumeLayout(false);
            this.aboutAuthorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aboutAuthorAvatar)).EndInit();
            this.tasksTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GTStatusIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage tasksTab;

        private System.Windows.Forms.TabPage messageTab;
        private System.Windows.Forms.TabPage propTab;
        private System.Windows.Forms.CheckBox GTSelected;
        private System.Windows.Forms.PictureBox GTStatusIcon;
        private System.Windows.Forms.Button GTDelete;
        private System.Windows.Forms.TextBox tasksUrl;
        private System.Windows.Forms.Button tasksAdd;
        private System.Windows.Forms.Label GTStatus;
        private System.Windows.Forms.Label GTName;
        private System.Windows.Forms.TextBox propTargetUrl;
        private System.Windows.Forms.GroupBox propProfilesGroup;
        private System.Windows.Forms.Button propProfileNew;
        private System.Windows.Forms.Button propProfileSave;
        private System.Windows.Forms.Button propProfileLoad;
        private System.Windows.Forms.ComboBox propProfiles;
        private System.Windows.Forms.Label propMainUrlLabel;
        private System.Windows.Forms.TextBox propMainUrl;
        private System.Windows.Forms.Label propTargetLabel;
        private System.Windows.Forms.Button propCancel;
        private System.Windows.Forms.Button propApply;
        private System.Windows.Forms.GroupBox propAuthGroup;
        private System.Windows.Forms.Label propPasswordLabel;
        private System.Windows.Forms.TextBox propPassword;
        private System.Windows.Forms.Label propUsernameLabel;
        private System.Windows.Forms.TextBox propUsername;
        private System.Windows.Forms.GroupBox propScriptsGroup;
        private System.Windows.Forms.Button propScriptsAdd;
        private System.Windows.Forms.Button propScriptsRemove;
        private System.Windows.Forms.ListBox propScriptsList;
        private System.Windows.Forms.Button propProfileDelete;
        private System.Windows.Forms.Button propScriptsDown;
        private System.Windows.Forms.Button propScriptsUp;
        private System.Windows.Forms.Button propProfileBrowse;
        private System.Windows.Forms.RadioButton propAuthLocal;
        private System.Windows.Forms.RadioButton propAuthGlobal;
        public System.Windows.Forms.CheckBox propAuthShowPassword;
        private System.Windows.Forms.Label propAuthGlobalLabel;
        private System.Windows.Forms.TextBox propAuthGlobalUsername;
        private System.Windows.Forms.Button propAuthTryLogin;
        private System.Windows.Forms.Button propEngineDetect;
        private System.Windows.Forms.ComboBox propEngine;
        private System.Windows.Forms.Label propEngineLabel;
        private System.Windows.Forms.Button GTStart;
        private System.Windows.Forms.ProgressBar GTProgress;
        private System.Windows.Forms.Button GTStop;
        private System.Windows.Forms.Label tasksUrlLabel;
        private System.Windows.Forms.Timer TasksUpdater;
        public DBTableLayoutPanel tasksTable;
        public System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.TabPage scriptsTab;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TabControl scriptsTabs;
        private System.Windows.Forms.TabPage scriptsCodeTab;
        private System.Windows.Forms.Button scriptsDelete;
        private System.Windows.Forms.Button scriptsNew;
        private System.Windows.Forms.Button scriptsSave;
        private System.Windows.Forms.TabPage scriptsTestTab;
        private System.Windows.Forms.Panel scriptsRunPanel;
        private System.Windows.Forms.Label scriptsStatusLabel;
        private System.Windows.Forms.Button scriptsRun;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.TextBox scriptsMessage;
        private System.Windows.Forms.Panel scriptsSpacePanel;
        private System.Windows.Forms.TextBox scriptsSubject;
        private System.Windows.Forms.Panel scriptsListPanel;
        private System.Windows.Forms.ListBox scriptsList;
        private System.Windows.Forms.TextBox scriptsResult;
        private System.Windows.Forms.TextBox scriptsName;
        private System.Windows.Forms.TextBox scriptsCodeBox;
        private System.Windows.Forms.GroupBox tasksGroup;
        private System.Windows.Forms.Button tasksSave;
        private System.Windows.Forms.Button tasksLoad;
        private System.Windows.Forms.Timer scriptsTestAbortTimer;
        private System.Windows.Forms.Button scriptsCancel;
        private System.Windows.Forms.Button scriptsAccept;
        private System.Windows.Forms.Button propScriptsEdit;
        public System.Windows.Forms.TextBox messageText;
        public System.Windows.Forms.TextBox messageSubject;
        private System.Windows.Forms.Button propAuthLog;
        private System.Windows.Forms.TabPage aboutTab;
        private System.Windows.Forms.TabPage settingsTab;
        private System.Windows.Forms.GroupBox settingsGAuthGroup;
        public System.Windows.Forms.CheckBox settingsGAuthShowPassword;
        private System.Windows.Forms.Label settingsGAuthPasswordLabel;
        private System.Windows.Forms.TextBox settingsGAuthPassword;
        private System.Windows.Forms.Label settingsGAuthUsenameLabel;
        private System.Windows.Forms.TextBox settingsGAuthUsername;
        private System.Windows.Forms.Button settingsSave;
        private System.Windows.Forms.Button settingsCancel;
        private System.Windows.Forms.CheckBox settingsLoadLastTasklist;
        private System.Windows.Forms.CheckBox settingsApplySuggestedProfile;
        private System.Windows.Forms.Button messagePreview;
        private System.Windows.Forms.TabPage previewTab;
        private System.Windows.Forms.Panel previewPanel;
        private System.Windows.Forms.Button previewDockUndock;
        private System.Windows.Forms.Button messageNext;
        private System.Windows.Forms.Button previewNext;
        private System.Windows.Forms.Panel previewWBPanel;
        private System.Windows.Forms.WebBrowser previewWB;
        private System.Windows.Forms.Label aboutAboutInfo;
        private System.Windows.Forms.Label aboutProgramName;
        private System.Windows.Forms.Label aboutSupportLabel;
        private System.Windows.Forms.GroupBox aboutAuthorGroup;
        private System.Windows.Forms.LinkLabel aboutAurhorVK;
        private System.Windows.Forms.LinkLabel aboutAuthorEmail;
        private System.Windows.Forms.Label aboutAuthorName;
        private System.Windows.Forms.PictureBox aboutAuthorAvatar;
        private System.Windows.Forms.TextBox aboutLicenseBox;
        private System.Windows.Forms.ListBox aboutLicenseList;
        private System.Windows.Forms.Label aboutLicenseLabel;
        private System.Windows.Forms.PictureBox aboutLogo;
        private System.Windows.Forms.Label messageTextLabel;
        private System.Windows.Forms.Label messageSubjectLabel;
        private System.Windows.Forms.LinkLabel aboutAuthorSkype;


    }
}

