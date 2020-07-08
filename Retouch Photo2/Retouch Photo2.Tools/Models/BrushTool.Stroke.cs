﻿using FanKit.Transformers;
using HSVColorPickers;
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
                    Retouch_Photo2.DrawPage.FrameNavigatePhotosPage?.Invoke(PhotosPageMode.StrokeImage);//Delegate
                }
                else
                {
                    this.StrokeTypeChanged(brushType);
                    this.ShowControl.Invalidate();
                }
            };
        }


        //////////////////////////


        private void StrokeStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.Stroke == null) return;

            //Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this.HandleMode = this.Stroke.ContainsHandleMode(startingPoint, matrix);

            //InitializeController
            if (this.HandleMode == BrushHandleMode.None)
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

        private void StrokeDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Selection
            if (this.Stroke == null) return;

            switch (this.HandleMode)
            {
                //InitializeController
                case BrushHandleMode.None:
                    {
                        //Selection
                        this.Stroke.InitializeController(canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layerage.RefactoringParentsRender();
                            layer.Style.Stroke.InitializeController(canvasStartingPoint, canvasPoint);
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;

                //Controller
                default:
                    {
                        //Selection
                        this.Stroke.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layerage.RefactoringParentsRender();
                            layer.Style.Stroke.Controller(this.HandleMode, canvasStartingPoint, canvasPoint);
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
            }
        }

        private void StrokeComplete()
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
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Stroke = previous.Clone();
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


        private void StrokeTypeChanged(BrushType brushType, Photo photo = null)
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
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Stroke = previous.Clone();
                };


                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                Transformer transformer = layer.Transform.Transformer;
                layer.Style.Stroke.TypeChange(brushType, transformer, photo);

                this.SelectionViewModel.StandStyleLayerage = layerage;


                // Set stroke Onces: lock
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

        private void StrokeShow()
        {
            if (this.Stroke == null) return;

            switch (this.Stroke.Type)
            {
                case BrushType.None: break;

                case BrushType.Color:
                    DrawPage.StrokeColorShowAt(this.ShowControl);
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
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set stroke");

            //Selection
            this.Stroke.Stops = array.CloneArray();
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Stroke.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Stroke = previous.Clone();
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Style.Stroke.Stops = array.CloneArray();

                this.SelectionViewModel.StandStyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
            this.ShowControl.Invalidate();//Invalidate
        }

        private void StrokeStopsChangeStarted(CanvasGradientStop[] array)
        {
            //Selection
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;
                layer.Style.CacheStroke();
            });

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        private void StrokeStopsChangeDelta(CanvasGradientStop[] array)
        {
            //Selection
            this.Stroke.Stops = array.CloneArray();
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //Refactoring
                layer.IsRefactoringRender = true;
                layerage.RefactoringParentsRender();
                layer.Style.Stroke.Stops = array.CloneArray();
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        private void StrokeStopsChangeCompleted(CanvasGradientStop[] array)
        {
            this.Stroke.Stops = array.CloneArray();

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set stroke");

            //Selection
            this.Stroke.Stops = array.CloneArray();
            this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.Style.Stroke.Clone();
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Stroke = previous.Clone();
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Style.Stroke.Stops = array.CloneArray();

                this.SelectionViewModel.StandStyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            this.ShowControl.Invalidate();//Invalidate
        }

        private void StrokeExtendChanged(CanvasEdgeBehavior extend)
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
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Style.Stroke.Extend = previous;
                };

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Style.Stroke.Extend = extend;

                this.SelectionViewModel.StandStyleLayerage = layerage;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}