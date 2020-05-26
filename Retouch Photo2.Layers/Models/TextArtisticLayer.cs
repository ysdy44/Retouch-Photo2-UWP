﻿using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="TextLayer"/>'s TextArtisticLayer .
    /// </summary>
    public class TextArtisticLayer : TextLayer, ITextLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.TextArtistic;

        public override float FontSize
        {
            get => this.GetFontSize();
            set => this.SetFontSize(value);
        }

        private float fontSize;
        private void SetFontSize(float value)
        {
            float scale = value / this.fontSize;

            Vector2 leftTop = this.Transform.Transformer.LeftTop;
            Matrix3x2 matrix =
                Matrix3x2.CreateTranslation(-leftTop)
                * Matrix3x2.CreateScale(scale)
                * Matrix3x2.CreateTranslation(leftTop);

            base.CacheTransform();
            base.TransformMultiplies(matrix);
        }
        private float GetFontSize()
        {
            float height = base.Transform.Transformer.Vertical.Length();
            this.fontSize = height;

            return this.fontSize;
        }


        //@Construct
        /// <summary>
        /// Initializes a TextArtistic-layer.
        /// </summary>
        public TextArtisticLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new TextArtisticIcon(),
                Type = this.ConstructStrings(),
            };
        }

        
        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            TextArtisticLayer artisticLayer = new TextArtisticLayer();
         
            TextLayer.FontCopyWith(artisticLayer, this);
            Layer.CopyWith(resourceCreator, artisticLayer, this);
            return artisticLayer;
        }


        public override void CacheTransform()
        {
            base.CacheTransform();
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/TextArtistic");
        }

    }
}