using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s TextFrameTool.
    /// </summary>
    public partial class TextFrameTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        
        TextFrameLayer FrameLayer => this.SelectionViewModel.TextFrameLayer;


        //@VisualState
        public bool _vsIsFullScreen;
        public VisualState VisualState
        {
            get => this._vsIsFullScreen ? this.FullScreen : this.Normal;
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        public TextFrameTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.TextBox.TextChanged += (s, e) =>
            {
                if (this.FrameLayer == null) return;
                this.FrameLayer.Text = this.TextBox.Text;
                this.ViewModel.Invalidate();//Invalidate
            };

            this.CharacterButton.Tapped += (s, e) =>
            {
                this.TipViewModel.SetMenuState(MenuType.Character, MenuState.Hide, MenuState.FlyoutShow);
            };
            this.FullScreenButton.Tapped += (s, e) =>
            {
                this._vsIsFullScreen = !this._vsIsFullScreen;
                this.VisualState = this.VisualState;//State
            };
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }

    }


    /// <summary>
    /// <see cref="ITool"/>'s TextFrameTool.
    /// </summary>
    public partial class TextFrameTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/TextFrame");

            this.TextBox.PlaceholderText = resource.GetString("/Tools/TextFrame_PlaceholderText");
        }


        //@Content
        public ToolType Type => ToolType.TextFrame;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new TextFrameIcon();
        readonly ToolButton _button = new ToolButton(new TextFrameIcon());

        private ILayer CreateLayer(Transformer transformer)
        {
            return new TextFrameLayer
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformer),
                StyleManager = new StyleManager
                {
                    FillBrush = new Brush
                    {
                        Type = BrushType.Color,
                        Color = Colors.Black,
                    }
                }
            };
        }


        public void Starting(Vector2 point) => this.TipViewModel.CreateTool.Starting(point);
        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isSingleStarted);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }
}