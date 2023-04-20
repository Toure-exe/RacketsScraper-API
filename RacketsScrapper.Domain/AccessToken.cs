using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RacketsScrapper.Domain
{
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string? Access_Token { get; set; }

        
        [JsonProperty("expires_in")]
        public string? Expiration { get; set; }

        [JsonProperty("token_type")]
        public string? Type { get; set; }

        [JsonProperty("scope")]
        public string? Scope { get; set; }
    }
}
