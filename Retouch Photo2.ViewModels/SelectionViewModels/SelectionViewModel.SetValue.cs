using FanKit.Transformers;
using Retouch_Photo2.Layers;
using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some selection propertys of the application.
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
                    this.SelectionLayerage.SetValue(action);
                    break;

                case ListViewSelectionMode.Multiple:
                    foreach (Layerage child in this.SelectionLayerages)
                    {
                        child.SetValue(action);
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
                    this.SelectionLayerage.SetValueWithChildren(action);
                    break;

                case ListViewSelectionMode.Multiple:
                    foreach (Layerage child in this.SelectionLayerages)
                    {
                        child.SetValueWithChildren(action);
                    }
                    break;
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
                    this.SelectionLayerage.SetValueWithChildrenOnlyGroup(action);
                    break;

                case ListViewSelectionMode.Multiple:
                    foreach (Layerage child in this.SelectionLayerages)
                    {
                        child.SetValueWithChildrenOnlyGroup(action);
                    }
                    break;
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
                    return this.SelectionLayerage.GetActualTransformer();

                case ListViewSelectionMode.Multiple:
                    //TransformerBorder
                    TransformerBorder border = new TransformerBorder(this.SelectionLayerages);
                    return border.ToTransformer();

                default:
                    return new Transformer();
            }
        }


        /// <summary>
        /// Get the selected layerage.
        /// None: null;
        /// Single: layer;
        /// Multiple: first layer;
        /// </summary>
        /// <returns> The selected layerage. </returns>
        public Layerage GetFirstSelectedLayerage()
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None: return null;
                case ListViewSelectionMode.Single:
                    return this.SelectionLayerage;
                case ListViewSelectionMode.Multiple:
                    return this.SelectionLayerages.FirstOrDefault();
                default: return null;
            }
        }

    }
}