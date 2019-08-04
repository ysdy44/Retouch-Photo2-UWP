using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Pages.DrawPages
{
    public sealed partial class StrokeWidthPicker : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Converter
        public int StrokeWidthConverter(float strokeWidth) => (int)(strokeWidth * 100.0f);
        

        //@Construct
        public StrokeWidthPicker()
        {
            this.InitializeComponent();
            this.Picker.Minimum = 0;
            this.Picker.Maximum = 10000;
            this.Picker.ValueChange += (s, value) =>
            {
                float strokeWidth = value / 100.0f;

                //Selection
                this.SelectionViewModel.StrokeWidth = strokeWidth;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is IGeometryLayer geometryLayer)
                    {
                        geometryLayer.StrokeWidth = strokeWidth;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }
}