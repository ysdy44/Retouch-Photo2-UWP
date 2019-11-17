using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ICreateTool"/>'s TextArtisticTool.
    /// </summary>
    public class TextArtisticTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Override
        public override ILayer CreateLayer(Transformer transformer)
        {
            return new TextArtisticLayer
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformer)
                {
                    DisabledRadian = true//DisabledRadian
                },
            };
        }

        public override ToolType Type => ToolType.TextArtistic;
        public override FrameworkElement Icon { get; } = new TextArtisticIcon();
        public override IToolButton Button { get; } = new TextArtisticButton();
        public override IToolPage Page { get; } = new TextArtisticPage();
    }
}