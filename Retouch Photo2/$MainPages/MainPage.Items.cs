using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class MainPage : Page
    {

        /// <summary> Gets all items. </summary>
        public ObservableCollection<IProjectViewItem> Items { get; private set; } = new ObservableCollection<IProjectViewItem>();
        /// <summary> Gets all selected items. </summary>
        public IEnumerable<IProjectViewItem> SelectedItems => from i in this.Items where i.IsSelected select i;

        private void ItemClick(IProjectViewItem item)
        {

            switch (this.MainLayout.State)
            {
                case MainPageState.Main:
                case MainPageState.Pictures:
                    this.OpenFromProjectViewItem(item);
                    break;
                case MainPageState.Rename:
                    this.ShowRenameDialog(item);
                    break;
                case MainPageState.Delete:
                case MainPageState.Duplicate:
                    item.IsSelected = !item.IsSelected;
                    this.RefreshSelectCount();
                    break;
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "MainPage" />'s selected count. </summary>
        public int SelectedCount
        {
            get => (int)base.GetValue(SelectedCountProperty);
            set => base.SetValue(SelectedCountProperty, value);
        }
        /// <summary> Identifies the <see cref = "MainPage.SelectedCount" /> dependency property. </summary>
        public static readonly DependencyProperty SelectedCountProperty = DependencyProperty.Register(nameof(SelectedCount), typeof(int), typeof(MainPage), new PropertyMetadata(0));


        /// <summary> Gets or sets <see cref = "MainPage" />'s selected is enable. </summary>
        public bool SelectedIsEnabled
        {
            get => (bool)base.GetValue(SelectedIsEnabledProperty);
            set => base.SetValue(SelectedIsEnabledProperty, value);
        }
        /// <summary> Identifies the <see cref = "MainPage.SelectedCount" /> dependency property. </summary>
        public static readonly DependencyProperty SelectedIsEnabledProperty = DependencyProperty.Register(nameof(SelectedIsEnabled), typeof(bool), typeof(MainPage), new PropertyMetadata(false));


        /// <summary>
        /// Refresh the selected count.
        /// </summary>
        public void RefreshSelectCount()
        {
            int count = this.Items.Count(p => p.IsSelected);
            this.SelectedCount = count;

            bool isEnable = (count != 0);
            this.SelectedIsEnabled = isEnable;
        }


        #endregion


        /// <summary>
        /// Refresh all items select-mode.
        /// </summary>
        public void SelectAllAndDeselectIcon()
        {
            bool isAnyUnSelected = this.Items.Any(p => p.IsSelected == false);

            foreach (IProjectViewItem item in this.Items)
            {
                item.IsMultiple = true;
                item.IsSelected = isAnyUnSelected;
            }

            this.RefreshSelectCount();
        }

        /// <summary>
        /// Get a name that doesn't have a rename.
        /// If there are, add the number.
        /// [Untitled] --> [Untitled1]   
        /// </summary>
        /// <param name="name"> The previous name. </param>
        /// <returns> The new name. </returns>
        public string UntitledRenameByRecursive(string name)
        {
            // Is there a re-named item?
            if (this.Items.All(i => i.Name != name))
                return name;

            int num = 0;
            string newName;

            do
            {
                num++;
                newName = $"{name}{num}";
            }
            // Is there a re-named item?
            while (this.Items.Any(i => i.Name == newName));

            return newName;
        }

    }
}