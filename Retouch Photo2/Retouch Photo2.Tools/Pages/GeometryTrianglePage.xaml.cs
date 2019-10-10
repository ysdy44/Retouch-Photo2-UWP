using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryTriangleTool"/>.
    /// </summary>
    public sealed partial class GeometryTrianglePage : Page
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private int CenterNumberConverter(float center) => (int)(center * 100.0f);

        //@Construct
        public GeometryTrianglePage()
        {
            this.InitializeComponent();

            //Center
            this.CenterTouchbarButton.Unit = "%";
        }
    }
}