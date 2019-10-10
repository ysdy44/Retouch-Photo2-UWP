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
    /// <see cref="ITool"/>'s GeometryPieTool.
    /// </summary>
    public class GeometryPieTool : IGeometryTool
    {
        //@Override
        public override IGeometryLayer CreateGeometryLayer(Transformer transformer)
        {
            return new GeometryPieLayer();
        }
        public override ToolType Type => ToolType.GeometryPie;
        public override FrameworkElement Icon { get; } = new GeometryPieIcon();
        public override IToolButton Button { get; } = new GeometryPieButton();
        public override Page Page { get; } = new GeometryPiePage();
    }
}