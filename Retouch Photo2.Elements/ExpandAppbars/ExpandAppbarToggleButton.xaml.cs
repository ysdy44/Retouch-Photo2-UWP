using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class ExpandAppbarToggleButton : UserControl, IExpandAppbarElement
    {
        //@Content
        /// <summary> TextBlock's Text </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        public object CenterContent { get => this.ContentPresenter.Content; set => this.ContentPresenter.Content = value; }
        public double ExpandWidth => 40.0d;
        public FrameworkElement Self => this;
        public bool IsSecondPage
        {
            set
            {
                this._vsIsSecondPage = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@VisualState
        bool _vsIsSelected;
        ClickMode _vsClickMode;
        bool _vsIsSecondPage;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsSecondPage == false)
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
                else
                {
                    if (this._vsIsSelected) return this.SecondSelected;

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
        

        #region DependencyProperty

        /// <summary> Gets or sets whether ToggleButton is selected.. </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ExpandAppbarToggleButton.IsChecked" /> dependency property. </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ExpandAppbarToggleButton), new PropertyMetadata(false, (sender, e) =>
        {
            ExpandAppbarToggleButton con = (ExpandAppbarToggleButton)sender;

            if (e.NewValue is bool value)
            {
                con._vsIsSelected = value;
                con.VisualState = con.VisualState;//State
            }
        }));

        #endregion


        //@Construct
        public ExpandAppbarToggleButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) =>
            {
                this._vsIsSelected = this.IsChecked;
                this.VisualState = this.VisualState;//State
            };
            this.Tapped += (s, e) => this.IsChecked = !this.IsChecked;

            this.PointerEntered += (s, e) =>
            {
                this._vsClickMode = ClickMode.Hover;
                this.VisualState = this.VisualState;//State
            };
            this.PointerPressed += (s, e) =>
            {
                this._vsClickMode = ClickMode.Press;
                this.VisualState = this.VisualState;//State
            };
            this.PointerPressed += (s, e) =>
            {
                this._vsClickMode = ClickMode.Hover;
                this.VisualState = this.VisualState;//State
            };
            this.PointerExited += (s, e) =>
            {
                this._vsClickMode = ClickMode.Release;
                this.VisualState = this.VisualState;//State
            };
        }
    }
}