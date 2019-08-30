using FanKit.Transformers;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.Tools.Touchbar;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        NodeCollection NodeCollection => this.SelectionViewModel.CurveLayer.NodeCollection;

        //@Content
        /// <summary> PenPage's Touchbar. </summary>
        public PenTouchbar Touchbar => this._touchbar;

        //@Converter
        private Visibility FalseToVisibilityConverter(bool value) => value ? Visibility.Collapsed : Visibility.Visible;
        private Visibility TrueToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;
        
        //@Construct
        public PenPage()
        {
            this.InitializeComponent();
            this.MoreButton.Tapped += (s, e) => this.Flyout.ShowAt(this);

            this.RemoveButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.CurveLayer == null) return;
                NodeCollection.RemoveCheckedNodes(this.NodeCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
            this.AddButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.CurveLayer == null) return;
                NodeCollection.Interpolation(this.NodeCollection);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.SharpButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.CurveLayer == null) return;
                NodeCollection.SharpCheckedNodes(this.NodeCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
            this.SmoothButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.CurveLayer == null) return;
                NodeCollection.SmoothCheckedNodes(this.NodeCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }
}