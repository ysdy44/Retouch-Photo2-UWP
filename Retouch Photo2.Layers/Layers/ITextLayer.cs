// Core:              ★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★
// Complete:      ★★★★★
using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Texts;
using Windows.UI.Text;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// <see cref="LayerBase"/>'s ITextLayer .
    /// </summary>
    public interface ITextLayer
    {
        /// <summary> Gets or sets the font text. </summary>
        string FontText { get; set; }
        /// <summary> Gets or sets the font size. </summary>
        float FontSize { get; set; }
        /// <summary> Gets or sets the font family. </summary>
        string FontFamily { get; set; }

        /// <summary> Gets or sets the font horizontal alignment. </summary>
        CanvasHorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary> Gets or sets the underline. </summary>
        bool Underline { get; set; }
        /// <summary> Gets or sets the font style. </summary>
        FontStyle FontStyle { get; set; }
        /// <summary> Gets or sets the font weight. </summary>
        FontWeight2 FontWeight { get; set; }
        /// <summary> Gets or sets the direction. </summary>
        CanvasTextDirection Direction { get; set; }
    }
}