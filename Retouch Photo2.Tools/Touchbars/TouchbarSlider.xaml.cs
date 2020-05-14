using HSVColorPickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Represents the TouchBar that is used to adjust value.
    /// Touch slider, It has three events : Started, Delta and Completed.
    /// </summary>
    public sealed partial class TouchbarSlider : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the value change starts. </summary>
        public event TouchValueChangeHandler ValueChangeStarted;
        /// <summary> Occurs when value change. </summary>
        public event TouchValueChangeHandler ValueChangeDelta;
        /// <summary> Occurs when the value change is complete. </summary>
        public event TouchValueChangeHandler ValueChangeCompleted;
        /// <summary> Occurs when the value changes. </summary>
        public event ValueChangeHandler NumberChange;
                 

        #region DependencyProperty


        /// <summary> Get or set the current value for a NumberPicker. </summary>
        public int Number
        {
            get => this.NumberPicker.Value;
            set => this.NumberPicker.Value = value;
        }

        /// <summary> Get or set the minimum desirable Value for range elements. </summary>
        public int NumberMinimum
        {
            get => this.NumberPicker.Minimum;
            set => this.NumberPicker.Minimum = value;
        }

        /// <summary> Get or set the maximum desirable Value for range elements. </summary>
        public int NumberMaximum
        {
            get => this.NumberPicker.Maximum;
            set => this.NumberPicker.Maximum = value;
        }
        
        /// <summary> Get or set the string Unit for range elements. </summary>
        public string Unit
        {
            get => this.NumberPicker.Unit;
            set => this.NumberPicker.Unit = value;
        }


        private double value;
        private double _Value
        {
            get
            {
                double scale = this.offset / (this.Canvas.ActualWidth - this.Ellipse.ActualWidth);
                double value = scale * (this.Maximum - this.Minimum) + this.Minimum;
                if (value < this.Minimum) return this.Minimum;
                if (value > this.Maximum) return this.Maximum;
                return value;
            }
            set => this.value = value;
        }

        /// <summary> Get or set the current value for a TouchbarSlider. </summary>
        public double Value
        {
            get => this.value;
            set
            {
                double scale = (value - this.Minimum) / (this.Maximum - this.Minimum);
                double width = scale * (this.Canvas.ActualWidth - this.Ellipse.ActualWidth);
                if (width < 0) width = 0;
                Canvas.SetLeft(this.Ellipse, width);

                this.value = value;
            }
        }


        /// <summary> Get or set the thumb offset. </summary>
        public double Offset
        {
            set
            {
                double width = this.offset;
                if (this.offset < 0) width = 0;
                if (this.offset > (this.Canvas.ActualWidth - this.Ellipse.ActualWidth)) width = (this.Canvas.ActualWidth - this.Ellipse.ActualWidth);
                Canvas.SetLeft(this.Ellipse, width);
            }
        }
        private double offset;


        /// <summary> Get or set the minimum desirable Value for range elements. </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TouchbarSlider.Minimum" /> dependency property. </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(NumberPicker), new PropertyMetadata(0.0));


        /// <summary> Get or set the maximum desirable Value for range elements. </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TouchbarSlider.Minimum" /> dependency property. </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(NumberPicker), new PropertyMetadata(100.0));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TouchbarSlider.
        /// </summary>
        public TouchbarSlider()
        {
            this.InitializeComponent();

            this.NumberPicker.ValueChange += (s, value) => this.NumberChange?.Invoke(this, value);//Delegate

            this.Loaded += (s, e) => this.Value = this.value;
            this.SizeChanged += (s, e) => this.Value = this.value;

            this.Value = this.value;

            this.Thumb.CanDrag = true;
            this.Thumb.DragStarted += (sender, e) =>
            {
                this.Offset = this.offset = e.HorizontalOffset - this.Ellipse.ActualWidth / 2;
                this.value = this._Value;
                this.ValueChangeStarted?.Invoke(this, this.value);//Delegate
            };
            this.Thumb.DragDelta += (sender, e) =>
            {
                this.Offset = this.offset += e.HorizontalChange;
                this.value = this._Value;
                this.ValueChangeDelta?.Invoke(this, this.value);//Delegate
            };
            this.Thumb.DragCompleted += (sender, e) =>
            {
                this.value = this._Value;
                this.ValueChangeCompleted?.Invoke(this, this.value);//Delegate
            };
        }
    }
}