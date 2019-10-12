using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryDiamondTool.
    /// </summary>
    public class GeometryDiamondTool : IGeometryTool
    {
        //@Override
        public override IGeometryLayer CreateGeometryLayer(Transformer transformer)
        {
            return new GeometryDiamondLayer();
        }
        public override ToolType Type => ToolType.GeometryDiamond;
        public override FrameworkElement Icon { get; } = new GeometryDiamondIcon();
        public override IToolButton Button { get; } = new GeometryDiamondButton();
        public override IToolPage Page { get; } = new GeometryDiamondPage();
    }
}