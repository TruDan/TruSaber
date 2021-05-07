using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BeatMapInfo;
using BeatMapInfo.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using TruSaber.Configuration;
using TruSaber.Models;
using TruSaber.Services;

namespace LevelAnalyzer
{
    class Program
    {
        class Info
        {
            public BeatLevel                          Level              { get; set; }
            public ReadOnlyDictionary<EventType, int> EventsCountPerType { get; set; }

            public Info(BeatLevel level, IDictionary<EventType, int> eventsCountPerType)
            {
                Level = level;
                EventsCountPerType = new ReadOnlyDictionary<EventType, int>(eventsCountPerType);
            }
        }

        class PartialBeatmapDifficulty
        {
            public Event[] Events { get; set; }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var lvlMgr = new LevelManager(new GameOptions()
            {
                LevelDirectory =
                    @"C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels"
            });

            await lvlMgr.LoadLevels();

            var tasks = lvlMgr.Levels.Select(GetInfo).ToList();

            var results = await Task.WhenAll(tasks);

            Console.WriteLine($"Loaded {_successCount} levels, ({_failedCount} failed)");

            var typesICareAbout = new[]
            {
                EventType.ControlBackLasers, EventType.ControlRingLights, EventType.ControlLeftRotatingLasers,
                EventType.ControlRightRotatingLasers, EventType.ControlCenterLights, EventType.ControlBoostLights
            };

            var orderedResults = results.OrderByDescending(r =>
                    r?.EventsCountPerType
                        .Where(t => typesICareAbout.Contains(t.Key))
                        .Sum(t => t.Value) ?? int.MinValue)
                .Take(10)
                .ToArray();
            var a = "b";


            foreach (var or in orderedResults)
            {
                Console.WriteLine(
                    $"{or.EventsCountPerType.Sum(vs => vs.Value)}  Events - {string.Join(", ", or.EventsCountPerType.Select(kvp => $"{kvp.Key.ToString()} = {kvp.Value}"))}| Map {or.Level.MapInfo.SongName}");
                
                // copy to Out folder
                Console.WriteLine(or.Level.Directory);
                Process.Start("explorer.exe", or.Level.Directory);
                //CopyFilesRecursively(or.Level.Directory, Path.Combine(@"C:\Users\truda\Desktop\Out", Path.GetDirectoryName(or.Level.Directory)));
            }


            Console.ReadLine();
        }
        
        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        
        private static int    _consoleWidth     = 0;
        private static object _consoleWidthLock = new object();
        private static int    _successCount     = 0;
        private static int    _failedCount      = 0;

        private static JsonSerializer _jsonSerializer = new JsonSerializer()
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
            {
                NamingStrategy = new BeatSaberNamingStrategy()
            },
            Converters =
            {
                new StringEnumConverter(new DefaultNamingStrategy(), false),
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            },
        };

        private static async Task<Info> GetInfo(BeatLevel lvl)
        {
            try
            {
                Difficulty[]               mapDs;
                Difficulty                 hardest;
                string                     path;
                Dictionary<EventType, int> eventsCountPerType;
                if (lvl.AvailableDifficulties.TryGetValue(Characteristic.Lightshow, out mapDs))
                {
                    hardest = mapDs.OrderByDescending(m => m).First();
                    path = Path.Combine(lvl.Directory,
                        lvl.MapInfo.DifficultyBeatmapSets
                            .First(dbs => dbs.BeatmapCharacteristicName == Characteristic.Lightshow)
                            .DifficultyBeatmaps
                            .First(db => db.Difficulty == hardest).BeatmapFilename);
                }
                else if (lvl.AvailableDifficulties.TryGetValue(Characteristic.Standard, out mapDs))
                {
                    hardest = mapDs.OrderByDescending(m => m).First();
                    path = Path.Combine(lvl.Directory,
                        lvl.MapInfo.DifficultyBeatmapSets
                            .First(dbs => dbs.BeatmapCharacteristicName == Characteristic.Standard)
                            .DifficultyBeatmaps
                            .First(db => db.Difficulty == hardest).BeatmapFilename);
                }
                else
                {
                    throw new Exception("");
                }


                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (var sr = new StreamReader(fs))
                using (var jr = new JsonTextReader(sr))
                {
                    eventsCountPerType = _jsonSerializer.Deserialize<PartialBeatmapDifficulty>(jr)?.Events
                        .GroupBy(ks => ks.Type)
                        .ToDictionary(ks => ks.Key, vs => vs.Count());
                }


                Interlocked.Increment(ref _successCount);
                lock (_consoleWidthLock)
                {
                    _consoleWidth++;
                    Console.Write(".");

                    if (_consoleWidth >= 80)
                    {
                        Console.WriteLine();
                        _consoleWidth = 0;
                    }
                }

                return new Info(lvl, eventsCountPerType);
            }
            catch (Exception e)
            {
            }

            lock (_consoleWidthLock)
            {
                _consoleWidth++;
                Console.Write("X");

                if (_consoleWidth >= 80)
                {
                    Console.WriteLine();
                    _consoleWidth = 0;
                }
            }

            Interlocked.Increment(ref _failedCount);
            return null;
        }
    }
}