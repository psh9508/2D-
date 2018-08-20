using _2D보험구분검증툴.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2D보험구분검증툴.Interface
{
    public interface I검증하기
    {
        string ErrorMessage { get; set; }
        string Get바코드Data(string imagePath);
        string Get암호화해제Data(string 암호화된Data);
        bool IsValid(string imagePath);
        //BarcodeModel GetParsedModel(string imagePath);
        bool Has파일경로(string imagePath);
        bool Is파일존재(string imagePath);
        bool Is파일QR코드(string imagePath);
        bool IsValidVersion(string imagePath);
    }

    public interface IForm
    {
        //void SetAfter파일선택(Action<string> action);
        void OpenFileDialog();
        void SaveResult(string insuranceName, string data);
        void Show인증하기Button(Button btn인증하기);
        void SetAfter파일선택(Action<string> action);
    }

    public interface I인증하기
    {
        bool Is인증완료 { get; }
        bool UB2DCheckAuthProcess(string 요양기관기호);
        bool UB2DCheckAuth(out string day, string 요양기관기호 = "99999999");
    }

    public interface I외부모듈
    {
        int CallUBSetPharmInfo(string strPharmNum, string strPrdCode, StringBuilder iDay);
        int CallUB2DDecode(string data, StringBuilder decrypt);
    }

    public interface I보험구분검증
    {
        bool Validation(BarcodeModel model);
        IEnumerable<오류목록Model> GetErrorModel(BarcodeModel model, int cnt);
    }

    interface IHeader<T>
    {
        T GetHeaderModel();
    }
}
