using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Photos;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s TransparencyTool.
    /// </summary>
    public partial class TransparencyTool : ITool
    {
        //@ViewModel
        IBrush Transparency { get => this.SelectionViewModel.Transparency; set => this.SelectionViewModel.Transparency = value; }


        private void TransparencyStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.Transparency == null) return;

            //Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this.HandleMode = this.Transparency.ContainsHandleMode(startingPoint, matrix);


            if (this.HandleMode == BrushHandleMode.None)
            {
                switch (this.Transparency.Type)
                {
                    case BrushType.None:
                    case BrushType.Color:
                        {
                            this.HandleMode = BrushHandleMode.ToInitializeController;

                            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                            this.Transparency = BrushBase.LinearGradientBrush(canvasStartingPoint, canvasPoint, Colors.Transparent);
                            this.MethodViewModel.StyleChangeStarted(cache: (style) => style.CacheTransparency());
                        }
                        break;
                }
            }


            this.Transparency.CacheTransform();
            this.MethodViewModel.StyleChangeStarted(cache: (style) =>
            {
                style.CacheTransparency();

                style.Transparency = this.Transparency.Clone();
                style.Transparency.CacheTransform();
            });
        }

        private void TransparencyDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Selection
            if (this.Transparency == null) return;

            switch (this.HandleMode)
            {
                case BrushHandleMode.ToInitializeController:
                    this.Transparency.InitializeController(canvasStartingPoint, canvasPoint);
                    this.MethodViewModel.StyleChangeDelta(set: (style) => style.Transparency.InitializeController(canvasStartingPoint, canvasPoint));
                    break;

                default:
                    this.Transparency.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);
                    this.MethodViewModel.StyleChangeDelta(set: (style) => style.Transparency.Controller(this.HandleMode, canvasStartingPoint, canvasPoint));
                    break;
            }
        }

        private void TransparencyComplete(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Selection
            if (this.Transparency == null) return;
            this.Transparency.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);

            this.MethodViewModel.StyleChangeCompleted
            (
                set: (style) => style.Transparency.Controller(this.HandleMode, canvasStartingPoint, canvasPoint),
                historyTitle: "Set transparency",
                getHistory: (style) => style.StartingTransparency,
                setHistory: (style, previous) => style.Transparency = previous.Clone()
            );
        }

    }


    /// <summary>
    /// Page of <see cref="TransparencyTool"/>.
    /// </summary>
    internal partial class TransparencyPage : Page
    {

        //@ViewModel
        IBrush Transparency { get => this.SelectionViewModel.Transparency; set => this.SelectionViewModel.Transparency = value; }


        private void ConstructTransparencyType()
        {
            //Retouch_Photo2.PhotosPage.TransparencyImageCallBack += (photo) =>
            //{
            //    this.TransparencyTypeChanged(BrushType.Image, photo);
            //    this.ShowControl.Invalidate();
            //};
            this.TypeComboBox.TypeChanged += (s, brushType) =>
            {
                //if (brushType == BrushType.Image)
                //{
                //    Retouch_Photo2.DrawPage.FrameNavigatePhotosPage?.Invoke(PhotosPageMode.TransparencyImage);//Delegate
                //}
                //else
                //{
                this.TransparencyTypeChanged(brushType);
                this.ShowControl.Invalidate();
                //}
            };
        }


        //////////////////////////


        private void TransparencyTypeChanged(BrushType brushType, Photo photo = null)
        {
            if (this.Transparency.Type == brushType) return;


            IBrush brush = null;

            this.MethodViewModel.StyleChanged
            (
                set: (style, transformer) =>
                {
                    style.Transparency.TypeChange(brushType, transformer, Colors.Transparent, photo);

                    brush = style.Transparency;
                },

                historyTitle: "Set transparency type",
                getHistory: (style) => style.Transparency.Clone(),
                setHistory: (style, previous) => style.Transparency = previous.Clone()
            );

            if (brush != null)
            {
                this.Transparency = brush.Clone();

                if (brush.Type == BrushType.Color) this.SelectionViewModel.Color = this.Transparency.Color;
            }
        }

        private void TransparencyShow()
        {
            if (this.Transparency == null) return;

            switch (this.Transparency.Type)
            {
                case BrushType.None: break;

                //case BrushType.Color:
                //    DrawPage.TransparencyColorShowAt(this.ShowControl);
                //    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.StopsPicker.SetArray(this.Transparency.Stops);
                    this.StopsFlyout.ShowAt(this);//Flyout
                    break;
            }
        }


        //////////////////////////


        private void TransparencyStopsChanged(CanvasGradientStop[] array)
        {
            this.Transparency.Stops = array.CloneArray();

            this.MethodViewModel.StyleChanged
            (
                set: (style, transformer) => style.Transparency.Stops = array.CloneArray(),
                historyTitle: "Set transparency",
                getHistory: (style) => style.Transparency.Clone(),
                setHistory: (style, previous) => style.Transparency = previous.Clone()
            );
        }

        private void TransparencyStopsChangeStarted(CanvasGradientStop[] array)
        {
            this.MethodViewModel.StyleChangeStarted(cache: (style) => style.CacheTransparency());
        }
        private void TransparencyStopsChangeDelta(CanvasGradientStop[] array)
        {
            this.Transparency.Stops = array.CloneArray();

            this.MethodViewModel.StyleChangeDelta(set: (style) => style.Transparency.Stops = array.CloneArray());
        }
        private void TransparencyStopsChangeCompleted(CanvasGradientStop[] array)
        {
            this.Transparency.Stops = array.CloneArray();

            this.MethodViewModel.StyleChangeCompleted<IBrush>
            (
                set: (style) => style.Transparency.Stops = array.CloneArray(),
                historyTitle: "Set transparency",
                getHistory: (style) => style.Transparency.Clone(),
                setHistory: (style, previous) => style.Transparency = previous.Clone()
            );
        }

        //private void TransparencyExtendChanged(CanvasEdgeBehavior extend)
        //{
        //    this.Transparency.Extend = extend;
        //    this.ExtendComboBox.Extend = extend;
        //
        //    this.MethodViewModel.StyleChanged<CanvasEdgeBehavior>
        //    (
        //       set: (style, transformer) => style.Transparency.Extend = extend,
        //       historyTitle: "Set transparency extend",
        //       getHistory: (style) => style.Transparency.Extend,
        //       setHistory: (style, previous) => style.Transparency.Extend = previous
        //   );
    }

}