using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Retouch_Photo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.Foundation;
using Windows.System;
using System.Linq;
using System.Xml.Linq;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Microsoft.Graphics.Canvas;

namespace Retouch_Photo.Pages
{
    enum MainMode
    {
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
        ObservableCollection<Photo> PhotoFileList = new ObservableCollection<Photo>() { };
        MainMode mode;
        MainMode Mode
        {
            get => mode;
            set
            {
                this.AppbarControl.Visibility = (value == MainMode.None) ? Visibility.Visible : Visibility.Collapsed;

                this.AppbarPicturesControl.Visibility = (value == MainMode.Pictures) ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarSaveControl.Visibility = (value == MainMode.Save) ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarShareControl.Visibility = (value == MainMode.Share) ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarDeleteControl.Visibility = (value == MainMode.Delete) ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarDuplicateControl.Visibility = (value == MainMode.Duplicate) ? Visibility.Visible : Visibility.Collapsed;

                mode = value;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
        }


        #region Global


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadingControl.Visibility = Visibility.Collapsed;
             this.Refresh();
        }
        private void RefreshButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Refresh();
        private async void Refresh()
        {
            this.PhotoFileList.Clear();

            foreach (StorageFile file in await Photo.CreatePhotoFiles(ApplicationData.Current.LocalFolder))
            {
                Photo photo = Photo.CreatePhoto(file, ApplicationData.Current.LocalFolder.Path);

                if (photo != null) this.PhotoFileList.Add(photo);
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.Mode != MainMode.None) return;

            if (e.ClickedItem is Photo photo)
            {
                this.LoadingControl.Visibility = Visibility.Visible;

                XDocument document = XDocument.Load(photo.Path);
                this.Frame.Navigate(typeof(DrawPage), document);//Navigate     

                this.LoadingControl.Visibility = Visibility.Collapsed;
            }
        }


        #endregion

        #region Title


        private void PopupButton_Tapped(object sender, TappedRoutedEventArgs e) { }
        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            double offset = ((ScrollViewer)sender).VerticalOffset;
            if (offset > 120) offset = 120;

            this.Title.Height = 120 - offset;

            this.Header1.Height = offset;
            this.Header2.Height = offset;
        }


        #endregion



        #region Appbar
        

        //Appbar
        private void AppbarControl_CancelButtonTapped(object sender, TappedRoutedEventArgs e) => this.Mode = MainMode.None;
        private async void AppbarControl_AddButtonTapped(object sender, TappedRoutedEventArgs e) => await this.AddDialog.ShowAsync(ContentDialogPlacement.InPlace);//ContentDialogPlacement.InPlace
        private void AppbarControl_PicturesButtonTapped(object sender, TappedRoutedEventArgs e) => this.Mode = MainMode.Pictures;
        private void AppbarControl_SaveButtonTapped(object sender, TappedRoutedEventArgs e) => this.Mode = MainMode.Save;
        private void AppbarControl_ShareButtonTapped(object sender, TappedRoutedEventArgs e) => this.Mode = MainMode.Share;
        private void AppbarControl_DeleteButtonTapped(object sender, TappedRoutedEventArgs e) => this.Mode = MainMode.Delete;
        private void AppbarControl_DuplicateButtonTapped(object sender, TappedRoutedEventArgs e) => this.Mode = MainMode.Duplicate;
        private async void AppbarControl_FolderButtonTapped(object sender, TappedRoutedEventArgs e) => await this.FolderDialog.ShowAsync(ContentDialogPlacement.InPlace);//ContentDialogPlacement.InPlace

        private async void AddDialog_AddSize(BitmapSize pixels)
        {
            this.LoadingControl.Visibility = Visibility.Visible;

            await Task.Delay(1000);

            this.LoadingControl.Visibility = Visibility.Collapsed;

            this.Frame.Navigate(typeof(DrawPage), pixels);//Navigate            
        }
        private async void AppbarPicturesControl_PicturesPicker(PickerLocationId location)
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = location
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".bmp");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null) return;

            this.Frame.Navigate(typeof(DrawPage), file);//Navigate        
        }


        private void AppbarSaveControl_OKButtonTapped(object sender, TappedRoutedEventArgs e) { }
        private void AppbarShareControl_OKButtonTapped(object sender, TappedRoutedEventArgs e) { }
        private void AppbarDeleteControl_OKButtonTapped(object sender, TappedRoutedEventArgs e) { }
        private void AppbarDuplicateControl_OKButtonTapped(object sender, TappedRoutedEventArgs e) { }

        private void FolderDialog_FolderName(string name)
        {
            //  this.Frame.Navigate(typeof(DrawPage), name);//Navigate
        }


        #endregion


        #region More


        private void SearchButton_Tapped(object sender, TappedRoutedEventArgs e) { }
        private async void LocalButton_Tapped(object sender, TappedRoutedEventArgs e) => await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
        private async void OpensourceButton_Tapped(object sender, TappedRoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/ysdy44/Retouch-Photo-UWP.git"));


        #endregion

      
    }
}
