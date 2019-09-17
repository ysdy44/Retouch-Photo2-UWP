using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class ExpandAppbarRepeatButton : UserControl, IExpandAppbarElement
    {
        //@Content
        public object CenterContent { get => this.ContentPresenter.Content; set => this.ContentPresenter.Content = value; }
        public double ExpandWidth => 40.0d;
        public FrameworkElement Self => this;


        /// <summary> State of <see cref="ExpandAppbarButton"/>. </summary>
        public bool State
        {
            set
            {
                if (value)
                {
                    VisualStateManager.GoToState(this, this.Selected.Name, false);
                }
                else
                {
                    VisualStateManager.GoToState(this, this.UnSelected.Name, false);
                }
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
                con.State = value;
            }
        }));

        #endregion


        //@Construct
        public ExpandAppbarRepeatButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.State = this.IsChecked;
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