
namespace LUISTools.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class LuisBatchTestItem
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; }
    }
}
