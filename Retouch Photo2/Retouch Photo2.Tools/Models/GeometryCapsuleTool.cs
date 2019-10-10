using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.Tools.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryCapsuleTool.
    /// </summary>
    public class GeometryCapsuleTool : IGeometryTool
    {
        //@Override
        public override IGeometryLayer CreateGeometryLayer(Transformer transformer)
        {
            return new GeometryCapsuleLayer();
        }
        public override ToolType Type => ToolType.GeometryCapsule;
        public override FrameworkElement Icon { get; } = new GeometryCapsuleIcon();
        public override IToolButton Button { get; } = new GeometryCapsuleButton();
        public override Page Page { get; } = new GeometryCapsulePage();
    }
}