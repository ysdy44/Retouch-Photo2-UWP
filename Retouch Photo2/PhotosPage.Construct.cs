using Retouch_Photo2.Photos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Pickers;
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
            if (photo == null) return;
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


        private void ButtonClick(Photo photo, PhotosPageMode mode)
        {
            switch (mode)
            {
                case PhotosPageMode.None:
                    return;
                case PhotosPageMode.AddImage:
                    Retouch_Photo2.PhotosPage.AddImageCallBack?.Invoke(photo);//Delegate
                    break;

                case PhotosPageMode.FillImage:
                    Retouch_Photo2.PhotosPage.FillImageCallBack?.Invoke(photo);//Delegate
                    break;
                case PhotosPageMode.StrokeImage:
                    Retouch_Photo2.PhotosPage.StrokeImageCallBack?.Invoke(photo);//Delegate
                    break;

                case PhotosPageMode.SelectImage:
                    Retouch_Photo2.PhotosPage.SelectImageCallBack?.Invoke(photo);//Delegate
                    break;
                case PhotosPageMode.ReplaceImage:
                    Retouch_Photo2.PhotosPage.ReplaceImageCallBack?.Invoke(photo);//Delegate
                    break;
                default:
                    return;
            }

            this.Frame.GoBack();
        }

    }
}