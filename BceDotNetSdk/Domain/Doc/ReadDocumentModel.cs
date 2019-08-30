using Newtonsoft.Json;

namespace BceDotNetSdk.Domain.Doc
{
    public class ReadDocumentModel : BceDocObject
    {
        /// <summary>
        /// 阅读Token有效期，仅当阅读私有文档（"access" : "PRIVATE"）时有效。单位：秒，默认值：3600
        /// </summary>
        [JsonProperty("expireInSeconds")]
        public long ExpireInSeconds { get; set; }
    }
}
