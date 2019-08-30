using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace BceDotNetSdk
{
    public class BCESignerV1 : BCESignerBase
    {
        private readonly string _ak;
        private readonly string _sk;

        public BCESignerV1(string ak, string sk) : base(ak, sk)
        {
            _ak = ak;
            _sk = sk;
        }

        private Dictionary<string, SignResult> Items = new Dictionary<string, SignResult>();
        private int ExpirationInSeconds = 1800;

        public SignResult Sign(string url, string method, bool cache = true)
        {
            if (string.IsNullOrEmpty(_ak) || string.IsNullOrEmpty(_sk))
            {
                throw new Exception("未配置百度云密钥。");
            }

            var key = $"{url}_{method}";
            var now = DateTime.Now;
            if (!Items.TryGetValue(key, out var authItem) || now.Subtract(authItem.CreateTime).TotalSeconds >= ExpirationInSeconds)
            {
                var signDate = now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");

                var authString = "bce-auth-v1/" + _ak + "/" + signDate + "/" + ExpirationInSeconds;
                var signingKey = Hex(new HMACSHA256(Encoding.UTF8.GetBytes(_sk)).ComputeHash(Encoding.UTF8.GetBytes(authString)));
                var canonicalRequestString = CanonicalRequest(url, method);
                var signature = Hex(new HMACSHA256(Encoding.UTF8.GetBytes(signingKey)).ComputeHash(Encoding.UTF8.GetBytes(canonicalRequestString)));
                var authorization = authString + "/host/" + signature;

                authItem = new SignResult()
                {
                    Authorization = authorization,
                    CreateTime = now,
                    SignDate = signDate
                };
                if (cache)
                {
                    Items[key] = authItem;
                }
            }

            return authItem;
        }

        public string CanonicalRequest(string url, string method)
        {
            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.Method = method;

                return CanonicalRequest(webRequest);
            }

            return string.Empty;
        }

        public string CanonicalRequest(HttpWebRequest req)
        {
            var canonicalReq = new StringBuilder();

            var uri = req.RequestUri;
            canonicalReq.Append(req.Method).Append("\n").Append(UriEncode(Uri.UnescapeDataString(uri.AbsolutePath))).Append("\n");
            var canonicalQueryString = ParseQuery(uri.Query);
            canonicalReq.Append(canonicalQueryString).Append("\n");
            var host = uri.Host;
            if (!(uri.Scheme == "https" && uri.Port == 443) && !(uri.Scheme == "http" && uri.Port == 80))
            {
                host += ":" + uri.Port;
            }
            canonicalReq.Append("host:" + UriEncode(host));

            return canonicalReq.ToString();
        }

        private string ParseQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return string.Empty;
            }

            var paras = query.Substring(1).Split('&');
            if (paras.Length == 0)
            {
                return string.Empty;
            }

            var dic = new Dictionary<string, string>();
            foreach (var para in paras)
            {
                var nv = para.Split('=');
                if (nv.Length == 0)
                {
                    continue;
                }

                dic.Add(nv[0], nv.Length == 1 ? "" : nv[1]);
            }

            return GetCanonicalQueryString(dic, true);
        }
    }


}
