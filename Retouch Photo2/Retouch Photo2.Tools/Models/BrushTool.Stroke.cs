using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.Models;
using Retouch_Photo2.Historys;
using System;
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
        IBrush Stroke { get => this.SelectionViewModel.Stroke; set => this.SelectionViewModel.Stroke = value; }

        //@Static
        /// <summary> Navigate to <see cref="PhotosPage"/> </summary>
        public static Action StrokeImage;



        public void StrokeStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.Stroke == null) return;

            //Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this._operateMode = this.Stroke.ContainsOperateMode(startingPoint, matrix);

            //InitializeController
            if (this._operateMode == BrushOperateMode.InitializeController)
            {
                switch (this.Stroke.Type)
                {
                    case BrushType.None:
                    case BrushType.Color:
                        {
                            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                            //Selection          
                            this.Stroke = new LinearGradientBrush(canvasStartingPoint, canvasPoint);
                        }
                        break;
                }
            }

            //Selection
            this.Stroke.CacheTransform();
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.Style.CacheStroke();

                layer.Style.Stroke = this.Stroke.Clone();
                layer.Style.Stroke.CacheTransform();
            });
        }

        public void StrokeDelta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Stroke == null) return;


            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this._operateMode)
            {
                //InitializeController
                case BrushOperateMode.InitializeController:
                    {
                        //Selection
                        this.Stroke.InitializeController(canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.Style.Stroke.InitializeController(canvasStartingPoint, canvasPoint);
                        });
                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;

                //Controller
                default:
                    {
                        //Selection
                        this.Stroke.Controller(this._operateMode, canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.Style.Stroke.Controller(this._operateMode, canvasStartingPoint, canvasPoint);
                        });
                    }
                    break;
            }
        }

        public void StrokeComplete(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Stroke == null) return;

            //History
            IHistoryBase history = new IHistoryBase("Set stroke");

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                //History
                var previous = layer.Style.Stroke.Clone();
                history.Undos.Push(() => layer.Style.Stroke = previous.Clone());

                this.SelectionViewModel.StyleLayer = layer;
            });

            //History
            this.ViewModel.Push(history);
        }



        public void StrokeTypeChanged(BrushType brushType)
        {
            if (this.Stroke.Type == brushType) return;

            switch (brushType)
            {
                case BrushType.Image:
                    BrushTool.StrokeImage?.Invoke();
                    break;

                default:
                    {
                        //History
                        IHistoryBase history = new IHistoryBase("Set stroke type");

                        IBrush brush = this.GetTypeBrush(this.Stroke, brushType);
                        if (brushType == BrushType.Color) this.SelectionViewModel.Color = brush.Color;


                        //Selection
                        this.Stroke = brush;
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            //History
                            var previous = layer.Style.Stroke.Clone();
                            history.Undos.Push(() => layer.Style.Stroke = previous.Clone());

                            layer.Style.Stroke = brush.Clone();
                            this.SelectionViewModel.StyleLayer = layer;
                        });

                        //History
                        this.ViewModel.Push(history);

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
            }
        }

        public void StrokeShow()
        {
            if (this.Stroke == null) return;

            switch (this.Stroke.Type)
            {
                case BrushType.None: break;

                case BrushType.Color:
                    DrawPage.StrokeColorShowAt(this);
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.StopsPicker.SetArray(this.Stroke.Array);
                    this.StopsFlyout.ShowAt(this);//Flyout
                    break;

                case BrushType.Image:
                    this.ExtendComboBox.Extend = this.Stroke.Extend;
                    this.ImageFlyout.ShowAt(this);//Flyout
                    break;
            }
        }



        public void StrokeStopsChanged(CanvasGradientStop[] array)
        {
            if (this._isStopsFlyoutShowed == false) return;

            this.Stroke.Array = (CanvasGradientStop[])array.Clone();

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.Style.Stroke.Array = (CanvasGradientStop[])array.Clone();
                this.SelectionViewModel.StyleLayer = layer;
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        public void StrokeExtendChanged(CanvasEdgeBehavior extend)
        {
            this.ExtendComboBox.Extend = extend;

            //Selection
            this.Stroke.Extend = extend;
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.Style.Stroke.Extend = extend;
                this.SelectionViewModel.StyleLayer = layer;
            });

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}