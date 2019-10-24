using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
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
        SettingViewModel SettingViewModel { get => App.SettingViewModel; set => App.SettingViewModel = value; }

        IList<Photo> PhotoFileList = new List<Photo>();
        static bool _isLoaded;

        //Loading
        private bool IsLoading { set => this.LoadingControl.IsActive = value; }
        /// <summary> State of <see cref="MainPage"/>. </summary>
        public MainPageState State
        {
            get => this.state;
            set
            {
                if (value == MainPageState.None)
                {
                    this.InitialControl.Visibility = Visibility.Visible;

                    this.GridView.Visibility = Visibility.Collapsed;
                    this.RadiusAnimaPanel.Visibility = Visibility.Collapsed;
                    this.RadiusAnimaPanel.CenterContent = null;
                }
                else
                {
                    this.InitialControl.Visibility = Visibility.Collapsed;

                    this.GridView.Visibility = Visibility.Visible;
                    this.RadiusAnimaPanel.Visibility = Visibility.Visible;

                    switch (value)
                    {
                        case MainPageState.Main:
                            this.RadiusAnimaPanel.CenterContent = this.MainControl;
                            break;
                        //case MainPageState.Loading:
                        //break;

                        //case MainPageState.Add:
                        //break;
                        case MainPageState.Pictures:
                            this.RadiusAnimaPanel.CenterContent = this.PicturesControl;
                            break;

                        case MainPageState.Save:
                            this.RadiusAnimaPanel.CenterContent = this.SaveControl;
                            break;
                        case MainPageState.Share:
                            this.RadiusAnimaPanel.CenterContent = this.ShareControl;
                            break;

                        case MainPageState.Delete:
                            this.RadiusAnimaPanel.CenterContent = this.DeleteControl;
                            break;
                        case MainPageState.Duplicate:
                            this.RadiusAnimaPanel.CenterContent = this.DuplicateControl;
                            break;

                        //case MainPageState.Folder:
                        //break;
                        //case MainPageState.Move:
                        //break;

                        default:
                            break;
                    }
                }


                this.state = value;
            }
        }
        private MainPageState state;

        MainControl MainControl = new MainControl();
        PicturesControl PicturesControl = new PicturesControl();
        SaveControl SaveControl = new SaveControl();
        ShareControl ShareControl = new ShareControl();
        DeleteControl DeleteControl = new DeleteControl();
        DuplicateControl DuplicateControl = new DuplicateControl();

        //@Construct
        public MainPage()
        {
            this.InitializeComponent();            
            this.Loaded += async (s, e) =>
            {
                if (MainPage._isLoaded == false)
                {
                    MainPage._isLoaded = true;
                    await this.LoadSettingViewModel();
                    await this.Refresh();
                }
            };
            this.RefreshButton.Tapped += async (s, e) => await this.Refresh();
            this.SettingButton.Tapped += (s, e) => this.Frame.Navigate(typeof(SettingPage));//Navigate     


            //Photo
            if (Photo.ItemClick == null)
            {
                Photo.ItemClick += (object s, Photo photo) =>
                {
                    switch (photo.SelectMode)
                    {
                        case null:
                            if (this.State == MainPageState.Main)
                            {
                                if (s is FrameworkElement element)
                                {
                                    this.NavigatedFrom(element);
                                }
                                this.Frame.Navigate(typeof(DrawPage));//Navigate     
                            }
                            break;
                        case false:
                            photo.SelectMode = true;
                            break;
                        case true:
                            photo.SelectMode = false;
                            break;
                    }
                };
            }


            //Initial
            this.AddButton.Tapped += (s, e) => this.AddDialogShow();
            this.PhotoButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.PicturesLibrary);
            this.DestopButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.Desktop);

            //Main
            this.MainControl.AddButton.Tapped += (s, e) => this.AddDialogShow();
            this.MainControl.PicturesButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Pictures);
            this.MainControl.SaveButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Save);
            this.MainControl.ShareButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Share);
            this.MainControl.DeleteButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Delete);
            this.MainControl.DuplicateButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Duplicate);
            this.MainControl.FolderButton.Tapped += (s, e) => this.FolderDialogShow();
            this.MainControl.MoveButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Move);

            //Second
            this.MainControl.SecondAddButton.Tapped += (s, e) => this.AddDialogShow();
            this.MainControl.SecondPicturesButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Pictures);
            this.MainControl.SecondSaveButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Save);
            this.MainControl.SecondShareButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Share);
            this.MainControl.SecondDeleteButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Delete);
            this.MainControl.SecondDuplicateButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Duplicate);
            this.MainControl.SecondFolderButton.Tapped += (s, e) => this.FolderDialogShow();
            this.MainControl.SecondMoveButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Move);
            
            //Pictures
            this.PicturesControl.PhotoButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.PicturesLibrary);
            this.PicturesControl.DestopButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.Desktop);
            this.PicturesControl.CancelButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Main);

            //Save
            this.SaveControl.CancelButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Main);

            //Share
            this.ShareControl.CancelButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Main);

            //Delete
            this.DeleteControl.CancelButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Main);

            //Duplicate
            this.DuplicateControl.CancelButton.Tapped += (s, e) => this.SetMainPageState(MainPageState.Main);
        }

        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (MainPage._isLoaded)
            {
                //Theme
                ElementTheme theme = this.SettingViewModel.ElementTheme;
                this.RequestedTheme = theme;
                ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

    }
}