using FanKit.Transformers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {
        public static Action<ILayer> ItemClick;
        public static Action<ILayer> RightTapped;
        public static Action SelectChanged;

        //Overlay
        public static bool IsOverlay { get; internal set; }
        public static Action<ILayer, SelectMode> DragItemsStarted;
        public static Action<ILayer, OverlayMode> DragItemsDelta;
        public static Action DragItemsCompleted;

        //Root
        public IList<ILayer> RootLayers { get; private set; } = new List<ILayer>();
        public ObservableCollection<UIElement> RootControls { get; private set; } = new ObservableCollection<UIElement>();
               
    }
}