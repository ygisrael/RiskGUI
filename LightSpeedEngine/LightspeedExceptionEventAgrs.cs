using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZX.LightSpeedEngine
{
    public class LightspeedExceptionEventAgrs : EventArgs
    {
        private string exceptionMessage;
        private LIGHTSPEED_EXCEPTION_TYPE exceptionType;
        private Exception exception;

        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }
        public LIGHTSPEED_EXCEPTION_TYPE ExceptionType
        {
            get { return exceptionType; }
            set { exceptionType = value; }
        }
        public Exception Exception
        {
            get { return exception; }
            set { exception = value; }
        }


        public LightspeedExceptionEventAgrs()
            : base()
        {
        }
    }
}
