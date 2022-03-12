using System;
using System.Collections.Generic;
using System.Text;

namespace BHackerOverhaul.FileHandler
{
    public sealed class GlobalData
    {
        public byte[] ROM
        {
            get;set;
        }
        public string ROMPath { get; set; }
        //singleton. For when we request our ROM for instance, we can just call GlobalData
        private GlobalData()
        {

        }

        private const int TAmm = 14;
        public readonly int[] ftable_arr = new int[TAmm]
        {
        0x00120000, 0x00140000, 0x00160000,
        0x00180000, 0x001C0000, 0x001E0000,
        0x00200000, 0x00240000, 0x00260000,
        0x00280000, 0x002A0000, 0x002C0000,
        0X002E0000, 0x00300000
        };

        private static GlobalData instance = null;
        private static readonly object padlock = new object();
        public static GlobalData Instance
        {
            get
            {
                lock(padlock)
                {
                    if (instance == null)
                    {
                        instance = new GlobalData();
                    }
                    return instance;
                }
            }
        }
    }
}
