using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Brushs.Stops;
using System;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs.Controls
{
    //@Delegate
    public delegate void StopsChangedHandler(CanvasGradientStop[] array);

    /// <summary>
    /// Stops picker.
    /// </summary>
    public sealed partial class StopsPicker : UserControl
    {       
        //@Delegate
        public event StopsChangedHandler StopsChanged;


        StopsSize Size = new StopsSize();
        StopsManager Manager = new StopsManager();

        float SizeWidth;
        float SizeHeight;
        CanvasBitmap Bitmap; 

        private CanvasGradientStop[] array;
        public void SetArray(CanvasGradientStop[] value)
        {
            if (value != null)
            {
                this.Manager.Initialize(value);
                CanvasGradientStop stop = value.First();
                this.StopChanged(stop.Color, (int)(stop.Position * 100), false);
            }

            this.array = value;
        }


        //@Construct
        public StopsPicker()
        {
            this.InitializeComponent();


            // Reserve all stops. 
            this.ReserveButton.Tapped += (s, e) =>
            {
                if (this.array == null) return;

                this.Manager.Reserve();
                this.Manager.SetArray(this.array);

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this.array);//Delegate
            };
            // Remove current stop
            this.RemoveButton.Tapped += (s, e) =>
            {

                if (this.array == null) return;

                if (this.Manager.IsLeft) return;
                if (this.Manager.IsRight) return;

                this.Manager.Stops.RemoveAt(this.Manager.Index);

                if (this.Manager.Stops.Count == 0)
                {
                    this.Manager.IsLeft = true;
                    this.Manager.IsRight = false;
                    this.Manager.Index = -1;

                    this.StopChanged(this.Manager.LeftColor, 0, false);
                }
                else
                {
                    this.Manager.Index--;

                    if (this.Manager.Index > this.Manager.Stops.Count - 1)
                        this.Manager.Index = this.Manager.Stops.Count - 1;
                    else if (this.Manager.Index < 0)
                        this.Manager.Index = 0;

                    CanvasGradientStop stop = this.Manager.Stops[this.Manager.Index];
                    this.StopChanged(stop.Color, 0, false);
                }

                this.array = this.Manager.GetArray();

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this.array);//Delegate
            };


            //Canvas
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.SizeWidth = (float)e.NewSize.Width;
                this.SizeHeight = (float)e.NewSize.Height;
                this.Size.SIzeChange(this.SizeWidth, this.SizeHeight);
            };
            this.CanvasControl.CreateResources += (sender, args) =>
            {
                Color[] colors = new Color[]
                {
                     Windows.UI.Colors.LightGray, Windows.UI.Colors.White,
                     Windows.UI.Colors.White, Windows.UI.Colors.LightGray
                };
                this.Bitmap = CanvasBitmap.CreateFromColors(sender, colors, 2, 2);
            };
            this.CanvasControl.Draw += (sender, args) =>
            {
                if (this.array == null) return;

                args.DrawingSession.DrawImage(new DpiCompensationEffect
                {
                    Source = new ScaleEffect
                    {
                        Scale = new Vector2(this.SizeHeight / 5),
                        InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                        Source = new BorderEffect
                        {
                            ExtendX = CanvasEdgeBehavior.Wrap,
                            ExtendY = CanvasEdgeBehavior.Wrap,
                            Source = this.Bitmap
                        }
                    }
                });

                //Background
                this.Size.DrawBackground(args.DrawingSession, this.CanvasControl, this.array);

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
                if (this.array == null) return;

                this.Manager.Index = -1;
                this.Manager.IsLeft = false;
                this.Manager.IsRight = false;

                //Stops
                for (int i = this.Manager.Count - 1; i >= 0; i--)
                {
                    float x = this.Size.OffsetToPosition(this.Manager.Stops[i].Position);
                    if (Math.Abs(x - point.X) < this.Size.Radius)
                    {
                        this.Manager.Index = i;
                        CanvasGradientStop stop = this.Manager.Stops[i];
                        this.StopChanged(stop.Color, (int)(stop.Position * 100), true);//Delegate
                        return;
                    }
                }

                //Left
                bool isLeft = (Math.Abs(this.Size.Left - point.X) < this.Size.Radius);
                if (isLeft)
                {
                    this.Manager.IsLeft = true;
                    this.StopChanged(this.Manager.LeftColor, 0, false);//Delegate
                    return;
                }

                //Right
                bool isRight = (Math.Abs(this.Size.Right - point.X) < this.Size.Radius);
                if (isRight)
                {
                    this.Manager.IsRight = true;
                    this.StopChanged(this.Manager.RightColor, 100, false);//Delegate
                    return;
                }


                //Add
                float offset = this.Size.PositionToOffset(point.X);
                CanvasGradientStop addStop = this.Manager.GetNewStop(offset);

                this.Manager.Stops.Add(addStop);
                this.Manager.Index = this.Manager.Count - 1;

                CanvasGradientStop[] array = this.Manager.GetArray();
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
                this.StopsChanged?.Invoke(this.array);//Delegate
            };
            this.CanvasControl.PointerReleased += (s, e) =>
            {
                if (this.array == null) return;

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this.array);//Delegate
            };


            //Color            
            this.ColorPicker.ColorChange += (s, color) =>
            {
                this.SetColor(color);
            };
            this.StrawPicker.ColorChange += (s, color) =>
            {
                this.SetColor(color);
            };
            this.ColorButton.Tapped += (s, e) =>
            {
                if (this.array == null) return;

                if (this.Manager.IsLeft || this.Manager.IsRight || this.Manager.Index >= 0)
                {
                    this.ColorFlyout.ShowAt(this.ColorButton);
                    this.ColorPicker.Color = this.SolidColorBrush.Color;
                }
            };


            //Opacity         
            this.OpacityPicker.Minimum = 0;
            this.OpacityPicker.Maximum = 255;
            this.OpacityPicker.Unit = "º";
            this.OpacityPicker.ValueChange += (s, value) => this.SetColor(Color.FromArgb((byte)value, this.SolidColorBrush.Color.R, this.SolidColorBrush.Color.G, this.SolidColorBrush.Color.B));


            //Offset         
            this.OffsetPicker.Minimum = 0;
            this.OffsetPicker.Maximum = 100;
            this.OffsetPicker.Unit = "%";
            this.OffsetPicker.ValueChange += (s, value) => this.SetOffset((float)value / 100.0f);

        }


        private void OffsetChanged(float offset) => this.OffsetPicker.Value = (int)(offset * 100);
        private void StopChanged(Color color, int offset, bool isEnabled)
        {
            this.SolidColorBrush.Color = color;

            this.OpacityPicker.Value = color.A;
            this.OffsetPicker.Value = offset;

            this.RemoveButton.IsEnabled = this.OffsetPicker.IsEnabled = isEnabled;
        }


        // <summary> Set the offset of the current stop. </summary>
        public void SetOffset(float offset)
        {
            if (this.array == null) return;

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
            this.array[index + 1] = stop;

            this.CanvasControl.Invalidate();
            return;
        }
        // <summary> Set the color of the current stop. </summary>
        public void SetColor(Color color)
        {
            this.SolidColorBrush.Color = color;

            this.OpacityPicker.Value = color.A;

            if (this.array == null) return;

            if (this.Manager.IsLeft)
            {
                this.Manager.LeftColor = color;
                this.array[0] = new CanvasGradientStop
                {
                    Color = color,
                    Position = 0
                };

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this.array);//Delegate
                return;
            }

            if (this.Manager.IsRight)
            {
                this.Manager.RightColor = color;
                this.array[this.Manager.Count + 1] = new CanvasGradientStop
                {
                    Color = color,
                    Position = 1
                };

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this.array);//Delegate
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
                this.array[index + 1] = stop;

                this.CanvasControl.Invalidate();
                this.StopsChanged?.Invoke(this.array);//Delegate
                return;
            }
        }
                          
    }
}