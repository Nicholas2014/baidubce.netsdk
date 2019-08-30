using BceDotNetSdk.Response;
using RestSharp;

namespace BceDotNetSdk.Request.Doc
{
    /// <summary>
    /// 通过BOS Object路径，用从BOS导入的方法创建文档
    /// </summary>
    public class CreateDocumentRequest : IBosRequest<CreateDocumentResponse>
    {
        private readonly BceDocObject _bizModel;

        public CreateDocumentRequest(BceDocObject bizModel)
        {
            _bizModel = bizModel;
        }

        public string GetApiUrl(string documentId = "")
        {
            return $"/v2/document?source=bos";
        }

        public Method Method => Method.POST;
        
        public BceDocObject GetBizModel()
        {
            return this._bizModel;
        }
    }
}
