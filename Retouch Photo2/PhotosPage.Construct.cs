using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Represents a page to select a photo.
    /// </summary>
    public sealed partial class PhotosPage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TitleTextBlock.Text = resource.GetString("/$PhotosPage/Title");
            this.BackToolTip.Content = resource.GetString("/$PhotosPage/Page_Back");
            this.AddToolTip.Content = resource.GetString("/$PhotosPage/Page_Add");

            this.AddImageLayerButton.Content = resource.GetString("/$PhotosPage/AddImage");

            this.FillImageButton.Content = resource.GetString("/$PhotosPage/FillImage");
            this.StrokeImageButton.Content = resource.GetString("/$PhotosPage/StrokeImage");

            this.SelectImageButton.Content = resource.GetString("/$PhotosPage/SelectImage");
            this.ReplaceImageButton.Content = resource.GetString("/$PhotosPage/ReplaceImage");
        }


        //GridView
        private void ConstructGridView()
        {
            this.GridView.ItemsSource = Photo.Instances;
            this.GridView.ItemClick += async (s, e) =>
            {
                if (e.ClickedItem is Photo photo)
                {
                    if (this._vsPhoto == photo)
                    {
                        this._vsPhoto = null;
                        this.RadiusAnimaPanel.Visibility = Visibility.Collapsed;
                        this.GridView.SelectionMode = ListViewSelectionMode.None;
                        await Task.Delay(100);
                        this.GridView.SelectionMode = ListViewSelectionMode.Single;
                    }
                    else
                    {
                        this._vsPhoto = photo;
                        this.TextBlock.Text = $"{photo.Name}{photo.FileType}";
                        this.RadiusAnimaPanel.Visibility = Visibility.Visible;
                    }
                }
            };
        }


        //DragAndDrop
        private void ConstructDragAndDrop()
        {
            this.AllowDrop = true;
            this.Drop += async (s, e) =>
            {
                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {
                    IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();
                    if (items == null) return;

                    foreach (IStorageItem item in items)
                    {
                        await this.CopySingleImageFileAsync(item);
                    }
                }
            };
            this.DragOver += (s, e) =>
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
                //e.DragUIOverride.Caption = App.resourceLoader.GetString("DropAcceptable_");//可以接受的图片
                e.DragUIOverride.IsCaptionVisible = e.DragUIOverride.IsContentVisible = e.DragUIOverride.IsGlyphVisible = true;
            };

        }



        private async Task PickAndCopySingleImageFileAsync()
        {
            //Photo
            StorageFile copyFile = await FileUtil.PickAndCopySingleImageFileAsync(PickerLocationId.Desktop);
            if (copyFile == null) return;
            Photo photo = await Photo.CreatePhotoFromCopyFileAsync(this.ViewModel.CanvasDevice, copyFile);
            Photo.DuplicateChecking(photo);
        }
        private async Task CopySingleImageFileAsync(IStorageItem item)
        {
            //Photo
            StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(item);
            if (copyFile == null) return;
            Photo photo = await Photo.CreatePhotoFromCopyFileAsync(this.ViewModel.CanvasDevice, copyFile);
            Photo.DuplicateChecking(photo);
        }
               

        private void ButtonClick(PhotosPageMode mode)
        {
            //Photo
            Photo photo = this._vsPhoto;

            switch (mode)
            {
                case PhotosPageMode.None:
                    return;
                case PhotosPageMode.AddImager:
                    Retouch_Photo2.PhotosPage.AddCallBack?.Invoke(photo);//Delegate
                    break;

                case PhotosPageMode.FillImage:
                    Retouch_Photo2.PhotosPage.FillImageCallBack?.Invoke(photo);//Delegate
                    break;
                case PhotosPageMode.StrokeImage:
                    Retouch_Photo2.PhotosPage.StrokeImageCallBack?.Invoke(photo);//Delegate
                    break;

                case PhotosPageMode.SelectImage:
                    Retouch_Photo2.PhotosPage.SelectCallBack?.Invoke(photo);//Delegate
                    break;
                case PhotosPageMode.ReplaceImage:
                    Retouch_Photo2.PhotosPage.ReplaceCallBack?.Invoke(photo);//Delegate
                    break;
                default:
                    return;
            }

            this.Frame.GoBack();
        }

    }
}