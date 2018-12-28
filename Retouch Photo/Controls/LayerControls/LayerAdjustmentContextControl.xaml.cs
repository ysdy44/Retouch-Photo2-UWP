using Retouch_Photo.Models;
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
    public sealed partial class LayerAdjustmentContextControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        #region DependencyProperty

        public Adjustment Adjustment
        {
            get { return (Adjustment)GetValue(AdjustmentProperty); }
            set { SetValue(AdjustmentProperty, value); }
        }
        public static readonly DependencyProperty AdjustmentProperty = DependencyProperty.Register(nameof(Adjustment), typeof(Adjustment), typeof(LayerAdjustmentContextControl), new PropertyMetadata(null,(sender,e)=>
        {
            LayerAdjustmentContextControl con = (LayerAdjustmentContextControl)sender;

            if (e.NewValue  is Adjustment adjustment)
            {
                AdjustmentCandidate adjustmentCandidate = AdjustmentCandidate.GetAdjustmentCandidate(adjustment.Type);
                if (adjustmentCandidate.Page == null) return;

                adjustmentCandidate.SetPage(adjustment);
                con.AdjustmentFrame.Child = adjustmentCandidate.Page;
                 
                con.Visibility = Visibility.Visible;
            }
        }));

        #endregion


        public LayerAdjustmentContextControl()
        {
            this.InitializeComponent();
        }

        private void BackButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Clear();
        private void ResetButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Reset();


        private void Reset()
        {
            if (this.Adjustment == null) return;

            this.Adjustment.Reset();

            AdjustmentCandidate adjustmentCandidate = AdjustmentCandidate.GetAdjustmentCandidate(this.Adjustment.Type);
            adjustmentCandidate.SetPage(this.Adjustment);

            this.ViewModel.Invalidate();
        }
        private void Clear()
        {
            this.Adjustment = null;
            this.AdjustmentFrame.Child = null;
            this.Visibility = Visibility.Collapsed;
        }

    }
}
