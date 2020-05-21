using Retouch_Photo2.Blends;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    public interface ILayerControl
    {
        ILayer Layer { get; }

        LayerControl Self { get; }
        Visibility Visibility { get; set; }
        string Text { get; set; }
        string Type { get; set; }
        object Icon { get; set; }

        int ControlHeight { get; set; }
        int Depth { get; set; }
        int Index { get; set; }

        //Property
        void SetVisibility(Visibility value);
        void SetTagType(TagType value);

        //Control
        void SetOverlayMode(OverlayMode value);
        void SetIsSelected(bool value);
        void SetBackground(BackgroundMode value);
        void SetIsExpand(bool value);
        void SetChildrenZero(bool value);
    }
}