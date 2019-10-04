using Microsoft.Graphics.Canvas.Brushes;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Manager of CanvasGradientStop class array.
    /// </summary>
    public class StopsManager
    {

        /// <summary> Is it left? </summary>
        public bool IsLeft;
        /// <summary> Left stop color. </summary>
        public Color LeftColor = Colors.White;

        /// <summary> Is it right? </summary>
        public bool IsRight;
        /// <summary> Right stop color. </summary>
        public Color RightColor = Colors.Gray;

        /// <summary> Index of stops. </summary>
        public int Index;
        /// <summary> Count of stops. </summary>
        public int Count => this.Stops.Count;
        /// <summary> Stops. </summary>
        public List<CanvasGradientStop> Stops = new List<CanvasGradientStop>();


        /// <summary>
        /// Initialize this manager's data
        /// </summary>
        /// <param name="array"> array </param>
        public void InitializeDate(CanvasGradientStop[] array)
        {
            int count = array.Count();

            if (count <= 2)
            {
                this.Index = -1;
                this.IsLeft = true;
                this.IsRight = false;
            }
            else if (count - 2 <= this.Index)
            {
                this.Index = -1;
                this.IsLeft = false;
                this.IsRight = true;
            }

            //Left right
            this.LeftColor = array.First().Color;
            this.RightColor = array.Last().Color;

            //Stops
            this.Stops.Clear();
            for (int i = 1; i < count - 1; i++)
            {
                this.Stops.Add(array[i]);
            }
        }



        /// <summary>
        /// Generate a new CanvasGradientStop array from the manager's data.
        /// </summary>
        /// <returns> stops </returns>
        public CanvasGradientStop[] GenerateArrayFromDate()
        {
            CanvasGradientStop[] array = new CanvasGradientStop[this.Count + 2];
            this.Copy(array);
            return array;
        }

        /// <summary>
        /// Assigning a value to an array by the manager's data.
        /// </summary>
        /// <param name="array"> The destination array. </param>
        public void Copy(CanvasGradientStop[] array)
        {
            if (this.Count + 2 != array.Count()) return;

            //left
            array[0] = new CanvasGradientStop
            {
                Color = this.LeftColor,
                Position = 0.0f
            };

            //Right
            array[this.Count + 1] = new CanvasGradientStop
            {
                Color = this.RightColor,
                Position = 1.0f
            };

            //Stops
            if (this.Count == 0) return;
            for (int i = 0; i < this.Count; i++)
            {
                array[i + 1] = this.Stops[i];
            }
        }

        /// <summary>
        /// Insert a new step by offset.
        /// </summary>
        /// <param name="offset"> The source offset. </param>
        /// <returns> stop </returns>
        public CanvasGradientStop InsertNewStepByOffset(float offset)
        {
            //Left
            Color left = this.LeftColor;
            float leftDistance = 1.0f;

            //Right
            Color right = this.RightColor;
            float rightDistance = 1.0f;

            //Stops
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

        /// <summary>
        /// Reverse all manager's data.
        /// </summary>
        public void Reserve()
        {
            //Index
            this.Index = this.Count - this.Index - 1;

            //Reserve
            Color leftStopColor = this.LeftColor;
            Color RightColorColor = this.RightColor;
            this.LeftColor = RightColorColor;
            this.RightColor = leftStopColor;

            //Stops
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
        
    }
}