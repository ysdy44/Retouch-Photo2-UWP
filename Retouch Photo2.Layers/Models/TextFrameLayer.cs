// Core:              ★★★★
// Referenced:   ★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="TextLayer"/>'s TextFrameLayer .
    /// </summary>
    public class TextFrameLayer : TextLayer, ITextLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.TextFrame;

        /// <summary> Gets or sets the text. </summary>
        public override string FontText { get; set; } = string.Empty;
        /// <summary> Gets or sets the size. </summary>
        public override float FontSize { get; set; } = 22.0f;


        public override ILayer Clone()
        {
            TextArtisticLayer layer = new TextArtisticLayer();
            TextLayer.FontCopyWith(this, layer);
            LayerBase.CopyWith(this, layer);
            return layer;
        }

    }
}