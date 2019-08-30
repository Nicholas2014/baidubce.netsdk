using BceDotNetSdk.Response;
using RestSharp;

namespace BceDotNetSdk.Request.Doc
{
    public class RegisterDocumentRequest : IBosRequest<RegisterDocumentResponse>
    {
        private readonly BceDocObject _bizModel;

        public RegisterDocumentRequest(BceDocObject bizModel)
        {
            _bizModel = bizModel;
        }

        public string GetApiUrl(string documentId = "")
        {
            return $"/v2/document?register";
        }

        public Method Method => Method.POST;

        public BceDocObject GetBizModel()
        {
            return this._bizModel;
        }
    }
}
