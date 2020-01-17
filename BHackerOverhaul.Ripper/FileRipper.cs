using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BHackerOverhaul.Compression;

namespace BHackerOverhaul.Ripper
{
    public class FileRipper
    {
        private const int TAmm = 14;

        private Compression.Compression comp = new Compression.Compression();

        private byte[] RomBuffer;
        private string RomPath_;

        public static readonly int[] ftable_arr = new int[TAmm]
        {
        0x00120000, 0x00140000, 0x00160000,
        0x00180000, 0x001C0000, 0x001E0000,
        0x00200000, 0x00240000, 0x00260000,
        0x00280000, 0x002A0000, 0x002C0000,
        0X002E0000, 0x00300000
        };

        private struct ftable_t
        {
            public UInt32 Table_Entry;
            public UInt32 Offset_Start_Data;
            public UInt32 Table_Index;
            public UInt32 Table_Offset;
            public UInt32 Fentry_Csize;
            public UInt32 Fentry_Ucsize;
            public UInt16 Table_Entry_Count;
            public byte[] fentry_data;
        }

        private static ftable_t[] tables = new ftable_t[TAmm];

        public void RipFiles(string RomPath)
        {
            byte[] ROM = File.ReadAllBytes(RomPath);
            RomBuffer = ROM;
            RomPath_ = RomPath;

            string FolderF = RomPath.Substring(0, RomPath.LastIndexOf("\\") + 1);
            Directory.CreateDirectory(FolderF + "Compressed");
            Directory.CreateDirectory(FolderF + "DeCompressed");

            init_table_data();

            List<Thread> ThreadList = new List<Thread>();

            //seperate case for Table 13, as that one is the biggest
            for (int i = 0; i < TAmm; i++)
            {
                Thread th = null;
                th = new Thread(() => parse_table(tables[i], (UInt32)i));
                th.SetApartmentState(ApartmentState.STA);
                th.IsBackground = true;
                ThreadList.Add(th);
                th.Start();
                Thread.Sleep(100);
            }
        }

        private void init_table_data()
        {
            for (int i = 0; i < TAmm; i++)
            {
                tables[i].Table_Entry = (UInt32)ftable_arr[i];
                tables[i].Offset_Start_Data = 0x2008;
                tables[i].Table_Index = 0;
                tables[i].Table_Entry_Count = 0;
                tables[i].Table_Offset = 0;
                tables[i].Fentry_Csize = 0;
                tables[i].Fentry_Ucsize = 0;
            }
        }

