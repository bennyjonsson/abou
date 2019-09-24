using System;
using System.Collections.Generic;
using System.Text;
using AbouApi;

namespace AbouApiTest
{
    public class Global
    {
        public static Config GetConfig
        {
            get
            {
                return new Config()
                {
                    Name = "xUnitTest",
                    ApiUrl = "https://testservice.MINKOMMUN.se/api/v2/services",
                    ApiUserName = "UIPath",
                    ApiKey = "c0ff3c0f-fec0-ff3c-0ffe-c0ff3c0ff3c0f",
                    ServiceShortName = "abc_01",
                    Actor = "XUnitTest"
                };

         


            }
        }

        public static AbouRestApi ApiFactory()
        {
            var config = Global.GetConfig;
            var api = new AbouRestApi(config);
            return api;
        }
    }
}
