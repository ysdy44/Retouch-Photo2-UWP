using System.Xml.Linq;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Effect"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="effect"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveEffect(string elementName, Effect effect)
        {
            XElement element = new XElement(elementName);

            if (effect.GaussianBlur_IsOn)
            {
                element.Add(new XElement
               (
                   "GaussianBlur",
                   new XAttribute("Radius", effect.GaussianBlur_Radius)
                ));
            }

            if (effect.DirectionalBlur_IsOn)
            {
                element.Add(new XElement
               (
                   "DirectionalBlur",
                   new XAttribute("Radius", effect.DirectionalBlur_Radius),
                   new XAttribute("Angle", effect.DirectionalBlur_Angle)
               ));
            }

            if (effect.Sharpen_IsOn)
            {
                element.Add(new XElement
                (
                   "Sharpen",
                   new XAttribute("Amount", effect.Sharpen_Amount)
                ));
            }

            if (effect.OuterShadow_IsOn)
            { 
                element.Add(new XElement
                (
                   "OuterShadow",
                   new XAttribute("Radius", effect.OuterShadow_Radius),
                   new XAttribute("Opacity", effect.OuterShadow_Opacity),
                   FanKit.Transformers.XML.SaveColor("Color", effect.OuterShadow_Color),
                   new XAttribute("Offset", effect.OuterShadow_Offset),
                   new XAttribute("Angle", effect.OuterShadow_Angle)
                ));
            }

            if (effect.Morphology_IsOn)
            {
                element.Add(new XElement
                (
                   "Morphology",
                   new XAttribute("Size", effect.Morphology_Size)
                ));
            }

            if (effect.Emboss_IsOn)
            { 
                element.Add(new XElement
                (
                   "Emboss",
                   new XAttribute("Radius", effect.Emboss_Radius),
                   new XAttribute("Angle", effect.Emboss_Angle)
                ));
            }

            if (effect.Straighten_IsOn)
            {
                element.Add(new XElement
                (
                   "Straighten",
                   new XAttribute("Angle", effect.Straighten_Angle)
                ));
            }

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="Effect"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Effect"/>. </returns>
        public static Effect LoadEffect(XElement element)
        {
            Effect effect = new Effect();

            if (element.Element("GaussianBlur") is XElement gaussianBlur)
            {
                effect.GaussianBlur_IsOn = true;
                if (gaussianBlur.Attribute("Radius") is XAttribute radius) effect.GaussianBlur_Radius = (float)radius;
            }
            if (element.Element("DirectionalBlur") is XElement directionalBlur)
            {
                effect.DirectionalBlur_IsOn = true;
                if (directionalBlur.Attribute("Radius") is XAttribute radius) effect.DirectionalBlur_Radius = (float)radius;
                if (directionalBlur.Attribute("Angle") is XAttribute angle) effect.DirectionalBlur_Angle = (float)angle;
            }
            if (element.Element("Sharpen") is XElement sharpen)
            {
                effect.Sharpen_IsOn = true;
                if (sharpen.Attribute("Amount") is XAttribute amout) effect.Sharpen_Amount = (float)amout;
            }
            if (element.Element("OuterShadow") is XElement outerShadow)
            {
                effect.OuterShadow_IsOn = true;
                if (outerShadow.Attribute("Radius") is XAttribute radius) effect.OuterShadow_Radius = (float)radius;
                if (outerShadow.Attribute("Opacity") is XAttribute opacity) effect.OuterShadow_Opacity = (float)opacity;
                if (outerShadow.Element("Color") is XElement color) effect.OuterShadow_Color = FanKit.Transformers.XML.LoadColor(color);
                if (outerShadow.Attribute("Offset") is XAttribute offset) effect.OuterShadow_Offset = (float)offset;
                if (outerShadow.Attribute("Angle") is XAttribute angle) effect.OuterShadow_Angle = (float)angle;
            }
            if (element.Element("Morphology") is XElement morphology)
            {
                effect.Morphology_IsOn = true;
                if (morphology.Attribute("Size") is XAttribute size) effect.Morphology_Size = (int)size;
            }
            if (element.Element("Emboss") is XElement emboss)
            {
                effect.Emboss_IsOn = true;
                if (emboss.Attribute("Radius") is XAttribute radius) effect.Emboss_Radius = (float)radius;
                if (emboss.Attribute("Angle") is XAttribute angle) effect.Emboss_Angle = (float)angle;
            }
            if (element.Element("Straighten") is XElement straighten)
            {
                effect.Straighten_IsOn = true;
                if (straighten.Attribute("Angle") is XAttribute angle) effect.Straighten_Angle = (float)angle;
            }

            return effect;
        }

    }
}