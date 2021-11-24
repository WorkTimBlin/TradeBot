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
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.btnCansel = new System.Windows.Forms.Button();
            this.lblPort = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblTool = new System.Windows.Forms.Label();
            this.nmcN = new System.Windows.Forms.NumericUpDown();
            this.tbTool = new System.Windows.Forms.TextBox();
            this.lblN = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmcN)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.cbPort);
            this.panel1.Controls.Add(this.btnCansel);
            this.panel1.Controls.Add(this.lblPort);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.lblTool);
            this.panel1.Controls.Add(this.nmcN);
            this.panel1.Controls.Add(this.tbTool);
            this.panel1.Controls.Add(this.lblN);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(246, 176);
            this.panel1.TabIndex = 9;
            // 
            // cbPort
            // 
            this.cbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Items.AddRange(new object[] {
            "34132",
            "34134"});
            this.cbPort.Location = new System.Drawing.Point(135, 11);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(95, 23);
            this.cbPort.TabIndex = 3;
            // 
            // btnCansel
            // 
            this.btnCansel.Location = new System.Drawing.Point(74, 141);
            this.btnCansel.Name = "btnCansel";
            this.btnCansel.Size = new System.Drawing.Size(75, 23);
            this.btnCansel.TabIndex = 7;
            this.btnCansel.Text = "Отмена";
            this.btnCansel.UseVisualStyleBackColor = true;
            this.btnCansel.Click += new System.EventHandler(this.BtnCansel_Click);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(12, 14);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(117, 15);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "Порт подключения:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(155, 141);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Выбрать";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // lblTool
            // 
            this.lblTool.AutoSize = true;
            this.lblTool.Location = new System.Drawing.Point(12, 42);
            this.lblTool.Name = "lblTool";
            this.lblTool.Size = new System.Drawing.Size(77, 15);
            this.lblTool.TabIndex = 1;
            this.lblTool.Text = "Инструмент:";
            // 
            // nmcN
            // 
            this.nmcN.Location = new System.Drawing.Point(135, 69);
            this.nmcN.Name = "nmcN";
            this.nmcN.Size = new System.Drawing.Size(95, 23);
            this.nmcN.TabIndex = 5;
            this.nmcN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbTool
            // 
            this.tbTool.Location = new System.Drawing.Point(135, 39);
            this.tbTool.Name = "tbTool";
            this.tbTool.Size = new System.Drawing.Size(95, 23);
            this.tbTool.TabIndex = 2;
            this.tbTool.Text = "RIZ1";
            this.tbTool.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbTool.TextChanged += new System.EventHandler(this.TbTool_TextChanged);
            // 
            // lblN
            // 
            this.lblN.AutoSize = true;
            this.lblN.Location = new System.Drawing.Point(12, 71);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(77, 15);
            this.lblN.TabIndex = 4;
            this.lblN.Text = "Параметр N:";
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(270, 200);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmcN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbPort;
        private System.Windows.Forms.Button btnCansel;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblTool;
        private System.Windows.Forms.NumericUpDown nmcN;
        private System.Windows.Forms.TextBox tbTool;
        private System.Windows.Forms.Label lblN;
    }
}