using Retouch_Photo2.Elements;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Core;

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
            Window.Current.SizeChanged += this.Current_SizeChanged;
        }
        /// <summary>
        /// UnRegiste the device-layout.
        /// </summary>
        public void UnRegisteDeviceLayout()
        {
            Window.Current.SizeChanged -= this.Current_SizeChanged;
        }
        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            //Width
            DeviceLayout layout = this.Setting.DeviceLayout;
            {
                double width = e.Size.Width;
                DeviceLayoutType type = layout.GetActualType(width);
                this.DeviceLayoutType = type;
            }
        }


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
                        return new Vector2(70, 50);
                    default:
                        return new Vector2(0, 50);
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