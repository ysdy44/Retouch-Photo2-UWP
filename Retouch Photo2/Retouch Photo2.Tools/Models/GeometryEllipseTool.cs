// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="GeometryTool"/>'s GeometryEllipseTool.
    /// </summary>
    public partial class GeometryEllipseTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public FrameworkElement Icon { get; } = new GeometryEllipseIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            Type = ToolType.GeometryEllipse,
            CenterContent = new GeometryEllipseIcon()
        };
        public FrameworkElement Page { get; } = new GeometryPage();


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryEllipseLayer
            {
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

    }
}