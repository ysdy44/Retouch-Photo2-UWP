using FanKit.Transformers;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Sets all selection layer(s))' value .
        /// </summary>
        /// <param name="action"> action </param>
        public void SetValue(Action<Layerage> action)
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;

                case ListViewSelectionMode.Single:
                    action(this.Layerage);
                    break;

                case ListViewSelectionMode.Multiple:
                    foreach (Layerage child in this.Layerages)
                    {
                        action(child);
                    }
                    break;
            }
        }
        /// <summary>
        /// Sets all selection layer(s))' value  with children.
        /// </summary>
        /// <param name="action"> action </param>
        public void SetValueWithChildren(Action<Layerage> action)
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;

                case ListViewSelectionMode.Single:
                    this._setValueWithChildren(this.Layerage, action);
                    break;

                case ListViewSelectionMode.Multiple:
                    foreach (Layerage child in this.Layerages)
                    {
                        this._setValueWithChildren(child, action);
                    }
                    break;
            }
        }
        private void _setValueWithChildren(Layerage layerage, Action<Layerage> action)
        {
            action(layerage);
            foreach (Layerage child in layerage.Children)
            {
                this._setValueWithChildren(child, action);
            }
        }
        /// <summary>
        /// Sets all selection layer(s)' value with children only group.
        /// </summary>
        /// <param name="action"> action </param>
        public void SetValueWithChildrenOnlyGroup(Action<Layerage> action)
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;

                case ListViewSelectionMode.Single:
                    this._setValueWithChildren(this.Layerage, action);
                    break;

                case ListViewSelectionMode.Multiple:
                    foreach (Layerage child in this.Layerages)
                    {
                        this._setValueWithChildren(child, action);
                    }
                    break;
            }
        }
        private void _setValueWithChildrenOnlyGroup(Layerage layerage, Action<Layerage> action)
        {
            action(layerage);

            ILayer layer = layerage.Self;

            if (layer.Type == LayerType.Group)
            {
                foreach (Layerage child in layerage.Children)
                {
                    this._setValueWithChildren(child, action);
                }
            }
        }


        /// <summary>
        /// Refactoring the transformer.
        /// </summary>
        /// <returns> The transformer. </returns>
        public Transformer RefactoringTransformer()
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    return new Transformer();

                case ListViewSelectionMode.Single:
                    ILayer layer = this.Layerage.Self;
                    return layer.GetActualDestinationWithRefactoringTransformer;

                case ListViewSelectionMode.Multiple:
                    {
                        //TransformerBorder
                        IEnumerable<Transformer> transformers = from l in this.Layerages select l.Self.GetActualDestinationWithRefactoringTransformer;
                        TransformerBorder border = new TransformerBorder(transformers);
                        return border.ToTransformer();
                    }

                default:
                    return new Transformer();
            }
        }


        /// <summary>
        /// Get the selected layer.
        /// None: null;
        /// Single: layer;
        /// Multiple: first layer;
        /// </summary>
        /// <returns> The selected layer. </returns>
        public Layerage GetFirstLayer()
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None: return null;
                case ListViewSelectionMode.Single: return this.Layerage;
                case ListViewSelectionMode.Multiple: return this.Layerages.FirstOrDefault();
                default: return null;
            }
        }

    }
}
