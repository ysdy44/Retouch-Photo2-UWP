using Microsoft.Graphics.Canvas;
using System;
using System.Xml.Linq;

namespace Retouch_Photo2
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="CanvasBitmap"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="bitmap"> The destination <see cref="CanvasBitmap"/>. </param>
        public static XElement SaveBitmap(string elementName, CanvasBitmap bitmap)
        {
            XElement element = new XElement(elementName);
            element.Add(new XElement("PixelWidth", bitmap.SizeInPixels.Width));
            element.Add(new XElement("PixelHeight", bitmap.SizeInPixels.Height));

            byte[] bytes = bitmap.GetPixelBytes();
            string strings = Convert.ToBase64String(bytes);
            element.Add(new XElement("PixelBytes", strings));

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="CanvasBitmap"/> from an XElement.
        /// </summary>
        /// <param name="canvasResource"> The canvas-resource. </param>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="CanvasBitmap"/>. </returns>
        public static CanvasBitmap LoadBitmap(ICanvasResourceCreatorWithDpi canvasResource, XElement element)
        {
            int width = 1024, height = 1024;
            if (element.Element("PixelWidth") is XElement pixelWidth) width = (int)pixelWidth;
            if (element.Element("PixelHeight") is XElement pixelHeight) height = (int)pixelHeight;
            CanvasRenderTarget bitmap = new CanvasRenderTarget(canvasResource, width, height);

            if (element.Element("PixelBytes") is XElement pixelBytes) 
            {
                string strings = pixelBytes.Value;
                byte[] bytes = Convert.FromBase64String(strings);
                bitmap.SetPixelBytes(bytes);
            }

            return null;
        }

    }
}