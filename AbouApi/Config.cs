using AbouApi.Interfaces;
using System.Collections.Generic;

namespace AbouApi
{
    public class Configs : List<Config>
    {

    }
    public class Config : IActor
    {
        public string Name { get; set; }
        public string ApiUrl { get; set; }
        public string ApiUserName { get; set; }
        public string ApiKey { get; set; }
        public string ServiceShortName { get; set; }
        public string Actor { get; set; }
    }
}
