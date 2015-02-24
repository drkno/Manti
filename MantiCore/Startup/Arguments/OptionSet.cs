/*
 * Knox.Options (Manti Adjusted)
 * This is a Mono.Options semi-compatible library for managing CLI
 * arguments and displaying help text for a program. Created as
 * Mono.Options has an issue and was requiring significant
 * modification to meet my needs. It was quicker to write a new
 * version that supported a similar API than to fix the origional.
 * 
 * This version of Knox.Options has been modified significantly to
 * fit into the Manti Framework requirements.
 * 
 * Copyright © Matthew Knox, Knox Enterprises 2014-Present.
 * This code is avalible under the license that this copy of the
 * Manti Framework was aquired under.
*/

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MantiCore.Bundles;

#endregion

namespace MantiCore.Startup.Arguments
{
    /// <summary>
    /// Set of CLI interface
    /// </summary>
    public partial class OptionSet : IEnumerable
    {
        /// <summary>
        /// Index of individual options in option set.
        /// </summary>
        private readonly Dictionary<string, int> _lookupDictionary = new Dictionary<string, int>();
        /// <summary>
        /// List of options contained in this option set.
        /// </summary>
        private readonly List<Option> _options = new List<Option>();
        private static readonly char[] ArgumentSeparators = { '=', ':' };
        private static OptionStyle _optionsStyle;

        /// <summary>
        /// Option prefixes for use with various option styles.
        /// </summary>
        private static readonly string[] OptionsPrefixes = { "-", "--", "/", "/" };

        /// <summary>
        /// Enumerator of this OptionSet
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return _options.GetEnumerator();
        }

        /// <summary>
        /// Add a cli option to the set.
        /// </summary>
        /// <param name="cliOption">The option to associate this option with.</param>
        /// <param name="arguments">The arguments associated with this option.</param>
        /// <param name="conflictSilent">If a cli option has already been specified by a previous option
        /// handle the error silently rather than throwing an exception.</param>
        private void Add(string cliOption, string[] arguments, bool conflictSilent = false)
        {
            var i = (int) _optionsStyle;
            if (!cliOption.StartsWith(OptionsPrefixes[i]) && !cliOption.StartsWith(OptionsPrefixes[i + 1]))
            {
                throw new OptionException("Inconsistent usage of CLI option prefixes detected. Only one style is accepted at any given time.");
            }

            var option = new Option(cliOption, arguments);
            _options.Add(option);

            try
            {
                _lookupDictionary.Add(option.OptionValue, _options.Count);
            }
            catch (Exception)
            {
                if (!conflictSilent)
                {
                    throw new OptionException("Duplicate input option detected.", option.OptionValue);
                }
                Debug.WriteLine("Detected input argument conflict!");
            }
        }

        /// <summary>
        /// Sets the expected style of the input options.
        /// </summary>
        /// <param name="cliOption">An option to extract the styling from.</param>
        private void SetOptionStyle(string cliOption)
        {
            var i = OptionsPrefixes.TakeWhile(optionsPrefix => !cliOption.StartsWith(optionsPrefix)).Count();
            OptionStyle style;
            try
            {
                style = (OptionStyle)i;
            }
            catch (InvalidCastException)
            {
                throw new OptionException("The option \"" + cliOption + "\" starts with an invalid option prefix (valid are " + string.Join(", ", OptionsPrefixes) + ").");
            }
            _optionsStyle = style;
        }

        /// <summary>
        /// Parses a set of arguments into the option equivilents.
        /// </summary>
        /// <param name="arguments">Arguments to parse.</param>
        /// <param name="conflictSilent">If a cli option has already been specified by a previous option
        /// handle the error silently rather than throwing an exception.</param>
        public void Parse(IEnumerable<string> arguments, bool conflictSilent = false)
        {
            var temp = new List<string>();
            var optionRead = 0;

            var enumerable = arguments.ToList();
            if (enumerable.Count == 0)
            {
                return;
            }
            SetOptionStyle(enumerable[0]);
            for (var i = 1; i <= enumerable.Count; i++)
            {
                if (i != enumerable.Count && ArgumentSeparators.Any(s => enumerable[i].Contains(s)))
                {
                    enumerable.RemoveAt(i);
                    enumerable.InsertRange(i, enumerable[i].Split(ArgumentSeparators));
                }

                if (i == enumerable.Count || enumerable[i].StartsWith(OptionsPrefixes[(int) _optionsStyle]))
                {
                    Add(enumerable[optionRead], temp.ToArray(), conflictSilent);
                    temp.Clear();
                    optionRead = i;
                }
                else
                {
                    temp.Add(enumerable[i]);
                }
            }
            _options.Sort();
        }

        public void SetBundleArguments(IEnumerable<BundleArgument> arguments)
        {
            foreach (var arg in arguments)
            {
                foreach (var bundleOption in arg.Options)
                {
                    var ind = _options.BinarySearch(new Option(bundleOption));
                    if (ind < 0) continue;
                    arg.SetValue(_options[ind].Arguments);
                    break;
                }
            }
        }
    }
}