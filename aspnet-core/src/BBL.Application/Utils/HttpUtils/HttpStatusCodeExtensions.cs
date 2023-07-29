using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BBL.Utils.HttpUtils
{
    public static class HttpStatusCodeExtensions
    {
        public static int GetCode(this HttpStatusCode value) => (int)value;

    }
}
