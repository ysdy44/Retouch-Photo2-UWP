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
        

        //@Converter
        private int NumberConverter(float strokeWidth) => (int)(strokeWidth * 100.0f);

        private bool StrokeWidthTypeConverter(TouchbarType type) => type == TouchbarType.StrokeWidth;


        //@Construct
        public StrokeWidthButton()
        {            
            this.InitializeComponent();

            this.TouchbarButton.Unit = "%";
            this.TouchbarButton.Toggled += (s, isChecked) =>
            {
                if (isChecked)
                    this.TipViewModel.SetTouchbar(TouchbarType.None);//Touchbar
                else
                    this.TipViewModel.SetTouchbar(TouchbarType.StrokeWidth);//Touchbar
            };
        }
    }
}