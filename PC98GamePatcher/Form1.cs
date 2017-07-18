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
using DiscUtils.Fat;
using DiscUtils.Fdi;
using PatchBuilder;

namespace PC98GamePatcher
{
    public partial class Form1 : Form {
        private bool _sourceIsHdi;
        private string _sourceName;
        private string _destinationName;
        private string _patchFile;
        private string _sysImage;
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
                    bSelectSysDisk.Enabled = true;
                }
            } else {
                var dialog = new FolderBrowserDialog();
                dialog.Description = "Choose a folder containing FDI images";
                dialog.SelectedPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                if (dialog.ShowDialog() == DialogResult.OK) {
                    if (CheckFdiSource(dialog.SelectedPath)) {
                        _sourceName = dialog.SelectedPath;
                        bSelectSysDisk.Enabled = true;
                    } else {
                        MessageBox.Show("Selected directory doesn't contains FDI images");
                    }
                }
            }
        }

        private bool CheckFdiSource(string dirname) {
            var files = Directory.GetFiles(dirname);
            var found = false;
            var sys_found = false;
            foreach (var file in files) {
                if (!file.ToLower().Contains(".fdi")) continue;
                using (var disk = Disk.OpenDisk(file, FileAccess.Read)) {
                    using (var fs = new PC98FatFileSystem(disk.Content)) {
                        if (fs.FileExists(@"\HDFORMAT.EXE") && fs.FileExists(@"\MSDOS.SYS")) {
                            _sysImage = file;
                            sys_found = true;
                            bSelectPatch.Enabled = true;
                        } else {
                            found = true;
                        }
                    }
                }
                if (found && sys_found) break;
            }
            return found;
        }

        private void bSelectPatch_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Title = "Choose patch file",
                Filter = "Patch files (*.pc98)|*.pc98|All files (*.*)|*.*",
                FilterIndex = 0,
                DefaultExt = "pc98"
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                try {
                    if (LoadPatch(dialog.FileName)) {
                        _patchFile = dialog.FileName;
                        bApplyPatch.Enabled = true;
                    } else {
                        MessageBox.Show("Can't load patch file",
                            "Can't load and parse the patch. Please select another file");
                    }
                } catch (Exception ex) {
                    MessageBox.Show($"Error occurred during loading patch: {ex.Message}. Please select another file", "Can't load patch file");
                }
            }
        }

        private bool LoadPatch(string file) {
            _patch = PatchContainer.Load(file);
            if (_patch != null) return true;
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
                tabWizard.SelectedIndex = 1;
                ApplyPatch();
            }
        }

        private void ApplyPatch() {
            try {
                var patcher = new Patcher(_sourceIsHdi ? "" : _sourceName, _sourceIsHdi ? _sourceName : "",
                    _destinationName, _patch, _sysImage, this);
                patcher.Patch();
                lProgress.Text = "Done";
                lProgress.Hide();
                bDone.Show();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Patch failed");
            }
        }

        public string AskForFile(string fileName) {
            var name = Path.GetFileName(fileName);
            var dialog = new OpenFileDialog {
                Title = $"Choose source for {fileName}",
                Filter = $"{name}|{name}|All files (*.*)|*.*",
                FilterIndex = 0,
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                return dialog.FileName;
            } else {
                return "";
            }
        }

        private void bSelectSysDisk_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Title = "Choose FDI image containing DOS installation files",
                Filter = "FDI files (*.fdi)|*.fdi|All files (*.*)|*.*",
                FilterIndex = 0,
                DefaultExt = "fdi"
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                _sysImage = dialog.FileName;
                bSelectPatch.Enabled = true;
            }
        }

        private void bDone_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
