using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.ITool;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;

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
        public override Layer CreateLayer(Transformer transformer) => new AcrylicLayer
        {
            IsChecked = true,
            TintColor = this.SelectionViewModel.FillColor,
            TransformerMatrix = new TransformerMatrix(transformer)
            {
                DisabledRadian= true//DisabledRadian
            }
        };

        //@Construct
        public AcrylicTool()
        {
            base.Type = ToolType.Acrylic;
            base.Icon = new AcrylicControl();
            base.ShowIcon = new AcrylicControl();
            base.Page = new AcrylicPage();
        }
    }
}