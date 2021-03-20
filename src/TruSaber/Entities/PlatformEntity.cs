using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class PlatformEntity : DrawableEntity
    {
        public LightController BackLasersController          { get; }
        public LightController RingLightsController          { get; }
        public LightController LeftRotatingLasersController  { get; }
        public LightController RightRotatingLasersController { get; }
        public LightController CenterLightsController        { get; }

        public PlatformEntity(IGame game) : base(game)
        {
            BackLasersController = new LightController();
            RingLightsController = new LightController();
            LeftRotatingLasersController = new LightController();
            RightRotatingLasersController = new LightController();
            CenterLightsController = new LightController();

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Model = Game.Content.Load<Model>("Models/PlatformV3");

            foreach (var subModel in Model.Meshes)
            {
                if (subModel.Name.Contains("GlowLeft"))
                {
                    LeftRotatingLasersController.Add(subModel.Effects);
                }
                else if (subModel.Name.Contains("GlowRight"))
                {
                    RightRotatingLasersController.Add(subModel.Effects);
                }
            }

            Position = Vector3.Zero;
            Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, 180f.ToRadians());
        }
    }

    public class LightController
    {
        private List<BasicEffect> _effects;

        public LightController(List<BasicEffect> effects)
        {
            _effects = effects;
        }
        
        public LightController() : this(new List<BasicEffect>()) { }

        public void Add(ModelEffectCollection effects)
        {
            foreach (var effect in effects)
            {
                if (effect is BasicEffect basicEffect)
                {
                    _effects.Add(basicEffect);
                }
            }
        }

        public void Add(BasicEffect effect) => _effects.Add(effect);
        public void Remove(BasicEffect effect) => _effects.Remove(effect);
        public void Clear() => _effects.Clear();

        public void SetColor(Color color)
        {
            Apply(e =>
            {
                e.DiffuseColor = color.ToVector3();
                e.EmissiveColor = color.ToVector3();
                e.SpecularColor = color.ToVector3();
            });
        }

        public void SetEnabled(bool enabled)
        {
            Apply(e =>
            {
                e.SpecularPower = enabled ? 1.0f : 0f;
                e.Alpha = enabled ? 1.0f : 0f;
            });
        }

        public void Flash()
        {
            // TODO
        }

        public void FlashToBlack()
        {
            // TODO
        }

        private void Apply(Action<BasicEffect> action)
        {
            foreach (var effect in _effects)
            {
                action(effect);
            }
        }
    }
}