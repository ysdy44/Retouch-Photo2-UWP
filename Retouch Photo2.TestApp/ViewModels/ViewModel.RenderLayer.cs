using Retouch_Photo2.Layers;
using System;
using System.Collections.ObjectModel;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel
    {
                    
        /// <summary>
        /// Indicates that the contents of the CanvasControl need to be redrawn.
        /// </summary>
        /// <param name="mode"> invalidate mode </param>
        public void Invalidate(InvalidateMode mode = InvalidateMode.None) => this.InvalidateAction?.Invoke(mode);
        /// <summary> <see cref = "Action" /> of the <see cref = "ViewModel.Invalidate" />. </summary>
        public Action<InvalidateMode> InvalidateAction { private get; set; }      
      

        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Layers.Layer" />s. </summary>
        public ObservableCollection<Layer> Layers = new ObservableCollection<Layer>();


        /// <summary> The layer is Checked, the other layers are UnChecked. </summary>
        /// <param name="layer"> current layer </param>
        public void LayerChecked(Layer layer)
        {
            foreach (Layer item in this.Layers)
            {
                item.IsChecked = (item == layer);
            }
        }

        /// <summary> The all layers are UnChecked. </summary>
        public void LayerUnChecked()
        {
            foreach (Layer item in this.Layers)
            {
                item.IsChecked = false;
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        /// <summary> 
        /// If <see cref="ViewModel.IsMezzanine"/> is not **null**, 
        /// Insert <see cref="ViewModel.MezzanineLayer"/> between <see cref="ViewModel.Layers"/>
        /// </summary>
        public Layer MezzanineLayer { get; private set; }

        /// <summary> Index of <see cref="ViewModel.MezzanineLayer"/>. </summary>
        public int MezzanineIndex { get; private set; }


        /// <summary>
        /// Turn on <see cref="ViewModel.IsMezzanine"/>.
        /// </summary>
        /// <param name="layer"> MezzanineLayer </param>
        public void TurnOnMezzanine(Layer layer)
        {
            this.MezzanineLayer = layer;
            this.MezzanineIndex = 0;

            for (int i = 0; i < this.Layers.Count; i++)
            {
                if (this.Layers[i].IsChecked)
                {
                    this.MezzanineIndex = i;
                }
            }
        }

        /// <summary>
        /// Turn off <see cref="ViewModel.IsMezzanine"/>.
        /// </summary>
        public void TurnOffMezzanine()
        {
            this.MezzanineLayer = null;
            this.MezzanineIndex = -1;
        }

        /// <summary>
        /// Insert layer into<see cref="ViewModel.Layers"/>.
        /// </summary>
        public void InsertMezzanine(Layer layer)
        {
            int index = this.MezzanineIndex-1;
            if (index < 0) index = 0;

            this.Layers.Insert(index, layer);

            this.TurnOffMezzanine();
        }
        

    }
}