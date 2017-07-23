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
            this.components = new System.ComponentModel.Container();
            this.bSelectSysDisk = new System.Windows.Forms.Button();
            this.bSelectPatch = new System.Windows.Forms.Button();
            this.bApplyPatch = new System.Windows.Forms.Button();
            this.bSelectSource = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lSysDiskSelected = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lSourceSelected = new System.Windows.Forms.Label();
            this.lSourcePath = new System.Windows.Forms.Label();
            this.lSysDiskPath = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.bDone = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bSelectSysDisk
            // 
            this.bSelectSysDisk.Enabled = false;
            this.bSelectSysDisk.Location = new System.Drawing.Point(210, 95);
            this.bSelectSysDisk.Name = "bSelectSysDisk";
            this.bSelectSysDisk.Size = new System.Drawing.Size(98, 34);
            this.bSelectSysDisk.TabIndex = 5;
            this.bSelectSysDisk.Text = "System disk";
            this.bSelectSysDisk.UseVisualStyleBackColor = true;
            this.bSelectSysDisk.Click += new System.EventHandler(this.bSelectSysDisk_Click);
            // 
            // bSelectPatch
            // 
            this.bSelectPatch.Location = new System.Drawing.Point(2, 95);
            this.bSelectPatch.Name = "bSelectPatch";
            this.bSelectPatch.Size = new System.Drawing.Size(98, 34);
            this.bSelectPatch.TabIndex = 4;
            this.bSelectPatch.Text = "Patch";
            this.bSelectPatch.UseVisualStyleBackColor = true;
            this.bSelectPatch.Click += new System.EventHandler(this.bSelectPatch_Click);
            // 
            // bApplyPatch
            // 
            this.bApplyPatch.Enabled = false;
            this.bApplyPatch.Location = new System.Drawing.Point(314, 95);
            this.bApplyPatch.Name = "bApplyPatch";
            this.bApplyPatch.Size = new System.Drawing.Size(98, 34);
            this.bApplyPatch.TabIndex = 3;
            this.bApplyPatch.Text = "GO!";
            this.bApplyPatch.UseVisualStyleBackColor = true;
            this.bApplyPatch.Click += new System.EventHandler(this.bApplyPatch_Click);
            // 
            // bSelectSource
            // 
            this.bSelectSource.Enabled = false;
            this.bSelectSource.Location = new System.Drawing.Point(106, 95);
            this.bSelectSource.Name = "bSelectSource";
            this.bSelectSource.Size = new System.Drawing.Size(98, 34);
            this.bSelectSource.TabIndex = 1;
            this.bSelectSource.Text = "Source";
            this.toolTip1.SetToolTip(this.bSelectSource, "Click to select folder. Shift-click to select file");
            this.bSelectSource.UseVisualStyleBackColor = true;
            this.bSelectSource.Click += new System.EventHandler(this.bSelectSource_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(13, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Source:";
            // 
            // lSysDiskSelected
            // 
            this.lSysDiskSelected.AutoSize = true;
            this.lSysDiskSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lSysDiskSelected.Location = new System.Drawing.Point(126, 47);
            this.lSysDiskSelected.Name = "lSysDiskSelected";
            this.lSysDiskSelected.Size = new System.Drawing.Size(123, 16);
            this.lSysDiskSelected.TabIndex = 2;
            this.lSysDiskSelected.Text = "NOT SELECTED";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(13, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "MS-DOS disk:";
            // 
            // lSourceSelected
            // 
            this.lSourceSelected.AutoSize = true;
            this.lSourceSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lSourceSelected.Location = new System.Drawing.Point(126, 3);
            this.lSourceSelected.Name = "lSourceSelected";
            this.lSourceSelected.Size = new System.Drawing.Size(87, 16);
            this.lSourceSelected.TabIndex = 7;
            this.lSourceSelected.Text = "SELECTED";
            // 
            // lSourcePath
            // 
            this.lSourcePath.AutoSize = true;
            this.lSourcePath.Location = new System.Drawing.Point(31, 26);
            this.lSourcePath.Name = "lSourcePath";
            this.lSourcePath.Size = new System.Drawing.Size(87, 13);
            this.lSourcePath.TabIndex = 9;
            this.lSourcePath.Text = "p:\\ath\\to\\source";
            this.toolTip1.SetToolTip(this.lSourcePath, "Source Path");
            // 
            // lSysDiskPath
            // 
            this.lSysDiskPath.AutoSize = true;
            this.lSysDiskPath.Location = new System.Drawing.Point(31, 68);
            this.lSysDiskPath.Name = "lSysDiskPath";
            this.lSysDiskPath.Size = new System.Drawing.Size(87, 13);
            this.lSysDiskPath.TabIndex = 11;
            this.lSysDiskPath.Text = "p:\\ath\\to\\source";
            // 
            // tbDescription
            // 
            this.tbDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDescription.Enabled = false;
            this.tbDescription.Location = new System.Drawing.Point(9, 119);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.ReadOnly = true;
            this.tbDescription.Size = new System.Drawing.Size(410, 34);
            this.tbDescription.TabIndex = 12;
            this.tbDescription.Text = "Select patch";
            this.tbDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // imgLogo
            // 
            this.imgLogo.Location = new System.Drawing.Point(9, 3);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(410, 115);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgLogo.TabIndex = 13;
            this.imgLogo.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(129, 6);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(281, 12);
            this.progressBar1.TabIndex = 14;
            this.progressBar1.Value = 50;
            this.progressBar1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bDone);
            this.panel1.Controls.Add(this.progressBar2);
            this.panel1.Controls.Add(this.bSelectSource);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lSysDiskSelected);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.bApplyPatch);
            this.panel1.Controls.Add(this.lSourceSelected);
            this.panel1.Controls.Add(this.bSelectSysDisk);
            this.panel1.Controls.Add(this.lSourcePath);
            this.panel1.Controls.Add(this.bSelectPatch);
            this.panel1.Controls.Add(this.lSysDiskPath);
            this.panel1.Location = new System.Drawing.Point(7, 154);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(412, 153);
            this.panel1.TabIndex = 15;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(2, 141);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(410, 12);
            this.progressBar2.TabIndex = 16;
            this.progressBar2.Visible = false;
            // 
            // bDone
            // 
            this.bDone.Location = new System.Drawing.Point(174, 58);
            this.bDone.Name = "bDone";
            this.bDone.Size = new System.Drawing.Size(75, 23);
            this.bDone.TabIndex = 17;
            this.bDone.Text = "Done!";
            this.bDone.UseVisualStyleBackColor = true;
            this.bDone.Visible = false;
            this.bDone.Click += new System.EventHandler(this.bDone_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 313);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.imgLogo);
            this.Controls.Add(this.tbDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Japanese Computers Game Patcher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bSelectSource;
        private System.Windows.Forms.Button bApplyPatch;
        private System.Windows.Forms.Button bSelectPatch;
        private System.Windows.Forms.Button bSelectSysDisk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lSysDiskSelected;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lSourceSelected;
        private System.Windows.Forms.Label lSourcePath;
        private System.Windows.Forms.Label lSysDiskPath;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.PictureBox imgLogo;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Button bDone;
    }
}

