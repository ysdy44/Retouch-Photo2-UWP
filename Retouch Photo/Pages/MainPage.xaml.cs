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

namespace Retouch_Photo.Pages
{
    public sealed partial class MainPage : Page
    {
        ObservableCollection<Photo> PhotoFileList = new ObservableCollection<Photo>() { };

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


        private int Index
        {
            set
            {
                this.AppbarControl.Visibility = value == 0 ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarPicturesControl.Visibility = value == 2 ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarSaveControl.Visibility = value == 3 ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarShareControl.Visibility = value == 4 ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarDeleteControl.Visibility = value == 5 ? Visibility.Visible : Visibility.Collapsed;
                this.AppbarDuplicateControl.Visibility = value == 6 ? Visibility.Visible : Visibility.Collapsed;

            }
        }

        //Appbar
        private void AppbarControl_CancelButtonTapped(object sender, TappedRoutedEventArgs e) => this.Index = 0;
        private async void AppbarControl_AddButtonTapped(object sender, TappedRoutedEventArgs e) => await this.AddDialog.ShowAsync(ContentDialogPlacement.InPlace);//ContentDialogPlacement.InPlace
        private void AppbarControl_PicturesButtonTapped(object sender, TappedRoutedEventArgs e) => this.Index = 2;
        private void AppbarControl_SaveButtonTapped(object sender, TappedRoutedEventArgs e) => this.Index = 3;
        private void AppbarControl_ShareButtonTapped(object sender, TappedRoutedEventArgs e) => this.Index = 4;
        private void AppbarControl_DeleteButtonTapped(object sender, TappedRoutedEventArgs e) => this.Index = 5;
        private void AppbarControl_DuplicateButtonTapped(object sender, TappedRoutedEventArgs e) => this.Index = 6;
        private async void AppbarControl_FolderButtonTapped(object sender, TappedRoutedEventArgs e) => await this.FolderDialog.ShowAsync(ContentDialogPlacement.InPlace);//ContentDialogPlacement.InPlace

        private async void AddDialog_AddSize(BitmapSize pixels)
        {
            this.LoadingControl.Visibility = Visibility.Visible;

            await Task.Delay(1000);

            this.LoadingControl.Visibility = Visibility.Collapsed;
            this.Frame.Navigate(typeof(DrawPage), pixels);//Navigate            
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
