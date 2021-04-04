using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHackerOverhaul.SetupHandler
{
    public struct Section
    {
        public List<SubSection> SubSections;
        public byte AmmSubsections;
        public byte AmmPartsInSubsections;
        public byte UnkByte;
        public int SubsectionLength;
        public int SectionOffset;
    }

    public struct SubSection
    {
        public byte HeaderbyteOne;
        public byte HeaderbyteTwo;
        public List<ushort> Data;
        //dirty code but lazy
        public List<int> Offsets;
    }

    public class GetSetupNodeTree
    {
        const int SectionLength = 0xC33;
        const int SubsectionLength = 0x080;
        public static int AmmSections;

        public static Section[] GetSections(string FilePath)
        {
            List<Section> Sections = new List<Section>();
            List<byte> Data = new List<byte>();
            Data.AddRange(File.ReadAllBytes(FilePath));

            AmmSections = Data[0];

            int CurOffset = 5;
            //int ParsedSections = 0;

            for (int ParsedSections = 0; ParsedSections < AmmSections; ParsedSections++)
            {
                Section Cursec = new Section();
                Cursec.SubSections = new List<SubSection>();
                Cursec.UnkByte = Data[CurOffset + 2];
                Cursec.AmmSubsections = Data[CurOffset + 1];
                Cursec.AmmPartsInSubsections = Data[CurOffset];
                Cursec.SectionOffset = CurOffset;
                //Cursec.SubsectionLength = (int)SectionLength / ((int)Cursec.AmmSubsections * (int)Cursec.AmmPartsInSubsections);
                Cursec.SubsectionLength = 0x80;
                //0xC33 / (Amm sub sections * Amm parts) = Length + 2 (Header Bytes for sub section)

                CurOffset += 3;

                for (int S = 0; S < 0xC33 / 0x80; S++)
                {
                    SubSection s = new SubSection();
                    s.HeaderbyteOne = Data[CurOffset];
                    s.HeaderbyteTwo = Data[CurOffset + 1];
                    s.Data = new List<ushort>();
                    s.Offsets = new List<int>();
                    CurOffset += 2;
                    for (int i = 0; i < SubsectionLength; i += 2)
                    {
                        s.Data.Add(BitConverter.ToUInt16(Data.ToArray(), CurOffset));
                        s.Offsets.Add(CurOffset);
                        CurOffset += 2;
                    }
                    Cursec.SubSections.Add(s);
                }

                Sections.Add(Cursec);             
            }

            return Sections.ToArray();
        }
    }
}
