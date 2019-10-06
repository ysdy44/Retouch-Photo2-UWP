using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Layers
{
    public interface ILayerControl
    {
        FrameworkElement Self { get; }
        Visibility Visibility { get; set; }
        string Text { get; set; }
        object Icon { get; set; }

        int ControlHeight { get; set; }
        int Depth { get; set; }

        Button ExpanedButton { get; }
        Button SelectedButton { get; }

        void SetExpandMode(ExpandMode value);
        void SetSelectMode(SelectMode value);
        void SetOverlayMode(OverlayMode value);
    }
}