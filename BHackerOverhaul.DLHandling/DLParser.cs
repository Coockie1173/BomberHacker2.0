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
            return Vector3ToObj(ParseFile(Data));
        }

        public string[] Vector3ToObj(Tile[] Coords)
        {
            List<string> OutPut = new List<string>();
            List<string> V = new List<string>();
            List<string> F = new List<string>();

            int PrevTile = Coords[0].TileID;

            for(int i = 0; i < Coords.Length; i++)
            {
                try
                {
                    //if(Coords[i].TileID != PrevTile)
                    //{
                    //    //build the string 
                    //    if(PrevTile != 0)
                    //    {
                    //        string BuildString = "f";
                    //        foreach (int id in TempF)
                    //        {
                    //            BuildString += $" {id + 1}";
                    //        }
                    //        F.Add(BuildString);
                    //    }
                    //    PrevTile = Coords[i].TileID;
                    //    TempF.Clear();
                    //}
                    string vec = $"v {Coords[i].Pos.X} {Coords[i].Pos.Y} {Coords[i].Pos.Z}";
                    if(!V.Contains(vec))
                    {
                        V.Add(vec);
                    }
                }
                catch(Exception e)
                {
                    
                }
            }
            int PrevID = 0;
            //List<string> IDs = new List<string>();
            List<int> TempF = new List<int>();
            for(int i = 0; i < Coords.Length; i++)
            {
                //if(!OffsetIds.Contains(Coords[i].TileID))
                //{
                //    CoordOffsets.Add(new List<int>());
                //    OffsetIds.Add(Coords[i].TileID);
                //}
                //string vec = $"v {Coords[i].Pos.X} {Coords[i].Pos.Y} {Coords[i].Pos.Z}";
                //int vecOff = V.FindIndex(x => x == vec);
                //int Offs = OffsetIds.FindIndex(x => x == Coords[i].TileID);
                //CoordOffsets[Offs].Add(vecOff);                
                if(PrevID != Coords[i].TileID)
                {
                    if(PrevID != -1 && TempF.Count > 0)
                    {
                        string BuildF = "f ";
                        foreach (int id in TempF)
                        {
                            BuildF += (id + 1) + " ";
                        }
                        F.Add(BuildF);
                    }
                    PrevID = Coords[i].TileID;
                    TempF.Clear();
                }
                string vec = $"v {Coords[i].Pos.X} {Coords[i].Pos.Y} {Coords[i].Pos.Z}";
                int vecOff = V.FindIndex(x => x == vec);
                TempF.Add(vecOff);
            }
            OutPut.AddRange(V);
            OutPut.AddRange(F);

            return OutPut.ToArray();
        }

        public Tile[] ParseFile(byte[] Data)
        {
            List<Tile> OutPut = new List<Tile>();
            int[] HeaderOffsets = new HeaderReader().ReadOffsets(Data);

            //go over each DL command
            try
            {
                foreach (int offset in HeaderOffsets)
                {
                    try
                    {
                        int CurFilePos = offset;
                        int TileIDs = 0;
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
                                int TempCounter = 0;
                                #region ConvertData
                                TileIDs = -1;
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
                                    t.Pos = vec;
                                    temp = new byte[4];
                                    Array.Copy(Data, Offset + 0xC, temp, 0, 4);
                                    if (BitConverter.IsLittleEndian)
                                    {
                                        Array.Reverse(temp);
                                    }
                                    if(BitConverter.ToInt32(temp,0) == 0)
                                    {
                                        t.TileID = -1;
                                    }
                                    else
                                    {
                                        t.TileID = TileIDs;
                                    }
                                    OutPut.Add(t);
                                    Offset += 0x10;

                                    TempCounter++;

                                    if (Length % 3 == 0)
                                    {
                                        //tris
                                        if(TempCounter == 3)
                                        {
                                            TileIDs += 1;
                                            TempCounter = 0;
                                        }
                                    }
                                    else
                                    {
                                        //quads
                                        if (TempCounter == 4)
                                        {
                                            TileIDs += 1;
                                            TempCounter = 0;
                                        }
                                    }
                                }
                                
                                #endregion

                            }
                            CurFilePos += 0x08;
                        }
                    }
                    catch(Exception e)
                    {

                    }

                }                
            }
            catch(Exception e)
            {

            }

            return OutPut.ToArray();
        }
    }
}
