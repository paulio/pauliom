using LUISTools;
using System;
using System.IO;

namespace LuisToolsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var batchTestTool = new BatchTestTool();
            string templatePath = @"D:\Repos\pauliom\LUISTools\LUISTools\Examples\Template.json";
            string examplePath = @"D:\Repos\pauliom\LUISTools\LUISTools\Examples\Examples.json";
            string outputPath = @"D:\Repos\pauliom\LUISTools\LUISTools\Examples\result.json";
            using (FileStream template = new FileStream(templatePath, FileMode.Open))
            using (FileStream examples = new FileStream(examplePath, FileMode.Open))
            using (FileStream result = new FileStream(outputPath, FileMode.Create))
            {
                batchTestTool.GenerateTest(template, examples, result);
            }
        }
    }
}
