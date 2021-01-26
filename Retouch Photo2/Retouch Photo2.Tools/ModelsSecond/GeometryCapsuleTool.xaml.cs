using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="GeometryTool"/>'s GeometryCapsuleTool.
    /// </summary>
    public partial class GeometryCapsuleTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryCapsule;
        public FrameworkElement Icon { get; } = new GeometryCapsuleIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryCapsuleIcon()
        };
        public FrameworkElement Page { get; } = new GeometryCapsulePage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryCapsuleTool. 
        /// </summary>
        public GeometryCapsuleTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryCapsuleLayer(customDevice)
            {
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryCapsule");
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryTool"/>.
    /// </summary>
    internal partial class GeometryCapsulePage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Construct
        /// <summary>
        /// Initializes a GeometryCapsulePage. 
        /// </summary>
        public GeometryCapsulePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }
    }
}