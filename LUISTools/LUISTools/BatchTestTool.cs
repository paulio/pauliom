
namespace LUISTools
{
    using LUISTools.Helpers;
    using LUISTools.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class BatchTestTool
    {
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
