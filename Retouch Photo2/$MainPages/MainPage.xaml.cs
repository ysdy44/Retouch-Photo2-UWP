// Core:              ★★★★★
// Referenced:   ★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Globalization;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary>
    /// Represents a page used to manipulate some items in local folder.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //@ViewModel
        IList<IProjectViewItem> Items => App.Projects;
        IEnumerable<IProjectViewItem> SelectedItems => from i in this.Items where i.IsSelected select i;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Construct
        /// <summary>
        /// Initializes a MainPage. 
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.ConstructFlowDirection();
            this.ConstructStrings();
            this.Loaded += (s, e) => this.LoadAllProjectViewItems();
            this.ConstructInitialControl();
            this.ConstructDragAndDrop();


            //MainLayout
            this.MainLayout.GridView.ItemsSource = this.Items;
            this.MainLayout.GridView.IsItemClickEnabled = true;
            this.MainLayout.GridView.ItemClick += (s, e) =>
             {
                 if (e.ClickedItem is IProjectViewItem item)
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
             };
            this.MainLayout.SelectCheckBox.Unchecked += (s, e) =>
            {
                foreach (IProjectViewItem item in this.Items)
                {
                    item.IsMultiple = false;
                    item.IsSelected = false;
                }
            };
            this.MainLayout.SelectCheckBox.Checked += (s, e) =>
            {
                foreach (IProjectViewItem item in this.Items)
                {
                    item.IsMultiple = true;
                    item.IsSelected = false;
                }

                this.RefreshSelectCount();
            };


            //Select
            this.AllButton.Tapped += (s, e) =>
            {
                bool isAnyUnSelected = this.Items.Any(p => p.IsSelected == false);

                //Refresh all items select-mode.
                foreach (IProjectViewItem item in this.Items)
                {
                    item.IsMultiple = true;
                    item.IsSelected = isAnyUnSelected;
                }

                this.RefreshSelectCount();
            };
            //Head
            this.Head.LeftButtonTapped += async (s, e) => await Launcher.LaunchUriAsync(new Uri(this.DocumentationLink));
            this.Head.RightButtonTapped += (s, e) => this.Frame.Navigate(typeof(SettingPage));//Navigate     


            //Foot
            this.ConstructAddDialog();
            this.NewButton.Tapped += (s, e) => this.ShowAddDialog();

            this.ConstructPicturesControl();
            this.PicturesButton.Tapped += (s, e) => this.MainLayout.State = MainPageState.Pictures;

            this.ConstructRenameDialog();
            this.RenameCloseButton.Tapped += (s, e) => this.MainLayout.State = MainPageState.Main;
            this.RenameButton.Tapped += async (s, e) => this.MainLayout.State = MainPageState.Rename;

            this.ConstructDeleteControl();
            this.DeleteButton.Tapped += (s, e) => this.MainLayout.State = MainPageState.Delete;

            this.ConstructDuplicateControl();
            this.DuplicateButton.Tapped += (s, e) => this.MainLayout.State = MainPageState.Duplicate;
        }
    }


    public sealed partial class MainPage : Page
    {

        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Setting
            if (string.IsNullOrEmpty(ApplicationLanguages.PrimaryLanguageOverride) == false)
            {
                if (ApplicationLanguages.PrimaryLanguageOverride != this.Language)
                {
                    this.ConstructFlowDirection();
                    this.ConstructStrings();
                }
            }

            //Extension
            this.ApplicationView.IsAccent = false;
        }
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e) { }

    }
}