using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs.Controls;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs.Stops
{
    /// <summary>
    /// Size of <see cref="StopsPicker">.
    /// </summary>
    public class StopsSize
    {
        /// <summary> Node width. </summary>
        public const float Radius = 12.0f;
        /// <summary> Size property. </summary>
        public float InnerWidth, OuterWidth, OuterHeight;
        /// <summary> Center and Border. </summary>
        public float Center, Left, Right;


        //@Converter
        public float OffsetToPosition(float offset) => StopsSize.Radius + this.InnerWidth * offset;
        public float PositionToOffset(float position) => (position - StopsSize.Radius) / this.InnerWidth;

        /// <summary>
        /// Occurs when canvas size changed.
        /// </summary>
        /// <param name="width"> Canvas width. </param>
        /// <param name="height"> Canvas height. </param>
        public void SIzeChange(float width, float height)
        {
            this.OuterWidth = width;
            this.OuterHeight = height;

            this.InnerWidth = this.OuterWidth - StopsSize.Radius - StopsSize.Radius;

            this.Center = this.OuterHeight / 2;
            this.Left = StopsSize.Radius;
            this.Right = this.OuterWidth - StopsSize.Radius;
        }


        /// <summary>
        /// Draw linear-gradient.
        /// </summary>
        /// <param name="ds"> CanvasDrawingSession </param>
        /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="array"> array </param>
        public void DrawLinearGradient(CanvasDrawingSession ds, ICanvasResourceCreator resourceCreator, CanvasGradientStop[] array)
        {
            //LinearGradient
            ds.FillRectangle(0, 0, this.OuterWidth, this.OuterHeight, new CanvasLinearGradientBrush(resourceCreator, array)
            {
                StartPoint = new Vector2(this.Left, this.Center),
                EndPoint = new Vector2(this.Right, this.Center),
            });
        }

        /// <summary>
        /// Draw lines.
        /// </summary>
        /// <param name="ds"> CanvasDrawingSession </param>
        public void DrawLines(CanvasDrawingSession ds)
        { 
            //Left
            ds.DrawLine(this.Left, 0, this.Left, this.OuterHeight, Color.FromArgb(70, 127, 127, 127), 3);
            ds.DrawLine(this.Left, 0, this.Left, this.OuterHeight, Colors.White);
            //Right
            ds.DrawLine(this.Right, 0, this.Right, this.OuterHeight, Color.FromArgb(70, 127, 127, 127), 3);
            ds.DrawLine(this.Right, 0, this.Right, this.OuterHeight, Colors.White);
            //Center
            ds.DrawLine(this.Left, this.Center, this.Right, this.Center, Color.FromArgb(70, 127, 127, 127), 3);
            ds.DrawLine(this.Left, this.Center, this.Right, this.Center, Colors.White);
        }

        /// <summary>
        /// Draw nodes.
        /// </summary>
        /// <param name="ds"> CanvasDrawingSession </param>
        /// <param name="manager"> StopsManager </param>
        public void DrawNodes(CanvasDrawingSession ds, StopsManager manager)
        {
            //Stops
            for (int i = 0; i < manager.Count; i++)
            {
                CanvasGradientStop stop = manager.Stops[i];
                Vector2 position = new Vector2(this.OffsetToPosition(stop.Position), this.Center);
                if (manager.Index == i) ds.FillCircle(position, 10, Colors.Black);
                ds.DrawNode2(position, stop.Color);
            }

            //Left
            Vector2 left = new Vector2(this.Left, this.Center);
            if (manager.IsLeft) ds.FillCircle(left, 10, Colors.Black);
            ds.DrawNode2(left, manager.LeftColor);

            //Right
            Vector2 right = new Vector2(this.Right, this.Center);
            if (manager.IsLeft) ds.FillCircle(right, 10, Colors.Black);
            ds.DrawNode2(right, manager.LeftColor);
        }
    }
}
