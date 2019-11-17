using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s TextArtisticLayer .
    /// </summary>
    public class TextArtisticLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.TextArtistic;

        //@Content       
        public string Text = string.Empty;

        //@Construct
        /// <summary>
        /// Construct a TextArtistic-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public TextArtisticLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a TextArtistic-layer.
        /// </summary>
        public TextArtisticLayer()
        {
            base.StyleManager.FillBrush.Color = Color.FromArgb(255, 0, 0, 0);
            base.Control = new LayerControl(this)
            {
                Icon = new TextArtisticIcon(),
                Text = "Artistic Text",
            };
        }


        public ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                CanvasTextLayout textLayout = new CanvasTextLayout(resourceCreator, this.Text, new CanvasTextFormat(), 0, 0);
                CanvasGeometry geometry = CanvasGeometry.CreateText(textLayout);

                //Fill
                this.StyleManager.FillGeometry(resourceCreator, drawingSession, geometry, canvasToVirtualMatrix);
                //Stroke
                this.StyleManager.DrawGeometry(resourceCreator, drawingSession, geometry, canvasToVirtualMatrix);
            }
            return command;
        }


        public IEnumerable<IEnumerable<Node>> ConvertToCurves() => null;

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            TextArtisticLayer textArtisticLayer = new TextArtisticLayer
            {
                Text = this.Text,
            };

            LayerBase.CopyWith(resourceCreator, textArtisticLayer, this);
            return textArtisticLayer;
        }

        public void SaveWith(XElement element)
        {
            element.Add(new XElement("Text", this.Text));
        }
        public void Load(XElement element)
        {
            this.Text = element.Element("Text").Value;
        }

    }
}