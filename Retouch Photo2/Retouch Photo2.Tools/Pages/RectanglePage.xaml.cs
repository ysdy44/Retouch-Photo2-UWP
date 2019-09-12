using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "RectangleTool"/>.
    /// </summary>
    public sealed partial class RectanglePage : Page
    {
        //@ViewModel
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        
        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "RectanglePage" />'s ToolTip IsOpen. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "RectanglePage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(RectanglePage), new PropertyMetadata(false));

        #endregion

        //@Construct
        public RectanglePage()
        {
            this.InitializeComponent();
        }
    }
}