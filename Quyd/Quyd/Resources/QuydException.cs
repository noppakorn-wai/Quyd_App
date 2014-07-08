using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quyd.Resources
{
    class QuydException : Exception
    {
        public enum ErrorCode { store_nameTooShort, store_nameExist, store_locationInvalid, store_phoneInvalid, store_notFound}

        ErrorCode errorCode;
        public QuydException(ErrorCode errorCode, string message) : base(message) { this.errorCode = errorCode; }
    }
}
