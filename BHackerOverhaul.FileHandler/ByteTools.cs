using System;
using System.Collections.Generic;
using System.Text;

namespace BHackerOverhaul.FileHandler
{
    public static class ByteTools
    {
        public static byte[] Extend4MB(byte[] Input)
        {
            List<byte> InDat = new List<byte>();
            InDat.AddRange(Input);
            for (int i = 0; i < 4194304; i++)
            {
                InDat.Add(0xFF);
            }
            return InDat.ToArray();
        }

        public static UInt32 Read4Bytes(byte[] buf, UInt32 index)
        {
            return ((uint)(buf[index + 0] << 24 | buf[index + 1] << 16 | buf[index + 2] << 8 | buf[index + 3]));
        }

        public static byte[] TrimEnd(byte[] array)
        {
            int lastIndex = Array.FindLastIndex(array, b => b != 0);

            Array.Resize(ref array, lastIndex + 1);

            return array;
        }
    }
}
