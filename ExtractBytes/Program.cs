using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractBytes
{
    class Program
    {
        static void Main(string[] args) {
            if (args.Length < 3) {
                Console.WriteLine("usage: ExtractBytes.exe src_filename from_offset to_offset");
                return;
            }
            var start_offset = Convert.ToUInt32(args[1], 16);
            var end_offset = Convert.ToUInt32(args[2], 16);
            using (var fs = File.OpenRead(args[0])) {
                fs.Position = start_offset;
                using (var fsout = File.OpenWrite(args[0] + ".bin")) {
                    var buffer = new byte[end_offset - start_offset];
                    fs.Read(buffer, 0, buffer.Length);
                    fsout.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
