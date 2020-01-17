using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BHackerOverhaul.FileHandler;
using System.IO;

namespace BHackerOverhaul.MainForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string configpath = AppDomain.CurrentDomain.BaseDirectory + "\\config.ini";
            string[] BaseConfig = new string[1] 
            { "UNKNOWN PATH PLACEHOLDER TEXT " +
            "00000000000000000000000000000000000000000000000000000" +
            "00000000000000000000000000000000000000000000000000000" };
            Handler h = new Handler();
            GlobalData global = GlobalData.Instance;
            if (!File.Exists(configpath))
            {
                File.WriteAllLines(configpath, BaseConfig);
            }
            else
            {
                BaseConfig = File.ReadAllLines(configpath);
            }
            try
            {
                byte[] buf = File.ReadAllBytes(BaseConfig[0]);
                SetData(BaseConfig, h, global, buf);
            }
            catch
            {
                try
                {
                    MessageBox.Show("PLEASE SELECT A BOMBERMAN ROM");
                    string RPath = h.GetROMPath();
                    BaseConfig[0] = RPath;
                    byte[] buf = File.ReadAllBytes(BaseConfig[0]);
                    SetData(BaseConfig, h, global, buf);
                    File.WriteAllLines(configpath, BaseConfig);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                    Environment.Exit(0);
                }
            }
            Application.Run(new MainForm());
        }

        private static void SetData(string[] BaseConfig, Handler h, GlobalData global, byte[] buf)
        {
            if (h.GetHeaderName(buf) == "BOMBERMAN64U")
            {
                global.ROM = buf;
                global.ROMPath = BaseConfig[0];
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
