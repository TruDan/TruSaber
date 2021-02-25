using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Layout;

namespace TruSaber.Graphics.Gui
{
    public class Vector2Bars : MultiStackContainer
    {
        private Vector2 _value    = Vector2.Zero;
        private Vector2 _minValue = Vector2.Zero;
        private Vector2 _maxValue = Vector2.One;

        private ProgressBar _x;
        private TextElement _xTxt;
        private ProgressBar _y;
        private TextElement _yTxt;
        
        public Vector2Bars()
        {
            Orientation = Orientation.Vertical;
            ChildAnchor = Alignment.TopFill;
            Width = 200;
            Height = 20;
            
            AddRow(new TextElement("X") { Width = 15, Scale = 0.5f }, _x = new ProgressBar()
            {
                Height = 10,
                MinValue = -1.0f,
                MaxValue = 1.0f
            }, _xTxt = new TextElement("0")
            {
                Scale = 0.5f
            });
            AddRow(new TextElement("Y") { Width = 15, Scale = 0.5f}, _y = new ProgressBar()
            {
                Height = 10,
                MinValue = -1.0f,
                MaxValue = 1.0f
            }, _yTxt = new TextElement("0")
            {
                Scale = 0.5f
            });
        }

        
        public Vector2 MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                OnValueChanged();
            }
        }
        public Vector2 MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                OnValueChanged();
            }
        }
        
        public Vector2 Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged();
            }
        }

        private void OnValueChanged()
        {
            _x.MinValue = _minValue.X;
            _y.MinValue = _minValue.Y;
            _x.MaxValue = _maxValue.X;
            _y.MaxValue = _maxValue.Y;
            _x.Value = _value.X;
            _y.Value = _value.Y;
            _xTxt.Text = _value.X.ToString("F");
            _yTxt.Text = _value.Y.ToString("F");
        }
    }
}