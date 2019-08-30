using Newtonsoft.Json;

namespace BceDotNetSdk.Response
{
    public class GetDocumentResponse : BosResponse
    {
        [JsonProperty("documentId")]
        public string DocumentId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("format")]
        public string Format { get; set; }
        [JsonProperty("targetType")]
        public string TargetType { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("notification")]
        public string Notification { get; set; }

        [JsonProperty("access")]
        public string Access { get; set; }

        [JsonProperty("createTime")]
        public string CreateTime { get; set; }

        [JsonProperty("uploadInfo")]
        public UploadInfoitem UploadInfo { get; set; }

        [JsonProperty("publishInfo")]
        public PublishInfoItem PublishInfo { get; set; }

        [JsonProperty("error")]
        public ErrorInfoItem Error { get; set; }

        public class UploadInfoitem
        {
            [JsonProperty("bucket")]
            public string Bucket { get; set; }
            [JsonProperty("object")]
            public string Object { get; set; }
            [JsonProperty("bosEndpoint")]
            public string BosEndpoint { get; set; }
        }
        public class PublishInfoItem
        {
            [JsonProperty("pageCount")]
            public string PageCount { get; set; }
            [JsonProperty("sizeInBytes")]
            public string SizeInBytes { get; set; }
            [JsonProperty("coverUrl")]
            public string CoverUrl { get; set; }
            [JsonProperty("publishTime")]
            public string PublishTime { get; set; }
        }
        public class ErrorInfoItem
        {
            [JsonProperty("code")]
            public string Code { get; set; }
            [JsonProperty("message")]
            public string Message { get; set; }
        }

    }
}

