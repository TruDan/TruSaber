using Microsoft.Xna.Framework;

namespace RocketUI
{
    public class GuiTextureElement : GuiElement
    {
        public TextureSlice2D Texture { get; set; }
        public TextureRepeatMode RepeatMode { get; set; } = TextureRepeatMode.Stretch;
        public GuiTextureElement()
        {

        }

        protected override void OnDraw(GuiSpriteBatch graphics, GameTime gameTime)
        {
            base.OnDraw(graphics, gameTime);

            if (Texture != null)
            {
                graphics.FillRectangle(RenderBounds, Texture, RepeatMode);
            }
        }
    }
}