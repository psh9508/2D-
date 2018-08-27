using _2D보험구분검증툴.Class;
using _2D보험구분검증툴.Class.Logic.DecryptLogic;
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
using ZXing;

namespace _2D보험구분검증툴.Logic
{
    public abstract class Base검증하기Logic : IDecryptable//, I검증하기
    {
        protected readonly I외부모듈 _외부모듈;
        protected readonly I보험구분검증 _보험구분검증;
        protected string _encryptedData;
        protected string _filePath;

        public string ErrorMessage { get; set; }

        public Base검증하기Logic(I외부모듈 외부모듈/*, I보험구분검증 보험구분검증*/)
        {
            _외부모듈 = 외부모듈;
            //_보험구분검증 = 보험구분검증;
        }

        public bool IsValid()
        {
            try
            {
                ErrorMessage = string.Empty;

                IsValidByLogic();
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

        protected abstract void IsValidByLogic();

        public abstract string GetDecryptedString();

        public abstract string GetEncrytedString();

    }
}
