// Core:              ★★★
// Referenced:   ★★★
// Difficult:         ★★
// Only:              ★★★
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryTool.
    /// </summary>
    public abstract partial class GeometryTool
    {

        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;

        /// <summary>
        /// Create a <see cref="GeometryLayer"/>.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The producted layer. </returns>
        public abstract ILayer CreateLayer(Transformer transformer);


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);


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
        public GeometryPage(ToolType toolType)
        {
            this.InitializeComponent();
            this.ConstructStrings();


            /*
            < Border.Resources >
                < ResourceDictionary Source = "ms-appx:///Retouch Photo2.Tools/Icons/ViewIcon.xaml" />
             </ Border.Resources >
             < Border.Child >
                 < ContentControl HorizontalAlignment = "Center" VerticalAlignment = "Center" Template = "{StaticResource ViewIcon}" />
             </ Border.Child >
             */
            if (this.IconBorder is Border border)
            {
                border.Resources = new ResourceDictionary
                {
                    Source = new Uri($@"ms-appx:///Retouch Photo2.Tools/Icons/{toolType}Icon.xaml")
                };
                border.Child = new ContentControl
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Template = border.Resources[$"{toolType}Icon"] as ControlTemplate
                };
            }


            this.StrokeShowControl.Tapped += (s, e) =>
            {
                this.TipViewModel.ShowMenuLayoutAt(MenuType.Stroke, this.StrokeShowControl);
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FillTextBlock.Text = resource.GetString("Tools_Fill");
            this.StrokeTextBlock.Text = resource.GetString("Tools_Stroke");
        }

    }
}