using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RocketUI;
using TruSaber.Abstractions;

namespace TruSaber
{
    public class GuiScreenEntity : Screen, IGuiManaged, IGameComponent, IUpdateable, IDrawable, IGuiScreen3D
    {
        private readonly Game        _game;
        public           Transform3D Transform { get; } = new Transform3D();

        private float    _dotsPerMm;
        private Viewport _viewport;

        private RenderTarget2D _renderTarget;

        private Crosshair _crosshair;
        public float DotsPerInch
        {
            get => _dotsPerMm / 25.4f;
            set { _dotsPerMm = value * 25.4f; }
        }

        public GuiScreenEntity(Game game, int width, int height) : base()
        {
            _game = game;
            Background = (Color.Black * 0.2f);
            //AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClipToBounds = true;
            //Background = Color.GreenYellow;
            UpdateSize(width, height);
            _viewport = new Viewport(0, 0, Width, Height);
            _crosshair = new Crosshair();
            AddChild(_crosshair);
        }

        protected override void OnInit(IGuiRenderer renderer)
        {
            base.OnInit(renderer);
            
            _renderTarget = new RenderTarget2D(_game.GraphicsDevice, Width, Height, false, _game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            _verticies = new VertexPositionTexture[4];
            _indicies = new short[6];
            _verticies[0] = new VertexPositionTexture(new Vector3(0.5f, 0.5f, 0), new Vector2(1, 0));
            _verticies[1] = new VertexPositionTexture(new Vector3(-0.5f, 0.5f, 0), new Vector2(0, 0));
            _verticies[2] = new VertexPositionTexture(new Vector3(0.5f, -0.5f, 0), new Vector2(1, 1));
            _verticies[3] = new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0), new Vector2(0, 1));
            _indicies[0] = 0;
            _indicies[1] = 1;
            _indicies[2] = 2;
            _indicies[3] = 0;
            _indicies[4] = 2;
            _indicies[5] = 3;
            _effect = new BasicEffect(_game.GraphicsDevice);
            _effect.TextureEnabled = true;
            _effect.Texture = _renderTarget;
            //_effect.AmbientLightColor = (Color.White * 0.2f).ToVector3();
        }

        private VertexPositionTexture[] _verticies;
        private short[]                 _indicies;
        private BasicEffect             _effect;

        public void Draw(GameTime gameTime)
        {
            var game     = (IGame) _game;
            var cam      = game.Camera;
            var graphics = game.GuiManager.GuiSpriteBatch;
            
                //using (_game.GraphicsDevice.PushRenderTarget(_renderTarget))
                using (var cxt = graphics.BranchContext(BlendState.AlphaBlend, DepthStencilState.None, RasterizerState.CullNone, SamplerState.LinearClamp))
                    //using (var cxt = graphics.BranchContext())
                {
                    graphics.Effect.World = Transform.World;
                    graphics.Effect.View = cam.View;
                    graphics.Effect.Projection = cam.Projection;
                    graphics.Begin(true);

                    Draw(graphics, gameTime);
                    
                    game.GuiManager.InvokeDrawScreen(this, gameTime);
                    graphics.End();
                }
            
            // using (var cxt = graphics.BranchContext(BlendState.AlphaBlend, DepthStencilState.Default, RasterizerState.CullNone, SamplerState.AnisotropicWrap))
            // {
            //     _effect.World = Transform.World;
            //     _effect.View = cam.View;
            //     _effect.Projection = cam.Projection;
            //
            //     foreach (var pass in _effect.CurrentTechnique.Passes)
            //     {
            //         pass.Apply();
            //         _game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _verticies, 0, 2,
            //             VertexPositionTexture.VertexDeclaration);
            //     }
            // }
        }

        public void Initialize()
        {
            var game = ((IGame) _game);
            
            Init(game.GuiManager.GuiRenderer);
        }

        public bool                          Enabled     { get; } = true;
        public int                           UpdateOrder { get; } = 0;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public int                           DrawOrder { get; } = 10;
        public bool                          Visible   { get; } = true;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
    }
}
