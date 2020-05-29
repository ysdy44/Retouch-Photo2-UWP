using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
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
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainPage" />. 
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string Untitled = "Untitled";

        /// <summary>
        /// New from size.
        /// </summary>
        /// <param name="pixels"> The bitmap size. </param>
        private void NewFromSize(BitmapSize pixels)
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
            TransitionData data = new TransitionData
            {
                Type = TransitionType.Size
            };

            this.LoadingControl.IsActive = false;
            this.LoadingControl.State = LoadingState.None;
            this.Frame.Navigate(typeof(DrawPage), data);//Navigate
        }

        /// <summary>
        /// Open from ProjectViewItem.
        /// </summary>
        /// <param name="projectViewItem"> The ProjectViewItem. </param>
        private async void OpenFromProjectViewItem(IProjectViewItem projectViewItem)
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


            //Load all photos. 
            IEnumerable<Photo> photos = XML.LoadPhotosFile();
            Photo.Instances.Clear();
            foreach (Photo photo in photos)
            {
                await photo.ConstructPhotoSource(this.ViewModel.CanvasDevice);
                Photo.Instances.Add(photo);
            }

            //Load all layers. 
            IEnumerable<ILayer> layers = XML.LoadLayersFile();
            LayerBase.Instances.Clear();
            foreach (ILayer layer in layers)
            {
                LayerBase.Instances.Add(layer);
            }


            //Project
            {
                Project project = XML.LoadProjectFile(name);
                this.ViewModel.LoadFromProject(project);
                this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
            }

            //Transition
            TransitionData data = new TransitionData
            {
                Type = TransitionType.Transition,
                SourceRect = projectViewItem.GetVisualRect(Window.Current.Content),
                PageSize = new Size(this.ActualWidth, this.ActualHeight - 50)
            };

            this.LoadingControl.IsActive = false;
            this.LoadingControl.State = LoadingState.None;
            this.Frame.Navigate(typeof(DrawPage), data);//Navigate   
        }


        /// <summary>
        /// New from Picture.
        /// </summary>
        /// <param name="pixels"> The picker locationId. </param>
        private async Task NewFromPicture(PickerLocationId location)
        {
            StorageFile copyFile = await FileUtil.PickAndCopySingleImageFileAsync(location);

            await this._newFromPicture(copyFile);
        }
        private async Task NewFromPicture(IStorageItem item)
        {
            StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(item);

            await this._newFromPicture(copyFile);
        }
        private async Task _newFromPicture(StorageFile copyFile)
        {
            this.LoadingControl.State = LoadingState.Loading;
            this.LoadingControl.IsActive = true;

            //Photo
            if (copyFile == null) return;
            Photo photo = await Photo.CreatePhotoFromCopyFileAsync(this.ViewModel.CanvasDevice, copyFile);
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
            TransitionData data = new TransitionData
            {
                Type = TransitionType.Size
            };

            this.LoadingControl.IsActive = false;
            this.LoadingControl.State = LoadingState.None;
            this.Frame.Navigate(typeof(DrawPage), data);//Navigate
        }

    }
}