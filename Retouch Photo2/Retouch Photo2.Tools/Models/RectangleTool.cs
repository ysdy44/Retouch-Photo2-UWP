using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
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
        public override ILayer CreateLayer(Transformer transformer)
        {
            return new RectangleLayer
            {
                IsChecked = true,
                FillBrush = new Brush
                {
                    Type = BrushType.Color,
                    Color = this.SelectionViewModel.FillColor,
                },

                Source = transformer,
                Destination = transformer,
            };
        }

        public override ToolType Type => ToolType.Rectangle;
        public override FrameworkElement Icon { get; } = new RectangleControl();
        public override FrameworkElement ShowIcon { get; } = new RectangleControl();
        public override Page Page => this._rectanglePage;
        RectanglePage _rectanglePage { get; } = new RectanglePage();
    }
}