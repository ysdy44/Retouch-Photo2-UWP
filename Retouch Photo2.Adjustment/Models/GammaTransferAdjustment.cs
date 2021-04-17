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
    /// <see cref="IAdjustment"/>'s GammaTransferAdjustment.
    /// </summary>
    public class GammaTransferAdjustment : IAdjustment
    {

        //@Content
        public AdjustmentType Type => AdjustmentType.GammaTransfer;
        public Visibility PageVisibility => Visibility.Visible;

        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }

        public GammaTransferAdjustment()
        {
            this.Edit = new AdjustmentCommand(() => AdjustmentCommand.Edit(this));
            this.Remove = new AdjustmentCommand(() => AdjustmentCommand.Remove(this));
        }

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

            element.Add(new XAttribute("AlphaDisable", this.AlphaDisable));
            if (this.AlphaDisable == false)
            {
                element.Add(new XAttribute("AlphaDisable", false));
                element.Add(new XAttribute("AlphaOffset", this.AlphaOffset));
                element.Add(new XAttribute("AlphaExponent", this.AlphaExponent));
                element.Add(new XAttribute("AlphaAmplitude", this.AlphaAmplitude));
            }

            element.Add(new XAttribute("RedDisable", this.RedDisable));
            if (this.RedDisable == false)
            {
                element.Add(new XAttribute("RedOffset", this.RedOffset));
                element.Add(new XAttribute("RedExponent", this.RedExponent));
                element.Add(new XAttribute("RedAmplitude", this.RedAmplitude));
            }

            element.Add(new XAttribute("GreenDisable", this.GreenDisable));
            if (this.GreenDisable == false)
            {
                element.Add(new XAttribute("GreenOffset", this.GreenOffset));
                element.Add(new XAttribute("GreenExponent", this.GreenExponent));
                element.Add(new XAttribute("GreenAmplitude", this.GreenAmplitude));
            }

            element.Add(new XAttribute("BlueDisable", this.BlueDisable));
            if (this.BlueDisable == false)
            {
                element.Add(new XAttribute("BlueOffset", this.BlueOffset));
                element.Add(new XAttribute("BlueExponent", this.BlueExponent));
                element.Add(new XAttribute("BlueAmplitude", this.BlueAmplitude));
            }
        }
        public void Load(XElement element)
        {
            if (element.Attribute("ClampOutput") is XAttribute clampOutput) this.ClampOutput = (bool)clampOutput;

            if (element.Attribute("AlphaDisable") is XAttribute alphaDisable) this.AlphaDisable = (bool)alphaDisable;
            if (this.AlphaDisable == false)
            {
                if (element.Attribute("AlphaOffset") is XAttribute alphaOffset) this.AlphaOffset = (float)alphaOffset;
                if (element.Attribute("AlphaExponent") is XAttribute alphaExponent) this.AlphaExponent = (float)alphaExponent;
                if (element.Attribute("AlphaAmplitude") is XAttribute alphaAmplitude) this.AlphaAmplitude = (float)alphaAmplitude;
            }

            if (element.Attribute("RedDisable") is XAttribute redDisable) this.RedDisable = (bool)redDisable;
            if (this.RedDisable == false)
            {
                if (element.Attribute("RedOffset") is XAttribute redOffset) this.RedOffset = (float)redOffset;
                if (element.Attribute("RedExponent") is XAttribute redExponent) this.RedExponent = (float)redExponent;
                if (element.Attribute("RedAmplitude") is XAttribute redAmplitude) this.RedAmplitude = (float)redAmplitude;
            }

            if (element.Attribute("GreenDisable") is XAttribute greenDisable) this.GreenDisable = (bool)greenDisable;
            if (this.GreenDisable == false)
            {
                if (element.Attribute("GreenOffset") is XAttribute greenOffset) this.GreenOffset = (float)greenOffset;
                if (element.Attribute("GreenExponent") is XAttribute greenExponent) this.GreenExponent = (float)greenExponent;
                if (element.Attribute("GreenAmplitude") is XAttribute greenAmplitude) this.GreenAmplitude = (float)greenAmplitude;
            }

            if (element.Attribute("BlueDisable") is XAttribute blueDisable) this.BlueDisable = (bool)blueDisable;
            if (this.BlueDisable == false)
            {
                if (element.Attribute("BlueOffset") is XAttribute blueOffset) this.BlueOffset = (float)blueOffset;
                if (element.Attribute("BlueExponent") is XAttribute blueExponent) this.BlueExponent = (float)blueExponent;
                if (element.Attribute("BlueAmplitude") is XAttribute blueAmplitude) this.BlueAmplitude = (float)blueAmplitude;
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