using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    public sealed partial class StrokeWidthButton : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Static
        public static StrokeWidthControl StrokeWidthControl = new StrokeWidthControl();

        //@Converter
        private int NumberConverter(float strokeWidth) => (int)(strokeWidth * 100.0f);

        //@Construct
        public StrokeWidthButton()
        {            
            this.InitializeComponent();

            this.TouchbarButton.Unit = "%";
            this.TouchbarButton.Tapped2 += (s, isChecked) =>
            {
                if (isChecked)
                {
                    this.TouchbarButton.IsChecked = false;
                    this.TipViewModel.Touchbar = null;
                }
                else
                {
                    this.TouchbarButton.IsChecked = true;
                    this.TipViewModel.Touchbar = StrokeWidthButton.StrokeWidthControl;
                }
            };
        }
    }
}