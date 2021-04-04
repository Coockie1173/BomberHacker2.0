using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BHackerOverhaul.DLHandling;
using BHackerOverhaul.FileHandler;
using BHackerOverhaul._3DData;
using BHackerOverhaul.N64Graphics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Globalization;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace BHackerOverhaul.MainForm
{
    public partial class OBjtest : Form
    {
        List<PictureBox> LoadedImgs = new List<PictureBox>();
        List<Label> Labels = new List<Label>();
        List<int> OrigY = new List<int>();

        public OBjtest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int p = 1;
            int f5;
            int SizeX = 32;
            int SizeY = 32;
            UInt32 DataOffset = 0;
            List<int> imageOffsets = new List<int>();
            List<string> imageNames = new List<string>();


            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "bin file|*.bin";
            DialogResult res = open.ShowDialog();
            CommonOpenFileDialog FolSelect = new CommonOpenFileDialog();
            FolSelect.IsFolderPicker = true;
            if (res == DialogResult.OK && FolSelect.ShowDialog() == CommonFileDialogResult.Ok)
            {
                try
                {
                    //OpenFileDialog open = new OpenFileDialog();
                    //open.Filter = "BIN file|*.bin";
                    //DialogResult res = open.ShowDialog();
                    //if (res == DialogResult.OK)
                    {
                        foreach (PictureBox b in LoadedImgs)
                        {
                            b.Dispose();
                        }
                        foreach (Label l in Labels)
                        {
                            l.Dispose();
                        }
                        //vScrollBar1.Maximum = 0;
                        //LoadedImgs.Clear();
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
                            f5 = 0;
                            int CurOffset = Offsets[0];
                            while (CurOffset < Data.Length)
                            {
                                if (Data[CurOffset] == 0x04 || Data[CurOffset] == 0xB8)
                                {
                                    f5 = 0;
                                }
                                //count f5 third instance = pallette
                                if (Data[CurOffset] == 0xF5)
                                {
                                    f5++;
                                    int OffsetPrevFD = CurOffset;
                                    while (Data[OffsetPrevFD] != 0xFD)
                                    {
                                        OffsetPrevFD -= 1;
                                    }
                                    if (OffsetPrevFD >= CurOffset - 15)
                                    {
                                        //texture command
                                        //get sizes

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
                                        DataOffset = ByteTools.Read4Bytes(FixedData, 0); //UInt32
                                        //store offset at this point
                                        imageOffsets.Add((int)DataOffset);


                                    }

                                }
                                if (f5 == 3)
                                {
                                    Bitmap b = new Bitmap(SizeX, SizeY, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                                    Graphics g = Graphics.FromImage(b);



                                    byte[] Palette;
                                    int colval;
                                    //string ColourType = Convert.ToString(Data[CurOffset + 1], 2);
                                    //ColourType += Convert.ToString(Data[CurOffset + 2], 2);
                                    //ColourType = ColourType.Substring(4, 2);
                                    colval = (Data[CurOffset + 1]);
                                    colval = (colval & 24);

                                    colval = colval >> 3;
                                    //Convert.ToInt32(ColourType, 2);
                                    switch (colval)
                                    {
                                        //ci4
                                        case (0):
                                            {
                                                Palette = new byte[32];
                                                int PalOff = (int)DataOffset + 0x200;
                                                Array.Copy(Data, PalOff, Palette, 0, 32);
                                                N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.CI4, N64IMode.AlphaBinary);
                                                break;
                                            }
                                        //rgb16
                                        case (1):
                                            {
                                                Palette = new byte[512];
                                                int arrayoffset = (int)DataOffset + 0x400;
                                                Array.Copy(Data, arrayoffset, Palette, 0, 512);
                                                N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.CI8, N64IMode.AlphaCopyIntensity);
                                                break;
                                            }
                                        //rgb16
                                        case (2):
                                            {
                                                Palette = new byte[32];
                                                int PalOff = (int)DataOffset + 0x200;
                                                Array.Copy(Data, PalOff, Palette, 0, 32);
                                                N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.RGBA16, N64IMode.AlphaBinary);
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
                                    b.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                    string filepath = @"\textures\" + Path.GetFileName(open.FileName.Substring(0, open.FileName.Length - 4) + "-" + p + ".bmp");
                                    b.Save(FolSelect.FileName + filepath);
                                    imageNames.Add(FolSelect.FileName + filepath);
                                    p++;
                                    f5 = 0;
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
                        for (int i = 0; i < LoadedImgs.Count; i++)
                        {
                            LoadedImgs[i].Location = new System.Drawing.Point(CurPosX, CurPosY);
                            CurPosY += LoadedImgs[i].Image.Height + 10;
                            //vScrollBar1.Maximum += LoadedImgs[i].Image.Height + 10;
                            this.Controls.Add(LoadedImgs[i]);
                            OrigY.Add(CurPosY);
                            Labels[i].Location = new System.Drawing.Point(CurPosX + LoadedImgs[i].Width + 32, CurPosY);
                            this.Controls.Add(Labels[i]);
                        }
                        BaseBox.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                try
                {
                    SaveFileDialog mtl = new SaveFileDialog();
                    mtl.Filter = "material file|*.mtl";
                    res = mtl.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        //string[] OutObj = generateMtl(mtl.FileName, imageNames, RList);
                        //File.WriteAllLines(mtl.FileName, OutObj);
                    }

                    SaveFileDialog obj = new SaveFileDialog();
                    obj.Filter = "object file|*.obj";
                    res = obj.ShowDialog();

                    if (res == DialogResult.OK)
                    {


                        string[] OutObj = new DLParser().GetParsedObject2(File.ReadAllBytes(open.FileName), imageOffsets, imageNames, mtl.FileName.Substring(0, mtl.FileName.Length - 4));
                        File.WriteAllLines(obj.FileName, OutObj);
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            List<string> V = new List<string>();
            List<string> VT = new List<string>();
            List<string> F = new List<string>();
            List<int> imgOffList = new List<int>();
            List<int> palOffList = new List<int>();
            List<string> RList = new List<string>();
            List<string> GList = new List<string>();
            List<string> BList = new List<string>();
            List<int> fMat = new List<int>();
            int palOffset = 0;
            int imgOffset = 0;
            int imgFormat = 0;
            //int cCount;
            int AdditionalOffset;

            int[] mtlCol = new int[3];
            float R = 0;
            float G = 0;
            float B = 0;
            int p = 0;
            int lastImg = 0;
            int FD;
            int FD4B = 0;
            int SizeX = 32;
            int SizeY = 32;
            UInt32 DataOffset = 0;
            List<int> imageOffsets = new List<int>();
            List<string> imageNames = new List<string>();
            List<DLObject> Temp = new List<DLObject>();

            //open bin
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "bin file|*.bin";
            DialogResult res = open.ShowDialog();
            CommonOpenFileDialog FolSelect = new CommonOpenFileDialog();
            FolSelect.IsFolderPicker = true;

            if (res == DialogResult.OK && FolSelect.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Directory.CreateDirectory(FolSelect.FileName + @"\textures\");
                try
                {
                    //clear images
                    foreach (PictureBox b in LoadedImgs)
                    {
                        b.Dispose();
                    }
                    foreach (Label l in Labels)
                    {
                        l.Dispose();
                    }

                    //get file offsets
                    byte[] Data = File.ReadAllBytes(open.FileName);
                    int[] Offsets = new HeaderReader().ReadOffsets(Data);
                    //bool HeaderIsDL = true;


                    //for (int i = 0; i < Offsets.Length / 2; i++)
                    //{
                    foreach (int Offset in Offsets)
                    {
                        //try each offset
                        try
                        {
                            DLObject Buf = new DLObject();
                            int CurOffset = Offset;
                            //if (Data[Offsets[i]] != 4)
                            //{
                            //    HeaderIsDL = false;
                            //}
                            //process offset
                            //if (HeaderIsDL)
                            //{
                            FD = 0;
                            //int CurOffset = Offsets[0];
                            while (Data[CurOffset] != 0xB8)//Data.Length
                            {
                                if (Data[CurOffset] == 0x04)
                                {
                                    FD = 0;
                                    Buf = new DLObject();
                                    byte[] Pos = new byte[4];
                                    Array.Copy(Data, CurOffset + 4, Pos, 0, 4);
                                    Pos[0] = 0;
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        Array.Reverse(Pos);
                                    }
                                    Buf.DataOffset = BitConverter.ToInt32(Pos, 0);

                                    string Length = Convert.ToString(Data[CurOffset + 2], 2).PadLeft(8, '0');
                                    Length = Length.Substring(0, 6);
                                    Buf.Length = Convert.ToInt32(Length, 2);

                                    //gets last paint value for block of coordinates. Needs work
                                    mtlCol = getvtPaint(Data, Buf.DataOffset, Buf.Length);
                                    R = mtlCol[0] / (float)255;
                                    G = mtlCol[1] / (float)255;
                                    B = mtlCol[2] / (float)255;

                                    Temp.Add(Buf);
                                }

                                else if (Data[CurOffset] == 0xFD)
                                {
                                    FD++;
                                    //check whether pallette, image or image format
                                    byte[] Pos = new byte[2];
                                    Array.Copy(Data, CurOffset + 6, Pos, 0, 2);

                                    if (Data[CurOffset + 0x10] == 0xF5)
                                    {
                                        palOffset = Pos[0] * 256 + Pos[1];
                                    }
                                    else if (Data[CurOffset + 0x08] == 0xF5)
                                    {
                                        imgOffset = Pos[0] * 256 + Pos[1];
                                    }

                                    imgFormat = (Data[CurOffset + 1] & 24) >> 3;

                                }
                                else if (Data[CurOffset] == 0xF5)
                                {

                                    imgFormat = (Data[CurOffset + 1] & 24) >> 3;

                                    
                                    //byte[] Pos = new byte[2];
                                    //switch (f5)
                                    //{
                                    //    case 0:
                                    //        Array.Copy(Data, CurOffset - 10, Pos, 0, 2);
                                    //        //if (BitConverter.IsLittleEndian)
                                    //        //{
                                    //        //    Array.Reverse(Pos);
                                    //        //}
                                    //        palOffset = Pos[0] * 256 + Pos[1];   //BitConverter.ToInt32(Pos, 0);
                                    //        imgOffset = Pos[0] * 256 + Pos[1];
                                    //        imgFormat = (Data[CurOffset + 1] & 24) >> 3;
                                    //        f5++;
                                    //        break;
                                    //    case 1:
                                    //        Array.Copy(Data, CurOffset - 2, Pos, 0, 2);
                                    //        //if (BitConverter.IsLittleEndian)
                                    //        //{
                                    //        //    Array.Reverse(Pos);
                                    //        //}
                                    //        FD4B = Data[CurOffset - 5];
                                    //        imgOffset = Pos[0] * 256 + Pos[1];  // BitConverter.ToInt32(Pos, 0);
                                    //        imgFormat = (Data[CurOffset + 1] & 24) >> 3;
                                    //        f5++;
                                    //        break;
                                    //    case 2:
                                    //        //p++;
                                    //        imgFormat = (Data[CurOffset + 1] & 24) >> 3;
                                    //        //int[] sizes = getImgSize(imgFormat, imgOffset, palOffset, FD4B);
                                    //        //SizeX = sizes[0];
                                    //        //SizeY = sizes[1];
                                    //        //Bitmap b = new Bitmap(SizeX, SizeY, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                    //        //Graphics g = Graphics.FromImage(b);
                                    //        //createImg(g, Data, imgFormat, imgOffset, palOffset, SizeX, SizeY);
                                    //        //PictureBox box = new PictureBox();
                                    //        //box.Image = b;
                                    //        //box.Width = SizeX;
                                    //        //box.Height = SizeY;
                                    //        //LoadedImgs.Add(box);
                                    //        //Label l = new Label();
                                    //        //l.Text = DataOffset.ToString("X");
                                    //        //Labels.Add(l);
                                    //        //b.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                    //        //string filepath = open.FileName.Substring(0, open.FileName.Length - 4) + "-" + p;
                                    //        //b.Save(filepath + ".bmp");
                                    //        //if (SizeX == 1 && SizeY == 1) imageNames.Add(" ");
                                    //        //else imageNames.Add(filepath + ".bmp");
                                    //        //RList.Add(R.ToString());
                                    //        //GList.Add(G.ToString());
                                    //        //BList.Add(B.ToString());
                                    //        //f5 = 0;
                                    //        break;
                                    //}

                                }

                                //f2 states texture size according to documentation
                                else if (Data[CurOffset] == 0xF2)
                                {
                                    // incorrect for some textures
                                    int uppNib;
                                    int lowNib;
                                    byte[] Pos = new byte[3];
                                    Array.Copy(Data, CurOffset + 5, Pos, 0, 3);

                                    uppNib = Pos[1] >> 4;
                                    lowNib = Pos[1] & 15;

                                    SizeX = ((((((Pos[0] << 4) + uppNib) >> 2) + 1) >> 1) << 1);
                                    SizeY = (((((Pos[2]) >> 2) + 1) >> 1) << 1);
                                }

                                
                                else if (Data[CurOffset] == 0xB1)
                                {
                                    //checks if using last image, new image or vtx paint
                                    if (FD != 0)
                                    {
                                    int listImg = -1;      
                                        //checks existing images to assign faces with existing created image
                                        for(int i = 0; i < imgOffList.Count; i++)
                                        {                                           
                                            if (imgOffset == imgOffList[i]) listImg = i + 1;
                                        }

                                        if( listImg == -1)
                                        {
                                            p++;

                                            //original sizing function to get size of image from colformat and length not consistent
                                            //int[] sizes = getImgSize(imgFormat, imgOffset, palOffset, FD4B);
                                            //SizeX = sizes[0];
                                            //SizeY = sizes[1];

                                            Bitmap b = new Bitmap(SizeX, SizeY, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                            Graphics g = Graphics.FromImage(b);

                                            createImg(g, Data, imgFormat, imgOffset, palOffset, SizeX, SizeY);

                                            PictureBox box = new PictureBox();
                                            box.Image = b;
                                            box.Width = SizeX;
                                            box.Height = SizeY;
                                            LoadedImgs.Add(box);
                                            Label l = new Label();
                                            l.Text = DataOffset.ToString("X");
                                            Labels.Add(l);
                                            b.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                            string filepath = open.FileName.Substring(0, open.FileName.Length - 4) + "-" + p;
                                            b.Save(filepath + ".bmp");
                                            if (imgOffset == 0 && palOffset == 0) imageNames.Add(" ");
                                            else imageNames.Add(filepath + ".bmp");
                                            RList.Add(R.ToString());
                                            GList.Add(G.ToString());
                                            BList.Add(B.ToString());
                                            imgOffList.Add(imgOffset);
                                            palOffList.Add(palOffset);
                                            FD = 0;
                                            lastImg = p;
                                        }
                                        else
                                        {
                                            lastImg = listImg;
                                        }


                                    }

                                    Connection c = new Connection();
                                    c.Connection1 = Data[CurOffset + 1] / 2;
                                    c.Connection2 = Data[CurOffset + 2] / 2;
                                    c.Connection3 = Data[CurOffset + 3] / 2;
                                    //if (SizeX == 1 && SizeY == 1) c.mtlId = 0;
                                    //else 
                                    c.mtlId = lastImg;
                                    Buf.connections.Add(c);
                                    //fMat.Add(p);

                                    c = new Connection();
                                    c.Connection1 = Data[CurOffset + 5] / 2;
                                    c.Connection2 = Data[CurOffset + 6] / 2;
                                    c.Connection3 = Data[CurOffset + 7] / 2;
                                    //if (SizeX == 1 && SizeY == 1) c.mtlId = 0;
                                    //else 

                                    //storing mtl id
                                    c.mtlId = lastImg;
                                    Buf.connections.Add(c);
                                    //fMat.Add(p);
                                }
                                else if (Data[CurOffset] == 0xBF)
                                {

                                    if (FD != 0)
                                    {
                                    int listImg = -1;
                                        for (int i = 0; i < imgOffList.Count; i++)
                                        {
                                            if (imgOffset == imgOffList[i]) listImg = i + 1;
                                        }

                                        if (listImg == -1)
                                        {
                                            p++;
                                            //int[] sizes = getImgSize(imgFormat, imgOffset, palOffset, FD4B);
                                            //SizeX = sizes[0];
                                            //SizeY = sizes[1];

                                            Bitmap b = new Bitmap(SizeX, SizeY, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                            Graphics g = Graphics.FromImage(b);

                                            createImg(g, Data, imgFormat, imgOffset, palOffset, SizeX, SizeY);

                                            PictureBox box = new PictureBox();
                                            box.Image = b;
                                            box.Width = SizeX;
                                            box.Height = SizeY;
                                            LoadedImgs.Add(box);
                                            Label l = new Label();
                                            l.Text = DataOffset.ToString("X");
                                            Labels.Add(l);
                                            b.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                            string filepath = @"\textures\" + Path.GetFileName(open.FileName.Substring(0, open.FileName.Length - 4) + "-" + p + ".bmp");
                                            b.Save(FolSelect.FileName + filepath);
                                            if (imgOffset == 0 && palOffset == 0) imageNames.Add(" ");
                                            else imageNames.Add(FolSelect.FileName + filepath);
                                            RList.Add(R.ToString());
                                            GList.Add(G.ToString());
                                            BList.Add(B.ToString());
                                            imgOffList.Add(imgOffset);
                                            palOffList.Add(palOffset);
                                            FD = 0;
                                            lastImg = p;
                                        }
                                        else
                                        {
                                            lastImg = listImg;
                                        }


                                    }

                                    Connection c = new Connection();
                                    c.Connection1 = Data[CurOffset + 5] / 2;
                                    c.Connection2 = Data[CurOffset + 6] / 2;
                                    c.Connection3 = Data[CurOffset + 7] / 2;
                                    //if (SizeX == 1 && SizeY == 1) c.mtlId = 0;
                                    //else 
                                    c.mtlId = lastImg;
                                    Buf.connections.Add(c);
                                    //fMat.Add(p);
                                }


                                CurOffset += 0x08;
                            }
                            Temp.Add(Buf);
                            //}


                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                        }
                    }

                    //get vertices & uvs
                    AdditionalOffset = 1;
                    foreach (DLObject obj in Temp)
                    {
                        try { 

                        int CurOffset = obj.DataOffset;

                        for (int i = 0; i < obj.Length; i++)
                        {
                            Vector3 buf = new Vector3();
                            byte[] temp = new byte[2]; 
                            Array.Copy(Data, CurOffset, temp, 0, 2);
                            if (BitConverter.IsLittleEndian)
                            {
                                Array.Reverse(temp);
                            }
                            buf.X = (float)BitConverter.ToInt16(temp, 0);
                            temp = new byte[2];
                            Array.Copy(Data, CurOffset + 2, temp, 0, 2);
                            if (BitConverter.IsLittleEndian)
                            {
                                Array.Reverse(temp);
                            }
                            buf.Y = (float)BitConverter.ToInt16(temp, 0);
                            temp = new byte[2];
                            Array.Copy(Data, CurOffset + 4, temp, 0, 2);
                            if (BitConverter.IsLittleEndian)
                            {
                                Array.Reverse(temp);
                            }
                            buf.Z = (float)BitConverter.ToInt16(temp, 0);

                            string Builder = "v ";
                            Builder += buf.ToString();

                            V.Add(Builder);

                            //new
                            Vector3 VertexTexture = new Vector3();
                            temp = new byte[2];
                            Array.Copy(Data, CurOffset + 8, temp, 0, 2);
                            if (BitConverter.IsLittleEndian)
                            {
                                Array.Reverse(temp);
                            }

                            VertexTexture.X = (float)BitConverter.ToInt16(temp, 0) / 1024;

                            temp = new byte[2];
                            Array.Copy(Data, CurOffset + 10, temp, 0, 2);
                            if (BitConverter.IsLittleEndian)
                            {
                                Array.Reverse(temp);
                            }
                            VertexTexture.Y = (float)BitConverter.ToInt16(temp, 0) / 1024;


                            VT.Add("vt " + VertexTexture.X.ToString() + " " + VertexTexture.Y.ToString());

                            AdditionalOffset++;
                            CurOffset += 0x10;
                        }


                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }

                }


                try
                {
                    //get all faces
                    AdditionalOffset = 1;
                    foreach (DLObject obj in Temp)
                    {
                        foreach (Connection connection in obj.connections)
                        {
                            if (connection.Connection1 + AdditionalOffset <= V.Count && connection.Connection2 + AdditionalOffset <= V.Count && connection.Connection3 + AdditionalOffset <= V.Count)
                            {
                                string builder = "f ";
                                builder += $"{connection.Connection1 + AdditionalOffset}{"/"}{connection.Connection1 + AdditionalOffset} {connection.Connection2 + AdditionalOffset}{"/"}{connection.Connection2 + AdditionalOffset} {connection.Connection3 + AdditionalOffset}{"/"}{connection.Connection3 + AdditionalOffset}";
                                F.Add(builder);
                                fMat.Add(connection.mtlId);
                            }
                        }
                        AdditionalOffset += obj.Length;
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            } 
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            try
            {
                    //SaveFileDialog mtl = new SaveFileDialog();
                    //mtl.Filter = "material file|*.mtl";
                    //res = mtl.ShowDialog();
                    //if (res == DialogResult.OK)
                    //{
                    //    string[] OutObj = generateMtl(mtl.FileName, imageNames,RList,GList,BList);
                    //    File.WriteAllLines(mtl.FileName, OutObj);
                    //}

                    //SaveFileDialog obj = new SaveFileDialog();
                    //obj.Filter = "object file|*.obj";
                    //res = obj.ShowDialog();

                    //if (res == DialogResult.OK)
                    //{


                    //        string[] OutObj = GenerateObj("name", mtl.FileName, V, VT, F,fMat); //new DLParser().GetParsedObject2(File.ReadAllBytes(open.FileName), imageOffsets, imageNames, mtl.FileName.Substring(0, mtl.FileName.Length - 4));
                    //    File.WriteAllLines(obj.FileName, OutObj);
                    //}
                    string[] OutObj = generateMtl(FolSelect.FileName + @"\exported.mtl", imageNames, RList, GList, BList);
                    File.WriteAllLines(FolSelect.FileName + @"\exported.mtl", OutObj);
                    OutObj = new DLParser().GetParsedObject2(File.ReadAllBytes(open.FileName), imageOffsets, imageNames, (FolSelect.FileName + @"exported.mtl").Substring(0, (FolSelect.FileName + @"exported.mtl").Length - 4));
                    File.WriteAllLines(FolSelect.FileName + @"\exported.obj", OutObj);


                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }

    }
        public int[] getvtPaint(byte[] Data, int DataOffset, int Length)
        {
            int CurOffset = DataOffset;
            int[] mtlCol = new int[3];
            for (int i = 0; i < Length; i++)
            {
                mtlCol[0] = Data[CurOffset + 12];
                mtlCol[1] = Data[CurOffset + 13];
                mtlCol[2] = Data[CurOffset + 14];
                CurOffset += 0x10;
            }
            return mtlCol;
        }


        public int[] getImgSize(int colval, int imgOffset, int palOffset, int FD4B)
        {
            int[] sizes = new int[2];
            if(imgOffset == 0)
            {
                sizes[0] = 1;
                sizes[1] = 1;
            }

            else if (imgOffset == palOffset)
            {
                sizes[0] = FD4B + 1;
                sizes[1] = FD4B + 1;
            }
            else
            {


                switch (colval)
                {
                    case (0):
                        if (palOffset - imgOffset == 32)
                        {
                            sizes[0] = 8;
                            sizes[1] = 8;
                        }
                        else if (palOffset - imgOffset == 128)
                        {
                            sizes[0] = 16;
                            sizes[1] = 16;
                        }
                        else if (palOffset - imgOffset == 512)
                        {
                            sizes[0] = 32;
                            sizes[1] = 32;
                        }
                        else if (palOffset - imgOffset == 1024)
                        {
                            sizes[0] = 32;
                            sizes[1] = 64;
                        }
                        else if (palOffset - imgOffset == 2048)
                        {
                            sizes[0] = 64;
                            sizes[1] = 64;
                        }
                        else
                        {
                            sizes[0] = 32;
                            sizes[1] = 32;
                        }
                        break;
                    case (1):
                        if (palOffset - imgOffset == 64)
                        {
                            sizes[0] = 8;
                            sizes[1] = 8;
                        }
                        else if (palOffset - imgOffset == 256)
                        {
                            sizes[0] = 16;
                            sizes[1] = 16;
                        }
                        else if (palOffset - imgOffset == 1024)
                        {
                            sizes[0] = 32;
                            sizes[1] = 32;
                        }
                        else if (palOffset - imgOffset == 4096)
                        {
                            sizes[0] = 64;
                            sizes[1] = 64;
                        }
                        else if (palOffset - imgOffset == 16384)
                        {
                            sizes[0] = 64;
                            sizes[1] = 64;
                        }
                        else
                        {
                            sizes[0] = 32;
                            sizes[1] = 32;
                        }
                        break;

                }
            }
            return sizes;
        }

        public void createImg(Graphics g, byte[] Data, int colval, int DataOffset, int PalOff, int SizeX, int SizeY)
        {
            byte[] Palette;
            switch (colval)
            {
                //ci4
                case (0):
                    {
                        Palette = new byte[32];
                        //int PalOff = (int)DataOffset + 0x200;
                        Array.Copy(Data, PalOff, Palette, 0, 32);
                        if (DataOffset != PalOff) N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.CI4, N64IMode.AlphaBinary);
                        else N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.I4, N64IMode.AlphaBinary);
                        break;
                    }
                //rgb16
                case (1):
                    {
                        Palette = new byte[128];
                        //int arrayoffset = (int)DataOffset + 0x400;
                        if(Data.Length > PalOff + 512)
                        {
                            Array.Copy(Data, PalOff, Palette, 0, 512);
                        }
                        else
                        {
                            Array.Copy(Data, PalOff, Palette, 0, Data.Length - PalOff);
                        }
                        
                        if(DataOffset != PalOff) N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.CI8, N64IMode.AlphaCopyIntensity);
                        else N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.I8, N64IMode.AlphaCopyIntensity);
                        break;
                    }
                //rgb16
                case (2):
                    {
                        Palette = new byte[32];
                        //int PalOff = (int)DataOffset + 0x200;
                        Array.Copy(Data, PalOff, Palette, 0, 32);
                        N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.RGBA16, N64IMode.AlphaBinary);
                        break;
                    }
                //rgba32
                case (3):
                    {
                        Palette = new byte[32];
                        //int PalOff = (int)DataOffset + 0x200;
                        Array.Copy(Data, PalOff, Palette, 0, 32);
                        N64GraphicsCoding.RenderTexture(g, Data, Palette, (int)DataOffset, SizeX, SizeY, 1, N64Codec.RGBA32, N64IMode.AlphaBinary);
                        break;
                    }
            }
        }
        public string[] generateMtl(string name, List<string> imageNames, List<string> R, List<string> G, List<string> B)
        {
            List<string> Output = new List<string>();
            int i = 1;
            Output.Add("# " + name);
            Output.Add("");

            Output.Add("newmtl 0");
            Output.Add("illum 1");
            Output.Add("Kd 0.000000 0.000000 0.000000");
            Output.Add("Ka 0.000000 0.000000 0.00000");
            Output.Add("Ks 0.000000 0.000000 0.000000");
            Output.Add("Ke 0.501961 0.501961 0.501961");
            Output.Add("Ns 0.000000");
            Output.Add("");

            for (int j = 0; j < imageNames.Count; j++)
            {
                Output.Add("newmtl " + i);
                Output.Add("illum 1");
                Output.Add("Kd " + R[j] + " " +  G[j] + " " +  B[j]);
                Output.Add("Ka " + R[j] + " " + G[j] + " " + B[j]);
                Output.Add("Ks 0.000000 0.000000 0.000000");
                Output.Add("Ke 0.501961 0.501961 0.501961");
                Output.Add("Ns 0.000000");
                Output.Add("map_Kd " + imageNames[j]);
                Output.Add("");
                i++;
            }

            return Output.ToArray();
        }
        public string[] GenerateObj(string name, string mtlname, List<string> V, List<string> VT, List<string> F, List<int> fMat)
        {
            List<string> OutPut = new List<string>();
            OutPut.Add("mtllib " + mtlname);
            OutPut.AddRange(V);
            OutPut.AddRange(VT);

            int group = 1;
            int currMtl = 0;

            if( fMat[0] == 0)
            {
            OutPut.Add("o 0");
            OutPut.Add("usemtl 0");
            }


            //faces
            for (int i = 0; i < F.Count; i++)
            {
                if(currMtl != fMat[i])
                {
                    OutPut.Add("o " + group);
                    OutPut.Add("usemtl " + fMat[i]);
                    currMtl = fMat[i];
                    group++;
                }
                OutPut.Add(F[i]);
            }
            //for (int group = 0; group < Groupst.Count; group++)
            //{
            //    if (Groupst[group] != Groupend[group] + 1)
            //    {
            //        OutPut.Add("o " + group);

            //        for (int j = 0; j < imageOffset.Count; j++)
            //        {
            //            if (StartOff[group] == imageOffset[j])
            //            {
            //                OutPut.Add("usemtl " + fMat[]);
            //                break;
            //            }
            //        }



            //        for (int j = Groupst[group]; j <= Groupend[group]; j++)
            //        {
            //            if (j < F.Count) OutPut.Add(F[j]);
            //        }
            //    }

            //}
            return OutPut.ToArray();
        }

    }
}

