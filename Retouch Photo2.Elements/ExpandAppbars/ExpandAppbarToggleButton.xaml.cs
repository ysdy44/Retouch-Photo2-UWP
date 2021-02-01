// Core:              
// Referenced:   ★
// Difficult:         
// Only:              
// Complete:      
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// ToggleButton of <see cref="ExpandAppbar"/>.
    /// </summary>
    public sealed partial class ExpandAppbarToggleButton : UserControl, IExpandAppbarElement
    {
        //@Content
        /// <summary> TextBlock's Text </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        /// <summary> ContentPresenter's Content </summary>
        public object CenterContent { get => this.ContentPresenter.Content; set => this.ContentPresenter.Content = value; }
        /// <summary> Gets element width. </summary>
        public double ExpandWidth => 40.0d;
        /// <summary> Gets it yourself. </summary>
        public FrameworkElement Self => this;


        //@VisualState
        bool _vsIsChecked;
        ClickMode _vsClickMode;
        bool _vsIsSecondPage;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsSecondPage == false)
                {
                    if (this._vsIsChecked) return this.Selected;

                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Normal;
                        case ClickMode.Hover: return this.PointerOver;
                        case ClickMode.Press: return this.Pressed;
                    }
                    return this.Normal;
                }
                else
                {
                    if (this._vsIsChecked) return this.SecondSelected;

                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Second;
                        case ClickMode.Hover: return this.SecondPointerOver;
                        case ClickMode.Press: return this.SecondPressed;
                    }
                    return this.Second;
                }
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
        public bool IsSecondPage
        {
            set
            {
                this._vsIsSecondPage = value;
                this.VisualState = this.VisualState;//State
            }
        }


        #region DependencyProperty

        /// <summary> Gets or sets whether ToggleButton is selected.. </summary>
        public bool IsChecked
        {
            get => (bool)base.GetValue(IsCheckedProperty);
            set => base.SetValue(IsCheckedProperty, value);
        }
        /// <summary> Identifies the <see cref = "ExpandAppbarToggleButton.IsChecked" /> dependency property. </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ExpandAppbarToggleButton), new PropertyMetadata(false, (sender, e) =>
        {
            ExpandAppbarToggleButton control = (ExpandAppbarToggleButton)sender;

            if (e.NewValue is bool value)
            {
                control._vsIsChecked = value;
                control.VisualState = control.VisualState;//State
            }
        }));

        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ExpandAppbarToggleButton.
        /// </summary>
        public ExpandAppbarToggleButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) =>
            {
                this._vsIsChecked = this.IsChecked;
                this.VisualState = this.VisualState;//State
            };
            this.Tapped += (s, e) => this.IsChecked = !this.IsChecked;

            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }
    }
}