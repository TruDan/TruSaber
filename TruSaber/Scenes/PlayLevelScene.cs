using System;
using System.Collections.Generic;
using System.Linq;
using BeatMapInfo;
using BEPUutilities.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Collisions;
using SharpVR;
using TruSaber.Models;
using TruSaber.Utilities.Extensions;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TruSaber.Scenes
{
    public class PlayLevelScene : Scene
    {
        private double    _beat;
        public  BeatLevel Level { get; set; }

        private Queue<NoteEntity> _noteEntities;
        private List<NoteEntity> _activeNoteEntities;

        private Beatmap _beatmap;
        private float   _speed => (float) (_beatmap?.NoteJumpMovementSpeed ?? 0f);
        private string  _songPath;
        private Player  _player;

        private ParallelLooper _parallelLooper;
        private Space          _space;

        public PlayLevelScene() : this("/home/kenny/Desktop/TruSaber/Test Levels/2f1d (High Hopes - MinorSetback)")
        {
        }

        public PlayLevelScene(string level)
        {
            Level = new BeatLevel(level);
            _player = TruSaberGame.Instance.Player;

            _activeNoteEntities = new List<NoteEntity>();
            
            // _parallelLooper = new ParallelLooper();
            // for(int i = 0; i < Math.Min(Environment.ProcessorCount, 8); i++)
            // {
            //     _parallelLooper.AddThread();
            // }
            // _space = new Space(_parallelLooper);
            _space = new Space(Level);
            //_space.Solver.IterationLimit = 1;

        }
        
        protected override void OnInitialize()

        {
            base.OnInitialize();
            Components.Add(new PlatformEntity(TruSaberGame.Instance));

            Level.LoadLevelInfoAsync().GetAwaiter().GetResult();
            var selectedDifficulty = Level.AvailableDifficulties.FirstOrDefault();
            var map = Level.LoadDifficulty(selectedDifficulty.Key, selectedDifficulty.Value.LastOrDefault()).GetAwaiter()
                .GetResult();

            var bpm = Level.MapInfo.BeatsPerMinute;
            _beatmap = Level.MapInfo.DifficultyBeatmapSets
                .FirstOrDefault(d => d.BeatmapCharacteristicName == selectedDifficulty.Key).DifficultyBeatmaps
                .FirstOrDefault(b => b.Difficulty == selectedDifficulty.Value.LastOrDefault());

            _noteEntities = new Queue<NoteEntity>();
            foreach (var note in map.Notes)
            {
                var noteEntity = new NoteEntity(TruSaberGame.Instance, note, (float) bpm, _speed,
                    0f);
                
                SpawnNote(noteEntity);
                //_noteEntities.Enqueue(noteEntity);
            }

            _space.Start(TimeSpan.FromMilliseconds(Level.MapInfo.SongTimeOffset));

            InitPhysics();
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

        private bool        _started;
        private int         noteIndex;
        private int         noteTotal;
        private TimeSpan    _speedOffset;
        private BasicEffect _bbEffect;

        protected override void OnUpdate(GameTime gameTime)
        {
            if (!_started)
            {
                Start();
            }

            var playPosition       = MediaPlayer.PlayPosition + _speedOffset;

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
          //  _space.TimeStepSettings.TimeStepDuration = (float) gameTime.ElapsedGameTime.TotalSeconds;
         //   try
          //  {
         //       _space.Update(dt);
         //   }
         //   catch {}

         
            _space.Update(gameTime);
            base.OnUpdate(gameTime);

            //
            // foreach (var note in _noteEntities)
            // {
            //     if (note.Position.Z > 10f)
            //     {
            //         DespawnNote(note);
            //     }
            //
            //     if (!note.Visible && note.Enabled && note.Position.Z >= -_speed)
            //     {
            //         note.Visible = true;
            //         //_space.Add(note.PhysicsEntity);
            //     }
            //
            //     if (note.Type == NoteType.LeftNote)
            //     {
            //         
            //     }
            // }
            //
            //
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
            noteTotal = _noteEntities.Count;
            _activeNoteEntities.Clear();
            _speedOffset = TimeSpan.FromSeconds((1f / (float) _speed)* (60f / Level.MapInfo.BeatsPerMinute));
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
            _parallelLooper.Dispose();
        }
    }
}