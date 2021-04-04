using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruSaber.Abstractions;

namespace TruSaber
{
    public enum LightType
    {
        Ambient = 0, Directional, Point, Spot
    }
    
    public class LightEntity : DrawableEntity
    {
        internal protected Vector3 _color = Color.White.ToVector3();
        private            Effect  _deferredAmbientEffect;
        private            Effect  _deferredDirLightEffect;
        private            Effect  _deferredPointLightEffect;

        /// <summary>
        /// The color of the light.
        /// </summary>
        public Color Color
        {
            get => new Color(_color);
            set { _color = value.ToVector3(); }
        }

        /// <summary>
        /// The intensity of the light.
        /// </summary>
        public float Intensity { get; set; } = 1.0f;

        /// <summary>
        /// The maximum distance of emission.
        /// </summary>
        public float Radius { get; set; } = 25;

        public float FallOf { get; set; } = 2.0f;

        /// <summary>
        /// The type of the light.
        /// </summary>
        public LightType Type { get; set; } = LightType.Directional;

        /// <summary>
        /// The angle used by the Spot light.
        /// </summary>
        public float Angle { get; set; } = MathHelper.PiOver4;
        
        public LightEntity(IGame game) : base(game)
        {
            var content = game.Game.Content;
            _deferredAmbientEffect = content.Load<Effect>("Shaders/Deferred/AmbientLight");
            _deferredDirLightEffect = content.Load<Effect>("Shaders/Deferred/DirectionalLight");
            _deferredPointLightEffect = content.Load<Effect>("Shaders/Deferred/PointLight");
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
        }
    }
}