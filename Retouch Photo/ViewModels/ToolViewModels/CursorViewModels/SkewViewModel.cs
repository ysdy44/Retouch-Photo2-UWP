using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels
{
    public abstract class SkewViewModel : IToolViewModel
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;
        bool IsSkew{set => this.ViewModel.KeyAlt = value;}
        
        //@Override
        public abstract Vector2 GetLineA(Layer layer, Matrix3x2 matrix);
        public abstract Vector2 GetLineB(Layer layer, Matrix3x2 matrix);
        public abstract void SetRadian(Layer layer, Transformer startTransformer, float skew);
        
        Transformer StartTransformer;
        Vector2 Center;

        /// <summary> Point A (left point on the same side of the point) </summary>
        Vector2 LineA;
        /// <summary> Point b (right point on the same side of the point) </summary>
        Vector2 LineB;

        public void Start(Vector2 point, Layer layer)
        {
            Matrix3x2 matrix = layer.Transformer.Matrix * this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix;

            this.StartTransformer.CopyWith(layer.Transformer);

            this.LineA = this.GetLineA(layer, matrix);
            this.LineB = this.GetLineB(layer, matrix);

            this.Center = layer.Transformer.TransformCenter(matrix);
        }
        public void Delta(Vector2 point, Layer layer)
        {
            Vector2 footPoint = Transformer.FootPoint(point, this.LineA, this.LineB);

            float radians = Transformer.VectorToRadians(footPoint - this.Center);

            this.SetRadian(layer, this.StartTransformer, radians);
        }
        public void Complete(Vector2 point, Layer layer)
        {
            this.IsSkew = false;
        }

        public void Draw(CanvasDrawingSession ds, Layer layer)
        {
            Transformer.DrawBoundNodesWithSkew(ds, layer.Transformer, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }

    }
}
