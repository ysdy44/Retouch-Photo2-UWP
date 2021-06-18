// Core:              ★★
// Referenced:   
// Difficult:         ★*
// Only:              ★★
// Complete:      ★★
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents a control that the has a colorful star.
    /// </summary>  
    public sealed partial class StarButton : UserControl
    {

        //@Delegate
        public event RoutedEventHandler Click;


        //@VisualState
        bool _vsIsPointerOver = false;
        bool _vsIsPressed = false;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsPressed) return this.Pressed;
                else if (this._vsIsPointerOver) return this.PointerOver;
                else return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value?.Name ?? "Normal", true);
        }
        /// <summary> Gets or sets the state. </summary>
        public bool IsPointerOver
        {
            get => this._vsIsPointerOver;
            private set
            {
                this._vsIsPointerOver = value;
                this.VisualState = this.VisualState; // State
            }
        }
        /// <summary> Gets or sets the state. </summary>
        public bool IsPressed
        {
            get => this._vsIsPressed;
            private set
            {
                this._vsIsPressed = value;
                this.VisualState = this.VisualState; // State
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a StarButton.
        /// </summary>
        public StarButton()
        {
            this.InitializeComponent();
            this.Polygon.PointerEntered += (s, e) => this.IsPointerOver = true;
            this.Polygon.PointerExited += (s, e) => this.IsPointerOver = false;
            this.Polygon.PointerPressed += (s, e) => this.IsPressed = true;
            this.Polygon.PointerReleased += (s, e) =>
            {
                if (this.IsPressed) this.Click?.Invoke(this, new RoutedEventArgs());//Delegate

                this.IsPressed = false;
            };
        }
    }
}