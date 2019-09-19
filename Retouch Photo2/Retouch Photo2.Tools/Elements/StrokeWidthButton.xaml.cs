using Retouch_Photo2.ViewModels;
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
        
        //@Construct
        public StrokeWidthButton()
        {            
            this.InitializeComponent();

            this.TouchbarButton.Unit = "%";
        }
    }
}