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
            List<string> V = new List<string>();
            List<string> F = new List<string>();
            List<string> OutPut = new List<string>();
            int[] Headers = new HeaderReader().ReadOffsets(Data);
            int AdditionalOffset = 1;
            List<DLObject> Temp = new List<DLObject>();

            try
            {
                foreach (int Offset in Headers)
                {
                    int CurOffset = Offset;
                    DLObject Buf = new DLObject();
                    try
                    {
                        while (Data[CurOffset] != 0xB8)
                        {
                            if (Data[CurOffset] == 0x04)
                            {
                                Temp.Add(Buf);
                                Buf = new DLObject();
                                byte[] Pos = new byte[4];
                                Array.Copy(Data, CurOffset + 4, Pos, 0, 4);
                                Pos[0] = 0;
                                if(BitConverter.IsLittleEndian)
                                {
                                    Array.Reverse(Pos);
                                }
                                Buf.DataOffset = BitConverter.ToInt32(Pos, 0);

                                string Length = Convert.ToString(Data[CurOffset + 2], 2).PadLeft(8, '0');
                                Length = Length.Substring(0, 6);
                                Buf.Length = Convert.ToInt32(Length, 2);
                            }
                            else if (Data[CurOffset] == 0xB1)
                            {
                                Connection c = new Connection();
                                c.Connection1 = Data[CurOffset + 1] / 2;
                                c.Connection2 = Data[CurOffset + 2] / 2;
                                c.Connection3 = Data[CurOffset + 3] / 2;
                                Buf.connections.Add(c);

                                c = new Connection();
                                c.Connection1 = Data[CurOffset + 5] / 2;
                                c.Connection2 = Data[CurOffset + 6] / 2;
                                c.Connection3 = Data[CurOffset + 7] / 2;
                                Buf.connections.Add(c);
                            }
                            else if (Data[CurOffset] == 0xBF)
                            {
                                Connection c = new Connection();
                                c.Connection1 = Data[CurOffset + 5] / 2;
                                c.Connection2 = Data[CurOffset + 6] / 2;
                                c.Connection3 = Data[CurOffset + 7] / 2;
                                Buf.connections.Add(c);
                            }

                            CurOffset += 0x08;
                        }
                        Temp.Add(Buf);
                    }
                    catch(Exception ex)
                    {

                    }
                }

                foreach (DLObject obj in Temp)
                {
                    foreach (Connection connection in obj.connections)
                    {
                        string builder = "f ";
                        builder += $"{connection.Connection1 + AdditionalOffset} {connection.Connection2 + AdditionalOffset} {connection.Connection3 + AdditionalOffset}";
                        F.Add(builder);
                    }

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

                        AdditionalOffset++;
                        CurOffset += 0x10;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            OutPut.AddRange(V);
            OutPut.AddRange(F);
            return OutPut.ToArray();
        }        
    }
}
