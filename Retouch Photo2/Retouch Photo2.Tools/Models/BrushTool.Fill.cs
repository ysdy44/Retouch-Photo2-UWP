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
        IBrush Fill { get => this.SelectionViewModel.Fill; set => this.SelectionViewModel.Fill = value; }

        //@Static
        /// <summary> Navigate to <see cref="PhotosPage"/> </summary>
        public static Action FillImage;



        public void FillStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.Fill == null) return;

            //Contains Operate Mode
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            this._operateMode = this.Fill.ContainsOperateMode(startingPoint, matrix);
            
            //InitializeController
            if (this._operateMode == BrushOperateMode.InitializeController)
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
                            this.Fill = new LinearGradientBrush(canvasStartingPoint, canvasPoint);
                        }
                        break;
                }
            }
            
            //Selection
            this.Fill.CacheTransform();
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.Style.CacheFill();

                layer.Style.Fill = this.Fill.Clone();
                layer.Style.Fill.CacheTransform();
            });
        }

        public void FillDelta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Fill == null) return;


            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this._operateMode)
            {
                //InitializeController
                case BrushOperateMode.InitializeController:
                    {
                        //Selection
                        this.Fill.InitializeController(canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.Style.Fill.InitializeController(canvasStartingPoint, canvasPoint);
                        });
                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;

                //Controller
                default:
                    {
                        //Selection
                        this.Fill.Controller(this._operateMode, canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.Style.Fill.Controller(this._operateMode, canvasStartingPoint, canvasPoint);
                        });
                    }
                    break;
            }
        }

        public void FillComplete(Vector2 startingPoint, Vector2 point)
        {          
            //Selection
            if (this.Fill == null) return;

            //History
            IHistoryBase history = new IHistoryBase("Set fill");
            
            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                //History
                var previous = layer.Style.Fill.Clone();
                history.Undos.Push(() => layer.Style.Fill = previous.Clone());

                this.SelectionViewModel.StyleLayer = layer;
            });

            //History
            this.ViewModel.Push(history);
        }



        public void FillTypeChanged(BrushType brushType)
        {
            if (this.Fill.Type == brushType) return;

            switch (brushType)
            {
                case BrushType.Image:
                    BrushTool.FillImage?.Invoke();
                    break;

                default:
                    {
                        //History
                        IHistoryBase history = new IHistoryBase("Set fill type");

                        IBrush brush = this.GetTypeBrush(this.Fill, brushType);
                        if (brushType == BrushType.Color) this.SelectionViewModel.Color = brush.Color;


                        //Selection
                        this.Fill = brush;
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            //History
                            var previous = layer.Style.Fill.Clone();
                            history.Undos.Push(() => layer.Style.Fill = previous.Clone());

                            layer.Style.Fill = brush.Clone();
                            this.SelectionViewModel.StyleLayer = layer;
                        });

                        //History
                        this.ViewModel.Push(history);

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
            }
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
                    this.StopsPicker.SetArray(this.Fill.Array);
                    this.StopsFlyout.ShowAt(this);//Flyout
                    break;

                case BrushType.Image:
                    this.ExtendComboBox.Extend = this.Fill.Extend;
                    this.ImageFlyout.ShowAt(this);//Flyout
                    break;
            }
        }

                

        public void FillStopsChanged(CanvasGradientStop[] array)
        {
            if (this._isStopsFlyoutShowed == false) return;

            this.Fill.Array = (CanvasGradientStop[])array.Clone();

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.Style.Fill.Array = (CanvasGradientStop[])array.Clone();
                this.SelectionViewModel.StyleLayer = layer;
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        public void FillExtendChanged(CanvasEdgeBehavior extend)
        {
            this.ExtendComboBox.Extend = extend;

            //Selection
            this.Fill.Extend = extend;
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.Style.Fill.Extend = extend;
                this.SelectionViewModel.StyleLayer = layer;
            });

            this.ViewModel.Invalidate();//Invalidate
        } 

    }
}