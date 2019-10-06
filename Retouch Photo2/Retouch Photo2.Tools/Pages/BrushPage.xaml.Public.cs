using HSVColorPickers;
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
                case GradientBrushType.Linear: brushType = BrushType.LinearGradient; break;
                case GradientBrushType.Radial: brushType = BrushType.RadialGradient; break;
                case GradientBrushType.Elliptical: brushType = BrushType.EllipticalGradient; break;
            }

            //Brush
            this.SelectionViewModel.BrushType = brushType;
            this.SelectionViewModel.BrushArray = GreyWhiteMeshHelpher.GetGradientStopArray();
            this.SelectionViewModel.BrushPoints = brushPoints;

            //Selection
            this.SelectionViewModel.SetValue(((layer) =>
            {
                if (layer is IGeometryLayer geometryLayer)
                {
                    //FillOrStroke
                    switch (this.SelectionViewModel.FillOrStroke)
                    {
                        case FillOrStroke.Stroke:
                            geometryLayer.StrokeBrush.Type = brushType;
                            geometryLayer.StrokeBrush.Array = GreyWhiteMeshHelpher.GetGradientStopArray();
                            geometryLayer.StrokeBrush.Points = brushPoints;
                            break;

                        case FillOrStroke.Fill:
                            geometryLayer.FillBrush.Type = brushType;
                            geometryLayer.FillBrush.Array = GreyWhiteMeshHelpher.GetGradientStopArray();
                            geometryLayer.FillBrush.Points = brushPoints;
                            break;
                    }
                }
            }));
        }

        /// <summary>
        /// Sets the brush by fill or stroke.
        /// </summary>
        /// <param name="fillOrStroke"> The fill or stroke. </param>
        public void SetFillOrStroke(FillOrStroke fillOrStroke)
        {
            if (this.SelectionViewModel.SelectionMode != ListViewSelectionMode.Single) return;

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