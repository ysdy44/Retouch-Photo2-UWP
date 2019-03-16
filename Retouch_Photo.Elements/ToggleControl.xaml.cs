using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Elements
{
    public sealed partial class ToggleControl : UserControl
    {
        #region DependencyProperty


        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(ToggleControl), typeof(ToggleControl), new PropertyMetadata(false, (sender, e) =>
        {
            ToggleControl con = (ToggleControl)sender;

            if (e.NewValue is bool value)
            {
                con.Check(value);
            }
        }));


        public object CenterContent
        {
            get { return (object)GetValue(CenterContentProperty); }
            set { SetValue(CenterContentProperty, value); }
        }
        public static readonly DependencyProperty CenterContentProperty = DependencyProperty.Register(nameof(CenterContent), typeof(ToggleControl), typeof(ToggleControl), new PropertyMetadata(false, (sender, e) =>
        {
            ToggleControl con = (ToggleControl)sender;

            if (e.NewValue is object value)
            {
                con.ContentPresenter.Content = value;
            }
        }));


        #endregion

        //Delegate
        public delegate void CheckedChangedHandler(bool IsChecked);
        public event CheckedChangedHandler CheckedChanged = null;

        public ToggleControl()
        {
            this.InitializeComponent();
            this.ContentPresenter.Tapped += (sender, e) => this.IsChecked = !this.IsChecked;
        }

        private void Check(bool isChecked)
        {
            this.ContentPresenter.Background = IsChecked ? this.AccentColor : this.UnAccentColor;
            this.ContentPresenter.Foreground = IsChecked ? this.CheckColor : this.UnCheckColor;
            
            this.CheckedChanged?.Invoke(isChecked);
        }

    }
}
