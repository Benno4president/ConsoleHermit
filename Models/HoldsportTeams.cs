// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using Newtonsoft.Json;

namespace ConsoleHermit.Models
{
    public class HoldsportTeams : HoldsportBaseClass
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("primary_color")]
        public string PrimaryColor;

        [JsonProperty("secondary_color")]
        public string SecondaryColor;

        [JsonProperty("role")]
        public int Role;
    }
}