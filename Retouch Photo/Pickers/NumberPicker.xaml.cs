using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


namespace Retouch_Photo.Pickers
{
    public sealed partial class NumberPicker : UserControl
    {

        //Delegate
        public delegate void ValueChangeHandler(object sender, int value);
        public event ValueChangeHandler ValueChange = null;


        #region DependencyProperty


        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =DependencyProperty.Register(nameof(Value), typeof(int), typeof(NumberPicker), new PropertyMetadata(0, new PropertyChangedCallback(ValueOnChang)));
        private static void ValueOnChang(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumberPicker con = (NumberPicker)sender;

            if (e.NewValue is int value)
            {
                con.Button.Content =  value.ToString() + " " + con.Unit;
            }
        }


        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty =DependencyProperty.Register(nameof(Minimum), typeof(int), typeof(NumberPicker), new PropertyMetadata(0));
    

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =DependencyProperty.Register(nameof(Maximum), typeof(int), typeof(NumberPicker), new PropertyMetadata(100));
       


        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        public static readonly DependencyProperty UnitProperty =DependencyProperty.Register(nameof(Unit), typeof(string), typeof(NumberPicker), new PropertyMetadata(string.Empty, new PropertyChangedCallback(UnitOnChang)));
        private static void UnitOnChang(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumberPicker con = (NumberPicker)sender;

            if (e.NewValue is string value)
            {
                con.Button.Content = (con.IsNegative ? "-" : "") + con.Value.ToString() + " " + value;
            }
        }


        #endregion

        #region Property


        private int OldValue { get; set; }

        private int newValue;
        private int NewValue
        {
            get => newValue;
            set
            {
                this.Button.Content = (this.IsNegative ? "-" : "") + value.ToString() + " " + this.Unit;
                newValue = value;
            }
        }

        private bool isNegative;
        private bool IsNegative
        {
            get => isNegative;
            set
            {
                this.Button.Content = (value ? "-" : "") + this.Value.ToString() + " " + this.Unit;
                isNegative = value;
            }
        }


        #endregion


        public NumberPicker()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => this.Button.Content = this.Value.ToString() + " " + this.Unit;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //cache value
            if (this.Value >= 0)
            {
                this.NewValue = this.Value;
                this.OldValue = this.Value;
                this.IsNegative = false;
            }
            else
            {
                this.NewValue = -this.Value;
                this.OldValue = -this.Value;
                this.IsNegative = true;
            }
        }

        //Number
        private void Zero_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue * 10;
        private void One_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue * 10 + 1;
        private void Two_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue * 10 + 2;
        private void Three_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue * 10 + 3;
        private void Four_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue * 10 + 4;
        private void Five_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue * 10 + 5;
        private void Six_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue * 10 + 6;
        private void Seven_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue * 10 + 7;
        private void Eight_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue * 10 + 8;
        private void Nine_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue * 10 + 9;
      
        //Back, Negative
        private void Back_Click(object sender, RoutedEventArgs e) => this.NewValue = this.NewValue / 10;
        private void Negative_Click(object sender, RoutedEventArgs e) => this.IsNegative = !this.IsNegative;
        private void Decimal_Click(object sender, RoutedEventArgs e) => this.NewValue = 0;
 
        //OK, Cancel
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Value = this.GetValue(this.NewValue);
            this.ValueChange?.Invoke(this, this.Value);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Value = this.GetValue(this.OldValue);
            this.ValueChange?.Invoke(this, this.Value);
        }
        private int GetValue(int num)
        {
            this.Flyout.Hide();

            num = Math.Abs(num);
            int value = this.IsNegative ? -num : num;
            if (value > this.Maximum) return this.Maximum;
            if (value < this.Minimum) return this.Minimum;

            return value;
        }

    
    }
}
