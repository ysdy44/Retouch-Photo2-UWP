using Retouch_Photo.Models;
using Retouch_Photo.Models.Adjustments;
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

        public Layer Layer{get; set ; }

        #endregion


        public LayerPropertyControl()
        {
            this.InitializeComponent();
        }
        

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) => this.ViewModel.Invalidate();
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.ViewModel.Invalidate();


        private void LayerAdjustmentCandidateControl_AddChanged(Adjustment adjustment) => this.Add(adjustment);
        private void LayerAdjustmentControl_RemoveChanged(Adjustment adjustment) => this.Remove(adjustment);
        private void EffectButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }


        //Adjustment
        private void Add(Adjustment adjustment)
        {
            this.Layer.Adjustments.Add(adjustment);
            this.LayerAdjustmentControl.Invalidate();
            this.ViewModel.Invalidate();
        }
        private void Remove(Adjustment adjustment)
        {
            this.Layer.Adjustments.Remove(adjustment);
            this.LayerAdjustmentControl.Invalidate();
            this.ViewModel.Invalidate();
        }

    }
}
