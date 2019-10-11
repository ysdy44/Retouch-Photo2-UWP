using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryStarTool"/>.
    /// </summary>
    public sealed partial class GeometryStarPage : Page
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private int InnerRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);

        //@Construct
        public GeometryStarPage()
        {
            this.InitializeComponent();

            //Points
            this.PointsTouchbarButton.Type = TouchbarType.GeometryStarPoints;

            //InnerRadius
            this.InnerRadiusTouchbarButton.Type = TouchbarType.GeometryStarInnerRadius;
            this.InnerRadiusTouchbarButton.Unit = "%";
        }
    }
}