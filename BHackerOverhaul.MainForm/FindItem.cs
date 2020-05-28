using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BHackerOverhaul.FileHandler;

namespace BHackerOverhaul.MainForm
{
    public partial class FindItem : Form
    {
        public FindItem()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UInt32 FileOffset = (UInt32)numericUpDown1.Value;
            FileOffset = FileOffset - (UInt32)GlobalData.Instance.ftable_arr[(int)numericUpDown2.Value];
            FileOffset = FileOffset - 0x2008;

            UInt32 TableOffset = (UInt32)GlobalData.Instance.ftable_arr[(int)numericUpDown2.Value] + 0x10;
            while(ByteTools.Read4Bytes(GlobalData.Instance.ROM, TableOffset) != FileOffset)
            {
                TableOffset += 0x08;
            }
            TableOffset = TableOffset - (UInt32)GlobalData.Instance.ftable_arr[(int)numericUpDown2.Value] - 0x10;
            TableOffset = TableOffset / 0x04 / 0x02;
            MessageBox.Show(TableOffset.ToString());
        }
    }
}
