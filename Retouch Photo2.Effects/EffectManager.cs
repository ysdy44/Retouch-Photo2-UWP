using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Effects
{
    /// <summary> 
    /// <see cref = "Effect" />'s manager. 
    /// </summary>
    public class EffectManager
    {
        //@Static
        /// <summary>
        /// Indicates that the contents of the CanvasControl need to be redrawn.
        /// </summary>
        /// <param name="mode"> invalidate mode </param>
        public static void Invalidate(Action<EffectManager> action) => EffectManager.InvalidateAction?.Invoke(action);
        /// <summary> <see cref = "Action" /> of the <see cref = "EffectManager" />. </summary>
        public static Action<Action<EffectManager>> InvalidateAction { private get; set; }
        

        //GaussianBlur
        public bool GaussianBlur_IsOn;
        public float GaussianBlur_BlurAmount=0;

        //DirectionalBlur
        public bool DirectionalBlur_IsOn;
        public float DirectionalBlur_BlurAmount=0;
        public float DirectionalBlur_Angle=0;

        //Sharpen
        public bool Sharpen_IsOn;
        public float Sharpen_Amount = 0;

        //OuterShadow
        public bool OuterShadow_IsOn;
        public float OuterShadow_Radius = 0;
        public float OuterShadow_Opacity = 0.5f;
        public Color OuterShadow_Color = Colors.Black;

        private float OuterShadow_offset = 0;
        public float OuterShadow_Offset
        {
            get => this.OuterShadow_offset;
            set
            {
                this.OuterShadow_Position = new Vector2((float)Math.Cos(this.OuterShadow_Angle), (float)Math.Sin(this.OuterShadow_Angle)) * value;
                this.OuterShadow_offset = value;
            }
        }
        private float OuterShadow_angle= 0.78539816339744830961566084581988f;// 1/4 π
        public float OuterShadow_Angle
        {
            get => this.OuterShadow_angle;
            set
            {
                this.OuterShadow_Position = new Vector2((float)Math.Cos(value), (float)Math.Sin(value)) * this.OuterShadow_Offset;
                this.OuterShadow_angle = value;
            }
        }
        Vector2 OuterShadow_Position = Vector2.Zero;

        //Outline
        public bool Outline_IsOn;
        private int Outline_size=1;
        public int Outline_Size
        {
            get => this.Outline_size;
            set
            {
                this.Outline_Mode = (value > 0) ? MorphologyEffectMode.Dilate : MorphologyEffectMode.Erode;

                int s = Math.Abs(value);
                this.Outline_Height = this.Outline_Width = s > 90 ? 90 : s;

                this.Outline_size = value;
            }
        }

        public MorphologyEffectMode Outline_Mode { get; private set; }
        public int Outline_Width { get; private set; } = 1;
        public int Outline_Height { get; private set; } = 1;

        //Emboss
        public bool Emboss_IsOn;
        public float Emboss_Amount = 1;
        public float Emboss_Angle = 0;
        
        //Straighten
        public bool Straighten_IsOn;
        public float Straighten_Angle = 0;

        //@Static
        public static ICanvasImage Render(EffectManager  effectManager, ICanvasImage image)
        {
            //GaussianBlur
            if (effectManager.GaussianBlur_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.GaussianBlurEffect
                {
                    Source = image,
                    BlurAmount = effectManager.GaussianBlur_BlurAmount
                };
            }

            //DirectionalBlur
            if (effectManager.DirectionalBlur_IsOn)
            {
                image= new Microsoft.Graphics.Canvas.Effects.DirectionalBlurEffect
                {
                    Source = image,
                    BlurAmount = effectManager.DirectionalBlur_BlurAmount,
                    Angle = -effectManager.DirectionalBlur_Angle,
                };
            }

            //Sharpen
            if (effectManager.Sharpen_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.SharpenEffect
                {
                    Source = image,
                    Amount = effectManager.Sharpen_Amount,
                };
            }

            //OuterShadow
            if (effectManager.OuterShadow_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.CompositeEffect
                {
                    Sources =
                {
                     new Microsoft.Graphics.Canvas.Effects.Transform2DEffect
                     {
                          TransformMatrix = Matrix3x2.CreateTranslation(effectManager.OuterShadow_Position),
                          Source = new Microsoft.Graphics.Canvas.Effects.OpacityEffect
                          {
                              Opacity =effectManager.OuterShadow_Opacity,
                              Source=new Microsoft.Graphics.Canvas.Effects.ShadowEffect
                              {
                                   Source = image,
                                   BlurAmount = effectManager.OuterShadow_Radius,
                                   ShadowColor = effectManager.OuterShadow_Color,
                              }
                          }
                     },
                    image
                }
                };
            }

            //Outline
            if (effectManager.Outline_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.MorphologyEffect
                {
                    Source = image,
                    Mode = effectManager.Outline_Mode,
                    Width = effectManager.Outline_Width,
                    Height = effectManager.Outline_Height,
                };
            }

            //Emboss
            if (effectManager.Emboss_IsOn)
            {
                image = new Microsoft.Graphics.Canvas.Effects.EmbossEffect
                {
                    Source = image,
                    Amount = effectManager.Emboss_Amount,
                    Angle = -effectManager.Emboss_Angle,
                };
            }

            //Straighten
            if (effectManager.Straighten_IsOn)
            {
                image = new StraightenEffect
                {
                    Source = image,
                    MaintainSize = true,
                    Angle = effectManager.Straighten_Angle,
                };
            }

            return image;
        }
    }
}
