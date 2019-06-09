using Retouch_Photo2.Layers;
using Retouch_Photo2.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.ViewModels
{
    public class Selection
    {

        /// <summary>
        /// Gets selection-mode by count of checked layers. 
        /// 0>None.
        /// 1>Single.
        /// 2>Multiple.
        /// Temporary Transformer: Extended
        /// </summary>
        public ListViewSelectionMode Mode { get; private set; }


        /// <summary> Transformer of <see cref = "Retouch_Photo2.Library.Selection" />. </summary>
        public Transformer Transformer { get; set; }

        /// <summary> Layer of <see cref = "Retouch_Photo2.Library.Selection" />. </summary>
        public Layer Layer { get; private set; }
        /// <summary> Layers of <see cref = "Retouch_Photo2.Library.Selection" />. </summary>
        public IEnumerable<Layer> Layers { get; private set; }


        /// <summary> Sets <see cref = "Selection.Mode" /> to None. </summary>
        public void None()
        {
            this.Transformer = new Transformer();
            this.Layer = null;
            this.Layers = null;

            this.Mode = ListViewSelectionMode.None;//Transformer
        }

        /// <summary> Sets <see cref = "Selection.Mode" /> to Single. </summary>
        public void Single(Layer layer)
        {
            this.Transformer = layer.TransformerMatrix.Destination;
            this.Layer = layer;
            this.Layers = null;
            
            this.Mode = ListViewSelectionMode.Single;//Transformer
        }

        /// <summary> Sets <see cref = "Selection.Mode" /> to Multiple. </summary>
        public void Multiple(IEnumerable<Layer> layers)
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;

            {
                void aaa(Vector2 vector)
                {
                    if (left > vector.X) left = vector.X;
                    if (top > vector.Y) top = vector.Y;
                    if (right < vector.X) right = vector.X;
                    if (bottom < vector.Y) bottom = vector.Y;
                }

                //Foreach
                foreach (Layer item in layers)
                {
                    aaa(item.TransformerMatrix.Destination.LeftTop);
                    aaa(item.TransformerMatrix.Destination.RightTop);
                    aaa(item.TransformerMatrix.Destination.RightTop);
                    aaa(item.TransformerMatrix.Destination.LeftBottom);
                }
            }

            this.Transformer = new Transformer(left, top, right, bottom);
            this.Layer = null;
            this.Layers = layers;

            this.Mode = ListViewSelectionMode.Multiple;//Transformer
        }


        /// <summary>
        /// Sets all selection layer(s).
        /// </summary>
        /// <param name="action"> action </param>
        public void SetLayer(Action<Layer> action)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    action(this.Layer);
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layer layer in this.Layers)
                    {
                        action(layer);
                    }
                    break;
            }
        }
    }


    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel
    {

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModel.Selection" />. </summary>
        public Selection Selection { get; } = new Selection();
        /// <summary> Sets the <see cref = "ViewModel.Selection" />. </summary>
        public void SetSelection()
        {
            IEnumerable<Layer> checkedLayers = from item in this.Layers where item.IsChecked select item;
            int count = checkedLayers.Count();

            if (count == 0)
                this.SelectionNone();//None

            else if (count == 1)
                this.SelectionSingle(checkedLayers.Single());//Single

            else if (count >= 2)
                this.SelectionMultiple(checkedLayers);//Multiple
        }



        /// <summary> Sets <see cref = "Selection.Mode" /> to None. </summary>
        public void SelectionNone()
        {
            this.Selection.None();

            this.SetSelectionOpacity(0);
            this.SelectionIsVisual = false;
        }
        /// <summary> Sets <see cref = "Selection.Mode" /> to Single. </summary>
        public void SelectionSingle(Layer layer)
        {
            this.Selection.Single(layer);

            this.SetSelectionOpacity(layer.Opacity);
            this.SelectionIsVisual = layer.IsVisual;
        }
        /// <summary> Sets <see cref = "Selection.Mode" /> to Multiple. </summary>
        public void SelectionMultiple(IEnumerable<Layer> layers)
        {
             this.Selection.Multiple(layers);
        }



        /////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary> <see cref = "ViewModel.Selection" />'s opacity. </summary>
        public float SelectionOpacity;
        /// <summary> Sets the <see cref = "ViewModel.SelectionOpacity" />. </summary>
        public void SetSelectionOpacity(float value)
        {
            this.SelectionOpacity = value;
            this.OnPropertyChanged(nameof(this.SelectionOpacity));//Notify 
        }


        /// <summary> <see cref = "ViewModel.Selection" />'s isVisual. </summary>
        public bool SelectionIsVisual
        {
            get => this.selectionIsVisual;
            set
            {
                this.selectionIsVisual = value;
                this.OnPropertyChanged(nameof(this.SelectionIsVisual));//Notify 
            }
        }
        private bool selectionIsVisual;



    }
}