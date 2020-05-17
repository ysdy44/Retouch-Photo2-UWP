using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
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


        private void ConstructStrokeImage()
        {
            Retouch_Photo2.PhotosPage.StrokeImageCallBack += (photo) =>
            {
                this.StrokeTypeChanged(BrushType.Image, photo);
                this.ShowControl.Invalidate();
            };
            this.BrushTypeComboBox.StrokeTypeChanged += (s, brushType) =>
            {
                if (brushType == BrushType.Image)
                {
                    Retouch_Photo2.DrawPage.FrameNavigatePhotosPage?.Invoke(PhotosPageMode.StrokeImage);
                }
                else
                {
                    this.StrokeTypeChanged(brushType);
                    this.ShowControl.Invalidate();
                }
            };
        }



        public void StrokeStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.Stroke == null) return;

            //Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this._operateMode = this.Stroke.ContainsOperateMode(startingPoint, matrix);

            //InitializeController
            if (this._operateMode == BrushHandleMode.None)
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
                            this.Stroke = BrushBase.LinearGradientBrush(canvasStartingPoint, canvasPoint);
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
                case BrushHandleMode.None:
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
            IHistoryBase history = new IHistoryBase("Set Stroke");

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



        public void StrokeTypeChanged(BrushType brushType, Photo photo = null)
        {
            if (this.Stroke.Type == brushType) return;

            //History
            IHistoryBase history = new IHistoryBase("Set Stroke type");

            bool _lock = false;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                //History
                var previous = layer.Style.Stroke.Clone();
                history.Undos.Push(() => layer.Style.Stroke = previous.Clone());

                Transformer transformer = layer.Transform.Destination;
                layer.Style.Stroke.TypeChange(brushType, transformer, photo);
                this.SelectionViewModel.StyleLayer = layer;


                if (_lock == false)
                {
                    _lock = true;
                    this.Stroke = layer.Style.Stroke.Clone();

                    if (this.Stroke.Type == BrushType.Color) this.SelectionViewModel.Color = this.Stroke.Color;
                }
            });


            //History
            this.ViewModel.Push(history);

            this.ViewModel.Invalidate();//Invalidate
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
                    this.StopsPicker.SetArray(this.Stroke.Stops);
                    this.StopsFlyout.ShowAt(this);//Flyout
                    break;
            }
        }



        public void StrokeStopsChanged(CanvasGradientStop[] array)
        {
            if (this._isStopsFlyoutShowed == false) return;

            this.Stroke.Stops = (CanvasGradientStop[])array.Clone();

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.Style.Stroke.Stops = (CanvasGradientStop[])array.Clone();
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