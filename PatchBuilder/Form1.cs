using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using DiscUtils.Fat;
using DiscUtils.Hdi;
using DiscUtils.Partitions;
using System.Security.Cryptography;

namespace PatchBuilder {
    public partial class Form1 : Form {
        private string originalHdi;
        private PatchBuilder _patchBuilder;
        private Disk originalDisk;
        private Disk translatedDisk;
        private string translatedHdi;

        

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            _patchBuilder = new PatchBuilder(this);
            tabWizard.Appearance = TabAppearance.FlatButtons;
            tabWizard.ItemSize = new Size(0, 1);
            tabWizard.SelectedIndex = 0;
        }

        private void Form1_Shown(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Title = "Choose original HDI",
                Filter = "HDI files (*.hdi)|*.hdi|All files (*.*)|*.*",
                FilterIndex = 0,
                DefaultExt = "hdi"
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                originalHdi = dialog.FileName;
                LoadHDI(originalHdi, true);
            } else {
                Application.Exit();
            }

        }

        private void LoadHDI(string filename, bool isOriginal) {
            var disk = new Disk(filename);
            var pt = PartitionTable.GetPartitionTables(disk);
            var p = pt[0].Partitions[0];
            var fs = new FatFileSystem(p.Open());
            ListView loader;
            var checkedFiles = new Dictionary<string, bool>();
            if (isOriginal) {
                originalDisk = disk;
                _patchBuilder.originalFs = fs;
                loader = lvItems;
            } else {
                translatedDisk = disk;
                _patchBuilder.translatedFs = fs;
                loader = lvTranslatedItems;
                foreach (ListViewItem item in lvItems.CheckedItems) {
                    checkedFiles.Add(item.Name, true);
                }
            }
            
            var files = fs.GetFiles(@"\");
            var dirs = fs.GetDirectories(@"\");

            foreach (var dir in dirs) {
                var item = new ListViewItem {
                    Name = dir,
                    Text = dir.Replace("\\", ""),
                    ImageIndex = 0
                };
                if (checkedFiles.ContainsKey(dir)) item.Checked = true;
                loader.Items.Add(item);
            }
            foreach (var file in files) {
                var item = new ListViewItem {
                    Name = file,
                    Text = file.TrimStart('\\'),
                    ImageIndex = 1
                };
                if (checkedFiles.ContainsKey(file)) item.Checked = true;
                if (_patchBuilder.IsSysFile(item.Text)) {
                    item.Tag = "sys";
                    item.BackColor = Color.LightGray;
                    item.ForeColor = Color.Gray;
                } else if (item.Text == "AUTOEXEC.BAT" || item.Text == "CONFIG.SYS") {
                    item.Tag = "always";
                    item.Checked = true;
                    item.BackColor = Color.LightGray;
                    item.ForeColor = Color.Gray;
                } else {
                    item.Tag = "ord";
                }
                loader.Items.Add(item);
                
            }
            loader.Refresh();
        }

        

        private void bOK_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in lvItems.CheckedItems) {
                if (item.ImageIndex == 0) {
                    _patchBuilder.BuildFileList(true, item.Name);
                } else {
                    _patchBuilder.AddFile2List(true, item.Name);
                }
            }
            bOK.Enabled = false;
            lvItems.Enabled = false;
            var dialog = new OpenFileDialog {
                Title = "Choose translated HDI",
                Filter = "HDI files (*.hdi)|*.hdi|All files (*.*)|*.*",
                FilterIndex = 0,
                DefaultExt = "hdi"
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                translatedHdi = dialog.FileName;
                LoadHDI(translatedHdi, false);
            }
            if (cbShowTranslated.Checked) {
                tabWizard.SelectedIndex = 1;
            } else {
                bOK2_Click(sender, e);
            }
        }

        private void SavePatch(PatchContainer patchResult) {
            var dialog = new SaveFileDialog {
                Title = "Choose where to save patch",
                Filter = "PC98 files (*.p98)|*.p98|All files (*.*)|*.*",
                FilterIndex = 0,
                DefaultExt = "p98"
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                patchResult.Save(dialog.FileName);
            }
        }
        
        private PatchContainer MakePatch() {
            foreach (ListViewItem item in lvTranslatedItems.CheckedItems) {
                if (item.ImageIndex == 0) {
                    _patchBuilder.BuildFileList(false, item.Name);
                } else {
                    _patchBuilder.AddFile2List(false, item.Name);
                }
            }
            return _patchBuilder.Build();
        }

        

        private void bOK2_Click(object sender, EventArgs e) {
            tabWizard.SelectedIndex = 2;
        }

        private bool lvItemsCheckedGuard = false;
        private void lvItems_ItemChecked(object sender, ItemCheckedEventArgs e) {
            if (lvItemsCheckedGuard) return;
            lvItemsCheckedGuard = true;
            var tag = e.Item.Tag as string;
            if (tag == "sys") {
                e.Item.Checked = false;
            } else if (tag == "always") {
                e.Item.Checked = true;
            }
            lvItemsCheckedGuard = false;
        }

        public bool Ask(string header, string message) {
            var result = MessageBox.Show(header, message, MessageBoxButtons.YesNo);
            return result == DialogResult.Yes;
        }

        internal AskActionResult AskAction(string text) {
            var askForm = new AskAction();
            askForm.Text = text;
            var result = askForm.ShowDialog();
            if (result == DialogResult.Yes) {
                return AskActionResult.Copy;
            } else if (result == DialogResult.Retry) {
                return AskActionResult.Ask;
            } else {
                return AskActionResult.Skip;
            }
        }

        public bool AskConfigFileSource(string file) {
            var askForm = new AskSysFileAction();
            askForm.Text = file;
            var result = askForm.ShowDialog();
            if (result == DialogResult.Yes) {
                return true;
            } else {
                return false;
            }
        }

        private void bDescrOk_Click(object sender, EventArgs e) {
            tabWizard.SelectedIndex = 3;
            var patchResult = MakePatch();
            patchResult.Description = tbDescription.Text;
            if (!string.IsNullOrEmpty(_logoImage)) {
                patchResult.LogoImage = File.ReadAllBytes(_logoImage);
            }
            SavePatch(patchResult);
            lProgress.Text = patchResult.Stat();
            MessageBox.Show("OK");
        }

        private string _logoImage = "";
        private void bSelectImage_Click(object sender, EventArgs e) {
            var dialog = new OpenFileDialog {
                Title = "Choose image for logo",
                Filter = "Image files (*.png, *.jpg, *.gif, *.bmp)|*.png;*.jpg;*.gif;*.bmp|All files (*.*)|*.*",
                FilterIndex = 0,
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                _logoImage = dialog.FileName;
                pImage.Load(dialog.FileName);
            }
        }
    }
}
