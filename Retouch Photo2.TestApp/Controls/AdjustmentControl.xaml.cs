using Retouch_Photo2.Adjustments;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Controls
{
    public enum AdjustmentControlState
    {
        None,
        Disable,
        Null,
        Adjustments,
        Edit
    }

    public sealed partial class AdjustmentControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;
        SelectionViewModel SelectionViewModel  => Retouch_Photo2.TestApp.App.SelectionViewModel;


        private AdjustmentControlState state = AdjustmentControlState.None;
        public AdjustmentControlState State
        {
            set
            {
                if (this.state == value) return;

                this.BackButton.Visibility = this.ResetButton.Visibility = this.Frame.Visibility = (value == AdjustmentControlState.Edit) ? Visibility.Visible : Visibility.Collapsed;
                this.AddButton.Visibility = this.FilterButton.Visibility = (value == AdjustmentControlState.Edit) ? Visibility.Collapsed : Visibility.Visible;
                this.ListView.IsEnabled = this.GridView.IsEnabled = (value == AdjustmentControlState.Null || value == AdjustmentControlState.Adjustments);
                this.Border.Visibility = (value == AdjustmentControlState.Adjustments) ? Visibility.Visible : Visibility.Collapsed;
                this.TextBlock.Visibility = (value == AdjustmentControlState.Null || value == AdjustmentControlState.Disable) ? Visibility.Visible : Visibility.Collapsed;

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


        public AdjustmentManager AdjustmentManager
        {
            get { return (AdjustmentManager)GetValue(AdjustmentManagerProperty); }
            set { SetValue(AdjustmentManagerProperty, value); }
        }
        public static readonly DependencyProperty AdjustmentManagerProperty = DependencyProperty.Register(nameof(AdjustmentManager), typeof(AdjustmentManager), typeof(AdjustmentControl), new PropertyMetadata(null, (sender, e) =>
        {
            AdjustmentControl con = (AdjustmentControl)sender;

            if (e.NewValue is AdjustmentManager value)
            {
                con.Invalidate(value.Adjustments);
            }
            else
            {
                con.State = AdjustmentControlState.Disable;
            }
        }));


        #endregion


        public AdjustmentControl()
        {
            this.InitializeComponent();
            this.State = AdjustmentControlState.Disable;
            this.Loaded += async (s, e) =>
            {
                if (this.ListView.ItemsSource == null)
                    this.ListView.ItemsSource = AdjustmentPage.PageList;

                if (this.GridView.ItemsSource == null)
                    this.GridView.ItemsSource = (await AdjustmentControl.GetFilterSource()).ToList();
            };

            //Adjustment
            Retouch_Photo2.Adjustments.Adjustment.Invalidate = () => this.ViewModel.Invalidate();

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
        private void AdjustmentControl_Edit(Adjustment adjustment) => this.Edit(adjustment);

        /// <summary> Add a Adjustment. </summary>
        private void Add(Adjustment adjustment)
        {
            //Selection
            this.SelectionViewModel.SetValue((layer)=>
            {
                layer.AdjustmentManager.Adjustments.Add(adjustment);

                this.Invalidate(layer.AdjustmentManager.Adjustments);
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        /// <summary> Remove the Adjustment. </summary>
        private void Remove(Adjustment adjustment)
        {
            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.AdjustmentManager.Adjustments.Remove(adjustment);

                this.Invalidate(layer.AdjustmentManager.Adjustments);
            }); 

            this.ViewModel.Invalidate();//Invalidate
        }
        /// <summary> Replace all Adjustments. </summary>
        private void Replace(IEnumerable<Adjustment> adjustments)
        {
            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.AdjustmentManager.Adjustments.Clear();
                foreach (Adjustment adjustment in adjustments)
                {
                    layer.AdjustmentManager.Adjustments.Add(adjustment);
                }
                this.Invalidate(layer.AdjustmentManager.Adjustments);
            });
             
            this.ViewModel.Invalidate();//Invalidate
        }

        /// <summary> Edit the Adjustment. </summary>
        public void Edit(Adjustment adjustment)
        {
            if (adjustment == null) return;
            if (!adjustment.HasPage) return;

            AdjustmentPage adjustmentPage = AdjustmentPage.GetPage(adjustment.Type);

            adjustmentPage.SetAdjustment(adjustment);
            this.Page = adjustmentPage;

            this.State = AdjustmentControlState.Edit;
        }
        /// <summary> Clear the Adjustment. </summary>
        private void Close()
        {
            if (this.Page == null) return;

            this.Page.Close();
            this.Page = null;

            this.State = AdjustmentControlState.Adjustments;
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

            this.State = (this.SelectionViewModel.Layer.AdjustmentManager.Adjustments.Count == 0) ? AdjustmentControlState.Null : AdjustmentControlState.Adjustments;
        }

        #region File


        //Filter
        public static async Task<IEnumerable<Filter>> GetFilterSource()
        {
            string json = await AdjustmentControl.ReadFromLocalFolder("Filter.json");

            if (json == null)
            {
                json = await AdjustmentControl.ReadFromApplicationPackage("ms-appx:///Json/Filter.json");
                AdjustmentControl.WriteToLocalFolder(json, "ms-appx:///Json/Filter.json");
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
