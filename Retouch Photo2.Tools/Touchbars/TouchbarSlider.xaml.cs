using HSVColorPickers;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Represents the TouchBar that is used to adjust value.
    /// Touch slider, It has three events : Started, Delta and Completed.
    /// </summary>
    public sealed partial class TouchbarSlider : TouchSliderBase
    {

        //@Content
        /// <summary> Get the RootGrid. </summary>
        public override Grid RootGrid => this._RootGrid;
        /// <summary> Get the left GridLength. </summary>
        public override ColumnDefinition LeftGridLength => this._LeftGridLength;
        /// <summary> Get the center GridLength. </summary>
        public override ColumnDefinition CenterGridLength => this._CenterGridLength;
        /// <summary> Get the right GridLength. </summary>
        public override ColumnDefinition RightGridLength => this._RightGridLength;
        
        //@Construct
        /// <summary>
        /// Construct a TouchbarSlider.
        /// </summary>
        public TouchbarSlider()
        {
            this.InitializeComponent();
            base.InitializeComponent();
        }
    }
}