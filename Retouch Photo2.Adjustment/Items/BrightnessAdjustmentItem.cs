﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;
using System.Numerics;

namespace Retouch_Photo2.Adjustments.Items
{
    /// <summary>
    /// Item of <see cref="BrightnessAdjustment">.
    /// </summary>
    public class BrightnessAdjustmentItem : AdjustmentItem
    {       
        /// <summary> Interval 1.0->0.5 . </summary>
        public float WhiteLight;      
        /// <summary> Interval 1.0->0.5 . </summary>
        public float WhiteDark;

        /// <summary> Interval 0.0->0.5 . </summary>
        public float BlackLight;   
        /// <summary> Interval 0.0->0.5 . </summary>
        public float BlackDark;

        //@Construct
        public BrightnessAdjustmentItem()
        {
            base.Name = BrightnessAdjustment.Name;
        }

        //@Override
        public override Adjustment GetNewAdjustment()
        {
            BrightnessAdjustment adjustment = new BrightnessAdjustment();

            adjustment.BrightnessAdjustmentItem.WhiteLight = this.WhiteLight;
            adjustment.BrightnessAdjustmentItem.WhiteDark = this.WhiteDark;
            adjustment.BrightnessAdjustmentItem.BlackLight = this.BlackLight;
            adjustment.BrightnessAdjustmentItem.BlackDark = this.BlackDark;

            return adjustment;
        }
        public override void Reset()
        {
            this.WhiteLight = 1.0f;
            this.WhiteDark = 1.0f;
            this.BlackLight = 1.0f;
            this.BlackDark = 1.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new BrightnessEffect
            {
                WhitePoint = new Vector2
                (
                    x: this.WhiteLight,
                    y: this.WhiteDark
                ),
                BlackPoint = new Vector2
                (
                    x: this.BlackDark,
                    y: this.BlackLight
                ),
                Source = image
            };
        }
    }
}