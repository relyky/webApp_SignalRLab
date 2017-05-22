using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webApp_SignalRLab.ViewModels
{
    public class LastErrMsg
    {
        public LastErrMsg()
        {
            this.errType = ParamEnum.errType_SUCCESS;
            this.errMsg = string.Empty;
            this.errDtm = DateTime.Now;
            this.errClass = string.Empty;
        }

        public LastErrMsg(string errType, string errMsg)
        {
            this.errType = errType;
            this.errMsg = errMsg;
            this.errDtm = DateTime.Now;
            this.errClass = string.Empty;
        }

        public LastErrMsg(string errType, string errMsg, string errClass)
        {
            this.errType = errType;
            this.errMsg = errMsg;
            this.errDtm = DateTime.Now;
            this.errClass = errClass;
        }

        public override string ToString()
        {
            return string.Format("{0}! {1}: {2} at {3}", this.errType, this.errClass, this.errMsg, this.errDtm);
            //return base.ToString();
        }

        /// <summary>
        /// error code: SUCCESS, WARNNING, ERROR, FAIL, EXCEPTION
        /// </summary>
        public string errType;
        /// <summary>
        /// error message
        /// </summary>
        public string errMsg;
        /// <summary>
        /// error datetime
        /// </summary>
        public DateTime errDtm;
        /// <summary>
        /// exception class name
        /// </summary>
        public string errClass;

        public class ParamEnum
        {
            public const string errType_SUCCESS = "SUCCESS";
            public const string errType_WARNNING = "WARNNING";
            public const string errType_ERROR = "ERROR";
            public const string errType_FAIL = "FAIL";
            public const string errType_EXCEPTION = "EXCEPTION";
        }
    }

}
