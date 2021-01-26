using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// RepeatButton of <see cref="ExpandAppbar"/>.
    /// </summary>
    public sealed partial class ExpandAppbarRepeatButton : UserControl, IExpandAppbarElement
    {
        //@Content
        /// <summary> TextBlock's Text </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        /// <summary> ContentPresenter's Content </summary>
        public object CenterContent { get => this.ContentPresenter.Content; set => this.ContentPresenter.Content = value; }
        /// <summary> Gets element width. </summary>
        public double ExpandWidth => 50.0d;
        /// <summary> Gets it yourself. </summary>
        public FrameworkElement Self => this;


        bool _isShowStoryboard = true;
        private void Storyboard(bool isSelected)
        {
            if (this._isShowStoryboard != isSelected)
            {
                this._isShowStoryboard = isSelected;

                if (isSelected)
                    this.ShowStoryboard.Begin();//Storyboard
                else
                    this.HideStoryboard.Begin();//Storyboard
            }
        }


        //@VisualState
        bool _vsIsSelected;
        bool _vsIsSecondPage;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsSelected)
                {
                    this.Storyboard(true);

                    if (this._vsIsSecondPage == false)
                        return this.Selected;
                    else
                        return this.SecondSelected;
                }
                else
                {
                    this.Storyboard(false);

                    if (this._vsIsSecondPage == false)
                        return this.UnSelected;
                    else
                        return this.SecondUnSelected;
                }
            }
            set
            {
                if (value == null) return;
                
                VisualStateManager.GoToState(this, value.Name, false);
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
        /// <summary> Identifies the <see cref = "ExpandAppbarRepeatButton.IsChecked" /> dependency property. </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ExpandAppbarRepeatButton), new PropertyMetadata(false, (sender, e) =>
        {
            ExpandAppbarRepeatButton control = (ExpandAppbarRepeatButton)sender;

            if (e.NewValue is bool value)
            {
                control._vsIsSelected = value;
                control.VisualState = control.VisualState;//State
            }
        }));

        #endregion


        //@Construct
        /// <summary>
        /// Initializes a ExpandAppbarRepeatButton.
        /// </summary>
        public ExpandAppbarRepeatButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this.IsChecked = true;
                }
            }; 
            this.PointerPressed += (s, e) => this.IsChecked = true;
            this.PointerReleased += (s, e) => this.IsChecked = false;
            this.PointerExited += (s, e) => this.IsChecked = false;
        }
    }
}