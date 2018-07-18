using _2D보험구분검증툴.Class;
using _2D보험구분검증툴.Interface;
using _2D보험구분검증툴.Logic;
using _2D보험구분검증툴.Logic.보험구분Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using UB2DBarcodeVerifier;
using static _2D보험구분검증툴.Test.TestEnum;

namespace _2D보험구분검증툴
{
    public partial class Form1 : Form
    {
        IMessage _Message;
        public TextBox Get파일경로 { get { return txt파일경로; } }

        I검증하기 _검증하기Logic;
        I인증하기 _인증하기Logic;
        IForm _FormLogic;

        #region TEST
        public e인증결과 _e인증결과 { get; private set; }
        public List<오류목록Model> _오류목록 { get; private set; }
        #endregion

        public Form1(I검증하기 검증하기Logic, I인증하기 인증하기, IForm 파일선택)
        {
            InitializeComponent();

            _검증하기Logic = 검증하기Logic;
            _인증하기Logic = 인증하기;
            _FormLogic = 파일선택;

            Init();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            인증시도();
        }

        #region Private Funcs
        private void Init()
        {
            _Message = new Messages();

            groupBox3.Enabled = false;
        }

        public void 인증시도(string 요양기관기호 = "99999999")
        {
            try
            {
                if (_인증하기Logic.UB2DCheckAuthProcess(요양기관기호))
                {
                    _e인증결과 = e인증결과.인증완료;
                    SetReadyToStart();
                }
                else
                {
                    _e인증결과 = e인증결과.인증실패;
                    MessageHelper.ShowMessageBox("인증에 실패했습니다.\n\n서버에 문제가 있거나 인터넷 연결에 문제가 있을 수 있습니다.\n\n잠시 후 다시 시도해 주시기 바랍니다.");
                    Show인증하기Button();
                }
            }
            catch (Exception ex)
            {
                _e인증결과 = e인증결과.DLL없음;
                MessageHelper.ShowMessageBox(ex.Message);
                Show인증하기Button();
            }
        }

        public bool 검증하기버튼(string path, I보험구분검증 보험구분검증Logic)
        {
            if (!_검증하기Logic.IsValid(path))
                return false;

            try
            {
                var data = _검증하기Logic.Get암호화해제Data(path);
                var parsedModel = ParseLogic.Parse(data);

                DisplayData(parsedModel);

                if(data != null)
                    _오류목록 = GetErrorData(data);

                if(parsedModel != null)
                    if (!보험구분검증Logic.Validation(parsedModel))
                        _오류목록.AddRange(보험구분검증Logic.GetErrorModel(parsedModel, _오류목록.Count()));

                SetErrorData(_오류목록);

                if (_오류목록.Count <= 0)
                {
                    var selectedRadioButtonName = groupBox2.Controls.OfType<RadioButton>().Where(x => x.Checked).First()?.Name.Replace("rb", "");

                    if(!string.IsNullOrEmpty(selectedRadioButtonName))
                        _FormLogic.SaveResult(selectedRadioButtonName, data);
                }

                return parsedModel == null ? false : true;
            }
            catch (MyLogicException ex)
            {
                MessageHelper.ShowMessageBox(ex.Message);
                return false;
            }
        }

        private I보험구분검증 Get검증하기Logic()
        {
            if (rb국민공단.Checked)
                return new 국민공단Logic();
            else if (rb차상위1.Checked)
                return new 차상위1Logic();
            else if (rb차상위2.Checked)
                return new 차상위2Logic();
            else if (rb차상위F.Checked)
                return new 차상위FLogic();
            else if (rb의료급여1종.Checked)
                return new 의료급여1종Logic();
            else if (rb의료급여2종.Checked)
                return new 의료급여2종Logic();
            else if (rb공무원상해.Checked)
                return new 공무원상해Logic();
            else if (rb자동차보험.Checked)
                return new 자동차보험Logic();
            else if (rb산재.Checked)
                return new 산재Logic();
            //else if(rb보훈국비.Checked)
            //    return new 보훈
            else
                return new 국민공단Logic(); // UI 기본 값이 이 값이라서 default로 해놓음.
        }

