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
using BHackerOverhaul.DLHandling;

namespace BHackerOverhaul.MainForm
{
    public partial class ConvertDLToObj : Form
    {
        public ConvertDLToObj()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "bin file|*.bin";
            DialogResult res = open.ShowDialog();
            if(res == DialogResult.OK)
            {
                try
                {
                    string[] OutObj = new DLParser().GetParsedObject(File.ReadAllBytes(open.FileName));
                    SaveFileDialog save = new SaveFileDialog();
                    save.Filter = "object file|*.obj";
                    res = save.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        File.WriteAllLines(save.FileName, OutObj);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
