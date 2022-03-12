using System;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace BHackerOverhaul.FileHandler
{
    public class Handler
    {
        public string GetROMPath()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "ROM File|*.rom|Z64 file|*.z64";
            if (open.ShowDialog() == DialogResult.OK)
            {
                byte[] Buf = File.ReadAllBytes(open.FileName);
                string BomberMan = GetHeaderName(Buf);
                if (BomberMan == "BOMBERMAN64U")
                {
                    return open.FileName;
                }
                else
                {
                    throw new Exception("Not a bomberman ROM");
                }
            }
            throw new Exception("No valid path given.");
        }

        public string GetHeaderName(byte[] Buf)
        {
            byte[] BomberManCheck = new byte[12];
            Array.Copy(Buf, 0x20, BomberManCheck, 0, 12);
            string BomberMan = Encoding.ASCII.GetString(BomberManCheck);
            return BomberMan;
        }
    }
}
