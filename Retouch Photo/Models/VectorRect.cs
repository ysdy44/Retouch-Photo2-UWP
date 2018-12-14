using Microsoft.Graphics.Canvas;
using Retouch_Photo.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.Models
{
    public struct VectorRect
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public VectorRect(float x,float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width < 0 ? 0 : width;
            this.Height = height < 0 ? 0 : height;
        }

        public VectorRect(Vector2 start, Vector2 end)
        {
            this.X = Math.Min(start.X, end.X);
            this.Y = Math.Min(start.Y, end.Y);
            this.Width = Math.Abs(start.X - end.X);
            this.Height = Math.Abs(start.Y - end.Y);
        }

        /// <summary>Postion = ( X, Y ) </summary>
        public Vector2 Postion
        {
            get => new Vector2(this.X, this.Y);
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }
        /// <summary>Size = ( Width, Height ) </summary>
        public Vector2 Size
        {
            get => new Vector2(this.Width, this.Height);
            set
            {
                this.Width = value.X;
                this.Height = value.Y;
            }
        }


        public float Left => this.X;
        public float Top => this.Y;
        public float Right => this.X+this.Width;
        public float Bottom => this.Y+this.Height;

        public Vector2 LeftTop => new Vector2(this.Left, this.Top);
        public Vector2 RightTop => new Vector2(this.Right, this.Top);
        public Vector2 RightBottom => new Vector2(this.Right, this.Bottom);
        public Vector2 LeftBottom => new Vector2(this.Left, this.Bottom);



        public float CenterX => this.X + this.Width / 2;
        public float CenterY => this.Y + this.Height / 2;

        public Vector2 Center => new Vector2(this.CenterX, this.CenterY);


        public Rect ToRect() => new Rect(this.X, this.Y, this.Width, this.Height);

        /// <summary>Returns whether the area filled by the rect contains the specified point.</summary>
        public bool FillContainsPoint(Vector2 point)=>point.X > this.Left && point.X < this.Right && point.Y > this.Top && point.Y < this.Bottom;
        

        public static VectorRect Transform(VectorRect rect, Matrix3x2 matrix)
        {
            Vector2 leftTop = Vector2.Transform(rect.LeftTop, matrix);
            Vector2 rightBottom = Vector2.Transform(rect.RightBottom, matrix);
            return new VectorRect(leftTop,rightBottom);
        }

        /// <summary>Dstance of radius node' between centerTop node.</summary>
        public static float NodeRadius = 12.0f;
        public static bool NodeRadiusOut(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() > 12.0f * 12.0f;


        /// <summary>Min distance of node between node which is not center node.</summary>
        public static float NodeDistance = 20.0f;
        public static bool NodeDistanceOut(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() > 20.0f * 20.0f;


        /// <summary>Get radian control node.</summary>
        public static Vector2 GetRadianNode(VectorRect rect, Matrix3x2 canvasToVirtualToControlMatrix)
        {
            Vector2 centerTop = (canvasToVirtualToControlMatrix.Translation + Vector2.Transform(new Vector2(rect.Width, 0), canvasToVirtualToControlMatrix)) / 2;
            Vector2 centerBottom = (Vector2.Transform(new Vector2(0, rect.Height), canvasToVirtualToControlMatrix) + Vector2.Transform(rect.Size, canvasToVirtualToControlMatrix)) / 2;
            Vector2 centerTopBottom = centerTop - centerBottom;
            return centerTop - centerTopBottom * 40 / centerTopBottom.Length();
        }

        /// <summary>Draw nodes and lines ，just like【由】</summary>
        public static void DrawNodeLine(CanvasDrawingSession ds, VectorRect rect, Matrix3x2 canvasToVirtualToControlMatrix, bool isDrawNode = false)
        {
            Vector2 leftTop = canvasToVirtualToControlMatrix.Translation;
            Vector2 rightTop = Vector2.Transform(new Vector2(rect.Width, 0), canvasToVirtualToControlMatrix);
            Vector2 rightBottom = Vector2.Transform(rect.Size, canvasToVirtualToControlMatrix);
            Vector2 leftBottom = Vector2.Transform(new Vector2(0, rect.Height), canvasToVirtualToControlMatrix);

            //LTRB: Line
            ds.DrawLine(leftTop, rightTop, Colors.DodgerBlue);
            ds.DrawLine(rightTop, rightBottom, Colors.DodgerBlue);
            ds.DrawLine(rightBottom, leftBottom, Colors.DodgerBlue);
            ds.DrawLine(leftBottom, leftTop, Colors.DodgerBlue);

            if (isDrawNode)
            {
                //Center: Vector2
                Vector2 centerLeft = (leftTop + leftBottom) / 2;
                Vector2 centerTop = (leftTop + rightTop) / 2;
                Vector2 centerRight = (rightTop + rightBottom) / 2;
                Vector2 centerBottom = (leftBottom + rightBottom) / 2;

                float widthLength = (centerLeft - centerRight).Length();
                float heightLength = (centerTop - centerBottom).Length();

                //Center: Node
                if (widthLength > 40.0f)
                {
                    VectorRect.DrawNodeVector2(ds, centerTop);
                    VectorRect.DrawNodeVector2(ds, centerBottom);
                }
                if (heightLength > 40.0f)
                {
                    VectorRect.DrawNodeVector2(ds, centerLeft);
                    VectorRect.DrawNodeVector2(ds, centerRight);
                }

                //Radian: Vector
                Vector2 radian = centerTop - (centerBottom - centerTop) * 40 / heightLength;
                //Radian: Line
                ds.DrawLine(radian, centerTop, Colors.DodgerBlue);
                //Radian: Node
                VectorRect.DrawNodeVector(ds, radian);

                //LTRB: Node
                VectorRect.DrawNodeVector2(ds, leftTop);
                VectorRect.DrawNodeVector2(ds, rightTop);
                VectorRect.DrawNodeVector2(ds, rightBottom);
                VectorRect.DrawNodeVector2(ds, leftBottom);
            }
        }

        /// <summary>draw a ⊙ </summary>
        private static void DrawNodeVector(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Colors.DodgerBlue);
            ds.FillCircle(vector, 6, Colors.White);
        }
        /// <summary> draw a ●</summary>
        private static void DrawNodeVector2(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Colors.White);
            ds.FillCircle(vector, 6, Colors.DodgerBlue);
        }

    }


    public struct VectorRect222
    {
        public Vector2 Start;
        public Vector2 End;
        
        public VectorRect222(Vector2 start, Vector2 end)
        {
            this.Start = start;
            this.End = end;
        }
        public VectorRect222(Vector2 start, Vector2 end, MarqueeMode mode)
        {
            this.Start = start;
            this.End = end;

            switch (mode)
            {
                case MarqueeMode.Square:
                    float square = (Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y)) / 2;
                    this.Start = start;
                    this.End.X = start.X < end.X ? start.X + square : start.X - square;
                    this.End.Y = start.Y < end.Y ? start.Y + square : start.Y - square;
                    break;

                case MarqueeMode.Center:
                    this.Start = start + start - end;
                    this.End = end;
                    break;

                case MarqueeMode.SquareAndCenter:
                    float square2 = (Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y)) / 2;
                    this.Start.X = start.X - square2;
                    this.Start.Y = start.Y - square2;
                    this.End.X = start.X + square2;
                    this.End.Y = start.Y + square2;
                    break;

                default:
                    break;
            }

        }

        public VectorRect222(Rect rect)
        {
            this.Start.X = (float)rect.Left;
            this.Start.Y = (float)rect.Top;
            this.End.X = (float)rect.Right;
            this.End.Y = (float)rect.Bottom;
        }

        public float X => Math.Min(this.Start.X, this.End.X);
        public float Y => Math.Min(this.Start.Y, this.End.Y);
        public float Width => Math.Abs(this.Start.X - this.End.X);
        public float Height => Math.Abs(this.Start.Y - this.End.Y);
        
        public float Left => this.Start.X;
        public float Top => this.Start.Y;
        public float Right => this.End.X;
        public float Bottom => this.End.Y;

        public Vector2 LeftTop => this.Start;
        public Vector2 RightTop => new Vector2(this.End.X, this.Start.Y);
        public Vector2 RightBottom => this.End;
        public Vector2 LeftBottom => new Vector2(this.Start.X, this.End.Y);

        public Vector2 Center => new Vector2((this.Start.X + this.End.X) / 2, (this.Start.Y + this.End.Y) / 2);




    }
}
