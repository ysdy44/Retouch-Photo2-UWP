using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Operates
{
    /// <summary>
    /// Button of <see cref="Operate">.
    /// </summary>
    public sealed partial class Button : UserControl
    {
        //@Delegate
        public event TappedEventHandler ButtonTapped;


        //@Content     
        /// <summary> Root button. </summary>
        public Windows.UI.Xaml.Controls.Button RootButton => this._RootButton;
        /// <summary> Enabled icon. </summary>
        public UIElement EnabledIcon { get => this.EnabledViewbox.Child; set => this.EnabledViewbox.Child = value; }
        /// <summary> Disabled icon. </summary>
        public UIElement DisabledIcon { get => this.DisabledViewbox.Child; set => this.DisabledViewbox.Child = value; }


        //@Converter
        private Visibility BoolToVisibleConverter(bool isEnabled) => isEnabled ? Visibility.Visible : Visibility.Collapsed;
        private Visibility BoolToCollapsedConverter(bool isEnabled) => isEnabled ? Visibility.Collapsed : Visibility.Visible;


        //@Construct
        public Button()
        {
            this.InitializeComponent();
            this.RootButton.Tapped += (s, e) => this.ButtonTapped?.Invoke(s, e);
        }
    }
}