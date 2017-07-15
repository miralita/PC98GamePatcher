using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PatchBuilder
{
    [Serializable]
    class PatchContainer {
        public static decimal DefaultPatchContainerVersion = 1.02M;
        public decimal PatchContainerVersion;
        public List<PatchedFile> PatchData;
        public string[] SysFiles;

        public PatchContainer() {
            PatchContainerVersion = DefaultPatchContainerVersion;
            PatchData = new List<PatchedFile>();
        }

        public void Add(PatchedFile file) {
            this.PatchData.Add(file);
        }

        public void Save(string fileName) {
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
            using (var fs = File.OpenWrite(fileName)) {
                var serializer = new BinaryFormatter();
                serializer.Serialize(fs, this);
            }
        }

        public static PatchContainer Load(string filename) {
            var serializer = new BinaryFormatter();
            using (var fs = File.OpenRead(filename)) {
                return serializer.Deserialize(fs) as PatchContainer;
            }
        }

        public string Stat() {
            var totalFiles = 0;
            var patchedFiles = 0;
            var patchLength = 0.0;
            foreach (var file in PatchData) {
                totalFiles++;
                if (file.Action == PatchAction.Patch) {
                    patchedFiles++;
                    patchLength += file.Patch.Length;
                } else if (file.Action == PatchAction.Copy) {
                    patchLength += file.FileData.Length;
                }
            }
            return
                $"Total files: {totalFiles}\r\nPatchedFiles: {patchedFiles}\r\nPatch length: {(patchLength / 1024):F2} Kb";
        }
    }
}
