using System;
using System.Collections.Generic;
using System.Linq;
using BeatMapInfo;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.CollisionTests;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysicsDrawer.Lines;
using BEPUphysicsDrawer.Models;
using BEPUutilities;
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

        public PlayLevelScene() : this("C:\\Users\\truda\\Desktop\\2f1d (High Hopes - MinorSetback)")
        {
        }

        public PlayLevelScene(string level)
        {
            Level = new BeatLevel(level);
            _player = TruSaberGame.Instance.Player;

            _activeNoteEntities = new List<NoteEntity>();
            
            _parallelLooper = new ParallelLooper();
            for(int i = 0; i < Math.Min(Environment.ProcessorCount, 8); i++)
            {
                _parallelLooper.AddThread();
            }
            _space = new Space(_parallelLooper);
            _space.Solver.AllowMultithreading = false;
            _space.ForceUpdater.Enabled = true;
            _space.PositionUpdater.Enabled = true;
            _space.BoundingBoxUpdater.Enabled = true;
            _space.DeactivationManager.Enabled = false;
            //_space.Solver.IterationLimit = 1;
            
            _space.TimeStepSettings.MaximumTimeStepsPerFrame = 10;
            _space.TimeStepSettings.TimeStepDuration = 1f / 80f;
        }

        private InstancedModelDrawer _modelDrawer   = new (TruSaberGame.Instance.Game);
        private BoundingBoxDrawer    _bbDrawer      = new (TruSaberGame.Instance.Game);
        private ContactDrawer        _contactDrawer = new (TruSaberGame.Instance.Game);
        
        
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
                    (float) Level.MapInfo.SongTimeOffset);
                _noteEntities.Enqueue(noteEntity);
            }

            InitPhysics();
        }

        private void InitPhysics()
        {
            var leftNoteGroup  = new CollisionGroup();
            var rightNoteGroup = new CollisionGroup();
            
            var leftHandGroup  = new CollisionGroup();
            var rightHandGroup = new CollisionGroup();

            //CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(leftNoteGroup, leftNoteGroup), CollisionRule.NoSolver);
            //CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(rightNoteGroup, rightNoteGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(leftNoteGroup, rightNoteGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(leftHandGroup, rightNoteGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(leftHandGroup, rightHandGroup), CollisionRule.NoSolver);
            CollisionRules.CollisionGroupRules.Add(new CollisionGroupPair(leftNoteGroup, rightHandGroup), CollisionRule.NoSolver);


            foreach (var noteEntity in _noteEntities)
            {
                if (noteEntity.Type == NoteType.LeftNote)
                {
                    noteEntity.PhysicsEntity.CollisionInformation.CollisionRules.Group = leftNoteGroup;
                }
                else if (noteEntity.Type == NoteType.RightNote)
                {
                    noteEntity.PhysicsEntity.CollisionInformation.CollisionRules.Group = rightNoteGroup;
                }

                noteEntity.PhysicsEntity.CollisionInformation.Events.InitialCollisionDetected += EventsOnContactCreated;
            }

            _player.LeftHand.PhysicsEntity.CollisionInformation.CollisionRules.Group = leftHandGroup;
            _player.RightHand.PhysicsEntity.CollisionInformation.CollisionRules.Group = rightHandGroup;
            
            _player.LeftHand.PhysicsEntity.CollisionInformation.Events.InitialCollisionDetected += EventsOnContactCreated;
            _player.RightHand.PhysicsEntity.CollisionInformation.Events.InitialCollisionDetected += EventsOnContactCreated;
            
            _player.LeftHand.AddToSpace(_space);
            _player.RightHand.AddToSpace(_space);
            _space.ForceUpdater.Gravity = new BEPUutilities.Vector3();

            
            _bbEffect = new BasicEffect(TruSaberGame.Instance.Game.GraphicsDevice);
            _bbEffect.LightingEnabled = false;
            _bbEffect.VertexColorEnabled = true;
            _bbEffect.World = Matrix.Identity;
        }

        private void EventsOnContactCreated(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
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
        }

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

            var spawn = true;
            while (spawn)
            {
                if (_noteEntities.TryPeek(out var note))
                {
                    if(note.DueTime > playPosition)
                        break;

                    if (!_noteEntities.TryDequeue(out note))
                        break;
                    
                    if (!note.HasSpawnedAtLeastOnce)
                    {
                        SpawnNote(note);
                    }
                }
            }
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
//            _space.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _space.TimeStepSettings.TimeStepDuration = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _space.Update(dt);

            _modelDrawer.Update();
                        
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
            _contactDrawer.Draw(_bbEffect, _space);
            _bbDrawer.Draw(_bbEffect, _space);
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
            _modelDrawer.Add(note.PhysicsEntity);
        }

        private void DespawnNote(NoteEntity note)
        {
            if(!note.Spawned) return;
            
            _modelDrawer.Remove(note.PhysicsEntity);
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