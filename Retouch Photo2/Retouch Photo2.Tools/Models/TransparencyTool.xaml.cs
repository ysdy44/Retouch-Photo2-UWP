using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s TransparencyTool.
    /// </summary>
    public partial class TransparencyTool : ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.Transparency;
        public FrameworkElement Icon { get; } = new TransparencyIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new TransparencyIcon()
        };
        public FrameworkElement Page => this.TransparencyPage;
        TransparencyPage TransparencyPage = new TransparencyPage();


        //@Construct
        /// <summary>
        /// Initializes a TransparencyTool. 
        /// </summary>
        public TransparencyTool()
        {
            this.ConstructStrings();
        }


        public void Started(Vector2 startingPoint, Vector2 point) { }
        public void Delta(Vector2 startingPoint, Vector2 point) { }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) { }
        public void Clicke(Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession) { }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/Tools/Transparency");
        }

    }


    /// <summary>
    /// Page of <see cref="TransparencyTool"/>.
    /// </summary>
    internal partial class TransparencyPage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Construct
        /// <summary>
        /// Initializes a TransparencyPage. 
        /// </summary>
        public TransparencyPage()
        {
            this.InitializeComponent();
            //this.ConstructStrings();
        }

    }
}