using System;
using Xunit;
using AbouApi;
using AbouApi.Entities;

namespace AbouApiTest
{
    public class GetDetailedTests
    {



        [Fact]
        public void GetDetailed_GivenDataGetDetailedUniqueIdReturnsResultWithSameId()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);
            var data = new DataGetDetailed(config)
            {
                UniqueId = "190227-BSF_10-GC95"
            };
            DetailedResult detailedResult = api.GetDetailed(data);

            Assert.Equal(data.UniqueId, detailedResult.Id);
        }

        [Fact]
        public void GetDetailed_GivenUniqueIdReturnsResultWithSameId()
        {

            var api = Global.ApiFactory();
            string uniqueId = "190227-BSF_10-GC95";
            DetailedResult detailedResult = api.GetDetailed(uniqueId);

            Assert.Equal(uniqueId, detailedResult.Id);
        }
    }
}
