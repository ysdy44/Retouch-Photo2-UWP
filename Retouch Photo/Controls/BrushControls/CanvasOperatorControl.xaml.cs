using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo.ViewModels;
using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Controls.BrushControls
{
    public sealed partial class CanvasOperatorControl : UserControl
    {
        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        //Delegate
        public delegate void StopChangedHandler(CanvasGradientStop stop, bool isEnabled);
        public event StopChangedHandler StopChanged = null;
        public delegate void OffsetChangedHandler(float offset);
        public event OffsetChangedHandler OffsetChanged = null;
        
        StopsSize Size = new StopsSize();        
        StopsManager Manager = new StopsManager();//Left+o o o+right: 5 stops
        CanvasGradientStop[] Array; //o o o o o: 5 stops


        public CanvasOperatorControl()
        {
            this.InitializeComponent();
            this.Array = this.Manager.GetArray();
            
            //CanvasControl
            this.CanvasControl.SizeChanged += (s, e) => this.Size.SIzeChange((float)e.NewSize.Width, (float)e.NewSize.Height);
            this.CanvasControl.CreateResources += (sender, args) => { };
            this.CanvasControl.Draw += (sender, args) =>
            {
                //Background
                this.Size.DrawBackground(args.DrawingSession, this.ViewModel.CanvasDevice, this.Array);

                //Stops
                for (int i = 0; i < this.Manager.Count; i++)
                {
                    CanvasGradientStop stop = this.Manager.Stops[i];
                    this.Manager.DrawNode(args.DrawingSession, this.Size.OffsetToPosition(stop.Position), this.Size.Center, stop.Color, (i == this.Manager.Index));
                }
                this.Manager.DrawLeftNode(args.DrawingSession, this.Size.Left, this.Size.Center);
                this.Manager.DrawRightNode(args.DrawingSession, this.Size.Right, this.Size.Center);
            };
            
            //CanvasOperator
            this.CanvasOperator.Single_Start += (point) =>
            {

                //Stops
                this.Manager.Index = -1;
                for (int i = this.Manager.Count - 1; i >= 0; i--)
                {
                    float x = this.Size.OffsetToPosition(this.Manager.Stops[i].Position);
                    if (Math.Abs(x - point.X) < this.Size.Radius)
                    {
                        this.Manager.Index = i;
                        this.StopChanged?.Invoke(this.Manager.Stops[i], true);//Delegate
                        return;
                    }
                }


                //Left
                this.Manager.IsLeft = (Math.Abs(this.Size.Left - point.X) < this.Size.Radius);
                if (this.Manager.IsLeft)
                {
                    this.StopChanged?.Invoke(this.Manager.LeftStop, false);//Delegate
                    return;
                }


                //Right
                this.Manager.IsRight = (Math.Abs(this.Size.Right - point.X) < this.Size.Radius);
                if (this.Manager.IsRight)
                {
                    this.StopChanged?.Invoke(this.Manager.RightStop, false);//Delegate
                    return;
                }


                //Add
                float offset = this.Size.PositionToOffset(point.X);
                CanvasGradientStop stop = this.Manager.GetNewStop(offset);

                this.Manager.Stops.Add(stop);
                this.Manager.Index = this.Manager.Count - 1;
                this.Array = this.Manager.GetArray();

                this.StopChanged?.Invoke(stop, true);//Delegate
                return;
            };
            this.CanvasOperator.Single_Delta += (point) =>
            {
                if (this.Manager.IsLeft) return;
                if (this.Manager.IsRight) return;

                float offset = this.Size.PositionToOffset(point.X);
                this.SetOffset(offset);
                
                                this.OffsetChanged?.Invoke(offset);//Delegate

                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Single_Complete += (point) => this.CanvasControl.Invalidate();
        }


        // <summary> Set the offset of the current stop. </summary>
        public void SetOffset(float offset)
        {
            if (this.Manager.IsLeft) return;
            if (this.Manager.IsRight) return;

            int index = this.Manager.Index;
            int count = this.Manager.Count;

            if (count == 0) return;
            if (index < 0) return;
            if (index >= count) return;

            if (offset < 0) offset = 0;
            if (offset > 1) offset = 1;

            CanvasGradientStop stop = new CanvasGradientStop
            {
                Color = this.Manager.Stops[index].Color,
                Position = offset
            };
            this.Manager.Stops[index] = stop;
            this.Array[index + 1] = stop;

            this.CanvasControl.Invalidate();
            return;
        }
        // <summary> Set the color of the current stop. </summary>
        public void SetColor(Color color)
        {
            if (this.Manager.IsLeft)
            {
                CanvasGradientStop stop = new CanvasGradientStop
                {
                    Color = color,
                    Position = this.Manager.LeftStop.Position
                };
                this.Manager.LeftStop = stop;
                this.Array[0] = stop;

                this.CanvasControl.Invalidate();
                return;
            }

            if (this.Manager.IsRight)
            {
                CanvasGradientStop stop = new CanvasGradientStop
                {
                    Color = color,
                    Position = this.Manager.RightStop.Position
                };
                this.Manager.RightStop = stop;
                this.Array[this.Manager.Count + 1] = stop;

                this.CanvasControl.Invalidate();
                return;
            }

            {
            int index = this.Manager.Index;
            int count = this.Manager.Count;

            if (count == 0) return;
            if (index < 0) return;
            if (index >= count) return;

            CanvasGradientStop stop = new CanvasGradientStop
            {
                Color = color,
                Position = this.Manager.Stops[index].Position
            };
            this.Manager.Stops[index] = stop;
            this.Array[index + 1] = stop;

            this.CanvasControl.Invalidate();
                return;
            }
        }


        // <summary> Reserve all stops. </summary>
        public void Reserve()
        {
            this.Manager.Reserve();
            this.Manager.SetArray(this.Array);
            this.CanvasControl.Invalidate();
        }
        // <summary> Remove current stop. </summary>
        public void Remove()
        {
            if (this.Manager.IsLeft) return;
            if (this.Manager.IsRight) return;
            
            this.Manager.Stops.RemoveAt(this.Manager.Index);
            this.Manager.Index = -1;

            this.Array = this.Manager.GetArray();

            this.CanvasControl.Invalidate();
            return;
        }

    }
}
