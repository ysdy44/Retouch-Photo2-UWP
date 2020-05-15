﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
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
        IBrush FillBrush { get => this.SelectionViewModel.FillBrush; set => this.SelectionViewModel.FillBrush = value; }

        //@Static
        /// <summary> Navigate to <see cref="PhotosPage"/> </summary>
        public static Action FillImage;


        public void FillStarted(Vector2 startingPoint, Vector2 point)
        {
            if (this.FillBrush == null) return;

            switch (this.FillBrush.Type)
            {
                case BrushType.None:
                case BrushType.Color:
                    {
                        this.BrushTypeComboBox.EaseStoryboard.Begin();

                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        LinearGradientBrush linearGradientBrush = new LinearGradientBrush(startingPoint, point, inverseMatrix);

                        //Selection          
                        this.FillBrush = linearGradientBrush;
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.FillBrush = linearGradientBrush.Clone();
                            this.SelectionViewModel.StyleLayer = layer;
                        });

                        this._operateMode = BrushOperateMode.LinearEndPoint;
                        this.ViewModel.Invalidate(ViewModels.InvalidateMode.Thumbnail);//Invalidate
                    }
                    break;

                default:
                    {
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        this._operateMode = this.FillBrush.ContainsOperateMode(startingPoint, matrix);

                        if (this._operateMode != BrushOperateMode.None)
                        {
                            //Selection
                            this.FillBrush.CacheTransform();
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.StyleManager.FillBrush = this.FillBrush.Clone();
                                layer.StyleManager.FillBrush.CacheTransform();
                                this.SelectionViewModel.StyleLayer = layer;
                            });

                            this.ViewModel.Invalidate(ViewModels.InvalidateMode.Thumbnail);//Invalidate
                        }
                    }
                    break;
            }
        }
        public void FillDelta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            if (this.FillBrush == null) return;

            switch (this._operateMode)
            {
                case BrushOperateMode.None: break;

                default:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();

                        Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                        Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
                        
                        //Selection
                        this.FillBrush.Controller(this._operateMode, canvasStartingPoint, canvasPoint);
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.FillBrush.Controller(this._operateMode, canvasStartingPoint, canvasPoint);
                            this.SelectionViewModel.StyleLayer = layer;
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
            }
        }


        public void FillTypeChanged(BrushType brushType)
        {
            if (this.FillBrush.Type == brushType) return;

            switch (brushType)
            {
                case BrushType.None:
                    {
                        //Selection
                        this.FillBrush = new NoneBrush();
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.FillBrush = new NoneBrush();
                            this.SelectionViewModel.StyleLayer = layer;
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;

                case BrushType.Color:
                    {
                        //Selection
                        this.SelectionViewModel.Color = Colors.LightGray;
                        this.FillBrush = new ColorBrush(Colors.LightGray);
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.FillBrush = new ColorBrush(Colors.LightGray);
                            this.SelectionViewModel.StyleLayer = layer;
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;

                case BrushType.LinearGradient:
                    {
                        Transformer transformer = this.SelectionViewModel.Transformer;
                        LinearGradientBrush linearGradientBrush = this.FillBrush.Array == null ? new LinearGradientBrush(transformer) : new LinearGradientBrush(transformer)
                        {
                            Array = this.FillBrush.Array
                        };

                        //Selection
                        this.FillBrush = linearGradientBrush;
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.FillBrush = linearGradientBrush.Clone();
                            this.SelectionViewModel.StyleLayer = layer;
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        Transformer transformer = this.SelectionViewModel.Transformer;
                        RadialGradientBrush radialGradientBrush = this.FillBrush.Array == null ? new RadialGradientBrush(transformer) : new RadialGradientBrush(transformer)
                        {
                            Array = this.FillBrush.Array
                        };

                        //Selection                       
                        this.FillBrush = radialGradientBrush;
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.FillBrush = radialGradientBrush.Clone();
                            this.SelectionViewModel.StyleLayer = layer;
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        Transformer transformer = this.SelectionViewModel.Transformer;
                        EllipticalGradientBrush ellipticalGradientBrush = this.FillBrush.Array == null ? new EllipticalGradientBrush(transformer) : new EllipticalGradientBrush(transformer)
                        {
                            Array = this.FillBrush.Array
                        };

                        //Selection                       
                        this.FillBrush = ellipticalGradientBrush;
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            layer.StyleManager.FillBrush = ellipticalGradientBrush.Clone();
                            this.SelectionViewModel.StyleLayer = layer;
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
                case BrushType.Image:
                    {
                        BrushTool.FillImage?.Invoke();
                    }
                    break;
            }
        }

        public void FillShow()
        {
            if (this.FillBrush == null) return;

            switch (this.FillBrush.Type)
            {
                case BrushType.None: break;

                case BrushType.Color:
                    DrawPage.FillColorShowAt(this);
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.StopsPicker.SetArray(this.FillBrush.Array);
                    this.StopsFlyout.ShowAt(this);//Flyout
                    break;

                case BrushType.Image:
                    this.ExtendComboBox.Extend = this.FillBrush.Extend;
                    this.ImageFlyout.ShowAt(this);//Flyout
                    break;
            }
        }

                

        public void FillStopsChanged(CanvasGradientStop[] array)
        {
            if (this._isStopsFlyoutShowed == false) return;

            this.FillBrush.Array = (CanvasGradientStop[])array.Clone();

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.StyleManager.FillBrush.Array = (CanvasGradientStop[])array.Clone();
                this.SelectionViewModel.StyleLayer = layer;
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        public void FillExtendChanged(CanvasEdgeBehavior extend)
        {
            this.ExtendComboBox.Extend = extend;

            //Selection
            this.FillBrush.Extend = extend;
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.StyleManager.FillBrush.Extend = extend;
                this.SelectionViewModel.StyleLayer = layer;
            });

            this.ViewModel.Invalidate();//Invalidate
        } 

    }
}