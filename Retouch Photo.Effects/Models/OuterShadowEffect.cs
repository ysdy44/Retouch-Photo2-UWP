using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Pages;
using System;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo.Effects.Models
{
    public class OuterShadowEffect : Effect
    {
        public OuterShadowPage page = new OuterShadowPage();

        public OuterShadowEffect()
        {
            base.Type = EffectType.OuterShadow;
            base.Icon = new OuterShadowControl();
            base.Page = this.page;
        }

        public override EffectItem GetItem(EffectManager effectManager) => effectManager.OuterShadowEffectItem;
        public override void SetPage(EffectManager effectManager) => this.page.EffectManager = effectManager;
        public override void Reset(EffectManager effectManager)
        {
            this.page.EffectManager = null;

            effectManager.OuterShadowEffectItem.Radius = 0;
            effectManager.OuterShadowEffectItem.Opacity = 0.5f;
            effectManager.OuterShadowEffectItem.Color = Colors.Black;
            effectManager.OuterShadowEffectItem.Offset = 0;
            effectManager.OuterShadowEffectItem.Angle = 0.78539816339744830961566084581988f;// 1/4 π
        }
    }


    public class OuterShadowEffectItem : EffectItem
    {
        public float Radius;
        public float Opacity = 0.5f;
        public Color Color=Colors.Black;

        private float offset;
        public float Offset
        {
            get => this.offset;
            set
            {
                this.Position = new Vector2((float)Math.Cos(this.Angle), (float)Math.Sin(this.Angle)) * value; 
                this.offset = value;
            }
        }
        private float angle = 0.78539816339744830961566084581988f;// 1/4 π
        public float Angle
        {
            get => this.angle;
            set
            {
                this.Position = new Vector2((float)Math.Cos(value), (float)Math.Sin(value)) * this.Offset; 
                this.angle = value; 
            }
        }
        Vector2 Position;

        public override ICanvasImage Render(ICanvasImage image)
        {
            return new CompositeEffect
            {
                Sources =
                {
                     new Transform2DEffect
                     {
                          TransformMatrix = Matrix3x2.CreateTranslation(this.Position),
                          Source = new OpacityEffect
                          {
                              Opacity =this.Opacity,
                              Source=new ShadowEffect
                              {
                                   Source = image,
                                   BlurAmount = this.Radius,
                                   ShadowColor = this.Color,
                              }
                          }
                     },
                    image
                }
            };
        }
    }
}


