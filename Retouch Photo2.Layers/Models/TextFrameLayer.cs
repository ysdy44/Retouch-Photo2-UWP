﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
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
                Text = this.ConstructStrings(),
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
                FontSize = this.FontSize,
                FontFamily = this.FontFamily,

                HorizontalAlignment = this.HorizontalAlignment,
                FontStyle = this.FontStyle,
                FontWeight = this.FontWeight,
            };

            LayerBase.CopyWith(resourceCreator, textFrameLayer, this);
            return textFrameLayer;
        }

        public void SaveWith(XElement element)
        {
            element.Add(new XElement("Text", this.Text));
            element.Add(new XElement("FontSize", this.FontSize));
            element.Add(new XElement("FontFamily", this.FontFamily));

            element.Add(new XElement("HorizontalAlignment", this.HorizontalAlignment));
            element.Add(new XElement("FontStyle", this.FontStyle));
            element.Add(new XElement("FontWeight", this.FontWeight.Weight));
        }
        public void Load(XElement element)
        {
            if (element.Element("Text") is XElement text) this.Text = text.Value;
            if (element.Element("FontSize") is XElement fontSize) this.FontSize = (float)fontSize;
            if (element.Element("FontFamily") is XElement fontFamily) this.FontFamily = fontFamily.Value;

            if (element.Element("HorizontalAlignment") is XElement horizontalAlignment)
            {
                switch (horizontalAlignment.Value)
                {
                    case "Left": this.HorizontalAlignment = CanvasHorizontalAlignment.Left; break;
                    case "Right": this.HorizontalAlignment = CanvasHorizontalAlignment.Right; break;
                    case "Center": this.HorizontalAlignment = CanvasHorizontalAlignment.Center; break;
                    default: this.HorizontalAlignment = CanvasHorizontalAlignment.Justified; break;
                }
            }
            if (element.Element("FontStyle") is XElement fontStyle)
            {
                switch (fontStyle.Value)
                {
                    case "Normal": this.FontStyle = FontStyle.Normal; break;
                    case "Oblique": this.FontStyle = FontStyle.Oblique; break;
                    default: this.FontStyle = FontStyle.Italic; break;
                }
            }
            if (element.Element("FontWeight") is XElement fontWeight)
            {
                this.FontWeight = new FontWeight
                {
                    Weight = (ushort)(int)fontWeight
                };
            }
        }

        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/TextFrame");
        }

    }
}