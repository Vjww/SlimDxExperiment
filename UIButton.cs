using System;
using System.Windows.Forms;
using D3D = SlimDX.Direct3D9;
using DX = SlimDX;

namespace SlimDxExperiment
{
    /// <summary>
    /// Enumeration of button state.
    /// </summary>
    public enum ButtonState { OffNone, OffFocus, OffHover, OffDown, OnNone, OnFocus, OnHover, OnDown }

    /// <summary>
    /// A custom base class to encapsulate a DirectX User Interface Button.
    /// The control is rendered using a texture loaded from file.
    /// </summary>
    public class UIButton : UIBaseControl
    {
        private bool _disposed;

        private bool _isLeftMouseButtonDown;
        private bool _isSelected;

        private D3D.Font _font;

        private UITexture _offNoneTexture;
        private UITexture _offFocusTexture;
        private UITexture _offHoverTexture;
        private UITexture _offDownTexture;
        private UITexture _onNoneTexture;
        private UITexture _onFocusTexture;
        private UITexture _onHoverTexture;
        private UITexture _onDownTexture;

        public string Caption { get; set; }
        public DX.Color4 Color { get; set; }
        public ButtonState State { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="device">Requests the DirectX device.</param>
        public UIButton(D3D.Device device) : base(device)
        {
            Caption = "Caption";
            Color = new DX.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            System.Drawing.Font systemfont = new System.Drawing.Font("Arial", 12f, System.Drawing.FontStyle.Regular);
            _font = new D3D.Font(device, systemfont);
            systemfont.Dispose();
            _offNoneTexture = new UITexture(device);
            _offFocusTexture = new UITexture(device);
            _offHoverTexture = new UITexture(device);
            _offDownTexture = new UITexture(device);
            _onNoneTexture = new UITexture(device);
            _onFocusTexture = new UITexture(device);
            _onHoverTexture = new UITexture(device);
            _onDownTexture = new UITexture(device);

            // subscribe mouse event handlers
            MouseEnter += UIButton_MouseEnter;
            MouseDown += UIButton_MouseDown;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~UIButton()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of the object's resources.
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // dispose managed resources
                    if (_font != null)
                    {
                        _font.Dispose();
                        _font = null;
                    }
                    if (_offNoneTexture != null)
                    {
                        _offNoneTexture.Dispose();
                        _offNoneTexture = null;
                    }
                    if (_offFocusTexture != null)
                    {
                        _offFocusTexture.Dispose();
                        _offFocusTexture = null;
                    }
                    if (_offHoverTexture != null)
                    {
                        _offHoverTexture.Dispose();
                        _offHoverTexture = null;
                    }
                    if (_offDownTexture != null)
                    {
                        _offDownTexture.Dispose();
                        _offDownTexture = null;
                    }
                    if (_onNoneTexture != null)
                    {
                        _onNoneTexture.Dispose();
                        _onNoneTexture = null;
                    }
                    if (_onFocusTexture != null)
                    {
                        _onFocusTexture.Dispose();
                        _onFocusTexture = null;
                    }
                    if (_onHoverTexture != null)
                    {
                        _onHoverTexture.Dispose();
                        _onHoverTexture = null;
                    }
                    if (_onDownTexture != null)
                    {
                        _onDownTexture.Dispose();
                        _onDownTexture = null;
                    }

                    MouseEnter -= UIButton_MouseEnter;
                    MouseDown -= UIButton_MouseDown;
                    base.Dispose(true);
                }
            }
            // dispose unmanaged resources
            _disposed = true;
        }

        /// <summary>
        /// Responds to the MouseDown event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void UIButton_MouseDown(object sender, MouseEventArgs e)
        {
            // when the left mouse button is down, change the button texture
            if (e.Button == MouseButtons.Left)
            {
                // if mouse up event for left mouse button has not fired
                if (_isLeftMouseButtonDown)
                {
                    // unsubscribe mouse event handlers
                    MouseUp -= UIButton_MouseUp;
                }
                else
                {
                    // store the state of the left mouse button
                    _isLeftMouseButtonDown = true;
                }

                // subscribe mouse event handlers
                MouseUp += UIButton_MouseUp;

                // change state
                State = !_isSelected ? ButtonState.OnDown : ButtonState.OffDown;
            }
        }

