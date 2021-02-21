using System;
using DiscordRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using RocketUI;
using RocketUI.Controls;
using RocketUI.Graphics;
using RocketUI.Layout;
using RocketUI.Primitive;
using TruSaber.Graphics.Gui;
using TruSaber.Models;
using TruSaber.Services;

namespace TruSaber.Scenes
{
    public class SelectLevelScene : GuiSceneBase
    {
        private GuiButton        _playButton;
        private LevelManager     _levelManager;
        private GuiSelectionList _selectionList;
        private GuiLevelInfo     _levelInfo;
        
        public SelectLevelScene()
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _levelManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<LevelManager>();

            var page = new GuiMultiStackContainer()
            {
                Orientation = Orientation.Vertical,
                Anchor = Alignment.Fill,
                ChildAnchor = Alignment.TopFill,
                Width = 800,
                Height = 600
            };

            _selectionList = new GuiSelectionList()
            {
                Background = (Color.Black * 0.2f),
                Anchor = Alignment.FillLeft,
                Height = 500,
                Width = 400
            };
            _levelInfo = new GuiLevelInfo()
            {
                Background = (Color.Black * 0.5f),
                Anchor = Alignment.FillRight,
                Height = 500,
                Width = 400

            };
            page.AddRow(new GuiButton("< BACK", () => TruSaberGame.Instance.SceneManager.Back()), new GuiTextElement("LEVEL SELECT"));
            page.AddRow(_selectionList, _levelInfo);
            
            AddChild(page);

            _selectionList.SelectedItemChanged += SelectionListOnSelectedItemChanged;
            
            LoadSelectionList();
        }

        private void SelectionListOnSelectedItemChanged(object? sender, GuiSelectionListItem e)
        {
            var item = e as LevelSelectGuiSelectionListItem;
            if(item == null) return;
            
            UpdateSelectedLevel(item.Level);
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


        private BeatLevel _previewNowPlaying;
        private void UpdateSelectedLevel(BeatLevel beatLevel)
        {
            _levelInfo.Level = beatLevel;
            MediaPlayer.Stop();
            MediaPlayer.Volume = 1f;
            MediaPlayer.Play(Song.FromUri(beatLevel.MapInfo.SongName, new Uri(beatLevel.SongPath)), beatLevel.PreviewStartTime);
            _previewNowPlaying = beatLevel;
        }

        protected override void OnHide()
        {
            base.OnHide();
            MediaPlayer.Stop();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            if (MediaPlayer.State == MediaState.Playing)
            {
                if (MediaPlayer.Queue.ActiveSong.Name == _previewNowPlaying.MapInfo.SongName)
                {
                    if (MediaPlayer.PlayPosition >=
                        (_previewNowPlaying.PreviewStartTime + _previewNowPlaying.PreviewDuration))
                    {
                        MediaPlayer.Volume -= (float)(0.5f * gameTime.ElapsedGameTime.TotalSeconds);
                        if (MediaPlayer.Volume == 0)
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Volume = 1f;
                        }
                    }
                }
            }
        }

        public override RichPresence GetPresence()
            => new RichPresence()
            {
                State = "In Menus",
                Details = "Selecting Song"
            };

        class LevelSelectGuiSelectionListItem : GuiSelectionListItem
        {
            public BeatLevel Level { get; }

            private GuiMultiStackContainer _stack;
            private GuiImage          _cover;
            private GuiTextElement    _title;
            private GuiTextElement    _author;

            internal LevelSelectGuiSelectionListItem(BeatLevel level)
            {
                Level = level;
                Background = GuiTextures.PanelGlass;
                Background.RepeatMode = TextureRepeatMode.NoScaleCenterSlice;

                SetFixedSize(350, 50);


                
                AddChild(_stack = new GuiMultiStackContainer()
                {
                    Anchor = Alignment.FillLeft,
                    Orientation = Orientation.Horizontal,
                    ChildAnchor = Alignment.TopLeft,
                    Height = 50,
                    Width = 350
                });     
                
                _stack.AddRow(_cover = new GuiImage(level.CoverImagePath, TextureRepeatMode.ScaleToFit){ Width = 50, Height = 50, ResizeToImageSize = false, Anchor = Alignment.MiddleLeft });
                _stack.AddRow(_title = new GuiTextElement(level.MapInfo.SongName),_author = new GuiTextElement(level.MapInfo.SongAuthorName));
            }
        }
    }
}