using Xunit;
using AbouApi;
using AbouApi.Entities;

namespace AbouApiTest
{
    public partial class FileUploadTests
    {

        [Fact]
        public void FileUploadPdf()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            api.FileUpload(new FileUploadData(config) { UniqueId = "190227-BSF_10-GC95" }, ".\\Testdata\\AbouTestDoc.pdf", "Namn som syns.pdf");
        }

        [Fact]
        public void FileUploadWord()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            api.FileUpload(new FileUploadData(config) { UniqueId = "190227-BSF_10-GC95" }, ".\\Testdata\\AbouTestDocWord.docx", "Namn som syns.docx");
        }

        [Fact]
        public void FileUploadPngf()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            api.FileUpload(new FileUploadData(config) { UniqueId = "190227-BSF_10-GC95" }, ".\\Testdata\\Sokigo_logo.png", "Sokigo_logo.png");
        }

        [Fact]
        public void FileUploadEpub()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            Assert.Throws<AbouApiException>(() => api.FileUpload(new FileUploadData(config) { UniqueId = "190227-BSF_10-GC95" }, ".\\Testdata\\AbouTestDocEpub.epub", "Funkar inte.epub"));
        }
    }
}