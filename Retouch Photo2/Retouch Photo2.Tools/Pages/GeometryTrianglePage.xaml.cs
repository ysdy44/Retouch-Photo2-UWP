using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers.Models;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryTriangleTool"/>.
    /// </summary>
    public sealed partial class GeometryTrianglePage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private int CenterNumberConverter(float center) => (int)(center * 100.0f);

        //@Construct
        public GeometryTrianglePage()
        {
            this.InitializeComponent();

            //Center
            this.CenterTouchbarButton.Type = TouchbarType.GeometryTriangleCenter;
            this.CenterTouchbarButton.Unit = "%";

            this.MirrorButton.Tapped += (s, e) =>
            {
                float selectionCenter = 1.0f - this.SelectionViewModel.GeometryTriangleCenter;
                this.SelectionViewModel.GeometryTriangleCenter = selectionCenter;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is GeometryTriangleLayer geometryTriangleLayer)
                    {
                        float center = 1.0f - geometryTriangleLayer.Center;
                        geometryTriangleLayer.Center = center;
                    }
                });
                
                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }
}