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
using DiscUtils;

namespace PC98GamePatcher {
    class Patcher {
        private string[] originalFdi;
        private string originalHDI;
        private string outputHDI;
        private PatchContainer patchData;
        private string sysDisk;
        private Form1 form;
        private Action progressCb;

        public Patcher(List<string> sources, string outputHdi, PatchContainer patchData, string sysDisk, Form1 form) {
            if (sources[0].ToLower().EndsWith(".hdi")) {
                originalHDI = sources[0];
                originalFdi = new string[] { };
            } else {
                originalHDI = "";
                originalFdi = sources.ToArray();
            }
            outputHDI = outputHdi;
            this.patchData = patchData;
            this.sysDisk = sysDisk;
            this.form = form;
        }

        public void Patch() {
            var length = DetectLength();
            var disk = StreamFormatter.CreateDisk(outputHDI, sysDisk, length, patchData.SysFiles);
            var dstStream = disk.Partitions[0].Open();
            var dstFat = new PC98FatFileSystem(dstStream);
            if (!string.IsNullOrEmpty(originalHDI)) {
                using (var srcDisk = Disk.OpenDisk(originalHDI, FileAccess.Read)) {
                    using (var srcStream = srcDisk.Partitions[0].Open()) {
                        using (var srcFat = new PC98FatFileSystem(srcStream)) {
                            CopyFiles(srcFat, dstFat);
                        }
                    }
                }
            } else {
                foreach (var fdi in originalFdi) {
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
                    if (!string.IsNullOrEmpty(srcName)) {
                        //throw new FileNotFoundException($"Can't find file {file.Name} in original game image or checksums differ");
                        CopyFile(srcName, file.Name, srcFat, dstFat);
                        file.Processed = true;
                        progressCb?.Invoke();
                    }
                } else if (file.Action == PatchAction.Copy) {
                    CreateFile(file, dstFat);
                    file.Processed = true;
                } else if (file.Action == PatchAction.Patch) {
                    var srcName = FindFile(file, fileList);
                    if (!string.IsNullOrEmpty(srcName)) {
                        ApplyPatch(srcName, file, srcFat, dstFat);
                        file.Processed = true;
                        progressCb?.Invoke();
                    }
                } else if (file.Action == PatchAction.Ask) {
                    var fileName = form.AskForFile(file.Name);
                    if (string.IsNullOrEmpty(fileName)) {
                        throw new ArgumentException($"Can't continue patch without {file.Name}");
                    }
                    CopyFileFromFs(file.Name, fileName, dstFat);
                    file.Processed = true;
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

        private static string FindFile(PatchedFile file, Dictionary<string, string> fileList) {
            foreach (var f in fileList.Keys) {
                if (file.OriginalMd5Sum == fileList[f]) return f;
            }
            return "";
        }

        private static void BuildChecksums(Dictionary<string, string> fileList, PC98FatFileSystem srcFat, string dir) {
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

        private static string Md5sum(Stream file) {
            using (var md5 = MD5.Create()) {
                return BitConverter.ToString(md5.ComputeHash(file)).Replace("-", "");
            }
        }

        private int DetectLength() {
            //return 40;
            var length = patchData.TotalSize + patchData.TotalSourceFiles() * 256 + 0x20000;

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

        public static bool CheckSysDisk(string file) {
            using (var disk = Disk.OpenDisk(file, FileAccess.Read)) {
                using (var fs = new PC98FatFileSystem(disk.Content)) {
                    if (fs.FileExists(@"\HDFORMAT.EXE") && fs.FileExists(@"\MSDOS.SYS")) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CheckGameSource(PatchContainer patch, string file, Action cb) {
            var found = false;
            using (var disk = Disk.OpenDisk(file, FileAccess.Read)) {
                SparseStream s;
                if (file.ToLower().EndsWith(".fdi")) {
                    s = disk.Content;
                } else {
                    s = disk.Partitions[0].Open();
                }
                using (var fs = new PC98FatFileSystem(s)) {
                    var filelist = new Dictionary<string, string>();
                    BuildChecksums(filelist, fs, @"\");
                    foreach (var fdata in patch.PatchData) {
                        if (fdata.Found) continue;
                        if (!string.IsNullOrEmpty(FindFile(fdata, filelist))) {
                            fdata.Found = true;
                            cb();
                            found = true;
                            patch.FoundFiles++;
                        }
                    }
                }
            }
            return found;
        }

        public void Patch(Action action) {
            progressCb = action;
            Patch();
        }
    }
}
