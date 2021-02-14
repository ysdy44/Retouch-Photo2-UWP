﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //@Content
        private Color AccentColor => this.AccentColorBrush.Color;
        private Color ShadowColor => this.ShadowColorBrush.Color;
        private CanvasControl LayerRenderCanvasControl => this.DrawLayout.LayerRenderCanvasControl;
        private CanvasControl ToolDrawCanvasControl => this.DrawLayout.ToolDrawCanvasControl;


        bool _isSingleStarted;
        Vector2 _singleStartingPoint;
        InputDevice _inputDevice = InputDevice.None;


        //MainCanvasControl
        private void ConstructCanvasControl()
        {
            this.ViewModel.AccentColor = this.AccentColor;


            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.ViewModel.CanvasTransformer.Size = e.NewSize;
            };


            //High-Display screen
            if (this.LayerRenderCanvasControl.Dpi > 96.0f)
            {
                float dpiScale = 96.0f / this.LayerRenderCanvasControl.Dpi;
                if (dpiScale < 0.4f) dpiScale = 0.4f;
                if (dpiScale > 1.0f) dpiScale = 1.0f;

                this.ViewModel.InvalidateAction += (InvalidateMode mode) =>
                {
                    switch (mode)
                    {
                        case InvalidateMode.Thumbnail:
                            this.LayerRenderCanvasControl.DpiScale = dpiScale;
                            break;
                        case InvalidateMode.HD:
                            this.LayerRenderCanvasControl.DpiScale = 1.0f;
                            break;
                    }

                    this.LayerRenderCanvasControl.Invalidate();
                    this.ToolDrawCanvasControl.Invalidate();
                };
            }
            //Low-Display screen
            else
            {
                this.ViewModel.InvalidateAction += (_) =>
                {
                    this.LayerRenderCanvasControl.Invalidate();
                    this.ToolDrawCanvasControl.Invalidate();
                };
            }


            #region LayerRender & ToolDraw


            //LayerRender
            this.LayerRenderCanvasControl.UseSharedDevice = true;
            this.LayerRenderCanvasControl.CustomDevice = this.ViewModel.CanvasDevice;

            this.LayerRenderCanvasControl.Draw += (sender, args) =>
            {
                //Render & Crad
                this._drawRenderAndCrad(args.DrawingSession);
            };


            //ToolDraw
            this.ToolDrawCanvasControl.UseSharedDevice = true;
            this.ToolDrawCanvasControl.CustomDevice = this.ViewModel.CanvasDevice;

            this.ToolDrawCanvasControl.Draw += (sender, args) =>
            {
                switch (this._inputDevice)
                {
                    case InputDevice.None:
                    case InputDevice.Single:
                        {
                            //Tool
                            ToolManager.Instance.Draw(args.DrawingSession);
                        }
                        break;
                }

                //Ruler
                if (this.SettingViewModel.IsRuler)
                {
                    args.DrawingSession.DrawRuler(this.ViewModel.CanvasTransformer);
                }
            };


            #endregion


            #region CanvasOperator


            CanvasOperator canvasOperator = new CanvasOperator
            {
                DestinationControl = this.ToolDrawCanvasControl
            };

            //Single
            canvasOperator.Single_Start += (point) =>
            {
                this._inputDevice = InputDevice.None;
                this._isSingleStarted = false;
                this._singleStartingPoint = point;

                this.ViewModel.CanvasHitTestVisible = false;//IsHitTestVisible
            };
            canvasOperator.Single_Delta += (point) =>
            {
                //Delta
                if (this._isSingleStarted)
                {
                    //Tool
                    ToolManager.Instance.Delta(this._singleStartingPoint, point);//Delta

                    return;
                }

                //Started
                if (FanKit.Math.OutNodeDistance(this._singleStartingPoint, point))
                {
                    this._inputDevice = InputDevice.Single;
                    this._isSingleStarted = true;

                    //Tool
                    ToolManager.Instance.Started(this._singleStartingPoint, point);//Started
                }
            };
            canvasOperator.Single_Complete += (point) =>
            {
                this._inputDevice = InputDevice.None;

                if (this._isSingleStarted == false)
                {
                    //Tool
                    ToolManager.Instance.Clicke(this._singleStartingPoint);//Complete
                }
                else
                {
                    //Tool
                    bool isOutNodeDistance = FanKit.Math.OutNodeDistance(this._singleStartingPoint, point);
                    ToolManager.Instance.Complete(this._singleStartingPoint, point, isOutNodeDistance);//Complete
                }

                this.ViewModel.CanvasHitTestVisible = true;//IsHitTestVisible
            };


            //Right
            canvasOperator.Right_Start += (point) =>
            {
                this._inputDevice = InputDevice.Right;

                this.ViewModel.CanvasTransformer.CacheMove(point);
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.ViewModel.CanvasHitTestVisible = false;//IsHitTestVisible
            };
            canvasOperator.Right_Delta += (point) =>
            {
                this.ViewModel.CanvasTransformer.Move(point);
                this.ViewModel.Invalidate();//Invalidate
            };
            canvasOperator.Right_Complete += (point) =>
            {
                this._inputDevice = InputDevice.None;

                this.ViewModel.CanvasTransformer.Move(point);
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

                this.ViewModel.CanvasHitTestVisible = true;//IsHitTestVisible
            };


            //Double
            canvasOperator.Double_Start += (center, space) =>
            {
                this._inputDevice = InputDevice.Double;

                this.ViewModel.CanvasTransformer.CachePinch(center, space);

                this.ViewModel.NotifyCanvasTransformerScale();//Notify
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.ViewModel.CanvasHitTestVisible = false;//IsHitTestVisible
            };
            canvasOperator.Double_Delta += (center, space) =>
            {
                this.ViewModel.CanvasTransformer.Pinch(center, space);

                this.ViewModel.NotifyCanvasTransformerScale();//Notify
                this.ViewModel.Invalidate();//Invalidate
            };
            canvasOperator.Double_Complete += (center, space) =>
            {
                this._inputDevice = InputDevice.None;

                this.ViewModel.NotifyCanvasTransformerScale();//Notify
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

                this.ViewModel.CanvasHitTestVisible = true;//IsHitTestVisible
            };

            //Wheel
            canvasOperator.Wheel_Changed += (point, space) =>
            {
                if (space > 0)
                    this.ViewModel.CanvasTransformer.ZoomIn(point);
                else
                    this.ViewModel.CanvasTransformer.ZoomOut(point);

                this.ViewModel.NotifyCanvasTransformerScale();//Notify
                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion

        }
    }
}