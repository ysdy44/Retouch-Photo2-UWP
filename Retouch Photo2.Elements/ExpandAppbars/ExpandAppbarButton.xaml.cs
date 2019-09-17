using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class ExpandAppbarButton : UserControl, IExpandAppbarElement
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
                switch (value)
                {
                    case ClickMode.Release: VisualStateManager.GoToState(this, this.Normal.Name, false); break;
                    case ClickMode.Hover: VisualStateManager.GoToState(this, this.PointerOver.Name, false); break;
                    case ClickMode.Press: VisualStateManager.GoToState(this, this.Pressed.Name, false); break;
                }
            }
        }


        //@Construct
        public ExpandAppbarButton()
        {
            this.InitializeComponent();
            this.Width = this.ExpandWidth;
            this.RootGrid.PointerEntered += (s, e) =>
            {
                this.State = ClickMode.Hover;
            };
            this.RootGrid.PointerPressed += (s, e) =>
            {
                this.State = ClickMode.Press;
            };
            this.RootGrid.PointerExited += (s, e) =>
            {
                this.State = ClickMode.Release;
            };
        }
    }
}