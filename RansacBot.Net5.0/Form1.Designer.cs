namespace RansacBot.Net5._0
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.qUIKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.OxyChart = new OxyPlot.WindowsForms.PlotView();
            this.tbCurrentPos = new System.Windows.Forms.TextBox();
            this.lblCurrentPos = new System.Windows.Forms.Label();
            this.tbAvailableMax = new System.Windows.Forms.TextBox();
            this.lblAvailableMax = new System.Windows.Forms.Label();
            this.tbAvailableFunds = new System.Windows.Forms.TextBox();
            this.lblAvailableFunds = new System.Windows.Forms.Label();
            this.tbBlock = new System.Windows.Forms.TextBox();
            this.lblBlock = new System.Windows.Forms.Label();
            this.tbBalance = new System.Windows.Forms.TextBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.tbBalanceNoMargin = new System.Windows.Forms.TextBox();
            this.lblBalanceNoMargin = new System.Windows.Forms.Label();
            this.tbVarMargin = new System.Windows.Forms.TextBox();
            this.lblVarMargin = new System.Windows.Forms.Label();
            this.tbGoSell = new System.Windows.Forms.TextBox();
            this.lblGoSell = new System.Windows.Forms.Label();
            this.tbGoBuy = new System.Windows.Forms.TextBox();
            this.lblGoBuy = new System.Windows.Forms.Label();
            this.tbStepPrice = new System.Windows.Forms.TextBox();
            this.lblPriceStep = new System.Windows.Forms.Label();
            this.tbStep = new System.Windows.Forms.TextBox();
            this.lblStep = new System.Windows.Forms.Label();
            this.tbShortName = new System.Windows.Forms.TextBox();
            this.lblShortName = new System.Windows.Forms.Label();
            this.tbSecCode = new System.Windows.Forms.TextBox();
            this.lblSecCode = new System.Windows.Forms.Label();
            this.tbFirmID = new System.Windows.Forms.TextBox();
            this.lblFirmID = new System.Windows.Forms.Label();
            this.tbClassCode = new System.Windows.Forms.TextBox();
            this.lblClassCode = new System.Windows.Forms.Label();
            this.tbAccountID = new System.Windows.Forms.TextBox();
            this.lblAccountID = new System.Windows.Forms.Label();
            this.tbClientCode = new System.Windows.Forms.TextBox();
            this.lblClientCode = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.cbRansac = new System.Windows.Forms.ComboBox();
            this.nmcLevelRansac = new System.Windows.Forms.NumericUpDown();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmcLevelRansac)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.qUIKToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1424, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // qUIKToolStripMenuItem
            // 
            this.qUIKToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoginToolStripMenuItem,
            this.OnToolStripMenuItem});
            this.qUIKToolStripMenuItem.Name = "qUIKToolStripMenuItem";
            this.qUIKToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.qUIKToolStripMenuItem.Text = "QUIK";
            // 
            // LoginToolStripMenuItem
            // 
            this.LoginToolStripMenuItem.Name = "LoginToolStripMenuItem";
            this.LoginToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.LoginToolStripMenuItem.Text = "Подключиться";
            this.LoginToolStripMenuItem.Click += new System.EventHandler(this.LoginToolStripMenuItem_Click);
            // 
            // OnToolStripMenuItem
            // 
            this.OnToolStripMenuItem.Enabled = false;
            this.OnToolStripMenuItem.Name = "OnToolStripMenuItem";
            this.OnToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.OnToolStripMenuItem.Text = "Включить бот";
            this.OnToolStripMenuItem.Click += new System.EventHandler(this.OnToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton,
            this.toolStripStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 637);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1424, 24);
            this.statusStrip.TabIndex = 1;
            // 
            // toolStripSplitButton
            // 
            this.toolStripSplitButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton.Name = "toolStripSplitButton";
            this.toolStripSplitButton.RightToLeftAutoMirrorImage = true;
            this.toolStripSplitButton.Size = new System.Drawing.Size(60, 22);
            this.toolStripSplitButton.Text = "Logger";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.toolStripStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.toolStripStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripStatus.Size = new System.Drawing.Size(66, 19);
            this.toolStripStatus.Text = "RansacBot";
            // 
            // OxyChart
            // 
            this.OxyChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OxyChart.BackColor = System.Drawing.SystemColors.Control;
            this.OxyChart.Location = new System.Drawing.Point(240, 27);
            this.OxyChart.Name = "OxyChart";
            this.OxyChart.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.OxyChart.Size = new System.Drawing.Size(1184, 609);
            this.OxyChart.TabIndex = 4;
            this.OxyChart.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.OxyChart.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.OxyChart.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tbCurrentPos
            // 
            this.tbCurrentPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbCurrentPos.Enabled = false;
            this.tbCurrentPos.Location = new System.Drawing.Point(134, 467);
            this.tbCurrentPos.Name = "tbCurrentPos";
            this.tbCurrentPos.Size = new System.Drawing.Size(100, 23);
            this.tbCurrentPos.TabIndex = 76;
            this.tbCurrentPos.TabStop = false;
            // 
            // lblCurrentPos
            // 
            this.lblCurrentPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrentPos.AutoSize = true;
            this.lblCurrentPos.Location = new System.Drawing.Point(12, 470);
            this.lblCurrentPos.Name = "lblCurrentPos";
            this.lblCurrentPos.Size = new System.Drawing.Size(106, 15);
            this.lblCurrentPos.TabIndex = 75;
            this.lblCurrentPos.Text = "Текущая позиция:";
            // 
            // tbAvailableMax
            // 
            this.tbAvailableMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbAvailableMax.Enabled = false;
            this.tbAvailableMax.Location = new System.Drawing.Point(134, 438);
            this.tbAvailableMax.Name = "tbAvailableMax";
            this.tbAvailableMax.Size = new System.Drawing.Size(100, 23);
            this.tbAvailableMax.TabIndex = 74;
            this.tbAvailableMax.TabStop = false;
            // 
            // lblAvailableMax
            // 
            this.lblAvailableMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAvailableMax.AutoSize = true;
            this.lblAvailableMax.Location = new System.Drawing.Point(12, 441);
            this.lblAvailableMax.Name = "lblAvailableMax";
            this.lblAvailableMax.Size = new System.Drawing.Size(96, 15);
            this.lblAvailableMax.TabIndex = 73;
            this.lblAvailableMax.Text = "Макс. доступно:";
            // 
            // tbAvailableFunds
            // 
            this.tbAvailableFunds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbAvailableFunds.Enabled = false;
            this.tbAvailableFunds.Location = new System.Drawing.Point(134, 613);
            this.tbAvailableFunds.Name = "tbAvailableFunds";
            this.tbAvailableFunds.Size = new System.Drawing.Size(100, 23);
            this.tbAvailableFunds.TabIndex = 72;
            this.tbAvailableFunds.TabStop = false;
            // 
            // lblAvailableFunds
            // 
            this.lblAvailableFunds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAvailableFunds.AutoSize = true;
            this.lblAvailableFunds.Location = new System.Drawing.Point(12, 616);
            this.lblAvailableFunds.Name = "lblAvailableFunds";
            this.lblAvailableFunds.Size = new System.Drawing.Size(108, 15);
            this.lblAvailableFunds.TabIndex = 71;
            this.lblAvailableFunds.Text = "Доступно средств:";
            // 
            // tbBlock
            // 
            this.tbBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbBlock.Enabled = false;
            this.tbBlock.Location = new System.Drawing.Point(134, 584);
            this.tbBlock.Name = "tbBlock";
            this.tbBlock.Size = new System.Drawing.Size(100, 23);
            this.tbBlock.TabIndex = 70;
            this.tbBlock.TabStop = false;
            // 
            // lblBlock
            // 
            this.lblBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBlock.AutoSize = true;
            this.lblBlock.Location = new System.Drawing.Point(12, 587);
            this.lblBlock.Name = "lblBlock";
            this.lblBlock.Size = new System.Drawing.Size(97, 15);
            this.lblBlock.TabIndex = 69;
            this.lblBlock.Text = "Заблокировано:";
            // 
            // tbBalance
            // 
            this.tbBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbBalance.Enabled = false;
            this.tbBalance.Location = new System.Drawing.Point(134, 555);
            this.tbBalance.Name = "tbBalance";
            this.tbBalance.Size = new System.Drawing.Size(100, 23);
            this.tbBalance.TabIndex = 68;
            this.tbBalance.TabStop = false;
            // 
            // lblBalance
            // 
            this.lblBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(12, 558);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(49, 15);
            this.lblBalance.TabIndex = 67;
            this.lblBalance.Text = "Баланс:";
            // 
            // tbBalanceNoMargin
            // 
            this.tbBalanceNoMargin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbBalanceNoMargin.Enabled = false;
            this.tbBalanceNoMargin.Location = new System.Drawing.Point(134, 526);
            this.tbBalanceNoMargin.Name = "tbBalanceNoMargin";
            this.tbBalanceNoMargin.Size = new System.Drawing.Size(100, 23);
            this.tbBalanceNoMargin.TabIndex = 66;
            this.tbBalanceNoMargin.TabStop = false;
            // 
            // lblBalanceNoMargin
            // 
            this.lblBalanceNoMargin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBalanceNoMargin.AutoSize = true;
            this.lblBalanceNoMargin.Location = new System.Drawing.Point(12, 529);
            this.lblBalanceNoMargin.Name = "lblBalanceNoMargin";
            this.lblBalanceNoMargin.Size = new System.Drawing.Size(110, 15);
            this.lblBalanceNoMargin.TabIndex = 65;
            this.lblBalanceNoMargin.Text = "Балант без маржи:";
            // 
            // tbVarMargin
            // 
            this.tbVarMargin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbVarMargin.Enabled = false;
            this.tbVarMargin.Location = new System.Drawing.Point(134, 497);
            this.tbVarMargin.Name = "tbVarMargin";
            this.tbVarMargin.Size = new System.Drawing.Size(100, 23);
            this.tbVarMargin.TabIndex = 64;
            this.tbVarMargin.TabStop = false;
            // 
            // lblVarMargin
            // 
            this.lblVarMargin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVarMargin.AutoSize = true;
            this.lblVarMargin.Location = new System.Drawing.Point(12, 500);
            this.lblVarMargin.Name = "lblVarMargin";
            this.lblVarMargin.Size = new System.Drawing.Size(75, 15);
            this.lblVarMargin.TabIndex = 63;
            this.lblVarMargin.Text = "Вар. Маржа:";
            // 
            // tbGoSell
            // 
            this.tbGoSell.Enabled = false;
            this.tbGoSell.Location = new System.Drawing.Point(134, 288);
            this.tbGoSell.Name = "tbGoSell";
            this.tbGoSell.Size = new System.Drawing.Size(100, 23);
            this.tbGoSell.TabIndex = 62;
            this.tbGoSell.TabStop = false;
            // 
            // lblGoSell
            // 
            this.lblGoSell.AutoSize = true;
            this.lblGoSell.Location = new System.Drawing.Point(12, 291);
            this.lblGoSell.Name = "lblGoSell";
            this.lblGoSell.Size = new System.Drawing.Size(80, 15);
            this.lblGoSell.TabIndex = 61;
            this.lblGoSell.Text = "ГО продавца:";
            // 
            // tbGoBuy
            // 
            this.tbGoBuy.Enabled = false;
            this.tbGoBuy.Location = new System.Drawing.Point(134, 259);
            this.tbGoBuy.Name = "tbGoBuy";
            this.tbGoBuy.Size = new System.Drawing.Size(100, 23);
            this.tbGoBuy.TabIndex = 60;
            this.tbGoBuy.TabStop = false;
            // 
            // lblGoBuy
            // 
            this.lblGoBuy.AutoSize = true;
            this.lblGoBuy.Location = new System.Drawing.Point(12, 262);
            this.lblGoBuy.Name = "lblGoBuy";
            this.lblGoBuy.Size = new System.Drawing.Size(91, 15);
            this.lblGoBuy.TabIndex = 59;
            this.lblGoBuy.Text = "ГО покупателя:";
            // 
            // tbStepPrice
            // 
            this.tbStepPrice.Enabled = false;
            this.tbStepPrice.Location = new System.Drawing.Point(134, 230);
            this.tbStepPrice.Name = "tbStepPrice";
            this.tbStepPrice.Size = new System.Drawing.Size(100, 23);
            this.tbStepPrice.TabIndex = 58;
            this.tbStepPrice.TabStop = false;
            // 
            // lblPriceStep
            // 
            this.lblPriceStep.AutoSize = true;
            this.lblPriceStep.Location = new System.Drawing.Point(12, 233);
            this.lblPriceStep.Name = "lblPriceStep";
            this.lblPriceStep.Size = new System.Drawing.Size(101, 15);
            this.lblPriceStep.TabIndex = 57;
            this.lblPriceStep.Text = "Стоимость шага:";
            // 
            // tbStep
            // 
            this.tbStep.Enabled = false;
            this.tbStep.Location = new System.Drawing.Point(134, 201);
            this.tbStep.Name = "tbStep";
            this.tbStep.Size = new System.Drawing.Size(100, 23);
            this.tbStep.TabIndex = 56;
            this.tbStep.TabStop = false;
            // 
            // lblStep
            // 
            this.lblStep.AutoSize = true;
            this.lblStep.Location = new System.Drawing.Point(12, 204);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(64, 15);
            this.lblStep.TabIndex = 55;
            this.lblStep.Text = "Шаг цены:";
            // 
            // tbShortName
            // 
            this.tbShortName.Enabled = false;
            this.tbShortName.Location = new System.Drawing.Point(134, 172);
            this.tbShortName.Name = "tbShortName";
            this.tbShortName.Size = new System.Drawing.Size(100, 23);
            this.tbShortName.TabIndex = 54;
            this.tbShortName.TabStop = false;
            // 
            // lblShortName
            // 
            this.lblShortName.AutoSize = true;
            this.lblShortName.Location = new System.Drawing.Point(12, 175);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(115, 15);
            this.lblShortName.TabIndex = 53;
            this.lblShortName.Text = "Короткое название:";
            // 
            // tbSecCode
            // 
            this.tbSecCode.Enabled = false;
            this.tbSecCode.Location = new System.Drawing.Point(134, 143);
            this.tbSecCode.Name = "tbSecCode";
            this.tbSecCode.Size = new System.Drawing.Size(100, 23);
            this.tbSecCode.TabIndex = 52;
            this.tbSecCode.TabStop = false;
            // 
            // lblSecCode
            // 
            this.lblSecCode.AutoSize = true;
            this.lblSecCode.Location = new System.Drawing.Point(12, 146);
            this.lblSecCode.Name = "lblSecCode";
            this.lblSecCode.Size = new System.Drawing.Size(42, 15);
            this.lblSecCode.TabIndex = 51;
            this.lblSecCode.Text = "Тикер:";
            // 
            // tbFirmID
            // 
            this.tbFirmID.Enabled = false;
            this.tbFirmID.Location = new System.Drawing.Point(134, 114);
            this.tbFirmID.Name = "tbFirmID";
            this.tbFirmID.Size = new System.Drawing.Size(100, 23);
            this.tbFirmID.TabIndex = 50;
            this.tbFirmID.TabStop = false;
            // 
            // lblFirmID
            // 
            this.lblFirmID.AutoSize = true;
            this.lblFirmID.Location = new System.Drawing.Point(12, 117);
            this.lblFirmID.Name = "lblFirmID";
            this.lblFirmID.Size = new System.Drawing.Size(48, 15);
            this.lblFirmID.TabIndex = 49;
            this.lblFirmID.Text = "Фирма:";
            // 
            // tbClassCode
            // 
            this.tbClassCode.Enabled = false;
            this.tbClassCode.Location = new System.Drawing.Point(134, 85);
            this.tbClassCode.Name = "tbClassCode";
            this.tbClassCode.Size = new System.Drawing.Size(100, 23);
            this.tbClassCode.TabIndex = 48;
            this.tbClassCode.TabStop = false;
            // 
            // lblClassCode
            // 
            this.lblClassCode.AutoSize = true;
            this.lblClassCode.Location = new System.Drawing.Point(12, 88);
            this.lblClassCode.Name = "lblClassCode";
            this.lblClassCode.Size = new System.Drawing.Size(70, 15);
            this.lblClassCode.TabIndex = 47;
            this.lblClassCode.Text = "Код класса:";
            // 
            // tbAccountID
            // 
            this.tbAccountID.Enabled = false;
            this.tbAccountID.Location = new System.Drawing.Point(134, 56);
            this.tbAccountID.Name = "tbAccountID";
            this.tbAccountID.Size = new System.Drawing.Size(100, 23);
            this.tbAccountID.TabIndex = 46;
            this.tbAccountID.TabStop = false;
            // 
            // lblAccountID
            // 
            this.lblAccountID.AutoSize = true;
            this.lblAccountID.Location = new System.Drawing.Point(12, 59);
            this.lblAccountID.Name = "lblAccountID";
            this.lblAccountID.Size = new System.Drawing.Size(103, 15);
            this.lblAccountID.TabIndex = 45;
            this.lblAccountID.Text = "Номер счета (ID):";
            // 
            // tbClientCode
            // 
            this.tbClientCode.Enabled = false;
            this.tbClientCode.Location = new System.Drawing.Point(134, 27);
            this.tbClientCode.Name = "tbClientCode";
            this.tbClientCode.Size = new System.Drawing.Size(100, 23);
            this.tbClientCode.TabIndex = 44;
            this.tbClientCode.TabStop = false;
            // 
            // lblClientCode
            // 
            this.lblClientCode.AutoSize = true;
            this.lblClientCode.Location = new System.Drawing.Point(12, 30);
            this.lblClientCode.Name = "lblClientCode";
            this.lblClientCode.Size = new System.Drawing.Size(77, 15);
            this.lblClientCode.TabIndex = 43;
            this.lblClientCode.Text = "Код клиента:";
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(12, 428);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(0, 15);
            this.labelState.TabIndex = 42;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(134, 317);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 77;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // cbRansac
            // 
            this.cbRansac.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbRansac.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRansac.FormattingEnabled = true;
            this.cbRansac.Location = new System.Drawing.Point(12, 408);
            this.cbRansac.Name = "cbRansac";
            this.cbRansac.Size = new System.Drawing.Size(147, 23);
            this.cbRansac.TabIndex = 79;
            this.cbRansac.Visible = false;
            this.cbRansac.SelectedIndexChanged += new System.EventHandler(this.CbRansac_SelectedIndexChanged);
            // 
            // nmcLevelRansac
            // 
            this.nmcLevelRansac.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmcLevelRansac.Location = new System.Drawing.Point(165, 409);
            this.nmcLevelRansac.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nmcLevelRansac.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmcLevelRansac.Name = "nmcLevelRansac";
            this.nmcLevelRansac.Size = new System.Drawing.Size(69, 23);
            this.nmcLevelRansac.TabIndex = 78;
            this.nmcLevelRansac.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nmcLevelRansac.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmcLevelRansac.Visible = false;
            this.nmcLevelRansac.ValueChanged += new System.EventHandler(this.NmcLevelRansac_ValueChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 661);
            this.Controls.Add(this.cbRansac);
            this.Controls.Add(this.nmcLevelRansac);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbCurrentPos);
            this.Controls.Add(this.lblCurrentPos);
            this.Controls.Add(this.tbAvailableMax);
            this.Controls.Add(this.lblAvailableMax);
            this.Controls.Add(this.tbAvailableFunds);
            this.Controls.Add(this.lblAvailableFunds);
            this.Controls.Add(this.tbBlock);
            this.Controls.Add(this.lblBlock);
            this.Controls.Add(this.tbBalance);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.tbBalanceNoMargin);
            this.Controls.Add(this.lblBalanceNoMargin);
            this.Controls.Add(this.tbVarMargin);
            this.Controls.Add(this.lblVarMargin);
            this.Controls.Add(this.tbGoSell);
            this.Controls.Add(this.lblGoSell);
            this.Controls.Add(this.tbGoBuy);
            this.Controls.Add(this.lblGoBuy);
            this.Controls.Add(this.tbStepPrice);
            this.Controls.Add(this.lblPriceStep);
            this.Controls.Add(this.tbStep);
            this.Controls.Add(this.lblStep);
            this.Controls.Add(this.tbShortName);
            this.Controls.Add(this.lblShortName);
            this.Controls.Add(this.tbSecCode);
            this.Controls.Add(this.lblSecCode);
            this.Controls.Add(this.tbFirmID);
            this.Controls.Add(this.lblFirmID);
            this.Controls.Add(this.tbClassCode);
            this.Controls.Add(this.lblClassCode);
            this.Controls.Add(this.tbAccountID);
            this.Controls.Add(this.lblAccountID);
            this.Controls.Add(this.tbClientCode);
            this.Controls.Add(this.lblClientCode);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.OxyChart);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(1440, 700);
            this.Name = "FormMain";
            this.Text = "RansacBot 5.0";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmcLevelRansac)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem qUIKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OnToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private OxyPlot.WindowsForms.PlotView OxyChart;
        private System.Windows.Forms.TextBox tbCurrentPos;
        private System.Windows.Forms.Label lblCurrentPos;
        private System.Windows.Forms.TextBox tbAvailableMax;
        private System.Windows.Forms.Label lblAvailableMax;
        private System.Windows.Forms.TextBox tbAvailableFunds;
        private System.Windows.Forms.Label lblAvailableFunds;
        private System.Windows.Forms.TextBox tbBlock;
        private System.Windows.Forms.Label lblBlock;
        private System.Windows.Forms.TextBox tbBalance;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.TextBox tbBalanceNoMargin;
        private System.Windows.Forms.Label lblBalanceNoMargin;
        private System.Windows.Forms.TextBox tbVarMargin;
        private System.Windows.Forms.Label lblVarMargin;
        private System.Windows.Forms.TextBox tbGoSell;
        private System.Windows.Forms.Label lblGoSell;
        private System.Windows.Forms.TextBox tbGoBuy;
        private System.Windows.Forms.Label lblGoBuy;
        private System.Windows.Forms.TextBox tbStepPrice;
        private System.Windows.Forms.Label lblPriceStep;
        private System.Windows.Forms.TextBox tbStep;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.TextBox tbShortName;
        private System.Windows.Forms.Label lblShortName;
        private System.Windows.Forms.TextBox tbSecCode;
        private System.Windows.Forms.Label lblSecCode;
        private System.Windows.Forms.TextBox tbFirmID;
        private System.Windows.Forms.Label lblFirmID;
        private System.Windows.Forms.TextBox tbClassCode;
        private System.Windows.Forms.Label lblClassCode;
        private System.Windows.Forms.TextBox tbAccountID;
        private System.Windows.Forms.Label lblAccountID;
        private System.Windows.Forms.TextBox tbClientCode;
        private System.Windows.Forms.Label lblClientCode;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton;
        private System.Windows.Forms.ComboBox cbRansac;
        private System.Windows.Forms.NumericUpDown nmcLevelRansac;
    }
}
