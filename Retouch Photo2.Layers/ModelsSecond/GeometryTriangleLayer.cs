﻿// Core:              ★★★★
// Referenced:   ★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="GeometryLayer"/>'s GeometryTriangleLayer .
    /// </summary>
    public class GeometryTriangleLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryTriangle;

        //@Content
        public float Center = 0.5f;
        public float StartingCenter { get; private set; }
        public void CacheCenter() => this.StartingCenter = this.Center;


        public override ILayer Clone()
        {
            GeometryTriangleLayer triangleLayer = new GeometryTriangleLayer
            {
                Center = this.Center,
            };

            LayerBase.CopyWith(triangleLayer, this);
            return triangleLayer;
        }
        
        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Center", this.Center));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Center") is XElement center) this.Center = (float)center;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateTriangle(resourceCreator, transformer, this.Center);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateTriangle(resourceCreator, transformer, matrix, this.Center);
        }
        

        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("Layers_GeometryTriangle");
        }

    }
}