using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Element
{
    public sealed partial class StrokeWidthControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        //Converter
        public int StrokeWidthConverter(float value) => (int)(value * 100.0f);

        public StrokeWidthControl()
        {
            this.InitializeComponent();

            //StrokeWidth
            this.StrokeWidthPicker.Minimum = 0;
            this.StrokeWidthPicker.Maximum = 10000;
            this.StrokeWidthPicker.ValueChange += (s, value) =>
            {
                float width = value / 100.0f;

                if (width < 0.0f) width = 0.0f;
                if (width > 100.0f) width = 100.0f;

                this.ViewModel.StrokeWidth = width;

                //Line
                if (this.ViewModel.LineLayer != null)
                {
                    this.ViewModel.LineLayer.StrokeWidth = width;
                    this.ViewModel.Invalidate();
                }

                //Geometry
                if (this.ViewModel.GeometryLayer != null)
                {
                    this.ViewModel.GeometryLayer.StrokeWidth = width;
                    this.ViewModel.Invalidate();
                }
            };
        }
    }
}
