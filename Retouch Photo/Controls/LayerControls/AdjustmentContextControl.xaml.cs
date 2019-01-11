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
    public sealed partial class AdjustmentContextControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        #region DependencyProperty

        public Adjustment Adjustment
        {
            get { return (Adjustment)GetValue(AdjustmentProperty); }
            set { SetValue(AdjustmentProperty, value); }
        }
        public static readonly DependencyProperty AdjustmentProperty = DependencyProperty.Register(nameof(Adjustment), typeof(Adjustment), typeof(AdjustmentContextControl), new PropertyMetadata(null,(sender,e)=>
        {
            AdjustmentContextControl con = (AdjustmentContextControl)sender;

            if (e.NewValue  is Adjustment adjustment)
            {
                if (adjustment.HasPage)
                {
                    AdjustmentCandidate adjustmentCandidate = AdjustmentCandidate.GetAdjustmentCandidate(adjustment.Type);

                    adjustmentCandidate.SetPage(adjustment);
                    con.AdjustmentFrame.Child = adjustmentCandidate.Page;

                    con.Visibility = Visibility.Visible;
                }
            }
        }));

        #endregion


        public AdjustmentContextControl()
        {
            this.InitializeComponent();
        }

        private void BackButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Clear();
        private void ResetButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Reset();

        /// <summary> 重置 </summary>
        private void Reset()
        {
            if (this.Adjustment == null) return;

            this.Adjustment.Reset();

            AdjustmentCandidate adjustmentCandidate = AdjustmentCandidate.GetAdjustmentCandidate(this.Adjustment.Type);
            adjustmentCandidate.SetPage(this.Adjustment);

            this.ViewModel.Invalidate();
        }
        /// <summary> 清空 </summary>
        private void Clear()
        {
            this.Adjustment = null;
            this.AdjustmentFrame.Child = null;
            this.Visibility = Visibility.Collapsed;
        }

    }
}
