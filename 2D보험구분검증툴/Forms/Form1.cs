using _2D보험구분검증툴.Class;
using _2D보험구분검증툴.Interface;
using _2D보험구분검증툴.Logic;
using _2D보험구분검증툴.Logic.보험구분Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using UB2DBarcodeVerifier;
using _2D보험구분검증툴.Helper;
using static _2D보험구분검증툴.Test.TestEnum;
using static System.Net.Mime.MediaTypeNames;
using _2D보험구분검증툴.Class.Logic.보험구분Logic;
using _2D보험구분검증툴.Class.Logic.QRCode;
using MessagingToolkit.QRCode.ExceptionHandler;
using System.Diagnostics;

namespace _2D보험구분검증툴
{
    public partial class Form1 : Form
    {
        I검증하기 _검증하기Logic;
        I인증하기 _인증하기Logic;
        IForm _FormLogic;
        private string _barcodeString;
        private const string RESULT_PATH = @"C:\보험구분검증결과\";

        #region TEST
        public e인증결과 _e인증결과 { get; private set; }
        public List<오류목록Model> _오류목록 { get; private set; }
        public Button _인증하기Button { get { return btn인증하기; } }
        #endregion

        public Form1(I인증하기 인증하기, IForm formLogic)
        {
            InitializeComponent();

            //_검증하기Logic = 검증하기Logic;
            _인증하기Logic = 인증하기;
            _FormLogic = formLogic;

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
            this.TopMost = false;
            groupBox3.Enabled = false;
            groupBox6.Enabled = false;

            cbo보험구분테스트.DataSource = Get보험검증LogicLists();
            cbo보험구분테스트.DisplayMember = "Name";

            cbo보험구분테스트.SelectedIndex = 0;
            cbo약품구분테스트.SelectedIndex = 0;
        }

