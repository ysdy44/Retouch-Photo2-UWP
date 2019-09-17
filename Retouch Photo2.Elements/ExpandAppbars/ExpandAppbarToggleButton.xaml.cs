using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class ExpandAppbarToggleButton : UserControl, IExpandAppbarElement
    {
        //@Content
        public object CenterContent { get => this.ContentPresenter.Content; set => this.ContentPresenter.Content = value; }
        public double ExpandWidth => 40.0d;
        public FrameworkElement Self => this;


        /// <summary> State of <see cref="ExpandAppbarButton"/>. </summary>
        public ClickMode State
        {
            set
            {

                if (this.IsSelected)
                {
                    VisualStateManager.GoToState(this, this.Selected.Name, false);
                    return;
                }

                switch (value)
                {
                    case ClickMode.Release: VisualStateManager.GoToState(this, this.Normal.Name, false); break;
                    case ClickMode.Hover: VisualStateManager.GoToState(this, this.PointerOver.Name, false); break;
                    case ClickMode.Press: VisualStateManager.GoToState(this, this.Pressed.Name, false); break;
                }
            }
        }
        bool IsSelected = false;


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
                con.SetIsSelected(value);
            }
        }));

        #endregion


        //@Construct
        public ExpandAppbarToggleButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.SetIsSelected(this.IsChecked);
            this.RootGrid.PointerEntered += (s, e) => this.State = ClickMode.Hover;
            this.RootGrid.PointerPressed += (s, e) => this.State = ClickMode.Press;
            this.RootGrid.PointerExited += (s, e) => this.State = ClickMode.Release;
            this.RootGrid.Tapped += (s, e) => this.IsChecked = !this.IsChecked;
        }


        public void SetIsSelected(bool isSelected)
        {
            this.IsSelected = isSelected;
            this.State = ClickMode.Release;
        }
    }
}