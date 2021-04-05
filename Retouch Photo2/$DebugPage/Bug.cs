using HSVColorPickers;
using System;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2
{
    internal class Bug
    {
        private static readonly Random Random = new Random(123213213);

        public Vector2 Position;
        private Vector2 Flat;
        public Color Color;

        public Bug(int width, int height)
        {
            this.Position = new Vector2(Bug.Random.Next(0, width), Bug.Random.Next(0, height));
            this.Flat = new Vector2(Bug.Random.Next(-100, 100) / 100.0f, Bug.Random.Next(-100, 100) / 100.0f);
            this.UpdateColor();
        }

        public void UpdatePosition(float width, float height)
        {
            if (this.Position.X < 0)
            {
                this.Position.X = 1;
                this.Flat.X = -this.Flat.X;
                this.UpdateColor();
            }
            else if (this.Position.X > width)
            {
                this.Position.X = width - 1;
                this.Flat.X = -this.Flat.X;
                this.UpdateColor();
            }

            if (this.Position.Y < 0)
            {
                this.Position.Y = 1;
                this.Flat.Y = -this.Flat.Y;
                this.UpdateColor();
            }
            else if (this.Position.Y > height)
            {
                this.Position.Y = height - 1;
                this.Flat.Y = -this.Flat.Y;
                this.UpdateColor();
            }

            this.Position += this.Flat;
        }

        private void UpdateColor()
        {
            float h = Math.Abs(this.Flat.X + this.Flat.Y) * 180.0f;
            HSV hsv = new HSV(255, h, 100, 100);
            this.Color = HSV.HSVtoRGB(hsv);
        }
    }
}