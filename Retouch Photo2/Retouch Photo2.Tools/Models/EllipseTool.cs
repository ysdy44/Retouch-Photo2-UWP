using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ICreateTool"/>'s EllipseTool.
    /// </summary>
    public class EllipseTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        
        //@Override
        public override ILayer CreateLayer(Transformer transformer) => new EllipseLayer
        {
            IsChecked = true,
            FillBrush = new Brush
            {
                Type = BrushType.Color,
                Color = this.SelectionViewModel.FillColor,
            },

            Source = transformer,
            Destination = transformer,
        };

        public override bool IsOpen { set { this._ellipsePage.IsOpen = value; } }
        public override ToolType Type => ToolType.Ellipse;
        public override FrameworkElement Icon { get; } = new EllipseIcon();
        public override FrameworkElement ShowIcon { get; } = new EllipseIcon();
        public override Page Page => this._ellipsePage;
        EllipsePage _ellipsePage { get; } = new EllipsePage();
    }
}