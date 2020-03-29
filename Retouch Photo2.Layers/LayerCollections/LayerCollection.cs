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

        //@Static
        /// <summary> Occurs when a layer receive interaction. </summary>
        public static Action<ILayer> ItemClick;
        /// <summary> Occurs when right-click input a layer. </summary>
        public static Action<ILayer> RightTapped;
        /// <summary> Occurs when a layer visibility changes. </summary>
        public static Action<ILayer> VisualChanged;
        /// <summary> Occurs when the layers has changed </summary>
        public static Action SelectChanged;

        //Overlay
        /// <summary>
        /// Gets drag layer overlay properties.
        /// </summary>
        public static bool IsOverlay { get; internal set; }
        /// <summary>
        /// Occurs when drag items started.
        /// </summary>
        public static Action<ILayer, SelectMode> DragItemsStarted;
        /// <summary>
        /// Occurs when drag items delta.
        /// </summary>
        public static Action<ILayer, OverlayMode> DragItemsDelta;
        /// <summary>
        /// Occurs when drag items is completed.
        /// </summary>
        public static Action DragItemsCompleted;

        //Root
        /// <summary>
        /// The root layers.
        /// </summary>
        public IList<ILayer> RootLayers { get; private set; } = new List<ILayer>();
        /// <summary>
        /// The root controls.
        /// </summary>
        public ObservableCollection<UIElement> RootControls { get; private set; } = new ObservableCollection<UIElement>();

    }
}