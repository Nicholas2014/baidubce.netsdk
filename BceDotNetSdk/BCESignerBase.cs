using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RestSharp;

namespace BceDotNetSdk
{
    public abstract class BCESignerBase
    {
        private static readonly string[] PercentEncodedStrings = Enumerable.Range(0, 256).Select<int, string>((Func<int, string>)(v => "%" + v.ToString("X2"))).ToArray<string>();

        static BCESignerBase()
        {
            for (char ch = 'a'; ch <= 'z'; ++ch)
                PercentEncodedStrings[(int)ch] = ch.ToString();
            for (char ch = 'A'; ch <= 'Z'; ++ch)
                PercentEncodedStrings[(int)ch] = ch.ToString();
            for (char ch = '0'; ch <= '9'; ++ch)
                PercentEncodedStrings[(int)ch] = ch.ToString();
            PercentEncodedStrings[45] = "-";
            PercentEncodedStrings[46] = ".";
            PercentEncodedStrings[95] = "_";
            PercentEncodedStrings[126] = "~";
        }
        /// <summary>
        /// The version information for Document service APIs as URI prefix.
        /// </summary>
        private static readonly string VERSION = "v2";
        /// <summary>
        /// The common URI prefix for doc services.
        /// </summary>
        private static readonly string DOC = "document";
        /// <summary>
        /// The common URI prefix for notification services.
        /// </summary>
        private static readonly string NOTIFICATION = "notification";

        public static readonly string DEFAULT_SERVICE_DOMAIN = "baidubce.com";

        // should from base
        /// <summary>
        /// The common URL prefix for all BCE service APIs.
        /// </summary>
        public static readonly string URL_PREFIX = "v1";
        /// <summary>
        /// The default string encoding for all BCE service APIs.
        /// </summary>
        public static readonly string DEFAULT_ENCODING = "UTF-8";
        /// <summary>
        /// The default http request content type for all BCE service APIs.
        /// </summary>
        public static readonly string DEFAULT_CONTENT_TYPE = "application/json; charset=utf-8";
        public static readonly string AUTHORIZATION = "Authorization";


        public static readonly string CONTENT_LENGTH = "Content-Length";

        public static readonly string CONTENT_MD5 = "Content-MD5";

        public static readonly string CONTENT_TYPE = "Content-Type";

        public static readonly string DATE = "Date";

        public static readonly string ETAG = "ETag";

        public static readonly string EXPIRES = "Expires";

        public static readonly string HOST = "Host";

        public static readonly string LAST_MODIFIED = "Last-Modified";

        public static readonly string LOCATION = "Location";

        public static readonly string RANGE = "Range";

        public static readonly string SERVER = "Server";

        public static readonly string TRANSFER_ENCODING = "Transfer-Encoding";

        public static readonly string USER_AGENT = "User-Agent";
        public static readonly string BCE_PREFIX = "x-bce-";

        private readonly string _ak;
        private readonly string _sk;
        private readonly string _region;
        private readonly string _service;

        protected readonly HashSet<string> DefaultHeadersToSign = new HashSet<string>();

        protected BCESignerBase(string ak, string sk)
        {
            if (string.IsNullOrWhiteSpace(ak))
            {
                throw new ArgumentNullException(nameof(ak));
            }
            if (string.IsNullOrWhiteSpace(sk))
            {
                throw new ArgumentNullException(nameof(sk));
            }

            _ak = ak;
            _sk = sk;

            DefaultHeadersToSign.Add(HOST.ToLower());
            DefaultHeadersToSign.Add(CONTENT_LENGTH.ToLower());
            DefaultHeadersToSign.Add(CONTENT_TYPE.ToLower());
            DefaultHeadersToSign.Add(CONTENT_MD5.ToLower());
        }

        protected BCESignerBase(string ak, string sk, string region, string service)
        {
            if (string.IsNullOrWhiteSpace(ak))
            {
                throw new ArgumentNullException(nameof(ak));
            }
            if (string.IsNullOrWhiteSpace(sk))
            {
                throw new ArgumentNullException(nameof(sk));
            }
            if (string.IsNullOrWhiteSpace(region))
            {
                throw new ArgumentNullException(nameof(region));
            }
            if (string.IsNullOrWhiteSpace(service))
            {
                throw new ArgumentNullException(nameof(service));
            }

            _ak = ak;
            _sk = sk;
            _region = region;
            _service = service;

            DefaultHeadersToSign.Add(HOST.ToLower());
            DefaultHeadersToSign.Add(CONTENT_LENGTH.ToLower());
            DefaultHeadersToSign.Add(CONTENT_TYPE.ToLower());
            DefaultHeadersToSign.Add(CONTENT_MD5.ToLower());
        }

