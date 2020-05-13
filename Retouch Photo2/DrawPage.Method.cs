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
            float canvasWidth = this.ViewModel.CanvasTransformer.Width;
            float canvasHeight = this.ViewModel.CanvasTransformer.Height;
            
            float fileWidth = this.ViewModel.CanvasTransformer.Width;//@Debug
            float fileHeight = this.ViewModel.CanvasTransformer.Height;//@Debug

            float scaleX = fileWidth / canvasWidth;
            float scaleY = fileHeight / canvasHeight;
            Matrix3x2 matrix = Matrix3x2.CreateScale(scaleX, scaleY);

            //Render
            ICanvasResourceCreatorWithDpi canvasResource = this.MainCanvasControl.CanvasResourceCreatorWithDpi;
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(canvasResource, fileWidth, fileHeight);
            ICanvasImage canvasImage = this.MainCanvasControl.Render(matrix);
            using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
            {
                drawingSession.DrawImage(canvasImage);
            }

            //Export
            return await FileUtil.ExportStorageFile
            (
                renderTarget: renderTarget,

                fileChoices: this.ExportComboBox.FileChoices,
                suggestedFileName: this.ViewModel.Name,

                fileFormat: this.ExportComboBox.FileFormat,
                quality: 1.0f
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

            //Save Project File.
            Project project = new Project
            {
                Name = name,
                Width = width,
                Height = height,
                Layers = this.ViewModel.Layers.RootLayers
            };
            await FileUtil.SaveProject(project);
            
            //Save Thumbnail Image.
            CanvasRenderTarget thumbnail = this.MainCanvasControl.RenderThumbnail(this.ViewModel.CanvasDevice, width, height);
            FileUtil.SaveThumbnailAsync(thumbnail, name);
            
            //Save Photos File and Delete useless.
            IEnumerable<Photocopier> savedPhotocopiers = this.ViewModel.Layers.GetPhotocopiers();
            IEnumerable<Photo> savedPhotos = FileUtil.GetPhotosAndDeleteUseless(Photo.Instances, savedPhotocopiers);
            await FileUtil.SavePhotoFile(savedPhotos);

            //Clear Photos
            Photo.Instances.Clear();
            

            //FileUtil
            await FileUtil.CreateZipFile(name);
            await FileUtil.DeleteAllInTemporaryFolder();
            

            //Clear
            this.SelectionViewModel.SetModeNone();
            this.ViewModel.Layers.RootLayers.Clear();
            this.ViewModel.Layers.RootControls.Clear();
        }

    }
}