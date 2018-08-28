using _2D보험구분검증툴.Interface;
using _2D보험구분검증툴.Logic;
using MessagingToolkit.QRCode.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ZXing;

namespace _2D보험구분검증툴.Class.Logic.QRCode
{
    public class 검증하기LogicByFile : Base검증하기Logic
    {
        private readonly IBarcodeReader _barcodeReader;

        public 검증하기LogicByFile(string filePath, I외부모듈 외부모듈) : base(외부모듈)
        {
            _filePath = filePath;
            _barcodeReader = new BarcodeReader();
        }

        public override string GetDecryptedString()
        {
            var data = GetEncrytedString();

            if (string.IsNullOrEmpty(data))
                throw new MyLogicException("바코드 이미지에서 데이터를 가져오지 못함.");

            StringBuilder decodeData = new StringBuilder(2048);

            int result = _외부모듈.CallUB2DDecode(data, decodeData);

            return result == 1 ? decodeData.ToString() : null;
        }

        protected override void IsValidByLogic()
        {
            if (!Has파일경로(_filePath))
                ErrorMessage = "파일경로가 비어있습니다. 파일을 먼저 선택해주시기 바랍니다.";
            else if (!Is파일존재(_filePath))
                ErrorMessage = "입력하신 경로에 파일이 존재하지 않습니다.";
            else if (!IsValidVersion(_filePath))
                ErrorMessage = "이용가능한 버전의 QR코드가 아닙니다.";
            else if (!Is파일QR코드(_filePath))
                ErrorMessage = "입력하신 경로에 파일이 QR코드가 아닙니다.";
        }

        public bool Has파일경로(string imagePath)
        {
            return !string.IsNullOrWhiteSpace(imagePath);
        }

        public bool Is파일존재(string imagePath)
        {
            return File.Exists(imagePath);
        }

        private bool Is파일QR코드(string imagePath)
        {
            try
            {
                var s = string.Empty;

                var barcodeBitmap = (Bitmap)Image.FromFile(imagePath);

                var result = _barcodeReader.Decode(barcodeBitmap);

                if (result != null)
                    s = result.Text;

                return !string.IsNullOrWhiteSpace(s);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool IsValidVersion(string imagePath)
        {
            var errorMessage = "이용가능한 버전의 QR코드가 아닙니다.\n\n바코드를 직접 읽으신 후 그 값을 직접입력 창을 통해 입력해보시기 바랍니다.";

            try
            {
                var barcodeBitmap = (Bitmap)Image.FromFile(imagePath);

                var result = _barcodeReader.Decode(barcodeBitmap);

                if (result != null)
                    return true;

                return false;
            }
            catch (InvalidVersionException ex)
            {
                throw new InvalidVersionException(errorMessage);
            }
            catch (Exception ex)
            {
                // 버전에러 이외의 에러는 다른 곳에서 처리한다.
                return true;
            }
        }

        public override string GetEncrytedString()
        {
            var s = string.Empty;

            var barcodeBitmap = (Bitmap)Image.FromFile(_filePath);

            var result = _barcodeReader.Decode(barcodeBitmap);

            if (result != null)
                s = result.Text;

            // 줄바꿈 문제가 있는 바코드
            //s = @"UB<~M=<M""HcmtDYC#.(r-1iF7d#=IpkalB>694'N/]Tnf0&p,jU5-s5S=^%?$S;8G/*`a3q][':egX.5^EW)U@&=)gAZ_WW_eC6`mdbMODC([LY6\aEYMcqm&>=Mp7*""WWM(X(%rgBPh6YdQ'9*3N/Bb>ZeMo%mDS3GkU0M+O\(h;Q@q+K.R%/JfnBNg-g:5TO`J#'D0QW0FaF(1\`d!cLCLW6>e:*WB1](?]""jl2Hp<PS2G&iHo[b/jRV8hMjl*LQN4&fk1cjS5Pjc:pde_gUOe(]#hJ892S,B#c`b-`""Dbn.5<Vr/&kq5eM,YM[-MUr^ls[IJHL`8lQ+^l)9@KMr83B3%K0;SU#2LeAPnc93M*~>";

            return s;
        }
    }
}
