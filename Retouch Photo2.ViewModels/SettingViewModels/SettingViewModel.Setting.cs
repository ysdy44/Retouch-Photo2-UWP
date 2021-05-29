using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.ViewModels
{
    public partial class SettingViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the setting. </summary>
        public Setting Setting { get; set; } = new Setting();


        //@Construct
        /// <summary>
        /// Initializes the all settings.
        /// </summary>
        public void ConstructSetting(Setting setting)
        {
            if (setting != null)
            {
                this.Setting = setting;
            }

            // Theme
            this.ConstructTheme();

            // DeviceLayout
            this.ConstructDeviceLayout();
            this.RegisteDeviceLayout();

            // CanvasBackground
            this.ConstructCanvasBackground();

            // DeviceLayout
            this.ConstructLayersHeight();
        }


        //@Construct
        /// <summary>
        /// Initializes the theme.
        /// </summary>
        public void ConstructTheme()
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.RequestedTheme != this.Setting.Theme)
                {
                    frameworkElement.RequestedTheme = this.Setting.Theme;
                }
            }
        }

        //@Construct
        /// <summary>
        /// Initializes the canvas background.
        /// </summary>
        public void ConstructCanvasBackground()
        {
            if (this.Setting.CanvasBaclground is byte cannel)
            {
                Color color = Color.FromArgb(255, cannel, cannel, cannel);
                this.CanvasBackground = new SolidColorBrush(color);
            }
            else
            {
                this.CanvasBackground = null;
            }
        }

        //@Construct
        /// <summary>
        /// Initializes the layers height.
        /// </summary>
        public void ConstructLayersHeight()
        {
            LayerManager.ControlsHeight = this.Setting.LayersHeight;
        }

    }
}