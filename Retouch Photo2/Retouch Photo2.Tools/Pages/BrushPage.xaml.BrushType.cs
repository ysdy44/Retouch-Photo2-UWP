using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI;
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
            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Type = BrushType.None;
                        //Selection
                        this.SelectionViewModel.StyleManager.FillBrush.Type = BrushType.None;
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Type = BrushType.None;
                        //Selection
                        this.SelectionViewModel.StyleManager.StrokeBrush.Type = BrushType.None;
                        break;
                }
            });

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
            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Type = BrushType.Color;
                        layer.StyleManager.FillBrush.Color = this.SelectionViewModel.FillColor;
                        //Selection
                        this.SelectionViewModel.StyleManager.FillBrush.Type = BrushType.Color;
                        this.SelectionViewModel.StyleManager.FillBrush.Color = this.SelectionViewModel.FillColor;
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Type = BrushType.Color;
                        layer.StyleManager.StrokeBrush.Color = this.SelectionViewModel.StrokeColor;
                        //Selection
                        this.SelectionViewModel.StyleManager.StrokeBrush.Type = BrushType.Color;
                        this.SelectionViewModel.StyleManager.StrokeBrush.Color = this.SelectionViewModel.StrokeColor;
                        break;
                }
            });

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

            this.SelectionViewModel.OneBrushPoints();
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

            this.SelectionViewModel.OneBrushPoints();
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

            this.SelectionViewModel.OneBrushPoints();
            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeImage()
        {
            //BrushType
            this.SelectionViewModel.BrushType = BrushType.Image;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Type = BrushType.Image;
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Type = BrushType.Image;
                        break;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}