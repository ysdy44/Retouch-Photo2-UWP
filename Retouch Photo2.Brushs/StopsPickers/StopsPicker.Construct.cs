using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Stops picker.
    /// </summary>
    public sealed partial class StopsPicker : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.CopyTextBlock.Text = resource.GetString("/Tools/Brush_Stop_Copy");
            this.RemoveTextBlock.Text = resource.GetString("/Tools/Brush_Stop_Remove");
            this.ReserveTextBlock.Text = resource.GetString("/Tools/Brush_Stop_Reverse");

            this.AlphaTextBlock.Text = resource.GetString("/Tools/Brush_Stop_Alpha");
            this.OffsetTextBlock.Text = resource.GetString("/Tools/Brush_Stop_Offset");
        }


        private void ConstructCanvas()
        {
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.Size.SIzeChange((float)e.NewSize.Width, (float)e.NewSize.Height);
            };
            this.CanvasControl.CreateResources += (sender, args) =>
            {
                float width = (float)sender.ActualWidth;
                float height = (float)sender.ActualHeight;
                this.GrayAndWhiteBackground = new CanvasRenderTarget(sender, width, height);

                using (CanvasDrawingSession drawingSession = this.GrayAndWhiteBackground.CreateDrawingSession())
                {
                    CanvasBitmap bitmap = GreyWhiteMeshHelpher.GetGreyWhiteMesh(sender);
                    ICanvasImage extendMesh = GreyWhiteMeshHelpher.GetBorderExtendMesh(height / 4, bitmap);
                    drawingSession.DrawImage(extendMesh);
                }
            };

            this.CanvasControl.Draw += (sender, args) =>
            {
                if (this.array == null) return;

                //Background
                args.DrawingSession.DrawImage(this.GrayAndWhiteBackground);

                //LinearGradient
                this.Size.DrawLinearGradient(args.DrawingSession, this.CanvasControl, this.array);

                //Lines
                this.Size.DrawLines(args.DrawingSession);

                //Nodes
                this.Size.DrawNodes(args.DrawingSession, this.Manager);
            };
        }


        private void ConstructCanvasOperator()
        {
            this.CanvasOperator.Single_Start += (point) =>
            {
                if (this.array == null) return;

                this.Manager.Index = -1;
                this.Manager.IsLeft = false;
                this.Manager.IsRight = false;

                //Stops
                for (int i = this.Manager.Count - 1; i >= 0; i--)
                {
                    float x = this.Size.OffsetToPosition(this.Manager.Stops[i].Position);
                    if (Math.Abs(x - point.X) < StopsSize.Radius)
                    {
                        this.Manager.Index = i;
                        CanvasGradientStop stop = this.Manager.Stops[i];
                        this.StopChanged(stop.Color, (int)(stop.Position * 100), true);//Delegate
                        return;
                    }
                }

                //Left
                bool isLeft = (Math.Abs(this.Size.Left - point.X) < StopsSize.Radius);
                if (isLeft)
                {
                    this.Manager.IsLeft = true;
                    this.StopChanged(this.Manager.LeftColor, 0, false);//Delegate
                    return;
                }

                //Right
                bool isRight = (Math.Abs(this.Size.Right - point.X) < StopsSize.Radius);
                if (isRight)
                {
                    this.Manager.IsRight = true;
                    this.StopChanged(this.Manager.RightColor, 100, false);//Delegate
                    return;
                }


                //Add
                float offset = this.Size.PositionToOffset(point.X);
                CanvasGradientStop addStop = this.Manager.InsertNewStepByOffset(offset);

                this.Manager.Stops.Add(addStop);
                this.Manager.Index = this.Manager.Count - 1;

                CanvasGradientStop[]
                    array = this.Manager.GenerateArrayFromDate();
                this.SetArray(array);

                this.StopChanged(addStop.Color, (int)(addStop.Position * 100), true);//Delegate
                return;
            };
            this.CanvasOperator.Single_Delta += (point) =>
            {
                if (this.array == null) return;

                if (this.Manager.IsLeft) return;
                if (this.Manager.IsRight) return;

                float offset = this.Size.PositionToOffset(point.X);
                this.SetOffset(offset);

                this.OffsetChanged(offset);

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this, this.array);//Delegate
            };
            this.CanvasControl.PointerReleased += (s, e) =>
            {
                if (this.array == null) return;

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this, this.array);//Delegate
            };
        }


        private void ConstructStop()
        {
            // Copy a stop on right
            this.CopyButton.Tapped += (s, e) =>
            {
                if (this.array == null) return;

                CanvasGradientStop stop = this.Manager.CopyStopOnRight();
                this.StopChanged(stop.Color, (int)(stop.Position * 100.0f), true);

                this.array = this.Manager.GenerateArrayFromDate();

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this, this.array);//Delegate
            };

            // Reserve all stops. 
            this.ReserveButton.Tapped += (s, e) =>
            {
                if (this.array == null) return;

                this.Manager.Reserve();
                this.Manager.Copy(this.array);

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this, this.array);//Delegate
            };

            // Remove current stop.
            this.RemoveButton.Tapped += (s, e) =>
            {
                if (this.array == null) return;

                if (this.Manager.IsLeft) return;
                if (this.Manager.IsRight) return;

                Color color = this.Manager.Remove();
                this.StopChanged(color, 0, false);

                this.array = this.Manager.GenerateArrayFromDate();

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this, this.array);//Delegate
            };
        }


    }
}