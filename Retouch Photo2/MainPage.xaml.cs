using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Xml.Linq;
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

        //@Construct
        public MainPage()
        {
            this.InitializeComponent();            
            this.Loaded += async (s, e) =>
            {
                if (MainPage._isLoaded == false)
                {
                    MainPage._isLoaded = true;
                    await this.ConstructSettingViewModel();
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
                            this.NewProjectFromPhoto(s, photo);
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
            this.MainControl.PicturesButton.Tapped += (s, e) => this.State = MainPageState.Pictures;
            this.MainControl.SaveButton.Tapped += (s, e) => this.State = MainPageState.Save;
            this.MainControl.ShareButton.Tapped += (s, e) => this.State = MainPageState.Share;
            this.MainControl.DeleteButton.Tapped += (s, e) => this.State = MainPageState.Delete;
            this.MainControl.DuplicateButton.Tapped += (s, e) => this.State = MainPageState.Duplicate;
            this.MainControl.FolderButton.Tapped += (s, e) => this.FolderDialogShow();
            this.MainControl.MoveButton.Tapped += (s, e) => this.State = MainPageState.Move;

            //Second
            this.MainControl.SecondAddButton.Tapped += (s, e) => this.AddDialogShow();
            this.MainControl.SecondPicturesButton.Tapped += (s, e) => this.State = MainPageState.Pictures;
            this.MainControl.SecondSaveButton.Tapped += (s, e) => this.State = MainPageState.Save;
            this.MainControl.SecondShareButton.Tapped += (s, e) => this.State = MainPageState.Share;
            this.MainControl.SecondDeleteButton.Tapped += (s, e) => this.State = MainPageState.Delete;
            this.MainControl.SecondDuplicateButton.Tapped += (s, e) => this.State = MainPageState.Duplicate;
            this.MainControl.SecondFolderButton.Tapped += (s, e) => this.FolderDialogShow();
            this.MainControl.SecondMoveButton.Tapped += (s, e) => this.State = MainPageState.Move;
            
            //Pictures
            this.PicturesControl.PhotoButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.PicturesLibrary);
            this.PicturesControl.DestopButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.Desktop);
            this.PicturesControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;

            //Save
            this.SaveControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;

            //Share
            this.ShareControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;

            //Delete
            this.DeleteControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;

            //Duplicate
            this.DuplicateControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;
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