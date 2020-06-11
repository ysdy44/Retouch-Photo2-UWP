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
                    Retouch_Photo2.DrawPage.FrameNavigatePhotosPage?.Invoke(PhotosPageMode.FillImage);
                }
                else
                {
                    this.FillTypeChanged(brushType);
                    this.ShowControl.Invalidate();
                }
            };
        }


        //////////////////////////


        public void FillStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.Fill == null) return;

            //Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this.OperateMode = this.Fill.ContainsOperateMode(startingPoint, matrix);

            //InitializeController
            if (this.OperateMode == BrushHandleMode.None)
            {
                switch (this.Fill.Type)
                {
                    case BrushType.None:
                    case BrushType.Color:
                        {
                            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                            //Selection          
                            this.Fill = BrushBase.LinearGradientBrush(canvasStartingPoint, canvasPoint);
                        }
                        break;
                }
            }

            //Selection
            this.Fill.CacheTransform();
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.Style.CacheFill();

                layer.Style.Fill = this.Fill.Clone();
                layer.Style.Fill.CacheTransform();
            });
        }

        public void FillDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Selection
            if (this.Fill == null) return;

            switch (this.OperateMode)
            {
                //InitializeController
                case BrushHandleMode.None:
                    {
                        //Selection
                        this.Fill.InitializeController(canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layerage.RefactoringParentsRender();
                            layer.Style.Fill.InitializeController(canvasStartingPoint, canvasPoint);
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;

                //Controller
                default:
                    {
                        //Selection
                        this.Fill.Controller(this.OperateMode, canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layerage.RefactoringParentsRender();
                            layer.Style.Fill.Controller(this.OperateMode, canvasStartingPoint, canvasPoint);
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
            }
        }

        public void FillComplete()
        {
            //Selection
            if (this.Fill == null) return;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set fill");

            //Selection
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.StartingFill.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Fill = previous.Clone();
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();

                this.SelectionViewModel.StandStyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(history);
        }


        //////////////////////////


        public void FillTypeChanged(BrushType brushType, Photo photo = null)
        {
            if (this.Fill.Type == brushType) return;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set fill type");

            bool _lock = false;

            //Selection
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Fill.Clone(); ;
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Fill = previous.Clone();
                };


                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                Transformer transformer = layer.Transform.Transformer;
                layer.Style.Fill.TypeChange(brushType, transformer, photo);

                this.SelectionViewModel.StandStyleLayerage = layerage;


                // Set fill Onces: lock
                if (_lock == false)
                {
                    _lock = true;
                    this.Fill = layer.Style.Fill.Clone();

                    if (this.Fill.Type == BrushType.Color) this.SelectionViewModel.Color = this.Fill.Color;
                }
            });


            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }

        public void FillShow()
        {
            if (this.Fill == null) return;

            switch (this.Fill.Type)
            {
                case BrushType.None: break;

                case BrushType.Color:
                    DrawPage.FillColorShowAt(this);
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


        public void FillStopsChanged(CanvasGradientStop[] array)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set fill");

            //Selection
            this.Fill.Stops = (CanvasGradientStop[])array.Clone();
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Fill.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Fill = previous.Clone();
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Style.Fill.Stops = (CanvasGradientStop[])array.Clone();

                this.SelectionViewModel.StandStyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
            this.ShowControl.Invalidate();//Invalidate
        }

        public void FillStopsChangeStarted(CanvasGradientStop[] array)
        {
            //Selection
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;
                layer.Style.CacheFill();
            });

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void FillStopsChangeDelta(CanvasGradientStop[] array)
        {
            //Selection
            this.Fill.Stops = (CanvasGradientStop[])array.Clone();
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //Refactoring
                layer.IsRefactoringRender = true;
                layerage.RefactoringParentsRender();
                layer.Style.Fill.Stops = (CanvasGradientStop[])array.Clone();
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        public void FillStopsChangeCompleted(CanvasGradientStop[] array)
        {
            this.Fill.Stops = (CanvasGradientStop[])array.Clone();

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set fill");

            //Selection
            this.Fill.Stops = (CanvasGradientStop[])array.Clone();
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Fill.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Fill = previous.Clone();
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Style.Fill.Stops = (CanvasGradientStop[])array.Clone();

                this.SelectionViewModel.StandStyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            this.ShowControl.Invalidate();//Invalidate
        }

        public void FillExtendChanged(CanvasEdgeBehavior extend)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set fill extend");

            //Selection
            this.Fill.Extend = extend;
            this.ExtendComboBox.Extend = extend;
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Fill.Extend;
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Fill.Extend = previous;
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Style.Fill.Extend = extend;

                this.SelectionViewModel.StandStyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}