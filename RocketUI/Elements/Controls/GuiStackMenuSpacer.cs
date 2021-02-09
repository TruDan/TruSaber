﻿using Microsoft.Xna.Framework;
using RocketUI.Graphics;
using RocketUI.Primitive;

namespace RocketUI.Controls
{
	public class GuiStackMenuSpacer : GuiElement
	{
		public GuiStackMenuSpacer()
		{
			Margin = new Thickness(5, 5, 5, 5);
		}

		protected override void OnDraw(GuiSpriteBatch graphics, GameTime gameTime)
		{
			//	base.OnDraw(graphics, gameTime);
			graphics.DrawLine(new Vector2(RenderBounds.Left, RenderBounds.Height / 2f), RenderBounds.Width, 0f, Color.DarkGray, 1);
		}
	}
}