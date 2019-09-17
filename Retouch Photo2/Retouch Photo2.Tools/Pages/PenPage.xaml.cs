using FanKit.Transformers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
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
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        CurveLayer CurveLayer => this.SelectionViewModel.CurveLayer;


        //@Content
        /// <summary> PenPage's Flyout. </summary>
        public PenFlyout PenFlyout => this._penFlyout;


        //@Converter
        private Visibility FalseToVisibilityConverter(bool value) => value ? Visibility.Collapsed : Visibility.Visible;
        private Visibility TrueToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;

        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }


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