using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
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


        bool _isSingleStarted;
        Vector2 _singleStartingPoint;
        InputDevice _inputDevice = InputDevice.None;


        //CanvasControl
        private void CanvasControlInvalidate(InvalidateMode mode)
        {
            //High-Display screen
            if (this.ToolDrawCanvasControl.Dpi > 96.0f)
            {
                switch (mode)
                {
                    case InvalidateMode.Thumbnail:
                        float dpiScale = 96.0f / this.ToolDrawCanvasControl.Dpi;
                        if (dpiScale < 0.4f) dpiScale = 0.4f;
                        if (dpiScale > 1.0f) dpiScale = 1.0f;

                        this.LayerRenderCanvasControl.DpiScale = dpiScale;
                        break;
                    case InvalidateMode.HD:
                        this.LayerRenderCanvasControl.DpiScale = 1.0f;
                        break;
                }
            }

            this.LayerRenderCanvasControl.Invalidate();
            this.ToolDrawCanvasControl.Invalidate();
        }


        // LayerRender & ToolDraw
        private void ConstructCanvasControl()
        {
            this.ViewModel.AccentColor = this.AccentColor;

            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.ViewModel.CanvasTransformer.Size = e.NewSize;
            };

            //LayerRender
            this.LayerRenderCanvasControl.UseSharedDevice = true;
            this.LayerRenderCanvasControl.CustomDevice = LayerManager.CanvasDevice;

            this.LayerRenderCanvasControl.Draw += (sender, args) =>
            {
                //Render & Crad
                this.ViewModel.DrawRenderAndCrad(args.DrawingSession, this.ShadowColor);
            };


            //ToolDraw
            this.ToolDrawCanvasControl.UseSharedDevice = true;
            this.ToolDrawCanvasControl.CustomDevice = LayerManager.CanvasDevice;

            this.ToolDrawCanvasControl.Draw += (sender, args) =>
            {
                switch (this._inputDevice)
                {
                    case InputDevice.None:
                    case InputDevice.Single:
                        {
                            //Tool
                            this.ToolTypeComboBox.Tool.Draw(args.DrawingSession);
                        }
                        break;
                }
                /*
                //Ruler
                if (this.SettingViewModel.IsRuler)
                {
                    args.DrawingSession.DrawRuler(this.ViewModel.CanvasTransformer);
                }
                 */
            };
        }

        private void ConstructCanvasOperator()
        {
            this.ToolDrawCanvasControl.PointerEntered += (s, e) => CoreCursorExtension.None();
            this.ToolDrawCanvasControl.PointerExited += (s, e) => CoreCursorExtension.None();
            this.ToolDrawCanvasControl.PointerMoved += (s, e) =>
            {
                CoreCursorExtension.PointerDeviceType = e.Pointer.PointerDeviceType;

                //Tool
                Vector2 position = e.GetCurrentPoint(this.ToolDrawCanvasControl).Position.ToVector2();
                this.ToolTypeComboBox.Tool.Cursor(position);//Move
            };


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

                this.MenuOverlayCanvas.IsHitTestVisible = this.DrawLayout.IsHitTestVisible = false;//IsHitTestVisible
            };
            canvasOperator.Single_Delta += (point) =>
            {
                //Delta
                if (this._isSingleStarted)
                {
                    //Tool
                    this.ToolTypeComboBox.Tool.Delta(this._singleStartingPoint, point);//Delta

                    return;
                }

                //Started
                if (FanKit.Math.OutNodeDistance(this._singleStartingPoint, point))
                {
                    this._inputDevice = InputDevice.Single;
                    this._isSingleStarted = true;

                    //Tool
                    this.ToolTypeComboBox.Tool.Started(this._singleStartingPoint, point);//Started
                }
            };
            canvasOperator.Single_Complete += (point) =>
            {
                this._inputDevice = InputDevice.None;

                if (this._isSingleStarted == false)
                {
                    //Tool
                    this.ToolTypeComboBox.Tool.Clicke(this._singleStartingPoint);//Complete
                }
                else
                {
                    //Tool
                    bool isOutNodeDistance = FanKit.Math.OutNodeDistance(this._singleStartingPoint, point);
                    this.ToolTypeComboBox.Tool.Complete(this._singleStartingPoint, point, isOutNodeDistance);//Complete
                }

                this.MenuOverlayCanvas.IsHitTestVisible = this.DrawLayout.IsHitTestVisible = true;//IsHitTestVisible
            };


            //Right
            canvasOperator.Right_Start += (point) =>
            {
                this._inputDevice = InputDevice.Right;

                this.ViewModel.CanvasTransformer.CacheMove(point);
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.MenuOverlayCanvas.IsHitTestVisible = this.DrawLayout.IsHitTestVisible = false;//IsHitTestVisible

                //Cursor
                CoreCursorExtension.IsPointerEntered = true;
                CoreCursorExtension.IsManipulationStarted = true;
                CoreCursorExtension.SizeAll();
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

                this.MenuOverlayCanvas.IsHitTestVisible = this.DrawLayout.IsHitTestVisible = true;//IsHitTestVisible

                //Cursor
                CoreCursorExtension.IsPointerEntered = false;
                CoreCursorExtension.IsManipulationStarted = false;
                CoreCursorExtension.SizeAll();
            };


            //Double
            canvasOperator.Double_Start += (center, space) =>
            {
                this._inputDevice = InputDevice.Double;

                this.ViewModel.CanvasTransformer.CachePinch(center, space);

                this.ViewModel.NotifyCanvasTransformerScale();//Notify
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.MenuOverlayCanvas.IsHitTestVisible = this.DrawLayout.IsHitTestVisible = false;//IsHitTestVisible
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

                this.MenuOverlayCanvas.IsHitTestVisible = this.DrawLayout.IsHitTestVisible = true;//IsHitTestVisible
            };

            //Wheel
            canvasOperator.Wheel_Changed += (point, space) =>
            {
                if (this.SettingViewModel.IsWheelToRotate)
                {
                    if (space > 0)
                        this.ViewModel.CanvasTransformerLeftRotate();
                    else
                        this.ViewModel.CanvasTransformerRightRotate();

                    this.ViewModel.Invalidate();//Invalidate
                }
                else
                {
                    if (space > 0)
                        this.ViewModel.CanvasTransformer.ZoomIn(point, 1.05f);
                    else
                        this.ViewModel.CanvasTransformer.ZoomOut(point, 1.05f);

                    this.ViewModel.NotifyCanvasTransformerScale();//Notify
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

        }

    }
}