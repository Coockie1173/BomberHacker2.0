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
using BHackerOverhaul.Injection;
using BHackerOverhaul.FileHandler;
using BHackerOverhaul.N64Graphics;

namespace BHackerOverhaul.MainForm
{
    public partial class InjectorForm : Form
    {
        public InjectorForm()
        {
            InitializeComponent();
        }

        private void InjectorForm_Activated(object sender, EventArgs e)
        {
            ROMPathBox.Text = GlobalData.Instance.ROMPath;
        }

        private void InjectIMGBut_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "bin file|*.bin";
            DialogResult res = open.ShowDialog();
            if(res == DialogResult.OK)
            {
                byte[] binfile = File.ReadAllBytes(open.FileName);
                Injector inj = new Injector();
                N64Codec codec = N64Codec.CI4;
                switch(SelectedCodec.SelectedItem)
                {
                    case ("CI4"):
                        {
                            codec = N64Codec.CI4;
                            break;
                        }
                    case ("CI8"):
                        {
                            codec = N64Codec.CI8;
                            break;
                        }
                }
                open.Filter = "bitmap file|*.bmp|png file|*.png|jpg file|*.jpg";
                res = open.ShowDialog();
                if(res == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(open.FileName);
                    byte[] OutPut = inj.InjectImageIntoByteArray((int)ImgNum.Value, (int)PalleteNum.Value, binfile, bmp, codec);
                    SaveFileDialog save = new SaveFileDialog();
                    save.Filter = "bin file|*.bin";
                    res = save.ShowDialog();
                    if(res == DialogResult.OK)
                    {
                        File.WriteAllBytes(save.FileName, OutPut);
                    }
                }
            }
        }

        private void InjectFileBut_Click(object sender, EventArgs e)
        {
            Injector inj = new Injector();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "bin file|*.bin";
            DialogResult res = open.ShowDialog();
            if(res == DialogResult.OK)
            {
                byte[] OutPut = inj.InjectIntoROM((int)TableIDNum.Value, (int)FileIDNum.Value, File.ReadAllBytes(open.FileName), CompressedBox.Checked);
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "z64 rom file|*.z64";
                res = save.ShowDialog();
                if(res == DialogResult.OK)
                {
                    File.WriteAllBytes(save.FileName,OutPut);
                }
            }
        }

        private void ChangeROMBox_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "z64 file|*.z64";
            DialogResult res = open.ShowDialog();
            if(res == DialogResult.OK)
            {
                GlobalData.Instance.ROMPath = open.FileName;
                GlobalData.Instance.ROM = File.ReadAllBytes(open.FileName);
            }
        }
    }
}
