using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo.Library
{
    /// <summary>The marquee is a tool that can make selections that are  rectangular and elliptical.</summary>
    public class MarqueeTool
    {

        readonly Color LightBlue = Color.FromArgb(255, 128, 198, 255);
        readonly Color Blue = Color.FromArgb(255, 54, 135, 230);
        readonly Color White = Color.FromArgb(255, 255, 255, 255);
        readonly Color Black = Color.FromArgb(255, 0, 0, 0);
        readonly Color Shadow = Color.FromArgb(70, 127, 127, 127);

        Vector2 start;
        Vector2 end;
        readonly List<Vector2> list = new List<Vector2>();


        public MarqueeToolType Tool = MarqueeToolType.Rectangular;
        public MarqueeMode MarqueeMode = MarqueeMode.None;
        public MarqueeCompositeMode CompositeMode = MarqueeCompositeMode.New;

        //Delegate
        public delegate void CompleteHandler();
        /// <summary>Trigger it when the operation is complete.</summary>
        public event CompleteHandler Complete = null;


         
        #region Draw


        /// <summary>Draw MarqueeTool's operation.</summary>
        /// <param name="creator">CanvasdControl</param>
        /// <param name="ds">CanvasdControl's DrawingSession</param>
        /// <param name="matrix">The transformation of the canvas</param>
        public void Draw(ICanvasResourceCreator creator, CanvasDrawingSession ds)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                case MarqueeToolType.Elliptical:
                    this.RectangularEllipticalDraw(ds);
                    break;

                case MarqueeToolType.Polygonal:
                case MarqueeToolType.FreeHand:
                    this.PolygonalFreeHandDraw(creator, ds);
                    break;
            }
        }
        public void Draw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Matrix3x2 matrix)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                case MarqueeToolType.Elliptical:
                    this.RectangularEllipticalDraw(ds);
                    break;

                case MarqueeToolType.Polygonal:
                case MarqueeToolType.FreeHand:
                    this.PolygonalFreeHandDraw(creator, ds, matrix);
                    break;
            }
        }


        private void RectangularEllipticalDraw(CanvasDrawingSession ds)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                    this.RectangularDrawOrRender(ds, this.start, this.end, false);
                    break;

                case MarqueeToolType.Elliptical:
                    this.EllipticalDrawOrRender(ds, this.start, this.end, false);
                    break;
            }
        }


        private void PolygonalFreeHandDraw(ICanvasResourceCreator creator, CanvasDrawingSession ds)
        {
            if (this.list == null) return;
            if (this.list.Count == 0) return;

            switch (this.Tool)
            {
                case MarqueeToolType.Polygonal:
                    this.PolygonalDrawOrRender(creator, ds, this.list.ToArray(), false);
                    break;
                case MarqueeToolType.FreeHand:
                    this.FreeHandDraOrRender(creator, ds, this.list.ToArray(), false);
                    break;
                default:
                    break;
            }
        }
        private void PolygonalFreeHandDraw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Matrix3x2 matrix)
        {
            if (this.list == null) return;
            if (this.list.Count == 0) return;

            Vector2[] array = new Vector2[this.list.Count];
            for (int i = 0; i < this.list.Count; i++) array[i] = Vector2.Transform(this.list[i], matrix);

            switch (this.Tool)
            {
                case MarqueeToolType.Polygonal:
                    this.PolygonalDrawOrRender(creator, ds, array, false);
                    break;
                case MarqueeToolType.FreeHand:
                    this.FreeHandDraOrRender(creator, ds, array, false);
                    break;
                default:
                    break;
            }
        }


        #endregion


        #region Render


        /// <summary>Render the marquee.</summary>
        /// <param name="creator">CanvasControl</param>
        /// <param name="selection">Marquee selection</param>      
        /// <param name="matrix">The transformation of the canvas</param>
        public void Render(ICanvasResourceCreator creator, CanvasRenderTarget selection)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                case MarqueeToolType.Elliptical:
                    this.RectangularEllipticalRender(creator, selection);
                    break;

                case MarqueeToolType.Polygonal:
                case MarqueeToolType.FreeHand:
                    this.PolygonalFreeHandRender(creator, selection);
                    break;
            }
        }
        public void Render(ICanvasResourceCreator creator, CanvasRenderTarget selection, Matrix3x2 matrix)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                case MarqueeToolType.Elliptical:
                    this.RectangularEllipticalRender(creator, selection, matrix);
                    break;

                case MarqueeToolType.Polygonal:
                case MarqueeToolType.FreeHand:
                    this.PolygonalFreeHandRender(creator, selection);
                    break;
            }
        }


        private void RectangularEllipticalRender(ICanvasResourceCreator creator, CanvasRenderTarget selection)
        {
            CanvasCommandList command = new CanvasCommandList(creator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                switch (this.Tool)
                {
                    case MarqueeToolType.Rectangular:
                        this.RectangularDrawOrRender(ds, this.start, this.end, true);
                        break;
                    case MarqueeToolType.Elliptical:
                        this.EllipticalDrawOrRender(ds, this.start, this.end, true);
                        break;
                }
            }

            using (CanvasDrawingSession ds = selection.CreateDrawingSession())
            {
                if (this.CompositeMode == MarqueeCompositeMode.New) ds.Clear(Color.FromArgb(0, 0, 0, 0));
                ds.DrawImage(command, 0, 0, selection.Bounds, 1, CanvasImageInterpolation.Linear, this.GetCanvasComposite(this.CompositeMode));
            }
        }
        private void RectangularEllipticalRender(ICanvasResourceCreator creator, CanvasRenderTarget selection, Matrix3x2 matrix)
        {
            CanvasCommandList command = new CanvasCommandList(creator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                switch (this.Tool)
                {
                    case MarqueeToolType.Rectangular:
                        this.RectangularDrawOrRender(ds, this.start, this.end, true);
                        break;
                    case MarqueeToolType.Elliptical:
                        this.EllipticalDrawOrRender(ds, this.start, this.end, true);
                        break;
                }
            }

            ICanvasEffect effect = new Transform2DEffect
            {
                Source = command,
                TransformMatrix = matrix
            };

            using (CanvasDrawingSession ds = selection.CreateDrawingSession())
            {
                if (this.CompositeMode == MarqueeCompositeMode.New) ds.Clear(Color.FromArgb(0, 0, 0, 0));
                ds.DrawImage(effect, 0, 0, selection.Bounds, 1, CanvasImageInterpolation.Linear, this.GetCanvasComposite(this.CompositeMode));
            }
        }


        private void PolygonalFreeHandRender(ICanvasResourceCreator creator, CanvasRenderTarget selection)
        {
            if (this.list == null) return;
            if (this.list.Count == 0) return;

            CanvasCommandList command = new CanvasCommandList(creator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                switch (this.Tool)
                {
                    case MarqueeToolType.Polygonal:
                        this.PolygonalDrawOrRender(creator, ds, this.list.ToArray(), true);
                        break;
                    case MarqueeToolType.FreeHand:
                        this.FreeHandDraOrRender(creator, ds, this.list.ToArray(), true);
                        break;
                }
            }

            using (CanvasDrawingSession ds = selection.CreateDrawingSession())
            {
                if (this.CompositeMode == MarqueeCompositeMode.New) ds.Clear(Color.FromArgb(0, 0, 0, 0));
                ds.DrawImage(command, 0, 0, selection.Bounds, 1, CanvasImageInterpolation.Linear, this.GetCanvasComposite(this.CompositeMode));
            }
        }

         
        private CanvasComposite GetCanvasComposite(MarqueeCompositeMode mode)
        {
            switch (mode)
            {
                case MarqueeCompositeMode.New: return CanvasComposite.SourceOver;
                case MarqueeCompositeMode.Add: return CanvasComposite.SourceOver;
                case MarqueeCompositeMode.Subtract: return CanvasComposite.DestinationOut;
                case MarqueeCompositeMode.Intersect: return CanvasComposite.DestinationIn;
                case MarqueeCompositeMode.Xor: return CanvasComposite.Xor;
                default: break;
            }
            return CanvasComposite.SourceOver;
        }


        #endregion


        #region DrawAndRender


        /// <summary>Rectangular: Calculate location and draw graphics.</summary>
        private void RectangularDrawOrRender(CanvasDrawingSession ds, Vector2 start, Vector2 end, bool isRender)
        {
            switch (this.MarqueeMode)
            {
                case MarqueeMode.None:
                    float x0 = Math.Min(start.X, end.X);
                    float y0 = Math.Min(start.Y, end.Y);
                    float w0 = Math.Abs(start.X - end.X);
                    float h0 = Math.Abs(start.Y - end.Y);

                    if (isRender) ds.FillRectangle(x0, y0, w0, h0, this.LightBlue);
                    else
                    {
                        ds.DrawRectangle(x0, y0, w0, h0, this.Black, 2.0f);
                        ds.DrawRectangle(x0, y0, w0, h0, this.White, 1.0f);
                    }
                    break;


                case MarqueeMode.Square:
                    float w1 = Math.Abs(start.X - end.X);
                    float h1 = Math.Abs(start.Y - end.Y);
                    float square = (w1 + h1) / 2;

                    float x1 = (end.X > start.X) ? start.X : start.X - square;
                    float y1 = (end.Y > start.Y) ? start.Y : start.Y - square;

                    if (isRender) ds.FillRectangle(x1, y1, square, square, this.LightBlue);
                    else
                    {
                        ds.DrawRectangle(x1, y1, square, square, this.Black, 2.0f);
                        ds.DrawRectangle(x1, y1, square, square, this.White, 1.0f);
                    }
                    break;


                case MarqueeMode.Center:
                    float w2 = Math.Abs(start.X - end.X);
                    float h2 = Math.Abs(start.Y - end.Y);

                    float x2 = start.X - w2;
                    float y2 = start.Y - h2;

                    if (isRender) ds.FillRectangle(x2, y2, 2 * w2, 2 * h2, this.LightBlue);
                    else
                    {
                        ds.DrawRectangle(x2, y2, 2 * w2, 2 * h2, this.Black, 2.0f);
                        ds.DrawRectangle(x2, y2, 2 * w2, 2 * h2, this.White, 1.0f);
                    }
                    break;


                case MarqueeMode.SquareAndCenter:
                    float w3 = Math.Abs(start.X - end.X);
                    float h3 = Math.Abs(start.Y - end.Y);
                    float squareHalf3 = (w3 + h3) / 2;
                    float square3 = 2 * squareHalf3;

                    float x3 = start.X - squareHalf3;
                    float y3 = start.Y - squareHalf3;

                    if (isRender) ds.FillRectangle(x3, y3, square3, square3, this.LightBlue);
                    else
                    {
                        ds.DrawRectangle(x3, y3, square3, square3, this.Black, 2.0f);
                        ds.DrawRectangle(x3, y3, square3, square3, this.White, 1.0f);
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>Elliptical: Calculate location and draw graphics.</summary>
        private void EllipticalDrawOrRender(CanvasDrawingSession ds, Vector2 start, Vector2 end, bool isRender)
        {
            switch (this.MarqueeMode)
            {
                case MarqueeMode.None:
                    float x0 = (start.X + end.X) / 2;
                    float y0 = (start.Y + end.Y) / 2;
                    float w0 = Math.Abs(start.X - end.X) / 2;
                    float h0 = Math.Abs(start.Y - end.Y) / 2;

                    if (isRender) ds.FillEllipse(x0, y0, w0, h0, this.LightBlue);
                    else
                    {
                        ds.DrawEllipse(x0, y0, w0, h0, this.Black, 2.0f);
                        ds.DrawEllipse(x0, y0, w0, h0, this.White, 1.0f);
                    }
                    break;


                case MarqueeMode.Square:
                    float w1 = Math.Abs(start.X - end.X);
                    float h1 = Math.Abs(start.Y - end.Y);
                    float squareHalf = (w1 + h1) / 4;

                    float x1 = (end.X > start.X) ? start.X + squareHalf : start.X - squareHalf;
                    float y1 = (end.Y > start.Y) ? start.Y + squareHalf : start.Y - squareHalf;

                    if (isRender) ds.FillCircle(x1, y1, squareHalf, this.LightBlue);
                    else
                    {
                        ds.DrawCircle(x1, y1, squareHalf, this.Black, 2.0f);
                        ds.DrawCircle(x1, y1, squareHalf, this.White, 1.0f);
                    }
                    break;


                case MarqueeMode.Center:
                    float w2 = Math.Abs(start.X - end.X) / 2;
                    float h2 = Math.Abs(start.Y - end.Y) / 2;

                    if (isRender) ds.FillEllipse(start, w2, h2, this.LightBlue);
                    else
                    {
                        ds.DrawEllipse(start, w2, h2, this.Black, 2.0f);
                        ds.DrawEllipse(start, w2, h2, this.White, 1.0f);
                    }
                    break;


                case MarqueeMode.SquareAndCenter:
                    float w3 = Math.Abs(start.X - end.X) / 2;
                    float h3 = Math.Abs(start.Y - end.Y) / 2;
                    float square2 = (w3 + h3) / 2;

                    if (isRender) ds.FillCircle(start, square2, this.LightBlue);
                    else
                    {
                        ds.DrawCircle(start, square2, this.Black, 2.0f);
                        ds.DrawCircle(start, square2, this.White, 1.0f);
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>Polygonal: Calculate location and draw graphics.</summary>
        private void PolygonalDrawOrRender(ICanvasResourceCreator creator, CanvasDrawingSession ds, Vector2[] array, bool isRender)
        {
            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(creator, array);

            if (isRender) ds.FillGeometry(geometry, this.LightBlue);
            else
            {
                ds.DrawGeometry(geometry, this.Black, 2.0f);
                ds.DrawGeometry(geometry, this.White, 1.0f);

                this.DrawNodeVector(ds, array.First());
                this.DrawNodeVector(ds, array.Last());
            }
        }

        /// <summary>FreeHand: Calculate location and draw graphics.</summary>
        private void FreeHandDraOrRender(ICanvasResourceCreator creator, CanvasDrawingSession ds, Vector2[] array, bool isRender)
        {
            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(creator, array);

            if (isRender) ds.FillGeometry(geometry, this.LightBlue);
            else
            {
                ds.DrawGeometry(geometry, this.Black, 2.0f);
                ds.DrawGeometry(geometry, this.White, 1.0f);
            }
        }


        /// <summary>Draw a ● </summary>
        private void DrawNodeVector(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, this.Shadow);
            ds.FillCircle(vector, 8, this.White);
            ds.FillCircle(vector, 6, this.Blue);
        }


        #endregion


        #region Operator


        bool isPolygonalOperator = false;


        /// <summary>Call it at the starting of the operation</summary>
        /// <param name="v">Vector2</param>      
        /// <param name="inversionMatrix">The inversion transformation of the canvas</param>
        public void Operator_Start(Vector2 v)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                case MarqueeToolType.Elliptical:
                    this.start = this.end = v;
                    break;

                case MarqueeToolType.Polygonal:
                    this.PolygonalOperator_Start(v);
                    break;

                case MarqueeToolType.FreeHand:
                    this.FreeHandOperator_Start(v);
                    break;

                default:
                    break;
            }
        }
        public void Operator_Start(Vector2 v, Matrix3x2 inversionMatrix)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                case MarqueeToolType.Elliptical:
                    this.start = this.end = v;
                    break;

                case MarqueeToolType.Polygonal:
                    this.PolygonalOperator_Start(Vector2.Transform(v, inversionMatrix));
                    break;

                case MarqueeToolType.FreeHand:
                    this.FreeHandOperator_Start(Vector2.Transform(v, inversionMatrix));
                    break;

                default:
                    break;
            }
        }



        /// <summary>Call it at the moving of the operation</summary>
        /// <param name="v">Vector2</param>
        /// <param name="inversionMatrix">The inversion transformation of the canvas</param>
        public void Operator_Delta(Vector2 v)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                case MarqueeToolType.Elliptical:
                    this.end = v;
                    break;

                case MarqueeToolType.Polygonal:
                    this.PolygonalOperator_Delta(v);
                    break;

                case MarqueeToolType.FreeHand:
                    this.FreeHandOperator_Delta(v);
                    break;

                default:
                    break;
            }
        }
        public void Operator_Delta(Vector2 v, Matrix3x2 inversionMatrix)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                case MarqueeToolType.Elliptical:
                    this.end = v;
                    break;

                case MarqueeToolType.Polygonal:
                    this.PolygonalOperator_Delta(Vector2.Transform(v, inversionMatrix));
                    break;

                case MarqueeToolType.FreeHand:
                    this.FreeHandOperator_Delta(Vector2.Transform(v, inversionMatrix));
                    break;

                default:
                    break;
            }
        }


        /// <summary>Call it at the completing of the operation</summary>
        /// <param name="v">Vector2</param>
        /// <param name="inversionMatrix">The inversion transformation of the canvas</param>
        public void Operator_Complete(Vector2 v)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                case MarqueeToolType.Elliptical:
                    this.Complete?.Invoke();//Delegate
                    this.start = this.end = Vector2.Zero;
                    break;

                case MarqueeToolType.Polygonal:
                    this.PolygonalOperator_Complete(v);
                    break;

                case MarqueeToolType.FreeHand:
                    this.FreeHandOperator_Complete(v);
                    break;

                default:
                    break;
            }
        }
        public void Operator_Complete(Vector2 v, Matrix3x2 inversionMatrix)
        {
            switch (this.Tool)
            {
                case MarqueeToolType.Rectangular:
                case MarqueeToolType.Elliptical:
                    this.Complete?.Invoke();//Delegate
                    this.start = this.end = Vector2.Zero;
                    break;

                case MarqueeToolType.Polygonal:
                    this.PolygonalOperator_Complete(Vector2.Transform(v, inversionMatrix), inversionMatrix.M11, inversionMatrix.M22);
                    break;

                case MarqueeToolType.FreeHand:
                    this.FreeHandOperator_Complete(Vector2.Transform(v, inversionMatrix));
                    break;

                default:
                    break;
            }
        }


        


        private void FreeHandOperator_Start(Vector2 v) => this.list.Add(v);
        private void FreeHandOperator_Delta(Vector2 v) => this.list.Add(v);
        private void FreeHandOperator_Complete(Vector2 v)
        {
            this.Complete?.Invoke();//Delegate

            this.list.Clear();
        }




        private void PolygonalOperator_Start(Vector2 v)
        {
            if (this.isPolygonalOperator) this.PolygonalOperator_Delta(v);
            else this.FreeHandOperator_Start(v);
        }
        private void PolygonalOperator_Delta(Vector2 v)
        {
            if (this.list.Count > 1) this.list[this.list.Count - 1] = v;
        }
        private void PolygonalOperator_Complete(Vector2 v, float xscale = 1.0f, float Yscale = 1.0f)
        {
            if (this.isPolygonalOperator)
            {
                if (this.list.Count > 2)
                {
                    if ((this.list.First() - this.list.Last()).LengthSquared() < 100.0f * xscale * Yscale)
                    {
                        this.isPolygonalOperator = false;
                        this.FreeHandOperator_Complete(v);
                        return;
                    }
                }
            }

            this.isPolygonalOperator = true;
            this.FreeHandOperator_Delta(v);
        }


        #endregion
                
    }

    /// <summary> Tools of different shapes </summary>
    public enum MarqueeToolType
    {
        /// <summary> 口 </summary>
        Rectangular,
        /// <summary> ◯ </summary>
        Elliptical,
        /// <summary> 🗨 </summary>
        Polygonal,
        /// <summary> 🗯 </summary>
        FreeHand,
    }

    /// <summary> Constraints the marquee </summary>
    public enum MarqueeMode
    {
        /// <summary> None </summary>
        None,
        /// <summary> A square marquee </summary>
        Square,
        /// <summary> Mouse is the center of the marquee </summary>
        Center,
        /// <summary> Both of these are </summary>
        SquareAndCenter,
    }

    /// <summary>The composite mode used for the marquee. </summary>
    public enum MarqueeCompositeMode
    {
        /// <summary>New bitmaps</summary>
        New,
        /// <summary>Union of source and destination bitmaps. </summary>
        Add,
        /// <summary>Region of the source bitmap. </summary>
        Subtract,
        /// <summary> Intersection of source and destination bitmaps.</summary>
        Intersect,

        /// <summary>Union of source and destination bitmaps with xor function for pixels that overlap. </summary>
        Xor,
    }

}

