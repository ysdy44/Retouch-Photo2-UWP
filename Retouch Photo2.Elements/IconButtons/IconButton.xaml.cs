using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Icon and Text.
    /// </summary>
    public sealed partial class IconButton : UserControl
    {
        //@Content
        /// <summary> Icon. </summary>
        public UIElement Icon { get => this.Border.Child; set => this.Border.Child = value; }
        /// <summary> TextBlock' Text. </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }


        //@VisualState
        bool _vsIsEnabled;
        ClickMode _vsClickMode;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsEnabled == false) return this.Disabled;

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
        public IconButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) =>
            {
                this._vsIsEnabled = base.IsEnabled;
                this.VisualState = this.VisualState;//State
            };
            this.IsEnabledChanged += (s, e) =>
            {
                this._vsIsEnabled = base.IsEnabled;
                this.VisualState = this.VisualState;//State
            };

            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }
    }
}