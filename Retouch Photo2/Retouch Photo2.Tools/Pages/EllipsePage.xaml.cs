using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "EllipseTool"/>.
    /// </summary>
    public sealed partial class EllipsePage : Page
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "EllipsePage" />'s ToolTip IsOpen. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "EllipsePage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(EllipsePage), new PropertyMetadata(false));

        #endregion

        //@Construct
        public EllipsePage()
        {
            this.InitializeComponent();
        }
    }
}