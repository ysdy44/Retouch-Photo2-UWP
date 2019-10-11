using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryRoundRectTool"/>.
    /// </summary>
    public sealed partial class GeometryRoundRectPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

        //@Converter
        private int CornerNumberConverter(float corner) => (int)(corner * 100.0f);

        //@Construct
        public GeometryRoundRectPage()
        {
            this.InitializeComponent();

            //Corner
            this.CornerTouchbarButton.Type = TouchbarType.GeometryRoundRectCorner;
            this.CornerTouchbarButton.Unit = "%";
        }
    }
}