using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Content of <see cref="MenuLayout"/>..
    /// </summary>
    public sealed partial class MenuLayoutContent : UserControl
    {
        
        //@Content
        public Button StateButton { set => this._StateButton = value; get => this._StateButton; }
        public Button CloseButton { set => this._CloseButton = value; get => this._CloseButton; }
        public Grid TitlePanel { set => this._TitlePanel = value; get => this._TitlePanel; }
        public Border ContentBorder { set => this._ContentBorder = value; get => this._ContentBorder; }
        public Viewbox IconViewBox { set => this._IconViewBox = value; get => this._IconViewBox; }
        public TextBlock TextBlock { set => this._TextBlock = value; get => this._TextBlock; }
        public FontIcon StateIcon { set => this._StateIcon = value; get => this._StateIcon; }
        public Rectangle StoryboardRectangle { set => this._StoryboardRectangle = value; get => this._StoryboardRectangle; }


        //@Construct
        public MenuLayoutContent()
        {
            this.InitializeComponent();

            //Storyboard
            this.StoryboardBorder.SizeChanged += (s, e) =>
            {
                if (this.StoryboardBorder.Visibility ==  Visibility.Collapsed) return;

                this.Frame.Value = e.NewSize.Height;
                this.Storyboard.Begin();
            };
        }
    }
}