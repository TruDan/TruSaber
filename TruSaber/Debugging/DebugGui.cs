using System.Reflection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Graphics;
using RocketUI.Layout;
using RocketUI.Primitive;

namespace TruSaber.Debugging
{
    public class DebugGui : GuiScreen
    {

        private GuiStackContainer _topleft;

        public DebugGui()
        {
            AddChild(_topleft = new GuiStackContainer()
            {
                Anchor = Alignment.TopLeft
            });
            
            _topleft.AddChild(new GuiTextElement($"{Assembly.GetExecutingAssembly().GetName().FullName} - Version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}")
            {
                TextColor = Color.White
            });
            
            _topleft.AddChild(new GuiAutoUpdatingTextElement(() =>
            {
                var pos = TruSaberGame.Instance.GuiManager.FocusManager.CursorPosition;
                return $"Cursor: {pos.X.ToString("00000")}, {pos.Y.ToString("00000")}";
            })
            {
                TextColor = Color.White
            });
            
            _topleft.AddChild(new GuiAutoUpdatingTextElement(() => $"ActiveHand: {TruSaberGame.Instance.GuiManager.FocusManager.CursorPosition.ToString()}")
            {
                TextColor = Color.White
            });
        }


        protected override void OnDraw(GuiSpriteBatch graphics, GameTime gameTime)
        {
            base.OnDraw(graphics, gameTime);
        }
    }
}