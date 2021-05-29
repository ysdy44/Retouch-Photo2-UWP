// Core:              ★
// Referenced:   ★★★
// Difficult:         ★★★
// Only:              ★
// Complete:      ★★★★
using HSVColorPickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Provides constant and static methods 
    /// for gets and sets to touch-bar's picker and slider.
    /// </summary>
    public static class TouchbarExtension
    {

        //@Static  
        /// <summary> A border, contains a <see cref="TouchbarExtension.Picker"/>. </summary>
        public static Border PickerBorder { get; set; }
        /// <summary> A border, contains a <see cref="TouchbarExtension.Slider"/>. </summary>
        public static Border SliderBorder { get; set; }
        /// <summary> Instance </summary>
        public static SelectorItem Instance
        {
            get => TouchbarExtension.instance;
            set
            {
                if (TouchbarExtension.instance == value)
                {
                    if (TouchbarExtension.instance != null)
                    {
                        TouchbarExtension.instance.IsSelected = false;
                        TouchbarExtension.instance = null;
                    }

                    TouchbarExtension.PickerBorder.Child = null;
                    TouchbarExtension.SliderBorder.Child = null;

                    return;
                }

                // The current tool becomes the active button.
                if (TouchbarExtension.instance != null)
                {
                    TouchbarExtension.instance.IsSelected = false;
                    TouchbarExtension.PickerBorder.Child = null;
                    TouchbarExtension.SliderBorder.Child = null;
                }

                TouchbarExtension.instance = value;

                // The current tool does not become an active button.
                if (TouchbarExtension.instance != null)
                {
                    TouchbarExtension.instance.IsSelected = true;
                    TouchbarExtension.PickerBorder.Child = TouchbarExtension.GetPicker(value);
                    TouchbarExtension.SliderBorder.Child = TouchbarExtension.GetSlider(value);
                }
            }
        }
        private static SelectorItem instance;


        #region Attached DependencyProperty


        private const string Picker = "Picker";
        /// <summary> Gets the picker of <see cref = "TouchbarExtension" />. </summary>
        public static NumberPicker GetPicker(DependencyObject obj) => (NumberPicker)obj.GetValue(PickerProperty);
        /// <summary> Sets the picker of <see cref = "TouchbarExtension" />. </summary>
        public static void SetPicker(DependencyObject obj, NumberPicker value) => obj.SetValue(PickerProperty, value);
        /// <summary> Identifies the <see cref = "TouchbarExtension.Picker" /> dependency property. </summary>
        public static readonly DependencyProperty PickerProperty = DependencyProperty.Register(nameof(Picker), typeof(NumberPicker), typeof(SelectorItem), new PropertyMetadata(null));


        private const string Slider = "Slider";
        /// <summary> Gets the slider of <see cref = "TouchbarExtension" />. </summary>
        public static TouchSliderBase GetSlider(DependencyObject obj) => (TouchSliderBase)obj.GetValue(SliderProperty);
        /// <summary> Sets the slider of <see cref = "TouchbarExtension" />. </summary>
        public static void SetSlider(DependencyObject obj, TouchSliderBase value) => obj.SetValue(SliderProperty, value);
        /// <summary> Identifies the <see cref = "TouchbarExtension.Slider" /> dependency property. </summary>
        public static readonly DependencyProperty SliderProperty = DependencyProperty.Register(nameof(Slider), typeof(TouchSliderBase), typeof(SelectorItem), new PropertyMetadata(null));


        #endregion

    }
}