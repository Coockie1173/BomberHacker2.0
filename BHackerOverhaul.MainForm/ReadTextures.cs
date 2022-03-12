using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BHackerOverhaul.FileHandler;
using BHackerOverhaul.N64Graphics;

namespace BHackerOverhaul.MainForm
{
    public partial class ReadTextures : Form
    {
        public ReadTextures()
        {
            InitializeComponent();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            for(int i = 0; i < LoadedImgs.Count; i++)
            {
                LoadedImgs[i].Location = new Point(LoadedImgs[i].Location.X, OrigY[i] - vScrollBar1.Value);
                Labels[i].Location = new Point(Labels[i].Location.X, OrigY[i] - vScrollBar1.Value);
            }
        }

        List<PictureBox> LoadedImgs = new List<PictureBox>();
        List<Label> Labels = new List<Label>();
        List<int> OrigY = new List<int>();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "BIN file|*.bin";
                DialogResult res = open.ShowDialog();
                if (res == DialogResult.OK)
                {
                    foreach (PictureBox b in LoadedImgs)
                    {
                        b.Dispose();
                    }
                    foreach(Label l in Labels)
                    {
                        l.Dispose();
                    }
                    vScrollBar1.Maximum = 0;
                    LoadedImgs.Clear();
                    byte[] Data = File.ReadAllBytes(open.FileName);
                    //detect if header contains offsets or DL's contain offsets
                    /*List<string> HeaderData = new List<string>();
                    HeaderData.AddRange(new HeaderReader().ReadHeader(Data).Split('\n'));
                    HeaderData.RemoveAll(x => !x.Contains("Offset:"));
                    */
                    int[] Offsets = new HeaderReader().ReadOffsets(Data);

                    bool HeaderIsDL = true;
                    for (int i = 0; i < Offsets.Length / 2; i++)
                    {
                        if (Data[Offsets[i]] != 4)
                        {
                            HeaderIsDL = false;
                        }
                    }

                    if (HeaderIsDL)
                    {
                        int CurOffset = Offsets[0];
                        while (CurOffset < Data.Length)
                        {
                            if (Data[CurOffset] == 0xF5)
                            {
                                int OffsetPrevFD = CurOffset;
                                while (Data[OffsetPrevFD] != 0xFD)
                                {
                                    OffsetPrevFD -= 1;
                                }
                                if (OffsetPrevFD > CurOffset - 15)
                                {
                                    //texture command
                                    //get sizes
                                    int SizeX = 32;
                                    int SizeY = 32;
                                    switch (Data[CurOffset + 6])
                                    {
                                        case (0x40):
                                            {
                                                SizeY = 32;
                                                break;
                                            }
                                        case (0x80):
                                            {
                                                SizeY = 64;
                                                break;
                                            }
                                    }
                                    switch (Data[CurOffset + 7])
                                    {
                                        case (0x50):
                                            {
                                                SizeX = 32;
                                                break;
                                            }
                                        case (0x60):
                                            {
                                                SizeX = 64;
                                                break;
                                            }
                                    }
                                    byte[] FixedData = new byte[4];
                                    Array.Copy(Data, OffsetPrevFD + 4, FixedData, 0, 4);
                                    FixedData[0] = 0;
                                    UInt32 DataOffset = ByteTools.Read4Bytes(FixedData, 0);
                                    Bitmap b = new Bitmap(SizeX, SizeY, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                    Graphics g = Graphics.FromImage(b);

                                    byte[] Palette;

                                    string ColourType = Convert.ToString(Data[CurOffset + 1], 2);
                                    ColourType += Convert.ToString(Data[CurOffset + 2], 2);
                                    ColourType = ColourType.Substring(4, 2);

                                    switch (Convert.ToInt32(ColourType, 2))
                                    {
                                        case (0):
                                            {
                                                Palette = new byte[32];
                                                int PalOff = (int)DataOffset + 0x200;
                                                Array.Copy(Data, PalOff, Palette, 0, 32);
                                                N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.CI4, N64IMode.AlphaBinary);
                                                break;
                                            }
                                        case (1):
                                            {
                                                Palette = new byte[512];
                                                Array.Copy(Data, (int)DataOffset + 0x1000, Palette, 0, 512);
                                                N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.CI8, N64IMode.AlphaBinary);
                                                break;
                                            }
                                    }

                                    PictureBox box = new PictureBox();
                                    box.Image = b;
                                    box.Width = SizeX;
                                    box.Height = SizeY;
                                    LoadedImgs.Add(box);
                                    Label l = new Label();
                                    l.Text = DataOffset.ToString("X");
                                    Labels.Add(l);
                                }

                            }


                            CurOffset += 8;
                        }
                    }
                    else
                    {
                        /*
                        foreach(int off in Offsets)
                        {
                            byte[] pal = new byte[32];

                            Array.Copy(Data, off, pal, 0, 32);
                            Bitmap b = new Bitmap(32, 32, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            Graphics g = Graphics.FromImage(b);
                            N64GraphicsCoding.RenderTexture(g, Data, pal, off, 32,32,1,N64Codec.CI4, N64IMode.AlphaBinary);

                            PictureBox box = new PictureBox();
                            box.Image = b;
                            box.Width = 32;
                            box.Height = 32;
                            LoadedImgs.Add(box);

                            pal = new byte[256];

                            Array.Copy(Data, off, pal, 0, 256);
                            b = new Bitmap(256, 256, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            g = Graphics.FromImage(b);
                            N64GraphicsCoding.RenderTexture(g, Data, pal, off, 64, 64, 1, N64Codec.CI8, N64IMode.AlphaBinary);

                            box = new PictureBox();
                            box.Image = b;
                            box.Width = 32;
                            box.Height = 32;
                            LoadedImgs.Add(box);
                        }
                        byte[] Pal = new byte[32];

                        */
                    }
                    int CurPosY = BaseBox.Location.Y;
                    int CurPosX = BaseBox.Location.X;
                    /*foreach (PictureBox b in LoadedImgs)
                    {
                        b.Location = new System.Drawing.Point(CurPosX, CurPosY);
                        CurPosY += b.Image.Height + 10;
                        vScrollBar1.Maximum += b.Image.Height + 10;
                        this.Controls.Add(b);
                        OrigY.Add(CurPosY);
                    }*/
                    for(int i = 0; i < LoadedImgs.Count; i++)
                    { 
                        LoadedImgs[i].Location = new System.Drawing.Point(CurPosX, CurPosY);
                        CurPosY += LoadedImgs[i].Image.Height + 10;
                        vScrollBar1.Maximum += LoadedImgs[i].Image.Height + 10;
                        this.Controls.Add(LoadedImgs[i]);
                        OrigY.Add(CurPosY);
                        Labels[i].Location = new System.Drawing.Point(CurPosX + LoadedImgs[i].Width + 32, CurPosY);
                        this.Controls.Add(Labels[i]);
                    }
                    BaseBox.Visible = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
