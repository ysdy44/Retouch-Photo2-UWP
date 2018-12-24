using Retouch_Photo.Models;
using Retouch_Photo.Models.Adjustments;
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
    public sealed partial class LayerAdjustmentCandidateControl : UserControl
    {
        //AdjustmentCandidate
        List<AdjustmentCandidate> AdjustmentCandidates = new List<AdjustmentCandidate>()
        {
            new GrayAdjustmentCandidate(),
            new InvertAdjustmentCandidate(),
            new ExposureAdjustmentCandidate(),
        };

        //Delegate
        public delegate void AddChangedHandler(Adjustment adjustment);
        public event AddChangedHandler AddChanged = null;

        public LayerAdjustmentCandidateControl()
        {
            this.InitializeComponent();
        }

        private void AdjustmentCandidateListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is AdjustmentCandidate item)
            {
                Adjustment adjustment = item.GetNewAdjustment();
                this.AddChanged?.Invoke(adjustment);
            }
            this.AdjustmentCandidateFlyout.Hide();
        }

    }
}
