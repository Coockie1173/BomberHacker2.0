using System;
using System.Collections.Generic;
using System.Text;

namespace BHackerOverhaul.FileHandler
{
    public class HeaderReader
    {
        public string ReadHeader(byte[] file)
        {
            string OutPut = "";
            UInt32 Length = ByteTools.Read4Bytes(file, (UInt32)0x4);
            OutPut += string.Format("Header length: {0}{1}", Length, Environment.NewLine + Environment.NewLine);

            int Inpos = 0xC;

            for (int i = 0; i < Length; i++)
            {
                string InHex = Inpos.ToString("X");

                OutPut += string.Format("Data offset in file: {0}", InHex + Environment.NewLine);
                UInt32 OffsetterInt = ByteTools.Read4Bytes(file, (UInt32)(Inpos));
                OutPut += string.Format("{2}: Unknown Int: {0}{1}", OffsetterInt, Environment.NewLine, i);

                byte[] Floater = new byte[4];
                for (int j = 0; j != 4; j++)
                {
                    Floater[j] = file[Inpos + j + 4];
                }
                Array.Reverse(Floater);
                float UnknownFloat = BitConverter.ToSingle(Floater, 0);
                OutPut += string.Format("{2}: Unknown Float: {0}{1}", UnknownFloat, Environment.NewLine, i);

                UInt32 UnknownInt = ByteTools.Read4Bytes(file, (UInt32)(Inpos + 8));

                string hexValue = UnknownInt.ToString("X");

                OutPut += string.Format("{2}: Offset: {0}{1}", hexValue, Environment.NewLine + Environment.NewLine, i);

                Inpos += 0xC;
            }

            return OutPut;
        }

        public int[] ReadOffsets(byte[] file)
        {
            List<int> OutPut = new List<int>();
            UInt32 Length = ByteTools.Read4Bytes(file, (UInt32)0x4);

            int Inpos = 0xC;

            for (int i = 0; i < Length; i++)
            {
                UInt32 UnknownInt = ByteTools.Read4Bytes(file, (UInt32)(Inpos + 8));

                string hexValue = UnknownInt.ToString("X");

                OutPut.Add((int)UnknownInt);

                Inpos += 0xC;
            }

            return OutPut.ToArray();
        }
    }

}
