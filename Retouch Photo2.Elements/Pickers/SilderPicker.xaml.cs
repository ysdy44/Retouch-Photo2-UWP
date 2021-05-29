// Core:              
// Referenced:   ★★★
// Difficult:         ★
// Only:              ★
// Complete:      ★★
using HSVColorPickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Manipulation of the slider.
    /// </summary>
    public sealed partial class SliderPicker : TouchSliderBase
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


        //@VisualState
        bool _vsIsEnabled = true;
        ClickMode _vsClickMode;
        bool _vsIsManipulationStarted;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsEnabled == false) return this.Disabled;

                if (this._vsIsManipulationStarted) return this.Pressed;

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
                this.VisualState = this.VisualState; // State
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a SliderPicker. 
        /// </summary>
        public SliderPicker()
        {
            this.InitializeComponent();
            base.InitializeComponent();

            this.IsEnabledChanged += (s, e) => this.IsEnabledChange();
            this.Loaded += (s, e) => this.IsEnabledChange();

            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerPressed += (s, e) =>
            {
                base.CapturePointer(e.Pointer);
                this.ClickMode = ClickMode.Press;
            };
            this.PointerReleased += (s, e) =>
            {
                base.ReleasePointerCapture(e.Pointer);
                this.ClickMode = ClickMode.Release;
            };


            // Manipulation
            //htis.RootGrid.ManipulationMode = ManipulationModes.All;
            this.RootGrid.ManipulationStarted += (sender, e) =>
            {
                this._vsIsManipulationStarted = true;
                this.VisualState = this.VisualState; // VisualState
            };
            this.RootGrid.ManipulationDelta += (sender, e) =>
            {
            };
            this.RootGrid.ManipulationCompleted += (sender, e) =>
            {
                this._vsIsManipulationStarted = false;
                this.VisualState = this.VisualState; // VisualState
            };
        }

        private void IsEnabledChange()
        {
            this._vsIsEnabled = this.IsEnabled;
            this.VisualState = this.VisualState; // State
        }

    }
}