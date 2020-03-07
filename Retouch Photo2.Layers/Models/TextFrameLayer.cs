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
using Windows.UI.Text;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s TextFrameLayer .
    /// </summary>
    public class TextFrameLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.TextFrame;

        //@Content       
        public string Text = string.Empty;
        public float FontSize = 22;
        public string FontFamily = "Arial";

        public CanvasHorizontalAlignment HorizontalAlignment = CanvasHorizontalAlignment.Left;
        public FontStyle FontStyle = FontStyle.Normal;
        public FontWeight FontWeight = new FontWeight
        {
            Weight = 100,
        };

        //@Construct
        /// <summary>
        /// Construct a TextFrame-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public TextFrameLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a TextFrame-layer.
        /// </summary>
        public TextFrameLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new TextFrameIcon(),
                Text = "Frame Text",
            };
        }


        public ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                float scale = (canvasToVirtualMatrix.M11 + canvasToVirtualMatrix.M22) / 2;
                CanvasTextFormat textFormat = new CanvasTextFormat
                {
                    FontSize = this.FontSize * scale,
                    FontFamily = this.FontFamily,

                    HorizontalAlignment = this.HorizontalAlignment,
                    FontStyle= this.FontStyle,
                    FontWeight =this.FontWeight,
                };

                float width = transformer.Horizontal.Length() * scale;
                float height = transformer.Vertical.Length() * scale;
                TransformerRect rect = new TransformerRect(width, height, Vector2.Zero);
                Matrix3x2 matrix = Transformer.FindHomography(rect, transformer) * canvasToVirtualMatrix;

                CanvasTextLayout textLayout = new CanvasTextLayout(resourceCreator, this.Text, textFormat, width, height);
                CanvasGeometry geometry = CanvasGeometry.CreateText(textLayout).Transform(matrix);

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
            TextFrameLayer textFrameLayer = new TextFrameLayer
            {
                Text = this.Text,
            };

            LayerBase.CopyWith(resourceCreator, textFrameLayer, this);
            return textFrameLayer;
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