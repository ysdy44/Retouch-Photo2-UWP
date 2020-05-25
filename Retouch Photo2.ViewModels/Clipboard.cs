using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    public class Clipboard
    {
        /// <summary> The single copyed layerage. </summary>
        public Layerage Layerage { get; private set; }

        /// <summary> The all copyed layerages. </summary> 
        public IEnumerable<Layerage> Layerages { get; private set; }


        public ListViewSelectionMode SelectionMode { get; set; }
        public bool CanPaste => this.SelectionMode == ListViewSelectionMode.Single || this.SelectionMode == ListViewSelectionMode.Multiple;

        public void SetModeNone()
        {
            this.Layerage = null;
            this.Layerages = null;
            this.SelectionMode = ListViewSelectionMode.None;
        }
        public void SetModeSingle(Layerage layerage)
        {
            this.Layerage = layerage.Clone();
            this.Layerages = null;
            this.SelectionMode = ListViewSelectionMode.Single;
        }
        public void SetModeMultiple(IEnumerable<Layerage> layerage)
        {
            this.Layerage = null;
            this.Layerages = from layer in layerage select layer.Clone();
            this.SelectionMode = ListViewSelectionMode.Multiple;
        }
    }

}
