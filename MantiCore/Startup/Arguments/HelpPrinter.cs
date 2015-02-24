using System;
using System.Collections.Generic;
using MantiCore.Bundles;

namespace MantiCore.Startup.Arguments
{
    public static class HelpPrinter
    {
        /// <summary>
        /// Print help.
        /// </summary>
        /// <param name="bundleArguments">Arguments of bundles to print.</param>
        /// <param name="programNameDescription">Decription to accompany the program name.</param>
        /// <param name="programSynopsis">Synopsis section of the help.</param>
        /// <param name="optionDescriptionPrefix">Text before options.</param>
        /// <param name="optionDescriptionPostfix">Text after options.</param>
        /// <param name="programAuthor">Author section of the help.</param>
        /// <param name="programReportBugs">Bugs section of the help.</param>
        /// <param name="programCopyright">Copyright section of the help.</param>
        /// <param name="confirm">Halt before continuing execution after printing.</param>
        public static void ShowHelp(
            IEnumerable<BundleArgument> bundleArguments,
            string programNameDescription,
            string programSynopsis,
            string optionDescriptionPrefix,
            string optionDescriptionPostfix,
            string programAuthor,
            string programReportBugs,
            string programCopyright,
            bool confirm)
        {
            WriteProgramName(programNameDescription);
            WriteProgramSynopsis(programSynopsis);
            WriteOptionDescriptions(bundleArguments, optionDescriptionPrefix, optionDescriptionPostfix);
            WriteProgramAuthor(programAuthor);
            WriteProgramReportingBugs(programReportBugs);
            WriteProgramCopyrightLicense(programCopyright);

            if (confirm)
            {
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Print program name and description.
        /// </summary>
        /// <param name="description">Description to print.</param>
        private static void WriteProgramName(string description)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("NAME");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine('\t' + appName + " - " + description + '\n');
            Console.ForegroundColor = origColour;
        }

        /// <summary>
        /// Print the program synopsis.
        /// </summary>
        /// <param name="synopsis">Synopsis to print.</param>
        private static void WriteProgramSynopsis(string synopsis)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("SYNOPSIS");
            Console.ForegroundColor = ConsoleColor.Gray;
            synopsis = synopsis.Replace("{appName}", appName);
            Console.WriteLine('\t' + synopsis + '\n');
            Console.ForegroundColor = origColour;
        }

        /// <summary>
        /// Print the program author.
        /// </summary>
        /// <param name="authorByString">Author string to print.</param>
        private static void WriteProgramAuthor(string authorByString)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("AUTHOR");
            Console.ForegroundColor = ConsoleColor.Gray;
            authorByString = authorByString.Replace("{appName}", appName);
            Console.WriteLine('\t' + authorByString + '\n');
            Console.ForegroundColor = origColour;
        }

        /// <summary>
        /// Print the program reporting bugs section.
        /// </summary>
        /// <param name="reportString">Report bugs string.</param>
        private static void WriteProgramReportingBugs(string reportString)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("REPORTING BUGS");
            Console.ForegroundColor = ConsoleColor.Gray;
            reportString = reportString.Replace("{appName}", appName);
            var spl = reportString.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in spl)
            {
                Console.WriteLine('\t' + s);
            }
            Console.WriteLine();
            Console.ForegroundColor = origColour;
        }

        /// <summary>
        /// Print the program copyright license.
        /// </summary>
        /// <param name="copyrightLicense">Copyright license text.</param>
        private static void WriteProgramCopyrightLicense(string copyrightLicense)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("COPYRIGHT");
            Console.ForegroundColor = ConsoleColor.Gray;
            copyrightLicense = copyrightLicense.Replace("{appName}", appName);
            var spl = copyrightLicense.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in spl)
            {
                Console.WriteLine('\t' + s);
            }
            Console.WriteLine();
            Console.ForegroundColor = origColour;
        }

        /// <summary>
        /// Prints all the options in an OptionsSet and prefix/postfix text for the description.
        /// </summary>
        /// <param name="ba">BundleArguments to use options from.</param>
        /// <param name="prefixText">Text to print before options.</param>
        /// <param name="postText">Text to print after options.</param>
        private static void WriteOptionDescriptions(IEnumerable<BundleArgument> ba, string prefixText, string postText)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("DESCRIPTION");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (!string.IsNullOrWhiteSpace(prefixText))
            {
                prefixText = prefixText.Replace("{appName}", appName);
                var spl = prefixText.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in spl)
                {
                    Console.WriteLine('\t' + s);
                }
            }

            var buffWid = Console.BufferWidth;
            foreach (var p in ba)
            {
                Console.Write('\t');
                for (var j = 0; j < p.Options.Length; j++)
                {
                    Console.Write(p.Options[j]);
                    if (j + 1 != p.Options.Length)
                    {
                        Console.Write(", ");
                    }
                    else
                    {
                        if (p.ArgumentNames.Length > 0)
                        {
                            Console.Write('\t');
                            foreach (var t in p.ArgumentNames)
                            {
                                Console.Write(" [" + t + "]");
                            }
                        }

                        Console.WriteLine();
                    }
                }

                Console.Write("\t\t");
                var len = buffWid - Console.CursorLeft;

                foreach (var l in p.Description.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
                    )
                {
                    var lenP = 0;
                    foreach (var w in l.Split(' '))
                    {
                        var word = w;

                        if (lenP != 0 && (lenP + word.Length + 1) > len)
                        {
                            if (lenP != len) Console.Write("\n");
                            Console.Write("\t\t");
                            lenP = 0;
                        }
                        else if (lenP != 0)
                        {
                            word = ' ' + word;
                        }
                        Console.Write(word);
                        lenP += word.Length;
                    }
                    if (lenP != len) Console.Write("\n");
                    Console.Write("\t\t");
                }
                Console.WriteLine();
            }

            if (!string.IsNullOrWhiteSpace(postText))
            {
                postText = postText.Replace("{appName}", appName);
                var spl = postText.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in spl)
                {
                    Console.WriteLine('\t' + s);
                }
            }
            Console.WriteLine();
            Console.ForegroundColor = origColour;
        }
    }
}
