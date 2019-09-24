using System;
using Xunit;
using AbouApi;
using AbouApi.Entities;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AbouApiTest
{
    public partial class AddCitizenTests
    {


        [Fact]
        public void AddCitizen()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            CitizensData data = new CitizensData(config)
            {
                Citizens = new System.Collections.Generic.List<Citizen>()
                {
                    new Citizen() { Email = "benny.jonsson@lund.se", FirstName = "Benny", LastName = "Jonsson", UserIdentity = "121212121212" }
                },
                UniqueId = "190227-BSF_10-GC95",
                OmitSocialSecUrityNumberValidation = true,
                UpdateExistingCitizen = true
            };

            api.AddCitizens(data);


        }



        [Fact]
        public void ConfigsSerialize()
        {
            var c = new Config() { Actor = "a", ApiKey = "ap", ApiUserName = "Un", ApiUrl = "apliurl", Name = "test1", ServiceShortName = "acn" };

            List<Config> configs = new List<Config>();
            configs.Add(c);
            configs.Add(c);

            var s = JsonConvert.SerializeObject(configs, AbouApi.Entities.Converter.Settings);
        }

        [Fact]
        public void GetByDateAndStateDate()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);

            GetCaseData data = new GetCaseData(config)
            {
                FromDate = new DateTime(2019, 02, 27),
                ToDate = new DateTime(2019, 12, 11),
                States = new string[] { "Beslut" },
                ExcludeCasesWithDiaryNumber = false

            };

            var res = api.GetByDateAndState(data);

        }

        [Fact]
        public void GetByDateAndStat()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);


            DateTime FromDate = new DateTime(2019, 02, 27);
            DateTime ToDate = new DateTime(2019, 12, 11);
            string[] States = new string[] { "Beslut" };
            bool ExcludeCasesWithDiaryNumber = false;


            var res = api.GetByDateAndState(FromDate, ToDate, States, ExcludeCasesWithDiaryNumber);

        }
    }
}