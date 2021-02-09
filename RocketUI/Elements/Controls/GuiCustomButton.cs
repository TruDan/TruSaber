using System;
using Microsoft.Xna.Framework;
using RocketUI.Graphics;

namespace RocketUI.Controls
{
	public class GuiCustomButton : GuiButton
	{
		//public Color OutlineColor { get; set; } = Color.Transparent;
		//public Thickness OutlineSize { get; set; } = new Thickness(1);
		public GuiCustomButton(string text, Action action = null) : base(text, action)
		{

		}

		protected override void OnModernChanged(bool oldValue, bool newValue)
		{
			
		}

		protected override void OnEnabledChanged()
		{
			if (!Enabled && Modern)
			{
				TextElement.TextColor = Color.Blue;
			}
		}

		protected override void OnDraw(GuiSpriteBatch graphics, GameTime gameTime)
		{
			base.OnDraw(graphics, gameTime);

			//if (OutlineColor == Color.Transparent || OutlineSize == Thickness.Zero) return;
			
			//graphics.DrawRectangle(Bounds, OutlineColor, OutlineSize);
		}
	}
}
