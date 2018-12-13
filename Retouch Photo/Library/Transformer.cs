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
            float widthScale = this.ControlWidth / this.Width / 8.0f * 7.0f;
            float heightScale = this.ControlHeight / this.Height / 8.0f * 7.0f;

            this.Scale = Math.Min(widthScale, heightScale);

            this.Position.X = this.ControlWidth / 2.0f;
            this.Position.Y = this.ControlHeight / 2.0f;

            this.Radian = 0.0f;
        }


        /// <summary>画布宽度</summary>
        public int Width = 1000;
        /// <summary>画布高度</summary>
        public int Height = 1000;

        /// <summary>缩放</summary>
        public float Scale = 1.0f;


        /// <summary>控件宽度</summary>
        public float ControlWidth = 1000.0f;
        /// <summary>控件高度</summary>
        public float ControlHeight = 1000.0f;

        /// <summary>位置</summary>
        public Vector2 Position;
        /// <summary>旋转</summary>
        public float Radian = 0.0f;


        /// <summary>控件尺寸改变</summary>
        public void ControlSizeChanged(Size size)
        {
            this.ControlWidth = size.Width < 100 ? 100.0f : (float)size.Width;
            this.ControlHeight = size.Height < 100 ? 100.0f : (float)size.Height;

            //   this.CanvasHalfSize.X = this.ControlWidth / 2;
            //   this.CanvasHalfSize.Y = this.ControlHeight / 2;
        }


        /// <summary>
        /// Canvas > Virtual > Control 
        /// </summary>      
        public Matrix3x2 CanvasToVirtualToControlMatrix => this.CanvasToVirtualMatrix * this.VirtualToControlMatrix;
        public Matrix3x2 CanvasToVirtualMatrix => Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) * Matrix3x2.CreateScale(this.Scale);
        public Matrix3x2 VirtualToControlMatrix => Matrix3x2.CreateRotation(this.Radian) * Matrix3x2.CreateTranslation(this.Position);


        /// <summary>
        /// Control > Virtual > Canvas 
        /// </summary>      
        public Matrix3x2 ControlToVirtualToCanvasMatrix => this.ControlToVirtualMatrix * this.VirtualToCanvasMatrix;
        public Matrix3x2 ControlToVirtualMatrix => Matrix3x2.CreateTranslation(-this.Position) * Matrix3x2.CreateRotation(-this.Radian);
        public Matrix3x2 VirtualToCanvasMatrix => Matrix3x2.CreateScale(1 / this.Scale) * Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);


    }
}
