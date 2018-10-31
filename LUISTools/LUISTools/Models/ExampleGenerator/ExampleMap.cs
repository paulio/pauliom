
namespace LUISTools.Models.ExampleGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ExampleMap
    {
        public string Intent { get; set; }

        public List<EntityMapping> EntitySources { get; set; }
    }
}
