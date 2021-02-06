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
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        

        //@Construct
        /// <summary>
        /// Initializes a MainPage. 
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructInitialControl();
            this.ConstructDragAndDrop();

            //Select
            this.AllButton.Click += (s, e) => this.MainLayout.SelectAllAndDeselectIcon();
            //Head
            this.DocumentationButton.Click += async (s, e) => await Launcher.LaunchUriAsync(new Uri(this.DocumentationLink));
            this.SettingButton.Click += (s, e) => this.Frame.Navigate(typeof(SettingPage));//Navigate     
            

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


            //ProjectViewItem
            ProjectViewItem.ItemClick += (item) =>
            {
                switch (this.MainLayout.State)
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
                        this.MainLayout.RefreshSelectCount();
                        break;

                    default:
                        break;
                }
            };

        }

        /// <summary> The current page becomes the active page. </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //Extension
            this.AVTBBE.Invalidate();

            await this._lockOnNavigatedTo();

            // Occurs occuse after <see  cref="DrawPage.Frame.GoBack()"/>;
            if (this.ViewModel.IsUpdateThumbnailByName)
            {
                this.ViewModel.IsUpdateThumbnailByName = false;

                IProjectViewItem item = this.MainLayout.Items.FirstOrDefault(i => i.Name == this.ViewModel.Name);
                if (item != null)
                {
                    item.RefreshImageSource();
                }
            }
        }
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e) { }

    }
}