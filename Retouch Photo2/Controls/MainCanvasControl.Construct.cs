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
            CanvasControl canvasControl = new CanvasControl
            {
                UseSharedDevice = true,
                CustomDevice = this.ViewModel.CanvasDevice,
            };
            CanvasOperator canvasOperator = new CanvasOperator { DestinationControl = canvasControl };

            this.Content = canvasControl;
            canvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.ViewModel.CanvasTransformer.Size = e.NewSize;
            };


            //ViewModel
            this.ConstructViewModel(canvasControl);
            this.ConstructKeyboardViewModel();


            #region Draw


            //Draw
            canvasControl.CreateResources += (sender, args) =>
            {
                this.ViewModel.CanvasTransformer.Size = new Size(sender.ActualWidth, sender.ActualHeight);
            };
            canvasControl.Draw += (sender, args) =>
            {
                //Render & Crad
                this._drawRenderAndCrad(args.DrawingSession);

                //Tool & Bound                
                this._drawToolAndBound(sender, args.DrawingSession);

                //Ruler
                if (this.ViewModel.CanvasRulerVisible)
                {
                    args.DrawingSession.DrawRuler(this.ViewModel.CanvasTransformer);
                }
            };


            #endregion


            #region CanvasOperator


            //Single
            canvasOperator.Single_Start += (point) =>
            {
                this._isSingleStarted = false;
                this._singleStartingPoint = point;

                //Tool
                this.TipViewModel.Tool.Starting(point);//Starting

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
                    this._isSingleStarted = true;

                    //Tool
                    this.TipViewModel.Tool.Started(this._singleStartingPoint, point);//Started
                }
            };
            canvasOperator.Single_Complete += (point) =>
            {
                //Tool
                this.TipViewModel.Tool.Complete(this._singleStartingPoint, point, this._isSingleStarted);//Complete

                this.ViewModel.CanvasHitTestVisible = true;//IsHitTestVisible
            };


            //Right
            canvasOperator.Right_Start += (point) =>
            {
                this.ViewModel.CanvasTransformer.CacheMove(point);
                this.ViewModel.CanvasHitTestVisible = false;//IsHitTestVisible
            };
            canvasOperator.Right_Delta += (point) =>
            {
                this.ViewModel.CanvasTransformer.Move(point);
                this.ViewModel.Invalidate();//Invalidate
            };
            canvasOperator.Right_Complete += (point) =>
            {
                this.ViewModel.CanvasTransformer.Move(point);
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                this.ViewModel.CanvasHitTestVisible = true;//IsHitTestVisible
            };


            //Double
            canvasOperator.Double_Start += (center, space) =>
            {
                this.ViewModel.CanvasTransformer.CachePinch(center, space);

                this.ViewModel.NotifyCanvasTransformerScale();//Notify
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);

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
                this.ViewModel.NotifyCanvasTransformerScale();//Notify
                this.ViewModel.Invalidate(InvalidateMode.HD);

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