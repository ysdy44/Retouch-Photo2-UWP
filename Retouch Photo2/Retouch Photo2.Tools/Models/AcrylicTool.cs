using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
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

            TransformManager = new TransformManager
            {
                Source = transformer,
                Destination = transformer,
                DisabledRadian = true//DisabledRadian
            }
        };

        public override bool IsSelected
        {
            set
            {
                this.Button.IsSelected = value;
                this._acrylicPage.IsSelected = value;
            }
        }
        public override ToolType Type => ToolType.Acrylic;
        public override FrameworkElement Icon { get; } = new AcrylicIcon();
        public override IToolButton Button { get; } = new AcrylicButton();
        public override Page Page => this._acrylicPage;
        AcrylicPage _acrylicPage { get; } = new AcrylicPage();
    }
}