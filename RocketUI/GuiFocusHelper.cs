using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RocketUI.Abstractions;
using RocketUI.Input;
using RocketUI.Input.Listeners;

namespace RocketUI
{
    public class GuiFocusHelper
    {
        private GuiManager     GuiManager     { get; }
        private GraphicsDevice GraphicsDevice { get; }
        private InputManager   InputManager   { get; }

        private ICursorInputListener CursorInputListener => InputManager.GetOrAddPlayerManager(PlayerIndex.One).CursorInputListener;

        private Viewport Viewport => GraphicsDevice.Viewport;

        private Vector2 _previousCursorPosition;
        public  Vector2 CursorPosition { get; private set; }


        private IGuiControl _highlightedElement;
        private IGuiControl _focusedElement;

        public IGuiControl HighlightedElement
        {
            get => _highlightedElement;
            set
            {
                _highlightedElement?.InvokeHighlightDeactivate();
                _highlightedElement = value;
                _highlightedElement?.InvokeHighlightActivate();
            }
        }

        public IGuiControl FocusedElement
        {
            get => _focusedElement;
            set
            {
                _focusedElement?.InvokeFocusDeactivate();
                _focusedElement = value;
                _focusedElement?.InvokeFocusActivate();
            }
        }

        private IGuiFocusContext _activeFocusContext;

        public IGuiFocusContext ActiveFocusContext
        {
            get => _activeFocusContext;
            set
            {
                if (_activeFocusContext == value) return;

                _activeFocusContext?.HandleContextInactive();
                _activeFocusContext = value;
                _activeFocusContext?.HandleContextActive();
            }
        }

        private GamePadInputListener _gamePadInputListener;

        public GuiFocusHelper(GuiManager guiManager, InputManager inputManager, GraphicsDevice graphicsDevice)
        {
            GuiManager = guiManager;
            InputManager = inputManager;
            GraphicsDevice = graphicsDevice;

            var ip = InputManager.GetOrAddPlayerManager(PlayerIndex.One);

            if (ip.TryGetListener<GamePadInputListener>(out var gamePadInputListener))
            {
                _gamePadInputListener = gamePadInputListener;
            }
            else
            {
                _gamePadInputListener = new GamePadInputListener(PlayerIndex.One);
            }
        }


        public void Update(GameTime gameTime)
        {
            UpdateHighlightedElement();
            UpdateInput();
        }

        public void OnTextInput(object sender, TextInputEventArgs args)
        {
            //if (args.Key == Keys.None) return;
            if (args.Key != Keys.None && TryGetElement(e => e is IGuiControl c && c.AccessKey == args.Key,
                out var controlByAccessKey))
            {
                if (FocusedElement != controlByAccessKey)
                {
                    FocusedElement = controlByAccessKey as IGuiControl;
                    return;
                }
            }

            if (FocusedElement == null || !FocusedElement.InvokeKeyInput(args.Character, args.Key))
            {
                if (args.Key == Keys.Tab)
                {
                    // Switch to next control
                    var activeTabIndex = FocusedElement?.TabIndex ?? -1;
                    var nextControl    = GetNextTabIndexedControl(activeTabIndex);

                    if (nextControl == null)
                    {
                        nextControl = GetNextTabIndexedControl(-1);
                    }

                    FocusedElement = nextControl;
                }
                else if (args.Key == Keys.Escape)
                {
                    // Exit focus
                    FocusedElement = null;
                }
                else
                {
                }
            }
        }

        private Vector2 _previousGamepadPosition = Vector2.Zero;

        private void UpdateHighlightedElement()
        {
            if (_gamePadInputListener != null)
            {
                var gamepadPosition = _gamePadInputListener.GetVirtualCursorPosition();

                if (gamepadPosition != _previousGamepadPosition)
                {
                    var gp = gamepadPosition.ToPoint();
                    Mouse.SetPosition(gp.X, gp.Y);

                    UpdateCursor(gamepadPosition);

                    _previousGamepadPosition = gamepadPosition;
                    return;
                }
            }


            var cursorRay = CursorInputListener.GetCursorRay();
            var screen    = FindScreen(cursorRay);
            if (screen.HasValue)
            {
                var (screen3d, rawCursorPosition) = screen.Value;
                var cursorPosition = GuiManager.GuiRenderer.Unproject(rawCursorPosition);

                if (Vector2.DistanceSquared(rawCursorPosition, _previousCursorPosition) >= 1)
                {
                    ActiveFocusContext = screen3d;
                    _previousCursorPosition = CursorPosition;
                    CursorPosition = cursorPosition;
                    UpdateCursor(CursorPosition);
                }
            }
        }

