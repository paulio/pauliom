// ***********************************************************************
// <copyright file="BatchTestTool.cs">
//     Copyright (c) CBC Ltd. All rights reserved.
// </copyright>
// <summary>Creates files for use with the LUIS online Batch Tester</summary>
// ***********************************************************************

namespace LUISTools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using LUISTools.Helpers;
    using LUISTools.Models;

    /// <summary>
    /// Class BatchTestTool.
    /// Implements the <see cref="LUISTools.ISupportsCommands" />
    /// </summary>
    /// <seealso cref="LUISTools.ISupportsCommands" />
    public class BatchTestTool : ISupportsCommands
    {
        private const string TemplateCommandKey = "template";
        private const string TemplateExampleEntitiesKey = "exampleentities";
        private const string TemplateBatchResultKey = "batchresult";

        /// <summary>
        /// Gets the help.
        /// </summary>
        /// <value>The help.</value>
        public string Help => "GenerateBatchTest:=true Template:=<path to template.json> ExampleEntities:=<path to example entities.json> Out:=<path to resulting json>";

        /// <summary>
        /// Determines whether this instance can handle the specified commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <returns><c>true</c> if this instance can handle the specified commands; otherwise, <c>false</c>.</returns>
        public bool CanHandle(Dictionary<string, string> commands)
        {
            return commands.ContainsKey("GenerateBatchTest");
        }

        /// <summary>
        /// Executes the specified commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <returns>Message information about the execution.</returns>
        /// <remarks>Missing key exceptions will throw default exceptions.</remarks>
        public string Execute(Dictionary<string, string> commands)
        {
            string templatePath = commands[TemplateCommandKey];
            string examplePath = commands[TemplateExampleEntitiesKey];
            string outputPath = commands[TemplateBatchResultKey];
            using (FileStream template = new FileStream(templatePath, FileMode.Open))
            using (FileStream examples = new FileStream(examplePath, FileMode.Open))
            using (FileStream result = new FileStream(outputPath, FileMode.Create))
            {
                this.GenerateTest(template, examples, result);
            }

            return nameof(BatchTestTool) + " wrote to " + outputPath;
        }

        /// <summary>
        /// Generates the test.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="entityExamples">The entity examples.</param>
        /// <param name="output">The output.</param>
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
                        LuisBatchTestItem item = new LuisBatchTestItem
                        {
                            Intent = intent,
                            Entities = new List<Entity>()
                        };
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
