using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorSkewViewModels
{
    public class ToolCursorSkewTopViewModel : ToolViewModel2
    {
        Vector2 Center;
        Vector2 LeftTop;
        Vector2 RightTop;

        float LayerStartRadian;

        public override void Start(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            Matrix3x2 matrix = layer.Transformer.Matrix * viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix;
            this.Center = Vector2.Transform(layer.Transformer.Center, matrix);
            this.LeftTop = matrix.Translation;
            this.RightTop = Vector2.Transform(new Vector2(layer.Transformer.Width, 0), matrix);

            this.LayerStartRadian = layer.Transformer.RadianX;
        }
        public override void Delta(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            Vector2 foot = Transformer.FootPoint(point, this.LeftTop, this.RightTop);
            float radiansX = Transformer.VectorToRadians(foot - this.Center);
            layer.Transformer.RadianX = Transformer.PiHalf - radiansX + layer.Transformer.Radian;
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
