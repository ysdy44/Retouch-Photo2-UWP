using Microsoft.Graphics.Canvas.Text;
using Windows.UI.Text;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// <see cref="ILayer"/>'s ITextLayer .
    /// </summary>
    public interface ITextLayer
    {
        /// <summary> Gets or sets ITextLayer's text. </summary>
        string FontText { get; set; }
        /// <summary> Gets or sets ITextLayer's size. </summary>
        float FontSize { get; set; }
        /// <summary> Gets or sets ITextLayer's FontFamily. </summary>
        string FontFamily { get; set; }

        /// <summary> Gets or sets ITextLayer's HorizontalAlignment. </summary>
        CanvasHorizontalAlignment FontAlignment { get; set; }
        /// <summary> Gets or sets ITextLayer's style. </summary>
        FontStyle FontStyle { get; set; }
        /// <summary> Gets or sets ITextLayer's weight. </summary>
        FontWeight FontWeight { get; set; }
    }
}