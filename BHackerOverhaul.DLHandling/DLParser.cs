using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHackerOverhaul.FileHandler;
using BHackerOverhaul._3DData;

namespace BHackerOverhaul.DLHandling
{
    public class DLParser
    {
        public Vector3[] ParseFile(byte[] Data)
        {
            List<Vector3> OutPut = new List<Vector3>();
            int[] HeaderOffsets = new HeaderReader().ReadOffsets(Data);

            //go over each DL command
            foreach(int offset in HeaderOffsets)
            {
                int CurFilePos = offset;
                while(Data[CurFilePos] != 0xB8)
                {
                    if(Data[CurFilePos] == 0x04)
                    {
                        //load command spotted
                        //get length
                        string BinLength = Convert.ToString(Data[CurFilePos + 2], 2).PadLeft(8, '0');
                        BinLength = BinLength.Substring(0, BinLength.Length - 2);
                        int Length = Convert.ToInt32(BinLength, 2);
                        int Offset = Convert.ToInt32(new byte[2] {Data[CurFilePos + 6], Data[CurFilePos + 7] });
                        for (int i = 0; i < Length; i++)
                        {
                            Vector3 vec = new Vector3();
                            int CurOffset = 0x10 * i;
                            Int16 buf = Convert.ToInt16(new byte[2] {Data[offset], Data[offset + 1] });
                            vec.X = (float)buf;
                            buf = Convert.ToInt16(new byte[2] { Data[offset + 2], Data[offset + 3] });
                            vec.Y = (float)buf;
                            buf = Convert.ToInt16(new byte[2] { Data[offset + 4], Data[offset + 5] });
                            vec.Z = (float)buf;
                            OutPut.Add(vec);
                        }
                    }
                    CurFilePos += 0x08;
                }
            }
            return OutPut.ToArray();
        }
    }
}
