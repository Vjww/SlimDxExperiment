using System.Drawing;
using System.Windows.Forms;
using D3D = SlimDX.Direct3D9;

namespace SlimDxExperiment
{
    /// <summary>
    /// A custom base class to encapsulate a DirectX User Interface Window.
    /// The control is designed to move within the boundaries of the parent
    /// control and is rendered using a texture loaded from file.
    /// </summary>
    public class UIWindow : UIBaseControl
    {
        private bool _disposed;

        private bool _isLeftMouseButtonDown;
        private Point _leftMouseDownLocation;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="device">Requests the DirectX device.</param>
        public UIWindow(D3D.Device device) : base(device)
        {
            // subscribe mouse event handlers
            MouseDown += UIWindow_MouseDown;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~UIWindow()
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
        private void UIWindow_MouseDown(object sender, MouseEventArgs e)
        {
            // when the left mouse button is down, store the mouse down location and set
            // the clipping area of the mouse cursor to limit movement of the control
            if (e.Button == MouseButtons.Left)
            {
                // if mouse up event for left mouse button has not fired
                if (_isLeftMouseButtonDown)
                {
                    // restore cursor clipping area to full screen area
                    Cursor.Clip = default(Rectangle);

                    // unsubscribe mouse event handlers
                    MouseUp -= UIWindow_MouseUp;
                    MouseMove -= UIWindow_MouseMove;
                }
                else
                {
                    // store the state of the left mouse button
                    _isLeftMouseButtonDown = true;
                }

                // store the location of the mouse
                _leftMouseDownLocation = e.Location;

                // subscribe mouse event handlers
                MouseUp += UIWindow_MouseUp;
                MouseMove += UIWindow_MouseMove;

                // calculate the absolute boundaries of the parent control
                var parentTopLeft = Parent.PointToScreen(new Point(Parent.ClientRectangle.X, Parent.ClientRectangle.Y));
                var parentBottomRight = Parent.PointToScreen(new Point(Parent.ClientRectangle.Size.Width, Parent.ClientRectangle.Size.Height));
                parentBottomRight.X -= parentTopLeft.X;
                parentBottomRight.Y -= parentTopLeft.Y;

                // limit mouse cursor movement to keep the control within the
                // boundaries of the parent control and compensate for the
                // distance between the mouse and the corners of the control
                Cursor.Clip = new Rectangle(
                    parentTopLeft.X + (Width - (Width - e.X)),
                    parentTopLeft.Y + (Height - (Height - e.Y)),
                    parentBottomRight.X - (Width - (Width - e.X)) - (Width - e.X) + 1,
                    parentBottomRight.Y - (Height - (Height - e.Y)) - (Height - e.Y) + 1);
            }
        }

        /// <summary>
        /// Responds to the MouseUp event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void UIWindow_MouseUp(object sender, MouseEventArgs e)
        {
            // respond to left mouse button up
            if (_isLeftMouseButtonDown)
            {
                // restore cursor clipping area to full screen area
                Cursor.Clip = default(Rectangle);

                // unsubscribe mouse event handlers
                MouseUp -= UIWindow_MouseUp;
                MouseMove -= UIWindow_MouseMove;
            }
            _isLeftMouseButtonDown = false;
        }

        /// <summary>
        /// Responds to the MouseMove event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void UIWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // move the control with the mouse cursor when the left mouse button is down
                Location = new Point(Location.X - (_leftMouseDownLocation.X - e.X), Location.Y - (_leftMouseDownLocation.Y - e.Y));
            }
        }
    }
}