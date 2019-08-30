using BceDotNetSdk.Response;
using RestSharp;

namespace BceDotNetSdk.Request.Doc
{
    /// <summary>
    /// 通过文档的唯一标识 documentId 查询指定文档的详细信息。
    /// </summary>
    public class GetDocumentRequest : IBosRequest<GetDocumentResponse>
    {
        private readonly BceDocObject _bizModel;

        public GetDocumentRequest(BceDocObject bizModel)
        {
            _bizModel = bizModel;
        }

        public string GetApiUrl(string documentId = "")
        {
            return $"/v2/document/{documentId}";
        }

        public Method Method => Method.GET;

        public BceDocObject GetBizModel()
        {
            return this._bizModel;
        }
    }
}
