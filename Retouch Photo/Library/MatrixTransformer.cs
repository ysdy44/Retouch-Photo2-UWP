using Retouch_Photo.Models;
using System;
using System.Numerics;
using Windows.Foundation;

namespace Retouch_Photo.Library
{
    /// <summary>
    /// Transformer: 
    /// Provide matrix for RenderLayer by Position、Scale、Radians.
    /// 
    /// ——————————
    /// 
    /// TODO：
    /// Canvas画布层：
    ///    包含了每个图层的数据源，比如图片图层的Bitmap；
    /// Virtual虚拟层： 
    ///    所谓虚拟层，将所有图层都渲染到一起，并使其中心点重合于原点（0，0）再缩放；(画布无旋转角度)
    /// Control控件层： 
    ///    先原点旋转，再位移。(画布有旋转角度)
    ///    
    /// ——————————
    /// 
    /// Warning:
    /// 把数据源呈现到界面的控件上，需要经过【Canvas画布层 —Virtual虚拟层—Control控件层】的转化；
    /// 每个图层的GetRender方法里，需要返回自身数据源的【Canvas画布层 —Virtual虚拟层】的转化；
    /// 由CanvasControl画布控件的Draw绘制事件，来完成【Virtual虚拟层 —Control控件层】的转化。
    /// 
    /// ——————————
    /// 
    /// Using：
    /// 只有渲染时，需要先从Canvas画布层渲染到Virtual虚拟层，再渲染到Control控件层
    /// 正常情况下，直接使用Matrix，即从Canvas画布层到Control控件层
    /// 
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
        public void Fit(float scale)
        {
            this.Scale = scale;

            this.Position.X = this.ControlWidth / 2.0f;
            this.Position.Y = this.ControlHeight / 2.0f;

            this.Radian = 0.0f;
        }


        /// <summary>Width</summary>
        public int Width = 1000;
        /// <summary>Height</summary>
        public int Height = 1000;

        /// <summary>Size</summary>
        public float Scale = 1.0f;

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
        }


        /// <summary> CanvasToVirtualToControlMatrix: Canvas > Virtual > Control </summary>      
        public Matrix3x2 Matrix => this.CanvasToVirtualMatrix * this.VirtualToControlMatrix;
        /// <summary> CanvasToVirtualMatrix: Canvas > Virtual </summary>      
        public Matrix3x2 CanvasToVirtualMatrix => Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) * Matrix3x2.CreateScale(this.Scale);
        /// <summary> VirtualToControlMatrix: Virtual > Control </summary>      
        public Matrix3x2 VirtualToControlMatrix => Matrix3x2.CreateRotation(this.Radian) * Matrix3x2.CreateTranslation(this.Position);


        /// <summary> ControlToVirtualToCanvasMatrix: Control > Virtual > Canvas </summary>      
        public Matrix3x2 InverseMatrix => this.ControlToVirtualInverseMatrix * this.VirtualToCanvasInverseMatrix;
        /// <summary> ControlToVirtualMatrix: Control > Virtual </summary>      
        public Matrix3x2 ControlToVirtualInverseMatrix => Matrix3x2.CreateTranslation(-this.Position) * Matrix3x2.CreateRotation(-this.Radian);
        /// <summary> VirtualToCanvasMatrix: Virtual > Canvas </summary>      
        public Matrix3x2 VirtualToCanvasInverseMatrix => Matrix3x2.CreateScale(1 / this.Scale) * Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);

        /// <summary>
        /// Center of 
        ///    <see cref = "ControlWidth" />
        ///    <see cref = "ControlHeight" />
        ///    to Canvas.
        /// </summary>
        public Vector2 Center  => Vector2.Transform(new Vector2(this.ControlWidth, this.ControlHeight) / 2, this.InverseMatrix);

    }
}
