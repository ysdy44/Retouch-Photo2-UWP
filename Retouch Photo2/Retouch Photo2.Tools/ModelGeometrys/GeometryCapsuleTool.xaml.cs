// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
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
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page { get; } = new GeometryCapsulePage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryCapsuleLayer
            {
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
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
        }

    }
}