        private void parse_table(ftable_t tab, UInt32 num)
        {
            tab.Table_Entry_Count = Convert.ToUInt16((num == 13) ? 0x0368 : RomBuffer[tab.Table_Entry + tab.Offset_Start_Data]);
            if (num == 0xD)
            {
                tab.Table_Entry_Count = 1000;
            }
            int temp = tab.Table_Entry_Count;
            UInt32 Table_Index = 0x10;
            for (int i = 0; i <= tab.Table_Entry_Count; i++)
            {
                //if (num == 0xD && i == 47)
                //{
                //    break;
                //}

                tab.Table_Offset = Read4Bytes(RomBuffer, tab.Table_Entry + Table_Index);
                if (tab.Table_Offset == 0xFFFFFFFF)
                {
                    //DebugBox.Text += "Error trying to read another entry " + tab.Table_Offset.ToString() + Environment.NewLine;
                    break;
                }

                tab.Fentry_Csize = Read4Bytes(RomBuffer, (tab.Table_Entry + Table_Index) + 0x4);

                if (i == 35)
                {
                    //DebugBox.Text += "Huge Entry + " + num + "_" + i + Environment.NewLine;
                }

                if (tab.Fentry_Csize > 0x00010000 && i != 34)
                {
                    //DebugBox.Text += "Next entry is huge. Uncompressed file, moving along..." + Environment.NewLine;
                    //DebugBox.Text += "Huge Entry + " + num + "_" + i + Environment.NewLine;
                    /*if (tab.Fentry_Csize < 2147483648)
                    {
                        
                        tab.fentry_data = new byte[sizeof(byte) + tab.Fentry_Csize];
                        Array.Copy(RomBuffer, tab.Table_Index, tab.fentry_data, 0, tab.Fentry_Csize);
                        File.WriteAllBytes(PathBox.Text.Substring(0, PathBox.Text.LastIndexOf("\\") + 1) + "DeCompressed" + "\\" + "Table " + num + "_" + i, tab.fentry_data);
                    }
                    if (i == 0x22 && num == 0xD)
                    {
                        tab.Fentry_Ucsize = Read4Bytes(RomBuffer, (tab.Table_Entry + tab.Offset_Start_Data) + tab.Table_Offset);
                        //unsafe, not sure if correct
                        tab.fentry_data = new byte[sizeof(byte) + tab.Fentry_Csize];
                        Array.Copy(RomBuffer, tab.Table_Index, tab.fentry_data, 0, tab.Fentry_Csize);
                        File.WriteAllBytes(PathBox.Text.Substring(0, PathBox.Text.LastIndexOf("\\") + 1) + "Compressed" + "\\" + "Table " + num + "_" + i, tab.fentry_data);


                        string tmp = new string(new char[30]);


                        byte[] Decomp = new byte[307200];
                        byte[] Decompressed = new byte[307200];

                        tmp = string.Format("decompressed//FT_{0:X2}_F_{1:X2}.bin", num, i);

                        string tmp_decomp = new string(new char[307200]);


                        Decompressed = Decode(tab.fentry_data, tab.Fentry_Csize, tab.Fentry_Ucsize * 4, 0x4);

                        byte[] DecompOut = new byte[TrimEnd(Decompressed).Length];

                        DecompOut = TrimEnd(Decompressed);


                        File.WriteAllBytes(PathBox.Text.Substring(0, PathBox.Text.LastIndexOf("\\") + 1) + "DeCompressed" + "\\" + "Table " + num + "_" + i, DecompOut);
                        
                    }*/
                }
                else
                {

                    tab.Fentry_Ucsize = Read4Bytes(RomBuffer, (tab.Table_Entry + tab.Offset_Start_Data) + tab.Table_Offset);
                    //unsafe, not sure if correct
                    tab.fentry_data = new byte[sizeof(byte) + tab.Fentry_Csize];
                    Array.Copy(RomBuffer, Table_Index, tab.fentry_data, 0, tab.Fentry_Csize);
                    for (UInt32 pos = 0; pos < tab.Fentry_Csize; pos++)
                    {
                        tab.fentry_data[pos] = RomBuffer[((tab.Table_Entry + tab.Offset_Start_Data) + tab.Table_Offset) + pos];
                    }

                    string tmp = new string(new char[30]);


                    byte[] Decomp = new byte[307200];
                    byte[] Decompressed = new byte[307200];

                    tmp = string.Format("decompressed//FT_{0:X2}_F_{1:X2}.bin", num, i);

                    string tmp_decomp = new string(new char[307200]);


                    Decompressed = comp.Decompress(tab.fentry_data, tab.Fentry_Csize, 307200, 0x4);

                    //List<byte> ActDecomp = new List<byte>();
                    //ActDecomp = Decompressed.ToList();

                    byte[] DecompOut = new byte[TrimEnd(Decompressed).Length];

                    DecompOut = TrimEnd(Decompressed);
                    /*
                    if (DecompOut.Length - 1 > tab.Fentry_Ucsize)
                    {
                        DecompOut = DecompOut.ToList().GetRange(0, (int)tab.Fentry_Ucsize).ToArray();
                    }
                    */

                    //this can be done in an easier fashion
                    string tempo = i.ToString();

                    if (tempo.Length == 1)
                    {
                        tempo = "00" + tempo;
                    }
                    else if (tempo.Length == 2)
                    {
                        tempo = "0" + tempo;
                    }

                    //File.WriteAllBytes(PathBox.Text.Substring(0, PathBox.Text.LastIndexOf("\\") + 1) + "DeCompressed" + "\\" + "Table " + num + "_" + tempo + ".bin", DecompOut);
                    //File.WriteAllBytes(PathBox.Text.Substring(0, PathBox.Text.LastIndexOf("\\") + 1) + "Compressed" + "\\" + "Table " + num + "_" + i + ".bin", tab.fentry_data);

                    string FileName = (RomPath_.Substring(0, RomPath_.LastIndexOf("\\") + 1) + "DeCompressed" + "\\" + "Table " + num + "_" + tempo + ".bin");

                    FileStream file = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                    file.Write(DecompOut, 0, DecompOut.Length);
                    file.Close();

                    FileName = (RomPath_.Substring(0, RomPath_.LastIndexOf("\\") + 1) + "Compressed" + "\\" + "Table " + num + "_" + i + ".bin");

                    file = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                    file.Write(tab.fentry_data, 0, tab.fentry_data.Length);
                    file.Close();

                    //debug stuff
                    string newline = Environment.NewLine;
                    //DebugBox.Text += string.Format("[TABLE {0} {1}]{2}Table Offset {3}{2}Compressed size {4}{2}" +
                    //   "Decompressed size {5}{2}Table Index {6} {2}Entry Count {7}{2}{8}{2}{2}", num, tab.Table_Entry, newline, tab.Table_Offset, tab.Fentry_Csize, tab.Fentry_Ucsize, i, tab.Table_Entry_Count, Table_Index);
                }
                Table_Index += 0x08;
            }

            Thread.EndThreadAffinity();
        }

        public UInt32 Read4Bytes(byte[] buf, UInt32 index)
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
