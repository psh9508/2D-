using _2D보험구분검증툴.Class;
using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace _2D보험구분검증툴.Logic
{
    public class 인증Logic : I인증하기
    {
        private bool _is인증완료;
        public bool Is인증완료 { get { return _is인증완료; } }

        I외부모듈 _외부모듈;

        public 인증Logic(I외부모듈 외부모듈)
        {
            _외부모듈 = 외부모듈;
        }

        public bool UB2DCheckAuthProcess(string 요양기관기호/*, BackgroundWorker loadingSpinner*/)
        {
            string day;
            //string caption = "2D 인증확인";

            var progressbar = new Spinner();
            var loadingThread = progressbar.GetLoadingWorker();

            try
            {
                loadingThread.RunWorkerAsync();

                if (UB2DCheckAuth(out day, 요양기관기호))
                {
                    loadingThread.Dispose();

                    if ((day != null && day.Equals("")) || day == null)
                        _is인증완료 = true;
                    else
                    {
                        var sb = new StringBuilder();
                        sb.Append($"[U pharm 2D바코드] 사용 기한이 {day}일 남았습니다.\n\n");
                        sb.Append($"사용자 인증 정보를 확인할 수 없을 경우 '{DateTime.Today.AddDays(Convert.ToSingle(day) - 1).ToShortDateString()}'일 까지만 사용 가능합니다.\n\n");
                        sb.Append("미리 본사 고객센터(☎ 02-2105-5000) 또는 해당 대리점으로 연락하여 주시기 바랍니다.");

                        //MessageBox.Show(sb.ToString(), caption);
                        _is인증완료 = false;
                    }
                }
                else
                {
                    loadingThread.Dispose();

                    var sb = new StringBuilder();
                    sb.Append("[U pharm 2D바코드] [서버인증]에 실패하였습니다.\n\n");
                    sb.Append("[U pharm 2D바코드]에 가입하시면 계속해서 서비스를 이용하실 수 있습니다.\n\n");
                    sb.Append("본사 고객센터(☎ 02-2105-5000) 또는 해당 대리점으로 연락하여 주시기 바랍니다.");

                    //MessageBox.Show(sb.ToString(), caption);
                    _is인증완료 = false;
                }
            }
            catch (DllNotFoundException ex)
            {
                loadingThread.Dispose();
                throw new DllNotFoundException(ex.Message);
                //throw new MyDllNotFoundException(ex.Message, loadingThread);
            }

            return _is인증완료;
        }

        public bool UB2DCheckAuth(out string day, string 요양기관기호 = "99999999")
        {
            day = null;
            const int USERCHECK_SUCCESS = 1; // 사용자 인증 성공

            bool retv = false;
            var resultDay = new StringBuilder(10);
            int 인증결과 = _외부모듈.CallUBSetPharmInfo(요양기관기호, "001", resultDay);
            var rDay = resultDay.ToString().Trim();

            if (인증결과 == USERCHECK_SUCCESS)
            {
                retv = true;

                if (!(rDay == null || rDay.Equals("")))
                    day = rDay;

                // Test
                //if (rDay != null)
                //    if (rDay != "")
                //        day = rDay;
            }

            return retv;
        }
    }
}
