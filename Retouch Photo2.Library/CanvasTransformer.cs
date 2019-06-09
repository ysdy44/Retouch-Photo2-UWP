using System;
using System.Numerics;
using Windows.Foundation;

namespace Retouch_Photo2.Library
{
    /// <summary> <see cref = "CanvasTransformer" />'s matrix mode. </summary>
    public enum MatrixTransformerMode
    {
        /// <summary> Canvas > Virtual > Control . </summary>
        CanvasToVirtualToControl,
        /// <summary> Canvas > Virtual. </summary>
        CanvasToVirtual,
        /// <summary> Virtual > Control . </summary>
        VirtualToControl,
    }

    /// <summary> <see cref = "CanvasTransformer" />'s inverse matrix mode. </summary>
    public enum InverseMatrixTransformerMode
    {
        /// <summary> Control > Virtual > Canvas . </summary>
        ControlToVirtualToCanvas,
        /// <summary> Control > Virtual. </summary>
        ControlToVirtual,
        /// <summary> Virtual > Canvas . </summary>
        VirtualToCanvas,
    }
    
    /// <summary>
    /// Transformer: 
    /// Provide matrix for RenderLayer by Position、Scale、Radians.
    /// 
    /// 
    /// TODO：
    /// Canvas Layer：
    ///    The original size of the layer.
    /// Virtual Layer： 
    ///    Render all layers together and make their center points coincide with the origin (0,0) and then zoom;
    /// Control Layer： 
    ///    Rotate around the origin first, then shift. (The canvas has a rotation angle)
    ///    
    /// </summary>
    public class CanvasTransformer
    {

        /// <summary> <see cref = "CanvasTransformer" />'s width. </summary>
        public int Width = 1000;
        /// <summary> <see cref = "CanvasTransformer" />'s height. </summary>
        public int Height = 1000;

        /// <summary> <see cref = "CanvasTransformer" />'s scale. </summary>
        public float Scale = 1.0f;

        /// <summary> CanvasControl's width. </summary>
        public float ControlWidth = 1000.0f;
        /// <summary> CanvasControl's height. </summary>
        public float ControlHeight = 1000.0f;
        /// <summary> CanvasControl's center. </summary>
        public Vector2 ControlCenter = new Vector2(500.0f, 500.0f);

        /// /// <summary> <see cref = "CanvasTransformer" />'s translation. </summary>
        public Vector2 Position = new Vector2(0.0f, 0.0f);
        /// <summary> <see cref = "CanvasTransformer" />'s rotation. </summary>
        public float Radian = 0.0f;


        //@Construct
        public CanvasTransformer()
        {
            this.ReloadMatrix();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary> <see cref = "ControlWidth" /> and <see cref = "ControlHeight" />'s setter. </summary>
        public Size Size
        {
            get => this.size;
            set
            {
                this.ControlWidth = size.Width < 100 ? 100.0f : (float)size.Width;
                this.ControlHeight = size.Height < 100 ? 100.0f : (float)size.Height;
                this.ControlCenter = new Vector2(this.ControlWidth / 2, this.ControlHeight / 2);
                this.size = value;
            }
        }
        private Size size;
               

        /// <summary> Fit to the screen. </summary>
        public void Fit()
        {
            float widthScale = this.ControlWidth / this.Width / 8.0f * 7.0f;
            float heightScale = this.ControlHeight / this.Height / 8.0f * 7.0f;

            this.Scale = Math.Min(widthScale, heightScale);

            this.Position.X = this.ControlWidth / 2.0f;
            this.Position.Y = this.ControlHeight / 2.0f;

            this.Radian = 0.0f;

            this.ReloadMatrix();
        }

        /// <summary>
        /// Fit to the screen with scale.
        /// </summary>
        /// <param name="scale"> scale </param>
        public void FitWithScale(float scale)
        {
            this.Scale = scale;

            this.Position.X = this.ControlWidth / 2.0f;
            this.Position.Y = this.ControlHeight / 2.0f;

            this.Radian = 0.0f;

            this.ReloadMatrix();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //Matrix
        Matrix3x2 Matrix; 
        Matrix3x2 CanvasToVirtualMatrix; 
        Matrix3x2 VirtualToControlMatrix;

        //InverseMatrix
        Matrix3x2 InverseMatrix;
        Matrix3x2 ControlToVirtualInverseMatrix; 
        Matrix3x2 VirtualToCanvasInverseMatrix; 

        
        /// <summary>
        /// Gets <see cref = "CanvasTransformer" />'s matrix.
        /// </summary>
        /// <param name="mode"> <see cref = "CanvasTransformer" />'s matrix mode. </param>
        /// <returns> matrix </returns>
        public Matrix3x2 GetMatrix(MatrixTransformerMode mode = MatrixTransformerMode.CanvasToVirtualToControl)
        {
            switch (mode)
            {
                case MatrixTransformerMode.CanvasToVirtualToControl:
                    return this.Matrix;
                case MatrixTransformerMode.CanvasToVirtual:
                    return this.CanvasToVirtualMatrix;
                case MatrixTransformerMode.VirtualToControl:
                    return this.VirtualToControlMatrix;
            }
            return this.Matrix;
        }

        /// <summary>
        /// Gets <see cref = "CanvasTransformer" />'s inverse matrix.
        /// </summary>
        /// <param name="mode"> <see cref = "CanvasTransformer" />'s inverse matrix mode. </param>
        /// <returns> inverse matrix </returns>
        public Matrix3x2 GetInverseMatrix(InverseMatrixTransformerMode mode = InverseMatrixTransformerMode.ControlToVirtualToCanvas)
        {
            switch (mode)
            {
                case InverseMatrixTransformerMode.ControlToVirtualToCanvas:
                    return this.InverseMatrix;
                case InverseMatrixTransformerMode.ControlToVirtual:
                    return this.ControlToVirtualInverseMatrix;
                case InverseMatrixTransformerMode.VirtualToCanvas:
                    return this.VirtualToCanvasInverseMatrix;
            }
            return this.InverseMatrix;
        }


        /// <summary>
        /// Reload <see cref = "CanvasTransformer" />'s all matrix.
        /// </summary>
        public void ReloadMatrix()
        {
            //Matrix
            this.VirtualToControlMatrix = Matrix3x2.CreateRotation(this.Radian) * Matrix3x2.CreateTranslation(this.Position);
            this.CanvasToVirtualMatrix = Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) * Matrix3x2.CreateScale(this.Scale);
            this.Matrix = this.CanvasToVirtualMatrix * this.VirtualToControlMatrix;

            //InverseMatrix
            this.VirtualToCanvasInverseMatrix = Matrix3x2.CreateScale(1 / this.Scale) * Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);
            this.ControlToVirtualInverseMatrix = Matrix3x2.CreateTranslation(-this.Position) * Matrix3x2.CreateRotation(-this.Radian);
            this.InverseMatrix = this.ControlToVirtualInverseMatrix * this.VirtualToCanvasInverseMatrix;
        }

    }
}