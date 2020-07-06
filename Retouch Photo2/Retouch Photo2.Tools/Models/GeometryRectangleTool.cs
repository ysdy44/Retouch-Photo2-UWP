using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryRectangleTool.
    /// </summary>
    public partial class GeometryRectangleTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        GeometryTool GeometryTool = new GeometryTool();


        //@Construct
        /// <summary>
        /// Initializes a GeometryRectangleTool. 
        /// </summary>
        public GeometryRectangleTool()
        {
            this.Content = this.GeometryTool;
            this.ConstructStrings();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.GeometryTool.OnNavigatedFrom();
        }
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryRectangleTool.
    /// </summary>
    public partial class GeometryRectangleTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/Tools/Rectangle");
        }


        //@Content
        public ToolType Type => ToolType.GeometryRectangle;
        public FrameworkElement Icon { get; } = new GeometryRectangleIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new GeometryRectangleIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryRectangleLayer(customDevice)
            {
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => ToolBase.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => ToolBase.CreateTool.Draw(drawingSession);

    }
}