using System.Xml.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="TransformManager"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="transformManager"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        private static XElement SaveTransformManager(string elementName, TransformManager transformManager)
        {
            XElement element= new XElement
            (
                elementName,
                FanKit.Transformers.XML.SaveTransformer("Source", transformManager.Source),
                FanKit.Transformers.XML.SaveTransformer("Destination", transformManager.Destination)
            );

            if (transformManager.IsCrop)
            {
                element.Add(new XElement("IsCrop",true));
                element.Add(FanKit.Transformers.XML.SaveTransformer("CropDestination", transformManager.CropDestination));
            }
            else
            {
                element.Add(new XElement("IsCrop", false));
            }

            return element;
         }

        /// <summary>
        ///  Loads a <see cref="TransformManager"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="TransformManager"/>. </returns>
        private static TransformManager LoadTransformManager(XElement element)
        {
            TransformManager transformManager = new TransformManager();

            if (element.Element("Source") is XElement source) transformManager.Source = FanKit.Transformers.XML.LoadTransformer(source);
            if (element.Element("Destination") is XElement destination) transformManager.Destination = FanKit.Transformers.XML.LoadTransformer(destination);

            if (element.Element("IsCrop") is XElement isCrop) transformManager.IsCrop = (bool)isCrop;
            if (transformManager.IsCrop)
            {
                if (element.Element("CropDestination") is XElement cropDestination) transformManager.CropDestination = FanKit.Transformers.XML.LoadTransformer(cropDestination);
            }

            return transformManager;
        }

    }
}