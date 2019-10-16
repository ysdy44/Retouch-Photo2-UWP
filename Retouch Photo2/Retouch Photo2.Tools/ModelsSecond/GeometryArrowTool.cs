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
    /// <see cref="ITool"/>'s GeometryArrowTool.
    /// </summary>
    public class GeometryArrowTool : IGeometryTool
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Override
        public override IGeometryLayer CreateGeometryLayer(Transformer transformer)
        {
            return new GeometryArrowLayer
            {
                LeftTail = this.SelectionViewModel.GeometryArrowLeftTail,
                RightTail = this.SelectionViewModel.GeometryArrowRightTail,
            };
        }
        public override ToolType Type => ToolType.GeometryArrow;
        public override FrameworkElement Icon { get; } = new GeometryArrowIcon();
        public override IToolButton Button { get; } = new GeometryArrowButton();
        public override IToolPage Page { get; } = new GeometryArrowPage();
    }
}