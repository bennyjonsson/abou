using AbouApi.Entities;
using System;
using System.IO;

namespace AbouApi.Interfaces
{
    public interface IAbouRestApi
    {
        DetailedResult GetDetailed(DataGetDetailed dataGetDetailed);
        DetailedResult GetDetailed(string uniqueId);
        void UpdateDiaryNumber(DataDiaryNumber diaryNumber);
        void UpdateDiaryNumber(string uniqueId, string diaryNumber, bool disableSendNotification = false);
        void UpdateStatus(DataUpdateStatus status);
        void UpdateStatus(string uniqueId, string state, bool disableSendNotification = false);

        void UpdateAdministrator(DataUpdateAdministrator administrator);
        void UpdateAdministrator(string uniqueId, string administrator, bool disableSendNotification = false);

        // void AddCitizensData(DataUpdateAdministrator administrator);
        // void AddCitizens(string uniqueId, string administrator, bool disableSendNotification = false);

        void FileUpload(FileUploadData data, string filePathName, string fileName);
        void FileUpload(string uniqueId, string filePathName, string fileName);
        void FileUpload(FileUploadData data, FileStream fileStream, string fileName);
        void FileUpload(string uniqueId, FileStream fileStream, string fileName);

        
        byte[] AttachmentDownload(AttachmentDownloadData data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="downloadFilePath"></param>
        /// <returns>Downloaded file name</returns>
        string AttachmentDownload2File(AttachmentDownloadData data, string downloadFilePath);
        string AttachmentDownload2File(string uniqueId, string systemFileName, string downloadFilePath);


        byte[] CasePdfDownload(string uniqueId);

        byte[] CasePdfDownload(CasePdfDownloadData data);

        // ToDo
        string CreateCase(CreateCaseData data);

        void AddCitizens(CitizensData data);

        string[] GetByDateAndState(DateTime fromDate, DateTime toDate, string[] states, bool excludeCasesWithDiaryNumber);
        string[] GetByDateAndState(GetCaseData data);
    }
}