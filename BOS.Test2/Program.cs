using System;
using BceDotNetSdk;
using BceDotNetSdk.Domain.Doc;
using BceDotNetSdk.Request.Doc;
using Newtonsoft.Json;

namespace BOS.Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            var ak = "";
            var sk = "";


            var bosClient = new DefaultBosRestClient(ak, sk, "http://doc.bj.baidubce.com");

            // Register Doc
            //var model = new RegisterDocumentModel { Title = "测试文档服务.doc", Format = "doc" };
            //var request = new RegisterDocumentRequest(model);

            // Create Doc from bos
            //var model = new CreateDocumentModel()
            //{
            //    Title = "教育部关于发布《网络学习空间建设与应用指南》的通知",
            //    Format = "pdf",
            //    Bucket = "test-bucket2",
            //    Object = "教育部关于发布《网络学习空间建设与应用指南》的通知.pdf"
            //};
            //var request = new CreateDocumentRequest(model);
            //doc-jhdm7ksjug28qhu

            // Publish Doc
            //var model = new PublishDocumentModel()
            //{
            //    DocumentId = "doc-jhdm7ksjug28qhu"
            //};
            //var request = new PublishDocumentRequest(model);

            // Get Doc
            //var model = new GetDocumentModel()
            //{
            //    DocumentId = "doc-jhdm7ksjug28qhu"
            //};
            //var request = new GetDocumentRequest(model);

            // Read Doc
            var model = new ReadDocumentModel()
            {
                DocumentId = "doc-jhdm7ksjug28qhu"
            };
            var request = new ReadDocumentRequest(model);

            var res = bosClient.Execute(request);

            Console.WriteLine($"Api result=> \r\n{JsonConvert.SerializeObject(res, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore })}");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
