using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "BrushTool"/>.
    /// </summary>
    public sealed partial class BrushPage : Page
    {

        private void ToBrushTypeNone()
        {
            //Brush
            this.SelectionViewModel.BrushType = BrushType.None;

            //Selection
            this.SelectionViewModel.SetValue(((layer) =>
            {
                if (layer is IGeometryLayer geometryLayer)
                {
                    //FillOrStroke
                    switch (this.SelectionViewModel.FillOrStroke)
                    {
                        case FillOrStroke.Fill:
                            geometryLayer.FillBrush.Type = BrushType.None;
                            break;
                        case FillOrStroke.Stroke:
                            geometryLayer.StrokeBrush.Type = BrushType.None;
                            break;
                    }
                }
            }));

            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeColor()
        {
            //Brush
            this.SelectionViewModel.BrushType = BrushType.Color;

            //FillOrStroke
            switch (this.SelectionViewModel.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.SelectionViewModel.Color = this.SelectionViewModel.FillColor;
                    break;
                case FillOrStroke.Stroke:
                    this.SelectionViewModel.Color = this.SelectionViewModel.StrokeColor;
                    break;
            }

            //Selection
            this.SelectionViewModel.SetValue(((layer) =>
            {
                if (layer is IGeometryLayer geometryLayer)
                {
                    //FillOrStroke
                    switch (this.SelectionViewModel.FillOrStroke)
                    {
                        case FillOrStroke.Fill:
                            geometryLayer.FillBrush.Type = BrushType.Color;
                            geometryLayer.FillBrush.Color = this.SelectionViewModel.FillColor;
                            break;
                        case FillOrStroke.Stroke:
                            geometryLayer.StrokeBrush.Type = BrushType.Color;
                            geometryLayer.StrokeBrush.Color = this.SelectionViewModel.StrokeColor;
                            break;
                    }
                }
            }));

            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeLinearGradient(bool isResetBrushArray)
        {
            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 startPoint = transformer.CenterTop;
            Vector2 endPoint = transformer.CenterBottom;

            BrushPoints brushPoints = new BrushPoints
            {
                LinearGradientStartPoint = startPoint,
                LinearGradientEndPoint = endPoint,
            };
            this.Gradient(GradientBrushType.Linear, brushPoints, isResetBrushArray: isResetBrushArray);

            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeRadialGradient(bool isResetBrushArray)
        {
            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 center = transformer.Center;
            Vector2 point = transformer.CenterBottom;

            BrushPoints brushPoints = new BrushPoints
            {
                RadialGradientCenter = center,
                RadialGradientPoint = point,
            };
            this.Gradient(GradientBrushType.Radial, brushPoints, isResetBrushArray: isResetBrushArray);

            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeEllipticalGradient(bool isResetBrushArray)
        {
            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 center = transformer.Center;
            Vector2 xPoint = transformer.CenterRight;
            Vector2 yPoint = transformer.CenterBottom;

            BrushPoints brushPoints = new BrushPoints
            {
                EllipticalGradientCenter = center,
                EllipticalGradientXPoint = xPoint,
                EllipticalGradientYPoint = yPoint,
            };
            this.Gradient(GradientBrushType.Elliptical, brushPoints, isResetBrushArray: isResetBrushArray);

            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeImage()
        {
            //BrushType
            this.SelectionViewModel.BrushType = BrushType.Image;

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}