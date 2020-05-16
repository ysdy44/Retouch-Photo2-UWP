using System;
using System.Collections.Generic;
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Retouch_Photo2.Brushs
{
    public partial class BrushHandle : ICacheTransform
    {

        public Vector2 Center { get; set; }
        Vector2 _startingCenter;
        public Vector2 XPoint { get; set; }
        Vector2 _startingXPoint;
        public Vector2 YPoint { get; set; }
        Vector2 _startingYPoint;



        public BrushHandle(Transformer transformer)
        {
            Vector2 center = transformer.Center;
            Vector2 xPoint = transformer.CenterRight;
            Vector2 yPoint = transformer.CenterBottom;

            this.Center = center;
            this.XPoint = xPoint;
            this.YPoint = yPoint;
        }


        public void Move(Vector2 point)
        {
            Vector2 center = point;
            Vector2 xPoint = center + this._startingXPoint - this._startingCenter;
            Vector2 yPoint = center + this._startingYPoint - this._startingCenter;

            this.Center = center;
            this.XPoint = xPoint;
            this.YPoint = yPoint;
        }



        public void XToY(Vector2 point)
        {
            Vector2 xPoint = point;

            Vector2 normalize = Vector2.Normalize(xPoint - this._startingCenter);
            float radiusY = Vector2.Distance(this._startingYPoint, this._startingCenter);
            Vector2 reflect = new Vector2(-normalize.Y, normalize.X);
            Vector2 yPoint = radiusY * reflect + this._startingCenter;

            this.XPoint = xPoint;
            this.YPoint = yPoint;
        }
        public void YToX(Vector2 point)
        {
            Vector2 yPoint = point;

            Vector2 normalize = Vector2.Normalize(yPoint - this._startingCenter);
            float radiusX = Vector2.Distance(this._startingXPoint, this._startingCenter);
            Vector2 reflect = new Vector2(normalize.Y, -normalize.X);
            Vector2 xPoint = radiusX * reflect + this._startingCenter;

            this.XPoint = xPoint;
            this.YPoint = yPoint;
        }





        public void CacheTransform()
        {
            this._startingCenter = this.Center;
            this._startingXPoint = this.XPoint;
            this._startingYPoint = this.YPoint;
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Center = Vector2.Transform(this._startingCenter, matrix);
            this.XPoint = Vector2.Transform(this._startingXPoint, matrix);
            this.YPoint = Vector2.Transform(this._startingYPoint, matrix);
        }
        public void TransformAdd(Vector2 vector)
        {
            this.Center = Vector2.Add(this._startingCenter, vector);
            this.XPoint = Vector2.Add(this._startingXPoint, vector);
            this.YPoint = Vector2.Add(this._startingYPoint, vector);
        }

    }
}
