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
        public string[] GetParsedObject(byte[] Data)
        {
           
            int[] Headers = new HeaderReader().ReadOffsets(Data);
            List<string> OutPut = new List<string>();
            List<string> V = new List<string>();
            List<string> F = new List<string>();
            int AdditionalFOffset = 0;

            try
            {
                foreach(int CurHeader in Headers)
                {
                    int CurPos = CurHeader;
                    int GeometryOffset = 0;
                    List<Connection> Con = new List<Connection>();
          
                    while(Data[CurPos] != 0xB8)
                    {
                        if(Data[CurPos] == 0x04)
                        {
                            GeometryOffset = CurPos;
                        }
                        else if (Data[CurPos] == 0xB1)
                        {
                            Connection c = new Connection();
                            c.Connection1 = Data[CurPos + 1] / 2 + 1;
                            c.Connection2 = Data[CurPos + 2] / 2 + 1;
                            c.Connection3 = Data[CurPos + 3] / 2 + 1;
                            Con.Add(c);
                            c = new Connection();
                            c.Connection1 = Data[CurPos + 5] / 2 + 1;
                            c.Connection2 = Data[CurPos + 6] / 2 + 1;
                            c.Connection3 = Data[CurPos + 7] / 2 + 1;
                            Con.Add(c);
                        }
                        else if(Data[CurPos] == 0xBF)
                        {
                            Connection c = new Connection();
                            c.Connection1 = Data[CurPos + 5] / 2 + 1;
                            c.Connection2 = Data[CurPos + 6] / 2 + 1;
                            c.Connection3 = Data[CurPos + 7] / 2 + 1;
                        }
                        CurPos += 0x08;
                    }
                    byte[] temp;
                    string BinLength = Convert.ToString(Data[GeometryOffset + 2], 2).PadLeft(8, '0');
                    BinLength = BinLength.Substring(0, 6);
                    int Length = Convert.ToInt32(BinLength, 2);
                    temp = new byte[4];
                    Array.Copy(Data, GeometryOffset + 4, temp, 0,4);
                    temp[0] = 0;
                    if(BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(temp);
                    }
                    GeometryOffset = BitConverter.ToInt32(temp,0);

                    foreach (Connection c in Con)
                    {
                        string Builder = "f ";
                        Builder += $"{c.Connection1 + AdditionalFOffset} {c.Connection2 + AdditionalFOffset} {c.Connection3 + AdditionalFOffset}";
                        F.Add(Builder);
                    }

                    for (int i  = 0; i < Length; i++)
                    {
                        string Builder = "v ";
                        int buf = 0;
                        temp = new byte[2];
                        while(buf != 6)
                        {
                            Array.Copy(Data, GeometryOffset + buf, temp, 0, 2);
                            if(BitConverter.IsLittleEndian)
                            {
                                Array.Reverse(temp);
                            }
                            Builder += BitConverter.ToInt16(temp,0).ToString() + " ";
                            buf += 2;
                        }
                        V.Add(Builder);
                        GeometryOffset += 0x10;

                        AdditionalFOffset++;
                    }

                    
                }
            }
            catch(Exception e)
            {

            }
            OutPut.AddRange(V);
            OutPut.AddRange(F);
            return OutPut.ToArray();
        }        
    }
}
