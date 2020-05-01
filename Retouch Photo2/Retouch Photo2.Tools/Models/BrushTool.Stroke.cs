using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.Models;
using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public sealed partial class BrushTool : Page, ITool
    {

        //@ViewModel
        IBrush StrokeBrush { get => this.SelectionViewModel.StrokeBrush; set => this.SelectionViewModel.StrokeBrush = value; }

        //@Static
        /// <summary> Navigate to <see cref="PhotosPage"/> </summary>
        public static Action StrokeImage;


        public void StrokeStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.StrokeBrush == null) return;

            switch (this.StrokeBrush.Type)
            {
                case BrushType.None:
                case BrushType.Color:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        LinearGradientBrush linearGradientBrush = new LinearGradientBrush(startingPoint, point, inverseMatrix);

                        //Selection          
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.StrokeBrush = linearGradientBrush.Clone();
                        });
                        this.StrokeBrush = linearGradientBrush;

                        this._operateMode = BrushOperateMode.LinearEndPoint;
                        this.ViewModel.Invalidate(ViewModels.InvalidateMode.Thumbnail);//Invalidate
                    }
                    break;

                default:
                    {
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        this._operateMode = this.StrokeBrush.ContainsOperateMode(startingPoint, matrix);

                        if (this._operateMode != BrushOperateMode.None)
                        {
                            //Selection
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.StyleManager.StrokeBrush = this.StrokeBrush.Clone();
                            });

                            this.ViewModel.Invalidate(ViewModels.InvalidateMode.Thumbnail);//Invalidate
                        }
                    }
                    break;
            }
        }
        public void StrokeDelta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            if (this.StrokeBrush == null) return;

            switch (this._operateMode)
            {
                case BrushOperateMode.None: break;

                default:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();

                        Vector2 point2 = Vector2.Transform(point, inverseMatrix);

                        //Selection
                        this.StrokeBrush.Controller(this._operateMode, point2);
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.StrokeBrush.Controller(this._operateMode, point2);
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
            }
        }


        public void StrokeTypeChanged(BrushType brushType)
        {
            if (this.StrokeBrush.Type == brushType) return;

            switch (brushType)
            {
                case BrushType.None:
                    {
                        //Selection
                        this.StrokeBrush = new NoneBrush();
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.StrokeBrush = new NoneBrush();
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;

                case BrushType.Color:
                    {
                        //Selection
                        this.SelectionViewModel.Color = Colors.LightGray;
                        this.StrokeBrush = new ColorBrush(Colors.LightGray);
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.StrokeBrush = new ColorBrush(Colors.LightGray);
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;

                case BrushType.LinearGradient:
                    {
                        Transformer transformer = this.SelectionViewModel.Transformer;
                        LinearGradientBrush linearGradientBrush = new LinearGradientBrush(transformer);

                        //Selection
                        this.StrokeBrush = linearGradientBrush;
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.StrokeBrush = linearGradientBrush.Clone();
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        Transformer transformer = this.SelectionViewModel.Transformer;
                        RadialGradientBrush radialGradientBrush = new RadialGradientBrush(transformer);

                        //Selection                       
                        this.StrokeBrush = radialGradientBrush;
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.StrokeBrush = radialGradientBrush.Clone();
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        Transformer transformer = this.SelectionViewModel.Transformer;
                        EllipticalGradientBrush ellipticalGradientBrush = new EllipticalGradientBrush(transformer);

                        //Selection                       
                        this.StrokeBrush = ellipticalGradientBrush;
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.StrokeBrush = ellipticalGradientBrush.Clone();
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
                case BrushType.Image:
                    {
                        BrushTool.StrokeImage?.Invoke();
                    }
                    break;
            }
        }

        public void StrokeShow()
        {
            if (this.StrokeBrush == null) return;

            switch (this.StrokeBrush.Type)
            {
                case BrushType.None: break;

                case BrushType.Color:
                    DrawPage.StrokeColorShowAt(this);
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.StopsPicker.SetArray(this.StrokeBrush.Array);
                    this.StopsFlyout.ShowAt(this);//Flyout
                    break;

                case BrushType.Image:
                    BrushTool.StrokeImage?.Invoke();
                    break;
            }
        }

        

        public void StrokeStopsChanged(CanvasGradientStop[] array)
        {
            if (this._isStopsFlyoutShowed == false) return;

            this.StrokeBrush.Array = (CanvasGradientStop[])array.Clone();

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.StyleManager.StrokeBrush.Array = (CanvasGradientStop[])array.Clone();
            });

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}