        private void DisplayData(BarcodeModel parsedModel)
        {
            if (parsedModel == null)
                return;
            
            int startLabelNumber = 40; // 폼에서 라벨을 만들다보니 시작이 40번이 되었다.

            foreach(var value in ParseLogic.GetAllPropertiesValue(parsedModel))
            {
                var labels = this.Controls.Find("label" + startLabelNumber++, true);

                if(labels.Count() > 0)
                {
                    var targetLabel = labels[0] as Label;

                    if(targetLabel != null)
                    {
                        targetLabel.Text = value;
                    }
                }
            }

            lstDrugs.Items.Clear();
            lstDrugs.BeginUpdate();

            for(int i = 0; i < parsedModel.RXDs.Count; i++)
            {
                ListViewItem item = new ListViewItem((i + 1).ToString());
                item.SubItems.Add(parsedModel.RXDs[i].처방구분);
                item.SubItems.Add(parsedModel.RXDs[i].급여구분);
                item.SubItems.Add(parsedModel.RXDs[i].코드구분);
                item.SubItems.Add(parsedModel.RXDs[i].청구코드사용자코드);
                item.SubItems.Add(parsedModel.RXDs[i].약품명);
                item.SubItems.Add(parsedModel.RXDs[i].일회투약량);
                item.SubItems.Add(parsedModel.RXDs[i].일회투여횟수);
                item.SubItems.Add(parsedModel.RXDs[i].총투약일수);
                item.SubItems.Add(parsedModel.RXDs[i].용법코드);
                item.SubItems.Add(parsedModel.RXDs[i].용법);

                lstDrugs.Items.Add(item);
            }

            lstDrugs.EndUpdate();
        }

        public List<오류목록Model> GetErrorData(string data)
        {
            // 가장 마지막에 \r\n이 있다면 삭제해준다.
            while(data.EndsWith("\r\n"))
            {
                var idx = data.LastIndexOf("\r\n");
                data = data.Substring(0, idx);
            }

            var splitData = data.Replace("\r\n", "¿").Split('¿');

            return ValidationLogic.GetErrorMessage(splitData);
        }

        public void SetErrorData(List<오류목록Model> errorData)
        {
            lstError.Items.Clear();
            lstError.BeginUpdate();

            foreach (var error in errorData)
            {
                ListViewItem item = new ListViewItem(error.No.ToString());
                item.SubItems.Add("유형");
                item.SubItems.Add(error.메세지);

                lstError.Items.Add(item);
            }

            lstError.EndUpdate();
        }

        public void 파일선택버튼()
        {
            _FormLogic.SetAfter파일선택((imagePath) => {
                //Is그룹박스닫힘 = true;
                Enable검증결과Group(false);
                txt파일경로.Text = imagePath;
            });

            _FormLogic.OpenFileDialog();
        }
        #endregion

        #region Private Funcs
        private void SetReadyToStart()
        {
            btn파일선택.Enabled = true;
            groupBox2.Enabled = true;
            btn인증하기.Visible = false;
        }

        private void Show인증하기Button()
        {
            btn인증하기.Visible = true;
        }

        private void Enable검증결과Group(bool isEnable)
        {
            tabControl1.TabIndex = 0;
            groupBox3.Enabled = isEnable;

            if (!isEnable)
                lstError.Items.Clear();
        }
        #endregion
        public class Sample
        {
            public string version { get; set; }
        }
        #region Events
        private void btn검증_Click(object sender, EventArgs e)
        {
            var 검증하기Logic = Get검증하기Logic();

            if (검증하기버튼(txt파일경로.Text, 검증하기Logic))
            {                
                Enable검증결과Group(true);
            }
            else
            {
                Enable검증결과Group(false);
            }
        }

        private void btn파일선택_Click(object sender, EventArgs e)
        {
            파일선택버튼();
        }

        private void btn닫기_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn인증하기_Click(object sender, EventArgs e)
        {
            인증시도();
        }
        #endregion

        private void btn도움말_Click(object sender, EventArgs e)
        {
            if (!CheckOpened("도움말"))
            {
                var frmHelp = new FrmHelp();
                frmHelp._parentForm = this;
                frmHelp.ShowForm();
            }
        }

        private bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    frm.Activate();
                    return true;
                }
            }
            return false;
        }
    }
}
