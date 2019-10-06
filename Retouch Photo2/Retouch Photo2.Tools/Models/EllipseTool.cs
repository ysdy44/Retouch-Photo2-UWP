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
    /// <see cref="ICreateTool"/>'s EllipseTool.
    /// </summary>
    public class EllipseTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        
        //@Override
        public override ILayer CreateLayer(Transformer transformer) => new EllipseLayer
        {
            SelectMode = SelectMode.Selected,
            TransformManager = new TransformManager(transformer),

            FillBrush = new Brush
            {
                Type = BrushType.Color,
                Color = this.SelectionViewModel.FillColor,
            },
        };

        public override bool IsSelected
        {
            set
            {
                this.Button.IsSelected = value;
                this._ellipsePage.IsSelected = value;
            }
        }
        public override ToolType Type => ToolType.Ellipse;
        public override FrameworkElement Icon { get; } = new EllipseIcon();
        public override IToolButton Button { get; } = new EllipseButton();
        public override Page Page => this._ellipsePage;
        EllipsePage _ellipsePage { get; } = new EllipsePage();
    }
}