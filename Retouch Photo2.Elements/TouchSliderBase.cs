using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace HSVColorPickers
{
    /// <summary>
    /// Base of <see cref="TouchSlider"/>.
    /// </summary>
    public abstract class TouchSliderBase : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the value change starts. </summary>
        public event TouchValueChangeHandler ValueChangeStarted;
        /// <summary> Occurs when value change. </summary>
        public event TouchValueChangeHandler ValueChangeDelta;
        /// <summary> Occurs when the value change is complete. </summary>
        public event TouchValueChangeHandler ValueChangeCompleted;

        //@Content
        /// <summary> Get the RootGrid. </summary>
        public abstract Grid RootGrid { get; }
        /// <summary> Get the LeftGridLength. </summary>
        public abstract ColumnDefinition LeftGridLength { get; }
        /// <summary> Get the CenterGridLength. </summary>
        public abstract ColumnDefinition CenterGridLength { get; }
        /// <summary> Get the RightGridLength. </summary>
        public abstract ColumnDefinition RightGridLength { get; }


        #region DependencyProperty


        /// <summary> Get or set the current value for a TouchSlider. </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TouchSliderBase.Value" /> dependency property. </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(TouchSliderBase), new PropertyMetadata(0.0, (sender, e) =>
        {
            TouchSliderBase con = (TouchSliderBase)sender;

            if (e.NewValue is double value)
            {
                double proportion = con.ValueToProportionConverter(value);
                con.SetGridLength(proportion);
            }
        }));


        /// <summary> Get or set the minimum desirable Value for range elements. </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TouchSliderBase.Minimum" /> dependency property. </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(NumberPicker), new PropertyMetadata(0.0));


        /// <summary> Get or set the maximum desirable Value for range elements. </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        /// <summary> Identifies the <see cref = "TouchSliderBase.Minimum" /> dependency property. </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(NumberPicker), new PropertyMetadata(100.0));


        #endregion


        double _offset;
        //@Construct
        /// <summary>
        /// Construct a TouchSliderBase.
        /// </summary>
        public void InitializeComponent()
        {
            this.SizeChanged += (s, e) => this.SetGridLength(this.Value);
            this.Loaded += (s, e) => this.SetGridLength(this.Value);

            this.RootGrid.ManipulationMode = ManipulationModes.All;
            this.RootGrid.ManipulationStarted += (sender, e) =>
            {
                this._offset = e.Position.X;
                double proportion = this.OffsetToProportionConverter(this._offset);
                this.Value = this.ProportionToValueConverter(proportion);
                this.ValueChangeStarted?.Invoke(this, this.Value);//Delegate
            };
            this.RootGrid.ManipulationDelta += (sender, e) =>
            {
                this._offset += e.Delta.Translation.X;
                double proportion = this.OffsetToProportionConverter(this._offset);
                this.Value = this.ProportionToValueConverter(proportion);
                this.ValueChangeDelta?.Invoke(this, this.Value);//Delegate
            };
            this.RootGrid.ManipulationCompleted += (sender, e) =>
            {
                double proportion = this.OffsetToProportionConverter(this._offset);
                this.Value = this.ProportionToValueConverter(proportion);
                this.ValueChangeCompleted?.Invoke(this, this.Value);//Delegate
            };
        }


        //@Converter
        private double ValueToProportionConverter(double value)
        {
            double proportion = (value - this.Minimum) / (this.Maximum - this.Minimum);
            if (proportion < 0.0d) return 0.0d;
            if (proportion > 1.0d) return 1.0d;
            return proportion;
        }
        private double OffsetToProportionConverter(double offset)
        {
            double proportion = offset / (this.RootGrid.ActualWidth - this.CenterGridLength.ActualWidth / 2.0d);
            if (proportion < 0.0d) return 0.0d;
            if (proportion > 1.0d) return 1.0d;
            return proportion;
        }
        private double ProportionToValueConverter(double proportion)
        {
            double value = proportion * (this.Maximum - this.Minimum) + this.Minimum;
            if (value < this.Minimum) return this.Minimum;
            if (value > this.Maximum) return this.Maximum;
            return value;
        }


        private void SetGridLength(double proportion)
        {
            if (proportion <= 0)
            {
                this.LeftGridLength.Width = new GridLength(0);
                this.RightGridLength.Width = new GridLength(1, GridUnitType.Star);
            }
            else if (proportion >= 1)
            {
                this.LeftGridLength.Width = new GridLength(1, GridUnitType.Star);
                this.RightGridLength.Width = new GridLength(0);
            }
            else
            {
                this.LeftGridLength.Width = new GridLength(proportion, GridUnitType.Star);
                this.RightGridLength.Width = new GridLength(1.0d - proportion, GridUnitType.Star);
            }
        }
    }
}