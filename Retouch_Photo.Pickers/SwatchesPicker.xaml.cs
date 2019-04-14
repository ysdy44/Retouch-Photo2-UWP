using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo.Pickers
{
    public sealed partial class SwatchesPicker : UserControl, IPicker
    {
        //Delegate
        public event ColorChangeHandler ColorChange = null;

        public Color Color = Color.FromArgb(255, 255, 255, 255);
        public Color GetColor() => this.Color;
        public void SetColor(Color value) => this.Color = value;

        int Count;
        private int index;
        public int Index
        {
            get => this.index;
            set
            {
                value %= this.Count;
                value += this.Count;
                value %= this.Count;

                if (value != this.index)
                {
                    Color[] colors = this.Collection[value].Colors;

                    for (int i = 0; i < this.Count; i++)
                    {
                        this.Brushs[i].Color = colors[i];
                    }
                }

                this.index = value;
            }
        }

        double CurrentX;
        RainbowSize Size = new RainbowSize();

        SolidColorBrush[] Brushs;
        Swatches[] Collection = new Swatches[]
        {
            new Swatches(Color.FromArgb(255,0,0,0),true),
            new Swatches(Color.FromArgb(255,192,0,0)),
            new Swatches(Color.FromArgb(255,255,0,0)),
            new Swatches(Color.FromArgb(255,254,68,1)),
            new Swatches(Color.FromArgb(255,255,192,0)),
            new Swatches(Color.FromArgb(255,255,255,0)),
            new Swatches(Color.FromArgb(255,146,208,80)),
            new Swatches(Color.FromArgb(255,86,197,1)),
            new Swatches(Color.FromArgb(255,0,176,80)),
            new Swatches(Color.FromArgb(255,6,192,197)),
            new Swatches(Color.FromArgb(255,0,176,240)),
            new Swatches(Color.FromArgb(255,0,112,192)),
            new Swatches(Color.FromArgb(255,0,32,96)),
            new Swatches(Color.FromArgb(255,112,48,160)),
            new Swatches(Color.FromArgb(255,255,64,196)),
            new Swatches(Color.FromArgb(255,254,14,111)),
        };

        public SwatchesPicker()
        {
            this.InitializeComponent();
            this.Count = this.Collection.Count();
            this.Index = 0;

            //Brush
            IEnumerable<SolidColorBrush> brushs = from item in this.Collection[this.Index].Colors select new SolidColorBrush(item);
            this.Brushs = brushs.ToArray();
            this.GridView.ItemsSource = from item in this.Brushs select this.BuildRectangle(item);

            //Manipulation
            this.CanvasControl.ManipulationMode = ManipulationModes.TranslateX;
            this.CanvasControl.ManipulationStarted += (s, e) => this.CurrentX = e.Position.X;
            this.CanvasControl.ManipulationDelta += (s, e) =>
            {
                this.CurrentX += e.Delta.Translation.X;
                this.Index = this.Size.Index((float)this.CurrentX);
                this.CanvasControl.Invalidate();
            };

            //Draw
            this.CanvasControl.SizeChanged += (s, e) => this.Size.Change((float)e.NewSize.Width, (float)e.NewSize.Height, this.Count);
            this.CanvasControl.Draw += (sender, args) =>
            {
                args.DrawingSession.FillRectangle(this.Size.ItemBackgroundX, this.Size.ItemBackgroundY, this.Size.ItemBackgroundWidth, this.Size.ItemBackgroundHeight, Windows.UI.Colors.Gray);
                for (int i = 0; i < this.Count; i++)
                {
                    args.DrawingSession.FillRectangle(this.Size.ItemX(i), this.Size.ItemY, this.Size.ItemWidth, this.Size.ItemHeight, Collection[i].Color);
                }

                Swatches current = this.Collection[this.Index];
                args.DrawingSession.FillRectangle(this.Size.CurrentBackgroundX(this.Index), this.Size.CurrentBackgroundY, this.Size.CurrentBackgroundWidth, this.Size.CurrentBackgroundHeight, Windows.UI.Colors.Gray);
                args.DrawingSession.FillRectangle(this.Size.CurrentX(this.Index), this.Size.CurrentY, this.Size.CurrentWidth, this.Size.CurrentHeight, current.Color);
            };

            //Wheel
            this.CanvasControl.PointerWheelChanged += (s, e) =>
            {
                if (e.GetCurrentPoint(this.CanvasControl).Properties.MouseWheelDelta > 0) this.Index++;
                else this.Index--;
                this.CanvasControl.Invalidate();
            };
        }

        private Rectangle BuildRectangle(SolidColorBrush brush)
        {
            Rectangle rectangle = new Rectangle
            {
                Fill = brush,
                Width = 44,
                Height = 44,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            rectangle.Tapped += (s, e) =>
            {
                this.Color = brush.Color;
                this.ColorChange?.Invoke(this, brush.Color);
            };

            return rectangle;
        }
    }


    public class Swatches
    {
        public Color Color;
        public Color[] Colors;

        public Swatches(Color color, bool isGray = false, int count = 16)
        {
            this.Color = color;
            this.Colors = isGray ? this.GetGrayColors(count) : this.GetColorfulColors(color, count);
        }

        private Color[] GetGrayColors(int count)
        {
            Color[] colors = new Color[count];

            double span = 255 / count;

            for (int i = 0; i < count; i++)
            {
                byte c = (byte)(255 - i * span);
                colors[i] = Color.FromArgb(255, c, c, c);
            }

            return colors;
        }

        private Color[] GetColorfulColors(Color color, int count)
        {
            Color[] colors = new Color[count];

            double h = HSL.RGBtoHSL(color).H;
            double span = 100 / count;

            for (int i = 0; i < count; i++)
            {
                double l = 100 - i * span;
                double s = i % 4 * 20 + 20;
                colors[i] = HSL.HSLtoRGB(255, h, s, l);
            }

            return colors;
        }
    }


    public class RainbowSize
    {
        public readonly float Span = 4;
        public readonly float thiscikneee = 1;

        //ItemBackground
        public float ItemBackgroundX => this.Span;
        public float ItemBackgroundY => this.Span;
        public float ItemBackgroundWidth;
        public float ItemBackgroundHeight;
        //Item
        public float ItemX(int index) => this.Span + this.thiscikneee + index * this.ItemWidth;
        public float ItemY => this.Span + this.thiscikneee;
        public float ItemWidth;
        public float ItemHeight;

        //Current
        public float CurrentX(int index) => this.thiscikneee + index * this.ItemWidth;
        public float CurrentY => this.thiscikneee;
        public float CurrentWidth;
        public float CurrentHeight;
        // CurrentBackground
        public float CurrentBackgroundX(int i) => i * this.ItemWidth;
        public float CurrentBackgroundY = 0;
        public float CurrentBackgroundWidth;
        public float CurrentBackgroundHeight;

        public int Index(float x) => (int)((x - this.Span - this.thiscikneee) / this.ItemWidth);

        public void Change(float width, float height, int count)
        {
            this.ItemBackgroundWidth = width - this.Span - this.Span;
            this.ItemBackgroundHeight = height - this.Span - this.Span;

            this.ItemWidth = (this.ItemBackgroundWidth - this.thiscikneee - this.thiscikneee) / count;
            this.ItemHeight = (this.ItemBackgroundHeight - this.thiscikneee - this.thiscikneee);

            this.CurrentWidth = this.ItemWidth + this.Span + this.Span;
            this.CurrentHeight = height - this.thiscikneee - this.thiscikneee;

            this.CurrentBackgroundWidth = this.CurrentWidth + this.thiscikneee + this.thiscikneee;
            this.CurrentBackgroundHeight = height;
        }
    }
}
