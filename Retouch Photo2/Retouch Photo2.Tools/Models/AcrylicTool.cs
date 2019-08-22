using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ICreateTool"/>'s AcrylicTool.
    /// </summary>
    public class AcrylicTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Override
        public override ILayer CreateLayer(Transformer transformer) => new AcrylicLayer
        {
            IsChecked = true,
            TintColor = this.SelectionViewModel.FillColor,

            Source = transformer,
            Destination = transformer,
            DisabledRadian = true//DisabledRadian
        };

        public override ToolType Type => ToolType.Acrylic;
        public override FrameworkElement Icon { get; } = new AcrylicControl();
        public override FrameworkElement ShowIcon { get; } = new AcrylicControl();
        public override Page Page { get; } = new AcrylicPage();
    }
}