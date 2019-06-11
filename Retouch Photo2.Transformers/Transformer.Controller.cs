using System;
using System.Numerics;

namespace Retouch_Photo2.Transformers
{
    /// <summary> Represents a Transformer (LeftTop, RightTop, RightBottom, LeftBottom). </summary>
    public partial struct Transformer
    {

        /// <summary>
        /// It controls the transformation of <see cref = "Transformer" />.
        /// </summary>
        /// <param name="mode"> TransformerMode </param>
        /// <param name="startingPoint"> starting point </param>
        /// <param name="point"> point </param>
        /// <param name="startingTransformer"> starting transformer </param>
        /// <param name="inverseMatrix"> inverse matrix </param>
        /// <param name="isRatio"> Maintain a ratio when scaling.  </param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isStepFrequency"> Step Frequency when spinning. </param>
        /// <returns> Transformer </returns>
        public static Transformer Controller(TransformerMode mode, Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio = false, bool isCenter = false, bool isStepFrequency = false)
        {
            switch (mode)
            {
                case TransformerMode.None: return startingTransformer;

                case TransformerMode.Translation: return Transformer.Translation(startingPoint, point, startingTransformer, inverseMatrix);

                case TransformerMode.Rotation: return Transformer.Rotation(startingPoint, point, startingTransformer, inverseMatrix, isStepFrequency);

                case TransformerMode.SkewLeft: return Transformer.SkewLeft(startingPoint, point, startingTransformer, inverseMatrix, isCenter);
                case TransformerMode.SkewTop: return Transformer.SkewTop(startingPoint, point, startingTransformer, inverseMatrix, isCenter);
                case TransformerMode.SkewRight: return Transformer.SkewRight(startingPoint, point, startingTransformer, inverseMatrix, isCenter);
                case TransformerMode.SkewBottom: return Transformer.SkewBottom(startingPoint, point, startingTransformer, inverseMatrix, isCenter);

                case TransformerMode.ScaleLeft: return Transformer.ScaleLeft(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleTop: return Transformer.ScaleTop(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleRight: return Transformer.ScaleRight(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleBottom: return Transformer.ScaleBottom(point, startingTransformer, inverseMatrix, isRatio, isCenter);

                case TransformerMode.ScaleLeftTop: return Transformer.ScaleLeftTop(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleRightTop: return Transformer.ScaleRightTop(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleRightBottom: return Transformer.ScaleRightBottom(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleLeftBottom: return Transformer.ScaleLeftBottom(point, startingTransformer, inverseMatrix, isRatio, isCenter);
            }

            return startingTransformer;
        }


        //Translation
        private static Transformer Translation(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix)
        {
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Vector2 vector = canvasPoint - canvasStartingPoint;

            return Transformer.Add(startingTransformer, vector);
        }

        //Rotation
        private static Transformer Rotation(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isStepFrequency)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 center = startingTransformer.Center;

            float canvasRadian = Transformer.VectorToRadians(canvasPoint - center);
            if (isStepFrequency) canvasRadian = Transformer.RadiansStepFrequency(canvasRadian);

            float canvasStartingRadian = Transformer.VectorToRadians(canvasStartingPoint - center);
            float radian = canvasRadian - canvasStartingRadian;

            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(radian, center);
            return Transformer.Multiplies(startingTransformer, rotationMatrix);
        }

        //Skew
        private static Vector2 Skew(Vector2 startingPoint, Vector2 point, Vector2 linePoineA, Vector2 linePoineB)
        {
            Vector2 canvasStartingSkewPoint = Transformer.FootPoint(startingPoint, linePoineA, linePoineB);
            Vector2 canvasSkewPoint = Transformer.FootPoint(point, linePoineA, linePoineB);

            Vector2 vector = canvasSkewPoint - canvasStartingSkewPoint;
            Vector2 halfVector = vector / 2;

            return halfVector;
        }

        private static Transformer SkewLeft(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isCenter)
        {
            Vector2 linePoineA = startingTransformer.LeftTop;
            Vector2 linePoineB = startingTransformer.LeftBottom;
            Vector2 vector = Transformer.Skew(startingPoint, point, linePoineA, linePoineB);

            startingTransformer.LeftTop += vector;
            startingTransformer.LeftBottom += vector;

            if (isCenter)
            {
                startingTransformer.RightTop -= vector;
                startingTransformer.RightBottom -= vector;
            }

            return startingTransformer;
        }
        private static Transformer SkewTop(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isCenter)
        {
            Vector2 linePoineA = startingTransformer.LeftTop;
            Vector2 linePoineB = startingTransformer.RightTop;
            Vector2 vector = Transformer.Skew(startingPoint, point, linePoineA, linePoineB);

            startingTransformer.LeftTop += vector;
            startingTransformer.RightTop += vector;

            if (isCenter)
            {
                startingTransformer.RightBottom -= vector;
                startingTransformer.LeftBottom -= vector;
            }

            return startingTransformer;
        }
        private static Transformer SkewRight(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isCenter)
        {
            Vector2 linePoineA = startingTransformer.RightTop;
            Vector2 linePoineB = startingTransformer.RightBottom;
            Vector2 vector = Transformer.Skew(startingPoint, point, linePoineA, linePoineB);

            startingTransformer.RightTop += vector;
            startingTransformer.RightBottom += vector;

            if (isCenter)
            {
                startingTransformer.LeftTop -= vector;
                startingTransformer.LeftBottom -= vector;
            }

            return startingTransformer;
        }
        private static Transformer SkewBottom(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isCenter)
        {
            Vector2 linePoineA = startingTransformer.LeftBottom;
            Vector2 linePoineB = startingTransformer.RightBottom;
            Vector2 vector = Transformer.Skew(startingPoint, point, linePoineA, linePoineB);

            startingTransformer.RightBottom += vector;
            startingTransformer.LeftBottom += vector;

            if (isCenter)
            {
                startingTransformer.LeftTop -= vector;
                startingTransformer.RightTop -= vector;
            }

            return startingTransformer;
        }


        /// <summary> 
        /// Distance of points on these points in a line: 
        /// ------D[Diagonal Point]、C[Center Point]、P[Point) and F[FootPoint] .
        /// </summary>
        internal struct LineDistance
        {
            /// <summary> Distance between [Foot Point] and [Center Point] . </summary>
            public float FC;
            /// <summary> Distance between [Foot Point] and [Point] . </summary>
            public float FP;
            /// <summary> Distance between [Foot Point] and [Diagonal Point] . </summary>
            public float FD;
            /// <summary> Distance between [Point] and [Center Point] . </summary>
            public float PC;


            // These points in a line: 
            //      D[Diagonal Point]、C[Center Point]、P[Point] and F[FootPoint] .
            //
            //                                         2m                                           1m                          1m
            //————•————————————————•————————•————————•————
            //              D                                                          C                                                           P
            public LineDistance(Vector2 footPoint, Vector2 point, Vector2 center)
            {
                var diagonal = center + center - point;

                this.FC = Vector2.Distance(footPoint, center);
                this.FP = Vector2.Distance(footPoint, point);
                this.FD = Vector2.Distance(footPoint, diagonal);
                this.PC = Vector2.Distance(point, center);
            }

            /// <summary> Scale of [Foot Point] betwwen [Center Point] / scale of [Point] betwwen [Center Point] (may be negative)</summary>
            /// <param name="distance"> The distance </param>
            /// <returns>Scale</returns>
            public static float Scale(LineDistance distance)
            {
                float scale = distance.FC / distance.PC;
                bool isReverse = (distance.FP < distance.FD);
                return isReverse ? scale : -scale;
            }
        }


        //ScaleAround
        private static Transformer ScaleAround(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter, Vector2 linePoint, Vector2 lineDiagonalPoint, Func<Transformer, bool, Vector2, Transformer> _func)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Vector2 footPoint = Transformer.FootPoint(canvasPoint, linePoint, lineDiagonalPoint);

            if (isRatio)
            {
                Vector2 center = isCenter ? startingTransformer.Center : lineDiagonalPoint;

                LineDistance distance = new LineDistance(footPoint, linePoint, center);
                Matrix3x2 scaleMatrix = Matrix3x2.CreateScale(LineDistance.Scale(distance), center);

                return Transformer.Multiplies(startingTransformer, scaleMatrix);
            }
            else
            {
                Vector2 vector = footPoint - linePoint;
                return _func(startingTransformer, isCenter, vector);
            }
        }


        private static Transformer ScaleLeft(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterLeft;
            Vector2 lineDiagonalPoint = startingTransformer.CenterRight;

            return Transformer.ScaleAround(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleLeft);
        }
        static Transformer _funcScaleLeft(Transformer startingTransformer, bool isCenter, Vector2 vector)
        {
            startingTransformer.LeftTop += vector;
            startingTransformer.LeftBottom += vector;

            if (isCenter)
            {
                startingTransformer.RightTop -= vector;
                startingTransformer.RightBottom -= vector;
            }

            return startingTransformer;
        }

        private static Transformer ScaleTop(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterTop;
            Vector2 lineDiagonalPoint = startingTransformer.CenterBottom;

            return Transformer.ScaleAround(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleTop);
        }
        static Transformer _funcScaleTop(Transformer startingTransformer, bool isCenter, Vector2 vector)
        {
            startingTransformer.LeftTop += vector;
            startingTransformer.RightTop += vector;

            if (isCenter)
            {
                startingTransformer.LeftBottom -= vector;
                startingTransformer.RightBottom -= vector;
            }

            return startingTransformer;
        }

        private static Transformer ScaleRight(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterRight;
            Vector2 lineDiagonalPoint = startingTransformer.CenterLeft;

            return Transformer.ScaleAround(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleRight);
        }
        static Transformer _funcScaleRight(Transformer startingTransformer, bool isCenter, Vector2 vector)
        {
            startingTransformer.RightTop += vector;
            startingTransformer.RightBottom += vector;

            if (isCenter)
            {
                startingTransformer.LeftTop -= vector;
                startingTransformer.LeftBottom -= vector;
            }

            return startingTransformer;
        }

        private static Transformer ScaleBottom(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterBottom;
            Vector2 lineDiagonalPoint = startingTransformer.CenterTop;

            return Transformer.ScaleAround(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleBottom);
        }
        static Transformer _funcScaleBottom(Transformer startingTransformer, bool isCenter, Vector2 vector)
        {
            startingTransformer.LeftBottom += vector;
            startingTransformer.RightBottom += vector;

            if (isCenter)
            {
                startingTransformer.LeftTop -= vector;
                startingTransformer.RightTop -= vector;
            }

            return startingTransformer;
        }





        //ScaleCorner
        private static Transformer ScaleCorner(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter, Vector2 linePoint, Vector2 lineDiagonalPoint, Func<Vector2, Vector2, Vector2, Vector2, Transformer> _func)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (isRatio)
            {
                Vector2 center = isCenter ? startingTransformer.Center : lineDiagonalPoint;
                Vector2 footPoint = Transformer.FootPoint(canvasPoint, linePoint, center);

                LineDistance lineDistance = new LineDistance(footPoint, linePoint, center);
                Matrix3x2 scaleMatrix = Matrix3x2.CreateScale(LineDistance.Scale(lineDistance), center);

                return Transformer.Multiplies(startingTransformer, scaleMatrix);
            }
            else
            {
                Vector2 center = isCenter ? startingTransformer.Center * 2 - canvasPoint : lineDiagonalPoint;
                Vector2 horizontal = startingTransformer.CenterRight - startingTransformer.CenterLeft;
                Vector2 vertical = startingTransformer.CenterBottom - startingTransformer.CenterTop;

                Vector2 returnPoint = canvasPoint;
                Vector2 returnDiagonalPoint = center;
                Vector2 returnHorizontalPoint = Transformer.IntersectionPoint(canvasPoint, (canvasPoint - horizontal), (center + vertical), center);
                Vector2 returnVerticalPoint = Transformer.IntersectionPoint(canvasPoint, (canvasPoint - vertical), (center + horizontal), center);

                return _func(returnPoint, returnDiagonalPoint, returnHorizontalPoint, returnVerticalPoint);
            }
        }


        private static Transformer ScaleLeftTop(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.LeftTop;
            Vector2 lineDiagonalPoint = startingTransformer.RightBottom;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleLeftTop);
        }
        static Transformer _funcScaleLeftTop(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
        {
            return new Transformer
            {
                LeftTop = returnPoint,
                RightTop = returnHorizontalPoint,
                RightBottom = returnDiagonalPoint,
                LeftBottom = returnVerticalPoint,
            };
        }

        private static Transformer ScaleRightTop(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.RightTop;
            Vector2 lineDiagonalPoint = startingTransformer.LeftBottom;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleRightTop);
        }
        static Transformer _funcScaleRightTop(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
        {
            return new Transformer
            {
                LeftTop = returnHorizontalPoint,
                RightTop = returnPoint,
                RightBottom = returnVerticalPoint,
                LeftBottom = returnDiagonalPoint,
            };
        }

        private static Transformer ScaleRightBottom(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.RightBottom;
            Vector2 lineDiagonalPoint = startingTransformer.LeftTop;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleRightBottom);
        }
        static Transformer _funcScaleRightBottom(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
        {
            return new Transformer
            {
                LeftTop = returnDiagonalPoint,
                RightTop = returnVerticalPoint,
                RightBottom = returnPoint,
                LeftBottom = returnHorizontalPoint,
            };
        }

        private static Transformer ScaleLeftBottom(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.LeftBottom;
            Vector2 lineDiagonalPoint = startingTransformer.RightTop;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleLeftBottom);
        }
        static Transformer _funcScaleLeftBottom(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
        {
            return new Transformer
            {
                LeftTop = returnVerticalPoint,
                RightTop = returnDiagonalPoint,
                RightBottom = returnHorizontalPoint,
                LeftBottom = returnPoint,
            };
        }


    }
}