using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using System.IO;

namespace PrependFile
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            var helpOptions = new HelpOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, helpOptions))
            {
                if (helpOptions.Help)
                {
                    Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options));
                    ContinueScreen.WaitAndExit();
                }
            }

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                try
                {
                    Rename.RenameFile(options.Prepend, options.TargetFile);
                    Console.WriteLine("File changed from " + options.TargetFile + " to " + options.Prepend + options.TargetFile);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception Source: {0}", e.Source);
                }
                
            }
        }
    }

    // Define a class to receive parsed values
    class Options
    {
        [Option('f', "file", Required = true,
          HelpText = "Target file to prepend.")]
        public string TargetFile { get; set; }        

        [Option('p', "prepend", Required = true,
          HelpText = "String to prepend file with.")]
        public string Prepend { get; set; }

        [Option('h', "help", HelpText = "Prints this help", Required = false, DefaultValue = false)]
        public bool Help { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

    }

    class ContinueScreen
    {
        public static void WaitAndExit()
        {
            Console.WriteLine("Press any key to continue ....");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }

    // Define a class to receive parsed values
    class HelpOptions
    {
        [Option('h', "help", HelpText = "Prints this help", Required = false, DefaultValue = false)]
        public bool Help { get; set; }

    }


    // File Renaming Class
    class Rename
    {
        public static void RenameFile(string pre, string filename)
        {
            string oldFileName = filename;
            string newFileName = pre + filename;
            File.Move(oldFileName, newFileName);
        }
    }
}
