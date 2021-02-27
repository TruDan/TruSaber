using System;
using DiscordRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using RocketUI;
using TruSaber.Graphics.Gui;
using TruSaber.Models;
using TruSaber.Services;
using Button = RocketUI.Button;

namespace TruSaber.Scenes
{
    public class SelectLevelScene : GuiSceneBase
    {
        private Button        _playButton;
        private LevelManager     _levelManager;
        private SelectionList _selectionList;
        private LevelInfo     _levelInfo;
        
        public SelectLevelScene()
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _levelManager = TruSaberGame.Instance.ServiceProvider.GetRequiredService<LevelManager>();

            var page = new MultiStackContainer()
            {
                Orientation = Orientation.Vertical,
                Anchor = Alignment.Fill,
                ChildAnchor = Alignment.TopFill,
                Width = 800,
                Height = 600
            };

            _selectionList = new SelectionList()
            {
                Background = (Color.Black * 0.2f),
                Anchor = Alignment.FillLeft,
                Height = 500,
                Width = 400
            };
            _levelInfo = new LevelInfo()
            {
                Background = (Color.Black * 0.5f),
                Anchor = Alignment.FillRight,
                Height = 500,
                Width = 400

            };
            page.AddRow(new Button("< BACK", () => TruSaberGame.Instance.SceneManager.Back()), new TextElement("LEVEL SELECT"));
            page.AddRow(_selectionList, _levelInfo);
            
            AddChild(page);

            _selectionList.SelectedItemChanged += SelectionListOnSelectedItemChanged;
            
            LoadSelectionList();
        }

        private void SelectionListOnSelectedItemChanged(object? sender, SelectionListItem e)
        {
            var item = e as LevelSelectSelectionListItem;
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
                var levelItem = new LevelSelectSelectionListItem(level);
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

        class LevelSelectSelectionListItem : SelectionListItem
        {
            public BeatLevel Level { get; }

            private MultiStackContainer _stack;
            private Image          _cover;
            private TextElement    _title;
            private TextElement    _author;

            internal LevelSelectSelectionListItem(BeatLevel level)
            {
                Level = level;
                Background = GuiTextures.PanelGlass;
                Background.RepeatMode = TextureRepeatMode.NoScaleCenterSlice;

                SetFixedSize(350, 50);


                
                AddChild(_stack = new MultiStackContainer()
                {
                    Anchor = Alignment.FillLeft,
                    Orientation = Orientation.Horizontal,
                    ChildAnchor = Alignment.TopLeft,
                    Height = 50,
                    Width = 350
                });     
                
                _stack.AddRow(_cover = new Image(level.CoverImagePath, TextureRepeatMode.ScaleToFit){ Width = 50, Height = 50, ResizeToImageSize = false, Anchor = Alignment.MiddleLeft });
                _stack.AddRow(_title = new TextElement(level.MapInfo.SongName),_author = new TextElement(level.MapInfo.SongAuthorName));
            }
        }
    }
}