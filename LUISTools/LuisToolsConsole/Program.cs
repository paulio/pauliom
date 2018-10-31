using LUISTools;
using System;
using System.Collections.Generic;
using System.IO;

namespace LuisToolsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ISupportsCommands> tools = new List<ISupportsCommands>
            {
                new ExampleGeneratorTool(),
                new BatchTestTool()
            };

            var commands = CommandsFromArgs(args);

            if (commands.Keys.Count == 0 || commands.ContainsKey("help"))
            {
                foreach(var tool in tools)
                {
                    Console.WriteLine(tool.Help);
                }

                return;
            }

            foreach(var tool in tools)
            {
                Console.WriteLine(tool.Execute(commands));
            }

            //var batchTestTool = new BatchTestTool();
            //string templatePath = @"D:\Repos\pauliom\LUISTools\LUISTools\Examples\Template.json";
            //string examplePath = @"D:\Repos\pauliom\LUISTools\LUISTools\Examples\Examples.json";
            //string outputPath = @"D:\Repos\pauliom\LUISTools\LUISTools\Examples\result.json";
            //using (FileStream template = new FileStream(templatePath, FileMode.Open))
            //using (FileStream examples = new FileStream(examplePath, FileMode.Open))
            //using (FileStream result = new FileStream(outputPath, FileMode.Create))
            //{
            //    batchTestTool.GenerateTest(template, examples, result);
            //}

            //var exampleGeneratorTool = new ExampleGeneratorTool();
            //string mapPath = @"D:\Repos\pauliom\LUISTools\LUISTools\Examples\ExampleGeneratorMap.json";
            //string outputPath = @"D:\Repos\pauliom\LUISTools\LUISTools\Examples\result.json";
            //using (FileStream map = new FileStream(mapPath, FileMode.Open))
            //using (FileStream result = new FileStream(outputPath, FileMode.Create))
            //{
            //    exampleGeneratorTool.GenerateExampleFromFiles(map, result);
            //}
        }

        private static Dictionary<string, string> CommandsFromArgs(string[] args)
        {
            var commands = new Dictionary<string, string>();
            var splitter = new string[] { ":=" };
            foreach (var arg in args)
            {
                if (arg.Contains(":="))
                {
                    var pair = arg.Split(splitter, StringSplitOptions.None);
                    if (pair.Length == 2)
                    {
                        commands.Add(pair[0].ToLower(), pair[1]);
                    }
                }
            }

            return commands;
        }
    }
}
