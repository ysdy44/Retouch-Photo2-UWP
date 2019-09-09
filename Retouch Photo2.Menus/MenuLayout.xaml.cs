using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Layout of IMenu.
    /// </summary>
    public partial class MenuLayout : UserControl
    {
        //@Content
        public string Text { set => this.TextBlock.Text = value; get => this.TextBlock.Text; }
        public UIElement Icon { set => this.IconViewbox.Child = value; get => this.IconViewbox.Child; }
        public UIElement ContentChild { set => this.ContentBorder.Child = value; get => this.ContentBorder.Child; }
                
        public UIElement StateButton => this._StateButton;
        public UIElement CloseButton => this._CloseButton;
        public UIElement TitlePanel => this._TitlePanel;

        
        public MenuState State
        {
            set
            {
                switch (value)
                {
                    case MenuState.FlyoutHide:
                    case MenuState.FlyoutShow:
                        this._CloseButton.Visibility = Visibility.Collapsed;
                        break;
                    case MenuState.RootExpanded:
                    case MenuState.RootNotExpanded:
                        this._CloseButton.Visibility = Visibility.Visible;
                        break;
                }

                switch (value)
                {
                    case MenuState.FlyoutHide:
                    case MenuState.RootNotExpanded:
                        {
                            this.StateIcon.Glyph = "\uE196";
                            this.ContentBorder.Visibility = Visibility.Collapsed;
                        }
                        break;
                    case MenuState.FlyoutShow:
                    case MenuState.RootExpanded:
                        {
                            this.StateIcon.Glyph = "\uE141";
                            this.ContentBorder.Visibility = Visibility.Visible;
                        }
                        break;
                }
            }
        }
                  

        //@Construct
        public MenuLayout()
        {
            this.InitializeComponent();

            //Storyboard
            this.StoryboardBorder.SizeChanged += (s, e) =>
            {
                if (this.StoryboardBorder.Visibility == Visibility.Collapsed) return;

                this.Frame.Value = e.NewSize.Height;
                this.Storyboard.Begin();
            };
        }
    }
}