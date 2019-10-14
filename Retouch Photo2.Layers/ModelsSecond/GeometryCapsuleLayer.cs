using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryCapsuleLayer .
    /// </summary>
    public class GeometryCapsuleLayer : IGeometryLayer
    {
        //@Construct
        public GeometryCapsuleLayer()
        {
            base.Control.Icon = new GeometryCapsuleIcon();
            base.Control.Text = "Capsule";
        }

        //@Override       
        public override string Type => "Capsule";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.GetActualDestinationWithRefactoringTransformer;
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCapsuleLayer CapsuleLayer = new GeometryCapsuleLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, CapsuleLayer, this);
            return CapsuleLayer;
        }
    }
}