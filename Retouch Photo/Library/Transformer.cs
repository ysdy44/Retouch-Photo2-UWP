using Retouch_Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Retouch_Photo.Library
{
    public class Transformer
    {

        /// <summary>重新加载Transformer，可以多次调用</summary>
        /// <param name="project">Project类型</param>
        public void LoadFromProject(Project project)
        {
            this.Width = project.Width;
            this.Height = project.Height;

            this.Fit();
        }
        public void Fit()
        {
            float widthScale = this.CanvasWidth / this.Width / 8.0f * 7.0f;
            float heightScale = this.CanvasHeight / this.Height / 8.0f * 7.0f;

            this.Scale = Math.Min(widthScale, heightScale);

            this.Position.X = this.CanvasWidth / 2.0f;
            this.Position.Y = this.CanvasHeight / 2.0f;

            this.Radian = 0.0f;
        }


        /// <summary>宽度</summary>
        public int Width = 1000;
        /// <summary>高度</summary>
        public int Height = 1000;

        /// <summary>缩放</summary>
        public float Scale = 1.0f;
        /// <summary>位置</summary>
        public Vector2 Position;

        /// <summary>旋转</summary>
        public float Radian = 0.0f;



        #region Matrix


        /// <summary>变换矩阵</summary>
        public Matrix3x2 Matrix =>this.GetMatrix();
     
        /// <summary>变换向量</summary>
        public Vector2 Transform(Vector2 point) => Vector2.Transform(point, this.GetMatrix());

        /// <summary>变换矩形</summary>
        public VectorRect TransformRect(VectorRect rect) => new VectorRect(Vector2.Transform(rect.End, this.Matrix), Vector2.Transform(rect.Start, this.Matrix));
        
        private Matrix3x2 GetMatrix() =>
            Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
            Matrix3x2.CreateRotation(this.Radian) *
            Matrix3x2.CreateScale(this.Scale) *
            Matrix3x2.CreateTranslation(this.Position);


        #endregion


        #region InversionMatrix


        /// <summary>反向变换矩阵</summary>
        public Matrix3x2 InversionMatrix => this.GetInversionMatrix();
    
        /// <summary>反向变换向量</summary>
        public Vector2 InversionTransform(Vector2 point) => Vector2.Transform(point, this.GetInversionMatrix());

        /// <summary>反向变换矩形</summary>
        public VectorRect InversionTransformRect(VectorRect rect) => new VectorRect(Vector2.Transform(rect.End, this.InversionMatrix), Vector2.Transform(rect.Start, this.InversionMatrix));

        private Matrix3x2 GetInversionMatrix() =>
            Matrix3x2.CreateTranslation(-this.Position) *
            Matrix3x2.CreateRotation(-this.Radian) *
            Matrix3x2.CreateScale(1 / this.Scale) *
            Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);


        #endregion


        #region CanvasSize


        /// <summary>画布宽度</summary>
        public float CanvasWidth = 1000.0f;
      
        /// <summary>画布高度</summary>
        public float CanvasHeight = 1000.0f;

        /// <summary>画布改变</summary>
        public void CanvasSizeChanged(Size size)
        {
            this.CanvasWidth = size.Width < 100 ? 100.0f : (float)size.Width;
            this.CanvasHeight = size.Height < 100 ? 100.0f : (float)size.Height;
        }


        #endregion

    }
}
