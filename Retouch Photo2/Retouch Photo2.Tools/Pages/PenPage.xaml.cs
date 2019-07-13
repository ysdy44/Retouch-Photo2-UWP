using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Tips;
using System;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "PenTool"/>.
    /// </summary>
    public sealed partial class PenPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Converter
        private Visibility FalseToVisibilityConverter(bool value) => value ? Visibility.Collapsed : Visibility.Visible;
        private Visibility TrueToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;

        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "PenPage" />'s AddOrNodes. </summary>
        public bool AddOrNodes
        {
            get { return (bool)GetValue(AddOrNodesProperty); }
            set { SetValue(AddOrNodesProperty, value); }
        }
        /// <summary> Identifies the <see cref = "PenPage.AddOrNodes" /> dependency property. </summary>
        public static readonly DependencyProperty AddOrNodesProperty = DependencyProperty.Register(nameof(AddOrNodes), typeof(bool), typeof(PenPage), new PropertyMetadata(false));

        #endregion

        //@Construct
        public PenPage()
        {
            this.InitializeComponent();
        }
    }
}