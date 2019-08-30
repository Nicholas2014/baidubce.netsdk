using BceDotNetSdk.Response;
using RestSharp;

namespace BceDotNetSdk.Request.Doc
{
    /// <summary>
    /// 用于对已完成注册和BOS上传的文档进行发布处理。仅对状态为UPLOADING的文档有效。处理过程中，文档状态为PROCESSING；处理完成后，状态转为PUBLISHED。
    /// 发布文档是文档三步创建法（注册文档、上传BOS、发布文档）的第三步。
    /// </summary>
    public class PublishDocumentRequest : IBosRequest<PublishDocumentResponse>
    {
        private readonly BceDocObject _bizModel;

        public PublishDocumentRequest(BceDocObject bizModel)
        {
            _bizModel = bizModel;
        }

        public string GetApiUrl(string documentId = "")
        {
            return $"/v2/document/{documentId}?publish";
        }

        public Method Method => Method.PUT;

        public BceDocObject GetBizModel()
        {
            return this._bizModel;
        }
    }
}
