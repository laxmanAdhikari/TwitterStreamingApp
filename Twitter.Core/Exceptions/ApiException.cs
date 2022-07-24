using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Core.Exceptions
{
    public  class ApiException: Exception
    {
        public JObject Error { get; init; }

        public ApiException() : base() { }

        public ApiException(string message) : base(message) { }

        public ApiException(string message, Exception innerException) : base(message, innerException) { }

        public ApiException(string message, JObject error) : base(message)
        {
            Error = error;
        }

        public ApiException(string message, Exception innerException, JObject error) : base(message, innerException)
        {
            Error = error;
        }
    }
}
