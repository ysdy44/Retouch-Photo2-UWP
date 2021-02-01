// Core:              ★
// Referenced:   ★★★
// Difficult:         ★★★
// Only:              ★
// Complete:      ★★★★
using HSVColorPickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Represents the TouchBar that is used to adjust value.
    /// The Button.
    /// </summary>
    public sealed partial class TouchbarButton : UserControl
    {

        //@Static  
        /// <summary> A border, contains a <see cref="TouchbarButton.Picker"/>. </summary>
        public static Border PickerBorder = new Border();
        /// <summary> A border, contains a <see cref="TouchbarButton.Slider"/>. </summary>
        public static Border SliderBorder = new Border();
        /// <summary> Instance </summary>
        public static TouchbarButton Instance
        {
            get => TouchbarButton.instance;
            set
            {
                if (TouchbarButton.instance == value)
                {
                    TouchbarButton.PickerBorder.Child = value;
                    TouchbarButton.SliderBorder.Child = value;

                    TouchbarButton.instance = value;
                    return;
                }

                //The current tool becomes the active button.
                TouchbarButton oldButton = TouchbarButton.instance;
                if (oldButton != null) oldButton.IsSelected = false;

                //The current tool does not become an active button.
                TouchbarButton newButton = value;
                if (newButton != null)
                {
                    newButton.IsSelected = true;

                    TouchbarButton.PickerBorder.Child = value.Picker;
                    TouchbarButton.SliderBorder.Child = value.Slider;
                }
                else
                {
                    TouchbarButton.PickerBorder.Child = value;
                    TouchbarButton.SliderBorder.Child = value;
                }

                TouchbarButton.instance = value;
            }
        }
        private static TouchbarButton instance;


        #region DependencyProperty


        /// <summary> Gets or sets the picker of <see cref = "TouchbarButton" />. </summary>
        public NumberPicker Picker
        {
            get => (NumberPicker)base.GetValue(PickerProperty);
            set => base.SetValue(PickerProperty, value);
        }
        /// <summary> Identifies the <see cref = "TouchbarButton.Picker" /> dependency property. </summary>
        public static readonly DependencyProperty PickerProperty = DependencyProperty.Register(nameof(Picker), typeof(NumberPicker), typeof(TouchbarSlider), new PropertyMetadata(null));


        /// <summary> Gets or sets the slider of <see cref = "TouchbarButton" />. </summary>
        public TouchSliderBase Slider
        {
            get => (TouchSliderBase)base.GetValue(SliderProperty);
            set => base.SetValue(SliderProperty, value);
        }
        /// <summary> Identifies the <see cref = "TouchbarButton.Slider" /> dependency property. </summary>
        public static readonly DependencyProperty SliderProperty = DependencyProperty.Register(nameof(Slider), typeof(TouchSliderBase), typeof(TouchbarSlider), new PropertyMetadata(null));


        /// <summary> Get or set the content. </summary>
        public object CenterContent
        {
            get  => (object)base.GetValue(CenterContentProperty);
            set => base.SetValue(CenterContentProperty, value);
        }
        /// <summary> Identifies the <see cref = "TouchbarButton.CenterContent" /> dependency property. </summary>
        public static readonly DependencyProperty CenterContentProperty = DependencyProperty.Register(nameof(CenterContent), typeof(object), typeof(TouchbarSlider), new PropertyMetadata(null));


        #endregion


        //@VisualState
        bool _vsIsSelected;
        ClickMode _vsClickMode;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsSelected) return this.Selected;

                switch (this._vsClickMode)
                {
                    case ClickMode.Release: return this.Normal;
                    case ClickMode.Hover: return this.PointerOver;
                    case ClickMode.Press: return this.Pressed;
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        /// <summary> VisualState's ClickMode. </summary>
        private ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
            }
        }
        /// <summary> Gets or sets the selected state of <see cref = "TouchbarButton" />. </summary>
        public bool IsSelected
        {
            get => this._vsIsSelected;
            set
            {
                this._vsIsSelected = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a TouchbarButton.
        /// </summary>
        public TouchbarButton()
        {
            this.InitializeComponent();
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;

            this.Tapped += (s, e) =>
            {
                TouchbarButton.Instance = this.IsSelected ? null : this;
            };
        }
    }
}