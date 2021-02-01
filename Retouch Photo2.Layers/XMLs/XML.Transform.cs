// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System.Xml.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Transform"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="transform"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        private static XElement SaveTransform(string elementName, Transform transform)
        {
            XElement element= new XElement
            (
                elementName,
                FanKit.Transformers.XML.SaveTransformer("Transformer", transform.Transformer)
            );

            if (transform.IsCrop)
            {
                element.Add(new XElement("IsCrop",true));
                element.Add(FanKit.Transformers.XML.SaveTransformer("CropTransformer", transform.CropTransformer));
            }
            else
            {
                element.Add(new XElement("IsCrop", false));
            }

            return element;
         }

        /// <summary>
        ///  Loads a <see cref="Transform"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Transform"/>. </returns>
        private static Transform LoadTransform(XElement element)
        {
            Transform transform = new Transform();

            if (element.Element("Transformer") is XElement transformer) transform.Transformer = FanKit.Transformers.XML.LoadTransformer(transformer);

            if (element.Element("IsCrop") is XElement isCrop) transform.IsCrop = (bool)isCrop;
            if (transform.IsCrop)
            {
                if (element.Element("CropTransformer") is XElement cropTransformer) transform.CropTransformer = FanKit.Transformers.XML.LoadTransformer(cropTransformer);
            }

            return transform;
        }

    }
}