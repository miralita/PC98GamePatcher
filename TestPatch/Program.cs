using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using CodeIsle.LibIpsNet;
using deltaq.BsDiff;

namespace TestPatch
{
    class Program
    {
        static void Main(string[] args) {
            var src = @"S:\Translations\Patcher\newdisk5.hdi";
            var patched = @"S:\Translations\Patcher\newdisk-test.hdi";
            var patch = @"S:\Translations\Patcher\newdisk.patch";
            var dst = @"S:\Translations\Patcher\newdisk-test_patched.hdi";
            //MakePatch(src, patched, patch);

            src = @"H:\translations\Translations\Tools\Patcher\PC98GamePatcher\bin\Debug\CRUISER.COM";
            patch = @"H:\translations\Translations\Tools\Patcher\PC98GamePatcher\bin\Debug\CRUISER.COM.patch";
            dst = @"H:\translations\Translations\Tools\Patcher\PC98GamePatcher\bin\Debug\CRUISER_P.COM";
            CreatePatch(src, dst, patch);
            ApplyPatch(src, patch, dst + ".1");
        }

        private static void ApplyPatch(string src, string patch, string dst) {
            if (File.Exists(dst)) File.Delete(dst);
            using (var fh = File.Create(dst)) {
                BsPatch.Apply(File.ReadAllBytes(src), File.ReadAllBytes(patch), fh);
            }
        }

        private static void CreatePatch(string src, string dst, string patch) {
            if (File.Exists(patch)) File.Delete(patch);
            using (var fh = File.Create(patch)) {
                BsDiff.Create(File.ReadAllBytes(src), File.ReadAllBytes(dst), fh);
            }
        }
    }
}
