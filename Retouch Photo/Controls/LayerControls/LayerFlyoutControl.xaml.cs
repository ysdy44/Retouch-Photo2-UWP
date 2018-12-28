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
    public sealed partial class LayerFlyoutControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        #region DependencyProperty
        
        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty,value); }
        }
        public static readonly DependencyProperty LayerProperty =  DependencyProperty.Register(nameof(Layer),typeof(Layer),typeof(LayerFlyoutControl),new PropertyMetadata(null,(sender,e)=> 
        {
            LayerFlyoutControl con = (LayerFlyoutControl)sender;

            if (e.NewValue is Layer layer)
            {
                con.LayerAdjustmentContextControl.Visibility = Visibility.Collapsed;
                con.Invalidate(layer.Adjustments);
            }
        }));
        
        #endregion


        public LayerFlyoutControl()
        {
            this.InitializeComponent();
        }

        //Flyout
        private void Slider_ValueChanged(object sender,RangeBaseValueChangedEventArgs e) => this.ViewModel.Invalidate();

        private void LayerBlendControl_IndexChanged(int index) => this.ViewModel.Invalidate();
                
        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e)=>  this.ViewModel.RenderLayer.Remove(this.Layer);
        private void AdjustmentButton_Tapped(object sender, TappedRoutedEventArgs e) => this.AdjustmentCandidateFlyout.ShowAt((Button)sender);
        private void EffectButton_Tapped(object sender,TappedRoutedEventArgs e)
        {
        }



        //Adjustment
        private void LayerAdjustmentControl_AdjustmentRemove(Adjustment adjustment) => this.Remove(adjustment);
        private void LayerAdjustmentControl_AdjustmentContext(Adjustment adjustment) => this.LayerAdjustmentContextControl.Adjustment = adjustment;
        //AdjustmentCandidate
        private void AdjustmentCandidateListView_Loaded(object sender, RoutedEventArgs e)=> ((ListView)sender).ItemsSource = AdjustmentCandidate.AdjustmentCandidateList;
        private void AdjustmentCandidateListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is AdjustmentCandidate item)
            {
                Adjustment adjustment = item.GetNewAdjustment();
                this.Add(adjustment);
            }
            this.AdjustmentCandidateFlyout.Hide();
        }


        #region Adjustment

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







        #endregion


    }
}
