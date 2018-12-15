using Microsoft.Graphics.Canvas;
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
    public struct Transformer
    {

        

        public float Width;
        public float Height;

        public Vector2 Postion;

        public bool DisabledRadian;
        private float radian;
        public float Radian
        {
            get => this.DisabledRadian ? 0 : radian;
            set => radian = value;
        }


        public Matrix3x2 Matrix => this.DisabledRadian ? Matrix3x2.CreateTranslation(this.Postion) : 
            Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) * 
            Matrix3x2.CreateRotation(this.Radian) * 
            Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2) * 
            Matrix3x2.CreateTranslation(this.Postion);





        public static Transformer CreateFromRect(Rect rect, float radian = 0.0f, bool disabledRadian=false) => new Transformer
        {
            Width = (float)rect.Width,
            Height = (float)rect.Height,
            Postion = new Vector2((float)rect.X, (float)rect.Y),

            Radian= radian,
            DisabledRadian =disabledRadian
        };



        /// <summary>Returns whether the area filled by the rect contains the specified point.</summary>
        public bool FillContainsPoint(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.DisabledRadian ? Matrix3x2.CreateTranslation(-this.Postion) : Matrix3x2.CreateTranslation(-this.Postion) * Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) * Matrix3x2.CreateRotation(-this.Radian) * Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);
            Vector2 v = Vector2.Transform(point, inverseMatrix);
            return v.X > 0 && v.X < this.Height && v.Y > 0 && v.Y < this.Width;
        }




        /// <summary>Dstance of radius node' between centerTop node.</summary>
        public static float NodeRadius = 12.0f;
        public static bool NodeRadiusOut(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() > Transformer.NodeRadius * Transformer.NodeRadius;

        /// <summary>Min distance of node between node which is not center node.</summary>
        public static float NodeDistance = 20.0f;
        public static bool NodeDistanceOut(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() > Transformer.NodeDistance * Transformer.NodeDistance;




        /// <summary>Get radian control node.</summary>
        public static Vector2 GetRadianNode(Transformer transformer, Matrix3x2 canvasToVirtualToControlMatrix)
        {
            Matrix3x2 matrix = transformer.Matrix * canvasToVirtualToControlMatrix;

            Vector2 centerTop = (matrix.Translation + Vector2.Transform(new Vector2(transformer.Width, 0), matrix)) / 2;
            Vector2 centerTopBottom = (Vector2.Transform(new Vector2(0, transformer.Height), matrix) + Vector2.Transform(new Vector2(transformer.Width, transformer.Height), matrix)) / 2 - centerTop;

            return centerTop - centerTopBottom * 40 / centerTopBottom.Length();
        }

        /// <summary>Draw nodes and lines ，just like【由】</summary>
        public static void DrawNodeLine(CanvasDrawingSession ds, Transformer transformer, Matrix3x2 canvasToVirtualToControlMatrix, bool isDrawNode = false)
        {
            Matrix3x2 matrix = transformer.Matrix * canvasToVirtualToControlMatrix;

            Vector2 leftTop = matrix.Translation;
            Vector2 rightTop = Vector2.Transform(new Vector2(transformer.Width, 0), matrix);
            Vector2 rightBottom = Vector2.Transform(new Vector2(transformer.Width, transformer.Height), matrix);
            Vector2 leftBottom = Vector2.Transform(new Vector2(0, transformer.Height), matrix);

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
                    Transformer.DrawNodeVector2(ds, centerTop);
                    Transformer.DrawNodeVector2(ds, centerBottom);
                }
                if (heightLength > 40.0f)
                {
                    Transformer.DrawNodeVector2(ds, centerLeft);
                    Transformer.DrawNodeVector2(ds, centerRight);
                }

                if (transformer.DisabledRadian==false)
                {
                    //Radian: Vector
                    Vector2 radian = centerTop - (centerBottom - centerTop) * 40 / heightLength;
                    //Radian: Line
                    ds.DrawLine(radian, centerTop, Colors.DodgerBlue);
                    //Radian: Node
                    Transformer.DrawNodeVector(ds, radian);
                }

                //LTRB: Node
                Transformer.DrawNodeVector2(ds, leftTop);
                Transformer.DrawNodeVector2(ds, rightTop);
                Transformer.DrawNodeVector2(ds, rightBottom);
                Transformer.DrawNodeVector2(ds, leftBottom);
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
}
