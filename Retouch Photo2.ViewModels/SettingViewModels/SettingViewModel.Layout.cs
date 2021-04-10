using Retouch_Photo2.Elements;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace Retouch_Photo2.ViewModels
{
    public partial class SettingViewModel : INotifyPropertyChanged
    {

        //@Construct
        /// <summary>
        /// Initializes the device-layout.
        /// </summary>
        public void ConstructDeviceLayout()
        {
            //Width
            DeviceLayout layout = this.Setting.DeviceLayout;
            {
                double width = Window.Current.Bounds.Width;
                DeviceLayoutType type = layout.GetActualType(width);
                this.DeviceLayoutType = type;
            }
        }
        /// <summary>
        /// Registe the device-layout.
        /// </summary>
        public void RegisteDeviceLayout()
        {
            Rect rect = Window.Current.Bounds;
            this.WindowWidth = rect.Width;
            this.WindowHeight = rect.Height;
            Window.Current.SizeChanged += this.Window_SizeChanged;
        }
        /// <summary>
        /// UnRegiste the device-layout.
        /// </summary>
        public void UnRegisteDeviceLayout()
        {
            Window.Current.SizeChanged -= this.Window_SizeChanged;
        }
        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.WindowWidth = e.Size.Width;
            this.WindowHeight = e.Size.Height;

            DeviceLayout layout = this.Setting.DeviceLayout;
            {
                double width = e.Size.Width;
                DeviceLayoutType type = layout.GetActualType(width);
                this.DeviceLayoutType = type;
            }
        }


        /// <summary> Gets or sets the device windows width. </summary>
        public double WindowWidth
        {
            get => this.windowWidth;
            set
            {
                this.windowWidth = value;
                this.OnPropertyChanged(nameof(this.WindowWidth));//Notify 
            }
        }
        private double windowWidth = 400;

        /// <summary> Gets or sets the device windows height. </summary>
        public double WindowHeight
        {
            get => this.windowHeight;
            set
            {
                this.windowHeight = value;
                this.OnPropertyChanged(nameof(this.WindowHeight));//Notify 
            }
        }
        private double windowHeight = 400;


        /// <summary> Gets or sets the device layout type. </summary>
        public DeviceLayoutType DeviceLayoutType
        {
            get => this.deviceLayoutType;
            set
            {
                this.deviceLayoutType = value;
                this.OnPropertyChanged(nameof(this.DeviceLayoutType));//Notify 
            }
        }
        private DeviceLayoutType deviceLayoutType = DeviceLayoutType.PC;


        /// <summary> Gets the center child canvas background. </summary>
        public SolidColorBrush CanvasBackground
        {
            get => this.canvasBackground;
            set
            {
                this.canvasBackground = value;
                this.OnPropertyChanged(nameof(this.CanvasBackground));//Notify 
            }
        }
        private SolidColorBrush canvasBackground;


        /// <summary>
        /// Gets the offset of full-screen statue layout.
        /// </summary>
        /// <returns></returns>
        public Vector2 FullScreenOffset
        {
            get
            {
                DeviceLayoutType type = this.DeviceLayoutType;

                switch (type)
                {
                    case DeviceLayoutType.PC:
                        return new Vector2(70, 42);
                    default:
                        return new Vector2(0, 42);
                }
            }
        }

        /// <summary> Gets the center child canvas width. </summary>
        public float CanvasWidth
        {
            get
            {
                float rootWidth = (float)Window.Current.Bounds.Width;

                DeviceLayoutType type = this.DeviceLayoutType;

                switch (type)
                {
                    case DeviceLayoutType.PC:
                        return rootWidth - 70 - 220;
                    default:
                        return rootWidth;
                }
            }
        }
        /// <summary> Gets the center child canvas height. </summary>
        public float CanvasHeight
        {
            get
            {
                float rootHeight = (float)Window.Current.Bounds.Height;
                return rootHeight - 50;
            }
        }

    }
}