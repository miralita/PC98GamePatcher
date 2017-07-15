namespace PatchBuilder
{
    partial class AskAction
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
            this.bCopy = new System.Windows.Forms.Button();
            this.bAsk = new System.Windows.Forms.Button();
            this.bSkip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 92);
            this.label1.TabIndex = 0;
            this.label1.Text = "What should we do with the file?\r\nCopy - copy whole file to patch;\r\nAsk user for " +
    "file location - ask user to provide file during patching process;\r\nSkip - ignore" +
    " this difference.";
            // 
            // bCopy
            // 
            this.bCopy.Location = new System.Drawing.Point(12, 123);
            this.bCopy.Name = "bCopy";
            this.bCopy.Size = new System.Drawing.Size(75, 23);
            this.bCopy.TabIndex = 1;
            this.bCopy.Text = "Copy";
            this.bCopy.UseVisualStyleBackColor = true;
            this.bCopy.Click += new System.EventHandler(this.bCopy_Click);
            // 
            // bAsk
            // 
            this.bAsk.Location = new System.Drawing.Point(115, 123);
            this.bAsk.Name = "bAsk";
            this.bAsk.Size = new System.Drawing.Size(136, 23);
            this.bAsk.TabIndex = 2;
            this.bAsk.Text = "Ask user for file location";
            this.bAsk.UseVisualStyleBackColor = true;
            this.bAsk.Click += new System.EventHandler(this.bAsk_Click);
            // 
            // bSkip
            // 
            this.bSkip.Location = new System.Drawing.Point(277, 123);
            this.bSkip.Name = "bSkip";
            this.bSkip.Size = new System.Drawing.Size(75, 23);
            this.bSkip.TabIndex = 3;
            this.bSkip.Text = "Skip";
            this.bSkip.UseVisualStyleBackColor = true;
            this.bSkip.Click += new System.EventHandler(this.bSkip_Click);
            // 
            // AskAction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 158);
            this.Controls.Add(this.bSkip);
            this.Controls.Add(this.bAsk);
            this.Controls.Add(this.bCopy);
            this.Controls.Add(this.label1);
            this.Name = "AskAction";
            this.Text = "AskAction";
            this.Load += new System.EventHandler(this.AskAction_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bCopy;
        private System.Windows.Forms.Button bAsk;
        private System.Windows.Forms.Button bSkip;
    }
}