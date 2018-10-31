
namespace LUISTools
{
    using LUISTools.Helpers;
    using LUISTools.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class BatchTestTool : ISupportsCommands
    {
        public string Help => "GenerateBatchTest:=true Template:=<path to template.json> ExampleEntities:=<path to example entities.json> Out:=<path to resulting json>";

        public bool CanHandle(Dictionary<string, string> commands)
        {
            return commands.ContainsKey("GenerateBatchTest");
        }

        public string Execute(Dictionary<string, string> commands)
        {
            string templatePath = commands["template"];
            string examplePath = commands["exampleentities"];
            string outputPath = commands["batchresult"];
            using (FileStream template = new FileStream(templatePath, FileMode.Open))
            using (FileStream examples = new FileStream(examplePath, FileMode.Open))
            using (FileStream result = new FileStream(outputPath, FileMode.Create))
            {
                this.GenerateTest(template, examples, result);
            }

            return nameof(BatchTestTool) + " wrote to " + outputPath;
        }

        public void GenerateTest(Stream template, Stream entityExamples, Stream output)
        {
            var templateBatch = JsonHelper.DeserializeFromStream<BatchTest>(template);
            var batchExamples = JsonHelper.DeserializeFromStream<BatchExamples>(entityExamples);

            var batchTest = new BatchTest();

            foreach( var example in batchExamples.Examples )
            {
                var intent = example.Intent;
                var templatesForIntent = templateBatch.BatchTestItems.Where(b => b.Intent.Equals(intent, StringComparison.InvariantCultureIgnoreCase));
                foreach (var entityExample in example.ExamplesByEntity)
                {
                    foreach(var intentTemplate in templatesForIntent)
                    {
                        LuisBatchTestItem item = new LuisBatchTestItem();
                        item.Intent = intent;
                        item.Entities = new List<Entity>();
                        var text = intentTemplate.Text;
                        foreach(var entityKeyValue in entityExample)
                        {
                            var entityName = entityKeyValue.Key;
                            var entityValue = entityKeyValue.Value;
                            Entity newEntity = new Entity { Name = entityName };
                            var placeHolder = $"<{entityName}>";
                            newEntity.StartPosition = text.IndexOf(placeHolder);
                            text = text.Replace(placeHolder, entityValue);
                            newEntity.EndPosition = newEntity.StartPosition + entityValue.Length;
                             
                            item.Entities.Add(newEntity);
                        }

                        item.Text = text;
                        batchTest.BatchTestItems.Add(item);
                    }
                }
            }

            JsonHelper.SerializeToStream(batchTest.BatchTestItems, output);
        }

       
    }
}
