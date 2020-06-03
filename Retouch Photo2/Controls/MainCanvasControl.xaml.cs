using FanKit.Transformers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    public sealed partial class MainCanvasControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        bool _isSingleStarted;
        Vector2 _singleStartingPoint;
        InputDevice _inputDevice = InputDevice.None;


        //@Construct
        public MainCanvasControl()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.ViewModel.CanvasTransformer.Size = e.NewSize;
            };

            this.ViewModel.InvalidateAction += (InvalidateMode mode) =>
            {
                switch (mode)
                {
                    case InvalidateMode.Thumbnail:
                        this.CanvasControl.DpiScale = 0.4f;
                        break;
                    case InvalidateMode.HD:
                        this.CanvasControl.DpiScale = 1.0f;
                        break;
                }

                this.CanvasControl.Invalidate();
                this.ToolCanvasControl.Invalidate();
            };


            #region Draw


            //Draw
            this.CanvasControl.UseSharedDevice = true;
            this.CanvasControl.CustomDevice = this.ViewModel.CanvasDevice;

            this.CanvasControl.Draw += (sender, args) =>
            {
                //Render & Crad
                this._drawRenderAndCrad(args.DrawingSession);
            };


            //Draw
            this.ToolCanvasControl.UseSharedDevice = true;
            this.ToolCanvasControl.CustomDevice = this.ViewModel.CanvasDevice;

            this.ToolCanvasControl.Draw += (sender, args) =>
            { 
                switch (this._inputDevice)
                {
                    case InputDevice.None:
                    case InputDevice.Single:
                        {
                            //Tool & Bound
                            this._drawToolAndBound(sender, args.DrawingSession);
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
                DestinationControl = this.ToolCanvasControl
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
                    this.TipViewModel.Tool.Delta(this._singleStartingPoint, point);//Delta

                    return;
                }

                //Started
                if (FanKit.Math.OutNodeDistance(this._singleStartingPoint, point))
                {
                    this._inputDevice = InputDevice.Single;
                    this._isSingleStarted = true;

                    //Tool
                    this.TipViewModel.Tool.Started(this._singleStartingPoint, point);//Started
                }
            };
            canvasOperator.Single_Complete += (point) =>
            {
                this._inputDevice = InputDevice.None;

                if (this._isSingleStarted == false)
                {
                    //Tool
                    this.TipViewModel.Tool.Clicke(this._singleStartingPoint);//Complete
                }
                else
                {
                    //Tool
                    bool isOutNodeDistance = FanKit.Math.OutNodeDistance(this._singleStartingPoint, point);
                    this.TipViewModel.Tool.Complete(this._singleStartingPoint, point, isOutNodeDistance);//Complete
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