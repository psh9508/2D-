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
            richTextBox1.Text = barcodeString;
        }


        private void btn닫기_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
