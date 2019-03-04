using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Pickers
{
    public class Square
    {
        public Vector2 Center = new Vector2(50, 50);
        public float Width = 100;
        public float Height = 100;
        public float HalfWidth => this.Width / 2;
        public float HalfHeight => this.Height / 2;
        public float StrokePadding = 12;
    }

    public partial class PalettePicker : UserControl, IPicker
    {
        //Delegate
        public event ColorChangeHandler ColorChange = null;
        public Color GetColor() => HSL.HSLtoRGB(this.HSL);
        public void SetColor(Color value) => this.HSL = HSL.RGBtoHSL(value);


        #region DependencyProperty


        private HSL hsl = new HSL { A = 255, H = 0, S = 1, L = 1 };
        private HSL _HSL
        {
            get => this.hsl;
            set
            {
                this.ColorChange?.Invoke(this, HSL.HSLtoRGB(value.A, value.H, value.S, value.L));

                this.hsl = value;
            }
        }
        public HSL HSL
        {
            get => this.hsl;
            set
            {
                this.Action(value);
                this.hsl = value;

                this.CanvasControl.Invalidate();
            }
        }
        public Color Color
        {
            get => this.GetColor();
            set => this.SetColor(value);
        }


        #endregion



        bool IsPalette = false;
        Vector2 Vector;
        Action<HSL> Action;
        Square Square = new Square();


        public PalettePicker(PaletteBase paletteBase)
        {
            this.InitializeComponent();

            //Picker
            this.Slider.Minimum = paletteBase.Minimum;
            this.Slider.Maximum = paletteBase.Maximum;

            this.Slider.Value = paletteBase.GetValue(this.hsl);
            this.LinearGradientBrush.GradientStops = paletteBase.GetSliderBrush(this.hsl);

            this.Slider.ValueChangeDelta += (sender, value) => this.HSL = this._HSL = paletteBase.GetHSL(this.hsl, value);

            //Action
            this.Action = (HSL hsl) =>
            {
                this.Slider.Value = paletteBase.GetValue(hsl);
                this.LinearGradientBrush.GradientStops = paletteBase.GetSliderBrush(hsl);
            };

            //Canvas
            this.CanvasControl.SizeChanged += (sender, e) =>
            {
                this.Square.Center = e.NewSize.ToVector2() / 2;

                this.Square.Width = (float)e.NewSize.Width - this.Square.StrokePadding * 2;
                this.Square.Height = (float)e.NewSize.Height - this.Square.StrokePadding * 2;
            };
            this.CanvasControl.Draw += (sender, args) => paletteBase.Draw(this.CanvasControl, args.DrawingSession, this.hsl, this.Square.Center, this.Square.HalfWidth, this.Square.HalfHeight);



            //Manipulation
            this.CanvasControl.ManipulationMode = ManipulationModes.All;
            this.CanvasControl.ManipulationStarted += (sender, e) =>
            {
                this.Vector = e.Position.ToVector2() - this.Square.Center;

                this.IsPalette = Math.Abs(Vector.X) < this.Square.Width && Math.Abs(this.Vector.Y) < this.Square.Height;

                if (this.IsPalette) this.HSL = this._HSL = paletteBase.Delta(this.hsl, this.Vector, this.Square.HalfWidth, this.Square.HalfHeight);
            };
            this.CanvasControl.ManipulationDelta += (sender, e) =>
            {
                this.Vector += e.Delta.Translation.ToVector2();

                if (this.IsPalette) this.HSL = this._HSL = paletteBase.Delta(this.hsl, this.Vector, this.Square.HalfWidth, this.Square.HalfHeight);
            };
            this.CanvasControl.ManipulationCompleted += (sender, e) => this.IsPalette = false;



            this.CanvasControl.Invalidate();
        }


    }
}
