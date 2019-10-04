using HSVColorPickers;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Slider picker.
    /// </summary>
    public sealed partial class SliderControl : UserControl
    {
        //@Content
        /// <summary> APicker. </summary>
        public NumberPicker APicker => this._APicker;
        /// <summary> ASlider. </summary>
        public TouchSlider ASlider => this._ASlider;
        /// <summary> OPicker. </summary>
        public NumberPicker OPicker => this._OPicker;

        //@Construct
        public SliderControl()
        {
            this.InitializeComponent();
        }
    }
}