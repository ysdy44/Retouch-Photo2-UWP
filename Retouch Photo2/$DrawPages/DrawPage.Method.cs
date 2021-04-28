using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Photos;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        /// <summary>
        /// Export to ...
        /// </summary>
        private async Task<bool> Export()
        {
            //Render
            float width = (float)this.ExportSizePicker.SizeWith;
            float height = (float)this.ExportSizePicker.SizeHeight;
            int dpi = (int)this.DPIComboBox.DPI;
            bool isClearWhite = this.FileFormatComboBox.IsClearWhite;
            CanvasRenderTarget renderTarget = this.ViewModel.Render(width, height, dpi, isClearWhite);

            //Export
            return await FileUtil.SaveCanvasBitmapFile
            (
                renderTarget: renderTarget,

                fileChoices: this.FileFormatComboBox.FileChoices,
                suggestedFileName: this.ApplicationView.Title,

                fileFormat: this.FileFormatComboBox.FileFormat,
                quality: this.ExportQuality
            );
        }


        /// <summary>
        /// Save the current project.
        /// </summary>
        private async Task Save()
        {
            string name = this.ApplicationView.Title;
            int width = this.ViewModel.CanvasTransformer.Width;
            int height = this.ViewModel.CanvasTransformer.Height;
            StorageFolder zipFolder = await FileUtil.DeleteAllInZipFolder(name);


            //Save project file.
            Project project = new Project
            {
                Width = width,
                Height = height,
                Layerages = LayerManager.RootLayerage.Children
            };
            await Retouch_Photo2.XML.SaveProjectFile(zipFolder, project);

            //Save thumbnail file.
            IProjectViewItem item = this.Items.FirstOrDefault(p => p.Name == name);
            if (item != null)
            {
                CanvasRenderTarget thumbnail = this.ViewModel.Render(width, height);
                item.ImageSource = await FileUtil.SaveThumbnailFile(zipFolder, thumbnail);
            }

            //Save layers file.
            IEnumerable<Layerage> savedLayerages = LayerManager.GetFlatNestingLayerages(LayerManager.RootLayerage);
            IEnumerable<ILayer> savedLayers = from layer in LayerBase.Instances.Values where savedLayerages.Any(p => layer.Equals(p)) select layer;
            await XML.SaveLayersFile(zipFolder, savedLayers);

            //Save photos file.
            IEnumerable<Photocopier> savedPhotocopiers = LayerManager.GetPhotocopiers(savedLayerages);
            IEnumerable<Photo> savedPhotos = from photo in Photo.Instances.Values where savedPhotocopiers.Any(p => photo.Equals(p)) select photo;
            await XML.SavePhotosFile(zipFolder, savedPhotos);

            //Move photo file.
            foreach (Photo photo in savedPhotos)
            {
                //@Release: case Debug
                {
                    //await photo.MoveFile(zipFolder);
                }
                //@Release: case Release
                {
                    //Move photo file.
                    StorageFile file = await StorageFile.GetFileFromPathAsync(photo.ImageFilePath);
                    await file.CopyAsync(zipFolder);
                }
            }
        }


        /// <summary>
        /// Exit the current Project.
        /// </summary>
        private async Task Exit()
        {
            //Clear photos
            Photo.Instances.Clear();
            Photo.InstancesCollection.Clear();

            //Clear layers
            LayerBase.Instances.Clear();

            //Clear historys
            HistoryBase.Instances.Clear();

            //FileUtil
            await FileUtil.DeleteAllInTemporaryFolder();

            //Clear
            this.SelectionViewModel.SetModeNone();
            LayerManager.RootLayerage.Children.Clear();
            LayerManager.RootStackPanel.Children.Clear();
        }
    }
}