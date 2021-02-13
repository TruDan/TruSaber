using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class DrawableEntity : Entity, IDrawable
    {
        private bool _initialized;
        private bool _disposed;
        private int _drawOrder;
        private bool _visible = true;

        /// <summary>
        /// Get the <see cref="P:Microsoft.Xna.Framework.DrawableGameComponent.GraphicsDevice" /> that this <see cref="T:Microsoft.Xna.Framework.DrawableGameComponent" /> uses for drawing.
        /// </summary>
        public GraphicsDevice GraphicsDevice => this.Game.GraphicsDevice;

        public int DrawOrder
        {
            get => this._drawOrder;
            set
            {
                if (this._drawOrder == value)
                    return;
                this._drawOrder = value;
                this.OnDrawOrderChanged((object) this, EventArgs.Empty);
            }
        }

        public bool Visible
        {
            get => this._visible;
            set
            {
                if (this._visible == value)
                    return;
                this._visible = value;
                this.OnVisibleChanged((object) this, EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public event EventHandler<EventArgs> DrawOrderChanged;

        /// <inheritdoc />
        public event EventHandler<EventArgs> VisibleChanged;
        
        protected Model Model { get; set; }

        public DrawableEntity(IGame game) : base(game)
        {
        }

        public override void Initialize()
        {
            if (this._initialized)
                return;
            this._initialized = true;
            this.LoadContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (this._disposed)
                return;
            this._disposed = true;
            this.UnloadContent();
        }

        /// <summary>Load graphical resources needed by this component.</summary>
        protected virtual void LoadContent()
        {
        }

        /// <summary>Unload graphical resources needed by this component.</summary>
        protected virtual void UnloadContent()
        {
        }

        /// <summary>Draw this component.</summary>
        /// <param name="gameTime">The time elapsed since the last call to <see cref="M:Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime)" />.</param>
        public virtual void Draw(GameTime gameTime)
        {
            if(!Visible) return;
            
            if (Model != null)
            {
                var rasterBefore = GraphicsDevice.RasterizerState;
                var pencilBefore = GraphicsDevice.DepthStencilState;
                var blend        = GraphicsDevice.BlendState;
                
                GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.AlphaBlend;

                try
                {
                    var camera = ((IGame) Game).Camera;
                    Model.Draw(World, camera.View, camera.Projection);
                }
                finally
                {
                    GraphicsDevice.RasterizerState = rasterBefore;
                    GraphicsDevice.DepthStencilState = pencilBefore;
                    GraphicsDevice.BlendState = blend;
                }
            }
        }

        /// <summary>
        /// Called when <see cref="P:Microsoft.Xna.Framework.DrawableGameComponent.Visible" /> changed.
        /// </summary>
        /// <param name="sender">This <see cref="T:Microsoft.Xna.Framework.DrawableGameComponent" />.</param>
        /// <param name="args">Arguments to the <see cref="E:Microsoft.Xna.Framework.DrawableGameComponent.VisibleChanged" /> event.</param>
        protected virtual void OnVisibleChanged(object sender, EventArgs args) => EventHelpers.Raise<EventArgs>(sender, this.VisibleChanged, args);

        /// <summary>
        /// Called when <see cref="P:Microsoft.Xna.Framework.DrawableGameComponent.DrawOrder" /> changed.
        /// </summary>
        /// <param name="sender">This <see cref="T:Microsoft.Xna.Framework.DrawableGameComponent" />.</param>
        /// <param name="args">Arguments to the <see cref="E:Microsoft.Xna.Framework.DrawableGameComponent.DrawOrderChanged" /> event.</param>
        protected virtual void OnDrawOrderChanged(object sender, EventArgs args) => EventHelpers.Raise<EventArgs>(sender, this.DrawOrderChanged, args);
    }
}