using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo.Pickers
{
    public sealed partial class TouchSlider : UserControl
    {


        #region DependencyProperty


        private double value;
        private double _Value
        {
            get
            {
                double scale = this.offset / (this.Border.ActualWidth - this.Ellipse.ActualWidth);
                double value = scale * (this.Maximum - this.Minimum) + this.Minimum;
                if (value < this.Minimum) return this.Minimum;
                if (value > this.Maximum) return this.Maximum;
                return value;
            }
            set
            {
                this.ValueChange?.Invoke(this, value);

                this.value = value;
            }
        }
        public double Value
        {
            get => value;
            set
            {
                double scale = (value - this.Minimum) / (this.Maximum - this.Minimum);
                double width = scale * (this.Border.ActualWidth - this.Ellipse.ActualWidth);
                if (width < 0) width = 0;
                Canvas.SetLeft(this.Ellipse, width);

                this.value = value;
            }
        }


        private double offset;
        public double Offset
        {
            set
            {
                double width = this.offset;
                if (this.offset < 0) width = 0;
                if (this.offset > (this.Border.ActualWidth - this.Ellipse.ActualWidth)) width = (this.Border.ActualWidth - this.Ellipse.ActualWidth);
                Canvas.SetLeft(this.Ellipse, width);
            }
        }


        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(NumberPicker), new PropertyMetadata(0.0));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(NumberPicker), new PropertyMetadata(100.0));


        public UIElement SliderBackground { get => this.Border.Child; set => this.Border.Child = value; }
        public Brush SliderBrush { get => this.Border.Background; set => this.Border.Background = value; }


        #endregion


        //event
        public delegate void TouchValueChangeHandler(object sender, double value);
        public event TouchValueChangeHandler ValueChange;


        public TouchSlider()
        {
            this.InitializeComponent();

            this.Thumb.CanDrag = true;
            this.Thumb.DragStarted += (sender, e) =>
            {
                this.Offset = this.offset = e.HorizontalOffset - this.Ellipse.ActualWidth / 2;
                this._Value = this.value = this._Value;
            };
            this.Thumb.DragDelta += (sender, e) =>
            {
                this.Offset = this.offset += e.HorizontalChange;
                this._Value = this.value = this._Value;
            };
            this.Thumb.DragCompleted += (sender, e) =>
            {
            };

            this.Value = this.Value;
            this.Loaded += (sender, e) => this.Value = this.Value;
        }


    }
}
