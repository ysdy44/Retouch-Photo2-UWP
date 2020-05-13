using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.ViewModels;
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
    /// Retouch_Photo2's the only <see cref = "MainPage" />. 
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //@Static
        /// <summary> Is this Loaded? </summary>
        static bool _lockIsLoaded = false;

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
        /// <param name="data"> The data. </param>
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
        private async Task LoadAllProjectViewItems()
        {
            IEnumerable<StorageFolder> zipFolders = await FileUtil.FIndZipFolders();
             
            //Refresh, when the count is not equal.
            if (zipFolders.Count() != this.ProjectViewItems.Count)
            {
                this.ProjectViewItems.Clear(); //Notify

                foreach (StorageFolder folder in zipFolders)
                {
                    // [StorageFolder] --> [projectViewItem]
                    ProjectViewItem item = FileUtil.ConstructProjectViewItem(folder);
                    if (item != null) this.ProjectViewItems.Add(item); //Notify
                }
            }

            if (this.ProjectViewItems.Count == 0)
                this.MainLayout.MainPageState = MainPageState.Initial;
            else
                this.MainLayout.MainPageState = MainPageState.Main;
        }

        /// <summary>
        /// Refresh all photos select-mode.
        /// </summary>
        private void SelectAllProjectViewItems(SelectMode selectMode)
        {
            foreach (ProjectViewItem item in this.ProjectViewItems)
            {
                item.SelectMode = selectMode;
            }
        }

        /// <summary>
        /// Refresh the selected count.
        /// </summary>
        private void RefreshSelectCount()
        {
            int count = this.ProjectViewItems.Count(p => p.SelectMode == SelectMode.Selected);

            this.SelectCountRun.Text = $"{count}";

            bool isEnable = (count != 0);
            this.DeletePrimaryButton.IsEnabled = isEnable;
            this.DuplicatePrimaryButton.IsEnabled = isEnable;
        }


        /// <summary>
        /// Rename the ProjectViewItem.
        /// </summary>
        /// <param name="item"> The ProjectViewItem. </param>
        private async Task RenameProjectViewItem(ProjectViewItem item)
        {
            //Same name.
            string oldName = item.Name;
            string newName = this.RenameTextBox.Text;
            if (oldName == newName)
            {
                this.RenameTipTextBlock.Visibility = Visibility.Visible;
                return;
            }

            //Name is already occupied.
            bool hasRenamed = this.ProjectViewItems.Any(p => p.Name == newName);
            if (hasRenamed)
            {
                this.RenameTipTextBlock.Visibility = Visibility.Visible;
                return;
            }

            //Rename
            await FileUtil.RenameZipFolder(oldName, newName, item);

            this.HideRenameDialog();
        }

        /// <summary>
        /// Delete all selected ProjectViewItem(s).
        /// </summary>
        private async Task DeleteProjectViewItems(IList<ProjectViewItem> items)
        {
            foreach (ProjectViewItem item in items)
            {
                //Delete
                string name = item.Name;
                await FileUtil.DeleteZipFolder(name);

                item.Visibility = Visibility.Collapsed;
                this.ProjectViewItems.Remove(item);//Notify

                await Task.Delay(300);
            }
        }

        /// <summary>
        /// Duplicate all selected ProjectViewItem(s).
        /// </summary>     
        private async Task DuplicateProjectViewItems(IList<ProjectViewItem> items)
        {
            foreach (ProjectViewItem item in items)
            {
                string oldName = item.Name;
                string newName = this.UntitledRenameByRecursive(oldName);
                StorageFolder storageFolder = await FileUtil.DuplicateZipFolder(oldName, newName);

                ProjectViewItem newItem = FileUtil.ConstructProjectViewItem(newName, storageFolder);
                this.ProjectViewItems.Add(newItem);//Notify
            }
        }

    }
}