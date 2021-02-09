﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RocketUI.Graphics.Textures;
using RocketUI.Graphics.Typography;

namespace RocketUI.Graphics
{
    public interface IGuiRenderer
    {
        GuiScaledResolution ScaledResolution { get; set; }
        void Init(GraphicsDevice graphics, IServiceProvider serviceProvider);

        IFont Font { get; set; }
        
        TextureSlice2D GetTexture(GuiTextures guiTexture);
        Texture2D GetTexture2D(GuiTextures guiTexture);

        string GetTranslation(string key);

        Vector2 Project(Vector2 point);
        Vector2 Unproject(Vector2 screen);
    }
}
