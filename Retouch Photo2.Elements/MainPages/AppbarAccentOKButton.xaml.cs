using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    public sealed partial class AppbarAccentOKButton : UserControl
    {
        //@Content
        /// <summary> RootButton. </summary>
        public Button RootButton => this._RootButton;

        //@Construct
        public AppbarAccentOKButton()
        {
            this.InitializeComponent();
        }
    }
}