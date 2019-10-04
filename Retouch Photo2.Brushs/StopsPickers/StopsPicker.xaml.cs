using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Stops picker.
    /// </summary>
    public sealed partial class StopsPicker : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the stops changes. </summary>
        public event EventHandler<CanvasGradientStop[]> StopsChanged;

        //@Content        
        public ComboBox ComboBox => this._ComboBox;
        /// <summary> ComboBox's item. </summary>
        public ComboBoxItem LinearGradientItem => this._LinearGradientItem;
        /// <summary> ComboBox's item. </summary>
        public ComboBoxItem RadialGradientItem => this._RadialGradientItem;
        /// <summary> ComboBox's item. </summary>
        public ComboBoxItem EllipticalGradientItem => this._EllipticalGradientItem;

        //Background
        CanvasBitmap GrayAndWhiteBackground;

        StopsSize Size = new StopsSize();
        StopsManager Manager = new StopsManager();


        private CanvasGradientStop[] array;
        /// <summary>
        /// Set a brush value for the control.
        /// </summary>
        /// <param name="value"> brush </param>
        public void SetArray(CanvasGradientStop[] value)
        {
            if (value != null)
            {
                this.Manager.InitializeDate(value);
                CanvasGradientStop stop = value.First();
                this.StopChanged(stop.Color, (int)(stop.Position * 100), false);
            }

            this.array = value;
        }


        //@Construct
        public StopsPicker()
        {
            this.InitializeComponent();

            //Canvas
            {
                this.CanvasControl.SizeChanged += (s, e) =>
                {
                    if (e.NewSize == e.PreviousSize) return;
                    this.Size.SIzeChange((float)e.NewSize.Width, (float)e.NewSize.Height);
                };
                this.CanvasControl.CreateResources += (sender, args) => this.GrayAndWhiteBackground = Brush.CreateGrayAndWhiteBackground(sender, (float)sender.ActualWidth, (float)sender.ActualHeight, 6);
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

                //CanvasOperator
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

                        CanvasGradientStop[] array = this.Manager.GenerateArrayFromDate();
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


                //Color      
                {
                    this.ColorPicker.ColorChange += (s, color) => this.SetColor(color);
                    this.StrawPicker.ColorChange += (s, color) => this.SetColor(color);
                    this.ColorButton.Tapped += (s, e) =>
                    {
                        if (this.array == null) return;

                        if (this.Manager.IsLeft || this.Manager.IsRight || this.Manager.Index >= 0)
                        {
                            this.ColorPicker.Color = this.SolidColorBrush.Color;
                            this.ColorFlyout.ShowAt(this.ColorButton);//Flyout
                        }
                    };
                }


                //SliderControl     
                {
                    //Alpha
                    this.SliderControl.APicker.ValueChange += (s, value) =>
                    {
                        this.SliderControl.ASlider.Value = value;
                        this.SetColor(Color.FromArgb((byte)value, this.SolidColorBrush.Color.R, this.SolidColorBrush.Color.G, this.SolidColorBrush.Color.B));
                    };
                    this.SliderControl.ASlider.ValueChangeDelta += (s, value) =>
                    {
                        this.SliderControl.APicker.Value = (int)value;
                        this.SetColor(Color.FromArgb((byte)value, this.SolidColorBrush.Color.R, this.SolidColorBrush.Color.G, this.SolidColorBrush.Color.B));
                    };

                    //Offset         
                    this.SliderControl.OPicker.ValueChange += (s, value) =>
                    {
                        this.SetOffset(value / 100.0f);
                    };
                }


                //Button
                {
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

                        this.array = this.Manager.GenerateArrayFromDate();

                        this.CanvasControl.Invalidate();
                        this.StopsChanged?.Invoke(this, this.array);//Delegate
                    };
                }

            }
        }
}