using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary>
    /// Represents a page used to manipulate some items in local folder.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //@Static
        /// <summary> Is this Loaded? </summary>
        private static bool _lockIsLoaded = false;

        #region Lock


        /// <summary>
        /// Loaded.
        /// </summary>
        private async Task _lockLoaded()
        {
            if (MainPage._lockIsLoaded == false)
            {
                MainPage._lockIsLoaded = true;
                await this.LoadAllProjectViewItems();
            }
        }

        /// <summary>
        /// OnNavigatedTo.
        /// </summary>
        private async Task _lockOnNavigatedTo()
        {
            if (MainPage._lockIsLoaded)
            {
                await this.LoadAllProjectViewItems();
            }
        }


        #endregion


        /// <summary>
        /// Load all ProjectViewItems in GridView children.
        /// </summary>
        public async Task LoadAllProjectViewItems()
        {
            IEnumerable<StorageFolder> zipFolders = await FileUtil.FIndZipFolders();
             
            //Refresh, when the count is not equal.
            if (zipFolders.Count() != this.MainLayout.Count)
            {
                this.MainLayout.Items.Clear(); //Notify

                foreach (StorageFolder folder in zipFolders)
                {
                    // [StorageFolder] --> [projectViewItem]
                    IProjectViewItem item = FileUtil.ConstructProjectViewItem(folder);
                    if (item != null) this.MainLayout.Items.Add(item); //Notify
                }
            }

            if (this.MainLayout.Count == 0)
                this.MainLayout.State = MainPageState.Initial;
            else
                this.MainLayout.State = MainPageState.Main;
        }


        /// <summary>
        /// Rename the ProjectViewItem.
        /// </summary>
        /// <param name="oldName"> The old name. </param>
        /// <param name="newName"> The new name. </param>
        public async Task RenameProjectViewItem(string oldName, string newName)
        {
            //Same name. 
            if (oldName == newName)
            {
                this.RenameTipTextBlock.Visibility = Visibility.Visible;
                return;
            }

            //Name is already occupied.
            bool hasRenamed = this.MainLayout.Items.Any(p => p.Name == newName);
            if (hasRenamed)
            {
                this.RenameTipTextBlock.Visibility = Visibility.Visible;
                return;
            }

            //Rename
            IProjectViewItem item = this.MainLayout.Items.First(p=>p.Name==oldName);
            await FileUtil.RenameZipFolder(oldName, newName, item);

            this.HideRenameDialog();
            this.MainLayout.State = MainPageState.Main;
        }


        /// <summary>
        /// Delete all selected ProjectViewItem(s).
        /// </summary>
        /// <param name="items"> The items. </param>
        public async Task DeleteProjectViewItems(IEnumerable<IProjectViewItem> items)
        {
            foreach (IProjectViewItem item in items.ToList())
            {
                //Delete
                string name = item.Name;
                await FileUtil.DeleteZipFolder(name);

                item.Visibility = Visibility.Collapsed;
                this.MainLayout.Items.Remove(item);//Notify

                await Task.Delay(300);
            }
        }


        /// <summary>
        /// Duplicate all selected ProjectViewItem(s).
        /// </summary>     
        /// <param name="items"> The items. </param>
        public async Task DuplicateProjectViewItems(IEnumerable<IProjectViewItem> items)
        {
            foreach (IProjectViewItem item in items.ToList())
            {
                string oldName = item.Name;
                string newName = this.MainLayout.UntitledRenameByRecursive(oldName);
                StorageFolder storageFolder = await FileUtil.DuplicateZipFolder(oldName, newName);

                IProjectViewItem newItem = FileUtil.ConstructProjectViewItem(newName, storageFolder);
                this.MainLayout.Items.Add(newItem);//Notify
            }
        }

    }
}