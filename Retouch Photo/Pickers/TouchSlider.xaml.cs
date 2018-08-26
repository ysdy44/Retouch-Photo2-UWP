using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo.Pickers
{
    public sealed partial class TouchSlider : UserControl
    {
        

        #region DependencyProperty


        public double Value { get => this.Slider.Value; set => this.Slider.Value = value; }
        public double Minimum { get => this.Slider.Minimum; set => this.Slider.Minimum = value; }
        public double Maximum { get => this.Slider.Maximum; set => this.Slider.Maximum = value; }

        public Brush SliderForeground { get => this.Slider.Foreground; set => this.Slider.Foreground = value; }
        public Brush SliderBackground { get => this.Slider.Background; set => this.Slider.Background = value; }
        
        

        #endregion


        //event
        private RangeBaseValueChangedEventArgs e;
        public event RangeBaseValueChangedEventHandler ValueChangeStarted;
        public event RangeBaseValueChangedEventHandler ValueChangeDelta;
        public event RangeBaseValueChangedEventHandler ValueChangeCompleted;


        public TouchSlider()
        {
            this.InitializeComponent();
        }


        //Value Changed
        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.e = e;
           this.ValueChangeDelta?.Invoke(sender, this.e);
        }

        //State Changed
        bool IsPressed = false;
        private void CommonStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (this.e != null)
            {
                if (e.NewState.Name == "Pressed")
                {
                    IsPressed = true;
                  this.ValueChangeStarted?.Invoke(sender, this.e);
                }

                if (e.NewState.Name != "Pressed")
                {
                    if (IsPressed == true)
                    {
                        IsPressed = false;
                     this.ValueChangeCompleted?.Invoke(sender, this.e);
                    }
                }
            }
        }


    }
}