        private (IGuiScreen screen, Vector2 cursorPos)? FindScreen(Ray cursorRay)
        {
            var screens = GuiManager.Screens.ToArray();
            foreach (var screen in screens)
            {
                Matrix transform = Matrix.Identity;
                if (screen is IGuiScreen3D screen3d)
                {
                    transform = screen3d.Transform.World;
                }

                var position = Vector3.Transform(Vector3.Zero, transform);
                var normal   = Vector3.Transform(Vector3.Forward, transform);

                normal.Normalize();

                var plane        = new Plane(position, normal);
                var intersection = cursorRay.Intersects(plane);
                if (intersection.HasValue)
                {
                    // find intersectionpoint
                    var intersectionPoint = cursorRay.Position + (cursorRay.Direction * intersection.Value);

                    // unproject
                    var cursorPos = Vector3.Transform(intersectionPoint, Matrix.Invert(transform));
                    return (screen, new Vector2(cursorPos.X, cursorPos.Y));
                }
            }

            return null;
        }

        private void UpdateCursor(Vector2 cursorPosition, IGuiScreen screen = null)
        {
            IGuiControl newHighlightedElement = null;

            GuiElementPredicate predicate = (IGuiElement e) =>
                e is IGuiControl c && c.IsVisible && c.Enabled && c.CanHighlight;
            if (screen != null)
            {
                if (TryGetElementAt(screen, cursorPosition, predicate, out var controlMatchingPosition))
                    newHighlightedElement = controlMatchingPosition as IGuiControl;
            }

            else
            {
                if (TryGetElementAt(cursorPosition, predicate, out var controlMatchingPosition))
                    newHighlightedElement = controlMatchingPosition as IGuiControl;
            }

            if (newHighlightedElement != HighlightedElement)
            {
                HighlightedElement?.InvokeCursorLeave(cursorPosition);
                HighlightedElement = newHighlightedElement;
                HighlightedElement?.InvokeCursorEnter(cursorPosition);
            }
        }

        private bool _cursorDown = false;

        private void UpdateInput()
        {
            if (InputManager.Any(x => x.IsPressed(InputCommand.NavigateUp)))
            {
                if (TryFindNextControl(InputCommand.NavigateUp, out IGuiControl control))
                {
                    FocusedElement = control;
                }
            }
            else if (InputManager.Any(x => x.IsPressed(InputCommand.NavigateDown)))
            {
                if (TryFindNextControl(InputCommand.NavigateDown, out IGuiControl control))
                {
                    FocusedElement = control;
                }
            }
            else if (InputManager.Any(x => x.IsPressed(InputCommand.NavigateLeft)))
            {
                if (TryFindNextControl(InputCommand.NavigateLeft, out IGuiControl control))
                {
                    FocusedElement = control;
                }
            }
            else if (InputManager.Any(x => x.IsPressed(InputCommand.NavigateRight)))
            {
                if (TryFindNextControl(InputCommand.NavigateRight, out IGuiControl control))
                {
                    FocusedElement = control;
                }
            }

            if (HighlightedElement == null) return;

            if ((CursorInputListener.IsBeginPress(InputCommand.LeftClick) ||
                 _gamePadInputListener.IsBeginPress(InputCommand.Navigate)) && HighlightedElement.CanFocus)
            {
                FocusedElement = HighlightedElement;
            }

            var isDown = CursorInputListener.IsDown(InputCommand.LeftClick) ||
                         _gamePadInputListener.IsDown(InputCommand.Navigate);

            if (CursorPosition != _previousCursorPosition)
            {
                FocusedElement?.InvokeCursorMove(CursorPosition, _previousCursorPosition, isDown);
            }

            if (isDown)
            {
                FocusedElement?.InvokeCursorDown(CursorPosition);
            }

            if (HighlightedElement == FocusedElement && (CursorInputListener.IsPressed(InputCommand.LeftClick) ||
                                                         _gamePadInputListener.IsPressed(InputCommand.Navigate)))
            {
                FocusedElement?.InvokeCursorPressed(CursorPosition, MouseButton.Left);
            }

            if (HighlightedElement == FocusedElement && CursorInputListener.IsPressed(InputCommand.RightClick))
            {
                FocusedElement?.InvokeCursorPressed(CursorPosition, MouseButton.Right);
            }

            if (HighlightedElement == FocusedElement && CursorInputListener.IsPressed(InputCommand.MiddleClick))
            {
                FocusedElement?.InvokeCursorPressed(CursorPosition, MouseButton.Middle);
            }

            if (!isDown && _cursorDown)
            {
                FocusedElement?.InvokeCursorUp(CursorPosition);
            }

            _cursorDown = isDown;
        }

