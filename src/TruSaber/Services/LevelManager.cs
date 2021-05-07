using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TruSaber.Configuration;
using TruSaber.Models;

namespace TruSaber.Services
{
    public class LevelManager
    {
        private readonly GameOptions _options;

        public List<BeatLevel> Levels { get; set; }

        public LevelManager(GameOptions options)
        {
            _options = options;
        }

        public async Task LoadLevels()
        {
            var dir = _options.LevelDirectory;
            if (!Directory.Exists(dir))
                return;

            List<BeatLevel> levels = new List<BeatLevel>();
            
            var allFiles = Directory.GetFileSystemEntries(dir);
            foreach (var allFile in allFiles)
            {
                try
                {
                    var info = new FileInfo(allFile);
                    if (info.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        // load as directory
                        var beatLevel = new BeatLevel(info.FullName);
                        await beatLevel.LoadLevelInfoAsync();
                        levels.Add(beatLevel);
                    }
                    else if (info.Extension.ToLowerInvariant() == "zip")
                    {
                        // load as zip

                    }
                    
                }
                catch
                {
                    Console.WriteLine($"Failed to load level at {allFile}");
                }
            }

            Levels = levels;
        }
        
    }
}