using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Photos;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        // TaskCompletionSource
        private TaskCompletionSource<Photo> GalleryTaskSource;

        private async Task<Photo> ShowGalleryDialogTask()
        {
            this.GalleryDialog.Show();

            this.GalleryTaskSource = new TaskCompletionSource<Photo>();
            Photo resultPhoto = await this.GalleryTaskSource.Task;
            this.GalleryTaskSource = null;
            return resultPhoto;
        }

        private void GalleryDialogTrySetResult(FrameworkElement element, Photo photo)
        {
            if (this.GalleryTaskSource != null && this.GalleryTaskSource.Task.IsCanceled == false)
            {
                this.GalleryTaskSource.TrySetResult(photo);
            }

            this.GalleryDialog.Hide();
        }


        //////////////////////////


        // Gallery
        private void ConstructGalleryDialog()
        {
            this.GalleryDialog.SecondaryButtonClick += (s, e) => this.GalleryDialogTrySetResult(null, null);
            this.GalleryDialog.PrimaryButtonClick += async (s, e) =>
            {
                // Files
                IReadOnlyList<StorageFile> files = await FileUtil.PickMultipleImageFilesAsync(PickerLocationId.Desktop);
                if (files == null) return;

                foreach (StorageFile file in files)
                {
                    StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(file);
                    if (copyFile == null) continue;
                    Photo photo = await Photo.CreatePhotoFromCopyFileAsync(LayerManager.CanvasDevice, copyFile);
                    Photo.DuplicateChecking(photo);
                }
            };


            this.GalleryDialog.AllowDrop = true;
            this.GalleryDialog.Drop += async (s, e) =>
            {
                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {
                    IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();

                    if (items == null) return;

                    foreach (IStorageItem item in items)
                    {
                        // Photo
                        StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(item);
                        if (copyFile == null) continue;
                        Photo photo = await Photo.CreatePhotoFromCopyFileAsync(LayerManager.CanvasDevice, copyFile);
                        Photo.DuplicateChecking(photo);
                    }
                }
            };
            this.GalleryDialog.DragOver += (s, e) =>
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
                //e.DragUIOverride.Caption = App.resourceLoader.GetString("DropAcceptable_");
                e.DragUIOverride.IsCaptionVisible = e.DragUIOverride.IsContentVisible = e.DragUIOverride.IsGlyphVisible = true;
            };
        }


        // DragAndDrop
        private void ConstructDragAndDrop()
        {
            this.AllowDrop = true;
            this.Drop += async (s, e) =>
            {
                if (this.GalleryDialog.IsShow) return;

                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {
                    IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();
                    await this.CopyMultipleImageFilesAndCreateImageLayersAsync(items);
                }
            };
            this.DragOver += (s, e) =>
            {
                if (this.GalleryDialog.IsShow) return;

                e.AcceptedOperation = DataPackageOperation.Copy;
                //e.DragUIOverride.Caption = App.resourceLoader.GetString("DropAcceptable_");
                e.DragUIOverride.IsCaptionVisible = e.DragUIOverride.IsContentVisible = e.DragUIOverride.IsGlyphVisible = true;
            };
        }


        //////////////////////////


        private async void ShowGalleryDialog()
        {
            Photo photo = await this.ShowGalleryDialogTask();
            if (photo == null) return;

            // History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer);
            this.ViewModel.HistoryPush(history);

            // Layer
            Layerage imageLayerage = Layerage.CreateByGuid();
            ImageLayer imageLayer = new ImageLayer(photo)
            {
                Id = imageLayerage.Id,
                IsSelected = true,
            };
            LayerBase.Instances.Add(imageLayerage.Id, imageLayer);

            // Mezzanine
            LayerManager.Mezzanine(imageLayerage);

            this.SelectionViewModel.SetMode(); // Selection
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.ViewModel.Invalidate(); // Invalidate
        }
        private async Task CopyMultipleImageFilesAndCreateImageLayersAsync(IReadOnlyList<IStorageItem> items)
        {
            if (items == null) return;

            // History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer);
            this.ViewModel.HistoryPush(history);

            IList<Layerage> imageLayerages = new List<Layerage>();
            foreach (IStorageItem item in items)
            {
                // Photo
                StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(item);
                if (copyFile == null) continue;
                Photo photo = await Photo.CreatePhotoFromCopyFileAsync(LayerManager.CanvasDevice, copyFile);
                Photo.DuplicateChecking(photo);

                if (photo == null) continue;

                // Layer
                Layerage imageLayerage = Layerage.CreateByGuid();
                ImageLayer imageLayer = new ImageLayer(photo)
                {
                    Id = imageLayerage.Id,
                    IsSelected = true,
                };
                LayerBase.Instances.Add(imageLayerage.Id, imageLayer);
                imageLayerages.Add(imageLayerage);
            }

            // Mezzanine
            LayerManager.MezzanineRange(imageLayerages);

            this.SelectionViewModel.SetMode(); // Selection
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.ViewModel.Invalidate(); // Invalidate     
        }

    }
}