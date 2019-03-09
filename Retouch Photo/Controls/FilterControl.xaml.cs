using Retouch_Photo.Adjustments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Controls
{
    public sealed partial class FilterControl : UserControl
    {
        
        //@delegate
        public event AdjustmentsHandler AdjustmentsClick;

        public FilterControl()
        {
            this.InitializeComponent();

            // List<Filter>
            this.Loaded += async (s, e) =>
            {
                string json =
                    await this.ReadFromLocalFolder("Filter.json")
                    ??
                    await this.ReadFromApplicationPackage("ms-appx:///Json/Filter.json");

                IEnumerable<Filter> source = Filter.GetFiltersFromJson(json);

                this.ListView.ItemsSource = source.ToList();
            };

            // Filter
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Filter adjustmentFilter)
                {
                    // Filter -- > List<Item> -- > List<Adjustment>
                    IEnumerable<Adjustment> adjustments =
                       from adjustmentItem
                       in adjustmentFilter.AdjustmentItems
                       select adjustmentItem.GetAdjustment();

                    this.AdjustmentsClick?.Invoke(adjustments);//delegate
                }
            };

        }



        /// <summary> Read json file from Application Package. </summary> 
        public async Task<string> ReadFromApplicationPackage(string fileName)
        {
            Uri uri = new Uri(fileName);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            return await FileIO.ReadTextAsync(file);
        }

        /// <summary> Read json file from Local Folder. </summary> 
        public async Task<string> ReadFromLocalFolder(string fileName)
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
        public async void WriteToLocalFolder(string json, string fileName, StorageFolder folder)
        {
            try
            {
                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, json);
            }
            catch (Exception)
            {
            }
        }

    }
}