using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools.Pages
{
    public sealed partial class AcrylicPage : ToolPage
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        //Converter
        public int TintOpacityConverter(float value) => (int)(value * 100.0f);
        public int BlurAmountConverter(float value) => (int)value;

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
                if (opacity >0.9f) opacity = 0.9f;

                this.ViewModel.AcrylicTintOpacity = opacity ;
                if (this.ViewModel.AcrylicLayer!=null)
                {
                    this.ViewModel.AcrylicLayer.TintOpacity = opacity ;
                    this.ViewModel.Invalidate();
                }
            };

            //BlurAmount
            this.BlurAmountPicker.Minimum = 10;
            this.BlurAmountPicker.Maximum = 100;
            this.BlurAmountPicker.ValueChange += (s, value) =>
            {
                float amount = value  ;

                if (amount < 10.0f) amount = 10.0f;
                if (amount > 100.0f) amount = 100.0f;

                this.ViewModel.AcrylicBlurAmount = amount;
                if (this.ViewModel.AcrylicLayer != null)
                {
                    this.ViewModel.AcrylicLayer.BlurAmount = amount;
                    this.ViewModel.Invalidate();
                }
            };
        }
    }
}
