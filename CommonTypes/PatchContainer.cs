using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace PatchBuilder
{
    sealed class PreMergeToMergedDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName) {
            if (typeName.Contains("System.Collections.Generic.List")) {
                return new List<PatchedFile>().GetType();
            }
            if (typeName.Contains("PatchContainer")) {
                return new PatchContainer().GetType();
            }
            if (typeName.Contains("PatchedFile")) {
                return new PatchedFile().GetType();
            }
            Debug.WriteLine($"{assemblyName} {typeName}");
            Type typeToDeserialize = null;

            // For each assemblyName/typeName that you want to deserialize to
            // a different type, set typeToDeserialize to the desired type.
            try {
                var asm = Assembly.Load(assemblyName);
                typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                    typeName, asm));
            } catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
            }

            if (typeToDeserialize != null) return typeToDeserialize;

            String exeAssembly = Assembly.GetExecutingAssembly().FullName;

            // The following line of code returns the type.
            typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                typeName, exeAssembly));
            

            return typeToDeserialize;
        }

        /*public override Type BindToType(string assemblyName, string typeName) {
            Type ttd = null;
            try {
                string toassname = assemblyName.Split(',')[0];
                Assembly[] asmblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly ass in asmblies) {
                    if (ass.FullName.Split(',')[0] == toassname) {
                        ttd = ass.GetType(typeName);
                        break;
                    }
                }
            } catch (System.Exception e) {
                Debug.WriteLine(e.Message);
            }
            return ttd;
        }*/
    }
    [Serializable]
    class PatchContainer {
        public static decimal DefaultPatchContainerVersion = 1.03M;
        public decimal PatchContainerVersion;
        public List<PatchedFile> PatchData;
        public string[] SysFiles;
        public long TotalSize = 0;
        public string Description;
        public byte[] LogoImage;
        public string Platform;
        public bool NeedSystemDisk;

        private int totalSourceFiles;
        public int FoundFiles = 0;

        public int TotalSourceFiles() {
            if (totalSourceFiles == 0) {
                foreach (var file in PatchData) {
                    if (file.Action == PatchAction.Original || file.Action == PatchAction.Patch) {
                        totalSourceFiles++;
                    }
                }
            }
            return totalSourceFiles;
        }

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
                serializer.AssemblyFormat =
                    System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                serializer.Serialize(fs, this);
            }
        }

        public static PatchContainer Load(string filename) {
            var serializer = new BinaryFormatter();
            serializer.AssemblyFormat =
                System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            serializer.Binder = new PreMergeToMergedDeserializationBinder();
            using (var fs = File.OpenRead(filename)) {
                var data = serializer.Deserialize(fs);
                return (PatchContainer)data;
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

        public void ClearState() {
            foreach (var file in PatchData) {
                file.Found = false;
                file.Processed = false;
                FoundFiles = 0;
            }
        }

        public string[] ShowNotFoundFiles() {
            var files = new List<string>();
            foreach (var file in PatchData) {
                if ((file.Action == PatchAction.Original || file.Action == PatchAction.Patch) && !file.Found) {
                    files.Add(file.Name);
                }
            }
            return files.ToArray();
        }
    }
}
