using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PatchBuilder;

namespace PC98GamePatcher
{
    public partial class Form1 : Form {
        private bool _sourceIsHdi;
        private string _sourceName;
        private string _destinationName;
        private string _patchFile;
        private PatchContainer _patch;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            tabWizard.Appearance = TabAppearance.FlatButtons;
            tabWizard.ItemSize = new Size(0, 1);
            tabWizard.SelectedIndex = 0;
        }

        private void rbHDI_CheckedChanged(object sender, EventArgs e) {
            bSelectSource.Enabled = true;
            _sourceIsHdi = true;
        }

        private void rbFDI_CheckedChanged(object sender, EventArgs e) {
            bSelectSource.Enabled = true;
            _sourceIsHdi = false;
        }

        private void bSelectSource_Click(object sender, EventArgs e) {
            if (_sourceIsHdi) {
                var dialog = new OpenFileDialog {
                    Title = "Choose source HDI",
                    Filter = "HDI files (*.hdi)|*.hdi|All files (*.*)|*.*",
                    FilterIndex = 0,
                    DefaultExt = "hdi"
                };
                if (dialog.ShowDialog() == DialogResult.OK) {
                    _sourceName = dialog.FileName;
                    bSelectPatch.Enabled = true;
                }
            } else {
                var dialog = new FolderBrowserDialog();
                dialog.Description = "Choose a folder containing FDI images";
                if (dialog.ShowDialog() == DialogResult.OK) {
                    _sourceName = dialog.SelectedPath;
                    bSelectPatch.Enabled = true;
                }
            }
        }

        private void bSelectPatch_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Title = "Choose patch file",
                Filter = "Patch files (*.bin)|*.bin|All files (*.*)|*.*",
                FilterIndex = 0,
                DefaultExt = "bin"
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                try {
                    if (LoadPatch(_patchFile)) {
                        _patchFile = dialog.FileName;
                        bApplyPatch.Enabled = true;
                    } else {
                        MessageBox.Show("Can't load patch file",
                            "Can't load and parse the patch. Please select another file");
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Can't load patch file",
                        $"Error occurred during loading patch: {ex.Message}. Please select another file");
                }
            }
        }

        private bool LoadPatch(string file) {
            var serializer = new BinaryFormatter();
            using (var fs = File.OpenRead(file)) {
                _patch = serializer.Deserialize(fs) as PatchContainer;
                if (_patch != null) return true;
            }
            return false;
        }

        private void bApplyPatch_Click(object sender, EventArgs e) {
            var dialog = new SaveFileDialog {
                Title = "Choose HDI to save patched image",
                Filter = "HDI files (*.hdi)|*.hdi|All files (*.*)|*.*",
                FilterIndex = 0,
                DefaultExt = "hdi"
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                _destinationName = dialog.FileName;
                ApplyPatch();
            }
        }

        private void ApplyPatch() {
            
        }
    }
}
