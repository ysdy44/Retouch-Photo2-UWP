// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
        public ToolGroupType GroupType => ToolGroupType.Tool;
        public string Title => this.TextPage.Title;
        public ControlTemplate Icon => this.TextPage.Icon;
        public FrameworkElement Page => this.TextPage;
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.TextPage.IsOpen; set => this.TextPage.IsOpen = value; }
        readonly TextPage TextPage = new TextPage(ToolType.TextArtistic);


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