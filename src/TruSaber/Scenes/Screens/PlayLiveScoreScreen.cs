using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Serialization.Xaml;

namespace TruSaber.Scenes.Screens
{
    public class PlayLiveScoreScreen : Screen
    {
        private readonly ScoreHelper _scoreHelper;
        private          TextElement _scoreText;
        private          TextElement _comboText;
        
        public PlayLiveScoreScreen(Game game, ScoreHelper scoreHelper) : base()
        {
            _scoreHelper = scoreHelper;
            RocketXamlLoader.Load(this);

            _scoreText = FindControl<TextElement>("ScoreText");
            _comboText = FindControl<TextElement>("ComboText");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _scoreText.Text = $"{_scoreHelper.Score:F0}";
            _comboText.Text = $"{_scoreHelper.ComboMultiplier:F0}x";
        }
    }
}