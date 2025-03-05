using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXLib;

namespace EZX.LightSpeedEngine
{
    public class LightspeedException : Exception
    {
        private LIGHTSPEED_EXCEPTION_TYPE exceptionType = LIGHTSPEED_EXCEPTION_TYPE.UNHANDELLED_EXCEPTION;

        public LIGHTSPEED_EXCEPTION_TYPE ExceptionType
        {
            get { return exceptionType; }
            set 
            { 
                exceptionType = value; 
            }
        }

        public LightspeedException()
            : this(string.Empty)
        {
        }

        public LightspeedException(string message)
            : this(message, LIGHTSPEED_EXCEPTION_TYPE.UNHANDELLED_EXCEPTION)
        {
        }

        public LightspeedException(string message, LIGHTSPEED_EXCEPTION_TYPE _exceptionType)
            : base(message)
        {
            this.ExceptionType = _exceptionType;
            Logger.INFO(message);
        }
    }


    public enum LIGHTSPEED_EXCEPTION_TYPE
    {
        GUI_KNOWN,
        DATA_VALIDATION,
        UNHANDELLED_EXCEPTION,
    }
}
