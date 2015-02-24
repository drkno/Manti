using System;

namespace MantiCore.Startup.Arguments
{
    public partial class OptionSet
    {
        /// <summary>
        /// Represents an individual option of an OptionSet
        /// </summary>
        private class Option : IComparable<Option>
        {
            /// <summary>
            /// Creates a new option.
            /// </summary>
            /// <param name="option">Cli option that was passed.</param>
            /// <param name="arguments">Arguments that are associated with this option.</param>
            public Option(string option, params string[] arguments)
            {
                OptionValue = option;
                Arguments = arguments == null || arguments.Length == 0 ? new []{"true"} : arguments;
            }

            /// <summary>
            /// Option passed to the CLI.
            /// </summary>
            public string OptionValue { get; private set; }
            /// <summary>
            /// Arguments that this option provides.
            /// </summary>
            public string[] Arguments { get; private set; }

            public int CompareTo(Option other)
            {
                return string.CompareOrdinal(OptionValue, other.OptionValue);
            }
        }
    }
}
