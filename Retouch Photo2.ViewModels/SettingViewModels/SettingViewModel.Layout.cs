using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SettingViewModel" />. 
    /// </summary>
    public partial class SettingViewModel : INotifyPropertyChanged
    {

        //@Construct
        public void ConstructDeviceLayout()
        {
            Window.Current.SizeChanged += (s, e) =>
            {
                //Width
                double width = e.Size.Width;
                this.DeviceLayoutType = this.Setting.DeviceLayout.GetActualType(width);
            };

            //Width
            double width2 = Window.Current.Bounds.Width;
            this.DeviceLayoutType = this.Setting.DeviceLayout.GetActualType(width2);
        }
        

        /// <summary> Retouch_Photo2's the only device layout type. </summary>
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
        /// <summary> Gets the CenterChild width. </summary>
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
        /// <summary> Gets the CenterChild height. </summary>
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