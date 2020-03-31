using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Retouch_Photo2.Layers;

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
        IList<ProjectViewItem> ProjectViewItems => this.ViewModel.ProjectControls;
        
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
                    case MainPageState.Dialog: return this.Dialog;
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
            this.ConstructContextFlyout();            
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
            this.SelectCheckBox.Checked += (s, e) => this.RefreshPhotosSelectMode(SelectMode.UnSelected);
            this.SelectCheckBox.Unchecked += (s, e) => this.RefreshPhotosSelectMode(SelectMode.None);
            this.SelectAllButton.Tapped += (s, e) => this.RefreshPhotosSelectMode(this.ProjectViewItems.Any(p => p.SelectMode == SelectMode.UnSelected) ? SelectMode.Selected : SelectMode.UnSelected);

            //Photo
            ProjectViewItem.ItemClick += (item) =>
            {
                switch (item.SelectMode)
                {
                    case SelectMode.None:
                        this.OpenFromProjectViewItem(item);
                        break;
                    case SelectMode.UnSelected:
                        item.SelectMode = SelectMode.Selected;
                        this.RefreshSelectCountRun();
                        break;
                    case SelectMode.Selected:
                        item.SelectMode = SelectMode.UnSelected;
                        this.RefreshSelectCountRun();
                        break;
                }
            };
            ProjectViewItem.RightTapped += (item) =>
            {
                if (item.SelectMode == SelectMode.None)
                {
                    item.SelectMode = SelectMode.Selected;
                                        
                    // Notify TextBlock of the name of the ProjectViewItem.
                    this.ContextText = item.Tittle;
                    this.ContextFlyout.ShowAt(item);//Context
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