﻿namespace BHackerOverhaul.MainForm
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ripperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.injectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headerReaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ripperToolStripMenuItem,
            this.injectorToolStripMenuItem,
            this.headerReaderToolStripMenuItem,
            this.imageViewerToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(650, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ripperToolStripMenuItem
            // 
            this.ripperToolStripMenuItem.Name = "ripperToolStripMenuItem";
            this.ripperToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ripperToolStripMenuItem.Text = "Ripper";
            this.ripperToolStripMenuItem.Click += new System.EventHandler(this.RipperToolStripMenuItem_Click);
            // 
            // injectorToolStripMenuItem
            // 
            this.injectorToolStripMenuItem.Name = "injectorToolStripMenuItem";
            this.injectorToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.injectorToolStripMenuItem.Text = "Injector";
            this.injectorToolStripMenuItem.Click += new System.EventHandler(this.injectorToolStripMenuItem_Click);
            // 
            // headerReaderToolStripMenuItem
            // 
            this.headerReaderToolStripMenuItem.Name = "headerReaderToolStripMenuItem";
            this.headerReaderToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.headerReaderToolStripMenuItem.Text = "Header Reader";
            this.headerReaderToolStripMenuItem.Click += new System.EventHandler(this.headerReaderToolStripMenuItem_Click);
            // 
            // imageViewerToolStripMenuItem
            // 
            this.imageViewerToolStripMenuItem.Name = "imageViewerToolStripMenuItem";
            this.imageViewerToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.imageViewerToolStripMenuItem.Text = "Image Viewer";
            this.imageViewerToolStripMenuItem.Click += new System.EventHandler(this.imageViewerToolStripMenuItem_Click);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 422);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ripperToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem injectorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem headerReaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
    }
}

