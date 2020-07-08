using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
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
        //@Generic
        public static string GenericText = "GammaTransfer";
        public static IAdjustmentPage GenericPage;// = new GammaTransferPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.GammaTransfer;
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page { get; } = GammaTransferAdjustment.GenericPage;
        public string Text => GammaTransferAdjustment.GenericText;

        public bool ClampOutput = false;


        #region Alpha


        public bool AlphaDisable = true;

        public float AlphaOffset = 0.0f;
        public float StartingAlphaOffset { get; private set; }
        public void CacheAlphaOffset() => this.StartingAlphaOffset = this.AlphaOffset;

        public float AlphaExponent = 1.0f;
        public float StartingAlphaExponent { get; private set; }
        public void CacheAlphaExponent() => this.StartingAlphaExponent = this.AlphaExponent;

        public float AlphaAmplitude = 1.0f;
        public float StartingAlphaAmplitude { get; private set; }
        public void CacheAlphaAmplitude() => this.StartingAlphaAmplitude = this.AlphaAmplitude;


        #endregion


        #region Red


        public bool RedDisable = true;

        public float RedOffset = 0.0f;
        public float StartingRedOffset { get; private set; }
        public void CacheRedOffset() => this.StartingRedOffset = this.RedOffset;

        public float RedExponent = 1.0f;
        public float StartingRedExponent { get; private set; }
        public void CacheRedExponent() => this.StartingRedExponent = this.RedExponent;

        public float RedAmplitude = 1.0f;
        public float StartingRedAmplitude { get; private set; }
        public void CacheRedAmplitude() => this.StartingRedAmplitude = this.RedAmplitude;


        #endregion


        #region Green


        public bool GreenDisable = true;

        public float GreenOffset = 0.0f;
        public float StartingGreenOffset { get; private set; }
        public void CacheGreenOffset() => this.StartingGreenOffset = this.GreenOffset;

        public float GreenExponent = 1.0f;
        public float StartingGreenExponent { get; private set; }
        public void CacheGreenExponent() => this.StartingGreenExponent = this.GreenExponent;

        public float GreenAmplitude = 1.0f;
        public float StartingGreenAmplitude { get; private set; }
        public void CacheGreenAmplitude() => this.StartingGreenAmplitude = this.GreenAmplitude;


        #endregion


        #region Blue


        public bool BlueDisable = true;

        public float BlueOffset = 0.0f;
        public float StartingBlueOffset { get; private set; }
        public void CacheBlueOffset() => this.StartingBlueOffset = this.BlueOffset;

        public float BlueExponent = 1.0f;
        public float StartingBlueExponent { get; private set; }
        public void CacheBlueExponent() => this.StartingBlueExponent = this.BlueExponent;

        public float BlueAmplitude = 1.0f;
        public float StartingBlueAmplitude { get; private set; }
        public void CacheBlueAmplitude() => this.StartingBlueAmplitude = this.BlueAmplitude;


        #endregion
        

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


        public void SaveWith(XElement element)
        {
            element.Add(new XElement("ClampOutput", this.ClampOutput));
            

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