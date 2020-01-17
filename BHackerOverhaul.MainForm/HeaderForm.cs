using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BHackerOverhaul.FileHandler;

namespace BHackerOverhaul.MainForm
{
    public partial class HeaderForm : Form
    {
        public HeaderForm()
        {
            InitializeComponent();
        }

        private void ReadHeaderBut_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "BIN file|*.bin";
            DialogResult res = open.ShowDialog();
            if (res == DialogResult.OK)
            {
                HeaderBox.Text = new HeaderReader().ReadHeader(File.ReadAllBytes(open.FileName));
            }
        }
    }
}
