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
            //return Vector3ToObj(ParseFile(Data));
            Connection[] cn = null;
            Tile[] tl = null;
            ParseFile(Data, out tl, out cn);
            return Vector3ToObj(tl, cn);
        }

        public string[] Vector3ToObj(Tile[] Coords, Connection[] connections)
        {
            List<string> V = new List<string>();
            List<string> F = new List<string>();
            List<string> OutPut = new List<string>();

            int StartIdentifier = -1;
            int AddedOffset = 1;
            foreach (Tile T in Coords)
            {
                if(StartIdentifier == -1)
                {
                    if(T.InvisIdentifier != 0)
                    {
                        StartIdentifier = T.TileID;
                    }
                    else
                    {
                        AddedOffset++;
                    }
                }
                string temp = "v ";
                temp += T.Pos.ToString();
                V.Add(temp);
            }
            int PrevHeader = StartIdentifier;
            int Delta = 0;

            foreach(Connection C in connections)
            {                
                if(PrevHeader != C.HeaderID)
                {
                    PrevHeader = C.HeaderID;
                    AddedOffset += Delta;
                    Delta = 0;
                }
                string temp = "f ";
                temp += $"{C.Connection1 + AddedOffset} {C.Connection2 + AddedOffset} {C.Connection3 + AddedOffset}";
                F.Add(temp);
                Delta++;
            }

            OutPut.AddRange(V);
            OutPut.AddRange(F);
            return OutPut.ToArray();
        }

        public void ParseFile(byte[] Data, out Tile[] Vectors, out Connection[] Connections)
        {            
            List<Tile> OutPut = new List<Tile>();
            List<Connection> Cnections = new List<Connection>();
            int[] HeaderOffsets = new HeaderReader().ReadOffsets(Data);
            int CurHeader = 0;

            //go over each DL command
            try
            {
                foreach (int offset in HeaderOffsets)
                {
                    try
                    {
                        int CurFilePos = offset;
                        //int VecOffset = 0;
                        while (Data[CurFilePos] != 0xB8)
                        {
                            if (Data[CurFilePos] == 0x04)
                            {
                                //load command spotted
                                //get length
                                string BinLength = Convert.ToString(Data[CurFilePos + 2], 2).PadLeft(8, '0');
                                BinLength = BinLength.Substring(0, BinLength.Length - 2);
                                int Length = Convert.ToInt32(BinLength, 2);
                                byte[] temp = new byte[2] { Data[CurFilePos + 6], Data[CurFilePos + 7] };
                                if (BitConverter.IsLittleEndian)
                                {
                                    Array.Reverse(temp);
                                }
                                int Offset = BitConverter.ToInt16(temp, 0);
                                int LastIdentifier = 10;
                                for (int i = 0; i < Length; i++)
                                {
                                    Vector3 vec = new Vector3();
                                    temp = new byte[2] { Data[Offset], Data[Offset + 1] };
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        Array.Reverse(temp);
                                    }
                                    Int16 buf = BitConverter.ToInt16(temp, 0);
                                    vec.X = (float)buf;

                                    temp = new byte[2] { Data[Offset + 2], Data[Offset + 3] };
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        Array.Reverse(temp);
                                    }
                                    buf = BitConverter.ToInt16(temp, 0);
                                    vec.Y = (float)buf;

                                    temp = new byte[2] { Data[Offset + 4], Data[Offset + 5] };
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        Array.Reverse(temp);
                                    }
                                    buf = BitConverter.ToInt16(temp, 0);
                                    vec.Z = (float)buf;

                                    Tile t = new Tile();

                                    temp = new byte[4];
                                    Array.Copy(Data, Offset + 0xC, temp, 0, 4);
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        Array.Reverse(temp);
                                    }
                                    t.InvisIdentifier = BitConverter.ToInt32(temp, 0);

                                    t.Pos = vec;
                                    t.TileID = CurHeader;
                                    LastIdentifier = t.InvisIdentifier;
                                    if(t.InvisIdentifier != 0)
                                    {
                                        OutPut.Add(t);
                                    }                    
                                    Offset += 0x10;

                                }
                                if(LastIdentifier != 0)
                                {
                                    CurHeader++;
                                }
                                else
                                {
                                    //nuttin
                                }
                            }
                            else if(Data[CurFilePos] == 0xB1)
                            {
                                //tri2 command
                                //first connection
                                Connection con = new Connection();
                                con.Connection1 = Data[CurFilePos + 1] / 2;
                                con.Connection2 = Data[CurFilePos + 2] / 2;
                                con.Connection3 = Data[CurFilePos + 3] / 2;

                                con.HeaderID = CurHeader - 1;

                                Cnections.Add(con);

                                //second connection
                                con = new Connection();
                                con.HeaderID = CurHeader - 1;
                                con.Connection1 = Data[CurFilePos + 5] / 2;
                                con.Connection2 = Data[CurFilePos + 6] / 2;
                                con.Connection3 = Data[CurFilePos + 7] / 2;
                                Cnections.Add(con);
                            }
                            CurFilePos += 0x08;
                        }
                    }
                    catch(Exception e)
                    {
                        throw e;
                    }

                }                
            }
            catch(Exception e)
            {
                throw e;
            }

            Vectors = OutPut.ToArray();
            Connections = Cnections.ToArray();
        }
    }
}
