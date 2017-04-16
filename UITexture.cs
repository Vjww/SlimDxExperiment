using System;
using System.Drawing;
using D3D = SlimDX.Direct3D9;
using DX = SlimDX;

namespace SlimDxExperiment
{
    /// <summary>
    /// A custom base class to encapsulate a DirectX User Interface Texture.
    /// </summary>
    public class UITexture : IDisposable
    {
        private bool _disposed;

        private readonly D3D.Device _device;
        public float Rotation { get; set; }
        public DX.Vector2 RotationCenter { get; set; }
        public DX.Vector2 Scaling { get; set; }
        public Rectangle? SourceRect { get; set; }
        public D3D.Texture Texture { get; private set; }
        public DX.Vector2 Translation { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="device">Requests the DirectX graphics device.</param>
        public UITexture(D3D.Device device)
        {
            _device = device;
            RotationCenter = new DX.Vector2();
            Scaling = new DX.Vector2();
            Translation = new DX.Vector2();
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~UITexture()
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
                    if (Texture != null)
                    {
                        Texture.Dispose();
                        Texture = null;
                    }
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
        public bool LoadFromFile(string filePath)
        {
            // dispose of existing texture
            if (Texture != null)
            {
                Texture.Dispose();
                Texture = null;
            }

            // load texture from file
            Texture = D3D.Texture.FromFile(_device, filePath);

            // if load failed return false
            return Texture != null;
        }
    }
}