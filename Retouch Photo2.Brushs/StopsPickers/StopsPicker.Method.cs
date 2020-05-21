using Microsoft.Graphics.Canvas.Brushes;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Stops picker.
    /// </summary>
    public sealed partial class StopsPicker : UserControl
    {
        
        /// <summary>
        ///  Occur when the offset changed.
        /// </summary>
        /// <param name="offset"> offset </param>
        private void OffsetChanged(float offset)
        {
            int o = (int)(offset * 100);
        }
        /// <summary>
        ///  Occur when the stop changed.
        /// </summary>
        /// <param name="color"> The source stop color. </param>
        /// <param name="offset"> The source stop offset. </param>
        /// <param name="isEnabled"> Control enabled. </param>
        private void StopChanged(Color color, int offset, bool isEnabled)
        {
            this.ColorEllipse.Color = color;

            this.AlphaPicker.Value = color.A;

            this.OffsetPicker.Value = offset;

            //IsEnabled
            this.RemoveButton.IsEnabled = isEnabled;
            this.OffsetPicker.IsEnabled = isEnabled;
        }


        /// <summary>
        /// Set the offset of the current stop.
        /// </summary>
        /// <param name="offset"> The source stop offset. </param>
        public bool SetOffset(float offset)
        {
            if (this.array == null) return false;

            if (this.Manager.IsLeft) return false;
            if (this.Manager.IsRight) return false;

            int index = this.Manager.Index;
            int count = this.Manager.Count;

            if (count == 0) return false;
            if (index < 0) return false;
            if (index >= count) return false;

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
            return true;
        }
        /// <summary>
        /// Set the color of the current stop.
        /// </summary>
        /// <param name="color"> The source stop color. </param>
        public bool SetColor(Color color)
        {
            this.ColorEllipse.Color = color;

            if (this.array == null) return false;

            if (this.Manager.IsLeft)
            {
                this.Manager.LeftColor = color;
                this.array[0] = new CanvasGradientStop
                {
                    Color = color,
                    Position = 0
                };

                this.CanvasControl.Invalidate();
                return true;
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
                return true;
            }

            {
                int index = this.Manager.Index;
                int count = this.Manager.Count;

                if (count == 0) return false;
                if (index < 0) return false;
                if (index >= count) return false;

                CanvasGradientStop stop = new CanvasGradientStop
                {
                    Color = color,
                    Position = this.Manager.Stops[index].Position
                };
                this.Manager.Stops[index] = stop;
                this.array[index + 1] = stop;

                this.CanvasControl.Invalidate();
                return true;
            }

            return false;
        }

    }
}