        private object Get보험검증LogicLists()
        {
            return new I보험구분검증[]
            {
                new 국민공단Logic(),
                new 공무원상해Logic(),
                new 보훈국비Logic(),
                new 산재Logic(),
                new 의료급여1종Logic(),
                new 의료급여2종Logic(),
                new 자동차보험Logic(),
                new 차상위1Logic(),
                new 차상위2Logic(),
                new 차상위FLogic(),
            };
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
                    _FormLogic.Show인증하기Button(btn인증하기);
                }
            }
            catch (Exception ex)
            {
                _e인증결과 = e인증결과.DLL없음;
                MessageHelper.ShowMessageBox(ex.Message);
                _FormLogic.Show인증하기Button(btn인증하기);
            }
        }

        public bool 검증하기버튼(string path, Base검증하기Logic 검증하기Logic, I보험구분검증 보험구분검증Logic)
        {
            // 정합성 테스트
            try
            {
                if (!검증하기Logic.IsValid())
                    return false;
            }
            catch (InvalidVersionException ex)
            {
                MessageHelper.ShowMessageBox(ex.Message);
                return false;
            }

            try
            {
                // 올바른 바코드 테스트
                _barcodeString = GetDecryptedString(검증하기Logic);
                BasicLayoutTest(_barcodeString);

                // 실제로 파씽
                var parsedModel = ParseLogic.Parse(_barcodeString);

                DisplayData(parsedModel);

                if(_barcodeString != null)
                    _오류목록 = GetErrorData(_barcodeString);

                if(parsedModel != null)
                    if (!보험구분검증Logic.Validation(parsedModel))
                        _오류목록.AddRange(보험구분검증Logic.GetErrorModel(parsedModel, _오류목록.Count()));

                SetErrorData(_오류목록);

                if (_오류목록.Count <= 0)
                {
                    var selectedInsuranceTypeName = GetSelectedInsuranceType();

                    if(!string.IsNullOrEmpty(selectedInsuranceTypeName))
                        _FormLogic.SaveResult(selectedInsuranceTypeName, 검증하기Logic.GetEncrytedString());
                }

                return parsedModel == null ? false : true;
            }
            catch (MyLogicException ex)
            {
                InitUI();
                MessageHelper.ShowMessageBox(ex.exceptionMessage);
                return false;
            }
           
        }

        private void Change검증ModeTo직접입력()
        {
            txt파일경로.Enabled = false;
            txt파일경로.Text = string.Empty;

            lbl직접입력.Enabled = true;
            txt직접입력.Enabled = true;
            txt직접입력.Text = string.Empty;
        }

        private void Change검증ModeTo파일경로()
        {
            txt파일경로.Enabled = true;

            lbl직접입력.Enabled = false;
            txt직접입력.Enabled = false;
            txt직접입력.Text = string.Empty;
        }

        public string GetDecryptedString(IDecryptable decryptLogic)
        {
            return decryptLogic.GetDecryptedString();
        }

        public void BasicLayoutTest(string barcodeString)
        {
            string noHeaders = string.Empty;

            if (barcodeString == null)
                throw new MyLogicException(@"올바른 내용의 바코드가 아닙니다.");
            else if (ValidationLogic.Has줄바꿈Error(barcodeString))
                throw new MyLogicException(@"헤더와 헤더 사이에[\r\n]로 이루어진 줄바꿈이 없습니다.");
            else if (!ValidationLogic.HasAllProperHeaders(barcodeString, out noHeaders))
                throw new MyLogicException($@"{noHeaders}헤더가 존재하지 않습니다. 헤더의 이름과 존재유무를 확인해주세요.");
        }

        private I보험구분검증 Get보험검증Logic()
        {
            try
            {
                var 선택된보험구분 = cbo보험구분테스트.SelectedItem as I보험구분검증;

                if (선택된보험구분 != null)
                    return 선택된보험구분;
                else
                    throw new Exception("선택된 보험구분이 I보험구분검증으로 변환되지 않습니다.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private Base검증하기Logic Get검증하기Logic()
        {
            if (txt파일경로.Enabled == false && !string.IsNullOrWhiteSpace(txt직접입력.Text))
                return new 검증하기LogicByString(txt직접입력.Text, new 외부모듈());

            return new 검증하기LogicByFile(txt파일경로.Text, new 외부모듈());
        }


        private void DisplayData(BarcodeModel parsedModel)
        {
            if (parsedModel == null)
                return;

            SetLabelInfo(parsedModel);
            SetDrugInfo(parsedModel);
        }

        private void SetLabelInfo(BarcodeModel parsedModel)
        {
            int startLabelNumber = 40; // 폼에서 라벨을 만들다보니 시작이 40번이 되었다.

            foreach (var value in ParseLogic.GetAllPropertiesValue(parsedModel))
            {
                var labels = this.Controls.Find("label" + startLabelNumber++, true);

                if (labels.Count() > 0)
                {
                    var targetLabel = labels[0] as Label;

                    if (targetLabel != null)
                    {
                        targetLabel.Text = value;
                    }
                }
            }
        }

        private void SetDrugInfo(BarcodeModel parsedModel)
        {
            lstDrugs.Items.Clear();
            lstDrugs.BeginUpdate();

            for (int i = 0; i < parsedModel.RXDs.Count; i++)
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
                item.SubItems.Add(error.유형.IfNotHasValue("유형"));
                item.SubItems.Add(error.메세지);

                lstError.Items.Add(item);
            }

            lstError.EndUpdate();
        }

        public void 파일선택버튼()
        {
            _FormLogic.SetAfter파일선택((imagePath) =>
            {
                //Is그룹박스닫힘 = true;
                Change검증ModeTo파일경로();
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

        private void Enable검증결과Group(bool isEnable)
        {
            tabControl1.TabIndex = 0;
            groupBox3.Enabled = isEnable;

            if (!isEnable)
                lstError.Items.Clear();
        }
        #endregion
      
        #region Events
        private void btn검증_Click(object sender, EventArgs e)
        {
            //BackgroundWorker loadingThead = MakeLoadingBar();
            //loadingThead.RunWorkerAsync();

            _barcodeString = string.Empty;

            var 보험검증Logic = Get보험검증Logic();
            var 검증하기Logic = Get검증하기Logic();

            try
            {
                var 검증결과 = 검증하기버튼(txt파일경로.Text, 검증하기Logic, 보험검증Logic);

                Enable검증결과Group(검증결과);

                //loadingThead.Dispose();

                if (_오류목록?.Count <= 0 && 검증결과)
                    MessageHelper.ShowMessageBox($"검증이 완료 되었습니다.\n\n[{RESULT_PATH}]에 검증결과를 저장하였습니다.");
                else
                {
                    Change검증ModeTo직접입력();
                    throw new MyLogicException("검증에 실패했습니다.");
                }
            }
            catch (MyLogicException ex)
            {
                //loadingThead.Dispose();
                MessageHelper.ShowMessageBox(ex.exceptionMessage);
                //MessageHelper.ShowMessageBox(ex.Message);
            }
        }

        private void InitUI()
        {
            ClearLabelInfo();
            lstError.Items.Clear();
        }

        private void ClearLabelInfo()
        {
            int startLabelNumber = 40; // 폼에서 라벨을 만들다보니 시작이 40번이 되었다.
            int lastLabelNumber = 76; // 폼에서 라벨 마지막 번호.

            int modelPropertyCount = lastLabelNumber - startLabelNumber;

            for (int i = 0; i < modelPropertyCount; i++)
            {
                var labels = this.Controls.Find("label" + startLabelNumber++, true);

                if (labels.Count() > 0)
                {
                    var targetLabel = labels[0] as Label;

                    if (targetLabel != null)
                    {
                        targetLabel.Text = string.Empty;
                    }
                }
            }
        }

        private static BackgroundWorker MakeLoadingBar()
        {
            //var spinner = new Spinner(System.Windows.Forms.Application.OpenForms["Form1"], "검증하고 있습니다.");
            var spinner = Spinner.GetInstance(System.Windows.Forms.Application.OpenForms["Form1"], "검증하고 있습니다.");
            return spinner.GetLoadingWorker(spinner);
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
            FormCollection fc = System.Windows.Forms.Application.OpenForms;

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

        private void btn바코드보기_Click(object sender, EventArgs e)
        {
            FormCollection fc = System.Windows.Forms.Application.OpenForms;

            for (int i = 0; i < fc.Count; i++)
                if (fc[i].Text == "바코드 보기")
                    fc[i].Close();

            var frm = new FrmShowBarcode(GetSelectedInsuranceType(), _barcodeString);
            frm.Show();
        }

        private string GetSelectedInsuranceType()
        {
            return cbo보험구분테스트.Text;
        }

        private void rb약품구분테스트_CheckedChanged(object sender, EventArgs e)
        {
            groupBox6.Enabled = rb약품구분테스트.Checked;
            rb보험구분테스트.Checked = !rb약품구분테스트.Checked;
        }

        private void rb보험구분테스트_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = rb보험구분테스트.Checked;
            rb약품구분테스트.Checked = !rb보험구분테스트.Checked;
        }

        private void btn검증결과폴더이동_Click(object sender, EventArgs e)
        {
            Process.Start(RESULT_PATH);
        }
    }
}
