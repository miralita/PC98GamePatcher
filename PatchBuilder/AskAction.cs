using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatchBuilder
{
    public partial class AskAction : Form {
        private string formText;
        public AskAction() {
            InitializeComponent();
        }

        public string FormText {
            get { return formText; }
            set {
                formText = value;
                this.label1.Text = value;
            }
        }

        private void bCopy_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bAsk_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void bSkip_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Ignore;
            this.Close();
        }

        private void AskAction_Load(object sender, EventArgs e) {
            this.DialogResult = DialogResult.None;
            this.Close();
        }
    }
}
