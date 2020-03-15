using CommandLine;
using SdlXliffExporter.DataStructures;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace SdlXliffExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(ParseOptions)
                .WithNotParsed(HandleParseError);
        }
        private static void ParseOptions(CommandLineOptions options)
        {
            options = VerifyOptions(options);
            List<string> tradosFiles = GetTradosFiles(options);
            FileParser fileParser = new FileParser(options.TargetLanguage, ConfigurationManager.AppSettings["projectExtensions"].Split(","));
            Serializer serializer = new Serializer(options.Delimeter, options.FileExtension, options.OutputFolder);
            for(int i = 0; i < tradosFiles.Count; i++)
            {
                Console.Write("\rExporting " + (i + 1) + " of " + tradosFiles.Count);
                TradosObject tradosObject = fileParser.ParseFile(tradosFiles[i]);
                serializer.Serialize(tradosObject);
            }
        }
        private static CommandLineOptions VerifyOptions(CommandLineOptions options)
        {
            if (!options.FileExtension.StartsWith("."))
                options.FileExtension = "." + options.FileExtension;
            return options;
        }
        private static List<string> GetTradosFiles(CommandLineOptions options)
        {
            List<string> tradosFiles = new List<string>();
            FolderParser folderParser = new FolderParser(ConfigurationManager.AppSettings["tradosExtensions"].Split(","));
            foreach (string inputFile in options.InputFiles)
            {
                if (File.Exists(inputFile))
                    tradosFiles.Add(inputFile);
                else if (Directory.Exists(inputFile))
                    if (options.RecursiveParsing)
                        tradosFiles.AddRange(folderParser.ParseFolderRecursively(inputFile));
                    else
                        tradosFiles.AddRange(folderParser.ParseFolder(inputFile));
                else
                    Console.WriteLine("Could not find: " + inputFile);
            }
            return tradosFiles;
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
            Environment.Exit(-1);
        }
        
    }
}
