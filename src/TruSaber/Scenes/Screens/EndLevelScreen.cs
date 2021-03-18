using BeatMapInfo;
using RocketUI;
using RocketUI.Serialization.Xaml;
using TruSaber.Models;

namespace TruSaber.Scenes.Screens
{
    public class EndLevelScreen : Screen
    {
        private readonly BeatLevel         _level;
        private readonly BeatMapDifficulty _map;
        private readonly ScoreHelper       _scoreHelper;

        public EndLevelScreen() { }
        
        public EndLevelScreen(BeatLevel level, BeatMapDifficulty map, ScoreHelper scoreHelper)
        {
            _level = level;
            _map = map;
            _scoreHelper = scoreHelper;
            
            RocketXamlLoader.Load(this);

            FindControl<Button>("BackToMenu").Action =
                () => TruSaberGame.Instance.SceneManager.SetScene<SelectLevelScene>();

            FindControl<TextElement>("SongName").Text = level.MapInfo.SongName;
            FindControl<TextElement>("SongArtist").Text = level.MapInfo.SongAuthorName;
            FindControl<TextElement>("Score").Text = scoreHelper.Score.ToString("N0");
        }
    }
}