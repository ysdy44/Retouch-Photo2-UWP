using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ICreateTool"/>'s TextFrameTool.
    /// </summary>
    public class TextFrameTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Override
        public override ILayer CreateLayer(Transformer transformer)
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
                    },
                    StrokeBrush = new Brush
                    {
                        Type = BrushType.None,
                    },
                    StrokeWidth = 0,
                }
            };
        }

        public override ToolType Type => ToolType.TextFrame;
        public override FrameworkElement Icon { get; } = new TextFrameIcon();
        public override IToolButton Button { get; } = new TextFrameButton();
        public override IToolPage Page { get; } = new TextFramePage();
    }
}