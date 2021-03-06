﻿// Core:              ★★★★
// Referenced:   ★
// Difficult:         ★★★
// Only:              ★★★
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="GeometryLayer"/>'s GeometryRectangleLayer .
    /// </summary>
    public class GeometryRectangleLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryRectangle;


        public override ILayer Clone() => LayerBase.CopyWith(this, new GeometryRectangleLayer());


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator, matrix);
        }

    }
}