        private bool TryFindNextControl(InputCommand command, out IGuiControl control)
        {
            var focused = FocusedElement;
            if (focused == null)
            {
                if (TryGetElement(x => x is IGuiControl, out var element))
                {
                    control = (IGuiControl) element;
                    return true;
                }

                control = null;
                return false;
            }

            if (TryGetElement(
                x =>
                {
                    if (x is IGuiControl c)
                    {
                        switch (command)
                        {
                            case InputCommand.NavigateUp:
                                if (c.Position.Y > focused.Position.Y)
                                {
                                    return true;
                                }

                                break;
                            case InputCommand.NavigateDown:
                                if (c.Position.Y < focused.Position.Y)
                                {
                                    return true;
                                }

                                break;
                            case InputCommand.NavigateLeft:
                                if (c.Position.X < focused.Position.X)
                                {
                                    return true;
                                }

                                break;
                            case InputCommand.NavigateRight:
                                if (c.Position.X > focused.Position.X)
                                {
                                    return true;
                                }

                                break;
                            default:
                                return false;
                        }
                    }

                    return false;
                }, out IGuiElement el))
            {
                control = (IGuiControl) el;
                return true;
            }

            control = null;
            return false;
        }

        private bool TryFindNextControl(Vector2 scanVector, out IGuiElement nextControl)
        {
            Vector2 scan = CursorPosition + scanVector;

            while (Viewport.Bounds.Contains(scan))
            {
                if (TryGetElementAt(scan, e => true, out var matchedElement))
                {
                    if (matchedElement != HighlightedElement)
                    {
                        nextControl = matchedElement;
                        return true;
                    }
                }

                scan += scanVector;
            }

            nextControl = null;
            return false;
        }

        public bool TryGetElementAt(IGuiScreen screen, Vector2 position, GuiElementPredicate predicate,
            out IGuiElement                    element)
        {
            if (screen == null)
            {
                element = null;
                return false;
            }

            if (screen.TryFindDeepestChild(e => e.RenderBounds.Contains(position) && predicate(e),
                out var matchedChild))
            {
                element = matchedChild;
                return true;
            }

            element = null;
            return false;
        }

        public bool TryGetElementAt(Vector2 position, GuiElementPredicate predicate, out IGuiElement element)
        {
            foreach (var screen in GuiManager.Screens.ToArray().Reverse())
            {
                if (TryGetElementAt(screen, position, predicate, out element))
                    return true;
            }

            element = null;
            return false;
        }

        private bool TryGetElement(GuiElementPredicate predicate, out IGuiElement element)
        {
            foreach (var screen in GuiManager.Screens.ToArray().Reverse())
            {
                if (screen == null)
                    continue;

                if (screen.TryFindDeepestChild(predicate, out var matchedChild))
                {
                    element = matchedChild;
                    return true;
                }
            }

            element = null;
            return false;
        }

        private IGuiControl GetNextTabIndexedControl(int activeIndex)
        {
            var allControls = GuiManager.Screens
                .SelectMany(e => e.AllChildren)
                .OfType<IGuiControl>();

            return allControls.Where(c => c.TabIndex > activeIndex && activeIndex > -1)
                .OrderBy(c => c.TabIndex)
                .FirstOrDefault();
        }
    }
}