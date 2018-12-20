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
    /// <summary>
    /// Transformer: 
    /// Provide matrix for RenderLayer by Position、Scale、Radians.
    /// ——————————
    /// TODO：
    /// Canvas画布层：包含了每个图层的数据源，比如图片图层的Bitmap；
    /// Virtual虚拟层： 所谓虚拟层，将所有图层都渲染到一起，并使其中心点重合于原点（0，0）再缩放；
    /// Control:控件层： 先原点旋转，再位移。
    /// ——————————
    /// Warning:
    /// 把数据源呈现到界面的控件上，需要经过【Canvas画布层 —Virtual虚拟层—Control控件层】的转化；
    /// 每个图层的GetRender方法里，需要返回自身数据源的【Canvas画布层 —Virtual虚拟层】的转化；
    /// 由CanvasControl画布控件的Draw绘制事件，来完成【Virtual虚拟层 —Control控件层】的转化。
    /// </summary>
    public class MatrixTransformer
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


        /// <summary>Width</summary>
        public int Width = 1000;
        /// <summary>Height</summary>
        public int Height = 1000;

        /// <summary>Size</summary>
        public float Scale=1.0f;

        /// <summary>Width of CanvasControl</summary>
        public float ControlWidth = 1000.0f;
        /// <summary>Height of CanvasControl</summary>
        public float ControlHeight = 1000.0f;

        /// <summary>Translation</summary>
        public Vector2 Position;
        /// <summary>Rotation</summary>
        public float Radian = 1.0f; 


        /// <summary>SizeChanged of CanvasControl</summary>
        public void ControlSizeChanged(Size size)
        {
            this.ControlWidth = size.Width < 100 ? 100.0f : (float)size.Width;
            this.ControlHeight = size.Height < 100 ? 100.0f : (float)size.Height;

            //   this.CanvasHalfSize.X = this.ControlWidth / 2;
            //   this.CanvasHalfSize.Y = this.ControlHeight / 2;
        }


        /// <summary>
        /// Matrix3x2: Canvas > Virtual > Control 
        /// </summary>      
        public Matrix3x2 CanvasToVirtualToControlMatrix => this.CanvasToVirtualMatrix * this.VirtualToControlMatrix;
        public Matrix3x2 CanvasToVirtualMatrix => Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) * Matrix3x2.CreateScale(this.Scale);
        public Matrix3x2 VirtualToControlMatrix => Matrix3x2.CreateRotation(this.Radian) * Matrix3x2.CreateTranslation(this.Position);


        /// <summary>
        /// Matrix3x2: Control > Virtual > Canvas 
        /// </summary>      
        public Matrix3x2 ControlToVirtualToCanvasMatrix => this.ControlToVirtualMatrix * this.VirtualToCanvasMatrix;
        public Matrix3x2 ControlToVirtualMatrix => Matrix3x2.CreateTranslation(-this.Position) * Matrix3x2.CreateRotation(-this.Radian);
        public Matrix3x2 VirtualToCanvasMatrix => Matrix3x2.CreateScale(1 / this.Scale) * Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);


         

    }
}
