using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s GammaTransferAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GammaTransferAdjustment : IAdjustment
    {
        [JsonProperty]
        public string TypeName { get; } = AdjustmentType.GammaTransfer.ToString();
         public AdjustmentType Type => AdjustmentType.GammaTransfer;
         public FrameworkElement Icon { get; } = new GammaTransferControl();
         public Visibility Visibility => Visibility.Visible;


        [JsonProperty]
        public bool ClampOutput;

        [JsonProperty]
        public bool AlphaDisable;
        [JsonProperty]
        public float AlphaOffset;
        [JsonProperty]
        public float AlphaExponent;
        [JsonProperty]
        public float AlphaAmplitude;

        [JsonProperty]
        public bool RedDisable;
        [JsonProperty]
        public float RedOffset;
        [JsonProperty]
        public float RedExponent;
        [JsonProperty]
        public float RedAmplitude;

        [JsonProperty]
        public bool GreenDisable;
        [JsonProperty]
        public float GreenOffset;
        [JsonProperty]
        public float GreenExponent;
        [JsonProperty]
        public float GreenAmplitude;

        [JsonProperty]
        public bool BlueDisable;
        [JsonProperty]
        public float BlueOffset;
        [JsonProperty]
        public float BlueExponent;
        [JsonProperty]
        public float BlueAmplitude;


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
    }
}