using System.Collections.Generic;

namespace TruSaber.Configuration
{
    public class GameOptions
    {
        public string       DebugLevel        { get; set; }
        public string       LevelDirectory    { get; set; }
        public List<string> PluginDirectories { get; set; }
    }
}