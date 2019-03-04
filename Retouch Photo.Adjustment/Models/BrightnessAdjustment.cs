using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;
using System.Numerics;

namespace Retouch_Photo.Adjustments.Models
{
    public class BrightnessAdjustment : Adjustment
    {

        /// <summary> Interval (1.0, 1.0) -> (0.5, 0.5), white is (0.0, 1.0). </summary>
        Vector2 WhitePoint = Vector2.One;
        /// <summary> Interval (0.0, 0.0) -> (0.5, 0.5), black is (1.0, 0.0). </summary>
        Vector2 BlackPoint = Vector2.Zero;

        /// <summary> Interval 1.0->0.5 . </summary>
        public float WhiteLight
        {
            get => this.WhitePoint.X;
            set => this.WhitePoint.X = value;
        }
        /// <summary> Interval 1.0->0.5 . </summary>
        public float WhiteDark
        {
            get => this.WhitePoint.Y;
            set => this.WhitePoint.Y = value;
        }


        /// <summary> Interval 0.0->0.5 . </summary>
        public float BlackLight
        {
            get => this.BlackPoint.Y;
            set => this.BlackPoint.Y = value;
        } 
       /// <summary> Interval 0.0->0.5 . </summary>
        public float BlackDark
        {
            get => this.BlackPoint.X;
            set => this.BlackPoint.X = value;
        }

        public BrightnessAdjustment()
        {
            base.Type = AdjustmentType.Brightness;
            base.Icon = new BrightnessControl();
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.WhitePoint = Vector2.One;
            this.BlackPoint = Vector2.Zero;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new BrightnessEffect
            {
                WhitePoint = this.WhitePoint,
                BlackPoint = this.BlackPoint,
                Source = image
            };
        }
    }
}

