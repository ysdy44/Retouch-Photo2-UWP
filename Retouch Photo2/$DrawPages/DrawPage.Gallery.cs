using Retouch_Photo2.Layers;
using Retouch_Photo2.Photos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //@Static       
        private TaskCompletionSource<Photo> taskSource;


        private async Task<Photo> ShowGalleryDialogFunc()
        {
            this.GalleryDialog.Show();

            this.taskSource = new TaskCompletionSource<Photo>();
            Photo resultPhoto = await this.taskSource.Task;
            this.taskSource = null;
            return resultPhoto;
        }

        private void PhotoFlyoutShow(FrameworkElement element, Photo photo)
        {
            this.BillboardCanvas.Show(element, photo);
        }
        private void PhotoItemClick(FrameworkElement element, Photo photo)
        {
            if (this.taskSource != null && this.taskSource.Task.IsCanceled == false)
            {
                this.taskSource.TrySetResult(photo);
            }

            this.GalleryDialog.Hide();
        }


        private void ConstructDragAndDrop()
        {
            this.AllowDrop = true;
            this.Drop += async (s, e) =>
            {
                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {
                    IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();
                    await this.CopyMultipleImageFilesAsync(items);
                }
            };
            this.DragOver += (s, e) =>
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
                //e.DragUIOverride.Caption = App.resourceLoader.GetString("DropAcceptable_");//可以接受的图片
                e.DragUIOverride.IsCaptionVisible = e.DragUIOverride.IsContentVisible = e.DragUIOverride.IsGlyphVisible = true;
            };
        }

                 
        private async Task CopyMultipleImageFilesAsync(IReadOnlyList<StorageFile> files)
        {
            if (files == null) return;

            foreach (StorageFile file in files)
            {
                StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(file);
                if (copyFile == null) return;
                Photo photo = await Photo.CreatePhotoFromCopyFileAsync(LayerManager.CanvasDevice, copyFile);
                Photo.DuplicateChecking(photo);
            }
        }
        private async Task CopyMultipleImageFilesAsync(IReadOnlyList<IStorageItem> items)
        {
            if (items == null) return;

            foreach (IStorageItem item in items)
            {
                //Photo
                StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(item);
                if (copyFile == null) return;
                Photo photo = await Photo.CreatePhotoFromCopyFileAsync(LayerManager.CanvasDevice, copyFile);
                Photo.DuplicateChecking(photo);
            }
        }

    }
}