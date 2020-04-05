using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    public sealed partial class AppbarAccentCancelButton : UserControl
    {
        //@Content
        /// <summary> RootButton. </summary>
        public Button RootButton => this._RootButton;

        //@Construct
        public AppbarAccentCancelButton()
        {
            this.InitializeComponent();
        }
    }
}