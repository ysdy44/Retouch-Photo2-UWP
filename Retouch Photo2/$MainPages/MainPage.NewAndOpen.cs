using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Photos;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
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
    public sealed partial class MainPage : Page
    {

        /// <summary>
        /// New from size.
        /// </summary>
        /// <param name="pixels"> The bitmap size. </param>
        public void NewFromSize(BitmapSize pixels)
        {
            this.LoadingControl.State = LoadingState.Loading;
            this.LoadingControl.IsActive = true;

            //Project
            {
                string untitled = this.Untitled;
                string name = this.MainLayout.UntitledRenameByRecursive(untitled);
                int width = (int)pixels.Width;
                int height = (int)pixels.Height;

                Project project = new Project
                {
                    Name = name,
                    Width = width,
                    Height = height,
                };
                this.ViewModel.LoadFromProject(project);
            }


            //Transition
            Rect? sourceRect = null;

            this.LoadingControl.IsActive = false;
            this.LoadingControl.State = LoadingState.None;
            this.Frame.Navigate(typeof(DrawPage), sourceRect);//Navigate
        }


        /// <summary>
        /// Open from ProjectViewItem.
        /// </summary>
        /// <param name="projectViewItem"> The ProjectViewItem. </param>
        public async void OpenFromProjectViewItem(IProjectViewItem projectViewItem)
        {
            this.LoadingControl.State = LoadingState.Loading;
            this.LoadingControl.IsActive = true;

            //FileUtil
            string name = projectViewItem.Name;
            if (name == null || name == string.Empty)
            {
                this.LoadingControl.IsActive = false;
                this.LoadingControl.State = LoadingState.FileNull;
                await Task.Delay(800);
                this.LoadingControl.State = LoadingState.None;
                return;
            }


            await FileUtil.DeleteInTemporaryFolder();
            bool isExists = await FileUtil.MoveAllAndReturn(name);
            if (isExists == false)
            {
                this.LoadingControl.IsActive = false;
                this.LoadingControl.State = LoadingState.FileCorrupt;
                await Task.Delay(800);
                this.LoadingControl.State = LoadingState.None;
                return;
            }


            //Load all photos file. 
            Photo.Instances.Clear();
            IEnumerable<Photo> photos = XML.LoadPhotosFile();
            if (photos != null)
            {
                foreach (Photo photo in photos)
                {
                    await photo.ConstructPhotoSource(LayerManager.CanvasDevice);
                    Photo.Instances.Add(photo);
                }
            }

            //Load all layers file. 
            LayerBase.Instances.Clear();
            IEnumerable<ILayer> layers = XML.LoadLayersFile();
            if (layers != null)
            {
                foreach (ILayer layer in layers)
                {
                    LayerBase.Instances.Add(layer);
                }
            }

            //Load project file. 
            Project project = XML.LoadProjectFile(name);
            if (project == null)
            {
                this.LoadingControl.IsActive = false;
                this.LoadingControl.State = LoadingState.LoadFailed;
                await Task.Delay(800);
                this.LoadingControl.State = LoadingState.None;
                return;
            }

            this.ViewModel.LoadFromProject(project);
            this.SelectionViewModel.SetMode();//Selection


            //Transition
            Rect? sourceRect = projectViewItem.GetVisualRect(Window.Current.Content);

            this.LoadingControl.State = LoadingState.None;
            this.LoadingControl.IsActive = false;
            this.Frame.Navigate(typeof(DrawPage), sourceRect);//Navigate   
        }


        /// <summary>
        /// New from Picture.
        /// </summary>
        /// <param name="location"> The picker locationId. </param>
        public async Task NewFromPicture(PickerLocationId location)
        {
            StorageFile copyFile = await FileUtil.PickAndCopySingleImageFileAsync(location);

            await this._newFromPicture(copyFile);
        }
        /// <summary>
        /// New from Picture.
        /// </summary>
        /// <param name="item"> The storage item. </param>
        public async Task NewFromPicture(IStorageItem item)
        {
            StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(item);

            await this._newFromPicture(copyFile);
        }
        private async Task _newFromPicture(StorageFile copyFile)
        {
            this.LoadingControl.State = LoadingState.Loading;
            this.LoadingControl.IsActive = true;

            //Photo
            if (copyFile == null)
            {
                this.LoadingControl.State = LoadingState.None;
                this.LoadingControl.IsActive = false;
                return;
            }
            Photo photo = await Photo.CreatePhotoFromCopyFileAsync(LayerManager.CanvasDevice, copyFile);
            Photo.DuplicateChecking(photo);

            //Transformer
            string name = this.MainLayout.UntitledRenameByRecursive($"{photo.Name}");
            int width = (int)photo.Width;
            int height = (int)photo.Height;
            Transformer transformerSource = new Transformer(width, height, Vector2.Zero);

            //ImageLayer 
            Photocopier photocopier = photo.ToPhotocopier();
            ImageLayer imageLayer = new ImageLayer
            {
                Transform = new Transform(transformerSource),
                Photocopier = photocopier,
            };
            Layerage imageLayerage = imageLayer.ToLayerage();
            LayerBase.Instances.Add(imageLayer);

            //Project
            {
                Project project = new Project
                {
                    Name = name,
                    Width = width,
                    Height = height,
                    Layerages = new List<Layerage>
                    {
                         imageLayerage
                    }
                };
                this.ViewModel.LoadFromProject(project);
            }


            //Transition
            Rect? sourceRect = null;

            this.LoadingControl.State = LoadingState.None;
            this.LoadingControl.IsActive = false;
            this.Frame.Navigate(typeof(DrawPage), sourceRect);//Navigate
        }

    }
}