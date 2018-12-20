using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels
{

    public struct VectorLine
    {
        /// <summary>
        /// [Diagonal Point]
        /// (Such as: Left Point)
        /// </summary>
        public Vector2 Diagonal;

        /// <summary> 
        /// [Symmetric Point]
        /// (Diagonal Point as origin, the Symmetric Point of a Point) 
        /// </summary>
        public Vector2 Symmetric;

        /// <summary> 
        /// [Center Point]
        /// (The Center Point between Point and Diagonal Point) 
        /// </summary>
        public Vector2 Center;

        // These points in a line: 
        //      S[Symmetric Point]、D[Diagonal Point]、C[Center Point]、P[Point) and F[FootPoint] .
        //
        //                                         2m                                           1m                          1m
        //————•————————————————•————————•————————•————
        //              S                                                          D                            C                             P

    }

    public struct VectorDistance
    {
        /// <summary> Distance between [Foot Point] and [Diagonal Point] . </summary>
        public float FD;
        /// <summary> Distance between [Foot Point] and [Point] . </summary>
        public float FP;
        /// <summary> Distance between [Foot Point] and [Center Point] . </summary>
        public float FC;
        /// <summary> Distance between [Point] and [Center Point] .</summary>
        public float PC;
        /// <summary> Distance between [Foot Point] and [Symmetric Point] . </summary>
        public float FS;
        /// <summary> Distance between [Point] and [Diagonal Point] . </summary>
        public float PD;
    }


    public abstract class ToolCursorScaleViewModel : ToolViewModel2
    {
        protected Transformer StartTransformer;
        protected float Cos;
        protected float Sin;

        public override void Start(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            this.StartTransformer.CopyWith(layer.Transformer);

            this.Cos = (float)Math.Cos(-layer.Transformer.Radian);
            this.Sin = (float)Math.Sin(-layer.Transformer.Radian);
        }
        public override void Delta(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
        }
        public override void Complete(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
        }

        public override void Draw(CanvasDrawingSession ds, Layer layer, DrawViewModel viewModel)
        {
            Transformer.DrawBoundNodesWithRotation(ds, layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);

            /*
             * 
           ds.DrawLine(this.Line.Symmetric, this.Point, Colors.Red);

           ds.DrawText("S", this.Line.Symmetric,  Colors.Red);
           ds.FillCircle(this.Line.Symmetric, 6, Colors.Red);

           ds.DrawText("D", this.Line.Diagonal, Colors.Red);
           ds.FillCircle(this.Line.Diagonal, 6, Colors.Red);

           ds.DrawText("C", this.Line.Center, Colors.Red);
           ds.FillCircle(this.Line.Center, 6, Colors.Red);

           ds.DrawText("P", this.Point, Colors.Red);
           ds.FillCircle(this.Point, 6, Colors.Red);

             */

        }

    }
}

