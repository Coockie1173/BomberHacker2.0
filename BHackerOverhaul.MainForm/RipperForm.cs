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
using BHackerOverhaul.Ripper;

namespace BHackerOverhaul.MainForm
{
    public partial class RipperForm : Form
    {
        public RipperForm()
        {
            InitializeComponent();
        }

        private void OpenROMBut_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "Ripping...";
            try
            {
                Handler handler = new Handler();
                FileRipper rip = new FileRipper();
                rip.RipFiles(handler.GetROMPath());
                StatusLabel.Text = "Done";
            }
            catch(Exception ex)
            {
                StatusLabel.Text = "failed";
                MessageBox.Show(ex.Message);
            }

        }
    }
}
