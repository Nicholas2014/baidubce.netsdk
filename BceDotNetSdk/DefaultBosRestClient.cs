using Newtonsoft.Json;
using RestSharp;

namespace BceDotNetSdk
{
    public class DefaultBosRestClient : IBosRestClient
    {
        private readonly BCESignerV1 _signer;
        private RestClient _client;

        public DefaultBosRestClient(string ak, string sk, string apiBaseUrl)
        {
            _signer = new BCESignerV1(ak, sk);
            _client = new RestClient(apiBaseUrl);
        }

        public T Execute<T>(IBosRequest<T> request) where T : BosResponse, new()
        {
            var bizModel = request.GetBizModel();
            var restRequest = new RestRequest(request.GetApiUrl(bizModel.DocumentId), request.Method);
            var json = JsonConvert.SerializeObject(bizModel, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            restRequest.AddJsonBody(json);
            var authorization = this._signer.Sign(_client.BuildUri(restRequest).AbsoluteUri, restRequest.Method.ToString());
            restRequest.AddHeader("authorization", authorization.Authorization);
            var res = _client.Execute(restRequest);
            if (res.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<T>(res.Content);
            }

            return null;
        }
    }
}
