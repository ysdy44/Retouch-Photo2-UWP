using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels
{

    /// <summary>
    /// These points in a line:  
    /// ------S[Symmetric Point]、D[Diagonal Point]、C[Center Point]、P[Point) and F[FootPoint] .
    /// </summary>
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

    /// <summary>
    /// Distance of points on the [VectorLine]
    /// </summary>
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


    public abstract class ScaleViewModel : ToolViewModel2
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        //@Override
        public abstract Vector2 GetPoint(Layer layer, Matrix3x2 matrix);
        public abstract Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix);
        public abstract void SetPostion(Layer layer, Transformer startTransformer, float xCos, float xSin, float yCos, float ySin);


        protected Transformer StartTransformer;

        protected Vector2 Point;
        protected VectorLine Line;

        protected float XCos, XSin;
        protected float YCos,YSin;


        public override void Start(Vector2 point, Layer layer)
        {
            this.StartTransformer.CopyWith(layer.Transformer);

            float x = -layer.Transformer.Radian;
            this.XCos = (float)Math.Cos(x);
            this.XSin = (float)Math.Sin(x);

            float y =- layer.Transformer.Radian + layer.Transformer.Skew;
            this.YCos = (float)Math.Cos(y);
            this.YSin = (float)Math.Sin(y);
        }
        public override void Delta(Vector2 point, Layer layer)
        {
        }
        public override void Complete(Vector2 point, Layer layer)
        {
        }

        public override void Draw(CanvasDrawingSession ds, Layer layer)
        {
            Transformer.DrawBoundNodesWithRotation(ds, layer.Transformer, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }


        public VectorDistance GetVectorDistance(Vector2 footPoint, Vector2 point, VectorLine line) => new VectorDistance
        {
            FD = Vector2.Distance(footPoint, line.Diagonal),
            FP = Vector2.Distance(footPoint, point),
            FC = Vector2.Distance(footPoint, line.Center),
            PC = Vector2.Distance(point, line.Center),
            FS = Vector2.Distance(footPoint, line.Symmetric),
            PD = Vector2.Distance(point, line.Diagonal),
        };

    }
}

