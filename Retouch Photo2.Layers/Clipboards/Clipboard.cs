// Core:              ★★★★★
// Referenced:   ★★★★
// Difficult:         ★★★
// Only:              ★★★★
// Complete:      ★★★★
using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a cutboard used to paste layer(s).
    /// </summary>
    public partial class Clipboard
    {
        /// <summary> The all copyed layerage. </summary> 
        public Layerage Layerage { get; private set; }

        /// <summary> The all copyed layerages. </summary> 
        public IEnumerable<Layerage> Layerages { get; private set; }


        /// <summary>
        /// Gets selection-mode by count of selected layers. 
        /// </summary>
        public ListViewSelectionMode SelectionMode { get; set; }
        /// <summary>
        /// Can paste by <see cref="Clipboard.SelectionMode"/>
        /// </summary>
        public bool CanPaste => this.SelectionMode == ListViewSelectionMode.Single || this.SelectionMode == ListViewSelectionMode.Multiple;


        /// <summary>
        ///  Sets the mode and notify all properties.
        ///  and copy all selected layerage.
        /// </summary>     
        /// <param name="customDevice"> The custom-device. </param>
        public void SetMode(CanvasDevice customDevice)
        {
            //Layerages
            IEnumerable<Layerage> selectedLayerages = LayerageCollection.GetAllSelected();
            int count = selectedLayerages.Count();

            if (count == 0)
            {
                this.SelectionMode = ListViewSelectionMode.None;//None

                this.Layerage = null;
                this.Layerages = null;

                Clipboard.Instances.Clear();
            }
            else if (count == 1)
            {
                this.SelectionMode = ListViewSelectionMode.Single;//None

                Layerage layerage = selectedLayerages.Single();
                this.Layerage = layerage.Clone();
                this.Layerages = null;

                Clipboard.Instances.Clear();
                LayerageCollection.CopyLayerage(customDevice, this.Layerage);
            }
            else if (count >= 2)
            {
                this.SelectionMode = ListViewSelectionMode.Multiple;//None
                this.Layerage = null;
                this.Layerages = from layerage in selectedLayerages select layerage.Clone();

                Clipboard.Instances.Clear();
                LayerageCollection.CopyLayerages(customDevice, this.Layerages);
            }
        }
               
    }

}