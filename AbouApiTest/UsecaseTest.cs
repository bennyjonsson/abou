using System;
using Xunit;
using AbouApi;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace AbouApiTest
{

    public class UsecaseTest
    {
        [Fact]
        public void GetAllUsersFromSBK_02()
        {
            var config = new Config()
            {
                Name = "internsbk",
                ApiUrl = "https://testservice.MINKOMMUN.se/api/v2/services",
                ApiUserName = "SBK_RPA",
                ApiKey = "c0ff3c0f-fec0-ff3c-0ffe-c0ff3c0ff3c0f",
                ServiceShortName = "SBK_02",
                Actor = "SBK"
            };

            var api = new AbouRestApi(config);


            DateTime FromDate = new DateTime(2016, 02, 27);
            DateTime ToDate = new DateTime(2019, 12, 11);
            string[] States = new string[] { "Godkänd", "Inkommet" };
            bool ExcludeCasesWithDiaryNumber = false;

            var users = new ConcurrentBag<string>();

            var res = api.GetByDateAndState(FromDate, ToDate, States, ExcludeCasesWithDiaryNumber);

            Parallel.ForEach(res, (u) =>
            {
                var r = api.GetDetailed(u);
                var user = r.Signatures.First().UserIdentity;
                users.Add(user);
            });

            var f = string.Join(",", users.Distinct());
        }
    }

}