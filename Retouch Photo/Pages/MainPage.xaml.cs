using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo.Element;

namespace Retouch_Photo.Pages
{
    /// <summary> 
    /// State of <see cref="MainPage"/>.
    /// </summary>
    public enum MainPageState
    {
        Loading,
        None,

        Add,
        Pictures,
        Save,
        Share,
        Delete,
        Duplicate,
        Folder,
    }

    public sealed partial class MainPage : Page
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        ObservableCollection<Photo> PhotoFileList = new ObservableCollection<Photo>();

        private MainPageState state;
        public MainPageState State
        {
            get => this.state;
            set
            {
                this.LoadingControl.Visibility = (value == MainPageState.Loading) ? Visibility.Visible : Visibility.Collapsed;

                this.AppbarControl.Visibility = (value == MainPageState.None) ? Visibility.Visible : Visibility.Collapsed;

                this.AppbarPicturesControl.Visibility = (value == MainPageState.Pictures) ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarSaveControl.Visibility = (value == MainPageState.Save) ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarShareControl.Visibility = (value == MainPageState.Share) ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarDeleteControl.Visibility = (value == MainPageState.Delete) ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarDuplicateControl.Visibility = (value == MainPageState.Duplicate) ? Visibility.Visible : Visibility.Collapsed;

                this.state= value;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            this.State = MainPageState.None;
            this.Loaded += (s, e) =>
            {
                this.Refresh();

                return;
                this.Frame.Navigate(typeof(DrawPage), Project.CreateFromSize(App.ViewModel.CanvasDevice, new Windows.Graphics.Imaging.BitmapSize
                {
                      Width = 1024,
                      Height = 1024
                  }));//Navigate  

            };
            this.PopupButton.Tapped += (s, e) => { };
            this.ThemeButton.Tapped += (s, e) => this.ThemeControl._Theme = ThemeControl.Theme = (ThemeControl.Theme == ElementTheme.Dark) ? ElementTheme.Light : ElementTheme.Dark;

            this.GridView.ItemClick += (s, e) =>
            {
                if (this.State != MainPageState.None) return;

                if (e.ClickedItem is Photo photo)
                {
                    this.State = MainPageState.Loading;

                    XDocument document = XDocument.Load(photo.Path);

                    Project project = Project.CreateFromXDocument(this.ViewModel.CanvasDevice, document);//Project
                    this.Frame.Navigate(typeof(DrawPage), project);//Navigate     

                    this.State = MainPageState.None;
                }
            };

            //Appbar
            // this.AppbarControl.CancelButtonTapped+=(s,e)=>this.Mode = MainMode.None;
            this.AppbarControl.AddButtonTapped += async (s, e) =>await this.AddDialog.ShowAsync(ContentDialogPlacement.InPlace);
            this.AppbarControl.PicturesButtonTapped += (s, e) => this.State = MainPageState.Pictures;
            this.AppbarControl.SaveButtonTapped += (s, e) => this.State = MainPageState.Save;
            this.AppbarControl.ShareButtonTapped += (s, e) => this.State = MainPageState.Share;
            this.AppbarControl.DeleteButtonTapped += (s, e) => this.State = MainPageState.Delete;
            this.AppbarControl.DuplicateButtonTapped += (s, e) => this.State = MainPageState.Duplicate;
            this.AppbarControl.FolderButtonTapped += async (s, e) => await this.FolderDialog.ShowAsync(ContentDialogPlacement.InPlace);//ContentDialogPlacement.InPlace

            this.AddDialog.AddSize += (pixels) =>
            {
                this.AddDialog.Hide();

                this.State = MainPageState.Loading;

                Project project = Project.CreateFromSize(this.ViewModel.CanvasDevice, pixels);//Project
                this.Frame.Navigate(typeof(DrawPage), project);//Navigate    

                this.State = MainPageState.None;
            };
            this.AppbarPicturesControl.PicturesPicker += async (location) =>
            {
                this.State = MainPageState.Loading;

                FileOpenPicker openPicker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = location,
                    FileTypeFilter = { ".jpg", ".jpeg", ".png", ".bmp" }
                };

                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file == null) return;

                Project project = await Project.CreateFromFileAsync(this.ViewModel.CanvasDevice, file);//Project
                this.Frame.Navigate(typeof(DrawPage), project);//Navigate       

                this.State = MainPageState.None;
            };

            this.AppbarSaveControl.OKButtonTapped += (s, e) => { };
            this.AppbarShareControl.OKButtonTapped += (s, e) => { };
            this.AppbarDeleteControl.OKButtonTapped += (s, e) => { };
            this.AppbarDuplicateControl.OKButtonTapped += (s, e) => { };
            this.FolderDialog.FolderName += (name) => { };//  this.Frame.Navigate(typeof(DrawPage), name);//Navigate
            this.SearchButton.Tapped += (s, e) => { };
            this.LocalButton.Tapped += async (s, e) => await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
            this.OpensourceButton.Tapped += async (s, e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/ysdy44/Retouch-Photo-UWP.git"));
        }

        private async void Refresh()
        {
            this.PhotoFileList.Clear();

            foreach (StorageFile file in await Photo.CreatePhotoFiles(ApplicationData.Current.LocalFolder))
            {
                Photo photo = Photo.CreatePhoto(file, ApplicationData.Current.LocalFolder.Path);

                if (photo != null) this.PhotoFileList.Add(photo);
            }
        }
    }
}
