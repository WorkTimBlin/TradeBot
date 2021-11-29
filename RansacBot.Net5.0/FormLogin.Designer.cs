namespace RansacBot.Net5._0
{
    partial class FormLogin
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblError = new System.Windows.Forms.Label();
            this.gbStrategySettings = new System.Windows.Forms.GroupBox();
            this.nmcPercentCloseN2 = new System.Windows.Forms.NumericUpDown();
            this.nmcPercentCloseN1 = new System.Windows.Forms.NumericUpDown();
            this.lblPercent = new System.Windows.Forms.Label();
            this.nmcN = new System.Windows.Forms.NumericUpDown();
            this.lblN = new System.Windows.Forms.Label();
            this.cbCloseN2 = new System.Windows.Forms.ComboBox();
            this.cbCloseN1 = new System.Windows.Forms.ComboBox();
            this.cbFilterOR = new System.Windows.Forms.ComboBox();
            this.cbFilterMMR = new System.Windows.Forms.ComboBox();
            this.nmcLevelCloseN2 = new System.Windows.Forms.NumericUpDown();
            this.nmcLevelCloseN1 = new System.Windows.Forms.NumericUpDown();
            this.nmcLevelOR = new System.Windows.Forms.NumericUpDown();
            this.lblTypeSigma = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblCloseN1 = new System.Windows.Forms.Label();
            this.lblFilterOR = new System.Windows.Forms.Label();
            this.nmcLevelMMR = new System.Windows.Forms.NumericUpDown();
            this.lblFilterMMR = new System.Windows.Forms.Label();
            this.lblCloseN2 = new System.Windows.Forms.Label();
            this.gbQuikSettings = new System.Windows.Forms.GroupBox();
            this.cbClassCode = new System.Windows.Forms.ComboBox();
            this.cbTools = new System.Windows.Forms.ComboBox();
            this.lblTools = new System.Windows.Forms.Label();
            this.cbFirmID = new System.Windows.Forms.ComboBox();
            this.lblFirmID = new System.Windows.Forms.Label();
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.cbAccountID = new System.Windows.Forms.ComboBox();
            this.lblAccountId = new System.Windows.Forms.Label();
            this.cbClientCode = new System.Windows.Forms.ComboBox();
            this.lblClassCode = new System.Windows.Forms.Label();
            this.lblClientCode = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.btnCansel = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.gbStrategySettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmcPercentCloseN2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcPercentCloseN1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcLevelCloseN2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcLevelCloseN1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcLevelOR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcLevelMMR)).BeginInit();
            this.gbQuikSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.lblError);
            this.panel1.Controls.Add(this.gbStrategySettings);
            this.panel1.Controls.Add(this.gbQuikSettings);
            this.panel1.Controls.Add(this.btnCansel);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 412);
            this.panel1.TabIndex = 9;
            // 
            // lblError
            // 
            this.lblError.Location = new System.Drawing.Point(12, 345);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(279, 51);
            this.lblError.TabIndex = 14;
            // 
            // gbStrategySettings
            // 
            this.gbStrategySettings.Controls.Add(this.nmcPercentCloseN2);
            this.gbStrategySettings.Controls.Add(this.nmcPercentCloseN1);
            this.gbStrategySettings.Controls.Add(this.lblPercent);
            this.gbStrategySettings.Controls.Add(this.nmcN);
            this.gbStrategySettings.Controls.Add(this.lblN);
            this.gbStrategySettings.Controls.Add(this.cbCloseN2);
            this.gbStrategySettings.Controls.Add(this.cbCloseN1);
            this.gbStrategySettings.Controls.Add(this.cbFilterOR);
            this.gbStrategySettings.Controls.Add(this.cbFilterMMR);
            this.gbStrategySettings.Controls.Add(this.nmcLevelCloseN2);
            this.gbStrategySettings.Controls.Add(this.nmcLevelCloseN1);
            this.gbStrategySettings.Controls.Add(this.nmcLevelOR);
            this.gbStrategySettings.Controls.Add(this.lblTypeSigma);
            this.gbStrategySettings.Controls.Add(this.lblLevel);
            this.gbStrategySettings.Controls.Add(this.lblCloseN1);
            this.gbStrategySettings.Controls.Add(this.lblFilterOR);
            this.gbStrategySettings.Controls.Add(this.nmcLevelMMR);
            this.gbStrategySettings.Controls.Add(this.lblFilterMMR);
            this.gbStrategySettings.Controls.Add(this.lblCloseN2);
            this.gbStrategySettings.Location = new System.Drawing.Point(12, 151);
            this.gbStrategySettings.Name = "gbStrategySettings";
            this.gbStrategySettings.Size = new System.Drawing.Size(441, 191);
            this.gbStrategySettings.TabIndex = 13;
            this.gbStrategySettings.TabStop = false;
            this.gbStrategySettings.Text = "Настройки стратегии";
            // 
            // nmcPercentCloseN2
            // 
            this.nmcPercentCloseN2.DecimalPlaces = 1;
            this.nmcPercentCloseN2.Enabled = false;
            this.nmcPercentCloseN2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nmcPercentCloseN2.Location = new System.Drawing.Point(371, 124);
            this.nmcPercentCloseN2.Name = "nmcPercentCloseN2";
            this.nmcPercentCloseN2.Size = new System.Drawing.Size(56, 23);
            this.nmcPercentCloseN2.TabIndex = 29;
            this.nmcPercentCloseN2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nmcPercentCloseN2.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // nmcPercentCloseN1
            // 
            this.nmcPercentCloseN1.DecimalPlaces = 1;
            this.nmcPercentCloseN1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nmcPercentCloseN1.Location = new System.Drawing.Point(371, 95);
            this.nmcPercentCloseN1.Name = "nmcPercentCloseN1";
            this.nmcPercentCloseN1.Size = new System.Drawing.Size(56, 23);
            this.nmcPercentCloseN1.TabIndex = 28;
            this.nmcPercentCloseN1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nmcPercentCloseN1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(371, 19);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(58, 15);
            this.lblPercent.TabIndex = 27;
            this.lblPercent.Text = "Процент:";
            // 
            // nmcN
            // 
            this.nmcN.Enabled = false;
            this.nmcN.Location = new System.Drawing.Point(156, 154);
            this.nmcN.Name = "nmcN";
            this.nmcN.Size = new System.Drawing.Size(70, 23);
            this.nmcN.TabIndex = 26;
            this.nmcN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblN
            // 
            this.lblN.AutoSize = true;
            this.lblN.Location = new System.Drawing.Point(6, 156);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(77, 15);
            this.lblN.TabIndex = 25;
            this.lblN.Text = "Параметр N:";
            // 
            // cbCloseN2
            // 
            this.cbCloseN2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCloseN2.FormattingEnabled = true;
            this.cbCloseN2.Items.AddRange(new object[] {
            "ConfidenceInterval-90",
            "ErrorThreshold",
            "Sigma",
            "SigmaInliers"});
            this.cbCloseN2.Location = new System.Drawing.Point(156, 124);
            this.cbCloseN2.Name = "cbCloseN2";
            this.cbCloseN2.Size = new System.Drawing.Size(147, 23);
            this.cbCloseN2.TabIndex = 24;
            // 
            // cbCloseN1
            // 
            this.cbCloseN1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCloseN1.FormattingEnabled = true;
            this.cbCloseN1.Items.AddRange(new object[] {
            "ConfidenceInterval-90",
            "ErrorThreshold",
            "Sigma",
            "SigmaInliers"});
            this.cbCloseN1.Location = new System.Drawing.Point(156, 95);
            this.cbCloseN1.Name = "cbCloseN1";
            this.cbCloseN1.Size = new System.Drawing.Size(147, 23);
            this.cbCloseN1.TabIndex = 23;
            // 
            // cbFilterOR
            // 
            this.cbFilterOR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilterOR.FormattingEnabled = true;
            this.cbFilterOR.Items.AddRange(new object[] {
            "ConfidenceInterval-90",
            "ErrorThreshold",
            "Sigma",
            "SigmaInliers"});
            this.cbFilterOR.Location = new System.Drawing.Point(156, 66);
            this.cbFilterOR.Name = "cbFilterOR";
            this.cbFilterOR.Size = new System.Drawing.Size(147, 23);
            this.cbFilterOR.TabIndex = 22;
            // 
            // cbFilterMMR
            // 
            this.cbFilterMMR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilterMMR.FormattingEnabled = true;
            this.cbFilterMMR.Items.AddRange(new object[] {
            "ConfidenceInterval-90",
            "ErrorThreshold",
            "Sigma",
            "SigmaInliers"});
            this.cbFilterMMR.Location = new System.Drawing.Point(156, 37);
            this.cbFilterMMR.Name = "cbFilterMMR";
            this.cbFilterMMR.Size = new System.Drawing.Size(147, 23);
            this.cbFilterMMR.TabIndex = 21;
            // 
            // nmcLevelCloseN2
            // 
            this.nmcLevelCloseN2.Location = new System.Drawing.Point(309, 124);
            this.nmcLevelCloseN2.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nmcLevelCloseN2.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmcLevelCloseN2.Name = "nmcLevelCloseN2";
            this.nmcLevelCloseN2.Size = new System.Drawing.Size(56, 23);
            this.nmcLevelCloseN2.TabIndex = 19;
            this.nmcLevelCloseN2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nmcLevelCloseN2.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nmcLevelCloseN1
            // 
            this.nmcLevelCloseN1.Location = new System.Drawing.Point(309, 95);
            this.nmcLevelCloseN1.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nmcLevelCloseN1.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmcLevelCloseN1.Name = "nmcLevelCloseN1";
            this.nmcLevelCloseN1.Size = new System.Drawing.Size(56, 23);
            this.nmcLevelCloseN1.TabIndex = 17;
            this.nmcLevelCloseN1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nmcLevelCloseN1.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // nmcLevelOR
            // 
            this.nmcLevelOR.Location = new System.Drawing.Point(309, 66);
            this.nmcLevelOR.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nmcLevelOR.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmcLevelOR.Name = "nmcLevelOR";
            this.nmcLevelOR.Size = new System.Drawing.Size(56, 23);
            this.nmcLevelOR.TabIndex = 15;
            this.nmcLevelOR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nmcLevelOR.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // lblTypeSigma
            // 
            this.lblTypeSigma.AutoSize = true;
            this.lblTypeSigma.Location = new System.Drawing.Point(156, 20);
            this.lblTypeSigma.Name = "lblTypeSigma";
            this.lblTypeSigma.Size = new System.Drawing.Size(69, 15);
            this.lblTypeSigma.TabIndex = 13;
            this.lblTypeSigma.Text = "Тип сигмы:";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(309, 19);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(56, 15);
            this.lblLevel.TabIndex = 12;
            this.lblLevel.Text = "Уровень:";
            // 
            // lblCloseN1
            // 
            this.lblCloseN1.AutoSize = true;
            this.lblCloseN1.Location = new System.Drawing.Point(6, 98);
            this.lblCloseN1.Name = "lblCloseN1";
            this.lblCloseN1.Size = new System.Drawing.Size(145, 15);
            this.lblCloseN1.TabIndex = 10;
            this.lblCloseN1.Text = "Ранзак для закрытия №1:";
            // 
            // lblFilterOR
            // 
            this.lblFilterOR.AutoSize = true;
            this.lblFilterOR.Location = new System.Drawing.Point(6, 69);
            this.lblFilterOR.Name = "lblFilterOR";
            this.lblFilterOR.Size = new System.Drawing.Size(111, 15);
            this.lblFilterOR.TabIndex = 8;
            this.lblFilterOR.Text = "Фильтр OutRansac:";
            // 
            // nmcLevelMMR
            // 
            this.nmcLevelMMR.Location = new System.Drawing.Point(309, 37);
            this.nmcLevelMMR.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nmcLevelMMR.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmcLevelMMR.Name = "nmcLevelMMR";
            this.nmcLevelMMR.Size = new System.Drawing.Size(56, 23);
            this.nmcLevelMMR.TabIndex = 7;
            this.nmcLevelMMR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nmcLevelMMR.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lblFilterMMR
            // 
            this.lblFilterMMR.AutoSize = true;
            this.lblFilterMMR.Location = new System.Drawing.Point(6, 40);
            this.lblFilterMMR.Name = "lblFilterMMR";
            this.lblFilterMMR.Size = new System.Drawing.Size(135, 15);
            this.lblFilterMMR.TabIndex = 6;
            this.lblFilterMMR.Text = "Фильтр MaxMinRansac:";
            // 
            // lblCloseN2
            // 
            this.lblCloseN2.AutoSize = true;
            this.lblCloseN2.Location = new System.Drawing.Point(6, 127);
            this.lblCloseN2.Name = "lblCloseN2";
            this.lblCloseN2.Size = new System.Drawing.Size(145, 15);
            this.lblCloseN2.TabIndex = 4;
            this.lblCloseN2.Text = "Ранзак для закрытия №2:";
            // 
            // gbQuikSettings
            // 
            this.gbQuikSettings.Controls.Add(this.cbClassCode);
            this.gbQuikSettings.Controls.Add(this.cbTools);
            this.gbQuikSettings.Controls.Add(this.lblTools);
            this.gbQuikSettings.Controls.Add(this.cbFirmID);
            this.gbQuikSettings.Controls.Add(this.lblFirmID);
            this.gbQuikSettings.Controls.Add(this.cbPort);
            this.gbQuikSettings.Controls.Add(this.cbAccountID);
            this.gbQuikSettings.Controls.Add(this.lblAccountId);
            this.gbQuikSettings.Controls.Add(this.cbClientCode);
            this.gbQuikSettings.Controls.Add(this.lblClassCode);
            this.gbQuikSettings.Controls.Add(this.lblClientCode);
            this.gbQuikSettings.Controls.Add(this.lblPort);
            this.gbQuikSettings.Location = new System.Drawing.Point(12, 12);
            this.gbQuikSettings.Name = "gbQuikSettings";
            this.gbQuikSettings.Size = new System.Drawing.Size(440, 133);
            this.gbQuikSettings.TabIndex = 12;
            this.gbQuikSettings.TabStop = false;
            this.gbQuikSettings.Text = "Настройки соединения";
            // 
            // cbClassCode
            // 
            this.cbClassCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClassCode.FormattingEnabled = true;
            this.cbClassCode.Items.AddRange(new object[] {
            ""});
            this.cbClassCode.Location = new System.Drawing.Point(131, 50);
            this.cbClassCode.Name = "cbClassCode";
            this.cbClassCode.Size = new System.Drawing.Size(95, 23);
            this.cbClassCode.TabIndex = 16;
            this.cbClassCode.SelectedIndexChanged += new System.EventHandler(this.CbClassCode_SelectedIndexChanged);
            // 
            // cbTools
            // 
            this.cbTools.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTools.FormattingEnabled = true;
            this.cbTools.Items.AddRange(new object[] {
            ""});
            this.cbTools.Location = new System.Drawing.Point(131, 79);
            this.cbTools.Name = "cbTools";
            this.cbTools.Size = new System.Drawing.Size(95, 23);
            this.cbTools.TabIndex = 15;
            this.cbTools.SelectedIndexChanged += new System.EventHandler(this.CbTools_SelectedIndexChanged);
            // 
            // lblTools
            // 
            this.lblTools.AutoSize = true;
            this.lblTools.Location = new System.Drawing.Point(8, 82);
            this.lblTools.Name = "lblTools";
            this.lblTools.Size = new System.Drawing.Size(77, 15);
            this.lblTools.TabIndex = 14;
            this.lblTools.Text = "Инструмент:";
            // 
            // cbFirmID
            // 
            this.cbFirmID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFirmID.Enabled = false;
            this.cbFirmID.FormattingEnabled = true;
            this.cbFirmID.Items.AddRange(new object[] {
            ""});
            this.cbFirmID.Location = new System.Drawing.Point(319, 79);
            this.cbFirmID.Name = "cbFirmID";
            this.cbFirmID.Size = new System.Drawing.Size(108, 23);
            this.cbFirmID.TabIndex = 13;
            // 
            // lblFirmID
            // 
            this.lblFirmID.AutoSize = true;
            this.lblFirmID.Location = new System.Drawing.Point(234, 82);
            this.lblFirmID.Name = "lblFirmID";
            this.lblFirmID.Size = new System.Drawing.Size(65, 15);
            this.lblFirmID.TabIndex = 12;
            this.lblFirmID.Text = "ID фирмы:";
            // 
            // cbPort
            // 
            this.cbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPort.Enabled = false;
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Items.AddRange(new object[] {
            "34132",
            "34134"});
            this.cbPort.Location = new System.Drawing.Point(131, 22);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(95, 23);
            this.cbPort.TabIndex = 3;
            // 
            // cbAccountID
            // 
            this.cbAccountID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccountID.FormattingEnabled = true;
            this.cbAccountID.Items.AddRange(new object[] {
            ""});
            this.cbAccountID.Location = new System.Drawing.Point(319, 50);
            this.cbAccountID.Name = "cbAccountID";
            this.cbAccountID.Size = new System.Drawing.Size(108, 23);
            this.cbAccountID.TabIndex = 11;
            // 
            // lblAccountId
            // 
            this.lblAccountId.AutoSize = true;
            this.lblAccountId.Location = new System.Drawing.Point(234, 53);
            this.lblAccountId.Name = "lblAccountId";
            this.lblAccountId.Size = new System.Drawing.Size(81, 15);
            this.lblAccountId.TabIndex = 10;
            this.lblAccountId.Text = "Номер счета:";
            // 
            // cbClientCode
            // 
            this.cbClientCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClientCode.FormattingEnabled = true;
            this.cbClientCode.Items.AddRange(new object[] {
            ""});
            this.cbClientCode.Location = new System.Drawing.Point(319, 22);
            this.cbClientCode.Name = "cbClientCode";
            this.cbClientCode.Size = new System.Drawing.Size(108, 23);
            this.cbClientCode.TabIndex = 9;
            // 
            // lblClassCode
            // 
            this.lblClassCode.AutoSize = true;
            this.lblClassCode.Location = new System.Drawing.Point(8, 53);
            this.lblClassCode.Name = "lblClassCode";
            this.lblClassCode.Size = new System.Drawing.Size(70, 15);
            this.lblClassCode.TabIndex = 1;
            this.lblClassCode.Text = "Код класса:";
            // 
            // lblClientCode
            // 
            this.lblClientCode.AutoSize = true;
            this.lblClientCode.Location = new System.Drawing.Point(234, 25);
            this.lblClientCode.Name = "lblClientCode";
            this.lblClientCode.Size = new System.Drawing.Size(77, 15);
            this.lblClientCode.TabIndex = 8;
            this.lblClientCode.Text = "Код клиента:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(8, 25);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(117, 15);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "Порт подключения:";
            // 
            // btnCansel
            // 
            this.btnCansel.Location = new System.Drawing.Point(297, 373);
            this.btnCansel.Name = "btnCansel";
            this.btnCansel.Size = new System.Drawing.Size(75, 23);
            this.btnCansel.TabIndex = 7;
            this.btnCansel.Text = "Отмена";
            this.btnCansel.UseVisualStyleBackColor = true;
            this.btnCansel.Click += new System.EventHandler(this.BtnCansel_Click);
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(378, 373);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Выбрать";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(495, 436);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.panel1.ResumeLayout(false);
            this.gbStrategySettings.ResumeLayout(false);
            this.gbStrategySettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmcPercentCloseN2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcPercentCloseN1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcLevelCloseN2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcLevelCloseN1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcLevelOR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmcLevelMMR)).EndInit();
            this.gbQuikSettings.ResumeLayout(false);
            this.gbQuikSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbPort;
        private System.Windows.Forms.Button btnCansel;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblClassCode;
        private System.Windows.Forms.Label lblCloseN2;
        private System.Windows.Forms.ComboBox cbClientCode;
        private System.Windows.Forms.Label lblClientCode;
        private System.Windows.Forms.ComboBox cbAccountID;
        private System.Windows.Forms.Label lblAccountId;
        private System.Windows.Forms.GroupBox gbQuikSettings;
        private System.Windows.Forms.GroupBox gbStrategySettings;
        private System.Windows.Forms.Label lblTypeSigma;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblCloseN1;
        private System.Windows.Forms.Label lblFilterOR;
        private System.Windows.Forms.NumericUpDown nmcLevelMMR;
        private System.Windows.Forms.Label lblFilterMMR;
        private System.Windows.Forms.NumericUpDown nmcLevelCloseN2;
        private System.Windows.Forms.NumericUpDown nmcLevelCloseN1;
        private System.Windows.Forms.NumericUpDown nmcLevelOR;
        private System.Windows.Forms.ComboBox cbCloseN2;
        private System.Windows.Forms.ComboBox cbCloseN1;
        private System.Windows.Forms.ComboBox cbFilterOR;
        private System.Windows.Forms.ComboBox cbFilterMMR;
        private System.Windows.Forms.NumericUpDown nmcN;
        private System.Windows.Forms.Label lblN;
        private System.Windows.Forms.NumericUpDown nmcPercentCloseN2;
        private System.Windows.Forms.NumericUpDown nmcPercentCloseN1;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.ComboBox cbTools;
        private System.Windows.Forms.Label lblTools;
        private System.Windows.Forms.ComboBox cbFirmID;
        private System.Windows.Forms.Label lblFirmID;
        private System.Windows.Forms.ComboBox cbClassCode;
        private System.Windows.Forms.Label lblError;
    }
}