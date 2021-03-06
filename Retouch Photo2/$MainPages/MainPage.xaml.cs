// Core:              ★★★★★
// Referenced:   ★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary>
    /// Represents a page used to manipulate some items in local folder.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
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
            this.ConstructInitialControl();
            this.ConstructDragAndDrop();


            //ProjectViewItem
            ProjectViewItem.ItemClick = this.ItemClick;

            //MainLayout
            this.MainLayout.GridView.ItemsSource = this.Items;
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
            this.AllButton.Click += (s, e) => this.SelectAllAndDeselectIcon();
 
            //Head
            this.Head.LeftButtonClick += async (s, e) => await Launcher.LaunchUriAsync(new Uri(this.DocumentationLink));
            this.Head.RightButtonClick += (s, e) => this.Frame.Navigate(typeof(SettingPage));//Navigate     
            this.ModifyXamlStyle(this.MainLayout.GridView);

            this.Loaded += async (s, e) =>
            {
                await this.ConstructSetting();
                await this._lockLoaded();

                //FileUtil
                await FileUtil.DeleteInTemporaryFolder();
            };


            #region Foot


            this.ConstructAddDialog();
            this.NewButton.Click += (s, e) => this.ShowAddDialog();

            this.ConstructPicturesControl();
            this.PicturesButton.Click += (s, e) => this.MainLayout.State = MainPageState.Pictures;

            this.ConstructRenameDialog();
            this.RenameCloseButton.Click += (s, e) => this.MainLayout.State = MainPageState.Main;
            this.RenameButton.Click += (s, e) => this.MainLayout.State = MainPageState.Rename;

            this.ConstructDeleteControl();
            this.DeleteButton.Click += (s, e) => this.MainLayout.State = MainPageState.Delete;

            this.ConstructDuplicateControl();
            this.DuplicateButton.Click += (s, e) => this.MainLayout.State = MainPageState.Duplicate;


            #endregion
        }

        /// <summary> The current page becomes the active page. </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //Extension
            this.ApplicationView.Color = this.ApplicationView.Color;

            await this._lockOnNavigatedTo();
        }
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e) { }

        private void ModifyXamlStyle(GridView gridView)
        {
            gridView.Loaded += (s, e) =>
            {
                if (VisualTreeHelper.GetChild(gridView, 0) is FrameworkElement element)
                {
                    if (element.FindName("ScrollViewer") is ScrollViewer scrollViewer)
                    {
                        scrollViewer.ViewChanged += (s2, e2) =>
                        {
                            this.Head.Move(scrollViewer.VerticalOffset);
                        };
                    }
                }
            };
        }

    }
}