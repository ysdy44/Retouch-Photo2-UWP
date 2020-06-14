using Retouch_Photo2.Elements;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents an ViewModel that contains shortcut, layout and <see cref="ViewModels.Setting"/>.
    /// </summary>
    public partial class SettingViewModel : INotifyPropertyChanged
    {

        //@Construct
        /// <summary>
        /// Initializes the device-layout.
        /// </summary>
        public void ConstructDeviceLayout()
        {
            Window.Current.SizeChanged += (s, e) =>
            {
                //Width
                double width = e.Size.Width;
                this.DeviceLayoutType = this.Setting.DeviceLayout.GetActualType(width);
            };
            this.NotifyDeviceLayoutType();
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
        /// Notify <see cref="SettingViewModel.DeviceLayoutType"/>.
        /// </summary>
        public void NotifyDeviceLayoutType()
        {
            //Width
            double width = Window.Current.Bounds.Width;
            this.DeviceLayoutType = this.Setting.DeviceLayout.GetActualType(width);
        }


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
        public float CenterChildWidth
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
        public float CenterChildHeight
        {
            get
            {
                float rootHeight = (float)Window.Current.Bounds.Height;
                return rootHeight - 50;
            }
        }

    }
}