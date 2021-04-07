using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Photos;
using Retouch_Photo2.Tools.Elements;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    public partial class BrushTool : ITool
    {
        //@ViewModel
        IBrush Stroke { get => this.SelectionViewModel.Stroke; set => this.SelectionViewModel.Stroke = value; }


        private void StrokeStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.Stroke == null) return;

            //Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this.HandleMode = this.Stroke.ContainsHandleMode(startingPoint, matrix);


            if (this.HandleMode == BrushHandleMode.None)
            {
                switch (this.Stroke.Type)
                {
                    case BrushType.None:
                    case BrushType.Color:
                        {
                            this.HandleMode = BrushHandleMode.ToInitializeController;

                            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                            this.Stroke = BrushBase.LinearGradientBrush(canvasStartingPoint, canvasPoint);
                            this.MethodViewModel.StyleChangeStarted(cache: (style) => style.CacheStroke());
                        }
                        break;
                }
            }


            this.Stroke.CacheTransform();
            this.MethodViewModel.StyleChangeStarted(cache: (style) =>
            {
                style.CacheStroke();

                style.Stroke = this.Stroke.Clone();
                style.Stroke.CacheTransform();
            });
        }

        private void StrokeDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Selection
            if (this.Stroke == null) return;

            switch (this.HandleMode)
            {
                case BrushHandleMode.ToInitializeController:
                    this.Stroke.InitializeController(canvasStartingPoint, canvasPoint);
                    this.MethodViewModel.StyleChangeDelta(set: (style) => style.Stroke.InitializeController(canvasStartingPoint, canvasPoint));
                    break;

                default:
                    this.Stroke.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);
                    this.MethodViewModel.StyleChangeDelta(set: (style) => style.Stroke.Controller(this.HandleMode, canvasStartingPoint, canvasPoint));
                    break;
            }
        }

        private void StrokeComplete(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Selection
            if (this.Stroke == null) return;
            this.Stroke.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);

            this.MethodViewModel.StyleChangeCompleted
            (
                set: (style) => style.Stroke.Controller(this.HandleMode, canvasStartingPoint, canvasPoint),
                type: Historys.HistoryType.LayersProperty_SetStyle_Stroke,
                getUndo: (style) => style.StartingStroke,
                setUndo: (style, previous) => style.Stroke = previous.Clone()
            );
        }

    }


    /// <summary>
    /// Page of <see cref="BrushTool"/>.
    /// </summary>
    internal partial class BrushPage : Page
    {

        //@ViewModel
        IBrush Stroke { get => this.SelectionViewModel.Stroke; set => this.SelectionViewModel.Stroke = value; }


        private void ConstructStrokeType()
        {
            this.TypeComboBox.StrokeTypeChanged += async (s, brushType) =>
            {
                if (brushType == BrushType.Image)
                {
                    Photo photo = await Retouch_Photo2.DrawPage.ShowGalleryFunc?.Invoke();

                    this.StrokeTypeChanged(BrushType.Image, photo);
                    this.BrushShowControl.Invalidate();
                }
                else
                {
                    this.StrokeTypeChanged(brushType);
                    this.BrushShowControl.Invalidate();
                }
            };
        }


        //////////////////////////


        private void StrokeTypeChanged(BrushType brushType, Photo photo = null)
        {
            if (this.Stroke.Type == brushType) return;


            IBrush brush = null;

            this.MethodViewModel.StyleChanged
            (
                set: (style, transformer) =>
                {
                    style.Stroke.TypeChange(brushType, transformer, photo);

                    brush = style.Stroke;
                },

                type: Historys.HistoryType.LayersProperty_SetStyle_Stroke_Type,
                getUndo: (style) => style.Stroke.Clone(),
                setUndo: (style, previous) => style.Stroke = previous.Clone()
            );

            if (brush != null)
            {
                this.Stroke = brush.Clone();

                if (brush.Type == BrushType.Color) this.SelectionViewModel.Color = this.Stroke.Color;
            }
        }

        private void StrokeShow()
        {
            if (this.Stroke == null) return;

            switch (this.Stroke.Type)
            {
                case BrushType.None: break;

                case BrushType.Color:
                    Retouch_Photo2.DrawPage.ShowStrokeColorFlyout?.Invoke(this, this.BrushShowControl);
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.StopsPicker.SetArray(this.Stroke.Stops);
                    this.StopsFlyout.ShowAt(this);//Flyout
                    break;
            }
        }


        //////////////////////////


        private void StrokeStopsChanged(CanvasGradientStop[] array)
        {
            this.Stroke.Stops = array.CloneArray();

            this.MethodViewModel.StyleChanged
            (
                set: (style, transformer) => style.Stroke.Stops = array.CloneArray(),
                type: Historys.HistoryType.LayersProperty_SetStyle_Stroke,
                getUndo: (style) => style.Stroke.Clone(),
                setUndo: (style, previous) => style.Stroke = previous.Clone()
            );
        }

        private void StrokeStopsChangeStarted(CanvasGradientStop[] array)
        {
            this.MethodViewModel.StyleChangeStarted(cache: (style) => style.CacheStroke());
        }
        private void StrokeStopsChangeDelta(CanvasGradientStop[] array)
        {
            this.Stroke.Stops = array.CloneArray();

            this.MethodViewModel.StyleChangeDelta(set: (style) => style.Stroke.Stops = array.CloneArray());
        }
        private void StrokeStopsChangeCompleted(CanvasGradientStop[] array)
        {
            this.Stroke.Stops = array.CloneArray();

            this.MethodViewModel.StyleChangeCompleted<IBrush>
            (
                set: (style) => style.Stroke.Stops = array.CloneArray(),
                type: Historys.HistoryType.LayersProperty_SetStyle_Stroke,
                getUndo: (style) => style.Stroke.Clone(),
                setUndo: (style, previous) => style.Stroke = previous.Clone()
            );
        }

        private void StrokeExtendChanged(CanvasEdgeBehavior extend)
        {
            this.Stroke.Extend = extend;
            this.ExtendComboBox.Extend = extend;

            this.MethodViewModel.StyleChanged<CanvasEdgeBehavior>
            (
                set: (style, transformer) => style.Stroke.Extend = extend,
                type: Historys.HistoryType.LayersProperty_SetStyle_Stroke_Extend,
                getUndo: (style) => style.Stroke.Extend,
                setUndo: (style, previous) => style.Stroke.Extend = previous
            );
        }

    }
}