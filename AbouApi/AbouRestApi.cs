using AbouApi.Entities;
using AbouApi.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace AbouApi
{
    public class AbouRestApi : IAbouRestApi
    {
        private readonly Config config;

        public AbouRestApi(Config config)
        {
            this.config = config;
        }

        private string RequestUrl(string method)
        {
            return $"{config.ApiUrl}/{config.ServiceShortName}/cases/{method}?username={config.ApiUserName}&apikey={config.ApiKey}";
        }

        public virtual void UpdateStatus(string uniqueId, string state, bool disableSendNotification = false)
        {
            UpdateStatus(new DataUpdateStatus(config) { UniqueId = uniqueId, DisableSendNotification = disableSendNotification, State = state });
        }

        private void Put<T>(T update, string endpoint) where T : IToData
        {
            string content = update.ToJson();

            var stringContent = new StringContent(content);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            using (var client = new HttpClient())
            {
                string requestUrl = RequestUrl(endpoint);
                var request = client.PutAsync(requestUrl, stringContent);
                var httpResponse = request.Result;
                System.Net.HttpStatusCode statusCode = httpResponse.StatusCode;
                if (httpResponse.Content == null)
                {
                    // ToDo: Är detta rätt
                    return;
                }
                var stringContentsTask = httpResponse.Content.ReadAsStringAsync();
                var stringContents = stringContentsTask.Result;
                if (statusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new AbouApiException(statusCode, stringContents);
                }
            }

        }
        public virtual void UpdateStatus(DataUpdateStatus status)
        {
            Put<DataUpdateStatus>(status, "UpdateStatus");
        }
        public virtual void UpdateDiaryNumber(DataDiaryNumber diaryNumber)
        {
            Put<DataDiaryNumber>(diaryNumber, "UpdateDiaryNumber");
        }

        public virtual void UpdateDiaryNumber(string uniqueId, string diaryNumber, bool disableSendNotification = false)
        {
            try
            {
                UpdateDiaryNumber(new DataDiaryNumber(config) { DiaryNumber = diaryNumber, UniqueId = uniqueId, DisableSendNotification = disableSendNotification });
            }
            catch (AbouApiException exception)
            {
                if (exception.HttpStatusCode == System.Net.HttpStatusCode.NotAcceptable && exception.Content.Contains(" alreade used on case "))
                {
                    return;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }



        public virtual DetailedResult GetDetailed(string uniqueId)
        {
            return GetDetailed(new DataGetDetailed(config) { UniqueId = uniqueId });
        }
        public virtual DetailedResult GetDetailed(DataGetDetailed dataGetDetailed)
        {

            var byteArray = PostAsync<DataGetDetailed>(dataGetDetailed, "GetDetailed");

            var stringContents = Encoding.UTF8.GetString(byteArray);

            return Helper.FromJson<DetailedResult>(stringContents);


        }

        public virtual void UpdateAdministrator(DataUpdateAdministrator administrator)
        {
            Put<DataUpdateAdministrator>(administrator, "UpdateAdministrator");
        }

        public virtual void UpdateAdministrator(string uniqueId, string administrator, bool disableSendNotification = false)
        {
            UpdateAdministrator(new DataUpdateAdministrator(config) { UniqueId = uniqueId, Administrator = administrator, DisableSendNotification = disableSendNotification });
        }

        public void FileUpload(FileUploadData data, string filePathName, string fileName)
        {
            using (var fileStream = new FileStream(filePathName, FileMode.Open))
            {
                FileUpload(data, fileStream, fileName);
            }
        }

        public void FileUpload(FileUploadData data, FileStream fileStream, string fileName)
        {

            var message = new HttpRequestMessage();
            var content = new MultipartFormDataContent();
            var stringContent = new StringContent(data.ToJson());

            stringContent.Headers.Add("Content-Disposition", "form-data;name =\"CaseBaseRequest\"");
            content.Add(stringContent);



            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.Add("Content-Type", "application/octet-stream");
            streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\";filename =\"" + fileName + "\"");
            content.Add(streamContent, "file", fileName);

            message.Method = HttpMethod.Post;
            message.Content = content;

            string requestUrl = RequestUrl("FileUpload");
            message.RequestUri = new Uri(requestUrl);
            using (var client = new HttpClient())
            {
                var request = client.SendAsync(message, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
                var httpResponse = request.Result;
                var statusCode = httpResponse.StatusCode;
                if (httpResponse.Content == null)
                {
                    // ToDo är detta rätt?!
                    return;
                }
                var stringContentsTask = httpResponse.Content.ReadAsStringAsync();
                var stringContents = stringContentsTask.Result;
                if (statusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new AbouApiException(statusCode, stringContents);
                }
            }
        }

        public void FileUpload(string uniqueId, string filePathName, string fileName)
        {
            FileUpload(new FileUploadData(config) { UniqueId = uniqueId }, filePathName, fileName);
        }

        public void FileUpload(string uniqueId, FileStream fileStream, string fileName)
        {
            FileUpload(new FileUploadData(config) { UniqueId = uniqueId }, fileStream, fileName);
        }

        public byte[] AttachmentDownload(string uniqueId, string systemFileName)
        {
            return AttachmentDownload(new AttachmentDownloadData(config) { UniqueId = uniqueId, SystemFileName = systemFileName });
        }


        private byte[] PostAsync<T>(T data, string method) where T : IToData
        {
            using (var client = new HttpClient())
            {
                var stringContent = new StringContent(data.ToJson());
                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var request = client.PostAsync(RequestUrl(method), stringContent);
                var httpResponse = request.Result;
                var statusCode = httpResponse.StatusCode;

                var stringContentsTask = httpResponse.Content.ReadAsByteArrayAsync();

                return stringContentsTask.Result;
            }
        }
        public byte[] AttachmentDownload(AttachmentDownloadData data)
        {
            return PostAsync<AttachmentDownloadData>(data, "AttachmentDownload");
        }

        public string AttachmentDownload2File(AttachmentDownloadData data, string downloadFilePath)
        {
            byte[] bytes = AttachmentDownload(data);
            string pathAndFileName = downloadFilePath + data.SystemFileName;
            File.WriteAllBytes(pathAndFileName, bytes);

            return pathAndFileName;
        }

        public string AttachmentDownload2File(string uniqueId, string systemFileName, string downloadFilePath)
        {
            return AttachmentDownload2File(new AttachmentDownloadData(config) { UniqueId = uniqueId, SystemFileName = systemFileName }, downloadFilePath);
        }



        public byte[] CasePdfDownload(string uniqueId)
        {
            return CasePdfDownload(new CasePdfDownloadData(config) { UniqueId = uniqueId });
        }



        public byte[] CasePdfDownload(CasePdfDownloadData data)
        {

            return PostAsync<CasePdfDownloadData>(data, "CasePdfDownload");

        }

        public string AttachmentDownload2File(string uniqueId, string downloadFilePath)
        {

            throw new NotImplementedException();
        }

        public string CreateCase(CreateCaseData createCase)
        {
            // ToDo: Implement
            byte[] bytes = PostAsync<CreateCaseData>(createCase, "CreateCase");
            return Encoding.UTF8.GetString(bytes);
        }

        public void AddCitizens(CitizensData data)
        {
            var byteArray = PostAsync<CitizensData>(data, "AddCitizens");
        }

        public string[] GetByDateAndState(DateTime fromDate, DateTime toDate, string[] states, bool excludeCasesWithDiaryNumber)
        {

            GetCaseData data = new GetCaseData(config)
            {
                FromDate = fromDate,
                ToDate =toDate,
                States = states,
                ExcludeCasesWithDiaryNumber = excludeCasesWithDiaryNumber

            };

            return GetByDateAndState(data);

        }

        public string[] GetByDateAndState(GetCaseData data)
        {
            var bytes =  PostAsync<GetCaseData>(data, "GetByDateAndState");

            var ids = JsonConvert.DeserializeObject<IEnumerable<string>>(Encoding.UTF8.GetString(bytes));
            var enumerable = ids as string[] ?? ids.ToArray();
            return enumerable;
        }
    }
}
