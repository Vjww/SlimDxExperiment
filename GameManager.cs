using System.Drawing;
using SlimDX.Direct3D9;
using SlimDX.Windows;

namespace SlimDxExperiment
{
    public class GameManager
    {
        private Device _device;
        private UIForm _form;
        private UIWindow _myWindow;
        private UIButton _myButton;

        public void Loop()
        {
            _device.Clear(ClearFlags.Target, Color.DarkSlateBlue, 1.0f, 0);
            _device.BeginScene();
            _myWindow.Render();
            _myButton.Render();
            _device.EndScene();
            _device.Present();
        }

        public void Run()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            using (_form = new UIForm())
            {
                //_form.AutoScaleMode = AutoScaleMode.Dpi;
                //_form.WindowState = FormWindowState.Maximized;
                //_form.ControlBox = false;
                //_form.MinimizeBox = false;
                //_form.MaximizeBox = false;
                //_form.ShowIcon = false;
                //_form.SizeGripStyle = SizeGripStyle.Hide;
                //_form.FormBorderStyle = FormBorderStyle.None;
                _form.Width = 1280;
                _form.Height = 720;

                var presentParams = new PresentParameters()
                {
                    //BackBufferWidth = 1280,
                    //BackBufferHeight = 720,
                    Windowed = true,
                    BackBufferWidth = _form.ClientRectangle.Width,
                    BackBufferHeight = _form.ClientRectangle.Height,
                    DeviceWindowHandle = _form.Handle,

                    //BackBufferCount = 2,
                    //SwapEffect = SwapEffect.Discard,
                    //BackBufferFormat = Format.A8R8G8B8
                };

                _device = new Device(new Direct3D(), 0, DeviceType.Hardware, _form.Handle,
                    CreateFlags.HardwareVertexProcessing, presentParams);

                _myWindow = new UIWindow(_device);
                _myWindow.LoadFromFile(@"ui\window256.bmp");
                _myWindow.Location = new Point(100, 100);
                _form.Controls.Add(_myWindow);

                _myButton = new UIButton(_device);
                _myButton.SetImage(ButtonState.Default, @"ui\buttondefault.bmp");
                _myButton.SetImage(ButtonState.Down, @"ui\buttonselected.bmp");
                _myButton.SetImage(ButtonState.Hover, @"ui\buttonhover.bmp");
                _myButton.SetImage(ButtonState.Selected, @"ui\buttonselected.bmp");
                _myButton.Location = new Point(100, 100);
                _form.Controls.Add(_myButton);

                MessagePump.Run(_form, Loop);

                _myButton.Dispose();
                _myWindow.Dispose();
                
                _device?.Dispose();
            }
        }
    }
}