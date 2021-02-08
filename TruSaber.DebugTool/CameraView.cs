using System.ComponentModel;
using System.Windows.Forms;
using TruSaber.Abstractions;

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
                _viewType = value;
                if (_headerText == null)
                    _headerText = _viewType.ToString();
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

        public CameraView()
        {
            InitializeComponent();
            label1.Text = _headerText;
        }


    }
}