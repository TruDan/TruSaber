using System.Reflection;
using Microsoft.Xna.Framework;
using RocketUI;

namespace TruSaber.Debugging
{
    public class DebugGui : Screen
    {

        private StackContainer _topleft;

        public DebugGui()
        {
            AddChild(_topleft = new StackContainer()
            {
                Anchor = Alignment.TopLeft
            });
            
            _topleft.AddChild(new TextElement($"{Assembly.GetExecutingAssembly().GetName().FullName} - Version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}")
            {
                TextColor = Color.White
            });
            
            _topleft.AddChild(new AutoUpdatingTextElement(() =>
            {
                var pos = TruSaberGame.Instance.GuiManager.FocusManager.CursorPosition;
                return $"Cursor: {pos.X.ToString("00000")}, {pos.Y.ToString("00000")}";
            })
            {
                TextColor = Color.White
            });
            
            _topleft.AddChild(new AutoUpdatingTextElement(() => $"ActiveHand: {TruSaberGame.Instance.GuiManager.FocusManager.CursorPosition.ToString()}")
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