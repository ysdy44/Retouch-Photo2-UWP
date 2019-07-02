using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary> 
    /// Page of <see cref = "AcrylicTool"/>.
    /// </summary>
    public sealed partial class AcrylicPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        public int TintOpacityConverter(float value) => (int)(value * 100.0f);
        public int BlurAmountConverter(float value) => (int)value;

        //@Construct
        public AcrylicPage()
        {
            this.InitializeComponent();

            //TintOpacity
            this.TintOpacityPicker.Minimum = 0;
            this.TintOpacityPicker.Maximum = 90;
            this.TintOpacityPicker.ValueChange += (s, value) =>
            {
                float opacity = value / 100.0f;
                if (opacity < 0.0f) opacity = 0.0f;
                if (opacity > 0.9f) opacity = 0.9f;

                this.SelectionViewModel.AcrylicTintOpacity = opacity;
            
                //Selection
                this.SelectionViewModel.SetValue((layer)=> 
                {
                    if (layer is AcrylicLayer acrylicLayer)
                    {
                        acrylicLayer.TintOpacity = opacity;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            //BlurAmount
            this.BlurAmountPicker.Minimum = 10;
            this.BlurAmountPicker.Maximum = 100;
            this.BlurAmountPicker.ValueChange += (s, value) =>
            {
                float amount = value;
                if (amount < 10.0f) amount = 10.0f;
                if (amount > 100.0f) amount = 100.0f;

                this.SelectionViewModel.AcrylicBlurAmount = amount;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is AcrylicLayer acrylicLayer)
                    {
                        acrylicLayer.BlurAmount = amount;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };

        }
    }
}