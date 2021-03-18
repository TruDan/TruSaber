using BeatMapInfo;
using DiscordRPC;
using TruSaber.Models;
using TruSaber.Scenes.Screens;

namespace TruSaber.Scenes
{
    public class EndLevelScene : GuiSceneBase<EndLevelScreen>
    {
        public           BeatLevel         Level { get; }
        private readonly BeatMapDifficulty _beatMap;
        private readonly ScoreHelper       _scoreHelper;

        public EndLevelScene(BeatLevel level, BeatMapDifficulty beatMap, ScoreHelper scoreHelper) : base(new EndLevelScreen(level, beatMap, scoreHelper))
        {
            Level = level;
            _beatMap = beatMap;
            _scoreHelper = scoreHelper;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
        }

        protected override void OnShow()
        {
            base.OnShow();
        }

        public override RichPresence GetPresence()
        => new RichPresence()
            {
                State = "Playing",
                Details = Level.MapInfo.SongAuthorName + " - " + Level.MapInfo.SongName,
                Timestamps = Timestamps.Now
            };
    }
}