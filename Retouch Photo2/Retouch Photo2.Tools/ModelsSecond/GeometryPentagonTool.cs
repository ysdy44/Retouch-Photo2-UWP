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
    /// <see cref="ITool"/>'s GeometryPentagonTool.
    /// </summary>
    public class GeometryPentagonTool : IGeometryTool
    {
        //@Override
        public override IGeometryLayer CreateGeometryLayer(Transformer transformer)
        {
            return new GeometryPentagonLayer();
        }
        public override ToolType Type => ToolType.GeometryPentagon;
        public override FrameworkElement Icon { get; } = new GeometryPentagonIcon();
        public override IToolButton Button { get; } = new GeometryPentagonButton();
        public override IToolPage Page { get; } = new GeometryPentagonPage();
    }
}