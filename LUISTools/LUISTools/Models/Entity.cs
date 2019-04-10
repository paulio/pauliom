namespace LUISTools.Models
{
    using Newtonsoft.Json;

    public class Entity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("entity")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        /// <value>The start position.</value>
        [JsonProperty("startPos")]
        public int StartPosition { get; set; }

        /// <summary>
        /// Gets or sets the end position.
        /// </summary>
        /// <value>The end position.</value>
        [JsonProperty("endPos")]
        public int EndPosition { get; set; }
    }
}
