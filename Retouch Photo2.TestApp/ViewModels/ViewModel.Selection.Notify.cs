using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Transformers;
using System;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary>
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel
    {

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
        

        /// <summary> <see cref = "ViewModel.Selection" />'s EffectManager. </summary>
        public EffectManager SelectionIsEffectManager
        {
            get => this.selectionIsEffectManager;
            set
            {
                this.selectionIsEffectManager = value;
                this.OnPropertyChanged(nameof(this.SelectionIsEffectManager));//Notify 
            }
        }
        private EffectManager selectionIsEffectManager;

        

    }
}