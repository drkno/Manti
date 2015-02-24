namespace MantiCore.Startup.Arguments
{
    public partial class OptionSet
    {
        /// <summary>
        /// Style of options to use.
        /// </summary>
        public enum OptionStyle
        {
            Nix = 0,
            Linux = Nix,
            Unix = Nix,
            Osx = Nix,
            Windows = 2
        }
    }
}
