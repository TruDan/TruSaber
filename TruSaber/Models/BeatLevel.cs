using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using BeatMapInfo;

namespace TruSaber.Models
{
    public class BeatLevel
    {
        
        public BeatMapInfo.BeatMapInfo MapInfo { get; private set; }
        
        public Characteristic[] AvailableCharacteristics { get; private set; }
        public IReadOnlyDictionary<Characteristic, Difficulty[]> AvailableDifficulties { get; private set; }
        
        public string SongPath
        {
            get => Path.Combine(Directory, MapInfo.SongFilename);
        }

        public string CoverImagePath
        {
            get => Path.Combine(Directory, MapInfo.CoverImageFilename);
        }

        public TimeSpan PreviewStartTime
        {
            get => TimeSpan.FromSeconds(MapInfo.PreviewStartTime);
        }
        
        public TimeSpan PreviewDuration
        {
            get => TimeSpan.FromSeconds(MapInfo.PreviewDuration);
        }
        
        public string Directory { get; set; }

        private bool _infoLoaded;
        public BeatLevel(string directory)
        {
            Directory = Path.GetFullPath(directory);
        }

        public async Task LoadLevelInfoAsync()
        {
            if (_infoLoaded) return;
            var fileInfo = new FileInfo(Directory);

            string infoJson;
            if ((fileInfo.Attributes & FileAttributes.Directory) != 0)
            {                
                var infoPath = Path.Combine(Directory, "info.dat");
                if (!File.Exists(infoPath))
                    throw new Exception("Invalid level: no info.dat found!");

                infoJson = await File.ReadAllTextAsync(infoPath);
            }
            else if (fileInfo.Extension.ToLowerInvariant() == "zip")
            {
                var zip   = new ZipArchive(fileInfo.OpenRead(), ZipArchiveMode.Read);
                var entry = zip.GetEntry("info.dat");
                using (var e = entry.Open())
                using(var sr = new StreamReader(e))
                {
                    infoJson = await sr.ReadToEndAsync();
                }
            }
            else
            {
                throw new NotSupportedException(
                    "Level type is not a directory or zip archive containing an info.dat file.");
            }

            MapInfo = BeatMapInfo.BeatMapInfo.FromJson(infoJson);

            AvailableCharacteristics = MapInfo.DifficultyBeatmapSets.Select(d => d.BeatmapCharacteristicName).Distinct()
                .ToArray();

            AvailableDifficulties = MapInfo.DifficultyBeatmapSets.ToDictionary(ks => ks.BeatmapCharacteristicName,
                vs => vs.DifficultyBeatmaps.Select(b => b.Difficulty).ToArray());
            
            _infoLoaded = true;
        }

        public async Task<BeatMapDifficulty> LoadDifficulty(Characteristic characteristic, Difficulty difficulty)
        {
            if (!_infoLoaded)
                throw new Exception("Info not loaded.");

            var beatmapSet =
                MapInfo.DifficultyBeatmapSets.FirstOrDefault(set => set.BeatmapCharacteristicName == characteristic);
            if (beatmapSet == null)
                throw new Exception("Characteristic does not exist in level");

            var mapDifficulty = beatmapSet.DifficultyBeatmaps.FirstOrDefault(set => set.Difficulty == difficulty);
            if (mapDifficulty == null)
                throw new Exception("Difficulty does not exist in map");

            var filename = mapDifficulty.BeatmapFilename;
            return await LoadLevelDifficultyAsync(filename);
        }

        private async Task<BeatMapDifficulty> LoadLevelDifficultyAsync(string filename)
        {
            var dataPath = Path.Combine(Directory, filename);
            if (!File.Exists(dataPath))
                throw new Exception("Invalid level: no " + filename + " found!");

            var difficultyJson = await File.ReadAllTextAsync(dataPath);
            return BeatMapDifficulty.FromJson(difficultyJson);
        }
        
    }
}