using System.Drawing;
using System.Windows.Forms;
using D3D = SlimDX.Direct3D9;
using DX = SlimDX;

namespace SlimDxExperiment
{
    /// <summary>
    /// A custom base class to encapsulate a DirectX User Interface Control
    /// that derives from System.Windows.Forms.UserControl and overrides
    /// the inherent painting functionality to allow the control to be
    /// completely transparent and can be user-drawn in a derived class.
    /// 
    /// For further information on UserControl transparency, refer to:
    /// http://bytes.com/topic/c-sharp/answers/248836-need-make-user-control-transparent
    /// http://www.codeproject.com/Articles/26878/Making-Transparent-Controls-with-C-and-NET-3-5
    /// http://stackoverflow.com/questions/2612487/how-to-fix-the-flickering-in-user-controls
    /// </summary>
    public class UIBaseControl : UserControl
    {
        private bool _disposed;

        public UITexture Texture { get; set; }
        public UISprite Sprite { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="device">Requests the DirectX device.</param>
        protected UIBaseControl(D3D.Device device)
        {
            // define control styles to enable transparency
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            BackColor = Color.Transparent;

            // create texture and sprite
            Texture = new UITexture(device);
            Sprite = new UISprite(device);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~UIBaseControl()
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
                    if (Texture != null)
                    {
                        Texture.Dispose();
                        Texture = null;
                    }
                    if (Sprite != null)
                    {
                        Sprite.Dispose();
                        Sprite = null;
                    }
                    base.Dispose(true);
                }
            }
            // dispose unmanaged resources
            _disposed = true;
        }

        /// <summary>
        /// Loads a texture from a file.
        /// </summary>
        /// <param name="filePath">Requests the file path.</param>
        /// <returns>True if the file was successfully loaded.</returns>
        public virtual bool LoadFromFile(string filePath)
        {
            // return false if unable to load texture from file
            if (!Texture.LoadFromFile(filePath))
            {
                return false;
            }

            // set control and texture attributes
            var imageInfo = D3D.ImageInformation.FromFile(filePath);
            Width = imageInfo.Width;
            Height = imageInfo.Height;
            Texture.Scaling = new DX.Vector2(1.0f, 1.0f);
            Texture.Translation = new DX.Vector2(Location.X, Location.Y);
            return true;
        }

        /// <summary>
        /// Renders the texture of the control to the screen.
        /// </summary>
        /// <returns>False if there was no texture to render.</returns>
        public virtual bool Render()
        {
            // only render the texture if the texture exists
            if (Texture.Texture != null)
            {
                Sprite.DrawTexture(Texture);
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Overrides the creation parameters of the control to set transparency.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                // get the base parameters, update ExStyle and return
                //
                // ExStyle is set to WS_EX_TRANSPARENT, which is a style
                // indicating that the control should not repainted until
                // all windows beneath it have been repainted. this is one
                // of the ways "real" transparency effects can be acheived
                var createParams = base.CreateParams;
                createParams.ExStyle |= 0x20;
                return createParams;
            }
        }

        /// <summary>
        /// Updates the translation of the texture when the control moves.
        /// </summary>
        /// <param name="e">Requests the EventArgs.</param>
        protected override void OnMove(System.EventArgs e)
        {
            Texture.Translation = new DX.Vector2(Location.X, Location.Y);
            base.OnMove(e);
        }

        /// <summary>
        /// Overrides the inherent painting functionality and does nothing.
        /// </summary>
        /// <param name="e">Requests the PaintEventArgs.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // do not paint anything
        }

        /// <summary>
        /// Overrides the inherent painting functionality and does nothing.
        /// </summary>
        /// <param name="e">Requests the PaintEventArgs.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // do not paint anything
        }
    }
}