using BceDotNetSdk.Response;
using RestSharp;

namespace BceDotNetSdk.Request.Doc
{
    /// <summary>
    /// 通过文档的唯一标识 documentId 获取指定文档的阅读信息，以便在PC/Android/iOS设备上阅读。仅对状态为PUBLISHED的文档有效。
    /// </summary>
    public class ReadDocumentRequest : IBosRequest<ReadDocumentResponse>
    {
        private readonly BceDocObject _bizModel;

        public ReadDocumentRequest(BceDocObject bizModel)
        {
            _bizModel = bizModel;
        }

        public string GetApiUrl(string documentId = "")
        {
            return $"/v2/document/{documentId}?read";
        }

        public Method Method => Method.GET;
        
        public BceDocObject GetBizModel()
        {
            return this._bizModel;
        }
    }
}
