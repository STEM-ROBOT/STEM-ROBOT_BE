using Azure.Identity;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public abstract class Basersp
    {
      
        public Basersp()
        {
            Success = true;
            msg = string.Empty;
            titleError = "Eroor";
            Dev = true;
            if (string.IsNullOrEmpty(err))
            {
                err = "Please update common error";
            }
        }

        public Basersp(string message) : this()
        {
            msg = message;
        }

        public Basersp(string message, string titleError): this(message)
        {
            this.titleError = titleError;
        }

        public void SetError(string message)
        {
            Success = false;
            msg = message;
        }
        public void SetError(string code ,string message)
        {
            Success = false;
            Code = code;
            msg = message;
        }
        public void SetMessage(string message)
        {
            msg = message;
        }
        public void TestError()
        {
            SetError("We are tesing to show error message");
        }
        public bool Success { get; set; }
        public string Code { get; set; }

        public string Message
        {
            get
            {
                if (Success)
                {
                    return msg;
                }
                else
                {
                    return Dev ? msg : err;
                }
            }
        }

        public string Variant
        {
            get
            {
                return Success ? "success" : "error";
            }
        }

        public string Title
        {
            get
            {
                return Success ? "Success" : titleError;
            }
        }

        private static bool Dev { get; set; }

        private readonly string err;

        private readonly string titleError;

        private string msg;

    }
}
