// Core:              ★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s ColorMatrixAdjustment.
    /// </summary>
    public class ColorMatrixAdjustment : IAdjustment
    {

        //@Content
        public AdjustmentType Type => AdjustmentType.ColorMatrix;
        public Visibility PageVisibility => Visibility.Visible;

        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }

        public ColorMatrixAdjustment()
        {
            this.Edit = new AdjustmentCommand(() => AdjustmentCommand.Edit(this));
            this.Remove = new AdjustmentCommand(() => AdjustmentCommand.Remove(this));
        }

        /// <summary> Color matrix. </summary>
        public Matrix5x4 ColorMatrix = AdjustmentExtensions.One;
        public Matrix5x4 StartingColorMatrix { get; private set; }
        public void CacheColorMatrix() => this.StartingColorMatrix = this.ColorMatrix;

        public IAdjustment Clone()
        {
            return new ColorMatrixAdjustment
            {
                ColorMatrix = this.ColorMatrix
            };
        }


        public void SaveWith(XElement element)
        {
            // Red
            element.Add(new XAttribute("M11", this.ColorMatrix.M11));
            element.Add(new XAttribute("M12", this.ColorMatrix.M12));
            element.Add(new XAttribute("M13", this.ColorMatrix.M13));
            element.Add(new XAttribute("M14", this.ColorMatrix.M14));
            // Green
            element.Add(new XAttribute("M21", this.ColorMatrix.M21));
            element.Add(new XAttribute("M22", this.ColorMatrix.M22));
            element.Add(new XAttribute("M23", this.ColorMatrix.M23));
            element.Add(new XAttribute("M24", this.ColorMatrix.M24));
            // Blue
            element.Add(new XAttribute("M31", this.ColorMatrix.M31));
            element.Add(new XAttribute("M32", this.ColorMatrix.M32));
            element.Add(new XAttribute("M33", this.ColorMatrix.M33));
            element.Add(new XAttribute("M34", this.ColorMatrix.M34));
            // Alpha
            element.Add(new XAttribute("M41", this.ColorMatrix.M41));
            element.Add(new XAttribute("M42", this.ColorMatrix.M42));
            element.Add(new XAttribute("M43", this.ColorMatrix.M43));
            element.Add(new XAttribute("M44", this.ColorMatrix.M44));
            // Offset
            element.Add(new XAttribute("M51", this.ColorMatrix.M51));
            element.Add(new XAttribute("M52", this.ColorMatrix.M52));
            element.Add(new XAttribute("M53", this.ColorMatrix.M53));
            element.Add(new XAttribute("M54", this.ColorMatrix.M54));
        }
        public void Load(XElement element)
        {
            // Red
            if (element.Attribute("M11") is XAttribute m11) this.ColorMatrix.M11 = (float)m11;
            if (element.Attribute("M12") is XAttribute m12) this.ColorMatrix.M12 = (float)m12;
            if (element.Attribute("M13") is XAttribute m13) this.ColorMatrix.M13 = (float)m13;
            if (element.Attribute("M14") is XAttribute m14) this.ColorMatrix.M14 = (float)m14;
            // Green
            if (element.Attribute("M21") is XAttribute m21) this.ColorMatrix.M21 = (float)m21;
            if (element.Attribute("M22") is XAttribute m22) this.ColorMatrix.M22 = (float)m22;
            if (element.Attribute("M23") is XAttribute m23) this.ColorMatrix.M23 = (float)m23;
            if (element.Attribute("M24") is XAttribute m24) this.ColorMatrix.M24 = (float)m24;
            // Blue
            if (element.Attribute("M31") is XAttribute m31) this.ColorMatrix.M31 = (float)m31;
            if (element.Attribute("M32") is XAttribute m32) this.ColorMatrix.M32 = (float)m32;
            if (element.Attribute("M33") is XAttribute m33) this.ColorMatrix.M33 = (float)m33;
            if (element.Attribute("M34") is XAttribute m34) this.ColorMatrix.M34 = (float)m34;
            // Alpha
            if (element.Attribute("M41") is XAttribute m41) this.ColorMatrix.M41 = (float)m41;
            if (element.Attribute("M42") is XAttribute m42) this.ColorMatrix.M42 = (float)m42;
            if (element.Attribute("M43") is XAttribute m43) this.ColorMatrix.M43 = (float)m43;
            if (element.Attribute("M44") is XAttribute m44) this.ColorMatrix.M44 = (float)m44;
            // Offset
            if (element.Attribute("M51") is XAttribute m51) this.ColorMatrix.M51 = (float)m51;
            if (element.Attribute("M52") is XAttribute m52) this.ColorMatrix.M52 = (float)m52;
            if (element.Attribute("M53") is XAttribute m53) this.ColorMatrix.M53 = (float)m53;
            if (element.Attribute("M54") is XAttribute m54) this.ColorMatrix.M54 = (float)m54;
        }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new ColorMatrixEffect
            {
                Source = image,
                ColorMatrix = this.ColorMatrix
            };
        }

    }
}