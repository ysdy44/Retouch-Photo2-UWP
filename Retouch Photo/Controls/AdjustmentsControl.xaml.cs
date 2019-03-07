using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo.Adjustments;
using System;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.Storage;
using Windows.System;
using Retouch_Photo.Adjustments.Models;
using Retouch_Photo.Adjustments.Items;

namespace Retouch_Photo.Controls
{ 
    public sealed partial class AdjustmentsControl : UserControl
    {      
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public bool? ShowVisibility
        {
            set
            {
                this.BackButton.Visibility =
                this.ResetButton.Visibility =
                this.Frame.Visibility =
                (value == null) ? Visibility.Visible : Visibility.Collapsed;

                this.AddButton.Visibility =
                this.FilterButton.Visibility =
                (value == null) ? Visibility.Collapsed : Visibility.Visible;

                this.Border.Visibility = (value == true) ? Visibility.Visible : Visibility.Collapsed;
                this.TextBlock.Visibility = (value == false) ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        private Adjustment adjustment;
        public Adjustment Adjustment
        {
            get => this.adjustment;
            set
            {
                if (value == null) return;
                if (!value.HasPage) return;

                AdjustmentCandidate adjustmentCandidate = AdjustmentCandidate.GetAdjustmentCandidate(value.Type);

                adjustmentCandidate.SetPage(value);
                this.Frame.Child = adjustmentCandidate.Page;
                this.ShowVisibility = null;

                this.AddButton.IsEnabled = true;

                this.adjustment = value;
            }
        }


        #region DependencyProperty


        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(AdjustmentsControl), new PropertyMetadata(null, (sender, e) =>
        {
            AdjustmentsControl con = (AdjustmentsControl)sender;

            if (e.NewValue is Layer layer)
            {
                con.IsEnabled = true;

                con.ShowVisibility = null;
                con.Invalidate(layer.AdjustmentManager.Adjustments);
            }
            else
            {
                con.IsEnabled = false;

                con.Invalidate(null);
            }
        }));


        #endregion


        public AdjustmentsControl()
        {
            this.InitializeComponent();

            //Adjustment
            Retouch_Photo.Adjustments.Adjustment.InvalidateCall += () => this.ViewModel.Invalidate();

            this.ShowVisibility = false;


            //AdjustmentCandidate
            this.ListView.Loaded += (sender, e) => this.ListView.ItemsSource = AdjustmentCandidate.AdjustmentCandidateList;
            this.ListView.ItemClick += (sender, e) =>
            {
                if (e.ClickedItem is AdjustmentCandidate item)
                {
                    Adjustment adjustment = item.GetNewAdjustment();
                    this.Add(adjustment);
                    this.CandidateFlyout.Hide(); 
                }
            };


            //Button
            this.BackButton.Tapped += (sender, e) => this.Clear();
            this.ResetButton.Tapped += (sender, e) => this.Reset();
            this.FilterButton.Tapped += (sender, e) => this.FilterFlyout.ShowAt(this.FilterButton);
            this.AddButton.Tapped += (sender, e) =>
            {
                if (this.Layer == null)
                {
                    this.IsEnabled = false;
                    return;
                }

                this.CandidateFlyout.ShowAt((Button)sender);
            };


            //Filter
            this.FilterControl.AdjustmentsClick += (adjustments) =>
            {

                this.Replace(adjustments);
            };
        }


        //Adjustment
        private void AdjustmentControl_AdjustmentRemove(Adjustment adjustment) => this.Remove(adjustment);
        private void AdjustmentControl_AdjustmentContext(Adjustment adjustment) => this.Adjustment = adjustment;


        /// <summary> Add a Adjustment. </summary>
        private void Add(Adjustment adjustment)
        {
            if (this.Layer == null) return;
            this.Layer.AdjustmentManager.Adjustments.Add(adjustment);
            this.Invalidate(this.Layer.AdjustmentManager.Adjustments);
            this.ViewModel.Invalidate();
        }
        /// <summary> Remove the Adjustment. </summary>
        private void Remove(Adjustment adjustment)
        {
            if (this.Layer == null) return;
            this.Layer.AdjustmentManager.Adjustments.Remove(adjustment);
            this.Invalidate(this.Layer.AdjustmentManager.Adjustments);
            this.ViewModel.Invalidate();
        }
        /// <summary> Replace the Adjustment. </summary>
        private void Replace(IEnumerable<Adjustment> adjustments)
        {
            if (this.Layer == null) return;
            this.Layer.AdjustmentManager.Adjustments.Clear();
            this.Layer.AdjustmentManager.Adjustments.AddRange(adjustments);
            this.Invalidate(this.Layer.AdjustmentManager.Adjustments);
            this.ViewModel.Invalidate();
        }


        /// <summary>
        /// Invalidate Adjustment ItemsControl , 
        /// </summary>
        /// <param name="adjustments">Adjustment List</param>
        public void Invalidate(List<Adjustment> adjustments)
        {
            this.ItemsControl.ItemsSource = null;

            if (adjustments == null) this.ShowVisibility = false;
            else
            {
                this.ItemsControl.ItemsSource = adjustments;
                this.ShowVisibility = !(adjustments.Count == 0);
            }
        }



        /// <summary> Reset the Adjustment. </summary>
        private void Reset()
        {
            if (this.Adjustment == null) return;

            this.Adjustment.Reset();

            AdjustmentCandidate adjustmentCandidate = AdjustmentCandidate.GetAdjustmentCandidate(this.Adjustment.Type);
            adjustmentCandidate.SetPage(this.Adjustment);

            this.ViewModel.Invalidate();
        }
        /// <summary> Clear the Adjustment. </summary>
        private void Clear()
        {
            this.Adjustment = null;
            this.Frame.Child = null;
            this.ShowVisibility = true;
        }

        
    }
}
