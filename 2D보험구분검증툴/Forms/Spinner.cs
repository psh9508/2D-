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
    public partial class Spinner : Form
    {
        //public Form _parentForm { get; set; }
        private readonly Form _parentForm;

        public Spinner(Form parentForm, string message = "서버와 통신중입니다.")
        {
            InitializeComponent();
            _parentForm = parentForm;
            lblMessage.Text = message;
        }

        public void ShowFrom()
        {
            try
            {
                this.Top = (_parentForm.Top + (_parentForm.Height / 2)) - this.Height / 2;
                this.Left = (_parentForm.Left + (_parentForm.Width / 2)) - this.Width / 2;
            }
            finally
            {
                this.ShowDialog();
            }
        }

        public BackgroundWorker GetLoadingWorker(Spinner loadingImg)
        {
            var worker = new BackgroundWorker();
            //var loadingImg = new Spinner(Application.OpenForms["Form1"]);
            //var loadingImg = this;

            worker.DoWork += new DoWorkEventHandler((object o, DoWorkEventArgs eargs) => {
                loadingImg.StartPosition = FormStartPosition.Manual;
                //loadingImg.StartPosition = FormStartPosition.CenterParent;
                //loadingImg._parentForm = parentFrom;
                loadingImg.TopMost = true;
                loadingImg.ShowFrom();
                //loadingImg.ShowDialog(this);
            });

            worker.Disposed += (object obj, EventArgs ea) => {

                // 핸들이 없으면 강제로 만들어준다.
                IntPtr handle;
                if (!this.IsHandleCreated)
                    handle = this.Handle;

                var task = BeginInvoke(new Action(() => {
                    loadingImg.Close();
                }));

                //EndInvoke(task);
            };

            //worker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => {
            //    BeginInvoke(new Action(() => {
            //        loadingImg.Close();
            //    }));
            //};

            return worker;
        }
    }
}
