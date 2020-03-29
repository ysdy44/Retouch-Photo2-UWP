using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainPage" />. 
    /// </summary>
    public sealed partial class MainPage : Page
    {

        /// <summary>
        /// Refresh the GridView children.
        /// </summary>
        private async Task RefreshWrapGrid()
        {
            IEnumerable<StorageFile> orderedPhotos = await FileUtil.FindPhoto2pkFile();

            //Refresh, when the count is not equal.
            if (orderedPhotos.Count() != this.Photos.Count)
            {
                this.Photos.Clear(); //Notify
                this.GridView.Children.Clear();

                foreach (StorageFile storageFile in orderedPhotos)
                {
                    // [StorageFile] --> [Photo]
                    Photo photo = new Photo(storageFile, ApplicationData.Current.LocalFolder.Path);

                    if (photo != null)
                    {
                        this.Photos.Add(photo); //Notify
                        this.GridView.Children.Add(photo.Control);
                    }
                }
            }

            this._vsIsInitialVisibility = (this.Photos.Count == 0);
            this._vsState = MainPageState.Main;
            this.VisualState = this.VisualState;//State
        }

        /// <summary>
        /// Refresh the selected count.
        /// </summary>
        private void RefreshSelectCountRun()
        {
            int count = this.Photos.Count(p => p.Control.SelectMode == PhotoSelectMode.Selected);
            this.SelectCountRun.Text = count.ToString();
        }

        /// <summary>
        /// Refresh all photos select-mode.
        /// </summary>
        private void RefreshPhotosSelectMode(PhotoSelectMode selectMode)
        {
            foreach (Photo item in this.Photos)
            {
                item.Control.SelectMode = selectMode;
            }
        }


        private void NewProjectFromSize(BitmapSize pixels)
        {
            //Transition
            this.ViewModel.IsTransition = false;
            this.ViewModel.CanvasTransformer.Transition(0.0f);

            Project project = new Project((int)pixels.Width, (int)pixels.Height);//Project
            this.ViewModel.LoadFromProject(project);

            this.Frame.Navigate(typeof(DrawPage));//Navigate   
        }
        private async void NewProjectFromPhoto(Photo photo)
        {
            if (photo.Control is FrameworkElement element)
            {
                this.ViewModel.CanvasTransformer.Size = new Size(this.ActualWidth, this.ActualHeight - 50);

                //Transition
                this.ViewModel.IsTransition = true;
                this.ViewModel.SetCanvasTransformerRadian(0.0f);
                this.ViewModel.CanvasTransformer.Transition(0.0f);

                Point postion = Retouch_Photo2.Menus.MenuHelper.GetVisualPostion(element);
                float width = (float)element.ActualWidth;
                float height = (float)element.ActualHeight;
                this.ViewModel.CanvasTransformer.TransitionSource(postion, width, height);
            }

            if (photo.ZipFilePath != null)
            {
                await FileUtil.DeleteCacheAsync();

                await FileUtil.ExtractToDirectory(photo.ZipFilePath);
                await FileUtil.LoadImageRes(this.ViewModel.CanvasDevice);
                Project project = FileUtil.LoadProject(this.ViewModel.CanvasDevice);

                this.ViewModel.LoadFromProject(project);
            }

            this.Frame.Navigate(typeof(DrawPage));//Navigate     
        }
        private async Task NewProjectFromPictures(PickerLocationId location)
        {
            //ImageRe
            ImageRe imageRe = await FileUtil.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, location);
            if (imageRe == null) return;

            //Images
            ImageRe.DuplicateChecking(imageRe);
            ImageStr imageStr = imageRe.ToImageStr();

            //Transformer
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

            //Layer
            ImageLayer imageLayer = new ImageLayer
            {
                TransformManager = new TransformManager(transformerSource),
                StyleManager = new StyleManager(transformerSource, transformerSource, imageStr)
            };

            //Transition
            this.ViewModel.IsTransition = false;

            //Project
            Project project = new Project(imageLayer);
            this.Frame.Navigate(typeof(DrawPage), project);//Navigate       
        }

    }
}