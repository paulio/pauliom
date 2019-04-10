namespace LUISTools.Models.ExampleGenerator
{
    using System.Collections.Generic;

    /// <summary>
    /// Example Map.
    /// </summary>
    public class ExampleMap
    {
        /// <summary>
        /// Gets or sets the intent.
        /// </summary>
        /// <value>The intent.</value>
        public string Intent { get; set; }

        /// <summary>
        /// Gets or sets the entity sources.
        /// </summary>
        /// <value>The entity sources.</value>
        public List<EntityMapping> EntitySources { get; set; }
    }
}
