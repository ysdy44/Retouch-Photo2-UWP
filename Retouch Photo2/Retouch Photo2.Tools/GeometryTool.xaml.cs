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


        //@Content 
        public string Title { get; private set; }
        public ControlTemplate Icon => this.IconContentControl.Template;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryPage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryPage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryPage), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryPage. 
        /// </summary>
        public GeometryPage(ToolType toolType)
        {
            this.InitializeComponent();
            this.ResourceDictionary.Source = new Uri($@"ms-appx:///Retouch Photo2.Tools/Icons/{toolType}Icon.xaml");
            this.IconContentControl.Template = this.ResourceDictionary[$"{toolType}Icon"] as ControlTemplate;
            this.ConstructStrings(toolType);
                        
            //Flyout
            this.FillBrushButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowFill?.Invoke(this, this.FillBrushButton);
            this.StrokeBrushButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowStroke?.Invoke(this, this.StrokeBrushButton);
            this.StrokeShowControl.Tapped += (s, e) => this.TipViewModel.ShowMenuLayoutAt(MenuType.Stroke, this.StrokeShowControl);

            this.MoreCreateButton.Click += (s, e) => Retouch_Photo2.DrawPage.ShowMoreCreate?.Invoke(this, this.MoreCreateButton);
        }


        //Strings
        private void ConstructStrings(ToolType toolType)
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Title = resource.GetString($"Tools_{toolType}");

            this.FillTextBlock.Text = resource.GetString("Tools_Fill");
            this.StrokeTextBlock.Text = resource.GetString("Tools_Stroke");
            
            this.StrokeShowToolTip.Content = resource.GetString("Menus_Stroke");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_MoreCreate");
        }

    }
}