        public Uri ApiUrl { get; private set; }
        public string CanonicalURI => GetCanonicalURIPath(this.ApiUrl.AbsolutePath);
        public string CanonicalQueryString
        {
            get
            {
                var paras = this.ApiUrl.Query.Substring(1).Split('&');
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
        public string CanonicalHeaders { get; set; }
        public string CanonicalRequestString { get; private set; }
        public string TimeStamp { get; set; }
        public string SigningKey { get; private set; }
        public string SignedHeaders { get; set; }
        public string Signature { get; set; }
        public string AuthString { get; private set; }

        public string AuthStringPrefix
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.TimeStamp))
                {
                    return string.Empty;
                }

                return $"bce-auth-v2/{this._ak}/{this.TimeStamp}/{this._region}/{this._service}";
            }
        }

        public string CreateSigningKey(string timeStamp = null)
        {
            if (string.IsNullOrWhiteSpace(timeStamp))
            {
                this.TimeStamp = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");
            }
            else
            {
                this.TimeStamp = timeStamp;
            }

            this.SigningKey = Hex(new HMACSHA256(Encoding.UTF8.GetBytes(this._ak)).ComputeHash(Encoding.UTF8.GetBytes(this.AuthStringPrefix)));

            return this.SigningKey;
        }
        protected string UriEncode(string input, bool encodeSlash = false)
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte b in Encoding.UTF8.GetBytes(input))
            {
                if ((b >= 'a' && b <= 'z') || (b >= 'A' && b <= 'Z') || (b >= '0' && b <= '9') || b == '_' || b == '-' || b == '~' || b == '.')
                {
                    builder.Append((char)b);
                }
                else if (b == '/')
                {
                    if (encodeSlash)
                    {
                        builder.Append("%2F");
                    }
                    else
                    {
                        builder.Append((char)b);
                    }
                }
                else
                {
                    builder.Append('%').Append(b.ToString("X2"));
                }
            }
            return builder.ToString();
        }
        protected string Hex(byte[] data)
        {
            var sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        protected static string GetCanonicalQueryString(IDictionary<string, string> parameters, bool forSignature)
        {
            if (parameters.Count == 0)
                return "";
            List<string> stringList = new List<string>();
            foreach (KeyValuePair<string, string> parameter in (IEnumerable<KeyValuePair<string, string>>)parameters)
            {
                string key = parameter.Key;
                if (!forSignature || !"Authorization".Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    if (key == null)
                        throw new ArgumentNullException("parameter key should NOT be null");
                    string str = parameter.Value;
                    if (str == null)
                    {
                        if (forSignature)
                            stringList.Add(Normalize(key) + "=");
                        else
                            stringList.Add(Normalize(key));
                    }
                    else
                        stringList.Add(Normalize(key) + "=" + Normalize(str));
                }
            }
            stringList.Sort();
            return string.Join("&", stringList.ToArray());
        }
        public static string NormalizePath(string path)
        {
            return Normalize(path).Replace("%2F", "/");
        }
        public static string Normalize(string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in Encoding.UTF8.GetBytes(value))
                stringBuilder.Append(PercentEncodedStrings[(int)num & (int)byte.MaxValue]);
            return stringBuilder.ToString();
        }
        private string GetCanonicalURIPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "/";
            }

            if (path.StartsWith("/"))
            {
                return NormalizePath(path);
            }

            return "/" + NormalizePath(path);
        }
        //private string GetCanonicalHeaders(SortedDictionary<string, string> headers)
        //{
        //    if (headers.Count == 0)
        //    {
        //        return "";
        //    }

        //    var headerStrings = new HashSet<string>();
        //    foreach (KeyValuePair<string, string> header in headers)
        //    {
        //        var key = header.Key.Trim().ToLower();
        //        var value = header.Value.Trim();
        //        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
        //        {
        //            continue;
        //        }
        //        headerStrings.Add(Normalize(key) + ':' + Normalize(value));
        //    }

        //    return string.Join('\n', headerStrings.OrderBy(r => r));
        //}
        //private SortedDictionary<string, string> GetHeadersToSign(Dictionary<string, string> headers, List<string> headersToSign)
        //{
        //    SortedDictionary<string, string> ret = new SortedDictionary<string, string>();
        //    if (headersToSign != null)
        //    {
        //        var tempSet = new HashSet<string>();
        //        foreach (var header in headersToSign)
        //        {
        //            tempSet.Add(header.Trim().ToLower());
        //        }
        //        headersToSign = new List<string>(tempSet);
        //    }

        //    foreach (KeyValuePair<string, string> item in headers)
        //    {
        //        if (!string.IsNullOrWhiteSpace(item.Key))
        //        {
        //            if ((headersToSign == null && this.IsDefaultHeaderToSign(item.Key))
        //                || (headersToSign != null && headersToSign.Contains(item.Key.ToLower())
        //                                          && !Headers.AUTHORIZATION.Equals(item.Key, StringComparison.OrdinalIgnoreCase)))
        //            {
        //                ret.Add(item.Key, item.Value);
        //            }
        //        }
        //    }

        //    return ret;
        //}

        private bool IsDefaultHeaderToSign(string header)
        {
            header = header.Trim().ToLower();
            return header.StartsWith(BCE_PREFIX) || DefaultHeadersToSign.Contains(header);
        }

    }
}
