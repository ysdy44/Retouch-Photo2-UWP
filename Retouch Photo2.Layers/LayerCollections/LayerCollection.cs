using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {
        public Action<ILayer> ItemClick;
        public Action<ILayer> RightTapped;

        //Overlay
        public bool IsOverlay { get; internal set; }
        public Action<ILayer, SelectMode> DragItemsStarted;
        public Action<ILayer, OverlayMode> DragItemsDelta;
        public Action DragItemsCompleted;

        //Root
        public IList<ILayer> RootLayers { get; private set; } = new List<ILayer>();
        public ObservableCollection<UIElement> RootControls { get; private set; } = new ObservableCollection<UIElement>();
    }
}