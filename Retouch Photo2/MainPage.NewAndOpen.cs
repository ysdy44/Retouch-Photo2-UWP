using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
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
        
        /// <summary>
        /// New from size.
        /// </summary>
        /// <param name="pixels"> The bitmap size. </param>
        private void NewFromSize(BitmapSize pixels)
        {
            string name = this.RenameByRecursive("Untitled");
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

                //Load all images. 
                IEnumerable<ImageRe> imageRes = FileUtil.LoadImageResFile();
                ImageRe.Instances.Clear();
                foreach (ImageRe imageRe in imageRes)
                {
                    await FileUtil.ConstructImageReAndPushInstances(this.ViewModel.CanvasDevice, imageRe);
                    ImageRe.Instances.Add(imageRe);
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
            //ImageRe
            StorageFile copyFile = await FileUtil.PickAndCopySingleImageFileAsync(location);
            ImageRe imageRe = await FileUtil.CreateImageReFromCopyFileAsync(this.ViewModel.CanvasDevice, copyFile);
            ImageRe.DuplicateChecking(imageRe);

            //Transformer
            string name = this.RenameByRecursive($"{imageRe.Name}");
            int width = (int)imageRe.Width;
            int height = (int)imageRe.Height;
            Transformer transformerSource = new Transformer(width, height, Vector2.Zero);

            //ImageLayer 
            ImageStr imageStr = imageRe.ToImageStr();
            ImageLayer imageLayer = new ImageLayer
            {
                TransformManager = new TransformManager(transformerSource),
                StyleManager = new StyleManager(transformerSource, transformerSource, imageStr)
            };

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