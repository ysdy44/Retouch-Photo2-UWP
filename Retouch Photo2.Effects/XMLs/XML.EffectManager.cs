using System.Xml.Linq;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="EffectManager"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="effectManager"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveEffectManager(string elementName, EffectManager effectManager)
        {
            XElement element = new XElement(elementName);

            if (effectManager.GaussianBlur_IsOn)
            {
                element.Add(new XElement
               (
                   "GaussianBlur",
                   new XAttribute("Radius", effectManager.GaussianBlur_Radius)
                ));
            }

            if (effectManager.DirectionalBlur_IsOn)
            {
                element.Add(new XElement
               (
                   "DirectionalBlur",
                   new XAttribute("Radius", effectManager.DirectionalBlur_Radius),
                   new XAttribute("Angle", effectManager.DirectionalBlur_Angle)
               ));
            }

            if (effectManager.Sharpen_IsOn)
            {
                element.Add(new XElement
                (
                   "Sharpen",
                   new XAttribute("Amount", effectManager.Sharpen_Amount)
                ));
            }

            if (effectManager.OuterShadow_IsOn)
            { 
                element.Add(new XElement
                (
                   "OuterShadow",
                   new XAttribute("Radius", effectManager.OuterShadow_Radius),
                   new XAttribute("Opacity", effectManager.OuterShadow_Opacity),
                   FanKit.Transformers.XML.SaveColor("Color", effectManager.OuterShadow_Color),
                   new XAttribute("Offset", effectManager.OuterShadow_Offset),
                   new XAttribute("Angle", effectManager.OuterShadow_Angle)
                ));
            }

            if (effectManager.Outline_IsOn)
            {
                element.Add(new XElement
                (
                   "Outline",
                   new XAttribute("Size", effectManager.Outline_Size)
                ));
            }

            if (effectManager.Emboss_IsOn)
            { 
                element.Add(new XElement
                (
                   "Emboss",
                   new XAttribute("Radius", effectManager.Emboss_Radius),
                   new XAttribute("Angle", effectManager.Emboss_Angle)
                ));
            }

            if (effectManager.Straighten_IsOn)
            {
                element.Add(new XElement
                (
                   "Straighten",
                   new XAttribute("Angle", effectManager.Straighten_Angle)
                ));
            }

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="EffectManager"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="EffectManager"/>. </returns>
        public static EffectManager LoadEffectManager(XElement element)
        {
            EffectManager effectManager = new EffectManager();

            if (element.Element("GaussianBlur") is XElement gaussianBlur)
            {
                effectManager.GaussianBlur_IsOn = true;
                if (gaussianBlur.Attribute("Radius") is XAttribute radius) effectManager.GaussianBlur_Radius = (float)radius;
            }
            if (element.Element("DirectionalBlur") is XElement directionalBlur)
            {
                effectManager.DirectionalBlur_IsOn = true;
                if (directionalBlur.Attribute("Radius") is XAttribute radius) effectManager.DirectionalBlur_Radius = (float)radius;
                if (directionalBlur.Attribute("Angle") is XAttribute angle) effectManager.DirectionalBlur_Angle = (float)angle;
            }
            if (element.Element("Sharpen") is XElement sharpen)
            {
                effectManager.Sharpen_IsOn = true;
                if (sharpen.Attribute("Amount") is XAttribute amout) effectManager.Sharpen_Amount = (float)amout;
            }
            if (element.Element("OuterShadow") is XElement outerShadow)
            {
                effectManager.OuterShadow_IsOn = true;
                if (outerShadow.Attribute("Radius") is XAttribute radius) effectManager.OuterShadow_Radius = (float)radius;
                if (outerShadow.Attribute("Opacity") is XAttribute opacity) effectManager.OuterShadow_Opacity = (float)opacity;
                if (outerShadow.Element("Color") is XElement color) effectManager.OuterShadow_Color = FanKit.Transformers.XML.LoadColor(color);
                if (outerShadow.Attribute("Offset") is XAttribute offset) effectManager.OuterShadow_Offset = (float)offset;
                if (outerShadow.Attribute("Angle") is XAttribute angle) effectManager.OuterShadow_Angle = (float)angle;
            }
            if (element.Element("Outline") is XElement outline)
            {
                effectManager.Outline_IsOn = true;
                if (outline.Attribute("Size") is XAttribute size) effectManager.Outline_Size = (int)size;
            }
            if (element.Element("Emboss") is XElement emboss)
            {
                effectManager.Emboss_IsOn = true;
                if (emboss.Attribute("Radius") is XAttribute radius) effectManager.Emboss_Radius = (float)radius;
                if (emboss.Attribute("Angle") is XAttribute angle) effectManager.Emboss_Angle = (float)angle;
            }
            if (element.Element("Straighten") is XElement straighten)
            {
                effectManager.Straighten_IsOn = true;
                if (straighten.Attribute("Angle") is XAttribute angle) effectManager.Straighten_Angle = (float)angle;
            }

            return effectManager;
        }

    }
}