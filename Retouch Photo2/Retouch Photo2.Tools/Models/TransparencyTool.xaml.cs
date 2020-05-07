using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s TransparencyTool.
    /// </summary>
    public partial class TransparencyTool : Page, ITool
    { 
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Construct
        public TransparencyTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }

    }

    /// <summary>
    /// <see cref="ITool"/>'s TransparencyTool.
    /// </summary>
    public partial class TransparencyTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/Transparency");
        }


        //@Content
        public ToolType Type => ToolType.Transparency;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new TransparencyIcon();
        readonly ToolButton _button = new ToolButton(new TransparencyIcon());


        public void Starting(Vector2 point) { }
        public void Started(Vector2 startingPoint, Vector2 point) { }
        public void Delta(Vector2 startingPoint, Vector2 point) { }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) { }
        public void Clicke(Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession) { }

    }
}