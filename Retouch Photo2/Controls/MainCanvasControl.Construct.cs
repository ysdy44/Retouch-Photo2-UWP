using FanKit.Transformers;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Retouch_Photo2.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainCanvasControl" />. 
    /// </summary>
    public partial class MainCanvasControl : UserControl
    {

        //@Construct
        public MainCanvasControl()
        {
            CanvasControl canvasControl = this.ViewModel.CanvasDevice;
            if (canvasControl == null) return;

            this.Content = canvasControl;

            this.CanvasOperator = new CanvasOperator
            {
                DestinationControl = canvasControl
             };

            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.ViewModel.CanvasTransformer.Size = e.NewSize;
            };



            #region Draw


            //Draw
            canvasControl.Draw += (sender, args) =>
            {
                //Render & Crad
                this._drawRenderAndCrad(args.DrawingSession);

                switch (this._inputDevice)
                {
                    case InputDevice.None:
                    case InputDevice.Single:
                        {
                            //Tool & Bound
                            this._drawToolAndBound(sender, args.DrawingSession);

                            //Ruler
                            if (this.ViewModel.CanvasRulerVisible)
                            {
                                args.DrawingSession.DrawRuler(this.ViewModel.CanvasTransformer);
                            }
                        }
                        break;
                }
            };


            #endregion


            #region CanvasOperator


            //Single
            this.CanvasOperator.Single_Start += (point) =>
            {
                this._inputDevice = InputDevice.None;
                this._isSingleStarted = false;
                this._singleStartingPoint = point;

                //Tool
                this.TipViewModel.Tool.Starting(point);//Starting

                this.ViewModel.CanvasHitTestVisible = false;//IsHitTestVisible
            };
            this.CanvasOperator.Single_Delta += (point) =>
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
            this.CanvasOperator.Single_Complete += (point) =>
            {
                this._inputDevice = InputDevice.None;

                if (this._isSingleStarted==false)
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
            this.CanvasOperator.Right_Start += (point) =>
            {
                this._inputDevice = InputDevice.Right;

                this.ViewModel.CanvasTransformer.CacheMove(point);
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.ViewModel.CanvasHitTestVisible = false;//IsHitTestVisible
            };
            this.CanvasOperator.Right_Delta += (point) =>
            {
                this.ViewModel.CanvasTransformer.Move(point);
                this.ViewModel.Invalidate();//Invalidate
            };
            this.CanvasOperator.Right_Complete += (point) =>
            {
                this._inputDevice = InputDevice.None;

                this.ViewModel.CanvasTransformer.Move(point);
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate

                this.ViewModel.CanvasHitTestVisible = true;//IsHitTestVisible
            };


            //Double
            this.CanvasOperator.Double_Start += (center, space) =>
            {
                this._inputDevice = InputDevice.Double;

                this.ViewModel.CanvasTransformer.CachePinch(center, space);

                this.ViewModel.NotifyCanvasTransformerScale();//Notify
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);

                this.ViewModel.CanvasHitTestVisible = false;//IsHitTestVisible
            };
            this.CanvasOperator.Double_Delta += (center, space) =>
            {
                this.ViewModel.CanvasTransformer.Pinch(center, space);

                this.ViewModel.NotifyCanvasTransformerScale();//Notify
                this.ViewModel.Invalidate();//Invalidate
            };
            this.CanvasOperator.Double_Complete += (center, space) =>
            {
                this._inputDevice = InputDevice.None;

                this.ViewModel.NotifyCanvasTransformerScale();//Notify
                this.ViewModel.Invalidate(InvalidateMode.HD);

                this.ViewModel.CanvasHitTestVisible = true;//IsHitTestVisible
            };

            //Wheel
            this.CanvasOperator.Wheel_Changed += (point, space) =>
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