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
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public class GeometryRoundRectTool : ICreateTool
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Override
        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryRoundRectLayer
            {
                Corner = this.SelectionViewModel.GeometryRoundRectCorner,
            };
        }
        public override ToolType Type => ToolType.GeometryRoundRect;
        public override FrameworkElement Icon { get; } = new GeometryRoundRectIcon();
        public override IToolButton Button { get; } = new GeometryRoundRectButton();
        public override IToolPage Page { get; } = new GeometryRoundRectPage();
    }
}