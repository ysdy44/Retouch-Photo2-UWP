using Retouch_Photo.Models;
using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.Models.Blends;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Controls.LayerControls
{
    public sealed partial class LayerPropertyControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        #region DependencyProperty
        
        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty,value); }
        }
        public static readonly DependencyProperty LayerProperty =  DependencyProperty.Register(nameof(Layer),typeof(Layer),typeof(LayerPropertyControl),new PropertyMetadata(null,(sender,e)=> 
        {
            LayerPropertyControl con = (LayerPropertyControl)sender;

            if (e.NewValue is Layer layer)
            {
                con.Invalidate(layer.Adjustments);
            }
        }));
        
        #endregion


        public LayerPropertyControl()
        {
            this.InitializeComponent();
                       
            Adjustment.RemoveChanged += (adjustment) =>
            {
                this.Remove(adjustment);
            };
        }
        

        private void Slider_ValueChanged(object sender,RangeBaseValueChangedEventArgs e) => this.ViewModel.Invalidate();
        private void ComboBox_SelectionChanged(object sender,SelectionChangedEventArgs e) => this.ViewModel.Invalidate();


        private void LayerAdjustmentCandidateControl_AddChanged(Adjustment adjustment) => this.Add(adjustment);
        private void LayerAdjustmentControl_RemoveChanged(Adjustment adjustment) => this.Remove(adjustment);
        private void EffectButton_Tapped(object sender,TappedRoutedEventArgs e)
        {
        }



        //Adjustment
        private void Add(Adjustment adjustment)
        {
            this.Layer.Adjustments.Add(adjustment);
            this.Invalidate(this.Layer.Adjustments);
            this.ViewModel.Invalidate();
        }
        private void Remove(Adjustment adjustment)
        {
            this.Layer.Adjustments.Remove(adjustment);
            this.Invalidate(this.Layer.Adjustments);
            this.ViewModel.Invalidate();
        }


        public void Invalidate(List<Adjustment> adjustments)
        {
            if (adjustments == null) return;

            this.AdjustmentsItemsControl.ItemsSource = null;
            this.AdjustmentsItemsControl.ItemsSource = adjustments;

            this.AdjustmentTextBlock.Visibility =
            this.AdjustmentBorder.Visibility =
                 adjustments.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

    }
}
