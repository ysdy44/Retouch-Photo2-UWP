// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
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
        public FrameworkElement Icon { get; } = new TextArtisticIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            Type = ToolType.TextArtistic,
            CenterContent = new TextArtisticIcon()
        };
        public FrameworkElement Page { get; } = new TextPage();


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new TextArtisticLayer
            {
                FontText = "AAA",
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandTextStyle,
            };
        }

    }
}