using System;
using System.Drawing;
using D3D = SlimDX.Direct3D9;
using DX = SlimDX;

namespace SlimDxExperiment
{
    /// <summary>
    /// A custom base class to encapsulate a DirectX User Interface Pen.
    /// The pen (a DirectX sprite) is used to draw a texture onto a DirectX surface.
    /// </summary>
    public class UISprite : IDisposable
    {
        private bool _disposed;

        private D3D.Device _device;
        public D3D.Sprite Sprite { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="device">Requests the DirectX graphics device.</param>
        public UISprite(D3D.Device device)
        {
            _device = device;
            Sprite = new D3D.Sprite(device);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~UISprite()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the object's resources.
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // dispose managed resources
                    if (Sprite != null)
                    {
                        Sprite.Dispose();
                        Sprite = null;
                    }
                }
            }
            // dispose unmanaged resources
            _disposed = true;
        }

        /// <summary>
        /// Draws a texture onto a DirectX surface.
        /// </summary>
        /// <param name="texture">Requests the texture to draw onto the surface.</param>
        public void DrawTexture(UITexture texture)
        {
            var matrix = DX.Matrix.Transformation2D(new DX.Vector2(0.0f, 0.0f), 0.0f,
                texture.Scaling, texture.RotationCenter, texture.Rotation, texture.Translation);

            Sprite.Begin(D3D.SpriteFlags.None);
            Sprite.Transform = matrix;
            Sprite.Draw(texture.Texture, texture.SourceRect, null, null, new DX.Color4(Color.White));
            Sprite.End();
        }
    }
}