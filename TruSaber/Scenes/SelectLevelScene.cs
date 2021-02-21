using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Controls;
using RocketUI.Graphics;
using RocketUI.Layout;
using RocketUI.Primitive;
using TruSaber.Models;
using TruSaber.Services;

namespace TruSaber.Scenes
{
    public class SelectLevelScene : GuiSceneBase
    {
        private GuiButton        _playButton;
        private LevelManager     _levelManager;
        private GuiSelectionList _selectionList;

        public SelectLevelScene()
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _levelManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<LevelManager>();

            var page = new GuiStackContainer()
            {
                Orientation = Orientation.Horizontal,
                Anchor = Alignment.Fill,
                ChildAnchor = Alignment.Fill
            };

            _selectionList = new GuiSelectionList()
            {
                Background = (Color.Black * 0.2f),
                Anchor = Alignment.Fill,
                Height = 600,
                Width = 800
            };
            //page.AddChild(_selectionList);
            AddChild(_selectionList);

            LoadSelectionList();
        }

        private void LoadSelectionList()
        {
            _levelManager.LoadLevels().GetAwaiter().GetResult();

            var levels = _levelManager.Levels;
            foreach (var level in levels)
            {
                Console.WriteLine($"adding level: {level.MapInfo.SongName}");
                var levelItem = new LevelSelectGuiSelectionListItem(level);
                _selectionList.AddChild(levelItem);
            }
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
                Background = GuiTextures.PanelGlass;
                Background.RepeatMode = TextureRepeatMode.NoScaleCenterSlice;

                SetFixedSize(500, 50);

                AddChild(_stack = new GuiStackContainer()
                {
                    Orientation = Orientation.Vertical,
                    ChildAnchor = Alignment.Fill
                });

                _stack.AddChild(_title = new GuiTextElement(level.MapInfo.SongName));
                _stack.AddChild(_author = new GuiTextElement(level.MapInfo.SongAuthorName));
            }
        }
    }
}