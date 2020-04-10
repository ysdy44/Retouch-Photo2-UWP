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
		
        private void SetColor(Color value)
        {
            this.SelectionViewModel.Color = value;

            //Brush
            this.SelectionViewModel.BrushType = BrushType.Color;

            //Selection
            this.SelectionViewModel.FillColor = value;
            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Color = value;
                        //Selection
                        this.SelectionViewModel.StyleManager.FillBrush.Color = value;
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Color = value;
                        //Selection
                        this.SelectionViewModel.StyleManager.StrokeBrush.Color = value;
                        break;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        private void SetArray(CanvasGradientStop[] array)
        {
            //Selection
            this.SelectionViewModel.BrushArray = (CanvasGradientStop[])array.Clone();

            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Array = (CanvasGradientStop[])array.Clone();
                        //Selection
                        this.SelectionViewModel.StyleManager.FillBrush.Array = (CanvasGradientStop[])array.Clone();
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Array = (CanvasGradientStop[])array.Clone();
                        //Selection
                        this.SelectionViewModel.StyleManager.StrokeBrush.Array = (CanvasGradientStop[])array.Clone();
                        break;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }


        /// <summary>
        /// To a gradient brush.
        /// </summary>
        /// <param name="brushPoints"> The brush-points </param>
        public void Gradient(GradientBrushType gradientBrushType, BrushPoints brushPoints, bool isResetBrushArray)
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
            if (isResetBrushArray) this.SelectionViewModel.BrushArray = GreyWhiteMeshHelpher.GetGradientStopArray();
            this.SelectionViewModel.BrushPoints = brushPoints;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Type = brushType;
                        if (isResetBrushArray)
                            layer.StyleManager.StrokeBrush.Array = GreyWhiteMeshHelpher.GetGradientStopArray();
                        layer.StyleManager.StrokeBrush.Points = brushPoints;
                        break;

                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Type = brushType;
                        if (isResetBrushArray)
                            layer.StyleManager.FillBrush.Array = GreyWhiteMeshHelpher.GetGradientStopArray();
                        layer.StyleManager.FillBrush.Points = brushPoints;
                        break;
                }
            });
        }

    }
}