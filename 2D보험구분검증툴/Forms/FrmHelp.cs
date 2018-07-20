using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UB2DBarcodeVerifier
{
    public partial class FrmHelp : Form
    {
        public Form _parentForm {get; set;}
        public FrmHelp()
        {
            InitializeComponent();
        }

        private void FrmHelp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void btn닫기_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ShowForm()
        {
            this.Top = (_parentForm.Top + (_parentForm.Height / 2)) - this.Height / 2;
            this.Left = (_parentForm.Left + (_parentForm.Width / 2)) - this.Width / 2;

            this.Height = 415;

            this.Show();
        }

        private void btn닫기_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
