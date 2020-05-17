using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    public partial class BrushBase : IBrush
    {


        public BrushHandleMode ContainsOperateMode(Vector2 point, Matrix3x2 matrix)
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
                                    Vector2 yPoint = center + this._startingYPoint - this._startingCenter;
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
                                    this.XPoint = offset + this._startingXPoint;
                                    this.YPoint = offset + this._startingYPoint;
                                }
                                return;
                            case BrushHandleMode.XPoint:
                                {
                                    float radiusY = Vector2.Distance(this._startingYPoint, this._startingCenter);
                                    this.XPoint = point;
                                    this.YPoint = BrushBase.XToY(point, this._startingCenter, radiusY);
                                }
                                return;
                            case BrushHandleMode.YPoint:
                                {
                                    float radiusX = Vector2.Distance(this._startingXPoint, this._startingCenter);
                                    this.XPoint = BrushBase.YToX(point, this._startingCenter, radiusX);
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