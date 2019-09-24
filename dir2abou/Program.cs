using AbouApi;
using AbouApi.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;


namespace dir2abou
{
    public class Program
    {


        private const string ApiUrl = "https://testservice.lund.se/api/v2/services";
        private const string ApiUserName = "UIPath";
        private const string ApiKey = "748f42c7-6b6d-40a2-9691-3925ada3ebb6";
        private const string ServiceShortName = "BSF_10";
        private const string path2dir = @"\\intra.lund.se\GroupData\308608\TestArendeAttLaddaUppTest";

        //private const string ApiUrl = "https://service.lund.se/api/v2/services";
        //private const string ApiUserName = "RPA01";
        //private const string ApiKey = "b73852d9-1f88-43ec-a057-673906035c65";
        //private const string ServiceShortName = "BSF_10";
        //private const string path2dir = @"\\intra.lund.se\GroupData\308608\Till beslut\PDF_1";

        private const string actor = "BSF";

        private const string DiaryNumber = "";

        
        public Program(string configName, string dir, string operation, string status  = "") 
        {
            Config config = GetConfig(configName);
            string[] fileEntries = Directory.GetFiles(dir);

            var api = new AbouRestApi(config);
            foreach (var fileName in fileEntries)
            {
                
                string uniqueId = GetUniqueIdFromGileName(fileName);
                switch (operation)
                {
                    case "upload" :

                        api.FileUpload(uniqueId, fileName, status + Path.GetExtension(fileName));
                        break;
                    case "setstatus":
                        api.UpdateStatus(uniqueId,  status );
                        break;
                    default:
                        Console.WriteLine($"Operation {operation} not supported");
                        break;
                }
            }

        }

        private static string GetUniqueIdFromGileName(string fileName)
        {
            var length = fileName.Length;
            int caseNameLength = "190124-BSF_10-HD02.pdf".Length;
            var CaseUniqueId = fileName.Substring(length - caseNameLength, caseNameLength - 4);
            return CaseUniqueId;
        }

        private static Config GetConfig(string configName)
        {
            var file = File.ReadAllText(".\\config.json");
            var configs = Helper.FromJson<Configs>(file);
            var config = configs.Single(s => s.Name == configName);
            return config;
        }

        /// <summary>
        /// stage ./ExampleFiles upload DittBeslut
        /// stage ./ExampleFiles setstatus beslut
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length <2)
            {
                Console.WriteLine("argument");
                Console.WriteLine("configname dir operation (status)");
                return ;
            }
            string configName = args[0];
            string dir = args[1];
            string operation = args[2];
            string status = (args.Length > 3) ? args[3] : string.Empty;
            new Program(configName, dir, operation, status);

            return;



            string CaseUniqueId; ;
            string commentFileUpload = "Bifogat beslut";
            string commentStatus = "Status uppdaterad";
            string commentDetailed = "Detailed hämtad";
            
            int caseNameLength = "190124-BSF_10-HD02.pdf".Length;

            int counter = 1;


            string[] fileEntries = Directory.GetFiles(path2dir);
            foreach (string fileName in fileEntries)
            {
                var length = fileName.Length;
                CaseUniqueId = fileName.Substring(length - caseNameLength, caseNameLength - 4);

                Console.WriteLine($"{counter} {CaseUniqueId}");
                counter++;

                // Uncommet one row to execute.......

                // UploadPdf(fileName, CaseUniqueId, commentFileUpload, actor);
                // SetStatus(CaseUniqueId, commentStatus, actor, "Beslut");
                // System.Threading.Thread.Sleep(500);
                // SetStatus(CaseUniqueId, commentStatus, actor, "Inkommet");
                // GetDetailed(CaseUniqueId, commentDetailed, actor, true);
            }



