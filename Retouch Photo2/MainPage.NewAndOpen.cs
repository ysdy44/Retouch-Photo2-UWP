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
            string untitled = this.Untitled;
            string name = this.RenameByRecursive(untitled);
            int width = (int)pixels.Width;
            int height = (int)pixels.Height;
                
            //Project
            {
                Project project = new Project
                {
                    Name = name,
                    Width = width,
                    Height = height,
                };
                this.ViewModel.LoadFromProject(project);
                this.Frame.Navigate(typeof(DrawPage), new TransitionData { Type = TransitionType.Size });//Navigate  
            }
        }

        /// <summary>
        /// Open from ProjectViewItem.
        /// </summary>
        /// <param name="projectViewItem"> The ProjectViewItem. </param>
        private async void OpenFromProjectViewItem(ProjectViewItem projectViewItem)
        {
            this.LoadingControl.IsActive = true;

            //Transition
            //Get the position of the image element relative to the screen.   
            FrameworkElement source = projectViewItem.ImageEx;
            Point sourcePostion = Retouch_Photo2.Menus.MenuHelper.GetVisualPostion(source);
            Size sourceSize = new Size(source.ActualWidth, source.ActualHeight);
            Rect sourceRect = new Rect(sourcePostion, sourceSize);
            Size pageSize = new Size(this.ActualWidth, this.ActualHeight - 50);
            TransitionData data = new TransitionData
            {
                Type = TransitionType.Transition,
                SourceRect = sourceRect,
                PageSize = pageSize
            };

            //FileUtil
            {
                if (projectViewItem.Photo2pkFilePath == null) return;

                await FileUtil.DeleteAllInTemporaryFolder();
                await FileUtil.ExtractZipFile(projectViewItem.Photo2pkFilePath);

                //Load all photos. 
                IEnumerable<Photo> photos = FileUtil.LoadPhotoFile();
                Photo.Instances.Clear();
                foreach (Photo p in photos)
                {
                    await FileUtil.ConstructPhotoAndPushInstances(this.ViewModel.CanvasDevice, p);
                    Photo.Instances.Add(p);
                }
            }

            //Project
            {
                string name = projectViewItem.Name;
                Project project = FileUtil.LoadProject(name);
                this.ViewModel.LoadFromProject(project);
            }

            this.LoadingControl.IsActive = false;
            this.Frame.Navigate(typeof(DrawPage), data);//Navigate   
        }

        /// <summary>
        /// New from Picture.
        /// </summary>
        /// <param name="pixels"> The picker locationId. </param>
        private async Task NewFromPicture(PickerLocationId location)
        {
            //Photo
            StorageFile copyFile = await FileUtil.PickAndCopySingleImageFileAsync(location);
            Photo photo = await FileUtil.CreatePhotoFromCopyFileAsync(this.ViewModel.CanvasDevice, copyFile);
            Photo.DuplicateChecking(photo);

            //Transformer
            string name = this.RenameByRecursive($"{photo.Name}");
            int width = (int)photo.Width;
            int height = (int)photo.Height;
            Transformer transformerSource = new Transformer(width, height, Vector2.Zero);

            //ImageLayer 
            Photocopier photocopier = photo.ToPhotocopier();
            ImageLayer imageLayer = new ImageLayer(transformerSource, photocopier);

            //Project
            Project project = new Project
            {
                Name = name,
                Width = width,
                Height = height,
                Layers = new List<ILayer>
                {
                     imageLayer
                }
            };
            this.ViewModel.LoadFromProject(project);
            this.Frame.Navigate(typeof(DrawPage), new TransitionData { Type = TransitionType.Size });//Navigate  
        }


        /// <summary>
        /// Get a name that doesn't have a rename.
        /// If there are, add the number.
        /// [Untitled] --> [Untitled1]   
        /// </summary>
        /// <param name="name"> The previous name. </param>
        /// <returns> The new name. </returns>
        private string RenameByRecursive(string name)
        {
            if (this._renamed(name) == false) return name;

            int num = 0;
            string newName;

            do
            {
                num++;
                newName = $"{name}{num}";
            }
            while (this._renamed(newName));

            return newName;
        }
        // Is there a re-named item?
        private bool _renamed(string name)
        {
            foreach (ProjectViewItem item in this.ProjectViewItems)
            {
                if (name == item.Name)
                {
                    return true;
                }
            }
            return false;
        }

    }
}