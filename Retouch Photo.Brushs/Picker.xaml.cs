using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo.Brushs.Stops;
using System;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Brushs
{
    public sealed partial class Picker : UserControl
    {
        //Delegate
        public delegate void StopsChangeHandler();
        public event StopsChangeHandler StopsChange;

        bool IsPressed;
        Vector2 Vector;

        StopsSize Size = new StopsSize();
        StopsManager Manager = new StopsManager();

        private Brush brush;
        public Brush Brush
        {
            get => this.brush;
            set
            {
                if (value != null)
                {
                    this.Manager.Initialize(value.Array);
                    CanvasGradientStop stop = value.Array.First();
                    this.StopChanged(stop.Color, (int)(stop.Position * 100), false);
                }

                this.brush = value;
            }
        }


        public Picker()
        {
            this.InitializeComponent();
            this.ReserveButton.Tapped += (s, e) => this.Reserve();
            this.RemoveButton.Tapped += (s, e) =>
            {
                this.StopChanged(Colors.Transparent, 0, false);
                this.Remove();
            };

            //CanvasControl
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                float width = (float)e.NewSize.Width;
                float height = (float)e.NewSize.Height;
                this.Size.SIzeChange(width, height);
            };
            this.CanvasControl.CreateResources += (sender, args) => { };
            this.CanvasControl.Draw += (sender, args) =>
            {
                if (this.Brush == null) return;

                //Background
                this.Size.DrawBackground(args.DrawingSession, this.CanvasControl, this.Brush.Array);

                //Stops
                for (int i = 0; i < this.Manager.Count; i++)
                {
                    CanvasGradientStop stop = this.Manager.Stops[i];
                    this.Manager.DrawNode(args.DrawingSession, this.Size.OffsetToPosition(stop.Position), this.Size.Center, stop.Color, (i == this.Manager.Index));
                }
                this.Manager.DrawLeftNode(args.DrawingSession, this.Size.Left, this.Size.Center);
                this.Manager.DrawRightNode(args.DrawingSession, this.Size.Right, this.Size.Center);
            };

            //Manipulation
            this.CanvasControl.PointerPressed += (s, e) =>
            {
                this.IsPressed = true;
                this.Vector = e.GetCurrentPoint(this.CanvasControl).Position.ToVector2();

                if (this.Brush == null) return;

                this.Manager.Index = -1;
                this.Manager.IsLeft = false;
                this.Manager.IsRight = false;

                //Stops
                for (int i = this.Manager.Count - 1; i >= 0; i--)
                {
                    float x = this.Size.OffsetToPosition(this.Manager.Stops[i].Position);
                    if (Math.Abs(x - this.Vector.X) < this.Size.Radius)
                    {
                        this.Manager.Index = i;
                        CanvasGradientStop stop = this.Manager.Stops[i];
                        this.StopChanged(stop.Color, (int)(stop.Position * 100), true);//Delegate
                        return;
                    }
                }

                //Left
                bool isLeft = (Math.Abs(this.Size.Left - this.Vector.X) < this.Size.Radius);
                if (isLeft)
                {
                    this.Manager.IsLeft = true;
                    this.StopChanged(this.Manager.LeftColor, 0, false);//Delegate
                    return;
                }

                //Right
                bool isRight = (Math.Abs(this.Size.Right - this.Vector.X) < this.Size.Radius);
                if (isRight)
                {
                    this.Manager.IsRight = true;
                    this.StopChanged(this.Manager.RightColor, 100, false);//Delegate
                    return;
                }


                //Add
                float offset = this.Size.PositionToOffset(this.Vector.X);
                CanvasGradientStop addStop = this.Manager.GetNewStop(offset);

                this.Manager.Stops.Add(addStop);
                this.Manager.Index = this.Manager.Count - 1;
                this.Brush.Array = this.Manager.GetArray();

                this.StopChanged(addStop.Color, (int)(addStop.Position * 100), true);//Delegate
                return;
            };
            this.CanvasControl.PointerMoved += (s, e) =>
            {
                this.Vector = e.GetCurrentPoint(this.CanvasControl).Position.ToVector2();

                if (this.IsPressed == false) return;
                if (this.Brush == null) return;

                if (this.Manager.IsLeft) return;
                if (this.Manager.IsRight) return;

                float offset = this.Size.PositionToOffset(this.Vector.X);
                this.SetOffset(offset);

                this.OffsetChanged(offset);

                this.CanvasControl.Invalidate();
                this.StopsChange?.Invoke();//Delegate
            };
            this.CanvasControl.PointerReleased += (s, e) =>
            {
                this.IsPressed = false;
                if (this.Brush == null) return;

                this.CanvasControl.Invalidate();
                this.StopsChange?.Invoke();//Delegate
            };

            //Color            
            this.ColorPicker.ColorChange += (s, color) => this.SetColor(color);
            this.StrawPicker.ColorChange += (s, color) => this.SetColor(color);
            this.ColorButton.Tapped += (s, e) =>
            {
                if (this.Brush == null) return;

                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = this.SolidColorBrush.Color;
            };

            //Offset            
            this.NumberControl.ValueChange += (s, value) => this.SetOffset((float)value / 100.0f);
        }


        private void OffsetChanged(float offset) => this.NumberControl.Value = (int)(offset * 100);
        private void StopChanged(Color color, int offset, bool isEnabled)
        {
            this.SolidColorBrush.Color = color;
            this.NumberControl.Value = offset;
            this.RemoveButton.IsEnabled = this.NumberControl.IsEnabled = isEnabled;
        }


        // <summary> Set the offset of the current stop. </summary>
        public void SetOffset(float offset)
        {
            if (this.Brush == null) return;

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
            this.Brush.Array[index + 1] = stop;

            this.CanvasControl.Invalidate();
            return;
        }
        // <summary> Set the color of the current stop. </summary>
        public void SetColor(Color color)
        {
            this.SolidColorBrush.Color = color;
            if (this.Brush == null) return;

            if (this.Manager.IsLeft)
            {
                this.Manager.LeftColor = color;
                this.Brush.Array[0] = new CanvasGradientStop
                {
                    Color = color,
                    Position = 0
                };

                this.CanvasControl.Invalidate();
                this.StopsChange?.Invoke();//Delegate
                return;
            }

            if (this.Manager.IsRight)
            {
                this.Manager.RightColor = color;
                this.Brush.Array[this.Manager.Count + 1] = new CanvasGradientStop
                {
                    Color = color,
                    Position = 1
                };

                this.CanvasControl.Invalidate();
                this.StopsChange?.Invoke();//Delegate
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
                this.Brush.Array[index + 1] = stop;

                this.CanvasControl.Invalidate();
                this.StopsChange?.Invoke();//Delegate
                return;
            }
        }


        // <summary> Reserve all stops. </summary>
        public void Reserve()
        {
            if (this.Brush == null) return;

            this.Manager.Reserve();
            this.Manager.SetArray(this.Brush.Array);
            this.CanvasControl.Invalidate();
        }
        // <summary> Remove current stop. </summary>
        public void Remove()
        {
            if (this.Brush == null) return;

            if (this.Manager.IsLeft) return;
            if (this.Manager.IsRight) return;

            this.Manager.Stops.RemoveAt(this.Manager.Index);
            this.Manager.Index = -1;

            this.Brush.Array = this.Manager.GetArray();

            this.CanvasControl.Invalidate();
            return;
        }
    }
}