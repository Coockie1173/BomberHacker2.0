namespace BHackerOverhaul.MainForm
{
    partial class InjectorForm
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
            this.ROMPathBox = new System.Windows.Forms.TextBox();
            this.InjectIMGBut = new System.Windows.Forms.Button();
            this.InjectFileBut = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TableIDNum = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.FileIDNum = new System.Windows.Forms.NumericUpDown();
            this.ChangeROMBox = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ImgNum = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.PalleteNum = new System.Windows.Forms.NumericUpDown();
            this.SelectedCodec = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.TableIDNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileIDNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PalleteNum)).BeginInit();
            this.SuspendLayout();
            // 
            // ROMPathBox
            // 
            this.ROMPathBox.Location = new System.Drawing.Point(95, 15);
            this.ROMPathBox.Name = "ROMPathBox";
            this.ROMPathBox.ReadOnly = true;
            this.ROMPathBox.Size = new System.Drawing.Size(431, 20);
            this.ROMPathBox.TabIndex = 1;
            // 
            // InjectIMGBut
            // 
            this.InjectIMGBut.Location = new System.Drawing.Point(13, 43);
            this.InjectIMGBut.Name = "InjectIMGBut";
            this.InjectIMGBut.Size = new System.Drawing.Size(111, 23);
            this.InjectIMGBut.TabIndex = 2;
            this.InjectIMGBut.Text = "Inject image into file";
            this.InjectIMGBut.UseVisualStyleBackColor = true;
            this.InjectIMGBut.Click += new System.EventHandler(this.InjectIMGBut_Click);
            // 
            // InjectFileBut
            // 
            this.InjectFileBut.Location = new System.Drawing.Point(13, 73);
            this.InjectFileBut.Name = "InjectFileBut";
            this.InjectFileBut.Size = new System.Drawing.Size(111, 23);
            this.InjectFileBut.TabIndex = 3;
            this.InjectFileBut.Text = "Inject file into ROM";
            this.InjectFileBut.UseVisualStyleBackColor = true;
            this.InjectFileBut.Click += new System.EventHandler(this.InjectFileBut_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(130, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Table:";
            // 
            // TableIDNum
            // 
            this.TableIDNum.Location = new System.Drawing.Point(173, 76);
            this.TableIDNum.Maximum = new decimal(new int[] {
            13,
            0,
            0,
            0});
            this.TableIDNum.Name = "TableIDNum";
            this.TableIDNum.Size = new System.Drawing.Size(37, 20);
            this.TableIDNum.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "File ID:";
            // 
            // FileIDNum
            // 
            this.FileIDNum.Location = new System.Drawing.Point(262, 76);
            this.FileIDNum.Maximum = new decimal(new int[] {
            871,
            0,
            0,
            0});
            this.FileIDNum.Name = "FileIDNum";
            this.FileIDNum.Size = new System.Drawing.Size(45, 20);
            this.FileIDNum.TabIndex = 7;
            // 
            // ChangeROMBox
            // 
            this.ChangeROMBox.Location = new System.Drawing.Point(13, 13);
            this.ChangeROMBox.Name = "ChangeROMBox";
            this.ChangeROMBox.Size = new System.Drawing.Size(75, 23);
            this.ChangeROMBox.TabIndex = 0;
            this.ChangeROMBox.Text = "Load Rom";
            this.ChangeROMBox.UseVisualStyleBackColor = true;
            this.ChangeROMBox.Click += new System.EventHandler(this.ChangeROMBox_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(130, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Img Offset :";
            // 
            // ImgNum
            // 
            this.ImgNum.Hexadecimal = true;
            this.ImgNum.Location = new System.Drawing.Point(197, 46);
            this.ImgNum.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ImgNum.Name = "ImgNum";
            this.ImgNum.Size = new System.Drawing.Size(59, 20);
            this.ImgNum.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(259, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Palette Offset:";
            // 
            // PalleteNum
            // 
            this.PalleteNum.Hexadecimal = true;
            this.PalleteNum.Location = new System.Drawing.Point(339, 46);
            this.PalleteNum.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.PalleteNum.Name = "PalleteNum";
            this.PalleteNum.Size = new System.Drawing.Size(59, 20);
            this.PalleteNum.TabIndex = 11;
            // 
            // SelectedCodec
            // 
            this.SelectedCodec.FormattingEnabled = true;
            this.SelectedCodec.Items.AddRange(new object[] {
            "CI4",
            "CI8"});
            this.SelectedCodec.Location = new System.Drawing.Point(405, 46);
            this.SelectedCodec.Name = "SelectedCodec";
            this.SelectedCodec.Size = new System.Drawing.Size(121, 21);
            this.SelectedCodec.TabIndex = 12;
            // 
            // InjectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 111);
            this.Controls.Add(this.SelectedCodec);
            this.Controls.Add(this.PalleteNum);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ImgNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FileIDNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TableIDNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InjectFileBut);
            this.Controls.Add(this.InjectIMGBut);
            this.Controls.Add(this.ROMPathBox);
            this.Controls.Add(this.ChangeROMBox);
            this.Name = "InjectorForm";
            this.Text = "InjectorForm";
            this.Activated += new System.EventHandler(this.InjectorForm_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.TableIDNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FileIDNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PalleteNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ROMPathBox;
        private System.Windows.Forms.Button InjectIMGBut;
        private System.Windows.Forms.Button InjectFileBut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown TableIDNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown FileIDNum;
        private System.Windows.Forms.Button ChangeROMBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ImgNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown PalleteNum;
        private System.Windows.Forms.ComboBox SelectedCodec;
    }
}