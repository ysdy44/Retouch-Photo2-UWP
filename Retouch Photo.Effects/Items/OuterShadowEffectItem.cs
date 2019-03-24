using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo.Effects.Items
{
    public class OuterShadowEffectItem : EffectItem
    {
        public float Radius;
        public float Opacity;
        public Color Color;

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
        private float angle;
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

        public OuterShadowEffectItem()
        {
            this.Reset();
        }

        public override void Reset()
        {
            this.Radius = 0;
            this.Opacity = 0.5f;
            this.Color = Colors.Black;
            this.Offset = 0;
            this.Angle = 0.78539816339744830961566084581988f;// 1/4 π
        }
        public override ICanvasImage GetRender(ICanvasImage image)
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
