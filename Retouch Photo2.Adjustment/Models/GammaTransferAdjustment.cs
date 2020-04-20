using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s GammaTransferAdjustment.
    /// </summary>
    public class GammaTransferAdjustment : IAdjustment
    {
        //@Static
        public static readonly GammaTransferPage GammaTransferPage = new GammaTransferPage();

        public AdjustmentType Type => AdjustmentType.GammaTransfer;
        public FrameworkElement Icon { get; } = new GammaTransferIcon();
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page => GammaTransferAdjustment.GammaTransferPage;
        public string Text { get; private set; }

        public bool ClampOutput= false;

        public bool AlphaDisable = true;
        public float AlphaOffset = 0.0f;
        public float AlphaExponent = 1.0f;
        public float AlphaAmplitude = 1.0f;

        public bool RedDisable = true;
        public float RedOffset = 0.0f;
        public float RedExponent = 1.0f;
        public float RedAmplitude = 1.0f;

        public bool GreenDisable = true;
        public float GreenOffset = 0.0f;
        public float GreenExponent = 1.0f;
        public float GreenAmplitude = 1.0f;

        public bool BlueDisable = true;
        public float BlueOffset = 0.0f;
        public float BlueExponent = 1.0f;
        public float BlueAmplitude = 1.0f;


        //@Construct
        /// <summary>
        /// Construct a gammaTransfer-adjustment.
        /// </summary>
        public GammaTransferAdjustment()
        {
            this.Text = GammaTransferAdjustment.GammaTransferPage.Text;
        }


        public void Reset()
        {
            this.ClampOutput = false;

            this.AlphaDisable = true;
            this.AlphaOffset = 0;
            this.AlphaExponent = 1;
            this.AlphaAmplitude = 1;

            this.RedDisable = true;
            this.RedOffset = 0;
            this.RedExponent = 1;
            this.RedAmplitude = 1;

            this.GreenDisable = true;
            this.GreenOffset = 0;
            this.GreenExponent = 1;
            this.GreenAmplitude = 1;

            this.BlueDisable = true;
            this.BlueOffset = 0;
            this.BlueExponent = 1;
            this.BlueAmplitude = 1;

            if (GammaTransferAdjustment.GammaTransferPage.Adjustment == this)
            {
                GammaTransferAdjustment.GammaTransferPage.Follow(this);
            }
        }
        public void Follow()
        {
            GammaTransferAdjustment.GammaTransferPage.Adjustment = this;
            GammaTransferAdjustment.GammaTransferPage.Follow(this);
        }
        public void Close()
        {
            GammaTransferAdjustment.GammaTransferPage.Adjustment = null;
        }
        

        public IAdjustment Clone()
        {
            return new GammaTransferAdjustment
            {
                ClampOutput = this.ClampOutput,

                AlphaDisable = this.AlphaDisable,
                AlphaOffset = this.AlphaOffset,
                AlphaExponent = this.AlphaExponent,
                AlphaAmplitude = this.AlphaAmplitude,

                RedDisable = this.RedDisable,
                RedOffset = this.RedOffset,
                RedExponent = this.RedExponent,
                RedAmplitude = this.RedAmplitude,

                GreenDisable = this.GreenDisable,
                GreenOffset = this.GreenOffset,
                GreenExponent = this.GreenExponent,
                GreenAmplitude = this.GreenAmplitude,

                BlueDisable = this.BlueDisable,
                BlueOffset = this.BlueOffset,
                BlueExponent = this.BlueExponent,
                BlueAmplitude = this.BlueAmplitude,
            };
        }


        public XElement Save()
        {
            XElement element = new XElement
            (
                "GammaTransfer",
                new XAttribute("ClampOutput", this.ClampOutput)
            );

            if (this.AlphaDisable) element.Add(new XAttribute("AlphaDisable", false));
            else
            {
                element.Add(new XAttribute("AlphaDisable", true));
                element.Add(new XAttribute("AlphaOffset", this.AlphaOffset));
                element.Add(new XAttribute("AlphaExponent", this.AlphaExponent));
                element.Add(new XAttribute("AlphaAmplitude", this.AlphaAmplitude));
            }

            if (this.RedDisable) element.Add(new XAttribute("RedDisable", false));
            else
            {
                element.Add(new XAttribute("RedDisable", true));
                element.Add(new XAttribute("RedOffset", this.RedOffset));
                element.Add(new XAttribute("RedExponent", this.RedExponent));
                element.Add(new XAttribute("RedAmplitude", this.RedAmplitude));
            }

            if (this.GreenDisable) element.Add(new XAttribute("GreenDisable", false));
            else
            {
                element.Add(new XAttribute("GreenDisable", true));
                element.Add(new XAttribute("GreenOffset", this.GreenOffset));
                element.Add(new XAttribute("GreenExponent", this.GreenExponent));
                element.Add(new XAttribute("GreenAmplitude", this.GreenAmplitude));
            }

            if (this.BlueDisable) element.Add(new XAttribute("BlueDisable", false));
            else
            {
                element.Add(new XAttribute("BlueDisable", true));
                element.Add(new XAttribute("BlueOffset", this.BlueOffset));
                element.Add(new XAttribute("BlueExponent", this.BlueExponent));
                element.Add(new XAttribute("BlueAmplitude", this.BlueAmplitude));
            }

            return element;
        }
        public void Load(XElement element)
        {
            this.ClampOutput = (bool)element.Attribute("ClampOutput");

            bool alphaDisable = (bool)element.Attribute("AlphaDisable");
            if (alphaDisable) this.AlphaDisable = true;
            else
            {
                this.AlphaDisable = false;
                this.AlphaOffset = (float)element.Attribute("AlphaOffset");
                this.AlphaExponent = (float)element.Attribute("AlphaExponent");
                this.AlphaAmplitude = (float)element.Attribute("AlphaAmplitude");
            }

            bool redDisable = (bool)element.Attribute("RedDisable");
            if (redDisable) this.RedDisable = true;
            else
            {
                this.RedDisable = false;
                this.RedOffset = (float)element.Attribute("RedOffset");
                this.RedExponent = (float)element.Attribute("RedExponent");
                this.RedAmplitude = (float)element.Attribute("RedAmplitude");
            }

            bool greenDisable = (bool)element.Attribute("GreenDisable");
            if (greenDisable) this.GreenDisable = true;
            else
            {
                this.GreenDisable = false;
                this.GreenOffset = (float)element.Attribute("GreenOffset");
                this.GreenExponent = (float)element.Attribute("GreenExponent");
                this.GreenAmplitude = (float)element.Attribute("GreenAmplitude");
            }

            bool blueDisable = (bool)element.Attribute("BlueDisable");
            if (blueDisable) this.BlueDisable = true;
            else
            {
                this.BlueDisable = false;
                this.BlueOffset = (float)element.Attribute("BlueOffset");
                this.BlueExponent = (float)element.Attribute("BlueExponent");
                this.BlueAmplitude = (float)element.Attribute("BlueAmplitude");
            }
        }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new GammaTransferEffect
            {
                ClampOutput = this.ClampOutput,

                AlphaDisable = this.AlphaDisable,
                AlphaOffset = this.AlphaOffset,
                AlphaExponent = this.AlphaExponent,
                AlphaAmplitude = this.AlphaAmplitude,

                RedDisable = this.RedDisable,
                RedOffset = this.RedOffset,
                RedExponent = this.RedExponent,
                RedAmplitude = this.RedAmplitude,

                GreenDisable = this.GreenDisable,
                GreenOffset = this.GreenOffset,
                GreenExponent = this.GreenExponent,
                GreenAmplitude = this.GreenAmplitude,

                BlueDisable = this.BlueDisable,
                BlueOffset = this.BlueOffset,
                BlueExponent = this.BlueExponent,
                BlueAmplitude = this.BlueAmplitude,

                Source = image
            };
        }
    }
}