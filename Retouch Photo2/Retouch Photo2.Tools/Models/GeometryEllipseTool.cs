using FanKit.Transformers;
using Retouch_Photo2.Brushs;
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
    /// <see cref="ICreateTool"/>'s GeometryEllipseTool.
    /// </summary>
    public class GeometryEllipseTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        
        //@Override
        public override ILayer CreateLayer(Transformer transformer) => new GeometryEllipseLayer
        {
            SelectMode = SelectMode.Selected,
            TransformManager = new TransformManager(transformer),

            FillBrush = new Brush
            {
                Type = BrushType.Color,
                Color = this.SelectionViewModel.FillColor,
            },
        };

        public override ToolType Type => ToolType.GeometryEllipse;
        public override FrameworkElement Icon { get; } = new GeometryEllipseIcon();
        public override IToolButton Button { get; } = new EllipseButton();
        public override IToolPage Page { get; } = new GeometryEllipsePage();
    }
}