using HSVColorPickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents the TouchBar that is used to adjust value.
    /// The Button.
    /// </summary>
    public sealed partial class TouchbarButton : UserControl
    {

        //@Static  
        /// <summary> A border, contains a <see cref="TouchbarButton.TouchbarPicker"/>. </summary>
        public static Border TouchbarPickerBorder = new Border();
        /// <summary> A border, contains a <see cref="TouchbarButton.TouchbarSlider"/>. </summary>
        public static Border TouchbarSliderBorder = new Border();
        /// <summary> Instance </summary>
        public static TouchbarButton Instance
        {
            get => TouchbarButton.instance;
            set
            {
                if (TouchbarButton.instance == value)
                {
                    TouchbarButton.TouchbarPickerBorder.Child = value;
                    TouchbarButton.TouchbarSliderBorder.Child = value;

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

                    TouchbarButton.TouchbarPickerBorder.Child = value.TouchbarPicker;
                    TouchbarButton.TouchbarSliderBorder.Child = value.TouchbarSlider;
                }
                else
                {
                    TouchbarButton.TouchbarPickerBorder.Child = value;
                    TouchbarButton.TouchbarSliderBorder.Child = value;
                }

                TouchbarButton.instance = value;
            }
        }
        private static TouchbarButton instance;


        //@Content
        /// <summary> Gets or sets the picker of <see cref = "TouchbarButton" />. </summary>
        public NumberPicker TouchbarPicker { get; set; }
        /// <summary> Gets or sets the slider of <see cref = "TouchbarButton" />. </summary>
        public TouchSliderBase TouchbarSlider { get; set; }


        #region DependencyProperty


        /// <summary> Get or set the content. </summary>
        public object CenterContent
        {
            get { return (object)GetValue(CenterContentProperty); }
            set { SetValue(CenterContentProperty, value); }
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