using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels
{
    public abstract class ToolCursorSkewViewModel : ToolViewModel2
    {
        //@Override
        public abstract Vector2 GetLineA(Layer layer, Matrix3x2 matrix);
        public abstract Vector2 GetLineB(Layer layer, Matrix3x2 matrix);
        public abstract void SetRadian(Layer layer, Transformer startTransformer, float skew);


        Vector2 Center;
        Transformer StartTransformer;

        Vector2 LineA;
        Vector2 LineB;

        public override void Start(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            Matrix3x2 matrix = layer.Transformer.Matrix * viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix;

            this.Center = layer.Transformer.TransformCenter(matrix);
            this.StartTransformer.Radian = layer.Transformer.Radian;
            this.StartTransformer.Skew = layer.Transformer.Skew;
            this.StartTransformer.XScale = layer.Transformer.XScale;
            this.StartTransformer.YScale = layer.Transformer.YScale;

            this.LineA = this.GetLineA(layer, matrix);
            this.LineB = this.GetLineB(layer, matrix);

        }
        public override void Delta(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            Vector2 footPoint = Transformer.FootPoint(point, this.LineA, this.LineB);

            float radians = Transformer.VectorToRadians(footPoint - this.Center);

            this.SetRadian(layer, this.StartTransformer, radians);
        }
        public override void Complete(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            //viewModel.KeyCtrl = false;
        }

        public override void Draw(CanvasDrawingSession ds, Layer layer, DrawViewModel viewModel)
        {
            Transformer.DrawBoundNodesWithSkew(ds, layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }

    }
}
