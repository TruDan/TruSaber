using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TruSaber.Configuration;
using TruSaber.Models;

namespace TruSaber.Services
{
    public class LevelManager
    {
        private readonly IOptions<GameOptions> _optionsAccessor;

        public List<BeatLevel> Levels { get; set; }

        public LevelManager(IOptions<GameOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
        }

        public async Task LoadLevels()
        {
            var dir = _optionsAccessor.Value.LevelDirectory;
            if (!Directory.Exists(dir))
                return;

            List<BeatLevel> levels = new List<BeatLevel>();
            
            var allFiles = Directory.GetFileSystemEntries(dir);
            foreach (var allFile in allFiles)
            {
                var info = new FileInfo(allFile);
                if ((info.Attributes & FileAttributes.Directory) != 0)
                {
                    // load as directory
                    var beatLevel = new BeatLevel(info.FullName);
                    levels.Add(beatLevel);
                }
                else if (info.Extension.ToLowerInvariant() == "zip")
                {
                    // load as zip
                    
                }
            }

            await Task.WhenAll(levels.Select(l => l.LoadLevelInfoAsync()));
        }
        
    }
}