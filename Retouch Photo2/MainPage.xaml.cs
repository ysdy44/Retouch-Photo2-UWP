using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainPage" />. 
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        
        ObservableCollection<ProjectViewItem> ProjectViewItems = new ObservableCollection<ProjectViewItem>();


        //@Construct
        public MainPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructInitialControl();
            this.ConstructSelectHead();

            this.MainLayout.ItemsSource = this.ProjectViewItems;

            this.Loaded += async (s, e) =>
            {
                await this.ConstructSetting();
                await this._lockLoaded();
                
                //FileUtil
                await FileUtil.DeleteInTemporaryFolder();
            };


            #region Foot


            this.ConstructAddDialog();
            this.AddButton.Tapped += (s, e) => this.ShowAddDialog();

            this.ConstructPicturesControl();
            this.PicturesButton.Tapped += (s, e) => this.MainLayout.MainPageState = MainPageState.Pictures;

            this.RenameCloseButton.Tapped += (s, e) => this.MainLayout.MainPageState = MainPageState.Main;
            this.RenameButton.Tapped += (s, e) => this.MainLayout.MainPageState = MainPageState.Rename;
            this.RenameDialog.CloseButton.Click += (sender, args) => this.HideRenameDialog();

            this.ConstructDeleteControl();
            this.DeleteButton.Tapped += (s, e) => this.MainLayout.MainPageState = MainPageState.Delete;

            this.ConstructDuplicateControl();
            this.DuplicateButton.Tapped += (s, e) => this.MainLayout.MainPageState = MainPageState.Duplicate;


            #endregion


            //ProjectViewItem
            ProjectViewItem.ItemClick += (item) =>
            {
                switch (this.MainLayout.MainPageState)
                {
                    case MainPageState.Main:
                        this.OpenFromProjectViewItem(item);
                        break;

                    case MainPageState.Rename:
                        this.ShowRenameDialog(item);
                        break;

                    case MainPageState.Delete:
                    case MainPageState.Duplicate:
                        item.SwitchState();
                        this.RefreshSelectCount();
                        break;

                    default:
                        break;
                }
            };

        }

        //The current page becomes the active page
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await this._lockOnNavigatedTo();

            /// Occurs occuse after <see  cref="DrawPage.Frame.GoBack()"/>;
            if (this.ViewModel.IsUpdateThumbnailByName)
            {
                ProjectViewItem item = this.ProjectViewItems.FirstOrDefault(i => i.Name == this.ViewModel.Name);
                if (item != null)
                {
                    item.RefreshImageSource();
                }
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

    }
}