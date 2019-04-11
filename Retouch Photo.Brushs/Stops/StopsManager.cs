using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;

namespace Retouch_Photo.Brushs.Stops
{
    /// <summary> Stops </summary>
    public class StopsManager
    {
        public bool IsLeft;
        public Color LeftColor = Colors.White;

        public int Index;
        public int Count => this.Stops.Count;
        public List<CanvasGradientStop> Stops = new List<CanvasGradientStop>();

        public bool IsRight;
        public Color RightColor = Colors.Gray;

        /// <summary> Initialize </summary>
        public void Initialize(CanvasGradientStop[] array)
        {
            this.Index = -1;
            this.IsLeft = true;
            this.IsRight = false;

            int count = array.Count();
            this.LeftColor = array[0].Color;
            this.RightColor = array[count - 1].Color;

            this.Stops.Clear();
            for (int i = 1; i < count - 1; i++)
            {
                this.Stops.Add(array[i]);
            }
        }


        /// <summary> Left + Stops + Right </summary>
        public CanvasGradientStop[] GetArray()
        {
            CanvasGradientStop[] array = new CanvasGradientStop[this.Count + 2];
            this.SetArray(array);
            return array;
        }

        /// <summary> Left + Stops + Right </summary>
        public void SetArray(CanvasGradientStop[] array)
        {
            if (this.Count + 2 != array.Count()) return;
            array[0] = new CanvasGradientStop
            {
                Color = this.LeftColor,
                Position = 0.0f
            };

            array[this.Count + 1] = new CanvasGradientStop
            {
                Color = this.RightColor,
                Position = 1.0f
            };

            if (this.Count == 0) return;
            for (int i = 0; i < this.Count; i++)
            {
                array[i + 1] = this.Stops[i];
            }
        }

        /// <summary> Get new stop. </summary>
        public CanvasGradientStop GetNewStop(float offset)
        {
            Color left = this.LeftColor;
            float leftDistance = 1.0f;

            Color right = this.RightColor;
            float rightDistance = 1.0f;

            foreach (CanvasGradientStop stop in this.Stops)
            {
                float distance = offset - stop.Position;
                if (distance > 0.0f)
                {
                    if (distance < leftDistance)
                    {
                        leftDistance = distance;
                        left = stop.Color;
                    }
                }
                else// if (distance < 0.0f)
                {
                    if (distance < rightDistance)
                    {
                        rightDistance = distance;
                        right = stop.Color;
                    }
                }
            }

            int r = (left.R + right.R) / 2;
            int g = (left.G + right.G) / 2;
            int b = (left.B + right.B) / 2;
            return new CanvasGradientStop
            {
                Color = Color.FromArgb(255, (byte)r, (byte)g, (byte)b),
                Position = offset
            };
        }

        /// <summary> Reserve all stops. </summary>
        public void Reserve()
        {
            //Index
            this.Index = this.Count - this.Index - 1;

            //Reserve
            Color leftStopColor = this.LeftColor;
            Color RightColorColor = this.RightColor;
            this.LeftColor = RightColorColor;
            this.RightColor = leftStopColor;

            for (int i = 0; i < this.Stops.Count; i++)
            {
                CanvasGradientStop stop = this.Stops[i];
                this.Stops[i] = new CanvasGradientStop
                {
                    Position = 1.0f - stop.Position,
                    Color = stop.Color
                };
            }
        }


        //Node
        public void DrawLeftNode(CanvasDrawingSession ds, float x, float y) => this.DrawNode(ds, x, y, this.LeftColor, this.IsLeft);
        public void DrawRightNode(CanvasDrawingSession ds, float x, float y) => this.DrawNode(ds, x, y, this.RightColor, this.IsRight);
        public void DrawNode(CanvasDrawingSession ds, float x, float y, Color color, bool isCurrent)
        {
            ds.FillCircle(x, y, 10, isCurrent ? Colors.Black : Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(x, y, 8, Colors.White);
            ds.FillCircle(x, y, 6, color);
        }
    }
}
