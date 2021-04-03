// Core:              ★★★★★
// Referenced:   ★★★★
// Difficult:         ★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Represents a special effect that adds effects to layers
    /// </summary>
    public class Effect
    {

        #region GaussianBlur

        /// <summary> GaussianBlur's IsOn. </summary>
        public bool GaussianBlur_IsOn;
        /// <summary> GaussianBlur's radius. </summary>
        public float GaussianBlur_Radius = 0;
        /// <summary> Starting gaussianBlur's radius. </summary>
        public float StartingGaussianBlur_Radius { get; private set; }
        /// <summary> Starting gaussianBlur's border mode. </summary>
        public EffectBorderMode GaussianBlur_BorderMode = EffectBorderMode.Soft;
        /// <summary> Cache gaussianBlur. </summary>
        public void CacheGaussianBlur()
        {
            this.StartingGaussianBlur_Radius = this.GaussianBlur_Radius;
        }

        #endregion


        #region DirectionalBlur

        /// <summary> DirectionalBlur's IsOn. </summary>
        public bool DirectionalBlur_IsOn;
        /// <summary> DirectionalBlur's radius. </summary>
        public float DirectionalBlur_Radius = 0;
        /// <summary> Starting directionalBlur's radius. </summary>
        public float StartingDirectionalBlur_Radius { get; private set; }
        /// <summary> DirectionalBlur's angle. </summary>
        public float DirectionalBlur_Angle = 0;
        /// <summary> Starting directionalBlur's angle. </summary>
        public float StartingDirectionalBlur_Angle { get; private set; }
        /// <summary> Starting directionalBlur's border mode. </summary>
        public EffectBorderMode DirectionalBlur_BorderMode = EffectBorderMode.Soft;
        /// <summary> Cache directionalBlur. </summary>
        public void CacheDirectionalBlur()
        {
            this.StartingDirectionalBlur_Radius = this.DirectionalBlur_Radius;
            this.StartingDirectionalBlur_Angle = this.DirectionalBlur_Angle;
        }

        #endregion


        #region Sharpen

        /// <summary> Sharpen's IsOn. </summary>
        public bool Sharpen_IsOn;
        /// <summary> Sharpen's amount. </summary>
        public float Sharpen_Amount = 0;
        /// <summary> Starting sharpen's angle. </summary> 
        public float StartingSharpen_Amount { get; private set; }
        /// <summary> Cache sharpen. </summary>
        public void CacheSharpen()
        {
            this.StartingSharpen_Amount = this.Sharpen_Amount;
        }

        #endregion


        #region OuterShadow

        /// <summary> Outer-shadow's IsOn. </summary>
        public bool OuterShadow_IsOn;
        /// <summary> Outer-shadow's radius. </summary>
        public float OuterShadow_Radius = 12.0f;
        /// <summary> Starting outer-shadow's radius. </summary>
        public float StartingOuterShadow_Radius { get; private set; }
        /// <summary> Outer-shadow's opacity. </summary>
        public float OuterShadow_Opacity = 0.5f;
        /// <summary> Starting outer-shadow's opacity. </summary>
        public float StartingOuterShadow_Opacity { get; private set; }
        /// <summary> Outer-shadow's color. </summary>
        public Color OuterShadow_Color = Colors.Black;
        /// <summary> Starting outer-shadow's color. </summary>
        public Color StartingOuterShadow_Color { get; private set; }
        /// <summary> Outer-shadow's offset. </summary>
        public float OuterShadow_Offset
        {
            get => this.outerShadow_Offset;
            set
            {
                this.OuterShadow_Position = new Vector2((float)Math.Cos(this.OuterShadow_Angle), (float)Math.Sin(this.OuterShadow_Angle)) * value;
                this.outerShadow_Offset = value;
            }
        }
        private float outerShadow_Offset = 0;
        /// <summary> Starting outer-shadow's offset. </summary>
        public float StartingOuterShadow_Offset { get; private set; }
        /// <summary> Outer-shadow's angle. </summary>
        public float OuterShadow_Angle
        {
            get => this.outerShadow_Angle;
            set
            {
                this.OuterShadow_Position = new Vector2((float)Math.Cos(value), (float)Math.Sin(value)) * this.OuterShadow_Offset;
                this.outerShadow_Angle = value;
            }
        }
        private float outerShadow_Angle = FanKit.Math.PiOver4;
        /// <summary> Starting outer-shadow's angle. </summary>
        public float StartingOuterShadow_Angle { get; private set; }    
        private Vector2 OuterShadow_Position = Vector2.Zero;
        /// <summary> Cache outer-shadow. </summary>
        public void CacheOuterShadow()
        {
            this.StartingOuterShadow_Radius = this.OuterShadow_Radius;
            this.StartingOuterShadow_Opacity = this.OuterShadow_Opacity;
            this.StartingOuterShadow_Color = this.OuterShadow_Color;

            this.StartingOuterShadow_Offset = this.OuterShadow_Offset;
            this.StartingOuterShadow_Angle = this.OuterShadow_Angle;
        }

        #endregion


        #region Edge

        /// <summary> Edge's IsOn. </summary>
        public bool Edge_IsOn;
        /// <summary> Edge's amount. </summary>
        public float Edge_Amount = 0.5f;
        /// <summary> Starting edge's amount. </summary>
        public float StartingEdge_Amount { get; private set; }
        /// <summary> Edge's radius. </summary>
        public float Edge_Radius = 0.0f;
        /// <summary> Starting edge's radius. </summary>
        public float StartingEdge_Radius { get; private set; }
        /// <summary> Cache edge. </summary>
        public void CacheEdge()
        {
            this.StartingEdge_Radius = this.Edge_Radius;
            this.StartingEdge_Amount = this.Edge_Amount;
        }

        #endregion


        #region Morphology

        /// <summary> Morphology's IsOn. </summary>
        public bool Morphology_IsOn;
        /// <summary> Morphology's size. </summary>
        public int Morphology_Size
        {
            get => this.morphology_Size;
            set
            {
                this.Morphology_Mode = (value > 0) ? MorphologyEffectMode.Dilate : MorphologyEffectMode.Erode;

                int s = Math.Abs(value);
                if (s > 90) s = 90;
                if (s < 1) s = 1;
                this.Morphology_Height = s;
                this.Morphology_Width = s;

                this.morphology_Size = value;
            }
        }
        private int morphology_Size = 1;
        /// <summary> Morphology's mode. </summary>
        public MorphologyEffectMode Morphology_Mode { get; private set; }
        /// <summary> Morphology's width. </summary>
        public int Morphology_Width { get; private set; } = 1;
        /// <summary> Morphology's height. </summary>
        public int Morphology_Height { get; private set; } = 1;
        /// <summary> Morphology's size. </summary>
        public int StartingMorphology_Size { get; private set; }
        /// <summary> Cache morphology. </summary>
        public void CacheMorphology()
        {
            this.StartingMorphology_Size = this.Morphology_Size;
        }

        #endregion


        #region Emboss

        /// <summary> Emboss's IsOn. </summary>
        public bool Emboss_IsOn;
        /// <summary> Emboss's radius. </summary>
        public float Emboss_Radius = 1;
        /// <summary> Starting emboss's radius. </summary>
        public float StartingEmboss_Radius { get; private set; }
        /// <summary> Emboss's angle. </summary>
        public float Emboss_Angle = 0;
        /// <summary> Starting emboss's angle. </summary>
        public float StartingEmboss_Angle { get; private set; }
        /// <summary> Cache emboss. </summary>
        public void CacheEmboss()
        {
            this.StartingEmboss_Radius = this.Emboss_Radius;
            this.StartingEmboss_Angle = this.Emboss_Angle;
        }

        #endregion


        #region Straighten

        /// <summary> Straighten's IsOn. </summary>
        public bool Straighten_IsOn;
        /// <summary> Straighten's angle. </summary>
        public float Straighten_Angle = 0;
        /// <summary> Starting straighten's angle. </summary>
        public float StartingStraighten_Angle { get; private set; }
        /// <summary> Cache straighten. </summary>
        public void CacheStraighten()
        {
            this.StartingStraighten_Angle = this.Straighten_Angle;
        }

        #endregion


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned effect. </returns>
        public Effect Clone()
        {
            return new Effect
            {
                //GaussianBlur
                GaussianBlur_IsOn = this.GaussianBlur_IsOn,
                GaussianBlur_Radius = this.GaussianBlur_Radius,
                GaussianBlur_BorderMode = this.GaussianBlur_BorderMode,
                
                //DirectionalBlur
                DirectionalBlur_IsOn = this.DirectionalBlur_IsOn,
                DirectionalBlur_Radius = this.DirectionalBlur_Radius,
                DirectionalBlur_Angle = this.DirectionalBlur_Angle,
                DirectionalBlur_BorderMode = this.DirectionalBlur_BorderMode,

                //Sharpen
                Sharpen_IsOn = this.Sharpen_IsOn,
                Sharpen_Amount = this.Sharpen_Amount,

                //OuterShadow
                OuterShadow_IsOn = this.OuterShadow_IsOn,
                OuterShadow_Radius = this.OuterShadow_Radius,
                OuterShadow_Opacity = this.OuterShadow_Opacity,
                OuterShadow_Color = this.OuterShadow_Color,

                OuterShadow_Offset = this.OuterShadow_Offset,
                OuterShadow_Angle = this.OuterShadow_Angle,
                OuterShadow_Position = this.OuterShadow_Position,

                //Edge
                Edge_IsOn = this.Edge_IsOn,
                Edge_Amount = this.Edge_Amount,
                Edge_Radius = this.Edge_Radius,

                //Morphology
                Morphology_IsOn = this.Morphology_IsOn,
                Morphology_Size = this.Morphology_Size,

                //Emboss
                Emboss_IsOn = this.Emboss_IsOn,
                Emboss_Radius = this.Emboss_Radius,
                Emboss_Angle = this.Emboss_Angle,

                //Straighten
                Straighten_IsOn = this.Straighten_IsOn,
                Straighten_Angle = this.Straighten_Angle,
            };
        }

        //@Static
        /// <summary>
        /// Gets a specific rended-layer.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        /// <param name="image"> The source image. </param>
        /// <returns> The rendered image. </returns>
        public static ICanvasImage Render(Effect effect, ICanvasImage image)
        {
            //GaussianBlur
            if (effect.GaussianBlur_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.GaussianBlurEffect
                {
                    Source = image,
                    BlurAmount = effect.GaussianBlur_Radius,
                    BorderMode = effect.GaussianBlur_BorderMode,
                };
            }

            //DirectionalBlur
            if (effect.DirectionalBlur_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.DirectionalBlurEffect
                {
                    Source = image,
                    BlurAmount = effect.DirectionalBlur_Radius,
                    Angle = -effect.DirectionalBlur_Angle,
                    BorderMode = effect.DirectionalBlur_BorderMode,
                };
            }

            //Sharpen
            if (effect.Sharpen_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.SharpenEffect
                {
                    Source = image,
                    Amount = effect.Sharpen_Amount,
                };
            }

            //OuterShadow
            if (effect.OuterShadow_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.CompositeEffect
                {
                    Sources =
                {
                     new Microsoft.Graphics.Canvas.Effects.Transform2DEffect
                     {
                          TransformMatrix = Matrix3x2.CreateTranslation(effect.OuterShadow_Position),
                          Source = new Microsoft.Graphics.Canvas.Effects.OpacityEffect
                          {
                              Opacity = effect.OuterShadow_Opacity,
                              Source = new Microsoft.Graphics.Canvas.Effects.ShadowEffect
                              {
                                   Source = image,
                                   BlurAmount = effect.OuterShadow_Radius,
                                   ShadowColor = effect.OuterShadow_Color,
                              }
                          }
                     },
                    image
                }
                };
            }

            //Edge
            if (effect.Edge_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.EdgeDetectionEffect
                {
                    Source = image,
                    Amount = effect.Edge_Amount,
                    BlurAmount = effect.Edge_Radius,
                };
            }

            //Morphology
            if (effect.Morphology_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.MorphologyEffect
                {
                    Source = image,
                    Mode = effect.Morphology_Mode,
                    Width = effect.Morphology_Width,
                    Height = effect.Morphology_Height,
                };
            }

            //Emboss
            if (effect.Emboss_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.EmbossEffect
                {
                    Source = image,
                    Amount = effect.Emboss_Radius,
                    Angle = -effect.Emboss_Angle,
                };
            }

            //Straighten
            if (effect.Straighten_IsOn)
            {
                image = new StraightenEffect
                {
                    Source = image,
                    MaintainSize = true,
                    Angle = effect.Straighten_Angle,
                };
            }

            return image;
        }
    }
}
