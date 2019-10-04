using FanKit.Transformers;
using Retouch_Photo2.Brushs;
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
    /// <see cref="ICreateTool"/>'s RectangleTool.
    /// </summary>
    public class RectangleTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Coverride
        public override ILayer CreateLayer(LayerCollection layerCollection, Transformer transformer)
        {
            return new RectangleLayer(layerCollection)
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformer),
                
                FillBrush = new Brush
                {
                    Type = BrushType.Color,
                    Color = this.SelectionViewModel.FillColor,
                },
            };
        }

        public override bool IsSelected
        {
            set
            {
                this.Button.IsSelected = value;
                this._rectanglePage.IsSelected = value;
            }
        }
        public override ToolType Type => ToolType.Rectangle;
        public override FrameworkElement Icon { get; } = new RectangleIcon();
        public override IToolButton Button { get; } = new RectangleButton();
        public override Page Page => this._rectanglePage;
        RectanglePage _rectanglePage { get; } = new RectanglePage();
    }
}