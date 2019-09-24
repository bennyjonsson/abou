using Xunit;
using AbouApi;

namespace AbouApiTest
{
    public partial class AttachmentDownloadTests
    {
        [Fact]
        public void AttachmentDownload()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            var bytes = api.AttachmentDownload("190227-BSF_10-GC95", "190227-BSF_10-GC95_Sokigo_logo.png");
        }

        [Fact]
        public void AttachmentDownload2File()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            api.AttachmentDownload2File("190227-BSF_10-GC95", "190227-BSF_10-GC95_Sokigo_logo.png", ".\\");
        }
    }
}