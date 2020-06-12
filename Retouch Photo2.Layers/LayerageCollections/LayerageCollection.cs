using FanKit.Transformers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public partial class LayerageCollection
    {

        //@Static
        /// <summary>  Gets or sets layer controls height. </summary>
        public static int ControlsHeight { get; set; } = 40;


        //@Static
        /// <summary> Occurs when a layerage receive interaction. </summary>
        public static Action<ILayer> ItemClick;
        /// <summary> Occurs when right-click input a layerage. </summary>
        public static Action<ILayer> RightTapped;
        /// <summary> Occurs when a layerage visibility changes. </summary>
        public static Action<ILayer> VisibilityChanged;
        /// <summary> Occurs when the Select has changed </summary>
        public static Action<ILayer> IsSelectedChanged;
        /// <summary> Occurs when the expaned has changed </summary>
        public static Action<ILayer> IsExpandChanged;


        //Overlay
        /// <summary>
        /// Gets drag layerage overlay properties.
        /// </summary>
        public static bool IsOverlay { get; internal set; }
        /// <summary>
        /// Occurs when drag items started.
        /// </summary>
        public static Action<ILayer, ManipulationModes> DragItemsStarted;
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