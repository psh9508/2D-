using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2D보험구분검증툴
{
    public partial class FrmShowBarcode : Form
    {
        public FrmShowBarcode(string 검증타입, string barcodeString)
        {
            InitializeComponent();

            lblTitle.Text = 검증타입;

            richTextBox1.Text = string.IsNullOrEmpty(barcodeString) ? "바코드를 검증 후 확인해주시기 바랍니다." : barcodeString;
        }


        private void btn닫기_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    btn닫기.PerformClick();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
