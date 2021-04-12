// Core:              ★★★★
// Referenced:   ★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="TextLayer"/>'s TextArtisticLayer .
    /// </summary>
    public class TextArtisticLayer : TextLayer, ITextLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.TextArtistic;

        /// <summary> Gets or sets the text. </summary>
        public override string FontText { get; set; } = "AAA";
        /// <summary> Gets or sets the size. </summary>
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


        public override ILayer Clone()
        {
            TextArtisticLayer layer = new TextArtisticLayer();
            TextLayer.FontCopyWith(this, layer);
            LayerBase.CopyWith(this, layer);
            return layer;
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

    }
}