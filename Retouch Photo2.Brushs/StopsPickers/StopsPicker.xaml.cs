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
    public sealed partial class StopsPicker : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the stops changes. </summary>
        public event EventHandler<CanvasGradientStop[]> StopsChanged;

        //Background
        CanvasRenderTarget GrayAndWhiteBackground;

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
            this.ConstructStrings();
            this.ConstructCanvas();
            this.ConstructCanvasOperator();

            this.ConstructStop();


            //Color      
            this.ColorPicker.ColorChange += (s, color) => this.SetColor(color);
            this.ColorButton.Tapped += (s, e) =>
            {
                if (this.array == null) return;

                if (this.Manager.IsLeft || this.Manager.IsRight || this.Manager.Index >= 0)
                {
                    this.ColorPicker.Color = this.SolidColorBrush.Color;
                    this.ColorFlyout.ShowAt(this.ColorButton);//Flyout
                }
            };


            //Offset         
            this.OffsetPicker.ValueChange += (s, value) =>
            {
                float offset = value / 100.0f;
                this.SetOffset(offset);
            };
            //Alpha
            this.AlphaPicker.ValueChange += (s, value) =>
            {
                Color color = this.SolidColorBrush.Color;
                color.A = (byte)value;
                this.SetColor(color);
            };
        }
    }
}