using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Controls;
using RocketUI.Layout;
using RocketUI.Primitive;
using TruSaber.Models;
using TruSaber.Services;

namespace TruSaber.Scenes
{
    public class SelectLevelScene : Scene
    {
        private GuiScreenEntity _guiScreen;

        private GuiManager       _guiManager;
        private GuiButton        _playButton;
        private LevelManager     _levelManager;
        private GuiSelectionList _selectionList
            ;

        public SelectLevelScene()
        {
            _guiScreen = new GuiScreenEntity(TruSaberGame.Instance.Game, 800, 600);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _guiManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<GuiManager>();
            _levelManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<LevelManager>();

            _guiScreen.Transform.Position = (Vector3.Forward * 7.5f) + (Vector3.Up * 2f);
            _guiScreen.Transform.Scale = new Vector3(3f);
            //_guiScreen.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 180f.ToRadians());
            
            Components.Add(_guiScreen);

            var page = new GuiStackContainer()
            {
                Orientation = Orientation.Horizontal,
                Anchor = Alignment.Fill,
                ChildAnchor = Alignment.Fill
            };
            
            _selectionList = new GuiSelectionList();
            page.AddChild(_selectionList);

            LoadSelectionList();
        }

        private void LoadSelectionList()
        {
            _levelManager.LoadLevels().ContinueWith(task =>
            {
                var levels = _levelManager.Levels;
                foreach (var level in levels)
                {
                    var levelItem = new LevelSelectGuiSelectionListItem(level);
                    _selectionList.AddChild(levelItem);
                }
            });
        }

        class LevelSelectGuiSelectionListItem : GuiSelectionListItem
        {
            public BeatLevel Level { get; }

            private GuiStackContainer _stack;
            private GuiTextElement    _title;
            private GuiTextElement    _author;

            internal LevelSelectGuiSelectionListItem(BeatLevel level)
            {
                Level = level;
                
                SetFixedSize(500, 50);
                
                AddChild(_stack = new GuiStackContainer()
                {
                    Orientation = Orientation.Vertical,
                    ChildAnchor = Alignment.TopFill
                });
                
                _stack.AddChild(_title = new GuiTextElement(level.MapInfo.SongName));
                _stack.AddChild(_author = new GuiTextElement(level.MapInfo.SongName));
            }
        }
    }
}