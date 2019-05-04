using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    public enum AdjustmentsControlState
    {
        None,
        Disable,
        Null,
        Adjustments,
        Edit
    }

    public sealed partial class AdjustmentsControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        private AdjustmentsControlState state= AdjustmentsControlState.None;
        public AdjustmentsControlState State
        {
            set
            {
                if (this.state == value) return;

                this.BackButton.Visibility = this.ResetButton.Visibility = this.Frame.Visibility = (value == AdjustmentsControlState.Edit) ? Visibility.Visible : Visibility.Collapsed;
                this.AddButton.Visibility = this.FilterButton.Visibility = (value == AdjustmentsControlState.Edit) ? Visibility.Collapsed : Visibility.Visible;
                this.ListView.IsEnabled = this.GridView.IsEnabled = (value == AdjustmentsControlState.Null || value == AdjustmentsControlState.Adjustments);
                this.Border.Visibility = (value == AdjustmentsControlState.Adjustments) ? Visibility.Visible : Visibility.Collapsed;
                this.TextBlock.Visibility = (value == AdjustmentsControlState.Null || value == AdjustmentsControlState.Disable) ? Visibility.Visible : Visibility.Collapsed;

                this.state = value;
            }
        }

        private AdjustmentPage page;
        public AdjustmentPage Page
        {
            get => this.page;
            set
            {
                this.Frame.Child = value;

                this.page = value;
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
                con.Invalidate(layer.AdjustmentManager.Adjustments);
            }
            else
            {
                con.State = AdjustmentsControlState.Disable;
            }
        }));


        #endregion


        public AdjustmentsControl()
        {
            this.InitializeComponent();
            this.State = AdjustmentsControlState.Disable;
            this.Loaded += async (s, e) =>
            {
                if (this.ListView.ItemsSource == null)
                    this.ListView.ItemsSource = AdjustmentPage.PageList;

                if (this.GridView.ItemsSource == null)
                    this.GridView.ItemsSource = (await AdjustmentsControl.GetFilterSource()).ToList();
            };

            //Adjustment
            Retouch_Photo2.Adjustments.Adjustment.Invalidate =()=> this.ViewModel.Invalidate();

            //Button
            this.FilterButton.Tapped += (sender, e) => this.FilterFlyout.ShowAt(this.FilterButton);
            this.AddButton.Tapped += (sender, e) => this.PageFlyout.ShowAt((Button)sender);
            this.BackButton.Tapped += (sender, e) => this.Close();
            this.ResetButton.Tapped += (sender, e) => this.Reset();
            
            //Page
            this.ListView.ItemClick += (sender, e) =>
            {
                if (e.ClickedItem is AdjustmentPage item)
                {
                    Adjustment adjustment = item.GetNewAdjustment();
                    this.Add(adjustment);
                    this.PageFlyout.Hide();
                }
            };
            
            //Filter
            this.GridView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Filter filter)
                {
                    // Filter -- > List<Item> -- > List<Adjustment>
                    IEnumerable<Adjustment> adjustments =
                       from adjustmentItem
                       in filter.AdjustmentItems
                       select adjustmentItem.GetNewAdjustment();

                    this.Replace(adjustments);
                }
            };            
        }
        
        //Adjustment
        private void AdjustmentControl_Remove(Adjustment adjustment) => this.Remove(adjustment);
        private void AdjustmentControl_Edit(Adjustment adjustment) => this.Edit( adjustment);
        
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
        /// <summary> Replace all Adjustments. </summary>
        private void Replace(IEnumerable<Adjustment> adjustments)
        {
            if (this.Layer == null) return;

            this.Layer.AdjustmentManager.Adjustments.Clear();
            foreach (Adjustment adjustment in adjustments)
            {
                this.Layer.AdjustmentManager.Adjustments.Add(adjustment);
            }

            this.Invalidate(this.Layer.AdjustmentManager.Adjustments);
            this.ViewModel.Invalidate();
        }
         
        /// <summary> Edit the Adjustment. </summary>
        public void Edit(Adjustment adjustment)
        {
            if (adjustment == null) return;
            if (!adjustment.HasPage) return;

            AdjustmentPage adjustmentPage = AdjustmentPage.GetPage(adjustment.Type);

            adjustmentPage.SetAdjustment(adjustment);
            this.Page= adjustmentPage;

            this.State = AdjustmentsControlState.Edit;
        }
        /// <summary> Clear the Adjustment. </summary>
        private void Close()
        {
            if (this.Page == null) return;

            this.Page.Close();            
            this.Page = null;

            this.State = AdjustmentsControlState.Adjustments;
        }     
        /// <summary> Reset the Adjustment. </summary>
        private void Reset()
        {
            if (this.Page == null) return;
            
            this.Page.Reset();
            this.ViewModel.Invalidate();
        }
                          
        /// <summary> Invalidate Adjustment ItemsControl. </summary>
        public void Invalidate(IEnumerable<Adjustment> adjustments)
        {
            if (adjustments == null) return;

            this.ItemsControl.ItemsSource = null;
            this.ItemsControl.ItemsSource = adjustments;

            this.State = (this.Layer.AdjustmentManager.Adjustments.Count == 0) ? AdjustmentsControlState.Null : AdjustmentsControlState.Adjustments;
        }
        
        #region File


        //Filter
        public static async Task<IEnumerable<Filter>> GetFilterSource()
        {
            string json = await AdjustmentsControl.ReadFromLocalFolder("Filter.json");

            if (json == null)
            {
                json = await AdjustmentsControl.ReadFromApplicationPackage("ms-appx:///Json/Filter.json");
                AdjustmentsControl.WriteToLocalFolder(json, "ms-appx:///Json/Filter.json");
            }
            IEnumerable<Filter> source = Filter.GetFiltersFromJson(json);

            return source;
        }

        /// <summary> Read json file from Application Package. </summary> 
        public static async Task<string> ReadFromApplicationPackage(string fileName)
        {
            Uri uri = new Uri(fileName);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            return await FileIO.ReadTextAsync(file);
        }

        /// <summary> Read json file from Local Folder. </summary> 
        public static async Task<string> ReadFromLocalFolder(string fileName)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(file);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary> Write json file to Local Folder. </summary> 
        public static async void WriteToLocalFolder(string json, string fileName)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, json);
            }
            catch (Exception)
            {
            }
        }


        #endregion
    }
}
