namespace LUISTools.Models
{
    using System.Collections.Generic;

    public class BatchExample
    {
        /// <summary>
        /// Gets or sets the intent.
        /// </summary>
        /// <value>The intent.</value>
        public string Intent { get; set; }

        /// <summary>
        /// Gets or sets the examples by entity.
        /// </summary>
        /// <value>The examples by entity.</value>
        public List<Dictionary<string, string>> ExamplesByEntity { get; set; }
    }
}
