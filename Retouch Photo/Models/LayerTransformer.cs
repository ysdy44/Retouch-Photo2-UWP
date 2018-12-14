using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Retouch_Photo.Models
{
    public struct LayerTransformer
    {
        public VectorRect Rect;
        public float Radian;

        public Matrix3x2 Matrix =>
                Matrix3x2.CreateTranslation(-this.Rect.Width / 2, -this.Rect.Height / 2) *
                Matrix3x2.CreateRotation(this.Radian) * 
                Matrix3x2.CreateTranslation(this.Rect.Width / 2, this.Rect.Height / 2) *
               Matrix3x2.CreateTranslation(this.Rect.X,this.Rect.Y);



         

        


    }
}
