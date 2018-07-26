using _2D보험구분검증툴.Class;
using _2D보험구분검증툴.Interface;
using _2D보험구분검증툴.Test;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using MessagingToolkit.QRCode.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Logic
{
    public class 검증하기Logic : I검증하기
    {
        private readonly I외부모듈 _외부모듈;
        private readonly I보험구분검증 _보험구분검증;
        public string ErrorMessage { get; set; }

        public 검증하기Logic(I외부모듈 외부모듈/*, I보험구분검증 보험구분검증*/)
        {
            _외부모듈 = 외부모듈;
            //_보험구분검증 = 보험구분검증;
        }

        public bool Has파일경로(string imagePath)
        {
            return !string.IsNullOrWhiteSpace(imagePath);
        }

        public bool Is파일존재(string imagePath)
        {
            return File.Exists(imagePath);
        }

        public bool IsValidVersion(string imagePath)
        {
            try
            {
                var decoder = new QRCodeDecoder();

                var image = Image.FromFile(imagePath) as Bitmap;
                var bitmapImage = new QRCodeBitmapImage(image);
                var s = decoder.Decode(bitmapImage);

                return true;
            }
            catch (InvalidVersionException ex)
            {
                return false;
            }
            catch(Exception ex)
            {
                // 버전 이외의 에러는 다른 곳에서 처리한다.
                return true;
            }
        }

        public bool Is파일QR코드(string imagePath)
        {
            try
            {
                var decoder = new QRCodeDecoder();

                var image = Image.FromFile(imagePath) as Bitmap;
                var bitmapImage = new QRCodeBitmapImage(image);
                var s = decoder.Decode(bitmapImage);

                return !string.IsNullOrWhiteSpace(s);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsValid(string imagePath)
        {
            try
            {
                ErrorMessage = string.Empty;

                if (!Has파일경로(imagePath))
                    ErrorMessage = "파일경로가 비어있습니다. 파일을 먼저 선택해주시기 바랍니다.";
                else if (!Is파일존재(imagePath))
                    ErrorMessage = "입력하신 경로에 파일이 존재하지 않습니다.";
                else if (!IsValidVersion(imagePath))
                    ErrorMessage = "이용가능한 버전의 QR코드가 아닙니다.";
                else if (!Is파일QR코드(imagePath))
                    ErrorMessage = "입력하신 경로에 파일이 QR코드가 아닙니다.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(ErrorMessage))
                    MessageHelper.ShowMessageBox(ErrorMessage);
            }

            return string.IsNullOrWhiteSpace(ErrorMessage) ? true : false;
        }

        public string Get바코드Data(string imagePath)
        {
            var decoder = new QRCodeDecoder();

            var image = Image.FromFile(imagePath) as Bitmap;
            var bitmapImage = new QRCodeBitmapImage(image);
            var s = decoder.Decode(bitmapImage);

            // 줄바꿈 문제가 있는 바코드
            //s = @"UB<~M=<M""HcmtDYC#.(r-1iF7d#=IpkalB>694'N/]Tnf0&p,jU5-s5S=^%?$S;8G/*`a3q][':egX.5^EW)U@&=)gAZ_WW_eC6`mdbMODC([LY6\aEYMcqm&>=Mp7*""WWM(X(%rgBPh6YdQ'9*3N/Bb>ZeMo%mDS3GkU0M+O\(h;Q@q+K.R%/JfnBNg-g:5TO`J#'D0QW0FaF(1\`d!cLCLW6>e:*WB1](?]""jl2Hp<PS2G&iHo[b/jRV8hMjl*LQN4&fk1cjS5Pjc:pde_gUOe(]#hJ892S,B#c`b-`""Dbn.5<Vr/&kq5eM,YM[-MUr^ls[IJHL`8lQ+^l)9@KMr83B3%K0;SU#2LeAPnc93M*~>";

            return s;
        }

        public string Get암호화해제Data(string imagePath)
        {
            var data = Get바코드Data(imagePath);

            if (string.IsNullOrEmpty(data))
                throw new MyLogicException("바코드 이미지에서 데이터를 가져오지 못함.");

            StringBuilder decodeData = new StringBuilder(2048);

            int result = _외부모듈.CallUB2DDecode(data, decodeData);

            return result == 1 ? decodeData.ToString() : null;
        }

        //public BarcodeModel GetParsedModel(string data)
        //{
        //    return ParseLogic.Parse(data);
        //}

    }
}
