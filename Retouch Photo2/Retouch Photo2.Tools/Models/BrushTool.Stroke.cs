using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
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
            this.OperateMode = this.Stroke.ContainsOperateMode(startingPoint, matrix);

            //InitializeController
            if (this.OperateMode == BrushHandleMode.None)
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
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.Style.CacheStroke();

                layer.Style.Stroke = this.Stroke.Clone();
                layer.Style.Stroke.CacheTransform();
            });
        }

        public void StrokeDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Selection
            if (this.Stroke == null) return;

            switch (this.OperateMode)
            {
                //InitializeController
                case BrushHandleMode.None:
                    {
                        //Selection
                        this.Stroke.InitializeController(canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                        {
                            ILayer layer = layerage.Self;
                            layer.Style.Stroke.InitializeController(canvasStartingPoint, canvasPoint);
                        });
                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;

                //Controller
                default:
                    {
                        //Selection
                        this.Stroke.Controller(this.OperateMode, canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                        {
                            ILayer layer = layerage.Self;
                            layer.Style.Stroke.Controller(this.OperateMode, canvasStartingPoint, canvasPoint);
                        });
                    }
                    break;
            }
        }

        public void StrokeComplete()
        {
            //Selection
            if (this.Stroke == null) return;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set stroke");

            //Selection
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.StartingStroke.Clone();
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Style.Stroke = previous.Clone();
                });

                this.SelectionViewModel.StyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(history);
        }



        public void StrokeTypeChanged(BrushType brushType, Photo photo = null)
        {
            if (this.Stroke.Type == brushType) return;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set stroke type");

            bool _lock = false;

            //Selection
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Stroke.Clone(); ;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Style.Stroke = previous.Clone();
                });


                Transformer transformer = layer.Transform.Destination;
                layer.Style.Stroke.TypeChange(brushType, transformer, photo);
                this.SelectionViewModel.StyleLayerage = layerage;


                // Set Stroke Onces: lock
                if (_lock == false)
                {
                    _lock = true;
                    this.Stroke = layer.Style.Stroke.Clone();

                    if (this.Stroke.Type == BrushType.Color) this.SelectionViewModel.Color = this.Stroke.Color;
                }
            });


            //History
            this.ViewModel.HistoryPush(history);

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
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set stroke");

            //Selection
            this.Stroke.Stops = (CanvasGradientStop[])array.Clone();
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Stroke.Clone();
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Style.Stroke = previous.Clone();
                });


                layer.Style.Stroke.Stops = (CanvasGradientStop[])array.Clone();
                this.SelectionViewModel.StyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
            this.ShowControl.Invalidate();
        }

        //History
        LayersPropertyHistory historyStroke = null;
        public void StrokeStopsChangeStarted(CanvasGradientStop[] array)
        {
            //History
            this.historyStroke = new LayersPropertyHistory("Set stroke");

            //Selection
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.Style.CacheStroke();
            });

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void StrokeStopsChangeDelta(CanvasGradientStop[] array)
        {
            //Selection
            this.Stroke.Stops = (CanvasGradientStop[])array.Clone();
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.Style.Stroke.Stops = (CanvasGradientStop[])array.Clone();
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        public void StrokeStopsChangeCompleted(CanvasGradientStop[] array)
        {
            this.Stroke.Stops = (CanvasGradientStop[])array.Clone();

            //Selection
            this.ShowControl.Invalidate();
            this.Stroke.Stops = (CanvasGradientStop[])array.Clone();
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Stroke.Clone();
                this.historyStroke.UndoActions.Push(() =>
                {
                    layer.Style.Stroke = previous.Clone();
                });

                layer.Style.Stroke.Stops = (CanvasGradientStop[])array.Clone();
                this.SelectionViewModel.StyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(this.historyStroke);

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        public void StrokeExtendChanged(CanvasEdgeBehavior extend)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set stroke extend");

            //Selection
            this.Stroke.Extend = extend;
            this.ExtendComboBox.Extend = extend;
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Stroke.Extend;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.Style.Stroke.Extend = previous;
                });

                layer.Style.Stroke.Extend = extend;
                this.SelectionViewModel.StyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}