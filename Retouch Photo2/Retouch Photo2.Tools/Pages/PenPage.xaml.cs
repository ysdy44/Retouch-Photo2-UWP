using FanKit.Transformers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "PenTool"/>.
    /// </summary>
    public sealed partial class PenPage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        GeometryCurveLayer CurveLayer => this.SelectionViewModel.CurveLayer;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        /// <summary> PenPage's Flyout. </summary>
        public PenFlyout PenFlyout => this._penFlyout;
        
        //@Construct
        public PenPage()
        {
            this.InitializeComponent();
            this.MoreButton.Tapped += (s, e) => this.Flyout.ShowAt(this);

            this.RemoveButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.RemoveCheckedNodes(this.CurveLayer.Nodes);
                this.ViewModel.Invalidate();//Invalidate
            };
            this.AddButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.Interpolation(this.CurveLayer.Nodes);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.SharpButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.SharpCheckedNodes(this.CurveLayer.Nodes);
                this.ViewModel.Invalidate();//Invalidate
            };
            this.SmoothButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.SmoothCheckedNodes(this.CurveLayer.Nodes);
                this.ViewModel.Invalidate();//Invalidate
            };
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}