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
    /// <see cref="ITool"/>'s TextArtisticTool.
    /// </summary>
    public partial class TextArtisticTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Construct
        /// <summary>
        /// Initializes a TextArtisticTool. 
        /// </summary>
        public TextArtisticTool()
        {
            this.Content = new TextTool();
            this.ConstructStrings();
        }
        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }

    /// <summary>
    /// <see cref="ITool"/>'s TextArtisticTool.
    /// </summary>
    public partial class TextArtisticTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/Tools/TextArtistic");
        }


        //@Content
        public ToolType Type => ToolType.TextArtistic;
        public FrameworkElement Icon { get; } = new TextArtisticIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new TextArtisticIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new TextArtisticLayer(customDevice)
            {
                FontText = "AAA",
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandTextStyle,
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => ToolBase.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => ToolBase.CreateTool.Draw(drawingSession);

    }
}