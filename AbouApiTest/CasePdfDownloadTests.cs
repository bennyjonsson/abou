using Xunit;
using AbouApi;
using AbouApi.Entities;

namespace AbouApiTest
{
    public partial class CasePdfDownloadTests
    {

        [Fact]
        public void CasePdfDownload()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            var bytes = api.CasePdfDownload("190227-BSF_10-GC95");


        }

        //[Fact]
        // Ignore...
        public void CasePdfDowdsdsdnload()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            CreateCaseData createCase = new CreateCaseData()
            {
                Citizen = new CitizenBase() { Email = "benny.jonsson@lund.se", FirstName = "Benny", LastName = "Jonsson", UserIdentity = "121212121212" }
            };
            api.CreateCase(createCase);

        }
    }
}