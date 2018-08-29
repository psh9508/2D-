using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2D보험구분검증툴
{
    public partial class Spinner : Form
    {
        private static Spinner _instance;
        private static BackgroundWorker _worker;
        private readonly Form _parentForm;
        private readonly TaskScheduler _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        public static Spinner GetInstance(Form parentForm, string message = "서버와 통신중입니다.")
        {
            if (_instance == null)
                _instance = new Spinner(parentForm);

            return _instance;
        }

        public static Spinner GetInstance()
        {
            return _instance;
        }

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
            //var worker = new BackgroundWorker();
            _worker = new BackgroundWorker();

            _worker.DoWork += new DoWorkEventHandler((object o, DoWorkEventArgs eargs) => {
                loadingImg.StartPosition = FormStartPosition.Manual;
                loadingImg.ShowFrom();
            });

            _worker.Disposed += (object obj, EventArgs ea) => {

                // 핸들이 없으면 강제로 만들어준다.
                IntPtr handle;
                if (!this.IsHandleCreated)
                    handle = this.Handle;

                var task = BeginInvoke(new Action(() => {
                    loadingImg.Close();
                }));
            };

            return _worker;
        }
    }
}
