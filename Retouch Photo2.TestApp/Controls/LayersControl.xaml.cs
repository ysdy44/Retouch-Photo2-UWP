using Retouch_Photo2.Layers;
using Retouch_Photo2.Library;
using Retouch_Photo2.TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public sealed partial class LayersControl : UserControl
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        public LayersControl()
        {
            this.InitializeComponent();

            //Layer : ItemClick
            Layer.ItemClickAction = (layer, placementTarget) =>
            {
                this.ViewModel.LayerSingleChecked(layer);//Layer

                this.ViewModel.SetSelectionModeSingle(layer);//Transformer

                this.ViewModel.Invalidate();//Invalidate
            };

            //Layer : ItemVisualChanged
            Layer.ItemVisualChangedAction = (layer) =>
            {
                bool isVisual = !layer.IsVisual;

                if (layer.IsChecked == false) layer.IsVisual = isVisual;
                else
                {
                    foreach (Layer item in this.ViewModel.Layers)
                    {
                        if (item.IsChecked)    item.IsVisual = isVisual;
                    }
                }

                this.ViewModel.Invalidate();//Invalidate
            };

            //Layer : ItemIsCheckedChanged
            Layer.ItemIsCheckedChangedAction = (layer) =>
            {
                layer.IsChecked = !layer.IsChecked;

                this.ViewModel.SetSelectionMode(this.ViewModel.Layers);//Transformer

                this.ViewModel.Invalidate();//Invalidate
            };


            this.AddButton.Tapped += (s, e) =>
            {

            };
        }


    }
}