        /// <summary>
        /// Responds to the MouseUp event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void UIButton_MouseUp(object sender, MouseEventArgs e)
        {
            // respond to left mouse button up
            if (_isLeftMouseButtonDown)
            {
                // unsubscribe mouse event handlers
                MouseUp -= UIButton_MouseUp;
            }

            // change state
            _isSelected = !_isSelected;
            State = !_isSelected ? ButtonState.OffNone : ButtonState.OnNone;
            _isLeftMouseButtonDown = false;
        }

        /// <summary>
        /// Responds to the MouseHover event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void UIButton_MouseHover(object sender, EventArgs e)
        {
            State = !_isSelected ? ButtonState.OffHover : ButtonState.OnHover;
        }

        /// <summary>
        /// Responds to the MouseEnter event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void UIButton_MouseEnter(object sender, EventArgs e)
        {
            State = !_isSelected ? ButtonState.OffHover : ButtonState.OnHover;

            // subscribe mouse event handlers
            MouseLeave += UIButton_MouseLeave;
        }

        /// <summary>
        /// Responds to the MouseLeave event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void UIButton_MouseLeave(object sender, EventArgs e)
        {
            State = !_isSelected ? ButtonState.OffNone : ButtonState.OnNone;

            // unsubscribe mouse event handlers
            MouseLeave -= UIButton_MouseLeave;
        }

        /// <summary>
        /// Loads a texture from a file.
        /// </summary>
        /// <param name="filePath">Requests the file path.</param>
        /// <returns>True if the file was successfully loaded.</returns>
        public override bool LoadFromFile(string filePath)
        {
            throw new Exception("UIButton class does not use LoadFromFile method. Use SetTexture method to load textures.");
        }

        public override bool Render()
        {
            switch (State)
            {
                case ButtonState.OffNone:
                    Texture = _offNoneTexture;
                    break;
                case ButtonState.OffFocus:
                    Texture = _offFocusTexture;
                    break;
                case ButtonState.OffHover:
                    Texture = _offHoverTexture;
                    break;
                case ButtonState.OffDown:
                    Texture = _offDownTexture;
                    break;
                case ButtonState.OnNone:
                    Texture = _onNoneTexture;
                    break;
                case ButtonState.OnFocus:
                    Texture = _onFocusTexture;
                    break;
                case ButtonState.OnHover:
                    Texture = _onHoverTexture;
                    break;
                case ButtonState.OnDown:
                    Texture = _onDownTexture;
                    break;
                default:
                    throw new Exception("Invalid switch statement value.");
            }
            var result = base.Render();
            _font.DrawString(null, Caption, Location.X + 10, Location.Y + 10, Color);

            return result;
        }

        public bool SetTexture(ButtonState buttonState, string filePath)
        {
            switch (buttonState)
            {
                case ButtonState.OffNone:
                    return LoadAndConfigureTexture(_offNoneTexture, filePath);
                case ButtonState.OffFocus:
                    return LoadAndConfigureTexture(_offFocusTexture, filePath);
                case ButtonState.OffHover:
                    return LoadAndConfigureTexture(_offHoverTexture, filePath);
                case ButtonState.OffDown:
                    return LoadAndConfigureTexture(_offDownTexture, filePath);
                case ButtonState.OnNone:
                    return LoadAndConfigureTexture(_onNoneTexture, filePath);
                case ButtonState.OnFocus:
                    return LoadAndConfigureTexture(_onFocusTexture, filePath);
                case ButtonState.OnHover:
                    return LoadAndConfigureTexture(_onHoverTexture, filePath);
                case ButtonState.OnDown:
                    return LoadAndConfigureTexture(_onDownTexture, filePath);
                default:
                    throw new Exception("Invalid switch statement value.");
            }
        }

        private bool LoadAndConfigureTexture(UITexture texture, string filePath)
        {
            var imageInfo = D3D.ImageInformation.FromFile(filePath);
            //Width = imageInfo.Width;
            //Height = imageInfo.Height;

            if (!texture.LoadFromFile(filePath))
            {
                return false;
            }

            texture.Scaling = new DX.Vector2(1.0f, 1.0f);
            texture.Translation = new DX.Vector2(imageInfo.Width, imageInfo.Height);

            return true;
        }
    }
}