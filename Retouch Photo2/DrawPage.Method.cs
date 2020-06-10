using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Numerics;
using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Streams;
using System.Linq;
using Retouch_Photo2.Menus;
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {        

        /// <summary>
        /// Export to ...
        /// </summary>
        private async Task<bool> Export()
        {
            //Render
            BitmapSize size = this.ExportSizePicker.Size;
            int dpi = (int)this.DPIComboBox.DPI;
            bool isClearWhite = this.FileFormatComboBox.IsClearWhite;
            CanvasRenderTarget renderTarget = this.MainCanvasControl.Render(size, dpi, isClearWhite);

            //Export
            return await FileUtil.SaveCanvasBitmapFile
            (
                renderTarget: renderTarget,

                fileChoices: this.FileFormatComboBox.FileChoices,
                suggestedFileName: this.ViewModel.Name,

                fileFormat: this.FileFormatComboBox.FileFormat,
                quality: this.ExportQualityPicker.Value
            );
        }


        /// <summary>
        /// Save the current Project.
        /// </summary>
        private async Task Save()
        {
            string name = this.ViewModel.Name;
            int width = this.ViewModel.CanvasTransformer.Width;
            int height = this.ViewModel.CanvasTransformer.Height;
            StorageFolder zipFolder = await FileUtil.DeleteAllAndReturn(name);


            //Save project file.
            Project project = new Project
            {
                Name = name,
                Width = width,
                Height = height,
                Layerages = this.ViewModel.LayerageCollection.RootLayerages
            };
            await Retouch_Photo2.XML.SaveProjectFile(zipFolder, project);

            //Save thumbnail file.
            CanvasRenderTarget thumbnail = this.MainCanvasControl.Render(width, height);
            await FileUtil.SaveThumbnailFile(zipFolder, thumbnail);
            
            //Save layers file.
            IEnumerable<Layerage> savedLayerages = LayerageCollection.GetLayerages(this.ViewModel.LayerageCollection.RootLayerages);
            IEnumerable<ILayer> savedLayers = from layer in LayerBase.Instances where savedLayerages.Any(p => layer.Equals(p)) select layer;
            await XML.SaveLayersFile(zipFolder, savedLayers);

            //Save photos file.
            IEnumerable<Photocopier> savedPhotocopiers = LayerageCollection.GetPhotocopiers(savedLayerages);
            IEnumerable<Photo> savedPhotos = from photo in Photo.Instances where savedPhotocopiers.Any(p => photo.Equals(p)) select photo;
            await XML.SavePhotosFile(zipFolder, savedPhotos);

            //Move photo file.
            foreach (Photo photo in savedPhotos)
            {
                //await photo.MoveFile(zipFolder);
                //@Release
                {
                    //Move photo file.
                    StorageFile item = await StorageFile.GetFileFromPathAsync(photo.ImageFilePath);
                    await item.CopyAsync(zipFolder);
                }
            }
        }


        /// <summary>
        /// Exit the current Project.
        /// </summary>
        private async Task Exit()
        {
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                menu.Expander.State = ExpanderState.Hide;
            }

            //Clear photos
            Photo.Instances.Clear();

            //Clear layers
            LayerBase.Instances.Clear();

            //FileUtil
            await FileUtil.DeleteInTemporaryFolder();


            //Clear
            this.ViewModel.Historys.Clear();
            this.SelectionViewModel.SetModeNone();
            this.ViewModel.LayerageCollection.RootLayerages.Clear();
            this.ViewModel.LayerageCollection.RootControls.Clear();
        }
    }
}