using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC98GamePatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            //StreamFormatter.CreateDisk();
            /*using (var infile = File.OpenRead(@"D:\Translations\Tools\Patcher\tmp\HIMEM.SY_")) {
                using (var outfile = File.OpenWrite(@"D:\Translations\Tools\Patcher\tmp\HIMEM.SYS")) {
                    StreamFormatter.UnpackMSLZ(infile, outfile);
                }
            }*/

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
