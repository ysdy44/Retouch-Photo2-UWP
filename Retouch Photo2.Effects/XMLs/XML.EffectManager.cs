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

            if (effectManager.GaussianBlur_IsOn) element.Add(new XElement
            (
                "GaussianBlur",
                new XAttribute("BlurAmount", effectManager.GaussianBlur_BlurAmount)
             ));
            if (effectManager.DirectionalBlur_IsOn) element.Add(new XElement
            (
               "DirectionalBlur",
               new XAttribute("BlurAmount", effectManager.DirectionalBlur_BlurAmount),
               new XAttribute("Angle", effectManager.DirectionalBlur_Angle)
            ));
            if (effectManager.Sharpen_IsOn) element.Add(new XElement
            (
                "Sharpen",
                new XAttribute("Amount", effectManager.Sharpen_Amount)
             ));
            if (effectManager.OuterShadow_IsOn) element.Add(new XElement
            (
                "OuterShadow",
                new XAttribute("Radius", effectManager.OuterShadow_Radius),
                new XAttribute("Opacity", effectManager.OuterShadow_Opacity),
                FanKit.Transformers.XML.SaveColor("Color", effectManager.OuterShadow_Color),
                new XAttribute("Offset", effectManager.OuterShadow_Offset),
                new XAttribute("Angle", effectManager.OuterShadow_Angle)
             ));
            if (effectManager.Outline_IsOn) element.Add(new XElement
            (
               "Outline",
               new XAttribute("Size", effectManager.Outline_Size)
            ));
            if (effectManager.Emboss_IsOn) element.Add(new XElement
            (
               "Emboss",
               new XAttribute("Amount", effectManager.Emboss_Amount),
               new XAttribute("Angle", effectManager.Emboss_Angle)
            ));
            if (effectManager.Straighten_IsOn) element.Add(new XElement
            (
               "Straighten",
               new XAttribute("Angle", effectManager.Straighten_Angle)
            ));

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
                effectManager.GaussianBlur_BlurAmount = (float)gaussianBlur.Attribute("BlurAmount");
            }
            if (element.Element("DirectionalBlur") is XElement directionalBlur)
            {
                effectManager.DirectionalBlur_IsOn = true;
                effectManager.DirectionalBlur_BlurAmount = (float)directionalBlur.Attribute("BlurAmount");
                effectManager.DirectionalBlur_Angle = (float)directionalBlur.Attribute("Angle");
            }
            if (element.Element("Sharpen") is XElement sharpen)
            {
                effectManager.Sharpen_IsOn = true;
                effectManager.Sharpen_Amount = (float)sharpen.Attribute("Amount");
            }
            if (element.Element("OuterShadow") is XElement outerShadow)
            {
                effectManager.OuterShadow_IsOn = true;
                effectManager.OuterShadow_Radius = (float)outerShadow.Attribute("Radius");
                effectManager.OuterShadow_Opacity = (float)outerShadow.Attribute("Opacity");
                effectManager.OuterShadow_Color = FanKit.Transformers.XML.LoadColor(outerShadow.Element("Color"));
                effectManager.OuterShadow_Offset = (float)outerShadow.Attribute("Offset");
                effectManager.OuterShadow_Angle = (float)outerShadow.Attribute("Angle");
            }
            if (element.Element("Outline") is XElement outline)
            {
                effectManager.Outline_IsOn = true;
                effectManager.Outline_Size = (int)outline.Element("Size");
            }
            if (element.Element("Emboss") is XElement emboss)
            {
                effectManager.Emboss_IsOn = true;
                effectManager.Emboss_Amount = (float)emboss.Element("Amount");
                effectManager.Emboss_Angle = (float)emboss.Element("Angle");
            }
            if (element.Element("Straighten") is XElement straighten)
            {
                effectManager.Straighten_IsOn = true;
                effectManager.Straighten_Angle = (int)straighten.Element("Angle");
            }

            return effectManager;
        }

    }
}