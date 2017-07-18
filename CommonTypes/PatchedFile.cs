using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchBuilder
{
    enum PatchAction {
        Delete, Copy, Ignore, Ask, Patch, Original
    }
    [Serializable]
    class PatchedFile {
        public PatchAction Action;
        public string Name;
        public byte[] FileData;
        public byte[] Patch;
        public string OriginalMd5Sum;
        public bool Processed;

        public override string ToString() {
            return Name;
        }
    }
}
