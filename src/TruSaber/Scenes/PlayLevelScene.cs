using System;
using System.Collections.Generic;
using BeatMapInfo;
using DiscordRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using RocketUI.Utilities.Helpers;
using SharpVR;
using TruSaber.Models;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TruSaber.Scenes
{
    public class PlayLevelScene : Scene
    {

        private ILogger<PlayLevelScene> _logger;
        public  BeatLevel               Level          { get; }
        public  Characteristic          Characteristic { get; }
        public  Difficulty              Difficulty     { get; }

        private List<NoteEntity> _activeNoteEntities;

        private float   _speed => (float) (_map.BeatMap?.NoteJumpMovementSpeed ?? 0f);
        private Player  _player;

        private Space          _space;

        private ScoreHelper _scoreHelper;

        public PlayLevelScene(BeatLevel beatlevel, Characteristic characteristic, Difficulty difficulty)
        {
            Level = beatlevel;
            Characteristic = characteristic;
            Difficulty = difficulty;
            
            _logger = TruSaberGame.Instance.ServiceProvider.GetRequiredService<ILogger<PlayLevelScene>>();
            _player = TruSaberGame.Instance.Player;
            _activeNoteEntities = new List<NoteEntity>();
            _space = new Space(Level);
            _scoreHelper = new ScoreHelper();
        }
        
        protected override void OnInitialize()

        {
            base.OnInitialize();
            Components.Add(new PlatformEntity(TruSaberGame.Instance));

            var map = Level.LoadDifficulty(Characteristic, Difficulty).GetAwaiter().GetResult();

            InitBeatmap(map);
        }

        private BeatMapDifficulty _map;

        private float _countdown;
        
        private void InitBeatmap(BeatMapDifficulty map)
        {
            _map = map;
            var bpm = Level.MapInfo.BeatsPerMinute;
            foreach (var note in map.Notes)
            {
                var noteEntity = new NoteEntity(TruSaberGame.Instance, note, (float) bpm, _speed, 0f);
                
                SpawnNote(noteEntity);
                //_noteEntities.Enqueue(noteEntity);
            }
            
            InitPhysics();

            _countdown = 5f;
            _scoreHelper.Reset();
            _isReady = true;
        }

        private void InitPhysics()
        {
            //CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(leftNoteGroup, leftNoteGroup), CollisionRule.NoSolver);
            //CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(rightNoteGroup, rightNoteGroup), CollisionRule.NoSolver);

            _player.LeftHand.AddToSpace(_space);
            _player.RightHand.AddToSpace(_space);

            _bbEffect = new BasicEffect(TruSaberGame.Instance.Game.GraphicsDevice);
            _bbEffect.LightingEnabled = false;
            _bbEffect.VertexColorEnabled = true;
            _bbEffect.World = Matrix.Identity;
            //  _player.Enabled = false;
            // _bbEffect.
        }

        /*private void EventsOnContactCreated(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            HandEntity hand;
            NoteEntity note;
            if (!(other is EntityCollidable otherEntity))
                return;

            if (sender.Entity.Tag is NoteEntity noteA && otherEntity.Entity.Tag is HandEntity handB)
            {
                note = noteA;
                hand = handB;
            }
            else if (otherEntity.Entity.Tag is NoteEntity noteB && sender.Entity.Tag is HandEntity handA)
            {
                note = noteB;
                hand = handA;
            }
            else
            {
                return;
            }

            if (
                (note.Type == NoteType.LeftNote && hand.Hand == Hand.Left)
                || (note.Type == NoteType.RightNote && hand.Hand == Hand.Right)
                || note.Type == NoteType.Bomb
            )
            {
                DespawnNote(note);
            }
        }*/

        private bool        _isReady;
        private bool        _started;
        private int         noteIndex;
        private int         noteTotal;
        private TimeSpan    _speedOffset;
        private BasicEffect _bbEffect;

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            
            if (!_started && _isReady)
            {
                if (_countdown > 0)
                {
                    _countdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    return;
                }
                
                Start();
            }
            
            if(!_started) return;

            var playPosition       = MediaPlayer.PlayPosition + _speedOffset;

            _space.Update(gameTime);

            foreach (var note in _activeNoteEntities.ToArray())
            {
                if (note.Position.Z > 0 && note.Spawned)
                {
                    _scoreHelper.RegisterMissedBlock();
                    DespawnNote(note);
                }
                if (note.Position.Z > -10)
                {
                    CheckHandCollision(_player.LeftHand, note);
                    CheckHandCollision(_player.RightHand, note);
                }
            }
        }

        private void CheckHandCollision(HandEntity hand, NoteEntity note)
        {
            var intersection = hand.Ray.Intersects(note.BoundingBox);
            if (intersection.HasValue)
            {
                if (intersection < 2f) // saber length of 80cm.
                {
                    var intersectionPoint = (hand.Ray.Position + (intersection.Value * hand.Ray.Direction));
                    if (note.Type == NoteType.LeftNote && hand.Hand == Hand.Left)
                    {
                        // woohoo!!
                        //Console.WriteLine($"Left Hand hit a Left Block!!! +50 points to griffindor!");
                        _scoreHelper.RegisterHitBlock(115f);
                        DespawnNote(note);
                    }
                    else if (note.Type == NoteType.RightNote && hand.Hand == Hand.Right)
                    {
                        // woohoo!
                       // Console.WriteLine($"Right Hand hit a Right Block!!! +50 points to griffindor!");
                       _scoreHelper.RegisterHitBlock(115f);
                        DespawnNote(note);
                    }
                    else
                    {
                        //Console.WriteLine($"Block was hit, but using the wrong hand!");
                    }
                }
                else
                {
                   //Console.WriteLine($"Block interscected but its currently too far away to do anything with it");
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
            
           // _modelDrawer.Draw(view.ToBEPU(), proj.ToBEPU());
           // _contactDrawer.Draw(_bbEffect, _space);
           // _bbDrawer.Draw(_bbEffect, _space);
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

        private void SpawnNote(NoteEntity note)
        {
            note.Velocity = new Vector3(0f, 0f, _speed);
            Components.Add(note);
            note.AddToSpace(_space);
            _activeNoteEntities.Add(note);
        //    _modelDrawer.Add(note.PhysicsEntity);
        }

        private void DespawnNote(NoteEntity note)
        {
            if(!note.Spawned) return;
            
         //   _modelDrawer.Remove(note.PhysicsEntity);
            _activeNoteEntities.Remove(note);
            note.RemoveFromSpace(_space);
            Components.Remove(note);
        }

        private void Start()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(Song.FromUri(Level.MapInfo.SongName, new Uri(Level.SongPath)));
            _started = true;
            noteIndex = 0;
            noteTotal = _map.Notes.Length;
            //_activeNoteEntities.Clear();
            _speedOffset = TimeSpan.FromSeconds((1f / (float) _speed)* (60f / Level.MapInfo.BeatsPerMinute));
            _space.Start(TimeSpan.FromSeconds(Level.MapInfo.SongTimeOffset));
        }

        private void Stop()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
        }

        protected override void OnHide()
        {
            base.OnHide();
            if (_started) Stop();
        }
    }
}