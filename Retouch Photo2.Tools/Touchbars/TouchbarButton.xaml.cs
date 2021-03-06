// Core:              ★
// Referenced:   ★★★
// Difficult:         ★★★
// Only:              ★
// Complete:      ★★★★
using HSVColorPickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Represents a button that is used to adjust value on touch-bar.
    /// </summary>
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(PointerOver), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(Pressed), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(Selected), GroupName = nameof(CommonStates))]
    [ContentProperty(Name = nameof(Content))]
    public sealed partial class TouchbarButton  : ContentControl
    {
        
        //@Static  
        /// <summary> A border, contains a <see cref="TouchbarButton.Picker"/>. </summary>
        public static Border PickerBorder = new Border();
        /// <summary> A border, contains a <see cref="TouchbarButton.Slider"/>. </summary>
        public static Border SliderBorder = new Border();
        /// <summary> Instance </summary>
        public static TouchbarButton Instance
        {
            get => instance;
            set
            {
                if (instance == value) return;

                //The current tool becomes the active button.
                if (instance != null)
                {
                    instance.IsSelected = false;
                    PickerBorder.Child = null;
                    SliderBorder.Child = null;
                }

                instance = value;

                //The current tool does not become an active button.
                if (instance != null)
                {
                    instance.IsSelected = true;
                    PickerBorder.Child = value.Picker;
                    SliderBorder.Child = value.Slider;
                }
                else
                {
                    //instance.IsSelected = true;
                    PickerBorder.Child = null;
                    SliderBorder.Child = null;
                }
            }
        }
        private static TouchbarButton instance;


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


        #region DependencyProperty


        /// <summary> Gets or sets the picker of <see cref = "TouchbarButton" />. </summary>
        public NumberPicker Picker
        {
            get => (NumberPicker)base.GetValue(PickerProperty);
            set => base.SetValue(PickerProperty, value);
        }
        /// <summary> Identifies the <see cref = "TouchbarButton.Picker" /> dependency property. </summary>
        public static readonly DependencyProperty PickerProperty = DependencyProperty.Register(nameof(Picker), typeof(NumberPicker), typeof(TouchbarButton), new PropertyMetadata(null));


        /// <summary> Gets or sets the slider of <see cref = "TouchbarButton" />. </summary>
        public TouchSliderBase Slider
        {
            get => (TouchSliderBase)base.GetValue(SliderProperty);
            set => base.SetValue(SliderProperty, value);
        }
        /// <summary> Identifies the <see cref = "TouchbarButton.Slider" /> dependency property. </summary>
        public static readonly DependencyProperty SliderProperty = DependencyProperty.Register(nameof(Slider), typeof(TouchSliderBase), typeof(TouchbarButton), new PropertyMetadata(null));


        #endregion


        VisualStateGroup CommonStates;
        VisualState Normal;
        VisualState PointerOver;
        VisualState Pressed;
        VisualState Selected;


        //@Construct
        /// <summary>
        /// Initializes a TouchbarButton.
        /// </summary>
        public TouchbarButton()
        {
            this.DefaultStyleKey = typeof(TouchbarButton);
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;

            this.Tapped += (s, e) =>
            {
                TouchbarButton.Instance = this.IsSelected ? null : this;
            };
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CommonStates = base.GetTemplateChild(nameof(CommonStates)) as VisualStateGroup;
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.PointerOver = base.GetTemplateChild(nameof(PointerOver)) as VisualState;
            this.Pressed = base.GetTemplateChild(nameof(Pressed)) as VisualState;
            this.Selected = base.GetTemplateChild(nameof(Selected)) as VisualState;
            this.VisualState = this.VisualState;//State
        }

    }
}