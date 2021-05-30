using System.Net;

namespace ApiSkeleton.Common
{
    public class LogicException : System.Exception
    {
        public ResultCode ResultCode { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public string Detail { get; set; }

        public LogicException(ResultCode resultCode, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            ResultCode = resultCode;
            HttpStatusCode = httpStatusCode;
        }

        public LogicException(ResultCode resultCode, string detail, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            ResultCode = resultCode;
            HttpStatusCode = httpStatusCode;
            Detail = detail;
        }
    }
}
