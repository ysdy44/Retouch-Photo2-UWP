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

namespace Retouch_Photo.Controls
{
    public sealed partial class LayerControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        #region DependencyProperty

        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(LayerControl), new PropertyMetadata(null, (sender, e) =>
        {
            LayerControl con = (LayerControl)sender;

            if (e.NewValue is Layer layer)
            {
                con.IsEnabled = true;

                con.AdjustmentContextControl.Visibility = Visibility.Collapsed;
                con.Invalidate(layer.Adjustments);
            }
            else
            {
                con.IsEnabled = false;
                con.Invalidate(null);
            }
        }));

        #endregion


        public LayerControl()
        {
            this.InitializeComponent();
        }

        //Flyout
        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) => this.ViewModel.Invalidate();

        private void BlendControl_IndexChanged(int index) => this.ViewModel.Invalidate();

        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.ViewModel.RenderLayer.Remove(this.Layer);
            this.ViewModel.CurrentLayer = null;
            this.Layer = null;
        }
        private void AdjustmentButton_Tapped(object sender, TappedRoutedEventArgs e) => this.AdjustmentCandidateFlyout.ShowAt((Button)sender);
        private void EffectButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }



        //Adjustment
        private void AdjustmentControl_AdjustmentRemove(Adjustment adjustment) => this.Remove(adjustment);
        private void AdjustmentControl_AdjustmentContext(Adjustment adjustment) => this.AdjustmentContextControl.Adjustment = adjustment;
        //AdjustmentCandidate
        private void AdjustmentCandidateListView_Loaded(object sender, RoutedEventArgs e) => ((ListView)sender).ItemsSource = AdjustmentCandidate.AdjustmentCandidateList;
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

        /// <summary>
        /// 通过Adjustment列表，决定控件的初始化与可视
        /// </summary>
        /// <param name="adjustments">Adjustment List</param>
        public void Invalidate(List<Adjustment> adjustments)
        {
            if (adjustments == null)
            {
                this.AdjustmentsItemsControl.ItemsSource = null;
                this.AdjustmentTextBlock.Visibility = this.AdjustmentBorder.Visibility = Visibility.Collapsed;
                return;
            }

            this.AdjustmentsItemsControl.ItemsSource = null;
            this.AdjustmentsItemsControl.ItemsSource = adjustments;
            this.AdjustmentTextBlock.Visibility = this.AdjustmentBorder.Visibility = adjustments.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }







        #endregion


    }
}
