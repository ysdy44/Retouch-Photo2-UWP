using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Retouch_Photo2 Tools 's Button.
    /// </summary>
    public sealed partial class ToolSecondButton : UserControl
    {
        //@Content
        /// <summary> Button's IsSelected. </summary>
        public bool IsSelected
        {
            set
            {
                if (this._vsIsSelected == value) return;

                this._vsIsSelected = value;
                this.VisualState = this.VisualState;//State
            }
        }
        /// <summary> ContentPresenter's Content. </summary>
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }
        /// <summary> TextBlock' Text. </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        

        //@VisualState
        bool _vsIsSelected;
        ClickMode _vsClickMode;
        public VisualState VisualState
        {
            get
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


        //@Construct
        public ToolSecondButton()
        {
            this.InitializeComponent();
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }
    }
}