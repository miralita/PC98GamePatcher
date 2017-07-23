using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DiscUtils.Fat;
using DiscUtils.Fdi;
using PatchBuilder;

namespace PC98GamePatcher
{
    public partial class Form1 : Form {
        int maxLength = 56;
        private bool _sourceIsHdi;
        private List<string> _sourceFiles = new List<string>();
        private string _destinationName;
        private string _patchFile;
        private string _sysImage;
        private PatchContainer _patch;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            lSourcePath.Visible = false;
            lSourceSelected.Text = "NOT SELECTED";
            lSysDiskPath.Visible = false;
        }

        private void bSelectSource_Click(object sender, EventArgs e) {
            if (Control.ModifierKeys == Keys.Shift) {
                var dialog = new OpenFileDialog {
                    Title = "Choose source image",
                    Filter = "Image files (*.hdi, *.fdi, *.d88)|*.hdi;*.fdi;*.d88|All files (*.*)|*.*",
                    FilterIndex = 0,
                };
                if (dialog.ShowDialog() == DialogResult.OK) {
                    if (IsHdd(dialog.FileName)) {
                        CheckSourceFile(dialog.FileName);
                    }
                }
            } else {
                var dialog = new FolderBrowserDialog();
                dialog.Description = "Choose a folder containing source images";
                dialog.SelectedPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                if (dialog.ShowDialog() == DialogResult.OK) {
                    CheckSourceFiles(dialog.SelectedPath);
                }
            }
            if (_sourceFiles.Count > 0 && !string.IsNullOrEmpty(_sysImage)) {
                bApplyPatch.Enabled = true;
            }
        }

        private void bSelectPatch_Click(object sender, EventArgs e) {
            RestoreInitialState();
            var dialog = new OpenFileDialog {
                Title = "Choose patch file",
                Filter = "Patch files (*.p98)|*.p98|All files (*.*)|*.*",
                FilterIndex = 0,
                DefaultExt = "p98"
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                try {
                    if (LoadPatch(dialog.FileName)) {
                        _patchFile = dialog.FileName;
                        CheckSourceFiles(Path.GetDirectoryName(_patchFile));
                    } else {
                        MessageBox.Show("Can't load patch file",
                            "Can't load and parse the patch. Please select another file");
                    }
                } catch (Exception ex) {
                    MessageBox.Show($"Error occurred during loading patch: {ex.Message}. Please select another file", "Can't load patch file");
                }
            }
        }

        private void RestoreInitialState() {
            lSourceSelected.Text = "NOT SELECTED";
            lSourcePath.Text = "";
            lSysDiskSelected.Text = "NOT SELECTED";
            lSysDiskPath.Text = "";
            imgLogo.Image = null;
            tbDescription.Text = "Select patch";
            bSelectSource.Enabled = false;
            bSelectSysDisk.Enabled = false;
            bApplyPatch.Enabled = false;
            _sysImage = "";
            _patch = null;
            _patchFile = "";
            _sourceFiles = new List<string>();
        }

        private void CheckSourceFile(string file) {
            CheckSourceFiles(Path.GetDirectoryName(file), new string[]{file});
        }

        private void CheckSourceFiles(string dirname) {
            var files = Directory.GetFiles(dirname);
            CheckSourceFiles(dirname, files);
        }

