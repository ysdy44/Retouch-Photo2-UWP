using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryPentagonTool"/>.
    /// </summary>
    public sealed partial class GeometryPentagonPage : Page
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public GeometryPentagonPage()
        {
            this.InitializeComponent();

            //Points
            this.PointsTouchbarButton.Type = TouchbarType.GeometryPentagonPoints;
        }
    }
}