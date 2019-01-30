using Retouch_Photo.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Retouch_Photo.Models
{
    public struct VectRect
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;


        public VectRect(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width < 0 ? 0 : width;
            this.Height = height < 0 ? 0 : height;
        }

        public VectRect(Vector2 start, Vector2 end)
        {
            this.X = Math.Min(start.X, end.X);
            this.Y = Math.Min(start.Y, end.Y);
            this.Width = Math.Abs(start.X - end.X);
            this.Height = Math.Abs(start.Y - end.Y);
        }

        public VectRect(Vector2 start, Vector2 end, MarqueeMode mode)
        {
            switch (mode)
            {
                case MarqueeMode.None:
                    {
                        this.X = Math.Min(start.X, end.X);
                        this.Y = Math.Min(start.Y, end.Y);
                        this.Width = Math.Abs(start.X - end.X);
                        this.Height = Math.Abs(start.Y - end.Y);
                    }
                    break;

                case MarqueeMode.Square:
                    {
                        float w = Math.Abs(start.X - end.X);
                        float h = Math.Abs(start.Y - end.Y);
                        float square = (w + h) / 2;

                        this.X = (end.X > start.X) ? start.X : start.X - square;
                        this.Y = (end.Y > start.Y) ? start.Y : start.Y - square;
                        this.Width = square;
                        this.Height = square;
                    }
                    break;

                case MarqueeMode.Center:
                    {
                        float w = Math.Abs(start.X - end.X);
                        float h = Math.Abs(start.Y - end.Y);

                        this.X = start.X - w;
                        this.Y = start.Y - h;
                        this.Width = 2 * w;
                        this.Height = 2 * h;
                    }
                    break;

                case MarqueeMode.SquareAndCenter:
                    {
                        float square3 = Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
                        float squareHalf3 = square3 / 2;

                        this.X = start.X - squareHalf3;
                        this.Y = start.Y - squareHalf3;
                        this.Width = square3;
                        this.Height = square3;
                    }
                    break;

                default:
                    {
                        this.X = 0.0f;
                        this.Y = 0.0f;
                        this.Width = 0.0f;
                        this.Height = 0.0f;
                    }
                    break;
            }
        }



        public float Left => this.X;
        public float Top => this.Y;
        public float Right => this.X + this.Width;
        public float Bottom => this.Y + this.Height;

        public Vector2 LeftTop => new Vector2(this.Left, this.Top);
        public Vector2 RightTop => new Vector2(this.Right, this.Top);
        public Vector2 RightBottom => new Vector2(this.Right, this.Bottom);
        public Vector2 LeftBottom => new Vector2(this.Left, this.Bottom);

        public Vector2 Center => new Vector2(this.X+this.Width/2, this.Y+this.Height/2);

    }
}
