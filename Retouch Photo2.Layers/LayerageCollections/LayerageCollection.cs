using FanKit.Transformers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    public partial class LayerageCollection
    {

        //@Static
        /// <summary> Occurs when a layerage receive interaction. </summary>
        public static Action<Layerage> ItemClick;
        /// <summary> Occurs when right-click input a layerage. </summary>
        public static Action<Layerage> RightTapped;
        /// <summary> Occurs when a layerage visibility changes. </summary>
        public static Action<Layerage> VisibilityChanged;
        /// <summary> Occurs when the Select has changed </summary>
        public static Action<Layerage> IsSelectedChanged;
        /// <summary> Occurs when the expaned has changed </summary>
        public static Action<Layerage> IsExpandChanged;

        //Overlay
        /// <summary>
        /// Gets drag layerage overlay properties.
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
        /// The root layerages.
        /// </summary>
        public IList<Layerage> RootLayerages { get; private set; } = new List<Layerage>();
        /// <summary>
        /// The root controls.
        /// </summary>
        public ObservableCollection<LayerControl> RootControls { get; private set; } = new ObservableCollection<LayerControl>();


        /// <summary>
        /// Get a layerage parents children( or root layers when it has not parents).
        /// </summary>
        /// <param name="layerage"></param>
        /// <returns></returns>
        public IList<Layerage> GetParentsChildren(Layerage layerage)
        {
            if (layerage == null) return this.RootLayerages;
            if (layerage.Parents == null) return this.RootLayerages;
            return layerage.Parents.Children;
        }

    }
}