using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BHackerOverhaul.SetupHandler;

namespace BHackerOverhaul.MainForm
{
    public partial class SetupDataForm : Form
    {
        public SetupDataForm()
        {
            InitializeComponent();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        private void SetupDataForm_Load(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            
            if(diag.ShowDialog() == DialogResult.OK)
            {
                Section[] TreeData = GetSetupNodeTree.GetSections(diag.FileName);

                foreach(Section s in TreeData)
                {
                    int Length = 0;
                    List<TreeNode> Children = new List<TreeNode>();
                    foreach(SubSection ss in s.SubSections)
                    {
                        string TND = $"0x{Convert.ToString(ss.HeaderbyteOne, 16).PadLeft(2, '0')} 0x{Convert.ToString(ss.HeaderbyteTwo, 16).PadLeft(2, '0')}";
                        TreeNode TN = new TreeNode(TND);
                        Children.Add(TN);
                        int index = 0;
;                        foreach (ushort sht in ss.Data)
                        {
                            TND = $"0x{Convert.ToString(ss.Offsets[index],16).PadLeft(4,'0')} 0x{Convert.ToString(sht, 16).PadLeft(4,'0')}";
                            TN = new TreeNode(TND);
                            Children.Add(TN);
                            index++;
                            Length += 2;
                        }
                    }
                    treeView1.Nodes.Add(new TreeNode($"0x{Convert.ToString(s.SectionOffset,16).PadLeft(4, '0')} -- 0x{Convert.ToString(s.AmmPartsInSubsections, 16).PadLeft(2, '0')} 0x{Convert.ToString(s.AmmSubsections, 16).PadLeft(2, '0')} 0x{Convert.ToString(s.UnkByte, 16).PadLeft(2, '0')}", Children.ToArray()));
                }
            }
        }
    }
}
