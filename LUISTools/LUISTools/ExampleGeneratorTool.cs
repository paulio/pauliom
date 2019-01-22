// ***********************************************************************
// <copyright file="ExampleGeneratorTool.cs">
//     Copyright (c) CBC Ltd. All rights reserved.
// </copyright>
// <summary>Tool to help create the examples that are used by the Batch Test Tool.</summary>
// ***********************************************************************

namespace LUISTools
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using LUISTools.Helpers;
    using LUISTools.Models;
    using LUISTools.Models.ExampleGenerator;

    /// <summary>
    /// Class ExampleGeneratorTool.
    /// Implements the <see cref="LUISTools.ISupportsCommands" />
    /// </summary>
    /// <seealso cref="LUISTools.ISupportsCommands" />
    public class ExampleGeneratorTool : ISupportsCommands
    {
        /// <summary>
        /// The entity values
        /// </summary>
        private readonly Dictionary<string, List<string>> entityValues = new Dictionary<string, List<string>>();

        /// <summary>
        /// Gets the help.
        /// </summary>
        /// <value>The help.</value>
        public string Help => "GenerateExample:=true Map:=<path to map.json> Out:=<path to resulting json>";

        /// <summary>
        /// Determines whether this instance can handle the specified commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <returns><c>true</c> if this instance can handle the specified commands; otherwise, <c>false</c>.</returns>
        public bool CanHandle(Dictionary<string, string> commands)
        {
            return commands.ContainsKey("GenerateExample");
        }

        /// <summary>
        /// Executes the specified commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <returns>System.String.</returns>
        public string Execute(Dictionary<string, string> commands)
        {
            string mapPath = commands["map"];
            string outputPath = commands["exampleresult"];
            using (FileStream map = new FileStream(mapPath, FileMode.Open))
            using (FileStream result = new FileStream(outputPath, FileMode.Create))
            {
                this.GenerateExampleFromFiles(map, result);
            }

            return nameof(ExampleGeneratorTool) + " wrote to " + outputPath;
        }

        /// <summary>
        /// Generates the example from files.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="output">The output.</param>
        public void GenerateExampleFromFiles(Stream map, Stream output)
        {
            var exampleMaps = JsonHelper.DeserializeFromStream<List<ExampleMap>>(map);
            var batchExamples = new BatchExamples();

            foreach(var exampleMap in exampleMaps)
            {
                var batchExample = new BatchExample
                {
                    Intent = exampleMap.Intent,
                    ExamplesByEntity = new List<Dictionary<string, string>>()
                };

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
