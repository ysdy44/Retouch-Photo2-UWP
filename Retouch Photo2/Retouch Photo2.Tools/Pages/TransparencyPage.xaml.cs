using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "TransparencyTool"/>.
    /// </summary>
    public sealed partial class TransparencyPage : Page
    {
        //@Converter
        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }

        //@Construct
        public TransparencyPage()
        {
            this.InitializeComponent();
        }
    }
}