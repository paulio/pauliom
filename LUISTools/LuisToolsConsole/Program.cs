// ***********************************************************************
// <copyright file="Program.cs">
//     Copyright (c) CBC Ltd. All rights reserved.
// </copyright>
// <summary>Main console program</summary>
// ***********************************************************************

namespace LuisToolsConsole
{
    using System;
    using System.Collections.Generic;
    using LUISTools;

    /// <summary>
    /// Console Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The help message
        /// </summary>
        private const string helpMessage =
            @"GenerateExample:=true Map:=C:\LUISTools\Examples\ExampleGeneratorMap.json ExampleResult:=C:\LUISTools\Examples\Examples.json" +
            @"GenerateBatchTest:=true Template:=C:\LUISTools\Examples\Template.json ExampleEntities:=C:\LUISTools\Examples\Examples.json BatchResult:=C:\LUISTools\Examples\BatchTest.json";

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments, see help commands for more details</param>
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

                Console.WriteLine(helpMessage);
                return;
            }

            foreach(var tool in tools)
            {
                try
                {
                    Console.WriteLine(tool.Execute(commands));
                }
                catch (Exception e)
                {
                    Console.WriteLine(nameof(tool) + "failed with " + e.Message);
                }
            }
        }

        /// <summary>
        /// Commands from arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
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
