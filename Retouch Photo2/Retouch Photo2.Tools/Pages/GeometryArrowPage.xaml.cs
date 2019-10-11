using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryArrowTool"/>.
    /// </summary>
    public sealed partial class GeometryArrowPage : Page, IToolPage
    {
        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        //@Construct
        public GeometryArrowPage()
        {
            this.InitializeComponent();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}