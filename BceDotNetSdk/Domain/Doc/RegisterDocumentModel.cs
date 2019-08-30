using Newtonsoft.Json;

namespace BceDotNetSdk.Domain.Doc
{
    public class RegisterDocumentModel:BceDocObject
    {
        /// <summary>
        /// 文档标题，不超过50字符 必填
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 文档格式。有效值：doc, docx, ppt, pptx, xls, xlsx, vsd, pot, pps, rtf, wps, et, dps, pdf, txt, epub  必填
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
