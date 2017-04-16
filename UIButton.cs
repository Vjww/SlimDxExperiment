using System;
using System.Windows.Forms;
using D3D = SlimDX.Direct3D9;
using DX = SlimDX;

namespace SlimDxExperiment
{
    /// <summary>
    /// Enumeration of button state.
    /// </summary>
    public enum ButtonState { Default, Down, Hover, Selected }

    /// <summary>
    /// A custom base class to encapsulate a DirectX User Interface Button.
    /// The control is rendered using a texture loaded from file.
    /// </summary>
    public class UIButton : UIBaseControl
    {
        private bool _disposed;

        private bool _isLeftMouseButtonDown;

        private D3D.Font _font;
        private UITexture _defaultImage;
        private UITexture _downImage;
        private UITexture _hoverImage;
        private UITexture _selectedImage;

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
            _defaultImage = new UITexture(device);
            _downImage = new UITexture(device);
            _hoverImage = new UITexture(device);
            _selectedImage = new UITexture(device);

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
                    if (_defaultImage != null)
                    {
                        _defaultImage.Dispose();
                        _defaultImage = null;
                    }
                    if (_downImage != null)
                    {
                        _downImage.Dispose();
                        _downImage = null;
                    }
                    if (_hoverImage != null)
                    {
                        _hoverImage.Dispose();
                        _hoverImage = null;
                    }
                    if (_selectedImage != null)
                    {
                        _selectedImage.Dispose();
                        _selectedImage = null;
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

                    // change state
                    //State = ButtonState.Down;
                }

                // subscribe mouse event handlers
                MouseUp += UIButton_MouseUp;
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
            State = State != ButtonState.Selected ? ButtonState.Selected : ButtonState.Default;

            _isLeftMouseButtonDown = false;
        }

        /// <summary>
        /// Responds to the MouseHover event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void UIButton_MouseHover(object sender, EventArgs e)
        {
            // handle texture change
        }

        /// <summary>
        /// Responds to the MouseEnter event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void UIButton_MouseEnter(object sender, EventArgs e)
        {
            State = ButtonState.Hover;

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
            State = ButtonState.Default;

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
                case ButtonState.Default:
                    Texture = _defaultImage;
                    break;
                case ButtonState.Down:
                    Texture = _downImage;
                    break;
                case ButtonState.Hover:
                    Texture = _hoverImage;
                    break;
                case ButtonState.Selected:
                    Texture = _selectedImage;
                    break;
                default:
                    throw new Exception("Invalid switch statement value.");
            }
            var result = base.Render();
            //_font.DrawString(null, Caption, Location.X + 10, Location.Y + 10, Color);

            return result;
        }

        public bool SetImage(ButtonState buttonState, string filePath)
        {
            D3D.ImageInformation imageInfo = D3D.ImageInformation.FromFile(filePath);
            Width = imageInfo.Width;
            Height = imageInfo.Height;

            switch (buttonState)
            {
                case ButtonState.Default:
                    if (!_defaultImage.LoadFromFile(filePath))
                        return false;
                    _defaultImage.Scaling = new DX.Vector2(1.0f, 1.0f);
                    _defaultImage.Translation = new DX.Vector2(imageInfo.Width, imageInfo.Height);
                    break;
                case ButtonState.Down:
                    if (!_downImage.LoadFromFile(filePath))
                        return false;
                    _downImage.Scaling = new DX.Vector2(1.0f, 1.0f);
                    _downImage.Translation = new DX.Vector2(imageInfo.Width, imageInfo.Height);
                    break;
                case ButtonState.Hover:
                    if (!_hoverImage.LoadFromFile(filePath))
                        return false;
                    _hoverImage.Scaling = new DX.Vector2(1.0f, 1.0f);
                    _hoverImage.Translation = new DX.Vector2(imageInfo.Width, imageInfo.Height);
                    break;
                case ButtonState.Selected:
                    if (!_selectedImage.LoadFromFile(filePath))
                        return false;
                    _selectedImage.Scaling = new DX.Vector2(1.0f, 1.0f);
                    _selectedImage.Translation = new DX.Vector2(imageInfo.Width, imageInfo.Height);
                    break;
                default:
                    throw new Exception("Invalid switch statement value.");
            }

            return true;
        }
    }
}