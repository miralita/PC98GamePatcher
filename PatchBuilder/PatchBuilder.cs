using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using deltaq.BsDiff;
using DiscUtils.Fat;

namespace PatchBuilder
{
    enum AskActionResult { Ask, Skip, Copy}
    class PatchBuilder
    {
        private Form1 form;
        public Dictionary<string, string> originalList;
        private Dictionary<string, string> translatedList;
        public FatFileSystem originalFs;
        public FatFileSystem translatedFs;

        private string[] _sysFiles = new string[] {
            "BCKWHEAT.SYS",
            "COMMAND.COM",
            "CUSTOM.EXE",
            "DBLSPACE.BIN",
            "DISKCOPY.EXE",
            "EMM386.EXE",
            "EXPAND.EXE",
            "EXTDSWAP.SYS",
            "FORMAT.EXE",
            "HDFORMAT.EXE",
            "HIMEM.SYS",
            "INSTDOS.EXE",
            "IO.SYS",
            "KKCFUNC.SYS",
            "LISTFILE.TBL",
            "MSCDEX.EXE",
            "MSDOS.SYS",
            "NECAIK1.DRV",
            "NECAIK2.DRV",
            "NECCDA.SYS",
            "NECCDB.SYS",
            "NECCDC.SYS",
            "NECCDCHK.EXE",
            "NECCDD.SYS",
            "PRINT.SYS",
            "RSDRV.SYS",
            "SCOPY.EXE",
            "SETVER.EXE",
            "SYS.EXE",
            "UPSETVER.EXE",
        };

        public PatchBuilder(Form1 form) {
            this.form = form;
            originalList = new Dictionary<string, string>();
            translatedList = new Dictionary<string, string>();

        }

        public PatchContainer Build() {
            var patchResult = new PatchContainer();
            patchResult.Platform = "PC-98xx";
            patchResult.NeedSystemDisk = true;
            CheckSysFiles(patchResult);
            CheckConfigFiles(patchResult);
            CheckOriginalList(patchResult);
            CheckTranslatedList(patchResult);
            return patchResult;
        }

