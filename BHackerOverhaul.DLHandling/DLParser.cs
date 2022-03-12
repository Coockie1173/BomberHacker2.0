using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHackerOverhaul.FileHandler;
using BHackerOverhaul._3DData;
using System.Windows;

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

        public string[] GetParsedObject2(byte[] Data, List<int> imageOffset, List<string> imageNames, string fileName)
        {
            int i;
            int cCount;
            List<string> V = new List<string>();
            List<string> VT = new List<string>();
            List<string> F = new List<string>();
            List<string> OutPut = new List<string>();
            //List<string> ImgName = new List<string>();
            List<int> StartOff = new List<int>();
            List<int> EndOff = new List<int>();
            List<int> Groupst = new List<int>();
            List<int> Groupend = new List<int>();
            int[] Headers = new HeaderReader().ReadOffsets(Data);
            int AdditionalOffset = 1;
            List<DLObject> Temp = new List<DLObject>();

            try
            {
                i = -1;
                cCount = -1;
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
                                i++;
                                Temp.Add(Buf);
                                StartOff.Add(0);
                                EndOff.Add(0);
                                Groupst.Add(cCount + 1);
                                Groupend.Add(0);
                                //Groupst[i] = Temp.con;
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
                            }
                            else if (Data[CurOffset] == 0xB1)
                            {
                                Connection c = new Connection();
                                c.Connection1 = Data[CurOffset + 1] / 2;
                                c.Connection2 = Data[CurOffset + 2] / 2;
                                c.Connection3 = Data[CurOffset + 3] / 2;
                                Buf.connections.Add(c);
                                cCount++;

                                c = new Connection();
                                c.Connection1 = Data[CurOffset + 5] / 2;
                                c.Connection2 = Data[CurOffset + 6] / 2;
                                c.Connection3 = Data[CurOffset + 7] / 2;
                                Buf.connections.Add(c);
                                cCount++;
                            }
                            else if (Data[CurOffset] == 0xBF)
                            {
                                Connection c = new Connection();
                                c.Connection1 = Data[CurOffset + 5] / 2;
                                c.Connection2 = Data[CurOffset + 6] / 2;
                                c.Connection3 = Data[CurOffset + 7] / 2;
                                Buf.connections.Add(c);
                                cCount++;
                            }
                            else if (Data[CurOffset] == 0xFD)
                            {
                                if (EndOff[i] == 0)
                                {
                                    EndOff[i] = Data[CurOffset + 5] * 65536 + Data[CurOffset + 6] * 256 + Data[CurOffset + 7];
                                }
                                else
                                {
                                    StartOff[i] = Data[CurOffset + 5] * 65536 + Data[CurOffset + 6] * 256 + Data[CurOffset + 7];
                                }
                            }
                            CurOffset += 0x08;
                            Groupend[i] = cCount;
                        }
                        Temp.Add(Buf);

                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }

                foreach (DLObject obj in Temp)
                {


                    int CurOffset = obj.DataOffset;

                    for (i = 0; i < obj.Length; i++)
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
                        //VertexTexture.X = (float)temp[1]/4;
                        //VertexTexture.X = (float)temp[1] / (float)4;
                        VertexTexture.X = (float)BitConverter.ToInt16(temp, 0) / 1024;

                        temp = new byte[2];
                        Array.Copy(Data, CurOffset + 10, temp, 0, 2);
                        if (BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(temp);
                        }
                        //VertexTexture.Y =  (float)temp[1] / (float)4;
                        VertexTexture.Y = (float)BitConverter.ToInt16(temp, 0) / 1024;


                        VT.Add("vt " + VertexTexture.X.ToString() + " " + VertexTexture.Y.ToString());

                        AdditionalOffset++;
                        CurOffset += 0x10;
                    }


                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            try
            {
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
                            
                        }
                    }
                     AdditionalOffset+= obj.Length;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }




            OutPut.Add("mtllib " + fileName + ".mtl");
            OutPut.AddRange(V);
            OutPut.AddRange(VT);
            for (int group = 0; group < Groupst.Count; group++)
            {
                if(Groupst[group] != Groupend[group] + 1)
                {
                    OutPut.Add("o " + group);

                    for(int j = 0; j < imageOffset.Count; j++)
                    {
                        if (StartOff[group] == imageOffset[j])
                        {
                            OutPut.Add("usemtl " + j);
                            break;
                        }
                    }



                    for (int j = Groupst[group]; j <= Groupend[group]; j++)
                    {
                        if(j < F.Count) OutPut.Add(F[j]);
                    }
                }

    }





    return OutPut.ToArray();
} 
    }
}


       
