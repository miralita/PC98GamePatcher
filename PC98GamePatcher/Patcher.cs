using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DiscUtils.Fat;
using DiscUtils.Hdi;
using DiscUtils.Partitions;
using PatchBuilder;
using System.Windows.Forms;
using deltaq.BsDiff;

namespace PC98GamePatcher
{
    class Patcher {
        private string originalFDIPath;
        private string originalHDI;
        private string outputHDI;
        private PatchContainer patchData;
        private string sysDisk;
        private Form1 form;

        public Patcher(string originalFdiPath, string originalHdi, string outputHdi, PatchContainer patchData, string sysDisk, Form1 form) {
            originalFDIPath = originalFdiPath;
            originalHDI = originalHdi;
            outputHDI = outputHdi;
            this.patchData = patchData;
            this.sysDisk = sysDisk;
            this.form = form;
        }

        public void Patch() {
            var length = DetectLength();
            var disk = StreamFormatter.CreateDisk(outputHDI, sysDisk, length, patchData.SysFiles);
            //var disk = StreamFormatter.CreateDisk(outputHDI, sysDisk, length, patchData.SysFiles);
            //var disk = new Disk(outputHDI);
            var dstStream = disk.Partitions[0].Open();
            var dstFat = new PC98FatFileSystem(dstStream);
            if (!string.IsNullOrEmpty(originalHDI)) {
                using (var srcDisk = Disk.OpenDisk(originalHDI, FileAccess.Read)) {
                    using (var srcStream = srcDisk.Partitions[0].Open()) {
                        using (var srcFat = new PC98FatFileSystem(srcStream)) {
                            //try {
                                CopyFiles(srcFat, dstFat);
                            /*} catch (Exception ex) {
                                dstFat.Dispose();
                                dstStream.Dispose();
                                disk.Dispose();
                                disk = new Disk(outputHDI);
                                dstStream = disk.Partitions[0].Open();
                                dstFat = new PC98FatFileSystem(dstStream);
                                CopyFiles(srcFat, dstFat);
                            }*/
                        }
                    }
                }
            } else {
                var fdiFiles = Directory.GetFiles(originalFDIPath);
                foreach (var fdi in fdiFiles) {
                    if (!fdi.ToLower().EndsWith(".fdi")) continue;
                    using (var source = DiscUtils.Fdi.Disk.OpenDisk(fdi, FileAccess.Read)) {
                        using (var fs = new PC98FatFileSystem(source.Content)) {
                            CopyFiles(fs, dstFat);
                        }
                    }
                }
            }
        }

