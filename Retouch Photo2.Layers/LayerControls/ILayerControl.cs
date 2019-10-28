using Retouch_Photo2.Blends;
using Windows.UI.Xaml;

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

        //Property
        void SetVisibility(Visibility value);
        void SetTagType(TagType value);

        //Control
        void SetExpandMode(ExpandMode value);
        void SetSelectMode(SelectMode value);
        void SetOverlayMode(OverlayMode value);
    }
}