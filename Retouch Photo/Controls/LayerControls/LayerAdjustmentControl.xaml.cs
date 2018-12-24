using Retouch_Photo.Models;
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
    public sealed partial class LayerAdjustmentControl : UserControl
    {
        
        #region DependencyProperty

        public List<Adjustment> Adjustments { get; set; }

        #endregion

        //Delegate
        public delegate void RemoveChangedHandler(Adjustment adjustment);
        public event RemoveChangedHandler RemoveChanged = null;
        

        public void Invalidate()
        {
            this.AdjustmentsItemsControl.ItemsSource = null;
            this.AdjustmentsItemsControl.ItemsSource = this.Adjustments;

            this.Visibility = this.Adjustments.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public LayerAdjustmentControl()
        {
            this.InitializeComponent();

            Adjustment.RemoveChanged += (adjustment) =>
            {
                 this.RemoveChanged?.Invoke(adjustment);
            };
        }


    }
}
