
namespace LUISTools
{
    using LUISTools.Helpers;
    using LUISTools.Models;
    using LUISTools.Models.ExampleGenerator;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ExampleGeneratorTool
    {
        private Dictionary<string, List<string>> entityValues = new Dictionary<string, List<string>>();
        public void GenerateExampleFromFiles(Stream map, Stream output)
        {
            var exampleMaps = JsonHelper.DeserializeFromStream<List<ExampleMap>>(map);
            var batchExamples = new BatchExamples();

            foreach(var exampleMap in exampleMaps)
            {
                var batchExample = new BatchExample();
                batchExample.Intent = exampleMap.Intent;
                batchExample.ExamplesByEntity = new List<Dictionary<string, string>>();

                // load values
                foreach (var exampleEntity in exampleMap.EntitySources)
                {
                    if (!this.entityValues.ContainsKey(exampleEntity.EntityName))
                    {
                        var values = File.ReadAllLines(exampleEntity.FilePathToValue);
                        this.entityValues.Add(exampleEntity.EntityName, new List<string>(values));
                    }
                }

                var firstEntity = exampleMap.EntitySources.First();
                for (int exampleIndex = 0; exampleIndex < this.entityValues[firstEntity.EntityName].Count; exampleIndex++)
                {
                    var entityExampleValues = new Dictionary<string, string>();
                    foreach (var exampleEntity in exampleMap.EntitySources)
                    {
                        entityExampleValues.Add(exampleEntity.EntityName, this.entityValues[exampleEntity.EntityName][exampleIndex]);
                    }

                    batchExample.ExamplesByEntity.Add(entityExampleValues);
                }

                batchExamples.Examples.Add(batchExample);
            }

            JsonHelper.SerializeToStream(batchExamples, output);
        }
    }
}
