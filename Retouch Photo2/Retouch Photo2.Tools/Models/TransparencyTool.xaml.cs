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
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Construct
        /// <summary>
        /// Initializes a TransparencyTool. 
        /// </summary>
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

            this.Button.Title = resource.GetString("/Tools/Transparency");
        }


        //@Content
        public ToolType Type => ToolType.Transparency;
        public FrameworkElement Icon { get; } = new TransparencyIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new TransparencyIcon()
        };
        public FrameworkElement Page => this;


        public void Started(Vector2 startingPoint, Vector2 point) { }
        public void Delta(Vector2 startingPoint, Vector2 point) { }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) { }
        public void Clicke(Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession) { }

    }
}