using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class ExpandAppbarRepeatButton : UserControl, IExpandAppbarElement
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
        bool _vsIsSecondPage;        
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsSelected)
                {
                    this.ShowStoryboard.Begin();//Storyboard

                    if (this._vsIsSecondPage == false)
                        return this.Selected;
                    else
                        return this.SecondSelected;
                }
                else
                {
                    this.HideStoryboard.Begin();//Storyboard

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


        #region DependencyProperty

        /// <summary> Gets or sets whether ToggleButton is selected.. </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ExpandAppbarRepeatButton.IsChecked" /> dependency property. </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ExpandAppbarRepeatButton), new PropertyMetadata(false, (sender, e) =>
        {
            ExpandAppbarRepeatButton con = (ExpandAppbarRepeatButton)sender;

            if (e.NewValue is bool value)
            {
                con._vsIsSelected = value;
                con.VisualState = con.VisualState;//State
            }
        }));

        #endregion


        //@Construct
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