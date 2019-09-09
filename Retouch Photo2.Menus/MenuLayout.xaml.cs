using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Content of <see cref="MenuLayout"/>..
    /// </summary>
    public sealed partial class MenuLayout : UserControl
    {
        //@Content
        public string Text { set => this.TextBlock.Text = value; get => this.TextBlock.Text; }
        public UIElement Icon { set => this.IconViewbox.Child = value; get => this.IconViewbox.Child; }
        public UIElement ContentChild { set => this.ContentBorder.Child = value; get => this.ContentBorder.Child; }
                
        public UIElement StateButton => this._StateButton;
        public UIElement CloseButton => this._CloseButton;
        public UIElement TitlePanel => this._TitlePanel;


        #region State

        public MenuState State
        {
            get => this.state;
            set
            {
                switch (value)
                {
                    case MenuState.FlyoutHide:
                        {
                            this.FlyoutOrRoot = true;
                            this.HideOrShow = true;
                        }
                        break;
                    case MenuState.FlyoutShow:
                        {
                            this.FlyoutOrRoot = true;
                            this.HideOrShow = false;
                        }
                        break;
                    case MenuState.RootExpanded:
                        {
                            this.FlyoutOrRoot = false;
                            this.HideOrShow = false;
                        }
                        break;
                    case MenuState.RootNotExpanded:
                        {
                            this.FlyoutOrRoot = false;
                            this.HideOrShow = true;
                        }
                        break;
                }


                this.state = value;
            }
        }
        private MenuState state;

        private bool HideOrShow
        {
            set
            {
                if (value)
                {
                     this.StateIcon.Glyph = "\uE196";
                     this.ContentBorder.Visibility = Visibility.Collapsed;
                }
                else
                {
                     this.StateIcon.Glyph = "\uE141";
                     this.ContentBorder.Visibility = Visibility.Visible;
                }
            }
        }

        private bool FlyoutOrRoot
        {
            set
            {
                if (value)
                {
                    this.StoryboardRectangle.Visibility = Visibility.Collapsed;
                    this._CloseButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.StoryboardRectangle.Visibility = Visibility.Visible;
                    this._CloseButton.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion


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