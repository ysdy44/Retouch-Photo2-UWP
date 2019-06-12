using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.TestApp.Tools.Controls;
using Retouch_Photo2.TestApp.Tools.Pages;
using Retouch_Photo2.TestApp.ViewModels;
using Retouch_Photo2.Transformers;

namespace Retouch_Photo2.TestApp.Tools.Models
{
    /// <summary>
    /// <see cref="ICreateTool"/>'s EllipseTool.
    /// </summary>
    public class EllipseTool : ICreateTool
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        //@Override
        public override Layer CreateLayer(Transformer transformer) => new EllipseLayer
        {
            IsChecked = true,
            FillColor = this.ViewModel.FillColor,
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