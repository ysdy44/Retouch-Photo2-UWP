using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryPieTool.
    /// </summary>
    public class GeometryPieTool : ICreateTool
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Override
        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryPieLayer
            {
                InnerRadius = this.SelectionViewModel.GeometryPieInnerRadius,
                SweepAngle = this.SelectionViewModel.GeometryPieSweepAngle,
            };
        }
        public override ToolType Type => ToolType.GeometryPie;
        public override FrameworkElement Icon { get; } = new GeometryPieIcon();
        public override IToolButton Button { get; } = new GeometryPieButton();
        public override IToolPage Page { get; } = new GeometryPiePage();
    }
}