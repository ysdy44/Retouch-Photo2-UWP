using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryCogLayer .
    /// </summary>
    public class GeometryCogLayer : IGeometryLayer
    {
        //@Construct
        public GeometryCogLayer()
        {
            base.Control.Icon = new GeometryCogIcon();
            base.Control.Text = "Cog";
        }

        //@Override       
        public override string Type => "Cog";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.GetActualDestinationWithRefactoringTransformer;
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCogLayer CogLayer = new GeometryCogLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, CogLayer, this);
            return CogLayer;
        }
    }
}