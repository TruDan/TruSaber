using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using TruSaber.Abstractions;
using TruSaber.Graphics;

namespace TruSaber.DebugTool
{
    public enum CameraViewType
    {
        Top = 0,
        Right = 1,
        Front = 2,
        FirstPerson = 3
    }

    [DesignTimeVisible]
    public partial class CameraView : UserControl
    {
        /// <summary>
        /// Orientation of this camera
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(CameraViewType.Top)]
        public CameraViewType ViewType
        {
            get => _viewType;
            set
            {
                if (_viewType != value)
                {
                    _viewType = value;
                    if (_headerText == null)
                        _headerText = _viewType.ToString();
                    UpdateView();
                }
            }
        }


        private string _headerText;
        private CameraViewType _viewType;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(null)]
        public string HeaderText
        {
            get => _headerText;
            set
            {
                _headerText = value ?? ViewType.ToString();
                if (label1 != null)
                    label1.Text = _headerText;
            }
        }
        
        [Browsable(true)]
        public Camera Camera { get; set; }

        public CameraView()
        {
            InitializeComponent();
            label1.Text = _headerText;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRenderTarget();
        }

        private void UpdateRenderTarget()
        {
            if(DesignMode) return;

            var b = Bounds;
            new 
            Camera.RenderTarget = new RenderTarget2D(TruSaberGame.Instance.Game.GraphicsDevice, b.Width, b.Height);
        }
        
        private void UpdateView()
        {
            if(DesignMode) return;
            
            if (Camera == null)
            {
                Camera = new Camera(TruSaberGame.Instance);
                UpdateRenderTarget();
                TruSaberGame.Instance.Cameras.Add(Camera);
            }
            
            switch (ViewType)
            {
                case CameraViewType.Top:
                case CameraViewType.Right:
                case CameraViewType.Front:
                    Camera.ProjectionType = ProjectionType.Orthographic;
                    break;
                case CameraViewType.FirstPerson:
                    Camera.ProjectionType = ProjectionType.Perspective;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}