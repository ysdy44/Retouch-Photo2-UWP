using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a brush that can have fill properties. Provides a filling method.
    /// </summary>
    public partial class BrushBase : IBrush
    {


        /// <summary>
        /// Gets the all points by the brush contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The operate-mode. </returns>
        public BrushHandleMode ContainsHandleMode(Vector2 point, Matrix3x2 matrix)
        {
            switch (this.Type)
            {
                case BrushType.None: return BrushHandleMode.None;
                case BrushType.Color: return BrushHandleMode.None;
            }


            Vector2 center = Vector2.Transform(this.Center, matrix);
            bool isCenter = FanKit.Math.InNodeRadius(point, center);

            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);
            bool isYPoint = FanKit.Math.InNodeRadius(point, yPoint);

            switch (this.Type)
            {
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                    {
                        if (isCenter) return BrushHandleMode.Center;
                        if (isYPoint) return BrushHandleMode.YPoint;
                        return BrushHandleMode.None;
                    }
            }


            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            bool isXPoint = FanKit.Math.InNodeRadius(point, xPoint);

            switch (this.Type)
            {
                case BrushType.EllipticalGradient:
                case BrushType.Image:
                    {
                        if (isCenter) return BrushHandleMode.Center;
                        if (isXPoint) return BrushHandleMode.XPoint;
                        if (isYPoint) return BrushHandleMode.YPoint;
                        return BrushHandleMode.None;
                    }

                default:
                    return BrushHandleMode.None;
            }
        }


        /// <summary>
        /// It controls the transformation of brush.
        /// </summary>
        /// <param name="mode"> The mode. </param>
        /// <param name="startingPoint"> The starting point. </param>
        /// <param name="point"> The point. </param>
        public void Controller(BrushHandleMode mode, Vector2 startingPoint, Vector2 point)
        {
            switch (this.Type)
            {
                case BrushType.None: break;
                case BrushType.Color: break;
                case BrushType.LinearGradient:
                    {
                        switch (mode)
                        {
                            case BrushHandleMode.Center:
                                this.Center = point;
                                return;

                            case BrushHandleMode.YPoint:
                                this.YPoint = point;
                                return;
                        }
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        switch (mode)
                        {
                            case BrushHandleMode.Center:
                                {
                                    Vector2 center = point;
                                    Vector2 yPoint = center + this.StartingYPoint - this.StartingCenter;
                                    this.Center = center;
                                    this.YPoint = yPoint;
                                }
                                return;

                            case BrushHandleMode.YPoint:
                                this.YPoint = point;
                                return;
                        }
                    }
                    break;
                case BrushType.EllipticalGradient:
                case BrushType.Image:
                    {
                        switch (mode)
                        {
                            case BrushHandleMode.Center:
                                {
                                    Vector2 center = point;

                                    Vector2 offset = point - startingPoint;
                                    this.Center = center;
                                    this.XPoint = offset + this.StartingXPoint;
                                    this.YPoint = offset + this.StartingYPoint;
                                }
                                return;
                            case BrushHandleMode.XPoint:
                                {
                                    float radiusY = Vector2.Distance(this.StartingYPoint, this.StartingCenter);
                                    this.XPoint = point;
                                    this.YPoint = BrushBase.XToY(point, this.StartingCenter, radiusY);
                                }
                                return;
                            case BrushHandleMode.YPoint:
                                {
                                    float radiusX = Vector2.Distance(this.StartingXPoint, this.StartingCenter);
                                    this.XPoint = BrushBase.YToX(point, this.StartingCenter, radiusX);
                                    this.YPoint = point;
                                }
                                return;
                        }
                    }
                    return;
                default:
                    return;
            }
        }


        /// <summary>
        /// It initialize and controls the transformation of brush.
        /// </summary>
        /// <param name="startingPoint"> The starting point. </param>
        /// <param name="point"> The point. </param>
        public void InitializeController(Vector2 startingPoint, Vector2 point)
        {
            switch (this.Type)
            {
                case BrushType.None: break;
                case BrushType.Color: break;
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                    {
                        this.Center = startingPoint;
                        this.YPoint = point;
                    }
                    break;
                case BrushType.EllipticalGradient:
                case BrushType.Image:
                    {
                        this.Center = startingPoint;
                        this.XPoint = BrushBase.YToX(point, startingPoint);
                        this.YPoint = point;
                    }
                    break;
                default:
                    break;
            }
        }


    }
}