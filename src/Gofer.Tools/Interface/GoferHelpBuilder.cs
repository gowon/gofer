using System;
using System.CommandLine;
using System.CommandLine.Help;
using System.Drawing;
using System.Reflection;
using Pastel;

namespace Gofer.Tools.Interface
{
    public class GoferHelpBuilder : HelpBuilder
    {
        public static readonly Lazy<string> AssemblyVersion =
            new Lazy<string>(() =>
            {
                var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                var assemblyVersionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                return assemblyVersionAttribute is null
                    ? assembly.GetName().Version?.ToString()
                    : assemblyVersionAttribute.InformationalVersion;
            });

        public GoferHelpBuilder(IConsole console, int? columnGutter = null, int? indentationSize = null,
            int? maxWidth = null) : base(console, columnGutter, indentationSize, maxWidth)
        {
        }

        public override void Write(ICommand command)
        {
            var asciiArtStrings = new[]
            {
                "                            ___         ",
                $"    {".-\"\"\"-.__".Pastel(Color.DarkOrange)}       ___ ___|  _|___ ___ ",
                $"   {"/      ' o'\\".Pastel(Color.DarkOrange)}    | . | . |  _| -_|  _|",
                $"  {";  '.  :   _c".Pastel(Color.DarkOrange)}    |_  |___|_| |___|_|  ",
                $"  {"\"\\._ ) ::-\"".Pastel(Color.DarkOrange)}      |___|                ",
                "      \"\"m \"m".Pastel(Color.DarkOrange)
            };

            foreach (var line in asciiArtStrings)
            {
                System.Console.WriteLine(line);
            }

            System.Console.WriteLine($"\n\tGofer Database Command-line Tools {AssemblyVersion.Value}\n");
            base.Write(command);
        }
    }
}