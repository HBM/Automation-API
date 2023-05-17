namespace Hbm.Automation.Api.Weighing.Examples.GUISimple.NETStandard
{
    partial class AdjustmentWeigher
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
            this.lblCalibrationWeight = new System.Windows.Forms.Label();
            this.txtCalibrationWeight = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.cmdAdjust = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCalibrationWeight
            // 
            this.lblCalibrationWeight.AutoSize = true;
            this.lblCalibrationWeight.Location = new System.Drawing.Point(21, 36);
            this.lblCalibrationWeight.Name = "lblCalibrationWeight";
            this.lblCalibrationWeight.Size = new System.Drawing.Size(93, 13);
            this.lblCalibrationWeight.TabIndex = 0;
            this.lblCalibrationWeight.Text = "Calibration Weight";
            // 
            // txtCalibrationWeight
            // 
            this.txtCalibrationWeight.Location = new System.Drawing.Point(132, 33);
            this.txtCalibrationWeight.Name = "txtCalibrationWeight";
            this.txtCalibrationWeight.Size = new System.Drawing.Size(100, 20);
            this.txtCalibrationWeight.TabIndex = 1;
            this.txtCalibrationWeight.Text = "1.7";
            this.txtCalibrationWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCakibrationWeight_KeyPress);
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(238, 36);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(24, 13);
            this.lblUnit.TabIndex = 2;
            this.lblUnit.Text = "unit";
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(24, 87);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(139, 57);
            this.txtInfo.TabIndex = 3;
            // 
            // cmdAdjust
            // 
            this.cmdAdjust.Location = new System.Drawing.Point(182, 106);
            this.cmdAdjust.Name = "cmdAdjust";
            this.cmdAdjust.Size = new System.Drawing.Size(80, 38);
            this.cmdAdjust.TabIndex = 4;
            this.cmdAdjust.Text = "Start";
            this.cmdAdjust.UseVisualStyleBackColor = true;
            this.cmdAdjust.Click += new System.EventHandler(this.cmdAdjust_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(182, 77);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // AdjustmentWeigher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 165);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdAdjust);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.txtCalibrationWeight);
            this.Controls.Add(this.lblCalibrationWeight);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AdjustmentWeigher";
            this.Text = "Calibration";
            this.Load += new System.EventHandler(this.AdjustmentWeigher_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCalibrationWeight;
        private System.Windows.Forms.TextBox txtCalibrationWeight;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Button cmdAdjust;
        private System.Windows.Forms.Button cmdCancel;
    }
}