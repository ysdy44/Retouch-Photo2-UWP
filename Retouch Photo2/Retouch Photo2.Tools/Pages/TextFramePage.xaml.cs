using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "TextFrameTool"/>.
    /// </summary>
    public sealed partial class TextFramePage : Page, IToolPage
    {
        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        //@Converter
        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;

        //@Construct
        public TextFramePage()
        {
            this.InitializeComponent();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}