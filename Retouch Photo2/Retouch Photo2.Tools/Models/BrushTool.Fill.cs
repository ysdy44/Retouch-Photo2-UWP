using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public sealed partial class BrushTool : Page, ITool
    {

        //@ViewModel
        IBrush Fill { get => this.SelectionViewModel.Fill; set => this.SelectionViewModel.Fill = value; }


        private void ConstructFillImage()
        {
            Retouch_Photo2.PhotosPage.FillImageCallBack += (photo) =>
            {
                this.FillTypeChanged(BrushType.Image, photo);
                this.ShowControl.Invalidate();
            };
            this.BrushTypeComboBox.FillTypeChanged += (s, brushType) =>
            {
                if (brushType == BrushType.Image)
                {
                    Retouch_Photo2.DrawPage.FrameNavigatePhotosPage?.Invoke(PhotosPageMode.FillImage);//Delegate
                }
                else
                {
                    this.FillTypeChanged(brushType);
                    this.ShowControl.Invalidate();
                }
            };
        }


        //////////////////////////


        private void FillStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.Fill == null) return;

            //Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this.HandleMode = this.Fill.ContainsHandleMode(startingPoint, matrix);


            if (this.HandleMode == BrushHandleMode.None)
            {
                switch (this.Fill.Type)
                {
                    case BrushType.None:
                    case BrushType.Color:
                        {
                            this.HandleMode = BrushHandleMode.ToInitializeController;

                            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                            this.Fill = BrushBase.LinearGradientBrush(canvasStartingPoint, canvasPoint);
                            this.MethodViewModel.StyleChangeStarted(cache: (style) => style.CacheFill());
                        }
                        break;
                }
            }


            this.Fill.CacheTransform();
            this.MethodViewModel.StyleChangeStarted(cache: (style) =>
            {
                style.CacheFill();

                style.Fill = this.Fill.Clone();
                style.Fill.CacheTransform();
            });
        }

        private void FillDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Selection
            if (this.Fill == null) return;

            switch (this.HandleMode)
            {
                case BrushHandleMode.ToInitializeController:
                    this.Fill.InitializeController(canvasStartingPoint, canvasPoint);
                    this.MethodViewModel.StyleChangeDelta(set: (style) => style.Fill.InitializeController(canvasStartingPoint, canvasPoint));
                    break;

                default:
                    this.Fill.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);
                    this.MethodViewModel.StyleChangeDelta(set: (style) => style.Fill.Controller(this.HandleMode, canvasStartingPoint, canvasPoint));
                    break;
            }
        }

        private void FillComplete(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Selection
            if (this.Fill == null) return;
            this.Fill.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);

            this.MethodViewModel.StyleChangeCompleted
            (
                set: (style) => style.Fill.Controller(this.HandleMode, canvasStartingPoint, canvasPoint),
                historyTitle: "Set fill",
                getHistory: (style) => style.StartingFill,
                setHistory: (style, previous) => style.Fill = previous.Clone()
            );
        }


        //////////////////////////


        private void FillTypeChanged(BrushType brushType, Photo photo = null)
        {
            if (this.Fill.Type == brushType) return;


            IBrush brush = null;

            this.MethodViewModel.StyleChanged
            (
                set: (style, transformer) =>
                {
                    style.Fill.TypeChange(brushType, transformer, photo);

                    brush = style.Fill;
                },

                historyTitle: "Set fill type",
                getHistory: (style) => style.Fill.Clone(),
                setHistory: (style, previous) => style.Fill = previous.Clone()
            );

            if (brush != null)
            {
                this.Fill = brush.Clone();

                if (brush.Type == BrushType.Color) this.SelectionViewModel.Color = this.Fill.Color;
            }
        }

        private void FillShow()
        {
            if (this.Fill == null) return;

            switch (this.Fill.Type)
            {
                case BrushType.None: break;

                case BrushType.Color:
                    DrawPage.FillColorShowAt(this.ShowControl);
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.StopsPicker.SetArray(this.Fill.Stops);
                    this.StopsFlyout.ShowAt(this);//Flyout
                    break;
            }
        }


        //////////////////////////


        private void FillStopsChanged(CanvasGradientStop[] array)
        {
            this.Fill.Stops = array.CloneArray();

            this.MethodViewModel.StyleChanged
            (
                set: (style, transformer) => style.Fill.Stops = array.CloneArray(),
                historyTitle: "Set fill",
                getHistory: (style) => style.Fill.Clone(),
                setHistory: (style, previous) => style.Fill = previous.Clone()
            );
        }

        private void FillStopsChangeStarted(CanvasGradientStop[] array)
        {
            this.MethodViewModel.StyleChangeStarted(cache: (style) => style.CacheFill());
        }
        private void FillStopsChangeDelta(CanvasGradientStop[] array)
        {
            this.Fill.Stops = array.CloneArray();

            this.MethodViewModel.StyleChangeDelta(set: (style) => style.Fill.Stops = array.CloneArray());
        }
        private void FillStopsChangeCompleted(CanvasGradientStop[] array)
        {
            this.Fill.Stops = array.CloneArray();

            this.MethodViewModel.StyleChangeCompleted<IBrush>
            (
                set: (style) => style.Fill.Stops = array.CloneArray(),
                historyTitle: "Set fill",
                getHistory: (style) => style.Fill.Clone(),
                setHistory: (style, previous) => style.Fill = previous.Clone()
            );
        }

        private void FillExtendChanged(CanvasEdgeBehavior extend)
        {
            this.Fill.Extend = extend;
            this.ExtendComboBox.Extend = extend;

            this.MethodViewModel.StyleChanged<CanvasEdgeBehavior>
            (
                set: (style, transformer) => style.Fill.Extend = extend,
                historyTitle: "Set fill extend",
                getHistory: (style) => style.Fill.Extend,
                setHistory: (style, previous) => style.Fill.Extend = previous
            );
        }

    }
}