namespace PatchBuilder
{
    partial class AskSysFileAction
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
            this.label1 = new System.Windows.Forms.Label();
            this.bUseOriginal = new System.Windows.Forms.Button();
            this.bUseTranslated = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "The file above has different checksums in original and translated versions.\r\nWhic" +
    "h version use for a patch?";
            // 
            // bUseOriginal
            // 
            this.bUseOriginal.Location = new System.Drawing.Point(49, 76);
            this.bUseOriginal.Name = "bUseOriginal";
            this.bUseOriginal.Size = new System.Drawing.Size(75, 23);
            this.bUseOriginal.TabIndex = 1;
            this.bUseOriginal.Text = "Original";
            this.bUseOriginal.UseVisualStyleBackColor = true;
            this.bUseOriginal.Click += new System.EventHandler(this.bUseOriginal_Click);
            // 
            // bUseTranslated
            // 
            this.bUseTranslated.Location = new System.Drawing.Point(130, 76);
            this.bUseTranslated.Name = "bUseTranslated";
            this.bUseTranslated.Size = new System.Drawing.Size(75, 23);
            this.bUseTranslated.TabIndex = 2;
            this.bUseTranslated.Text = "Translated";
            this.bUseTranslated.UseVisualStyleBackColor = true;
            this.bUseTranslated.Click += new System.EventHandler(this.bUseTranslated_Click);
            // 
            // AskSysFileAction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 101);
            this.Controls.Add(this.bUseTranslated);
            this.Controls.Add(this.bUseOriginal);
            this.Controls.Add(this.label1);
            this.Name = "AskSysFileAction";
            this.Text = "AskSysFileAction";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bUseOriginal;
        private System.Windows.Forms.Button bUseTranslated;
    }
}