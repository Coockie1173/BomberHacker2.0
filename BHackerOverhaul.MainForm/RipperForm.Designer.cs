namespace BHackerOverhaul.MainForm
{
    partial class RipperForm
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
            this.OpenROMBut = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OpenROMBut
            // 
            this.OpenROMBut.Location = new System.Drawing.Point(13, 13);
            this.OpenROMBut.Name = "OpenROMBut";
            this.OpenROMBut.Size = new System.Drawing.Size(99, 23);
            this.OpenROMBut.TabIndex = 0;
            this.OpenROMBut.Text = "RIP Files";
            this.OpenROMBut.UseVisualStyleBackColor = true;
            this.OpenROMBut.Click += new System.EventHandler(this.OpenROMBut_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(35, 39);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(46, 13);
            this.StatusLabel.TabIndex = 1;
            this.StatusLabel.Text = "Standby";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RipperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(124, 68);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.OpenROMBut);
            this.Name = "RipperForm";
            this.Text = "Ripper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenROMBut;
        private System.Windows.Forms.Label StatusLabel;
    }
}