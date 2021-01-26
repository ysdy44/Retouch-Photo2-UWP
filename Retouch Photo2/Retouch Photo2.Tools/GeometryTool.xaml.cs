using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryTool.
    /// </summary>
    public abstract partial class GeometryTool
    {

        /// <summary>
        /// Create a <see cref="GeometryLayer"/>.
        /// </summary>
        /// <param name="customDevice"> The customDevice. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The producted layer. </returns>
        public abstract ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer);


        public void Started(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => ToolBase.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => ToolBase.CreateTool.Draw(drawingSession);


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }
    }


    /// <summary>
    /// Page of <see cref="GeometryTool"/>.
    /// </summary>
    public partial class GeometryPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Construct
        /// <summary>
        /// Initializes a GeometryPage. 
        /// </summary>
        public GeometryPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.StrokeShowControl.Tapped += (s, e) =>
            {
                this.TipViewModel.ShowMenuLayoutAt(MenuType.Stroke, this.StrokeShowControl);
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FillTextBlock.Text = resource.GetString("/ToolElements/Fill");
            this.StrokeTextBlock.Text = resource.GetString("/ToolElements/Stroke");
            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }

    }
}