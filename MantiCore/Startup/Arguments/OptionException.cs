/*
 * Knox.Options
 * This is a Mono.Options semi-compatible library for managing CLI
 * arguments and displaying help text for a program. Created as
 * Mono.Options has an issue and was requiring significant
 * modification to meet my needs. It was quicker to write a new
 * version that supported a similar API than to fix the origional.
 * 
 * Copyright © Matthew Knox, Knox Enterprises 2014-Present.
 * This code is avalible under the MIT license in the state
 * that it was avalible on 05/11/2014 from
 * http://opensource.org/licenses/MIT .
*/

using System;

namespace MantiCore.Startup.Arguments
{
    /// <summary>
    /// Exception that is thrown when there is an error with the options specified.
    /// </summary>
    [Serializable]
    public class OptionException : Exception
    {
        /// <summary>
        /// Create a new OptionException.
        /// </summary>
        /// <param name="errorText">The description of this exception.</param>
        /// <param name="errorArguments">Arguments that were in error.</param>
        public OptionException(string errorText, params string[] errorArguments)
            : base(errorText)
        {
            ErrorArguments = errorArguments;
        }

        /// <summary>
        /// Create a new OptionException.
        /// </summary>
        /// <param name="errorText">The description of this exception.</param>
        /// <param name="innerException">The inner exception that caused this one to occur.</param>
        /// <param name="errorArguments">Arguments that were in error.</param>
        public OptionException(string errorText, Exception innerException, params string[] errorArguments)
            : base(errorText, innerException)
        {
            ErrorArguments = errorArguments;
        }

        /// <summary>
        /// Arguments that were in error.
        /// </summary>
        public string[] ErrorArguments { get; private set; }
    }
}