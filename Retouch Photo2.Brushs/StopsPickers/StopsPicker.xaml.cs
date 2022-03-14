// Core:              ★
// Referenced:   
// Difficult:         ★★★
// Only:              ★★★
// Complete:      ★★★★
using HSVColorPickers;
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
    public partial class StopsPicker : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the stops value changed. </summary>
        public event EventHandler<CanvasGradientStop[]> StopsChanged;
        /// <summary> Occurs when the stops change starts. </summary>
        public event EventHandler<CanvasGradientStop[]> StopsChangeStarted;
        /// <summary> Occurs when stops changed. </summary>
        public event EventHandler<CanvasGradientStop[]> StopsChangeDelta;
        /// <summary> Occurs when the stops change is complete. </summary>
        public event EventHandler<CanvasGradientStop[]> StopsChangeCompleted;


        //@Content
        /// <summary> StopsPicker's ColorPicker. </summary>
        public HSVColorPickers.ColorPicker ColorPicker => this._ColorPicker;
        /// <summary> StopsPicker's ColorFlyout. </summary>
        public Flyout ColorFlyout => this._ColorFlyout;


        // Background
        CanvasRenderTarget GrayAndWhiteBackground;

        readonly StopsSize Size = new StopsSize();
        readonly StopsManager Manager = new StopsManager();


        private CanvasGradientStop[] array = GreyWhiteMeshHelpher.GetGradientStopArray();
        /// <summary>
        /// Set a brush value for the control.
        /// </summary>
        /// <param name="value"> brush </param>
        public void SetArray(CanvasGradientStop[] value)
        {
            if ((value is null) == false)
            {
                this.Manager.InitializeDate(value);
                CanvasGradientStop stop = value.First();
                this.StopChanged(stop.Color, (int)(stop.Position * 100), false);
            }

            this.array = value;
            this.CanvasControl.Invalidate(); // Invalidate
        }


        //@Construct
        /// <summary>
        /// Initializes a StopsPicker. 
        /// </summary>
        public StopsPicker()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructCanvas();
            this.ConstructCanvasOperator();

            this.ConstructStop();
            base.IsEnabledChanged += (s, e) =>
            {
                this.CanvasControl.Invalidate(); // Invalidate
            };

            // Color    
            this._ColorPicker.ColorChanged += (s, color) =>
            {
                bool isSucces = this.SetColor(color);
                if (isSucces) this.StopsChanged?.Invoke(this, this.array); // Delegate
            };

            this._ColorPicker.ColorChangedStarted += (s, color) =>
            {
                bool isSucces = this.SetColor(color);
                if (isSucces) this.StopsChangeStarted?.Invoke(this, this.array); // Delegate
            };
            this._ColorPicker.ColorChangedDelta += (s, color) =>
            {
                bool isSucces = this.SetColor(color);
                if (isSucces) this.StopsChangeDelta?.Invoke(this, this.array); // Delegate
            };
            this._ColorPicker.ColorChangedCompleted += (s, color) =>
            {
                bool isSucces = this.SetColor(color);
                if (isSucces) this.StopsChangeCompleted?.Invoke(this, this.array); // Delegate
            };

            this.ColorButton.Tapped += (s, e) =>
            {
                if (this.array is null) return;

                if (this.Manager.IsLeft || this.Manager.IsRight || this.Manager.Index >= 0)
                {
                    this._ColorPicker.Color = this.ColorEllipse.Color;
                    this._ColorFlyout.ShowAt(this.ColorEllipse); // Flyout
                }
            };


            // Offset         
            this.OffsetPicker.ValueChanged += (s, value) =>
            {
                float offset = value / 100.0f;
                bool isSucces = this.SetOffset(offset);
                if (isSucces) this.StopsChanged?.Invoke(this, this.array); // Delegate
            };
            // Alpha
            this.AlphaPicker.ValueChanged += (s, value) =>
            {
                Color color = this.ColorEllipse.Color;
                color.A = (byte)value;
                bool isSucces = this.SetColor(color);
                if (isSucces) this.StopsChanged?.Invoke(this, this.array); // Delegate
            };
        }
    }
}