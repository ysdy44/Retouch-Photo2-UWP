using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers.GeometryLayers;
using Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels
{
    public class ToolCursorRotationViewModel : ToolViewModel2
    {
        Vector2 Center;

        float LayerStartRadian;
        float StartRadian;

        float Radians;

        public override void Start(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            this.Center = Vector2.Transform(layer.Transformer.Center, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
            this.LayerStartRadian = layer.Transformer.Radian;
            this.StartRadian = Transformer.VectorToRadians(point - this.Center);
        }
        public override void Delta(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            this.Radians = Transformer.VectorToRadians(point - this.Center);
            float radian = this.LayerStartRadian - this.StartRadian + this.Radians;
            layer.Transformer.Radian = viewModel.KeyShift ? Transformer.RadiansStepFrequency(radian) : radian;
        }
        public override void Complete(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            viewModel.KeyShift = false;
        }

        public override void Draw(CanvasDrawingSession ds, Layer layer, DrawViewModel viewModel)
        {
            Transformer.DrawBoundNodesWithRotation(ds, layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);

            Transformer.DrawLine(ds, this.Center, Transformer.RadiansToVector(this.StartRadian, this.Center));
            Transformer.DrawLine(ds, this.Center, Transformer.RadiansToVector(this.Radians, this.Center));
        }

    }

}
