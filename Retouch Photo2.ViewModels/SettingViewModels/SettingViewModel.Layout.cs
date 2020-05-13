using Retouch_Photo2.Elements;
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
        public void ConstructLayout()
        {
            Window.Current.SizeChanged += (s, e) =>
            {
                double width = e.Size.Width;

                this.DeviceLayoutType = this.DeviceLayout.GetActualType(width);
            };

            {
                double width = Window.Current.Bounds.Width;

                this.DeviceLayoutType = this.DeviceLayout.GetActualType(width);
            }
        }


        /// <summary> Retouch_Photo2's the only Theme. </summary>
        public ElementTheme Theme
        {
            get => this.theme;
            set
            {
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    if (frameworkElement.RequestedTheme != value)
                    {
                        frameworkElement.RequestedTheme = value;
                    }
                }

                this.theme = value;
                this.OnPropertyChanged(nameof(this.Theme));//Notify 
            }
        }
        private ElementTheme theme = ElementTheme.Default;


        /// <summary> Retouch_Photo2's the only device layout. </summary>
        public DeviceLayout DeviceLayout
        {
            get => this.deviceLayout;
            set
            {
                this.deviceLayout = value;
                this.OnPropertyChanged(nameof(this.DeviceLayout));//Notify 
            }
        }
        private DeviceLayout deviceLayout = DeviceLayout.Default;
        
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
                if (this.DeviceLayout.IsAdaptive)
                {
                    if (this.DeviceLayoutType == DeviceLayoutType.PC)
                        return new Vector2(70, 50);
                }

                if (this.DeviceLayout.FallBackType == DeviceLayoutType.PC)
                    return new Vector2(70, 50);

                return new Vector2(0, 50);
            }
        }
        /// <summary> Gets the CenterChild width. </summary>
        public float CenterChildWidth
        {
            get
            {
                float rootWidth = (float)Window.Current.Bounds.Width;

                if (this.DeviceLayout.IsAdaptive)
                {
                    switch (this.DeviceLayoutType)
                    {
                        case DeviceLayoutType.PC:
                            return rootWidth - 70 - 220;
                    }
                }
                
                switch (this.DeviceLayout.FallBackType)
                {
                    case DeviceLayoutType.PC:
                        return rootWidth - 70 - 220;
                }

                return rootWidth;
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