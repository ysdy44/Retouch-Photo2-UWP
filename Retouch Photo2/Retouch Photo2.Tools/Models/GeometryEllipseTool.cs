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
    /// <see cref="ITool"/>'s GeometryEllipseTool.
    /// </summary>
    public partial class GeometryEllipseTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Construct
        /// <summary>
        /// Initializes a GeometryEllipseTool. 
        /// </summary>
        public GeometryEllipseTool()
        {
            this.ConstructStrings();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
        }
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryEllipseTool.
    /// </summary>
    public partial class GeometryEllipseTool : GeometryTool, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/Tools/Ellipse");
        }


        //@Content
        public ToolType Type => ToolType.GeometryEllipse;
        public FrameworkElement Icon { get; } = new GeometryEllipseIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new GeometryEllipseIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryEllipseLayer(customDevice)
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