        private void CheckSourceFiles(string dirname, string[] files) {
            var source_files = new List<string>();
            _patch.ClearState();
            bSelectSysDisk.Enabled = true;
            bSelectSource.Enabled = true;
            //lSourceSelected.Text = "Searching for game files...";
            progressBar1.Visible = true;
            progressBar1.Maximum = _patch.TotalSourceFiles();
            progressBar1.Value = 0;
            this.Update();
            var allSourceFiles = false;
            foreach (var file in files) {
                if (IsFloppy(file)) {
                    if (string.IsNullOrEmpty(_sysImage) && Patcher.CheckSysDisk(file)) {
                        _sysImage = file;
                        lSysDiskPath.Text = file;
                        lSysDiskPath.Visible = true;
                        this.toolTip1.SetToolTip(this.lSysDiskPath, file);
                        if (file.Length > maxLength) {
                            var letter = Path.GetPathRoot(file);
                            lSysDiskPath.Text = Path.Combine(letter, "...", Path.GetFileName(file));
                        }
                        lSysDiskSelected.Text = "OK";
                    } else if (!allSourceFiles) {
                        if (Patcher.CheckGameSource(_patch, file, () => {
                            progressBar1.Value++;
                            Update();
                        })) {
                            source_files.Add(file);
                        }
                    }
                } else if (!allSourceFiles) {
                    if (Patcher.CheckGameSource(_patch, file, () => {
                        progressBar1.Value++;
                        Update();
                    })) {
                        source_files.Add(file);
                    }
                }
                if (_patch.FoundFiles == _patch.TotalSourceFiles()) {
                    allSourceFiles = true;
                }
            }
            if (string.IsNullOrEmpty(_sysImage)) {
                bSelectSysDisk.Enabled = true;
            }
            if (_patch.FoundFiles == _patch.TotalSourceFiles()) {
                lSourceSelected.Visible = true;
                var fname = dirname;
                this.toolTip1.SetToolTip(this.lSourcePath, fname);
                if (fname.Length > maxLength) {
                    var letter = Path.GetPathRoot(fname);
                    fname = Path.Combine(letter, "...", Path.GetFileName(fname));
                }
                lSourcePath.Text = fname;
                lSourcePath.Visible = true;
                lSourceSelected.Text = "OK";
                _sourceFiles = source_files;
                if (!string.IsNullOrEmpty(_sysImage)) {
                    bApplyPatch.Enabled = true;
                } else {
                    bSelectSysDisk.Enabled = true;
                }
            } else {
                bSelectSource.Enabled = true;
                var msg = string.Join("\r\n", _patch.ShowNotFoundFiles());
                MessageBox.Show(msg, "Can't find source files");
            }
            progressBar1.Hide();
        }

        private bool IsFloppy(string name) {
            name = name.ToLower();
            return name.EndsWith(".fdi") || name.EndsWith(".d88");
        }

        private bool IsHdd(string name) {
            return name.ToLower().EndsWith(".hdi");
        }

        private bool LoadPatch(string file) {
            _patch = PatchContainer.Load(file);
            if (_patch != null) {
                if (!string.IsNullOrEmpty(_patch.Description)) {
                    tbDescription.Text = _patch.Description;
                }
                if (_patch.LogoImage != null && _patch.LogoImage.Length > 0) {
                    var ms = new MemoryStream(_patch.LogoImage);
                    var img = new Bitmap(ms);
                    imgLogo.Image = img;
                }
                return true;
            }
            return false;
        }

        private void bApplyPatch_Click(object sender, EventArgs e) {
            bApplyPatch.Enabled = false;
            bSelectPatch.Enabled = false;
            bSelectSource.Enabled = false;
            bSelectSysDisk.Enabled = false;
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
            progressBar2.Show();
            progressBar2.Value = 0;
            progressBar2.Maximum = _patch.TotalSourceFiles();
            try {
                var patcher = new Patcher(_sourceFiles,
                    _destinationName, _patch, _sysImage, this);
                patcher.Patch(() => {
                    progressBar2.Value++;
                    Update();
                });
                progressBar2.Value = progressBar2.Maximum;
                Update();
                lSourceSelected.Hide();
                lSourcePath.Hide();
                lSysDiskPath.Hide();
                lSysDiskSelected.Hide();
                label1.Hide();
                label4.Hide();
                bApplyPatch.Hide();
                bSelectSysDisk.Hide();
                bSelectPatch.Hide();
                bSelectSource.Hide();
                Update();
                bDone.Show();
                progressBar2.Hide();
            } catch (Exception ex) {
                bApplyPatch.Enabled = true;
                bSelectPatch.Enabled = true;
                bSelectSource.Enabled = true;
                bSelectSysDisk.Enabled = true;
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
                Title = "Choose image containing DOS installation files",
                Filter = "Floppy image files (*.fdi, *.d88)|*.fdi;*.d88|All files (*.*)|*.*",
                FilterIndex = 0,
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                var file = dialog.FileName;
                if (Patcher.CheckSysDisk(file)) {
                    _sysImage = file;
                    lSysDiskPath.Text = file;
                    lSysDiskPath.Visible = true;
                    this.toolTip1.SetToolTip(this.lSysDiskPath, file);
                    if (file.Length > maxLength) {
                        var letter = Path.GetPathRoot(file);
                        lSysDiskPath.Text = Path.Combine(letter, "...", Path.GetFileName(file));
                    }
                    lSysDiskSelected.Text = "OK";
                    if (_sourceFiles.Count > 0) {
                        bApplyPatch.Enabled = true;
                    }
                }
            }
        }

        private void bDone_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void Form1_Shown(object sender, EventArgs e) {
            var path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var files = Directory.GetFiles(path, "*.p98");
            if (files.Length > 1 || files.Length == 0) return;
            if (LoadPatch(files[0])) {
                _patchFile = files[0];
                CheckSourceFiles(Path.GetDirectoryName(_patchFile));
            }
        }
    }
}
