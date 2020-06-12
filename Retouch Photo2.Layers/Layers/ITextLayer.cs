using Microsoft.Graphics.Canvas.Text;
using Windows.UI.Text;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// <see cref="LayerBase"/>'s ITextLayer .
    /// </summary>
    public interface ITextLayer
    {
        /// <summary> Gets or sets the text. </summary>
        string FontText { get; set; }
        /// <summary> Gets or sets the size. </summary>
        float FontSize { get; set; }
        /// <summary> Gets or sets the FontFamily. </summary>
        string FontFamily { get; set; }

        /// <summary> Gets or sets the HorizontalAlignment. </summary>
        CanvasHorizontalAlignment FontAlignment { get; set; }
        /// <summary> Gets or sets the style. </summary>
        FontStyle FontStyle { get; set; }
        /// <summary> Gets or sets the weight. </summary>
        FontWeight FontWeight { get; set; }
    }
}