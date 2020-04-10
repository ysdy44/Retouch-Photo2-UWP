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


        //ViewModel
        private void ConstructViewModel()
        {
            this.MainCanvasControl.ConstructViewModel();
        }
        //KeyboardViewModel
        private void ConstructKeyboardViewModel()
        {
            //Move
            if (this.KeyboardViewModel.Move == null)
            {
                this.KeyboardViewModel.Move += (value) =>
                {
                    this.ViewModel.CanvasTransformer.Position += value;
                    this.ViewModel.CanvasTransformer.ReloadMatrix();
                    this.ViewModel.Invalidate();//Invalidate
                };
            }

            //FullScreen
            if (this.KeyboardViewModel.FullScreenChanged == null)
            {
                this.KeyboardViewModel.FullScreenChanged += (isFullScreen) =>
                {
                    this.IsFullScreen = isFullScreen;
                    this.ViewModel.Invalidate();//Invalidate
                };
            }
        }


        //Save
        private void ConstructExportDialog()
        {
            this.ExportDialog.CloseButton.Click += (sender, args) => this.ExportDialog.Hide();

            this.ExportDialog.PrimaryButton.Click += async (_, __) =>
            {
                this.ExportDialog.Hide();

                this.LoadingControl.IsActive = true;
                bool isSuccesful = await this.Export();
                this.LoadingControl.IsActive = false;

                this.ViewModel.TextVisibility = Visibility.Visible;
                this.ViewModel.Text = isSuccesful ? "  ✔  " : "  ❌  ";
                await Task.Delay(2000);
                this.ViewModel.TextVisibility = Visibility.Collapsed;
            };
        }


        //Setup
        private void ConstructSetupDialog()
        {
            this.SetupDialog.CloseButton.Click += (sender, args) => this.SetupDialog.Hide();

            this.SetupDialog.PrimaryButton.Click += (_, __) =>
            {
                this.SetupDialog.Hide();

                BitmapSize size = this.SetupSizePicker.Size;

                this.ViewModel.CanvasTransformer.Width = (int)size.Width;
                this.ViewModel.CanvasTransformer.Height = (int)size.Height;

                this.ViewModel.Invalidate();//Invalidate
            };
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

            {
                //ImageResFile
                IEnumerable<ImageRe> imageRes = ImageRe.Instances;//@Debug
                await FileUtil.SaveImageResFile(imageRes);
                ImageRe.Instances.Clear();

                //Save project to zip file.
                await FileUtil.SaveProject(project);
                await FileUtil.CreateZipFile(name);

                //FileUtil
                await FileUtil.DeleteAllInTemporaryFolder();
            }

            {
                //Clear
                this.SelectionViewModel.SetModeNone();
                this.ViewModel.Layers.RootLayers.Clear();
                this.ViewModel.Layers.RootControls.Clear();

                this.IsFullScreen = true;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}
            }
        }

    }
}