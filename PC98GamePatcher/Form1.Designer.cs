namespace PC98GamePatcher
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tabWizard = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bSelectSysDisk = new System.Windows.Forms.Button();
            this.bSelectPatch = new System.Windows.Forms.Button();
            this.bApplyPatch = new System.Windows.Forms.Button();
            this.bSelectSource = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbFDI = new System.Windows.Forms.RadioButton();
            this.rbHDI = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bDone = new System.Windows.Forms.Button();
            this.lProgress = new System.Windows.Forms.Label();
            this.tabWizard.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabWizard
            // 
            this.tabWizard.Controls.Add(this.tabPage1);
            this.tabWizard.Controls.Add(this.tabPage2);
            this.tabWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabWizard.Location = new System.Drawing.Point(0, 0);
            this.tabWizard.Name = "tabWizard";
            this.tabWizard.SelectedIndex = 0;
            this.tabWizard.Size = new System.Drawing.Size(248, 167);
            this.tabWizard.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.bSelectSysDisk);
            this.tabPage1.Controls.Add(this.bSelectPatch);
            this.tabPage1.Controls.Add(this.bApplyPatch);
            this.tabPage1.Controls.Add(this.bSelectSource);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(240, 141);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // bSelectSysDisk
            // 
            this.bSelectSysDisk.Enabled = false;
            this.bSelectSysDisk.Location = new System.Drawing.Point(122, 83);
            this.bSelectSysDisk.Name = "bSelectSysDisk";
            this.bSelectSysDisk.Size = new System.Drawing.Size(98, 34);
            this.bSelectSysDisk.TabIndex = 5;
            this.bSelectSysDisk.Text = "2. Select DOS installation image";
            this.bSelectSysDisk.UseVisualStyleBackColor = true;
            this.bSelectSysDisk.Click += new System.EventHandler(this.bSelectSysDisk_Click);
            // 
            // bSelectPatch
            // 
            this.bSelectPatch.Enabled = false;
            this.bSelectPatch.Location = new System.Drawing.Point(20, 120);
            this.bSelectPatch.Name = "bSelectPatch";
            this.bSelectPatch.Size = new System.Drawing.Size(98, 23);
            this.bSelectPatch.TabIndex = 4;
            this.bSelectPatch.Text = "3. Select patch";
            this.bSelectPatch.UseVisualStyleBackColor = true;
            this.bSelectPatch.Click += new System.EventHandler(this.bSelectPatch_Click);
            // 
            // bApplyPatch
            // 
            this.bApplyPatch.Enabled = false;
            this.bApplyPatch.Location = new System.Drawing.Point(122, 120);
            this.bApplyPatch.Name = "bApplyPatch";
            this.bApplyPatch.Size = new System.Drawing.Size(98, 23);
            this.bApplyPatch.TabIndex = 3;
            this.bApplyPatch.Text = "4. Apply patch";
            this.bApplyPatch.UseVisualStyleBackColor = true;
            this.bApplyPatch.Click += new System.EventHandler(this.bApplyPatch_Click);
            // 
            // bSelectSource
            // 
            this.bSelectSource.Enabled = false;
            this.bSelectSource.Location = new System.Drawing.Point(20, 83);
            this.bSelectSource.Name = "bSelectSource";
            this.bSelectSource.Size = new System.Drawing.Size(98, 34);
            this.bSelectSource.TabIndex = 1;
            this.bSelectSource.Text = "1. Select source";
            this.bSelectSource.UseVisualStyleBackColor = true;
            this.bSelectSource.Click += new System.EventHandler(this.bSelectSource_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbFDI);
            this.groupBox1.Controls.Add(this.rbHDI);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(20, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose source type";
            // 
            // rbFDI
            // 
            this.rbFDI.AutoSize = true;
            this.rbFDI.Location = new System.Drawing.Point(7, 44);
            this.rbFDI.Name = "rbFDI";
            this.rbFDI.Size = new System.Drawing.Size(109, 17);
            this.rbFDI.TabIndex = 1;
            this.rbFDI.TabStop = true;
            this.rbFDI.Text = "Set of FDI images";
            this.rbFDI.UseVisualStyleBackColor = true;
            this.rbFDI.CheckedChanged += new System.EventHandler(this.rbFDI_CheckedChanged);
            // 
            // rbHDI
            // 
            this.rbHDI.AutoSize = true;
            this.rbHDI.Location = new System.Drawing.Point(7, 20);
            this.rbHDI.Name = "rbHDI";
            this.rbHDI.Size = new System.Drawing.Size(75, 17);
            this.rbHDI.TabIndex = 0;
            this.rbHDI.TabStop = true;
            this.rbHDI.Text = "HDI image";
            this.rbHDI.UseVisualStyleBackColor = true;
            this.rbHDI.CheckedChanged += new System.EventHandler(this.rbHDI_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bDone);
            this.tabPage2.Controls.Add(this.lProgress);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(240, 141);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bDone
            // 
            this.bDone.Location = new System.Drawing.Point(74, 58);
            this.bDone.Name = "bDone";
            this.bDone.Size = new System.Drawing.Size(75, 23);
            this.bDone.TabIndex = 1;
            this.bDone.Text = "Done";
            this.bDone.UseVisualStyleBackColor = true;
            this.bDone.Click += new System.EventHandler(this.bDone_Click);
            // 
            // lProgress
            // 
            this.lProgress.Location = new System.Drawing.Point(59, 58);
            this.lProgress.Name = "lProgress";
            this.lProgress.Size = new System.Drawing.Size(104, 13);
            this.lProgress.TabIndex = 0;
            this.lProgress.Text = "Applying the patch...";
            this.lProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 167);
            this.Controls.Add(this.tabWizard);
            this.Name = "Form1";
            this.Text = "PC98 Game Patcher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabWizard.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabWizard;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbFDI;
        private System.Windows.Forms.RadioButton rbHDI;
        private System.Windows.Forms.Button bSelectSource;
        private System.Windows.Forms.Button bApplyPatch;
        private System.Windows.Forms.Label lProgress;
        private System.Windows.Forms.Button bSelectPatch;
        private System.Windows.Forms.Button bSelectSysDisk;
        private System.Windows.Forms.Button bDone;
    }
}

