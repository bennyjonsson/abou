using Xunit;
using AbouApi;
using AbouApi.Entities;
using System.Net.Http;

namespace AbouApiTest
{
    public partial class UpdateStatusTests
    {


        [Fact]
        public void UpdateStatusData()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);
            var data = new DataUpdateStatus(config)
            {
                UniqueId = "190227-BSF_10-GC95",
                State = "Avslag"
            };
            api.UpdateStatus(data);

        }

        [Fact]
        public void UpdateStatus()
        {

            var config = Global.GetConfig;
            var api = new AbouRestApi(config);
            string uniqueId = "190227-BSF_10-GC95";
            api.UpdateStatus(uniqueId, "Cykelpump", true);

        }
    }
}