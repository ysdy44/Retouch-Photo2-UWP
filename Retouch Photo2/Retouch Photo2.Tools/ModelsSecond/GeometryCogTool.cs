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
    /// <see cref="ITool"/>'s GeometryCogTool.
    /// </summary>
    public class GeometryCogTool : IGeometryTool
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Override
        public override IGeometryLayer CreateGeometryLayer(Transformer transformer)
        {
            return new GeometryCogLayer
            {
                Count = this.SelectionViewModel.GeometryCogCount,
                InnerRadius = this.SelectionViewModel.GeometryCogInnerRadius,
                Tooth = this.SelectionViewModel.GeometryCogTooth,
                Notch = this.SelectionViewModel.GeometryCogNotch,
            };
        }
        public override ToolType Type => ToolType.GeometryCog;
        public override FrameworkElement Icon { get; } = new GeometryCogIcon();
        public override IToolButton Button { get; } = new GeometryCogButton();
        public override IToolPage Page { get; } = new GeometryCogPage();
    }
}