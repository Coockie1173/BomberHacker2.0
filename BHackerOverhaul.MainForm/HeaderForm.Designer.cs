namespace BHackerOverhaul.MainForm
{
    partial class HeaderForm
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
            this.ReadHeaderBut = new System.Windows.Forms.Button();
            this.HeaderBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ReadHeaderBut
            // 
            this.ReadHeaderBut.Dock = System.Windows.Forms.DockStyle.Top;
            this.ReadHeaderBut.Location = new System.Drawing.Point(0, 0);
            this.ReadHeaderBut.Name = "ReadHeaderBut";
            this.ReadHeaderBut.Size = new System.Drawing.Size(270, 23);
            this.ReadHeaderBut.TabIndex = 0;
            this.ReadHeaderBut.Text = "Read Header";
            this.ReadHeaderBut.UseVisualStyleBackColor = true;
            this.ReadHeaderBut.Click += new System.EventHandler(this.ReadHeaderBut_Click);
            // 
            // HeaderBox
            // 
            this.HeaderBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderBox.Location = new System.Drawing.Point(0, 23);
            this.HeaderBox.Multiline = true;
            this.HeaderBox.Name = "HeaderBox";
            this.HeaderBox.ReadOnly = true;
            this.HeaderBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.HeaderBox.Size = new System.Drawing.Size(270, 427);
            this.HeaderBox.TabIndex = 1;
            // 
            // HeaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 450);
            this.Controls.Add(this.HeaderBox);
            this.Controls.Add(this.ReadHeaderBut);
            this.Name = "HeaderForm";
            this.Text = "HeaderForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ReadHeaderBut;
        private System.Windows.Forms.TextBox HeaderBox;
    }
}