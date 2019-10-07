using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class ExpandAppbarButton : UserControl, IExpandAppbarElement
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
        

        //@Construct
        public ExpandAppbarButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
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