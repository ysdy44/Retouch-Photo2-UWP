using Retouch_Photo2.Blends;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Transformers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI;
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
        public ListViewSelectionMode SelectionMode
        {
            get => this.selectionMode;
            set
            {
                this.selectionMode = value;
                this.OnPropertyChanged(nameof(this.SelectionMode));//Notify 
            }
        }
        private ListViewSelectionMode selectionMode;

        /// <summary> Transformer of checked layers.  </summary>
        public Transformer SelectionTransformer
        {
            get => this.selectionTransformer;
             set
            {
                this.selectionTransformer = value;
                this.OnPropertyChanged(nameof(this.SelectionTransformer));//Notify 
            }
        }
        private Transformer selectionTransformer;

        /// <summary> Transformer of the single checked layer.  </summary>
        public Layer SelectionLayer { get; private set; }

        /// <summary> Transformer of the all checked layers.  </summary>
        public IEnumerable<Layer> SelectionLayers { get; private set; }


        /////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Sets <see cref = "ViewModel.SelectionMode" /> to None.
        /// </summary>
        public void SetSelectionModeNone()
        {
            this.SelectionTransformer = new Transformer();
            this.SelectionLayer = null;
            this.SelectionLayers = null;

            this.SelectionMode = ListViewSelectionMode.None;

            //////////////////////////

            this.SetSelectionOpacity(1.0f);
            this.SetSelectionBlendType(BlendType.Normal);
            this.SelectionIsVisual = false;
        }

        /// <summary>
        /// Sets <see cref = "ViewModel.SelectionMode" /> to Single.
        /// </summary>
        /// <param name="layer"> The selection layer. </param>
        public void SetSelectionModeSingle(Layer layer)
        {
            this.SelectionTransformer = layer.TransformerMatrix.Destination;
            this.SelectionLayer = layer;
            this.SelectionLayers = null;

            this.SelectionMode = ListViewSelectionMode.Single;

            //////////////////////////

            this.SetSelectionOpacity(layer.Opacity);
            this.SetSelectionBlendType(layer.BlendType);
            this.SelectionIsVisual = layer.IsVisual;

            if (layer.GetFillColor() is Color color)
            {
                this.FillColor = color;
            }
        }

        /// <summary>
        /// Sets <see cref = "ViewModel.SelectionMode" /> to Multiple.
        /// </summary>
        /// <param name="layers"> All selection layers. </param>
        public void SetSelectionModeMultiple(IEnumerable<Layer> layers)
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

            this.SelectionTransformer = new Transformer(left, top, right, bottom);
            this.SelectionLayer = null;
            this.SelectionLayers = layers;

            this.SelectionMode = ListViewSelectionMode.Multiple;//Transformer
        }


        /// <summary> Sets <see cref = "ViewModel.SelectionMode" />. </summary>
        public void SetSelectionMode()
        {
            IEnumerable<Layer> checkedLayers = from item in this.Layers where item.IsChecked select item;
            int count = checkedLayers.Count();

            if (count == 0)
                this.SetSelectionModeNone();//None

            else if (count == 1)
                this.SetSelectionModeSingle(checkedLayers.Single());//Single

            else if (count >= 2)
                this.SetSelectionModeMultiple(checkedLayers);//Multiple
        }


        /////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Gets selection layer(s)'s transformer.
        /// </summary>
        /// <param name="action"> action </param>
        public Transformer GetSelectionTransformer( )
        {
            if (this.SelectionMode== ListViewSelectionMode.Single)
            {
                return this.SelectionLayer.TransformerMatrix.Destination;
            }

            return this.SelectionTransformer;
        }

        /// <summary>
        /// Sets all selection layer(s).
        /// </summary>
        /// <param name="action"> action </param>
        public void SelectionSetValue(Action<Layer> action)
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    action(this.SelectionLayer);
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layer layer in this.SelectionLayers)
                    {
                        action(layer);
                    }
                    break;
            }
        }


        /// <summary> <see cref = "ViewModel.Selection" />'s opacity. </summary>
        public float SelectionOpacity;
        /// <summary> Sets the <see cref = "ViewModel.SelectionOpacity" />. </summary>
        public void SetSelectionOpacity(float value)
        {
            if (this.SelectionOpacity == value) return;
            this.SelectionOpacity = value;
            this.OnPropertyChanged(nameof(this.SelectionOpacity));//Notify 
        }


        /// <summary> <see cref = "ViewModel.Selection" />'s blend type. </summary>
        public BlendType SelectionBlendType;
        /// <summary> Sets the <see cref = "ViewModel.SelectionBlend" />. </summary>
        public void SetSelectionBlendType(BlendType value)
        {
            if (this.SelectionBlendType == value) return;
            this.SelectionBlendType = value;
            this.OnPropertyChanged(nameof(this.SelectionBlendType));//Notify 
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