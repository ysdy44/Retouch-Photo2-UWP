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
    /// <see cref="ICreateTool"/>'s GeometryRectangleTool.
    /// </summary>
    public class GeometryRectangleTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Coverride
        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryRectangleLayer
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformer),
                
                FillBrush = new Brush
                {
                    Type = BrushType.Color,
                    Color = this.SelectionViewModel.FillColor,
                },
            };
        }

        public override ToolType Type => ToolType.GeometryRectangle;
        public override FrameworkElement Icon { get; } = new GeometryRectangleIcon();
        public override IToolButton Button { get; } = new RectangleButton();
        public override IToolPage Page { get; } = new GeometryRectanglePage();
    }
}