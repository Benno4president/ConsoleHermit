using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleHermit.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class HoldsportActivityModel
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("user_id")]
        public int UserId;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("status")]
        public string Status;

        [JsonProperty("status_code")]
        public int StatusCode;

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt;
    }


}
