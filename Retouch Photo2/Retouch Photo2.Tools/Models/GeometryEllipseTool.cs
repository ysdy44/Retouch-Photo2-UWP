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
        public ToolType Type => ToolType.GeometryEllipse;
        public FrameworkElement Icon { get; } = new GeometryEllipseIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new GeometryEllipseIcon()
        };
        public FrameworkElement Page { get; } = new GeometryPage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryEllipseTool. 
        /// </summary>
        public GeometryEllipseTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryEllipseLayer(customDevice)
            {
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/Tools/Ellipse");
        }

    }
}