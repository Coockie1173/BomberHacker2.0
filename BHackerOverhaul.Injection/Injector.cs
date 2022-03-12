using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHackerOverhaul.Compression;
using BHackerOverhaul.N64Graphics;
using BHackerOverhaul.FileHandler;

namespace BHackerOverhaul.Injection
{
    public class Injector
    {
        //This works fine as shared
        /// <summary>
        /// Injects a custom bitmap into a .bin file
        /// </summary>
        /// <param name="OriginalData">The original file we want to import our image into</param>
        /// <param name="Image">Our image to import</param>
        /// <param name="Codec">The N64Codec to use</param>
        /// <param name="ImageOffset">The offset of the image within the file</param>
        /// <param name="PaletteOffset">The offset of the palette within the file</param>
        public byte[] InjectImageIntoByteArray(int ImageOffset, int PaletteOffset, byte[] OriginalData, Bitmap Image, N64Codec Codec)
        {
            byte[] imageData = null, paletteData = null;

            N64GraphicsCoding.Convert(ref imageData, ref paletteData, Codec, Image);

            ByteTools.TrimEnd(paletteData);

            Array.Copy(imageData, 0, OriginalData, ImageOffset, imageData.Length);
            Array.Copy(paletteData, 0, OriginalData, PaletteOffset, paletteData.Length);

            return OriginalData;
        }

        /// <summary>
        /// injects bytes in the globaldata ROM
        /// </summary>
        /// <param name="TableID">The filetable to instert into</param>
        /// <param name="FileID">The FileID</param>
        /// <param name="Data">Uncompressed filedata to insert</param>
        /// <returns>Returns ROM with custom file</returns>
        public byte[] InjectIntoROM(int TableID, int FileID, byte[] Data, bool PreCompressed)
        {
            byte[] ROM = GlobalData.Instance.ROM;
            int FileTableOffset = GlobalData.Instance.ftable_arr[TableID];

            byte[] CompressedData = Compression.Compression.CompressInflate(Data);
            int[] TableOffsets = GlobalData.Instance.ftable_arr;

            int Offset = TableOffsets[TableID] + 0x10;
            Offset += FileID * 8;

            UInt32 FileOffset = ByteTools.Read4Bytes(ROM, (UInt32)Offset) + (UInt32)TableOffsets[TableID] + 0x2008;

            int CompressedSize = CompressedData.Length - 1;
            int UnCompressedSize = Data.Length - 1;

            //going to assume last file = final offset

            UInt32 OrigCompSize = ByteTools.Read4Bytes(ROM, (uint)Offset + 4);
            UInt32 LastIndex = 0x301B48;
            UInt32 LastCompression = ByteTools.Read4Bytes(ROM, LastIndex + 4);
            UInt32 LastUsedAddr = ByteTools.Read4Bytes(ROM, LastIndex) + LastCompression;

            if(LastUsedAddr + Math.Abs(OrigCompSize - CompressedSize) > ROM.Length)
            {
                ROM = ByteTools.Extend4MB(ROM);
            }

            List<byte> RomL = new List<byte>();
            RomL.AddRange(ROM);

            //get original compressed size and take out old data
            CompressedSize += 4; //compensation for first 4 bytes

            //get rid of original data
            RomL.RemoveRange((int)FileOffset, (int)OrigCompSize);

            //insert new data
            byte[] buf = BitConverter.GetBytes(UnCompressedSize);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }

            RomL.InsertRange((int)FileOffset, buf);
            RomL.InsertRange((int)FileOffset + 4, CompressedData);

            //other offsets are still not correct
            //fix overwritten file sizes
            buf = BitConverter.GetBytes(CompressedSize);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }

            RomL.RemoveRange(Offset + 4, 4);
            RomL.InsertRange(Offset + 4, buf);

            //fix all other offsets
            int BufOffset = Offset + 8;
            while(ByteTools.Read4Bytes(ROM, (uint)BufOffset) != 0xFFFFFFFF)
            {
                int OffsetToChange = (int)OrigCompSize - ((int)CompressedSize + 1);
                int ChangedOffset = (int)ByteTools.Read4Bytes(ROM, (uint)BufOffset) - OffsetToChange;
                RomL.RemoveRange(BufOffset, 4);
                buf = BitConverter.GetBytes(ChangedOffset);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buf);
                }
                RomL.InsertRange(BufOffset, buf);
                BufOffset += 8;
            }

            while(RomL.Count - 1 < 0x7FFFFF)
            {
                RomL.Add(0x00);
            }

            //trim ROM
            while((RomL.Count - 1) % 0x400000 != 0x3FFFFF)
            {
                RomL.RemoveAt(RomL.Count - 1);
            }

            return RomL.ToArray();
        }
    }
}