            Console.WriteLine("Ready. Press anykey to exit!");
            Console.ReadKey();
        }



        private static void GetDetailed(string CaseUniqueId, string comment, string actor, bool writeToFile)
        {
            using (var client = new HttpClient())
            {
                string data = string.Format("{{\"UniqueId\":\"{0}\",\"DiaryNumber\":\"{1}\",\"Comment\":\"{2}\",\"Actor\":\"{3}\",\"XmlGeneratorType\":\"{4}\",\"IncludeAllFields\":\"{5}\",\"IncludeAllAttachements\":\"{6}\",\"OmitXmlDeclaration\":\"{7}\"}}", CaseUniqueId, "", comment, actor, "Abou.Calamare.Framework.Integration.Xml.Default.DefaultXmlCaseGenerator", true, true, true);

                var stringContent = new StringContent(data);
                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
               
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var request = client.PostAsync(string.Format("{0}/{1}/cases/GetDetailed?username={2}&apikey={3}", ApiUrl, ServiceShortName, ApiUserName, ApiKey), stringContent);
                var httpResponse = request.Result;
                var statusCode = httpResponse.StatusCode;
                if (httpResponse.Content == null) return;
                var stringContentsTask = httpResponse.Content.ReadAsStringAsync();

                var stringContents = stringContentsTask.Result;
                if (!writeToFile) return;
                File.WriteAllText(string.Format(@"C:\Temp\{0}.json", CaseUniqueId), stringContents);

                DetailedResult detailedResult = Helper.FromJson<DetailedResult>(stringContents);

                var antal = detailedResult.Attachments.Count;

                string path = $"\\\\intra.lund.se\\GroupData\\309150\\abou2w3d3\\{detailedResult.Id}";
                Directory.CreateDirectory(path);
                Directory.CreateDirectory($"{path}\\ansokan");
                Directory.CreateDirectory($"{path}\\bilagor");
                Directory.CreateDirectory($"{path}\\beslut");

                foreach (var attachment in detailedResult.Attachments)
                {
                    if (attachment.SystemFileName.Length > 0)
                    {
                        CaseAttachmentDownload(CaseUniqueId, attachment.SystemFileName, path, true);
                    }

                }
                CasePdfDownload(CaseUniqueId,  path, true);
            }
        }

        private static void CasePdfDownload(string CaseUniqueId, string path, bool writeToFile)
        {

            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(string.Format("{{\"UniqueId\":\"{0}\",\"DiaryNumber\":\"{1}\",\"Comment\":\"{2}\",\"Actor\":\"{3}\"}}",
                    CaseUniqueId, "", "Download Case pdf", actor));
                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var request = client.PostAsync(string.Format("{0}/{1}/cases/CasePdfDownload?username={2}&apikey={3}", ApiUrl, ServiceShortName, ApiUserName, ApiKey), stringContent);
                var httpResponse = request.Result;
                var statusCode = httpResponse.StatusCode;
                if (httpResponse.Content == null) return;
                var stringContentsTask = httpResponse.Content.ReadAsByteArrayAsync();
                //  var stringContents = ;
                if (!writeToFile || statusCode != HttpStatusCode.OK) return;
                string fileWithPath = $"{path}\\ansokan\\Ansokan.pdf";
                File.WriteAllBytes(fileWithPath, stringContentsTask.Result);
            }
        }
        private static void CaseAttachmentDownload(string CaseUniqueId, string systemFilename, string path, bool writeToFile)
        {
            
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(string.Format("{{\"UniqueId\":\"{0}\",\"DiaryNumber\":\"{1}\",\"Comment\":\"{2}\",\"Actor\":\"{3}\",\"SystemFileName\":\"{4}\"}}",
                CaseUniqueId, "", "Attachments download", actor, systemFilename));
                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var request = client.PostAsync(string.Format("{0}/{1}/cases/AttachmentDownload?username={2}&apikey={3}", ApiUrl, ServiceShortName, ApiUserName, ApiKey), stringContent);
                var httpResponse = request.Result;
                var statusCode = httpResponse.StatusCode;
                if (httpResponse.Content == null) return;
                var stringContentsTask = httpResponse.Content.ReadAsByteArrayAsync();
              //  var stringContents = ;
                if (!writeToFile || statusCode != HttpStatusCode.OK) return;


                string fileWithPath;
                if (systemFilename.Contains("_Beslut"))
                {
                    fileWithPath = $"{path}\\beslut\\{systemFilename}";
                } else
                {
                    fileWithPath = $"{path}\\bilagor\\{systemFilename}";
                }
                File.WriteAllBytes(fileWithPath, stringContentsTask.Result);
            }
        }
        private static void SetStatus(string CaseUniqueId, string comment, string actor, string status, bool disableSendNotification = false)
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(
                    string.Format("{{\"UniqueId\":\"{0}\",\"DiaryNumber\":\"{1}\",\"Comment\":\"{2}\",\"Actor\":\"{3}\",\"State\":\"{4}\",\"DisableSendNotification\":\"{5}\"}}", 
                    CaseUniqueId, DiaryNumber, comment, actor, status, disableSendNotification));

                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var request = client.PutAsync(string.Format("{0}/{1}/cases/UpdateStatus?username={2}&apikey={3}", ApiUrl, ServiceShortName, ApiUserName, ApiKey), stringContent);
                var httpResponse = request.Result;
                var statusCode = httpResponse.StatusCode;
                if (httpResponse.Content == null) return;
                var stringContentsTask = httpResponse.Content.ReadAsStringAsync();
                var stringContents = stringContentsTask.Result;

                Console.WriteLine($"{statusCode}: {stringContents}");
            }
        }

        private static void UploadPdf(string file, string CaseUniqueId, string comment, string actor)
        {
            var message = new HttpRequestMessage();
            var content = new MultipartFormDataContent();
            var stringContent = new
            StringContent(string.Format("{{\"UniqueId\":\"{0}\",\"DiaryNumber\":\"{1}\",\"Comment\":\"{2}\",\"Actor\":\"{3}\"}}",
            CaseUniqueId, DiaryNumber, comment, actor));
            stringContent.Headers.Add("Content-Disposition", "form-data;name =\"CaseBaseRequest\"");
            content.Add(stringContent);
            using (var fileStream = new FileStream(file, FileMode.Open))
            {

                var fileName = "Beslut om skolplacering.pdf"; // Path.GetFileName(file);
                var streamContent = new StreamContent(fileStream);
                streamContent.Headers.Add("Content-Type", "application/octet-stream");
                streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\";filename =\"" + fileName + "\"");
                content.Add(streamContent, "file", fileName);

                message.Method = HttpMethod.Post;
                message.Content = content;

                var url = string.Format("{0}/{1}/cases/FileUpload?username={2}&apikey={3}", ApiUrl,ServiceShortName, ApiUserName, ApiKey);
                message.RequestUri = new  Uri(url);
                using (var client = new HttpClient())
                {
                    var request = client.SendAsync(message, HttpCompletionOption.ResponseContentRead,
                    CancellationToken.None);
                    var httpResponse = request.Result;
                    var statusCode = httpResponse.StatusCode;
                    if (httpResponse.Content == null) return;
                    var stringContentsTask = httpResponse.Content.ReadAsStringAsync();
                    var stringContents = stringContentsTask.Result;
                    Console.WriteLine($"{statusCode}: {httpResponse}");
                }

            }
        }
    }
}
