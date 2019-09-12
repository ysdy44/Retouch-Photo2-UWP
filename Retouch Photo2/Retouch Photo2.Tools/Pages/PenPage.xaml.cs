using FanKit.Transformers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        CurveLayer CurveLayer => this.SelectionViewModel.CurveLayer;

        //@Content
        /// <summary> PenPage's Flyout. </summary>
        public PenFlyout PenFlyout => this._penFlyout;

        //@Converter
        private Visibility FalseToVisibilityConverter(bool value) => value ? Visibility.Collapsed : Visibility.Visible;
        private Visibility TrueToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;

        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "PenPage" />'s ToolTip IsOpen. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "PenPage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(PenPage), new PropertyMetadata(false));

        #endregion

        //@Construct
        public PenPage()
        {
            this.InitializeComponent();
            this.MoreButton.Tapped += (s, e) => this.Flyout.ShowAt(this);

            this.RemoveButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.RemoveCheckedNodes(this.CurveLayer.NodeCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
            this.AddButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.Interpolation(this.CurveLayer.NodeCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.SharpButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.SharpCheckedNodes(this.CurveLayer.NodeCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
            this.SmoothButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.SmoothCheckedNodes(this.CurveLayer.NodeCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }
}