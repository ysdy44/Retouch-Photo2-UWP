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
    /// <see cref="GeometryTool"/>'s GeometryRectangleTool.
    /// </summary>
    public partial class GeometryRectangleTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryRectangle;
        public FrameworkElement Icon { get; } = new GeometryRectangleIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new GeometryRectangleIcon()
        };
        public FrameworkElement Page { get; } = new GeometryPage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryRectangleTool. 
        /// </summary>
        public GeometryRectangleTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryRectangleLayer(customDevice)
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

            this.Button.Title = resource.GetString("/Tools/Rectangle");
        }

    }
}