namespace BceDotNetSdk.Response
{
    public class RegisterDocumentResponse : BosResponse
    {
        public string DocumentId { get; set; }
        public string Bucket { get; set; }
        public string Object { get; set; }
        public string BosEndpoint { get; set; }
    }
}
