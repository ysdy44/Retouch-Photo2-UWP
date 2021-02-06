// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="GeometryTool"/>'s TextArtisticTool.
    /// </summary>
    public partial class TextArtisticTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.TextArtistic;
        public FrameworkElement Icon { get; } = new TextArtisticIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new TextArtisticIcon()
        };
        public FrameworkElement Page { get; } = new TextPage();


        //@Construct
        /// <summary>
        /// Initializes a TextArtisticTool. 
        /// </summary>
        public TextArtisticTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new TextArtisticLayer(customDevice)
            {
                FontText = "AAA",
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandTextStyle,
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("Tools_TextArtistic");
        }

    }
}