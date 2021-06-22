// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Microsoft.Graphics.Canvas;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerManager
    {

        // Action
        /// <summary> Occurs when a layerage receive interaction. </summary>
        public static Action<ILayer> ItemClick { get; set; }
        /// <summary> Occurs when right-click input a layerage. </summary>
        public static Action<ILayer> RightTapped { get; set; }
        /// <summary> Occurs when a layerage visibility changes. </summary>
        public static Action<ILayer> VisibilityChanged { get; set; }
        /// <summary> Occurs when the Select has changed </summary>
        public static Action<ILayer> IsSelectedChanged { get; set; }
        /// <summary> Occurs when the expaned has changed </summary>
        public static Action<ILayer> IsExpandChanged { get; set; }


        // Overlay
        /// <summary>
        /// Gets drag layerage overlay properties.
        /// </summary>
        public static bool IsOverlay { get; internal set; }
        /// <summary>
        /// Occurs when drag items started.
        /// </summary>
        public static Action<ILayer, ManipulationModes> DragItemsStarted { get; set; }
        /// <summary>
        /// Occurs when drag items delta.
        /// </summary>
        public static Action<ILayer, OverlayMode> DragItemsDelta { get; set; }
        /// <summary>
        /// Occurs when drag items is completed.
        /// </summary>
        public static Action DragItemsCompleted { get; set; }


        // Root
        /// <summary>
        /// The root layerage.
        /// </summary>
        public static Layerage RootLayerage { get; } = new Layerage();
        /// <summary>
        /// A stack panel, contains all <see cref="ILayer.Control"/>s. 
        /// </summary>
        public static StackPanel RootStackPanel { get; } = new StackPanel();

        /// <summary> 
        /// Gets or sets the canvas-device.
        /// </summary>
        public static CanvasDevice CanvasDevice { get; } = new CanvasDevice();
        /// <summary> 
        /// Gets or sets layer controls height. 
        /// </summary>
        public static int ControlsHeight { get; set; } = 40;


        /// <summary>
        /// Get a layerage parents children( or root layers when it has not parents).
        /// </summary>
        /// <param name="layerage"></param>
        /// <returns></returns>
        public static Layerage GetParentsChildren(Layerage layerage)
        {
            if (layerage is null) return LayerManager.RootLayerage;
            if (layerage.Parents is null) return LayerManager.RootLayerage;
            return layerage.Parents;
        }

    }
}