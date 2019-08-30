using Newtonsoft.Json;

namespace BceDotNetSdk.Response
{
    public class ReadDocumentResponse : BosResponse
    {
        /// <summary>
        /// 系统生成的文档的唯一标识
        /// </summary>
        [JsonProperty("documentId")]
        public string DocumentId { get; set; }
        /// <summary>
        /// 文档阅读ID，与documentId取值相同
        /// </summary>
        [JsonProperty("docId")]
        public string DocId { get; set; }
        /// <summary>
        /// 文档阅读Host，用于阅读器SDK，取固定值BCEDOC
        /// </summary>
        [JsonProperty("host")]
        public string Host { get; set; }
        /// <summary>
        /// 文档阅读Token，对于公开文档（"access" : "PUBLIC"）会返回固定值"TOKEN"，对于私有文档（"access" : "PRIVATE"）会返回随机字符串
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
        /// <summary>
        /// 阅读Token创建时间，仅当阅读私有文档时返回
        /// </summary>
        [JsonProperty("createTime")]
        public string CreateTime { get; set; }
        /// <summary>
        /// 阅读Token失效时间，仅当阅读私有文档时返回
        /// </summary>
        [JsonProperty("expireTime")]
        public string ExpireTime { get; set; }
    }
}
