﻿using System;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Enter number.
    /// </summary>
    public sealed partial class NumberControl : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the value changes. </summary>
        public event EventHandler<double> ValueChange;

        /// <summary> Get or set value. </summary>
        public double Value
        {
            get => this.Slider.Value;
            set => this.Slider.Value = value;
        }

        //@Construct
        public NumberControl()
        {
            this.InitializeComponent();

            this.TextBox.TextChanged += (s, e) =>
            {
                double? valueNull = this.ToNumber(this.TextBox.Text);
                if (valueNull is double value)
                {
                    this.Value = value;
                }
            };
            this.Slider.ValueChanged += (s, e) =>
            {
                this.TextBox.Text = e.NewValue.ToString();

                this.ValueChange?.Invoke(this, e.NewValue); //Delegate
            };
        }

        public double? ToNumber(string text) => this.IsNumeric(text) ? double.Parse(text) : (double?)null;
        public bool IsNumeric(string str) => int.TryParse(str, out int _integer);
    }
}