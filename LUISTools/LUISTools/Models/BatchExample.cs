
namespace LUISTools.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BatchExample
    {
        public string Intent { get; set; }
        public List<Dictionary<string, string>> ExamplesByEntity { get; set; }
    }
}
