using System;
using Xunit;
using AbouApi;
using AbouApi.Entities;

namespace AbouApiTest
{
    public partial class UpdateAdministratorTests
    {

        [Fact]
        public void UpdateAdministratorData()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            var data = new DataUpdateAdministrator(config)
            {
                UniqueId = "190227-BSF_10-GC95",
                Administrator = "Mr " + Guid.NewGuid().ToString()
            };
            api.UpdateAdministrator(data);

        }

        [Fact]
        public void UpdateAdministrator()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            api.UpdateAdministrator("190227-BSF_10-GC95", "Mrs " + Guid.NewGuid().ToString(), true);

        }
    }
}