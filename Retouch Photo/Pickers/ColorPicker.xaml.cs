using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Pickers
{
    public sealed partial class ColorPicker : UserControl
    {
        //Delegate
        public delegate void ColorChangeHandler(object sender, Color value);
        public event ColorChangeHandler ColorChange = null;

        #region DependencyProperty


        public Color Color
        {
            get => this.SolidColorBrushName.Color;
            set
            {
                if (this.Index != null)
                {
                    int i = this.Index ?? 0;

                     if (i == 0) this.SwatchesPicker.Color =value;
                    if (i == 1) this.WheelPicker.Color = value;
                    if (i == 2) this.RGBPicker.Color = value;
                    if (i == 3) this.HSLPicker.Color = value;
                }

                this.SolidColorBrushName.Color = value;
            }
        }

        private int? index = null;
        public int? Index
        {
            get => index;
            set
            {
                if (value != null)
                {
                    int i = value ?? 0;

                    this.SwatchesButton.IsChecked = i == 0;
                    this.WheelButton.IsChecked = i == 1;
                    this.RGBButton.IsChecked = i == 2;
                    this.HSLButton.IsChecked = i == 3;

                     if (i == 0) this.SwatchesPicker.Color = this.SolidColorBrushName.Color;
                    if (i == 1) this.WheelPicker.Color = this.SolidColorBrushName.Color;
                    if (i == 2) this.RGBPicker.Color = this.SolidColorBrushName.Color;
                    if (i == 3) this.HSLPicker.Color = this.SolidColorBrushName.Color;

                    this.SwatchesPicker.Visibility = i == 0 ? Visibility.Visible : Visibility.Collapsed;
                    this.WheelPicker.Visibility = i == 1 ? Visibility.Visible : Visibility.Collapsed;
                    this.RGBPicker.Visibility = i == 2 ? Visibility.Visible : Visibility.Collapsed;
                    this.HSLPicker.Visibility = i == 3 ? Visibility.Visible : Visibility.Collapsed;
                }
                index = value;
            }
        }
 
        #endregion

        public ColorPicker()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)=> this.Index = this.Index == null ? 0 : this.index;


        //Head
        private void StrawPicker_ColorChangeStarted(object sender, Color value){}
        private void StrawPicker_ColorChangeDelta(object sender, Color value) => this.SolidColorBrushName.Color = value;
        private void StrawPicker_ColorChangeCompleted(object sender, Color value) => Picker_ColorChange(sender, value);


        //Toggle
        private void SwatchesButton_Click(object sender, RoutedEventArgs e) => this.Index = 0;
        private void WheelButton_Click(object sender, RoutedEventArgs e) => this.Index =1;
        private void RGBButton_Click(object sender, RoutedEventArgs e) => this.Index = 2;
        private void HSLButton_Click(object sender, RoutedEventArgs e) => this.Index = 3;


        //Dialog
        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) => sender.Hide();
        private void Ellipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Window.Current.Content is FrameworkElement frameworkElement) this.Dialog.ShowAt(frameworkElement);

            this.AlphaPicker.Color = this.Color;
            this.PalettePicker.Color = this.Color;
        }


        //Body    
        private void Picker_ColorChange(object sender, Color value)
        {
            if (sender is PalettePicker)
            {
                this.AlphaPicker.ColorChange -= Picker_ColorChange; 
                this.AlphaPicker.Color = value;
                this.AlphaPicker.ColorChange += Picker_ColorChange; 
            }
            else if (sender is AlphaPicker)
            {
                this.PalettePicker.ColorChange -= Picker_ColorChange; 
                this.PalettePicker.Color = value;
                this.PalettePicker.ColorChange += Picker_ColorChange; 
            }

            this.SolidColorBrushName.Color = value;
            this.ColorChange?.Invoke(this, value);
        }
    }
}
