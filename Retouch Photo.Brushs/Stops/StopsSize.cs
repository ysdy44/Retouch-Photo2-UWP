using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo.Brushs.Stops
{
    /// <summary> Size </summary>
    public class StopsSize
    {
        public readonly float Radius = 12.0f;
        public float InnerWidth, OuterWidth, OuterHeight;
        public float Center, Left, Right;

        public float OffsetToPosition(float offset) => this.Radius + this.InnerWidth * offset;
        public float PositionToOffset(float position) => (position - this.Radius) / this.InnerWidth;

        public void SIzeChange(float width, float height)
        {
            this.OuterWidth = width;
            this.OuterHeight = height;

            this.InnerWidth = this.OuterWidth - this.Radius - this.Radius;

            this.Center = this.OuterHeight / 2;
            this.Left = this.Radius;
            this.Right = this.OuterWidth - this.Radius;
        }

        public void DrawBackground(CanvasDrawingSession ds, ICanvasResourceCreator creator, CanvasGradientStop[] array)
        {
            //Rect
            ds.FillRoundedRectangle(0, 0, this.OuterWidth, this.OuterHeight, 6, 6, new CanvasLinearGradientBrush(creator, array)
            {
                StartPoint = new Vector2(this.Left, this.Center),
                EndPoint = new Vector2(this.Right, this.Center),
            });
            ds.DrawRoundedRectangle(0, 0, this.OuterWidth, this.OuterHeight, 6, 6, Colors.Gray);


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
    }
}
