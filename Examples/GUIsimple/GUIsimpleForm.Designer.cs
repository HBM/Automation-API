namespace Hbm.Automation.Api.Weighing.Examples.GUIsimple
{
    partial class GUIsimpleForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.grrpSetup = new System.Windows.Forms.GroupBox();
            this.cboDeviceType = new System.Windows.Forms.ComboBox();
            this.cmdGrossNet = new System.Windows.Forms.Button();
            this.cmdTare = new System.Windows.Forms.Button();
            this.cmdZero = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.calibrationWithWeightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calibrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.picNE107 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.grrpSetup.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNE107)).BeginInit();
            this.SuspendLayout();
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(111, 28);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(100, 20);
            this.txtIPAddress.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP address";
            // 
            // cmdConnect
            // 
            this.cmdConnect.Location = new System.Drawing.Point(292, 25);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(84, 36);
            this.cmdConnect.TabIndex = 2;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.UseVisualStyleBackColor = true;
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // grrpSetup
            // 
            this.grrpSetup.Controls.Add(this.cboDeviceType);
            this.grrpSetup.Controls.Add(this.txtIPAddress);
            this.grrpSetup.Controls.Add(this.label1);
            this.grrpSetup.Controls.Add(this.cmdConnect);
            this.grrpSetup.Location = new System.Drawing.Point(10, 31);
            this.grrpSetup.Name = "grrpSetup";
            this.grrpSetup.Size = new System.Drawing.Size(390, 81);
            this.grrpSetup.TabIndex = 13;
            this.grrpSetup.TabStop = false;
            this.grrpSetup.Text = "Connection";
            // 
            // cboDeviceType
            // 
            this.cboDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDeviceType.FormattingEnabled = true;
            this.cboDeviceType.Items.AddRange(new object[] {
            "WTX (Jet)",
            "WTX (Modbus/TCP)",
            "DSE (Jet)"});
            this.cboDeviceType.Location = new System.Drawing.Point(111, 54);
            this.cboDeviceType.Name = "cboDeviceType";
            this.cboDeviceType.Size = new System.Drawing.Size(100, 21);
            this.cboDeviceType.TabIndex = 3;
            // 
            // cmdGrossNet
            // 
            this.cmdGrossNet.Location = new System.Drawing.Point(172, 118);
            this.cmdGrossNet.Name = "cmdGrossNet";
            this.cmdGrossNet.Size = new System.Drawing.Size(75, 23);
            this.cmdGrossNet.TabIndex = 24;
            this.cmdGrossNet.Text = "Gross/Net";
            this.cmdGrossNet.UseVisualStyleBackColor = true;
            this.cmdGrossNet.Click += new System.EventHandler(this.cmdGrossNet_Click);
            // 
            // cmdTare
            // 
            this.cmdTare.Location = new System.Drawing.Point(10, 118);
            this.cmdTare.Name = "cmdTare";
            this.cmdTare.Size = new System.Drawing.Size(75, 23);
            this.cmdTare.TabIndex = 22;
            this.cmdTare.Text = "Tare";
            this.cmdTare.UseVisualStyleBackColor = true;
            this.cmdTare.Click += new System.EventHandler(this.cmdTare_Click);
            // 
            // cmdZero
            // 
            this.cmdZero.Location = new System.Drawing.Point(91, 118);
            this.cmdZero.Name = "cmdZero";
            this.cmdZero.Size = new System.Drawing.Size(75, 23);
            this.cmdZero.TabIndex = 23;
            this.cmdZero.Text = "Zero";
            this.cmdZero.UseVisualStyleBackColor = true;
            this.cmdZero.Click += new System.EventHandler(this.cmdZero_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(412, 25);
            this.toolStrip1.TabIndex = 29;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.calibrationWithWeightToolStripMenuItem,
            this.calibrationToolStripMenuItem});
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(48, 22);
            this.toolStripDropDownButton1.Text = "Tools";
            // 
            // calibrationWithWeightToolStripMenuItem
            // 
            this.calibrationWithWeightToolStripMenuItem.Name = "calibrationWithWeightToolStripMenuItem";
            this.calibrationWithWeightToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.calibrationWithWeightToolStripMenuItem.Text = "Calculate calibration";
            this.calibrationWithWeightToolStripMenuItem.Click += new System.EventHandler(this.calibrationWithWeightToolStripMenuItem_Click_1);
            // 
            // calibrationToolStripMenuItem
            // 
            this.calibrationToolStripMenuItem.Name = "calibrationToolStripMenuItem";
            this.calibrationToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.calibrationToolStripMenuItem.Text = "Calibration with weight";
            this.calibrationToolStripMenuItem.Click += new System.EventHandler(this.calibrationToolStripMenuItem_Click_1);
            // 
            // txtInfo
            // 
            this.txtInfo.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInfo.Location = new System.Drawing.Point(10, 218);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(386, 104);
            this.txtInfo.TabIndex = 26;
            this.txtInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // picNE107
            // 
            this.picNE107.Location = new System.Drawing.Point(314, 118);
            this.picNE107.Name = "picNE107";
            this.picNE107.Size = new System.Drawing.Size(84, 72);
            this.picNE107.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picNE107.TabIndex = 27;
            this.picNE107.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(252, 150);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 21);
            this.button1.TabIndex = 30;
            this.button1.Text = "Read";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Application mode",
            "Calibration weight",
            "Connection type",
            "Device identification",
            "Firmware version",
            "LDW - Zero signal",
            "LWT - Nominal signal",
            "Scale range",
            "Serial number",
            "Tare mode",
            "Manual tare value",
            "Maximum capacity",
            "Weight stable",
            "Weight step (DSE)",
            "Zero value"});
            this.comboBox1.Location = new System.Drawing.Point(12, 150);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(234, 21);
            this.comboBox1.TabIndex = 31;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(11, 181);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(236, 20);
            this.textBox1.TabIndex = 32;
            // 
            // GUIsimpleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 331);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.picNE107);
            this.Controls.Add(this.cmdGrossNet);
            this.Controls.Add(this.cmdTare);
            this.Controls.Add(this.cmdZero);
            this.Controls.Add(this.grrpSetup);
            this.MinimumSize = new System.Drawing.Size(400, 350);
            this.Name = "GUIsimpleForm";
            this.Text = "GUIsimple";
            this.grrpSetup.ResumeLayout(false);
            this.grrpSetup.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNE107)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdConnect;
        private System.Windows.Forms.GroupBox grrpSetup;
        private System.Windows.Forms.Button cmdGrossNet;
        private System.Windows.Forms.Button cmdTare;
        private System.Windows.Forms.Button cmdZero;
        private System.Windows.Forms.PictureBox picNE107;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem calibrationWithWeightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calibrationToolStripMenuItem;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.ComboBox cboDeviceType;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

