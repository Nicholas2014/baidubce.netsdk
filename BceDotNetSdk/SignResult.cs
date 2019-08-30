using System;

namespace BceDotNetSdk
{
    public class SignResult
    {
        public SignResult()
        {
        }

        public string Authorization { get; set; }
        public DateTime CreateTime { get; set; }
        public string SignDate { get; set; }
    }
}