        private void CopyFiles(PC98FatFileSystem srcFat, FatFileSystem dstFat) {
            var fileList = new Dictionary<string, string>();
            BuildChecksums(fileList, srcFat, @"\");
            foreach (var file in patchData.PatchData) {
                if (file.Processed) continue;
                if (file.Action == PatchAction.Original) {
                    var srcName = FindFile(file, fileList);
                    if (! string.IsNullOrEmpty(srcName)) {
                        //throw new FileNotFoundException($"Can't find file {file.Name} in original game image or checksums differ");
                        CopyFile(srcName, file.Name, srcFat, dstFat);
                        file.Processed = true;
                    }
                } else if (file.Action == PatchAction.Copy) {
                    CreateFile(file, dstFat);
                    file.Processed = true;
                } else if (file.Action == PatchAction.Patch) {
                    var srcName = FindFile(file, fileList);
                    if (!string.IsNullOrEmpty(srcName)) {
                        ApplyPatch(srcName, file, srcFat, dstFat);
                        file.Processed = true;
                    }
                } else if (file.Action == PatchAction.Ask) {
                    var fileName = form.AskForFile(file.Name);
                    if (string.IsNullOrEmpty(fileName)) {
                        throw new ArgumentException($"Can't continue patch without {file.Name}");
                    }
                    CopyFileFromFs(file.Name, fileName, dstFat);
                }
            }
        }

        private void CopyFileFromFs(string dstFile, string srcFile, FatFileSystem dstFat) {
            MakeDirectory(dstFile, dstFat);
            using (var dst = dstFat.OpenFile(dstFile, FileMode.Create)) {
                using (var src = File.OpenRead(srcFile)) {
                    src.CopyTo(dst);
                }
            }
        }

        private void ApplyPatch(string srcName, PatchedFile file, PC98FatFileSystem srcFat, FatFileSystem dstFat) {
            if (srcName.EndsWith("CRUISER.COM")) {
                var dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                var name = Path.Combine(dir, srcName.TrimStart('\\'));
                using (var src = srcFat.OpenFile(srcName, FileMode.Open)) {
                    using (var dst = File.Create(name)) {
                        src.CopyTo(dst);
                    }
                }
                using (var dst = File.Create(name + ".patch")) {
                    dst.Write(file.Patch, 0, file.Patch.Length);
                }
            }
            MakeDirectory(file.Name, dstFat);
            using (var ms = new MemoryStream(file.Patch)) {
                ms.Position = 0;
                using (var src = srcFat.OpenFile(srcName, FileMode.Open)) {
                    using (var dst = dstFat.OpenFile(file.Name, FileMode.Create)) {
                        BsPatch.Apply(src, file.Patch, dst);
                    }
                }
            }
        }

        private void CreateFile(PatchedFile file, FatFileSystem dstFat) {
            MakeDirectory(file.Name, dstFat);
            using (var dst = dstFat.OpenFile(file.Name, FileMode.Create)) {
                using (var ms = new MemoryStream(file.FileData)) {
                    ms.Position = 0;
                    ms.CopyTo(dst);
                }
            }
        }

        private void MakeDirectory(string fileName, FatFileSystem dstFat) {
            var dirname = Path.GetDirectoryName(fileName);
            if (dirname != @"\" && dirname != "" && !dstFat.DirectoryExists(dirname)) {
                dstFat.CreateDirectory(dirname);
            }
        }

        private void CopyFile(string srcName, string dstName, PC98FatFileSystem srcFat, FatFileSystem dstFat) {
            using (var src = srcFat.OpenFile(srcName, FileMode.Open)) {
                MakeDirectory(dstName, dstFat);
                if (dstFat.FileExists(dstName)) {
                    Debug.WriteLine("exists");
                    //dstFat.DeleteFile(dstName);
                }
                var tmpfile = "~copy.tmp";
                if (dstFat.FileExists(tmpfile)) {
                    dstFat.DeleteFile(tmpfile);
                }
                using (var dst = dstFat.OpenFile(tmpfile, FileMode.Create)) {
                    src.CopyTo(dst);
                }
                dstFat.MoveFile(tmpfile, dstName, true);
                var fi = dstFat.GetFileInfo(dstName);
                fi.LastWriteTime = DateTime.Now;
                fi.LastAccessTime = DateTime.Now;
                /*try {
                    dstFat.MoveFile(tmpfile, dstName, true);
                    var fi = dstFat.GetFileInfo(dstName);
                    fi.LastWriteTime = DateTime.Now;
                } catch (Exception e) {
                    Debug.WriteLine(dstName);
                }*/
            }
        }

        private string FindFile(PatchedFile file, Dictionary<string, string> fileList) {
            foreach (var f in fileList.Keys) {
                if (file.OriginalMd5Sum == fileList[f]) return f;
            }
            return "";
        }

        private void BuildChecksums(Dictionary<string, string> fileList, PC98FatFileSystem srcFat, string dir) {
            var files = srcFat.GetFiles(dir);
            foreach (var file in files) {
                using (var s = srcFat.OpenFile(file, FileMode.Open)) {
                    fileList.Add(file, Md5sum(s));
                }
            }
            var dirs = srcFat.GetDirectories(dir);
            foreach (var dirname in dirs) {
                BuildChecksums(fileList, srcFat, dirname);
            }
        }

        private string Md5sum(Stream file) {
            using (var md5 = MD5.Create()) {
                return BitConverter.ToString(md5.ComputeHash(file)).Replace("-", "");
            }
        }

        private int DetectLength() {
            //return 40;
            var length = patchData.TotalSize + 5 * 0x100000;
            /*if (!string.IsNullOrEmpty(originalFDIPath)) {
                var files = Directory.GetFiles(originalFDIPath);
                foreach (var file in files) {
                    if (! file.ToLower().EndsWith(".fdi")) continue;
                    length += (int) new FileInfo(file).Length;
                }
                length += 0x100000; // ensure that all system data fit - add 1Mb
            } else {
                length = (int) new FileInfo(originalHDI).Length;
            }
            if (length == 0) {
                throw new ArgumentException("Empty source files");
            }*/
            
            var mb = 0x100000;
            if (length < 5 * mb) {
                return 5;
            } else if (length < 10 * mb) {
                return 10;
            } else if (length < 15 * mb) {
                return 15;
            } else if (length < 20 * mb) {
                return 20;
            } else if (length < 30 * mb) {
                return 30;
            } else if (length < 40 * mb) {
                return 40;
            } else {
                throw new ApplicationException("Data too long for known HDI types");
            }
        }
    }
}
