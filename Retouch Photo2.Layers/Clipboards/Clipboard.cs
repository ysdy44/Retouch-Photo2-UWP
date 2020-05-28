using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Layers
{
    public partial class Clipboard
    {
        /// <summary> The all copyed layerage. </summary> 
        public Layerage Layerage { get; private set; }

        /// <summary> The all copyed layerages. </summary> 
        public IEnumerable<Layerage> Layerages { get; private set; }


        public ListViewSelectionMode SelectionMode { get; set; }
        public bool CanPaste => this.SelectionMode == ListViewSelectionMode.Single || this.SelectionMode == ListViewSelectionMode.Multiple;

        public void SetMode(ICanvasResourceCreator resourceCreator, LayerageCollection layerageCollection)
        {
            //Layerages
            IEnumerable<Layerage> selectedLayerages = LayerageCollection.GetAllSelectedLayerages(layerageCollection);
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
                LayerageCollection.CopyLayerage(resourceCreator, this.Layerage);
            }
            else if (count >= 2)
            {
                this.SelectionMode = ListViewSelectionMode.Multiple;//None
                this.Layerage = null;
                this.Layerages = from layerage in selectedLayerages select layerage.Clone();

                Clipboard.Instances.Clear();
                LayerageCollection.CopyLayerages(resourceCreator, this.Layerages);
            }
        }
               
    }

}