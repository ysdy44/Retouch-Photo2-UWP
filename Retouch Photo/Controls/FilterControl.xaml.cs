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
        public static readonly string FileName = "Filter.json";

        //@delegate
        public event AdjustmentsHandler AdjustmentsClick;

        public FilterControl()
        {
            this.InitializeComponent();

            // List<Filter>
            this.Loaded +=async (s, e) => this.ListView.ItemsSource =await this.GetItemsSource();

            // Filter
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is AdjustmentFilter adjustmentFilter)
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


        /// <summary> Get ItemsSource. </summary> 
        private async Task<List<AdjustmentFilter>> GetItemsSource()
        {
            //Read
            string json = await this.Read(FilterControl.FileName, ApplicationData.Current.LocalFolder);
            if (json != null) return AdjustmentFilter.GetFilters(json).ToList();
          
            //Write
            string preinstall = "[{'Name':'AAA','AdjustmentItems':[{'Contrast':1.0,'Type':6},{'Contrast':1.0,'Type':6}]},{'Name':'AAA','AdjustmentItems':[{'Contrast':1.0,'Type':6},{'Contrast':1.0,'Type':6}]}]";
            this.Write(preinstall, FilterControl.FileName, ApplicationData.Current.LocalFolder);

            return AdjustmentFilter.GetFilters(preinstall).ToList();
        }
       
        /// <summary> Read File. </summary> 
        public async Task<string> Read(string fileName, StorageFolder folder)
        {
            try
            {
                StorageFile file = await folder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(file);
            }
            catch (Exception){return null;}
        }
       
        /// <summary> Write File. </summary> 
        public async void Write(string json, string fileName, StorageFolder folder)
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