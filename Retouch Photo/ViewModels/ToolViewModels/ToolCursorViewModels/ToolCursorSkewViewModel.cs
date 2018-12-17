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
        public abstract float GetStartRadian(Layer layer);
        public abstract void SetRadian(Layer layer, float value);


        Vector2 Center;

        Vector2 LineA;
        Vector2 LineB;

        float StartRadian;

        public override void Start(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            Matrix3x2 matrix = layer.Transformer.Matrix * viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix;

            this.Center = layer.Transformer.TransformCenter(matrix);

            this.LineA = this.GetLineA(layer, matrix);
            this.LineB = this.GetLineB(layer, matrix);

            this.StartRadian = this.GetStartRadian(layer);
        }
        public override void Delta(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            Vector2 footPoint = Transformer.FootPoint(point, this.LineA, this.LineB);

            float radians = Transformer.VectorToRadians(footPoint - this.Center);

            this.SetRadian(layer, layer.Transformer.Radian - radians + Transformer.PiHalf);
        }
        public override void Complete(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            viewModel.KeyCtrl = false;
        }

        public override void Draw(CanvasDrawingSession ds, Layer layer, DrawViewModel viewModel)
        {
            Transformer.DrawBoundNodesWithSkew(ds, layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }

    }
}
