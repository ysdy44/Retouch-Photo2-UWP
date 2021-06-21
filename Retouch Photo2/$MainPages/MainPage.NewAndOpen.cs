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
        /// New from project.
        /// </summary>
        /// <param name="project"> The project. </param>
        public void NewFromProject(Project project)
        {
            this.LoadingControl.State = LoadingState.Loading;

            string untitled = this.Untitled;
            string name = this.UntitledRenameByRecursive(untitled);

            // Item
            IProjectViewItem item = new ProjectViewItem
            {
                Name = name,
                ImageSource = null,
                Project = project,
            };
            this.Items.Insert(0, item);

            this.LoadingControl.State = LoadingState.None;
            this.Frame.Navigate(typeof(DrawPage), item); // Navigate
        }


        /// <summary>
        /// Open from ProjectViewItem.
        /// </summary>
        /// <param name="item"> The ProjectViewItem. </param>
        public async void OpenFromProjectViewItem(IProjectViewItem item)
        {
            this.LoadingControl.State = LoadingState.Loading;

            // FileUtil
            string name = item.Name;
            if (name == null || name == string.Empty)
            {
                this.LoadingControl.State = LoadingState.FileNull;
                await Task.Delay(800);
                this.LoadingControl.State = LoadingState.None;
                return;
            }

            // FileUtil
            await FileUtil.DeleteAllInTemporaryFolder();
            bool isExists = await FileUtil.MoveAllInZipFolderToTemporaryFolder(name);
            if (isExists == false)
            {
                this.LoadingControl.State = LoadingState.FileCorrupt;
                await Task.Delay(800);
                this.LoadingControl.State = LoadingState.None;
                return;
            }


            // Load all photos file. 
            Photo.Instances.Clear();
            Photo.InstancesCollection.Clear();
            IEnumerable<Photo> photos = XML.LoadPhotosFile();
            if (photos != null)
            {
                foreach (Photo photo in photos)
                {
                    string id = photo.FolderRelativeId;
                    await photo.ConstructPhotoSource(LayerManager.CanvasDevice);
                    Photo.Instances.Add(id, photo);
                    Photo.UpdateInstancesCollection();
                }
            }

            // Load all layers file. 
            LayerBase.Instances.Clear();
            IEnumerable<ILayer> layers = XML.LoadLayersFile();
            if (layers != null)
            {
                foreach (ILayer layer in layers)
                {
                    string id = layer.Id;
                    LayerBase.Instances.Add(id, layer);
                }
            }

            // Load project file. 
            Project project = XML.LoadProjectFile();
            if (project == null)
            {
                this.LoadingControl.State = LoadingState.LoadFailed;
                await Task.Delay(800);
                this.LoadingControl.State = LoadingState.None;
                return;
            }

            // Item
            item.Project = project;
            item.RenderImageVisualRect(Window.Current.Content);

            this.LoadingControl.State = LoadingState.None;
            this.Frame.Navigate(typeof(DrawPage), item); // Navigate   
        }


        /// <summary>
        /// New from Picture.
        /// </summary>
        /// <param name="location"> The picker locationId. </param>
        public async Task NewFromPicture(PickerLocationId location)
        {
            StorageFile file = await FileUtil.PickSingleImageFileAsync(location);
            StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(file);

            await this.NewFromPictureCore(copyFile);
        }
        /// <summary>
        /// New from Picture.
        /// </summary>
        /// <param name="item"> The storage item. </param>
        public async Task NewFromPicture(IStorageItem item)
        {
            StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(item);

            await this.NewFromPictureCore(copyFile);
        }
        private async Task NewFromPictureCore(StorageFile copyFile)
        {
            this.LoadingControl.State = LoadingState.Loading;

            // Photo
            if (copyFile == null)
            {
                this.LoadingControl.State = LoadingState.None;
                return;
            }
            Photo photo = await Photo.CreatePhotoFromCopyFileAsync(LayerManager.CanvasDevice, copyFile);
            Photo.DuplicateChecking(photo);

            // Transformer
            string name = this.UntitledRenameByRecursive($"{photo.Name}");
            int width = (int)photo.Width;
            int height = (int)photo.Height;
            Transformer transformerSource = new Transformer(width, height, Vector2.Zero);

            // ImageLayer 
            Photocopier photocopier = photo.ToPhotocopier();
            Layerage imageLayerage = Layerage.CreateByGuid();
            ImageLayer imageLayer = new ImageLayer
            {
                Id = imageLayerage.Id,
                Transform = new Transform(transformerSource),
                Photocopier = photocopier,
            };
            LayerBase.Instances.Add(imageLayerage.Id, imageLayer);

            // Project
            Project project = new Project
            {
                Width = width,
                Height = height,
                Layerages = new List<Layerage>
                {
                     imageLayerage
                }
            };

            // Item
            IProjectViewItem item = new ProjectViewItem
            {
                Name = name,
                ImageSource = null,
                Project = project,
            };
            this.Items.Insert(0, item);

            this.LoadingControl.State = LoadingState.None;
            this.Frame.Navigate(typeof(DrawPage), item); // Navigate
        }

    }
}