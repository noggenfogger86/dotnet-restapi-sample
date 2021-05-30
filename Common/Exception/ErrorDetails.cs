namespace ApiSkeleton.Common
{
    public class ErrorDetails : ResponseHeader
    {
        public int StatusCode { get; set; }

        public string Detail { get; set; }

        public string StackTrace { get; set; }
    }
}
