using RocketUI.Graphics;
using RocketUI.Graphics.Textures;
using RocketUI.Primitive;

namespace RocketUI
{
    public class GuiImage : GuiElement
    {
        public bool ResizeToImageSize { get; set; }
        
        public GuiImage(GuiTextures texture, TextureRepeatMode mode = TextureRepeatMode.Stretch)
        {
            Background = texture;
            Background.RepeatMode = mode;
            ResizeToImageSize = true;
        }

        public GuiImage(NinePatchTexture2D background, TextureRepeatMode mode = TextureRepeatMode.Stretch)
        {
            Background = background;
            Background.RepeatMode = mode;
            ResizeToImageSize = true;
        }

        public GuiImage(string filepath, TextureRepeatMode mode = TextureRepeatMode.Stretch)
        {
            Background = (GuiTexture2D) filepath;
            Background.RepeatMode = mode;
            ResizeToImageSize = true;
        }

        protected override void OnInit(IGuiRenderer renderer)
        {
            base.OnInit(renderer);

            if (ResizeToImageSize)
            {
                Width = Background.ClipBounds.Width;
                Height = Background.ClipBounds.Height;
            }
        }

        protected override void GetPreferredSize(out Size size, out Size minSize, out Size maxSize)
        {
            base.GetPreferredSize(out size, out minSize, out maxSize);
            if (Background.HasValue)
            {
                size = new Size(Background.Width, Background.Height);
                size = Size.Clamp(size, minSize, maxSize);
            }
        }
    }
}
