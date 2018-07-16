using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Class
{
    class MyDllNotFoundException : DllNotFoundException
    {
        public BackgroundWorker loadingForm;
        public string exceptionMessage;

        public MyDllNotFoundException(string message, BackgroundWorker loadingForm)
        {
            this.exceptionMessage = message;
            this.loadingForm = loadingForm;
        }
    }

    class MyFormException : Exception
    {

    }

    class MyLogicException : Exception
    {
        public string exceptionMessage;

        public MyLogicException(string message)
        {
            exceptionMessage = message;
        }
    }
}
