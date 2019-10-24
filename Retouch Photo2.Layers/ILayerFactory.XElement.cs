using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Retouch_Photo2.Layers.Models;

namespace Retouch_Photo2.Layers
{
    public static class ILayerFactory
    {
        
        /// <summary>
        ///  Create a ILayer from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded ILayer. </returns>
        public static ILayer CreateILayer(XElement element)
        {
            string type = (string)element.Element("LayerType");

            switch (type)
            {
                case GeometryRectangleLayer.ID: return new GeometryRectangleLayer(element);
                case GeometryEllipseLayer.ID: return new GeometryEllipseLayer(element);
                case GeometryCurveLayer.ID: return new GeometryCurveLayer(element);

                case ImageLayer.ID: return new ImageLayer(element);
                case AcrylicLayer.ID: return new AcrylicLayer(element);
                    

                case GeometryRoundRectLayer.ID: return new GeometryRoundRectLayer(element);
                case GeometryTriangleLayer.ID: return new GeometryTriangleLayer(element);

                case GeometryDiamondLayer.ID: return new GeometryDiamondLayer(element);
                case GeometryPentagonLayer.ID: return new GeometryPentagonLayer(element);
                case GeometryStarLayer.ID: return new GeometryStarLayer(element);
                case GeometryPieLayer.ID: return new GeometryPieLayer(element);

                case GeometryCogLayer.ID: return new GeometryCogLayer(element);
                case GeometryArrowLayer.ID: return new GeometryArrowLayer(element);
                case GeometryCapsuleLayer.ID: return new GeometryCapsuleLayer(element);
                case GeometryHeartLayer.ID: return new GeometryHeartLayer(element);

                default: return null;
            }
        }
        
    }
}