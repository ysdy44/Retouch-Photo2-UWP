using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;

namespace Retouch_Photo.Controls.BrushControls
{
    /// <summary> Stops </summary>
    public class StopsManager
    {
        public bool IsLeft;
        public CanvasGradientStop LeftStop = new CanvasGradientStop { Color = Colors.White, Position = 0.0f };

        public int Index;
        public int Count => this.Stops.Count;
        public List<CanvasGradientStop> Stops = new List<CanvasGradientStop>();

        public bool IsRight;
        public CanvasGradientStop RightStop = new CanvasGradientStop { Color = Colors.Gray, Position = 1.0f };


        /// <summary> Left + Stops + Right </summary>
        public CanvasGradientStop[] GetArray()
        {
            CanvasGradientStop[] array = new CanvasGradientStop[this.Count + 2];
            array[0] = this.LeftStop;
            array[this.Count + 1] = this.RightStop;
            for (int i = 0; i < this.Count; i++)
            {
                array[i + 1] = this.Stops[i];
            }
            return array;
        }
     
        /// <summary> Left + Stops + Right </summary>
        public void SetArray(CanvasGradientStop[] array)
        {
            if (this.Count + 2 != array.Count()) return;
            array[0] = this.LeftStop;
            array[this.Count + 1] = this.RightStop;

            if (this.Count == 0) return;
            for (int i = 0; i < this.Count; i++)
            {
                array[i + 1] = this.Stops[i];
            }
        }

        /// <summary> Get new stop. </summary>
        public CanvasGradientStop GetNewStop(float offset)
        {
            Color left = this.LeftStop.Color;
            float leftDistance = 1.0f;

            Color right = this.RightStop.Color;
            float rightDistance = 1.0f;

            foreach (CanvasGradientStop stop in this.Stops)
            {
                float distance = offset - stop.Position;
                if (distance > 0.0f )
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
                Color = Color.FromArgb(255, (byte)r, (byte)g, (byte)b ),
                Position = offset
            };
        }
    
        /// <summary> Reserve all stops. </summary>
        public void Reserve()
        {
            //Reserve
            Color leftStopColor = this.LeftStop.Color;
            Color rightStopColor = this.RightStop.Color;
            this.LeftStop = new CanvasGradientStop { Position = 0.0f, Color = rightStopColor };
            this.RightStop = new CanvasGradientStop { Position = 1.0f, Color = leftStopColor };

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
        public void DrawLeftNode(CanvasDrawingSession ds, float x, float y) => this.DrawNode(ds, x, y, this.LeftStop.Color, this.IsLeft);
        public void DrawRightNode(CanvasDrawingSession ds, float x, float y) => this.DrawNode(ds, x, y, this.RightStop.Color, this.IsRight);
        public void DrawNode(CanvasDrawingSession ds, float x, float y, Color color, bool isCurrent)
        {
            ds.FillCircle(x, y, 10, isCurrent ? Colors.Black : Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(x, y, 8, Colors.White);
            ds.FillCircle(x, y, 6, color);
        }
    }
}