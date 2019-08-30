using RestSharp;

namespace BceDotNetSdk
{
    public interface IBosRequest<T> where T : BosResponse
    {
        string GetApiUrl(string documentId = "");
        Method Method { get; }
        BceDocObject GetBizModel();

        //void SetBizModel(BceDocObject bizModel);
    }
}
