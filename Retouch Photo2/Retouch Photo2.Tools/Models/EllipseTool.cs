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
    /// <see cref="ICreateTool"/>'s EllipseTool.
    /// </summary>
    public class EllipseTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Override
        public override Layer CreateLayer(Transformer transformer) => new EllipseLayer
        {
            IsChecked = true,
            FillColor = this.SelectionViewModel.FillColor,
            TransformerMatrix = new TransformerMatrix(transformer)
        };

        //@Construct
        public EllipseTool()
        {
            base.Type = ToolType.Ellipse;
            base.Icon = new EllipseControl();
            base.ShowIcon = new EllipseControl();
            base.Page = new EllipsePage();
        }
    }
}