namespace LUISTools.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class LuisBatchTestItem
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the intent.
        /// </summary>
        /// <value>The intent.</value>
        [JsonProperty("intent")]
        public string Intent { get; set; }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; }
    }
}
