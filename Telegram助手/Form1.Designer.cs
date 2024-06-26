namespace Telegram助手
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            button_start = new Button();
            notifyIcon = new NotifyIcon(components);
            contextMenuStrip = new ContextMenuStrip(components);
            退出ToolStripMenuItem = new ToolStripMenuItem();
            StartRunToolStripMenuItem = new ToolStripMenuItem();
            打开ToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            dataGridView = new DataGridView();
            Column31 = new DataGridViewTextBoxColumn();
            Column32 = new DataGridViewTextBoxColumn();
            Column0 = new DataGridViewTextBoxColumn();
            Column33 = new DataGridViewTextBoxColumn();
            Column11 = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewCheckBoxColumn();
            Column5 = new DataGridViewCheckBoxColumn();
            Column6 = new DataGridViewButtonColumn();
            Column9 = new DataGridViewButtonColumn();
            Column10 = new DataGridViewTextBoxColumn();
            button_creatUser = new Button();
            button_restConfig = new Button();
            button_backupLogin = new Button();
            button_detection = new Button();
            comboBox_changeIp = new ComboBox();
            checkBox_browser = new CheckBox();
            button_collectionGroupMember = new Button();
            checkBox_log = new CheckBox();
            groupBox1 = new GroupBox();
            button_correctionBackups = new Button();
            button_changIp = new Button();
            button_checkFreezeGroup = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            statusStrip3 = new StatusStrip();
            toolStripStatusLabel_miandianGroups = new ToolStripStatusLabel();
            button3 = new Button();
            button2 = new Button();
            button_getCollectionGroups = new Button();
            button_botJoinGroup = new Button();
            dataGridView_collectionGroups = new DataGridView();
            Column12 = new DataGridViewTextBoxColumn();
            Column28 = new DataGridViewTextBoxColumn();
            Column18 = new DataGridViewTextBoxColumn();
            Column19 = new DataGridViewTextBoxColumn();
            Column27 = new DataGridViewButtonColumn();
            tabPage2 = new TabPage();
            statusStrip2 = new StatusStrip();
            toolStripStatusLabel_allGroups = new ToolStripStatusLabel();
            button1 = new Button();
            button_loadSelect = new Button();
            button_getAllGroups = new Button();
            dataGridView_groups = new DataGridView();
            button_checkGroups = new Button();
            Column29 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            Column14 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewButtonColumn();
            contextMenuStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            groupBox1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            statusStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_collectionGroups).BeginInit();
            tabPage2.SuspendLayout();
            statusStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_groups).BeginInit();
            SuspendLayout();
            // 
            // button_start
            // 
            button_start.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_start.Cursor = Cursors.Hand;
            button_start.Location = new Point(804, 52);
            button_start.Name = "button_start";
            button_start.Size = new Size(73, 26);
            button_start.TabIndex = 2;
            button_start.Text = "开始拉人";
            button_start.UseVisualStyleBackColor = true;
            button_start.Click += button_joinUser_Click;
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "左道云控拉人软件 : 双击显示";
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { 退出ToolStripMenuItem, StartRunToolStripMenuItem, 打开ToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip1";
            contextMenuStrip.Size = new Size(149, 70);
            // 
            // 退出ToolStripMenuItem
            // 
            退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            退出ToolStripMenuItem.Size = new Size(148, 22);
            退出ToolStripMenuItem.Text = "退出";
            退出ToolStripMenuItem.Click += 退出ToolStripMenuItem_Click;
            // 
            // StartRunToolStripMenuItem
            // 
            StartRunToolStripMenuItem.Name = "StartRunToolStripMenuItem";
            StartRunToolStripMenuItem.Size = new Size(148, 22);
            StartRunToolStripMenuItem.Text = "关闭开机启动";
            StartRunToolStripMenuItem.Click += StartRunToolStripMenuItem_Click;
            // 
            // 打开ToolStripMenuItem
            // 
            打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            打开ToolStripMenuItem.Size = new Size(148, 22);
            打开ToolStripMenuItem.Text = "打开";
            打开ToolStripMenuItem.Click += 打开ToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.Menu;
            statusStrip1.ImeMode = ImeMode.NoControl;
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(3, 86);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(956, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 8;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Margin = new Padding(0, 0, 0, 2);
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(53, 20);
            toolStripStatusLabel1.Text = "启动中...";
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.BackgroundColor = SystemColors.Control;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column31, Column32, Column0, Column33, Column11, Column1, Column3, Column4, Column5, Column6, Column9, Column10 });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.GridColor = SystemColors.ControlLight;
            dataGridView.ImeMode = ImeMode.NoControl;
            dataGridView.Location = new Point(6, 22);
            dataGridView.MultiSelect = false;
            dataGridView.Name = "dataGridView";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView.RowHeadersVisible = false;
            dataGridView.RowHeadersWidth = 10;
            dataGridView.RowTemplate.Height = 25;
            dataGridView.ScrollBars = ScrollBars.Vertical;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(950, 24);
            dataGridView.TabIndex = 9;
            dataGridView.VirtualMode = true;
            dataGridView.CellContentClick += dataGridView_CellContentClick;
            dataGridView.CellMouseLeave += dataGridView_CellMouseLeave;
            dataGridView.CellMouseMove += dataGridView_CellMouseMove;
            dataGridView.CellValueNeeded += dataGridView_CellValueNeeded;
            dataGridView.SelectionChanged += dataGridView_SelectionChanged;
            // 
            // Column31
            // 
            Column31.HeaderText = "序号";
            Column31.Name = "Column31";
            Column31.Width = 50;
            // 
            // Column32
            // 
            Column32.HeaderText = "Id";
            Column32.Name = "Column32";
            Column32.Width = 90;
            // 
            // Column0
            // 
            Column0.HeaderText = "手机";
            Column0.MinimumWidth = 120;
            Column0.Name = "Column0";
            Column0.ReadOnly = true;
            Column0.Resizable = DataGridViewTriState.False;
            Column0.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column0.Width = 120;
            // 
            // Column33
            // 
            Column33.HeaderText = "用户名";
            Column33.Name = "Column33";
            Column33.Width = 120;
            // 
            // Column11
            // 
            Column11.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column11.HeaderText = "名称";
            Column11.Name = "Column11";
            Column11.ReadOnly = true;
            Column11.Resizable = DataGridViewTriState.False;
            Column11.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column11.Width = 110;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column1.HeaderText = "受限";
            Column1.MinimumWidth = 90;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 90;
            // 
            // Column3
            // 
            Column3.HeaderText = "封禁";
            Column3.MinimumWidth = 40;
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column3.Width = 40;
            // 
            // Column4
            // 
            Column4.HeaderText = "日志";
            Column4.MinimumWidth = 40;
            Column4.Name = "Column4";
            Column4.Resizable = DataGridViewTriState.False;
            Column4.Width = 40;
            // 
            // Column5
            // 
            Column5.HeaderText = "窗口";
            Column5.MinimumWidth = 40;
            Column5.Name = "Column5";
            Column5.Resizable = DataGridViewTriState.False;
            Column5.Width = 40;
            // 
            // Column6
            // 
            Column6.HeaderText = "备份";
            Column6.MinimumWidth = 83;
            Column6.Name = "Column6";
            Column6.Resizable = DataGridViewTriState.False;
            Column6.Width = 83;
            // 
            // Column9
            // 
            Column9.HeaderText = "执行";
            Column9.MinimumWidth = 70;
            Column9.Name = "Column9";
            Column9.Resizable = DataGridViewTriState.False;
            Column9.Width = 70;
            // 
            // Column10
            // 
            Column10.HeaderText = "日拉/总拉";
            Column10.Name = "Column10";
            Column10.Resizable = DataGridViewTriState.False;
            Column10.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column10.Width = 80;
            // 
            // button_creatUser
            // 
            button_creatUser.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_creatUser.Cursor = Cursors.Hand;
            button_creatUser.Location = new Point(409, 52);
            button_creatUser.Name = "button_creatUser";
            button_creatUser.Size = new Size(73, 26);
            button_creatUser.TabIndex = 10;
            button_creatUser.Text = "添加账号";
            button_creatUser.UseVisualStyleBackColor = true;
            button_creatUser.Click += button_creatUser_Click;
            // 
            // button_restConfig
            // 
            button_restConfig.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_restConfig.Cursor = Cursors.Hand;
            button_restConfig.Location = new Point(488, 52);
            button_restConfig.Name = "button_restConfig";
            button_restConfig.Size = new Size(73, 26);
            button_restConfig.TabIndex = 16;
            button_restConfig.Text = "重置数据";
            button_restConfig.UseVisualStyleBackColor = true;
            button_restConfig.Click += button_restConfig_Click;
            // 
            // button_backupLogin
            // 
            button_backupLogin.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_backupLogin.Cursor = Cursors.Hand;
            button_backupLogin.Location = new Point(567, 52);
            button_backupLogin.Name = "button_backupLogin";
            button_backupLogin.Size = new Size(73, 26);
            button_backupLogin.TabIndex = 21;
            button_backupLogin.Text = "备份登录";
            button_backupLogin.UseVisualStyleBackColor = true;
            button_backupLogin.Click += button_backupLogin_Click;
            // 
            // button_detection
            // 
            button_detection.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_detection.Cursor = Cursors.Hand;
            button_detection.Location = new Point(725, 52);
            button_detection.Name = "button_detection";
            button_detection.Size = new Size(73, 26);
            button_detection.TabIndex = 24;
            button_detection.Text = "封号检测";
            button_detection.UseVisualStyleBackColor = true;
            button_detection.Click += button_detection_Click;
            // 
            // comboBox_changeIp
            // 
            comboBox_changeIp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            comboBox_changeIp.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_changeIp.FormattingEnabled = true;
            comboBox_changeIp.Items.AddRange(new object[] { "拨号换动态IP", "免费代理换IP", "付费代理换IP" });
            comboBox_changeIp.Location = new Point(133, 53);
            comboBox_changeIp.Name = "comboBox_changeIp";
            comboBox_changeIp.Size = new Size(95, 25);
            comboBox_changeIp.TabIndex = 25;
            comboBox_changeIp.SelectedIndexChanged += comboBox_changeIp_SelectedIndexChanged;
            // 
            // checkBox_browser
            // 
            checkBox_browser.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBox_browser.AutoSize = true;
            checkBox_browser.Location = new Point(64, 55);
            checkBox_browser.Name = "checkBox_browser";
            checkBox_browser.Size = new Size(63, 21);
            checkBox_browser.TabIndex = 26;
            checkBox_browser.Text = "浏览器";
            checkBox_browser.UseVisualStyleBackColor = true;
            checkBox_browser.CheckedChanged += checkBox_browser_CheckedChanged;
            // 
            // button_collectionGroupMember
            // 
            button_collectionGroupMember.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_collectionGroupMember.Cursor = Cursors.Hand;
            button_collectionGroupMember.Location = new Point(743, 297);
            button_collectionGroupMember.Name = "button_collectionGroupMember";
            button_collectionGroupMember.Size = new Size(89, 26);
            button_collectionGroupMember.TabIndex = 28;
            button_collectionGroupMember.Text = "采集群员";
            button_collectionGroupMember.UseVisualStyleBackColor = true;
            button_collectionGroupMember.Click += button_collectionGroupMember_Click;
            // 
            // checkBox_log
            // 
            checkBox_log.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBox_log.AutoSize = true;
            checkBox_log.Location = new Point(7, 55);
            checkBox_log.Name = "checkBox_log";
            checkBox_log.Size = new Size(51, 21);
            checkBox_log.TabIndex = 29;
            checkBox_log.Text = "日志";
            checkBox_log.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(button_correctionBackups);
            groupBox1.Controls.Add(button_changIp);
            groupBox1.Controls.Add(dataGridView);
            groupBox1.Controls.Add(checkBox_browser);
            groupBox1.Controls.Add(checkBox_log);
            groupBox1.Controls.Add(statusStrip1);
            groupBox1.Controls.Add(comboBox_changeIp);
            groupBox1.Controls.Add(button_detection);
            groupBox1.Controls.Add(button_start);
            groupBox1.Controls.Add(button_backupLogin);
            groupBox1.Controls.Add(button_creatUser);
            groupBox1.Controls.Add(button_restConfig);
            groupBox1.Location = new Point(10, 10);
            groupBox1.Margin = new Padding(1);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(962, 111);
            groupBox1.TabIndex = 30;
            groupBox1.TabStop = false;
            groupBox1.Text = "账号列表";
            // 
            // button_correctionBackups
            // 
            button_correctionBackups.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_correctionBackups.Cursor = Cursors.Hand;
            button_correctionBackups.Location = new Point(646, 52);
            button_correctionBackups.Name = "button_correctionBackups";
            button_correctionBackups.Size = new Size(73, 26);
            button_correctionBackups.TabIndex = 31;
            button_correctionBackups.Text = "校正备份";
            button_correctionBackups.UseVisualStyleBackColor = true;
            button_correctionBackups.Click += button_correctionBackups_Click;
            // 
            // button_changIp
            // 
            button_changIp.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_changIp.Cursor = Cursors.Hand;
            button_changIp.Location = new Point(883, 52);
            button_changIp.Name = "button_changIp";
            button_changIp.Size = new Size(73, 26);
            button_changIp.TabIndex = 30;
            button_changIp.Text = "换IP";
            button_changIp.UseVisualStyleBackColor = true;
            button_changIp.Click += ChangeIp;
            // 
            // button_checkFreezeGroup
            // 
            button_checkFreezeGroup.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_checkFreezeGroup.Cursor = Cursors.Hand;
            button_checkFreezeGroup.Location = new Point(572, 297);
            button_checkFreezeGroup.Name = "button_checkFreezeGroup";
            button_checkFreezeGroup.Size = new Size(135, 26);
            button_checkFreezeGroup.TabIndex = 32;
            button_checkFreezeGroup.Text = "检测无名称链接信息";
            button_checkFreezeGroup.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(10, 125);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(962, 381);
            tabControl1.TabIndex = 31;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(statusStrip3);
            tabPage1.Controls.Add(button3);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(button_getCollectionGroups);
            tabPage1.Controls.Add(button_collectionGroupMember);
            tabPage1.Controls.Add(button_botJoinGroup);
            tabPage1.Controls.Add(dataGridView_collectionGroups);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(954, 351);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "缅甸群";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // statusStrip3
            // 
            statusStrip3.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel_miandianGroups });
            statusStrip3.Location = new Point(3, 326);
            statusStrip3.Name = "statusStrip3";
            statusStrip3.Size = new Size(948, 22);
            statusStrip3.SizingGrip = false;
            statusStrip3.TabIndex = 39;
            statusStrip3.Text = "statusStrip3";
            // 
            // toolStripStatusLabel_miandianGroups
            // 
            toolStripStatusLabel_miandianGroups.Name = "toolStripStatusLabel_miandianGroups";
            toolStripStatusLabel_miandianGroups.Size = new Size(53, 17);
            toolStripStatusLabel_miandianGroups.Text = "启动中...";
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button3.Cursor = Cursors.Hand;
            button3.Location = new Point(609, 297);
            button3.Name = "button3";
            button3.Size = new Size(128, 26);
            button3.TabIndex = 38;
            button3.Text = "全设为未采集群成员";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.Cursor = Cursors.Hand;
            button2.Location = new Point(475, 297);
            button2.Name = "button2";
            button2.Size = new Size(128, 26);
            button2.TabIndex = 37;
            button2.Text = "全设为未导出群信息";
            button2.UseVisualStyleBackColor = true;
            // 
            // button_getCollectionGroups
            // 
            button_getCollectionGroups.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button_getCollectionGroups.Location = new Point(5, 297);
            button_getCollectionGroups.Name = "button_getCollectionGroups";
            button_getCollectionGroups.Size = new Size(79, 26);
            button_getCollectionGroups.TabIndex = 36;
            button_getCollectionGroups.Text = "获取全部群";
            button_getCollectionGroups.UseVisualStyleBackColor = true;
            button_getCollectionGroups.Click += button_getCollectionGroups_Click;
            // 
            // button_botJoinGroup
            // 
            button_botJoinGroup.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_botJoinGroup.Cursor = Cursors.Hand;
            button_botJoinGroup.Location = new Point(838, 297);
            button_botJoinGroup.Name = "button_botJoinGroup";
            button_botJoinGroup.Size = new Size(109, 26);
            button_botJoinGroup.TabIndex = 31;
            button_botJoinGroup.Text = "拉机器人进群";
            button_botJoinGroup.UseVisualStyleBackColor = true;
            button_botJoinGroup.Click += button_botJoinGroup_Click;
            // 
            // dataGridView_collectionGroups
            // 
            dataGridView_collectionGroups.AllowUserToAddRows = false;
            dataGridView_collectionGroups.AllowUserToResizeColumns = false;
            dataGridView_collectionGroups.AllowUserToResizeRows = false;
            dataGridView_collectionGroups.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView_collectionGroups.BackgroundColor = SystemColors.Control;
            dataGridView_collectionGroups.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dataGridView_collectionGroups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView_collectionGroups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView_collectionGroups.Columns.AddRange(new DataGridViewColumn[] { Column12, Column28, Column18, Column19, Column27 });
            dataGridView_collectionGroups.EnableHeadersVisualStyles = false;
            dataGridView_collectionGroups.GridColor = SystemColors.ControlLight;
            dataGridView_collectionGroups.Location = new Point(6, 6);
            dataGridView_collectionGroups.MultiSelect = false;
            dataGridView_collectionGroups.Name = "dataGridView_collectionGroups";
            dataGridView_collectionGroups.RowHeadersVisible = false;
            dataGridView_collectionGroups.RowTemplate.Height = 25;
            dataGridView_collectionGroups.ScrollBars = ScrollBars.Vertical;
            dataGridView_collectionGroups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_collectionGroups.Size = new Size(942, 285);
            dataGridView_collectionGroups.TabIndex = 32;
            dataGridView_collectionGroups.VirtualMode = true;
            dataGridView_collectionGroups.CellContentClick += dataGridView_collectionGroups_CellContentClick;
            dataGridView_collectionGroups.CellMouseLeave += dataGridView_collectionGroups_CellMouseLeave;
            dataGridView_collectionGroups.CellMouseMove += dataGridView_collectionGroups_CellMouseMove;
            dataGridView_collectionGroups.CellValueNeeded += dataGridView_collectionGroups_CellValueNeeded;
            dataGridView_collectionGroups.SelectionChanged += dataGridView_collectionGroups_SelectionChanged;
            // 
            // Column12
            // 
            Column12.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column12.HeaderText = "Id";
            Column12.Name = "Column12";
            Column12.ReadOnly = true;
            Column12.Resizable = DataGridViewTriState.False;
            Column12.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column12.Width = 65;
            // 
            // Column28
            // 
            Column28.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column28.HeaderText = "链接";
            Column28.Name = "Column28";
            Column28.ReadOnly = true;
            Column28.Resizable = DataGridViewTriState.False;
            Column28.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column28.Visible = false;
            // 
            // Column18
            // 
            Column18.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column18.HeaderText = "群名";
            Column18.Name = "Column18";
            Column18.ReadOnly = true;
            Column18.Resizable = DataGridViewTriState.False;
            Column18.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column18.Width = 250;
            // 
            // Column19
            // 
            Column19.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column19.HeaderText = "描述";
            Column19.Name = "Column19";
            Column19.ReadOnly = true;
            Column19.Resizable = DataGridViewTriState.False;
            Column19.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column19.Width = 538;
            // 
            // Column27
            // 
            Column27.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column27.HeaderText = "删除";
            Column27.Name = "Column27";
            Column27.ReadOnly = true;
            Column27.Resizable = DataGridViewTriState.False;
            Column27.Width = 71;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(statusStrip2);
            tabPage2.Controls.Add(button1);
            tabPage2.Controls.Add(button_checkFreezeGroup);
            tabPage2.Controls.Add(button_loadSelect);
            tabPage2.Controls.Add(button_getAllGroups);
            tabPage2.Controls.Add(dataGridView_groups);
            tabPage2.Controls.Add(button_checkGroups);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(954, 351);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "全部群";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // statusStrip2
            // 
            statusStrip2.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel_allGroups });
            statusStrip2.Location = new Point(3, 326);
            statusStrip2.Name = "statusStrip2";
            statusStrip2.Size = new Size(948, 22);
            statusStrip2.SizingGrip = false;
            statusStrip2.TabIndex = 38;
            statusStrip2.Text = "statusStrip2";
            // 
            // toolStripStatusLabel_allGroups
            // 
            toolStripStatusLabel_allGroups.Name = "toolStripStatusLabel_allGroups";
            toolStripStatusLabel_allGroups.Size = new Size(53, 17);
            toolStripStatusLabel_allGroups.Text = "启动中...";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.Cursor = Cursors.Hand;
            button1.Location = new Point(713, 297);
            button1.Name = "button1";
            button1.Size = new Size(124, 26);
            button1.TabIndex = 37;
            button1.Text = "重新扫描ExistUrls";
            button1.UseVisualStyleBackColor = true;
            // 
            // button_loadSelect
            // 
            button_loadSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button_loadSelect.Location = new Point(91, 297);
            button_loadSelect.Name = "button_loadSelect";
            button_loadSelect.Size = new Size(79, 26);
            button_loadSelect.TabIndex = 36;
            button_loadSelect.Text = "开始选择";
            button_loadSelect.UseVisualStyleBackColor = true;
            button_loadSelect.Click += button_loadSelect_Click;
            // 
            // button_getAllGroups
            // 
            button_getAllGroups.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button_getAllGroups.Location = new Point(6, 297);
            button_getAllGroups.Name = "button_getAllGroups";
            button_getAllGroups.Size = new Size(79, 26);
            button_getAllGroups.TabIndex = 35;
            button_getAllGroups.Text = "获取全部群";
            button_getAllGroups.UseVisualStyleBackColor = true;
            button_getAllGroups.Click += button_getAllGroups_ClickAsync;
            // 
            // dataGridView_groups
            // 
            dataGridView_groups.AllowUserToAddRows = false;
            dataGridView_groups.AllowUserToResizeColumns = false;
            dataGridView_groups.AllowUserToResizeRows = false;
            dataGridView_groups.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView_groups.BackgroundColor = SystemColors.Control;
            dataGridView_groups.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dataGridView_groups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridView_groups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView_groups.Columns.AddRange(new DataGridViewColumn[] { Column29, Column8, Column14, Column2 });
            dataGridView_groups.EnableHeadersVisualStyles = false;
            dataGridView_groups.GridColor = SystemColors.ControlLight;
            dataGridView_groups.Location = new Point(6, 6);
            dataGridView_groups.MultiSelect = false;
            dataGridView_groups.Name = "dataGridView_groups";
            dataGridView_groups.RowHeadersVisible = false;
            dataGridView_groups.RowTemplate.Height = 25;
            dataGridView_groups.ScrollBars = ScrollBars.Vertical;
            dataGridView_groups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_groups.Size = new Size(942, 285);
            dataGridView_groups.TabIndex = 34;
            dataGridView_groups.VirtualMode = true;
            dataGridView_groups.CellContentClick += dataGridView_groups_CellContentClick;
            dataGridView_groups.CellMouseLeave += dataGridView_groups_CellMouseLeave;
            dataGridView_groups.CellMouseMove += dataGridView_groups_CellMouseMove;
            dataGridView_groups.CellValueNeeded += dataGridView_groups_CellValueNeeded;
            dataGridView_groups.SelectionChanged += dataGridView_groups_SelectionChanged;
            // 
            // button_checkGroups
            // 
            button_checkGroups.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_checkGroups.Cursor = Cursors.Hand;
            button_checkGroups.Location = new Point(843, 297);
            button_checkGroups.Name = "button_checkGroups";
            button_checkGroups.Size = new Size(105, 26);
            button_checkGroups.TabIndex = 33;
            button_checkGroups.Text = "检查更新群";
            button_checkGroups.UseVisualStyleBackColor = true;
            button_checkGroups.Click += button_checkGroups_Click;
            // 
            // Column29
            // 
            Column29.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column29.HeaderText = "序号";
            Column29.Name = "Column29";
            Column29.ReadOnly = true;
            Column29.Resizable = DataGridViewTriState.False;
            Column29.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column29.Width = 60;
            // 
            // Column8
            // 
            Column8.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column8.HeaderText = "名称";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            Column8.Resizable = DataGridViewTriState.False;
            Column8.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column8.Width = 210;
            // 
            // Column14
            // 
            Column14.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column14.HeaderText = "描述";
            Column14.Name = "Column14";
            Column14.ReadOnly = true;
            Column14.Resizable = DataGridViewTriState.False;
            Column14.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column14.Width = 575;
            // 
            // Column2
            // 
            Column2.HeaderText = "归类";
            Column2.Name = "Column2";
            Column2.Width = 80;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(984, 518);
            Controls.Add(tabControl1);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Telegram助手";
            Load += Form_Load;
            contextMenuStrip.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            statusStrip3.ResumeLayout(false);
            statusStrip3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_collectionGroups).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            statusStrip2.ResumeLayout(false);
            statusStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_groups).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button button_start;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem 退出ToolStripMenuItem;
        private ToolStripMenuItem 打开ToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private DataGridView dataGridView;
        private Button button_creatUser;
        private ToolStripMenuItem StartRunToolStripMenuItem;
        private Button button_restConfig;
        private Button button_backupLogin;
        private Button button_detection;
        private ComboBox comboBox_changeIp;
        private CheckBox checkBox_browser;
        private Button button_collectionGroupMember;
        private CheckBox checkBox_log;
        private GroupBox groupBox1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button button_botJoinGroup;
        private Button button_checkGroups;
        private DataGridView dataGridView_collectionGroups;
        private DataGridView dataGridView_groups;
        private DataGridViewTextBoxColumn Column31;
        private DataGridViewTextBoxColumn Column32;
        private DataGridViewTextBoxColumn Column0;
        private DataGridViewTextBoxColumn Column33;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewCheckBoxColumn Column4;
        private DataGridViewCheckBoxColumn Column5;
        private DataGridViewButtonColumn Column6;
        private DataGridViewButtonColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private Button button_getAllGroups;
        private Button button_getCollectionGroups;
        private Button button_loadSelect;
        private Button button_changIp;
        private Button button_correctionBackups;
        private Button button_checkFreezeGroup;
        private Button button1;
        private DataGridViewTextBoxColumn Column12;
        private DataGridViewTextBoxColumn Column28;
        private DataGridViewTextBoxColumn Column18;
        private DataGridViewTextBoxColumn Column19;
        private DataGridViewButtonColumn Column27;
        private Button button2;
        private Button button3;
        private StatusStrip statusStrip2;
        private StatusStrip statusStrip3;
        private ToolStripStatusLabel toolStripStatusLabel_miandianGroups;
        private ToolStripStatusLabel toolStripStatusLabel_allGroups;
        private DataGridViewTextBoxColumn Column29;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column14;
        private DataGridViewButtonColumn Column2;
    }
}