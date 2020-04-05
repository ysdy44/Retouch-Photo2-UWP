using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class ExpandAppbarButton : UserControl, IExpandAppbarElement
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
        ClickMode _vsClickMode;
        bool _vsIsSecondPage;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsSecondPage==false)
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Normal;
                        case ClickMode.Hover: return this.PointerOver;
                        case ClickMode.Press: return this.Pressed;
                    }
                }
                else
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Second;
                        case ClickMode.Hover: return this.SecondPointerOver;
                        case ClickMode.Press: return this.SecondPressed;
                    }
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

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


        //@Construct
        public ExpandAppbarButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }
    }
}