        private void CheckSysFiles(PatchContainer patchResult) {
            var files = originalFs.GetFiles(@"\");
            var sysFiles = new List<string>();
            foreach (var file in files) {
                if (IsSysFile(file.TrimStart('\\'))) {
                    sysFiles.Add(file);
                    var info = originalFs.GetFileInfo(file);
                    patchResult.TotalSize += info.Length;
                }
            }
            patchResult.SysFiles = sysFiles.ToArray();
        }

        private void CheckConfigFiles(PatchContainer patchResult) {
            var files = new string[] {@"\AUTOEXEC.BAT", @"\CONFIG.SYS"};
            foreach (var file in files) {
                var fileInfo = new PatchedFile();
                fileInfo.Name = file;
                fileInfo.Action = PatchAction.Copy;
                if (originalFs.FileExists(file) && translatedFs.FileExists(file)) {
                    using (var origFh = originalFs.OpenFile(file, FileMode.Open)) {
                        using (var translatedFh = translatedFs.OpenFile(file, FileMode.Open)) {
                            var origMd5 = Md5sum(origFh);
                            var translatedMd5 = Md5sum(translatedFh);
                            if (origMd5 != translatedMd5) {
                                var useOriginal = form.AskConfigFileSource(file);
                                if (useOriginal) {
                                    origFh.Position = 0;
                                    fileInfo.FileData = DumpStream(origFh);
                                    patchResult.TotalSize += origFh.Length;
                                } else {
                                    translatedFh.Position = 0;
                                    fileInfo.FileData = DumpStream(translatedFh);
                                    patchResult.TotalSize += translatedFh.Length;
                                }
                            } else {
                                origFh.Position = 0;
                                fileInfo.FileData = DumpStream(origFh);
                                patchResult.TotalSize += origFh.Length;
                            }
                        }
                    }
                } else if (originalFs.FileExists(file)) {
                    using (var origFh = originalFs.OpenFile(file, FileMode.Open)) {
                        fileInfo.FileData = DumpStream(origFh);
                        patchResult.TotalSize += origFh.Length;
                    }
                } else if (translatedFs.FileExists(file)) {
                    using (var translatedFh = translatedFs.OpenFile(file, FileMode.Open)) {
                        fileInfo.FileData = DumpStream(translatedFh);
                        patchResult.TotalSize += translatedFh.Length;
                    }
                } else {
                    continue;
                }
                patchResult.Add(fileInfo);
            }
        }

        private void CheckTranslatedList(PatchContainer patchResult) {
            foreach (var key in translatedList.Keys) {
                if (key.TrimStart('\\') == "AUTOEXEC.BAT" || key.TrimStart('\\') == "CONFIG.SYS") {
                    continue;
                }
                if (!translatedList.ContainsKey(key)) {
                    var ret = form.AskAction($"Original HDI doesn't contains file {key}");
                    var file = new PatchedFile();
                    file.Name = key;
                    if (ret == AskActionResult.Copy) {
                        file.Action = PatchAction.Copy;
                        using (var fh = translatedFs.OpenFile(key, FileMode.Open)) {
                            file.FileData = DumpStream(fh);
                            patchResult.TotalSize += fh.Length;
                        }
                        patchResult.Add(file);
                    } else if (ret == AskActionResult.Ask) {
                        file.Action = PatchAction.Ask;
                        patchResult.Add(file);
                        patchResult.TotalSize += 65 * 1024;
                    }
                }
            }
        }

        private byte[] DumpStream(Stream s) {
            using (var ms = new MemoryStream()) {
                s.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private void CheckOriginalList(PatchContainer patchResult) {
            foreach (var key in originalList.Keys) {
                if (key.TrimStart('\\') == "AUTOEXEC.BAT" || key.TrimStart('\\') == "CONFIG.SYS") {
                    continue;
                }
                if (key.ToLower().EndsWith(@"\\CONFIG.SYS")) continue;
                var file = new PatchedFile {
                    Name = key,
                    Action = PatchAction.Original,
                    OriginalMd5Sum = originalList[key]
                };
                if (!translatedList.ContainsKey(key)) {
                    if (form.Ask($"Translated HDI doesn't contain file {key}",
                        $"Delete {key} from source during patching?")) {
                        file.Action = PatchAction.Delete;
                    }
                } else if (originalList[key] != translatedList[key]) {
                    file.Action = PatchAction.Patch;
                    using (var ms = new MemoryStream()) {
                        using (var src = originalFs.OpenFile(key, FileMode.Open)) {
                            using (var patched = translatedFs.OpenFile(key, FileMode.Open)) {
                                BsDiff.Create(src, patched, ms);
                                patchResult.TotalSize += patched.Length;
                            }
                        }
                        file.Patch = ms.ToArray();
                    }
                } else {
                    var info = originalFs.GetFileInfo(key);
                    patchResult.TotalSize += info.Length;
                }
                patchResult.Add(file);
            }
        }

        internal void BuildFileList(bool isOriginal, string name) {
            if (isOriginal) {
                BuildFileList(originalList, originalFs, name);
            } else {
                BuildFileList(translatedList, translatedFs, name);
            }
        }

        public bool IsSysFile(string text) {
            return _sysFiles.Any(s => s.Equals(text, StringComparison.InvariantCultureIgnoreCase));
        }

        public void BuildFileList(Dictionary<string, string> list, FatFileSystem fs, string dirname) {
            var dirs = fs.GetDirectories(dirname);
            foreach (var dir in dirs) {
                BuildFileList(list, fs, dir);
            }
            var files = fs.GetFiles(dirname);
            foreach (var file in files) {
                using (var fh = fs.OpenFile(file, FileMode.Open)) {
                    list.Add(file, Md5sum(fh));
                }
            }
        }

        private string Md5sum(string filename) {
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(filename)) {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                }
            }
        }

        private string Md5sum(Stream file) {
            using (var md5 = MD5.Create()) {
                return BitConverter.ToString(md5.ComputeHash(file)).Replace("-", "");
            }
        }

        public void AddFile2List(bool isOriginal, string name) {
            if (isOriginal) {
                AddFile2List(originalList, originalFs, name);
            } else {
                AddFile2List(translatedList, translatedFs, name);
            }
        }

        private void AddFile2List(Dictionary<string, string> list, FatFileSystem fs, string name) {
            using (var fh = fs.OpenFile(name, FileMode.Open)) {
                list.Add(name, Md5sum(fh));
            }
        }
    }
}
