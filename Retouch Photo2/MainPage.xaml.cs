using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainPage" />. 
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //@Static
        static bool _isLoaded;

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        SettingViewModel SettingViewModel { get => App.SettingViewModel; set => App.SettingViewModel = value; }
        IList<Photo> Photos => this.ViewModel.Photos;
        
        //@VisualState
        bool _vsIsInitialVisibility = false;
        MainPageState _vsState = MainPageState.Main;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsInitialVisibility) return this.Initial;
                
                switch (this._vsState)
                {
                    case MainPageState.None: return this.Normal;
                    case MainPageState.Main: return this.Main;
                    case MainPageState.Add: return this.Add;
                    case MainPageState.Pictures: return this.Pictures;
                    case MainPageState.Save: return this.Save;
                    case MainPageState.Share: return this.Share;
                    case MainPageState.Delete: return this.Delete;
                    case MainPageState.Duplicate: return this.Duplicate;
                    case MainPageState.Move: return this.Move;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        
        //@Construct
        public MainPage()
        {
            this.InitializeComponent();

            this.ConstructMainPage();
            this.ConstructRightFlyout();            
            this.ConstructAddDialog();
            this.Loaded += async (s, e) =>
            {
                await this.ConstructSettingViewModel();

                if (MainPage._isLoaded == false)
                {
                    MainPage._isLoaded = true;
                    await this.RefreshWrapGrid();
                }

                this.VisualState = this.VisualState;//State
            };

            //Head
            this.RefreshButton.Tapped += async (s, e) => await this.RefreshWrapGrid();
            this.SettingButton.Tapped += (s, e) => this.Frame.Navigate(typeof(SettingPage));//Navigate     

            //Select
            this.SelectCheckBox.Checked += (s, e) => this.RefreshPhotosSelectMode(PhotoSelectMode.UnSelected);
            this.SelectCheckBox.Unchecked += (s, e) => this.RefreshPhotosSelectMode(PhotoSelectMode.None);
            this.SelectAllButton.Tapped += (s, e) => this.RefreshPhotosSelectMode(this.Photos.Any(p => p.Control.SelectMode == PhotoSelectMode.UnSelected) ? PhotoSelectMode.Selected : PhotoSelectMode.UnSelected);

            //Photo
            Photo.ItemClick += (photo) =>
            {
                switch (photo.Control.SelectMode)
                {
                    case PhotoSelectMode.None:
                        this.NewProjectFromPhoto(photo);
                        break;
                    case PhotoSelectMode.UnSelected:
                        photo.Control.SelectMode = PhotoSelectMode.Selected;
                        this.RefreshSelectCountRun();
                        break;
                    case PhotoSelectMode.Selected:
                        photo.Control.SelectMode = PhotoSelectMode.UnSelected;
                        this.RefreshSelectCountRun();
                        break;
                }
            };
            Photo.RightTapped += (photo) =>
            {
                if (photo.Control.SelectMode == PhotoSelectMode.None)
                {
                    photo.Control.SelectMode = PhotoSelectMode.Selected;

                    this._tempRightIndex = this.Photos.IndexOf(photo);
                    this.RightFlyout.ShowAt(photo.Control);
                }
            };

        }

        //The current page becomes the active page
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (MainPage._isLoaded)
            {
                await this.RefreshWrapGrid();
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }
        
    }
}