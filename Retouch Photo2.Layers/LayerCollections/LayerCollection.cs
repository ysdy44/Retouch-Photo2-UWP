using FanKit.Transformers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        //@Static
        /// <summary> Occurs when a layer receive interaction. </summary>
        public static Action<Layerage> ItemClick;
        /// <summary> Occurs when right-click input a layer. </summary>
        public static Action<Layerage> RightTapped;
        /// <summary> Occurs when a layer visibility changes. </summary>
        public static Action<Layerage> VisibilityChanged;
        /// <summary> Occurs when the Select has changed </summary>
        public static Action<Layerage> IsSelectedChanged;
        /// <summary> Occurs when the expaned has changed </summary>
        public static Action<Layerage> IsExpandChanged;

        //Overlay
        /// <summary>
        /// Gets drag layer overlay properties.
        /// </summary>
        public static bool IsOverlay { get; internal set; }
        /// <summary>
        /// Occurs when drag items started.
        /// </summary>
        public static Action<Layerage, ManipulationModes> DragItemsStarted;
        /// <summary>
        /// Occurs when drag items delta.
        /// </summary>
        public static Action<Layerage, OverlayMode> DragItemsDelta;
        /// <summary>
        /// Occurs when drag items is completed.
        /// </summary>
        public static Action DragItemsCompleted;

        //Root
        /// <summary>
        /// The root layers.
        /// </summary>
        public IList<Layerage> RootLayers { get; private set; } = new List<Layerage>();
        /// <summary>
        /// The root controls.
        /// </summary>
        public ObservableCollection<LayerControl> RootControls { get; private set; } = new ObservableCollection<LayerControl>();
    

        /// <summary>
        /// Get a layer parents children( or root layers when it has not parents).
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public IList<Layerage> GetParentsChildren(Layerage layer)
        {
            if (layer == null) return this.RootLayers;
            if (layer.Parents == null) return this.RootLayers;
            return layer.Parents.Children;
        }

    }
}