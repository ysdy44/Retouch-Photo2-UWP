using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "BrushTool"/>.
    /// </summary>
    public sealed partial class BrushPage : Page
    {

        /// <summary>
        /// To a gradient brush.
        /// </summary>
        /// <param name="brushPoints"> The brush-points </param>
        public void Gradient(GradientBrushType gradientBrushType, BrushPoints brushPoints)
        {
            //GradientBrushType
            BrushType brushType = BrushType.LinearGradient;
            switch (gradientBrushType)
            {
                case GradientBrushType.LinearGradient: brushType = BrushType.LinearGradient; break;
                case GradientBrushType.RadialGradient: brushType = BrushType.RadialGradient; break;
                case GradientBrushType.EllipticalGradient: brushType = BrushType.EllipticalGradient; break;
            }

            //Brush
            this.SelectionViewModel.BrushType = brushType;
            this.SelectionViewModel.BrushArray = Brush.GetNewArray();
            this.SelectionViewModel.BrushPoints = brushPoints;

            //Selection
            this.SelectionViewModel.SetValue(((layer) =>
            {
                if (layer is IGeometryLayer geometryLayer)
                {
                    //FillOrStroke
                    switch (this.SelectionViewModel.FillOrStroke)
                    {
                        case FillOrStroke.Fill:
                            geometryLayer.StrokeBrush.Type = brushType;
                            geometryLayer.StrokeBrush.Array = Brush.GetNewArray();
                            geometryLayer.StrokeBrush.Points = brushPoints;
                            break;

                        case FillOrStroke.Stroke:
                            geometryLayer.FillBrush.Type = brushType;
                            geometryLayer.FillBrush.Array = Brush.GetNewArray();
                            geometryLayer.FillBrush.Points = brushPoints;
                            break;
                    }
                }
            }), true);
        }

        /// <summary>
        /// Sets the brush by fill or stroke.
        /// </summary>
        /// <param name="fillOrStroke"> The fill or stroke. </param>
        public void SetFillOrStroke(FillOrStroke fillOrStroke)
        {
            if (this.SelectionViewModel.Mode != ListViewSelectionMode.Single) return;

            //Selection
            if (this.SelectionViewModel.Layer is IGeometryLayer geometryLayer)
            {
                switch (fillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.SelectionViewModel.SetBrush(geometryLayer.FillBrush);
                        break;
                    case FillOrStroke.Stroke:
                        this.SelectionViewModel.SetBrush(geometryLayer.StrokeBrush);
                        break;
                }
            }
        }

    }
}