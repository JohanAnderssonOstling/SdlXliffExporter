using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace SdlXliffExporter
{
    class CommandLineOptions
    {
        [Option('t', "target", Required = true, HelpText = "Target language code")]
        public string TargetLanguage { get; set; }
        [Option('o', "output", Required = false, HelpText = "Output folder path")]
        public string OutputFolder { get; set; } = System.Environment.CurrentDirectory;
        [Option('d', "delimeter", Required = false, Default = "\t", HelpText = "Delimeter used in output files")]
        public string Delimeter { get; set; }
        [Option('e', "extension", Required = false, Default = ".tsv", HelpText = "File extension for output files")]
        public string FileExtension { get; set; }
        [Option('r', "recursive", HelpText = "Parse folders recursively")]
        public bool RecursiveParsing { get; set; }
        [Value(0, MetaName = "Input Files", Required = true, HelpText = "Input Files to be exported")]
        public IEnumerable<string> InputFiles { get; set; }

    }
}
