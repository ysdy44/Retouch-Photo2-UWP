using Retouch_Photo2.Layers;
using Retouch_Photo2.Library;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel
    {

        /// <summary>
        /// Gets selection-mode by count of checked layers. 
        /// 0>None.
        /// 1>Single.
        /// 2>Multiple.
        /// Temporary Transformer: Extended
        /// </summary>
        public ListViewSelectionMode SelectionMode { get; private set; }


        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.Library.Transformer" />. </summary>
        public Transformer Transformer { get; set; }

        public Layer SelectionLayer { get; private set; }
        public IEnumerable<Layer> SelectionLayers { get; private set; }

        
        public void SetSelectionModeNone()
        {
            this.SelectionMode = ListViewSelectionMode.None;//Transformer

            this.Transformer = new Transformer();
            this.SelectionLayer = null;
            this.SelectionLayers = null;
        }

        public void SetSelectionModeSingle(Layer layer)
        {
            this.SelectionMode = ListViewSelectionMode.Single;//Transformer
            
            this.Transformer = layer.TransformerMatrix.Destination;
            this.SelectionLayer = layer;
            this.SelectionLayers = null;
        }

        public void SetSelectionModeMultiple(IEnumerable<Layer> layers)
        {
            this.SelectionMode = ListViewSelectionMode.Multiple;//Transformer

            {
                float left = float.MaxValue;
                float top = float.MaxValue;
                float right = float.MinValue;
                float bottom = float.MinValue;

                void bbb(Vector2 vector)
                {
                    if (left > vector.X) left = vector.X;
                    if (top > vector.Y) top = vector.Y;
                    if (right < vector.X) right = vector.X;
                    if (bottom < vector.Y) bottom = vector.Y;
                }

                //Foreach
                foreach (Layer item in layers)
                {
                    bbb(item.TransformerMatrix.Destination.LeftTop);
                    bbb(item.TransformerMatrix.Destination.RightTop);
                    bbb(item.TransformerMatrix.Destination.RightTop);
                    bbb(item.TransformerMatrix.Destination.LeftBottom);
                }

                this.Transformer= new Transformer(left, top, right, bottom);
            }
            this.SelectionLayer = null;
            this.SelectionLayers = layers;
        }

        public void SetSelectionMode(IEnumerable <Layer> layers)
        {
            IEnumerable<Layer> checkedLayers = from item in layers where item.IsChecked select item;
            int count = checkedLayers.Count();

            if (count == 0)
                this.SetSelectionModeNone();//None
          
            else if (count == 1)
                this.SetSelectionModeSingle(checkedLayers.Single());//Single

            else if (count >= 2)
                this.SetSelectionModeMultiple(checkedLayers);//Multiple
        }

    }
}