
namespace LUISTools.Models
{
    using Newtonsoft.Json;

    public class Entity
    {
        [JsonProperty("entity")]
        public string Name { get; set; }

        [JsonProperty("startPos")]
        public int StartPosition { get; set; }

        [JsonProperty("endPos")]
        public int EndPosition { get; set; }
    }
}
