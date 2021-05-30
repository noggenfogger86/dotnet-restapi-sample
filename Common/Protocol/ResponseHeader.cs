using Newtonsoft.Json;

namespace ApiSkeleton.Common
{
    public class ResponseHeader
    {
        [JsonProperty]
        public readonly string Id;

        public ResultCode ResultCode { get; set; } = ResultCode.Success;

        public string ErrorMessage { get; set; }

        public ResponseHeader()
        {
            Id = this.GetType().FullName;
        }
    }
}
