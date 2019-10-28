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
    /// <see cref="ITool"/>'s GeometryCookieTool.
    /// </summary>
    public class GeometryCookieTool : ICreateTool
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Override
        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryCookieLayer
            {
                InnerRadius = this.SelectionViewModel.GeometryCookieInnerRadius,
                SweepAngle = this.SelectionViewModel.GeometryCookieSweepAngle,
            };
        }
        public override ToolType Type => ToolType.GeometryCookie;
        public override FrameworkElement Icon { get; } = new GeometryCookieIcon();
        public override IToolButton Button { get; } = new GeometryCookieButton();
        public override IToolPage Page { get; } = new GeometryCookiePage();
    }
}