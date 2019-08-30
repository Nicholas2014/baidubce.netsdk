using Newtonsoft.Json;

namespace BceDotNetSdk.Domain.Doc
{
    public class CreateDocumentModel : BceDocObject
    {
        /// <summary>
        /// BOS Bucket 必填
        /// </summary>
        [JsonProperty("bucket")]
        public string Bucket { get; set; }
        /// <summary>
        /// BOS Object  必填
        /// </summary>
        [JsonProperty("object")]
        public string Object { get; set; }

        /// <summary>
        /// 文档标题，不超过50字符 必填
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 文档格式。有效值：doc, docx, ppt, pptx, xls, xlsx, vsd, pot, pps, rtf, wps, et, dps, pdf, txt, epub。默认值：BOS Object后缀名（当BOS Object有后缀时）
        /// BOS Object有后缀时可选，否则必选
        /// </summary>
        [JsonProperty("format")]
        public string Format { get; set; }
        /// <summary>
        /// 通知名称
        /// </summary>
        [JsonProperty("notification")]
        public string Notification { get; set; }
        /// <summary>
        /// 文档权限。有效值：PUBLIC、PRIVATE，默认值：PUBLIC，表示公开文档，设为PRIVATE时表示私有文档
        /// </summary>
        [JsonProperty("access")]
        public string Access { get; set; }
        /// <summary>
        /// 转码结果类型：h5, image，默认值为h5。（目前暂不支持txt格式文档转码成image类型） 
        /// </summary>
        [JsonProperty("targetType")]
        public string TargetType { get; set; }
    }
}
