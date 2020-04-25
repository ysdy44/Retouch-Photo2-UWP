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
        /// Sets the page layout is full-screen. 
        /// </summary>
        public bool IsFullScreen
        {
            get => this.DrawLayout.IsFullScreen;
            set
            {
                this.UnFullScreenButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.DrawLayout.IsFullScreen = value;

                Vector2 offset = this.DrawLayout.FullScreenOffset;
                if (value)
                    this.ViewModel.CanvasTransformer.Position += offset;
                else
                    this.ViewModel.CanvasTransformer.Position -= offset;

                this.ViewModel.CanvasTransformer.ReloadMatrix();
            }
        }


        /// <summary>
        /// Export to ...
        /// </summary>
        private async Task<bool> Export()
        {
            //CanvasRenderTarget
            float width = this.ViewModel.CanvasTransformer.Width;
            float height = this.ViewModel.CanvasTransformer.Height;
            ICanvasResourceCreatorWithDpi resourceCreator = this.MainCanvasControl.CanvasControl;
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(resourceCreator, width, height);


            //Render
            Matrix3x2 matrix = Matrix3x2.CreateScale(1.0f, 1.0f);
            ICanvasImage canvasImage = this.MainCanvasControl.Render(matrix);
            using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
            {
                drawingSession.DrawImage(canvasImage);
            }


            //FileSavePicker
            string fileChoices = this.ExportComboBox.FileChoices;
            string suggestedFileName = this.ViewModel.Name;
            FileSavePicker savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.Desktop,
                SuggestedFileName = suggestedFileName,
            };
            savePicker.FileTypeChoices.Add("DB", new[] { fileChoices });


            //PickSaveFileAsync
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file == null) return false;

            try
            {
                CanvasBitmapFileFormat fileFormat = this.ExportComboBox.FileFormat;
                float quality = 1.0f;

                using (IRandomAccessStream accessStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await renderTarget.SaveAsync(accessStream, fileFormat, quality);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Save the current Project.
        /// </summary>
        private async Task Save()
        {
            string name = this.ViewModel.Name;
            int width = this.ViewModel.CanvasTransformer.Width;
            int height = this.ViewModel.CanvasTransformer.Height;
            IEnumerable<ILayer> layers = this.ViewModel.Layers.RootLayers;

            Project project = new Project
            {
                Name = name,
                Width = width,
                Height = height,
                Layers = layers
            };

            //Save thumbnail image.
            CanvasRenderTarget thumbnail = this.MainCanvasControl.RenderThumbnail(this.ViewModel.CanvasDevice, width, height);
            FileUtil.SaveThumbnailAsync(thumbnail, name);


            //Photos File
            //Save project to zip file.

            {
                await FileUtil.SaveProject(project);

                //@SavePhoto
                {
                    //Save Photo whitch "HasSavePhotocopier" is ""True"".            
                    /// <see cref="Retouch_Photo2.Elements.XML.SavePhotocopier"/>
                    IEnumerable<Photo> savedPhotos = from p in Photo.Instances where p.HasSavePhotocopier select p;
                    await FileUtil.SavePhotoFile(savedPhotos);

                    //Delete othors.
                    IEnumerable<Photo> unSavedPhotos = from p in Photo.Instances where p.HasSavePhotocopier == false select p;
                    foreach (Photo unSave in unSavedPhotos)
                    {
                        StorageFile item = await StorageFile.GetFileFromPathAsync(unSave.ImageFilePath);
                        await item.DeleteAsync();
                    }

                    //Clear
                    Photo.Instances.Clear();
                }

                //FileUtil
                await FileUtil.CreateZipFile(name);

                await FileUtil.DeleteAllInTemporaryFolder();
            }


            //Clear
            this.SelectionViewModel.SetModeNone();
            this.ViewModel.Layers.RootLayers.Clear();
            this.ViewModel.Layers.RootControls.Clear();

            this.IsFullScreen = true;
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}
        }

    }
}