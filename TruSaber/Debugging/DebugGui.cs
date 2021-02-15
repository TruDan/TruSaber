using System.Reflection;
using Microsoft.Xna.Framework;
using RocketUI;
using RocketUI.Graphics;
using RocketUI.Layout;

namespace TruSaber.Debugging
{
    public class DebugGui : GuiScreen
    {

        private GuiStackContainer _topleft;

        public DebugGui()
        {
            AddChild(_topleft = new GuiStackContainer());
            _topleft.AddChild(new GuiTextElement($"{Assembly.GetExecutingAssembly().GetName().FullName} - Version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}"));
                        
        }


        protected override void OnDraw(GuiSpriteBatch graphics, GameTime gameTime)
        {
            base.OnDraw(graphics, gameTime);
        }
    }
}