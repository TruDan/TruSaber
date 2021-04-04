using System;
using System.Collections.Generic;
using System.Linq;
using BeatMapInfo;
using DiscordRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using RocketUI;
using RocketUI.Utilities.Helpers;
using SharpVR;
using TruSaber.Models;
using TruSaber.Scenes.Screens;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TruSaber.Scenes
{
    public class PlayLevelScene : GuiSceneBase
    {
        public const float CountdownTime     = 5F;
        public const float TimeToScoreScreen = 5F;

        private ILogger<PlayLevelScene> _logger;
        public  BeatLevel               Level          { get; }
        public  Characteristic          Characteristic { get; }
        public  Difficulty              Difficulty     { get; }

        private List<NoteEntity> _activeNoteEntities;
        private List<WallEntity> _activeObstacles;

        private float  _positioningMultiplier;
        private float  _speed;
        private Player _player;

        private Space _space;

        private ScoreHelper _scoreHelper;

        private GuiScreenEntity _countdownScreenEntity;
        private TextElement     _countdownText;

        public PlayLevelScene(BeatLevel beatlevel, Characteristic characteristic, Difficulty difficulty)
        {
            Level = beatlevel;
            Characteristic = characteristic;
            Difficulty = difficulty;

            _logger = TruSaberGame.Instance.ServiceProvider.GetRequiredService<ILogger<PlayLevelScene>>();
            _player = TruSaberGame.Instance.Player;
            _activeNoteEntities = new List<NoteEntity>();
            _activeObstacles = new List<WallEntity>();
            _space = new Space(Level);
            _scoreHelper = new ScoreHelper();

            GuiScreen.Screen = new PlayLiveScoreScreen(TruSaberGame.Instance.Game, _scoreHelper);
            GuiScreen.Transform.LocalPosition += Vector3.Forward * 4;

            _countdownScreenEntity = new GuiScreenEntity((Game) TruSaberGame.Instance);
            _countdownScreenEntity.Transform.LocalPosition = new Vector3(-(ScreenSize.X / 2f), ScreenSize.Y, -3f);
            _countdownScreenEntity.Transform.LocalScale = new Vector3(
                (float) ScreenSize.X / GuiManager.ScaledResolution.ScaledWidth,
                (float) ScreenSize.Y / GuiManager.ScaledResolution.ScaledHeight, 1f);

            _countdownScreenEntity.Screen = new Screen();
            _countdownScreenEntity.Screen.UpdateSize(500, 500);
            _countdownText = new TextElement("...")
            {
                TextAlignment = TextAlignment.Center,
                Anchor = Alignment.MiddleCenter,
                Scale = 10F,
            };
        }

        protected override void OnInitialize()

        {
            base.OnInitialize();
            Components.Add(new PlatformEntity(TruSaberGame.Instance));

            var map = Level.LoadDifficulty(Characteristic, Difficulty).GetAwaiter().GetResult();

            InitBeatmap(map);
        }

        private BeatMapDifficulty _map;
        private TimeSpan          _mapDuration;
        private DateTime          _startTime;
        private Song              _song;

        private float _countdown;

        private void InitBeatmap(BeatMapDifficulty map)
        {
            _map = map;
            var bpm = Level.MapInfo.BeatsPerMinute;
            var bps = bpm / 60f;

            _speed = (float) (map.BeatMap?.NoteJumpMovementSpeed ?? 0f);
            _positioningMultiplier = (float) (_speed * (60f / bpm));
            
            
            foreach (var note in map.Notes)
            {
                var noteEntity = new NoteEntity(TruSaberGame.Instance, note, _positioningMultiplier);
                SpawnTrackEntity(noteEntity);
            }

            foreach (var obstacle in map.Obstacles)
            {
                var obstacleEntity = new WallEntity(TruSaberGame.Instance, obstacle, _positioningMultiplier);
                SpawnTrackEntity(obstacleEntity);
            }

            _song = Song.FromUri(Level.MapInfo.SongName, new Uri(Level.SongPath));

            var mapDuration  = TimeSpan.FromMinutes(map.Notes.Max(n => n.Time) / bpm);
            var songDuration = _song.Duration;
            _mapDuration = mapDuration > songDuration ? mapDuration : songDuration;

            InitPhysics();

            _countdown = CountdownTime;
            Components.Add(_countdownScreenEntity);
            _scoreHelper.Reset();
            _isReady = true;
        }

        private void InitPhysics()
        {
            _player.LeftHand.AddToSpace(_space);
            _player.RightHand.AddToSpace(_space);

            _bbEffect = new BasicEffect(TruSaberGame.Instance.Game.GraphicsDevice);
            _bbEffect.LightingEnabled = false;
            _bbEffect.VertexColorEnabled = true;
            _bbEffect.World = Matrix.Identity;
            //  _player.Enabled = false;
            // _bbEffect.
        }

        private bool        _isReady;
        private bool        _started;
        private int         noteIndex;
        private int         noteTotal;
        private TimeSpan    _speedOffset;
        private BasicEffect _bbEffect;

        private int  _prevSec = 6;
        private bool _finished;

        private void UpdateCountdownText()
        {
            var sec = (int) Math.Ceiling(_countdown);
            if (_prevSec != sec)
            {
                _prevSec = sec;

                // start over with new number
                _countdownText.Scale = 2F;
                _countdownText.Text = $"{sec}";
            }

            var pct = _countdown - ((int) Math.Floor(_countdown)); // 0.0 - 0.999..

            _countdownText.Scale = 2F + (8F * (1F - pct));
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (!_started && _isReady)
            {
                if (_countdown > 0)
                {
                    _countdown -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                    UpdateCountdownText();
                    return;
                }

                Start();
            }

            if (!_started) return;

            var now = DateTime.UtcNow;
            if ((now - _startTime) >= (_mapDuration + TimeSpan.FromSeconds(5)))
            {
                // end game pls
                Stop();
            }

            var playPosition = MediaPlayer.PlayPosition + _speedOffset;

            _space.Update(gameTime);

            foreach (var note in _activeNoteEntities.ToArray())
            {
                if (note.Position.Z > 0 && note.Spawned)
                {
                    _scoreHelper.RegisterMissedBlock();
                    DespawnTrackEntity(note);
                }

                if (note.Position.Z > -10)
                {
                    CheckHandCollision(_player.LeftHand, note);
                    CheckHandCollision(_player.RightHand, note);
                }
            }

            foreach (var wall in _activeObstacles.ToArray())
            {
                if (wall.BoundingBox.Min.Z > 0 && wall.BoundingBox.Max.Z > 0 && wall.Spawned)
                {
                    DespawnTrackEntity(wall);
                }

                if (wall.Position.Z > -10)
                {
                    CheckHandCollision(_player.LeftHand, wall);
                    CheckHandCollision(_player.RightHand, wall);
                    CheckHeadCollision(_player.Head, wall);
                }
            }
        }

        private void CheckHandCollision(HandEntity hand, BaseTrackEntity trackEntity)
        {
            if (trackEntity is NoteEntity note)
            {
                var intersection = hand.Ray.Intersects(note.BoundingBox);
                if (intersection.HasValue)
                {
                    if (intersection < 1.6f) // saber length of 80cm.
                    {
                        var intersectionPoint = (hand.Ray.Position + (intersection.Value * hand.Ray.Direction));
                        if (note.Type == NoteType.LeftNote && hand.Hand == Hand.Left)
                        {
                            // woohoo!!
                            //Console.WriteLine($"Left Hand hit a Left Block!!! +50 points to griffindor!");
                            _scoreHelper.RegisterHitBlock(115f);
                            DespawnTrackEntity(note);
                            return;
                        }
                        else if (note.Type == NoteType.RightNote && hand.Hand == Hand.Right)
                        {
                            // woohoo!
                            // Console.WriteLine($"Right Hand hit a Right Block!!! +50 points to griffindor!");
                            _scoreHelper.RegisterHitBlock(115f);
                            DespawnTrackEntity(note);
                            return;
                        }
                        else
                        {
                            _scoreHelper.RegisterMissedBlock();
                            return;
                        }
                    }
                }
            }
            else if (trackEntity is WallEntity wall)
            {
                if (wall.BoundingBox.Contains(hand.Position) == ContainmentType.Contains)
                {
                    // Vibrate!!
                    hand.Vibrate();
                }
            }
        }

        private void CheckHeadCollision(HeadEntity head, BaseTrackEntity trackEntity)
        {
            if (trackEntity is WallEntity wall)
            {
                if (wall.BoundingBox.Contains(head.Position) == ContainmentType.Contains)
                {
                    _scoreHelper.RegisterMissedBlock();
                }
            }
        }

        private void DrawPhysicsDebug()
        {
            var camera = TruSaberGame.Instance.Camera;
            var view   = camera.View;
            var proj   = camera.Projection;


            _bbEffect.View = view;
            _bbEffect.Projection = proj;
        }

        protected override void OnDraw(GameTime gameTime)
        {
            DrawPhysicsDebug();

            base.OnDraw(gameTime);
        }

        public override RichPresence GetPresence() =>
            new RichPresence()
            {
                State = "Playing",
                Details = Level.MapInfo.SongAuthorName + " - " + Level.MapInfo.SongName,
                Timestamps = Timestamps.Now
            };

        private void SpawnTrackEntity(BaseTrackEntity trackEntity)
        {
            trackEntity.Velocity = new Vector3(0f, 0f, _speed);
            
            Components.Add(trackEntity);
            trackEntity.AddToSpace(_space);
            if (trackEntity is NoteEntity note)
            {
                _activeNoteEntities.Add(note);
            }
        }

        private void DespawnTrackEntity(BaseTrackEntity trackEntity)
        {
            if (!trackEntity.Spawned) return;
            if (trackEntity is NoteEntity note)
            {
                _activeNoteEntities.Remove(note);
            }

            trackEntity.RemoveFromSpace(_space);
            Components.Remove(trackEntity);
        }

        private void Start()
        {
            if (_started) return;

            _countdownText.Text = "";
            Components.Remove(_countdownScreenEntity);
            MediaPlayer.Stop();
            MediaPlayer.Play(_song);
            _started = true;
            _finished = false;
            noteIndex = 0;
            noteTotal = _map.Notes.Length;
            //_activeNoteEntities.Clear();
            _speedOffset = TimeSpan.FromSeconds((1f / (float) _speed) * (60f / Level.MapInfo.BeatsPerMinute));
            _space.Start(TimeSpan.FromSeconds(Level.MapInfo.SongTimeOffset));
            _startTime = DateTime.UtcNow;
        }

        private void Stop()
        {
            if (!_started) return;
            if (_finished) return;
            _finished = true;
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
            else
            {
                // must have actually played the level, so go to the end-game
                TruSaberGame.Instance.SceneManager.SetScene(new EndLevelScene(Level, _map, _scoreHelper));
            }
        }

        protected override void OnHide()
        {
            base.OnHide();
            if (_started) Stop();
        }
    }
}