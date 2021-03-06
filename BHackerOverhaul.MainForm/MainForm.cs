﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BHackerOverhaul.MainForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RipperForm ripper;
        InjectorForm injector;
        HeaderForm header;
        ReadTextures reader;
        Info inforform;
        ConvertDLToObj convertDLToObj;
        FindItem Finditem;
        OBjtest objtester;
        SetupDataForm SDF;

        private void RipperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ripper = new RipperForm();
            ripper.MdiParent = this;
            ripper.Show();
        }

        private void main()
        {

        }

        private void injectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            injector = new InjectorForm();
            injector.MdiParent = this;
            injector.Show();
        }

        private void headerReaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            header = new HeaderForm();
            header.MdiParent = this;
            header.Show();
        }

        private void imageViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reader = new ReadTextures();
            reader.MdiParent = this;
            reader.Show();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(inforform != null)
            {
                inforform.Close();
            }
            inforform = new Info();
            inforform.MdiParent = this;
            inforform.Show();
        }

        private void dLObjToolStripMenuItem_Click(object sender, EventArgs e)
        {
            convertDLToObj = new ConvertDLToObj();
            convertDLToObj.MdiParent = this;
            convertDLToObj.Show();
        }

        private void findItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Finditem = new FindItem();
            Finditem.MdiParent = this;
            Finditem.Show();
        }

        private void objTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objtester = new OBjtest();
            objtester.MdiParent = this;
            objtester.Show();
        }

        private void showSetupInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SDF = new SetupDataForm();
            SDF.MdiParent = this;
            SDF.Show();
        }
    }
}
