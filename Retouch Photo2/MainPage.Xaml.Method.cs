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
            if (orderedPhotos.Count() != this.ProjectViewItems.Count)
            {
                this.ProjectViewItems.Clear(); //Notify
                this.GridView.Children.Clear();

                foreach (StorageFile storageFile in orderedPhotos)
                {
                    // [StorageFile] --> [projectViewItem]
                    ProjectViewItem projectViewItem = new ProjectViewItem(storageFile, ApplicationData.Current.LocalFolder.Path);

                    if (projectViewItem != null)
                    {
                        this.ProjectViewItems.Add(projectViewItem); //Notify
                        this.GridView.Children.Add(projectViewItem);
                    }
                }
            }

            this._vsIsInitialVisibility = (this.ProjectViewItems.Count == 0);
            this._vsState = MainPageState.Main;
            this.VisualState = this.VisualState;//State
        }

        /// <summary>
        /// Refresh the selected count.
        /// </summary>
        private void RefreshSelectCountRun()
        {
            int count = this.ProjectViewItems.Count(p => p.SelectMode == SelectMode.Selected);
            this.SelectCountRun.Text = count.ToString();
        }

        /// <summary>
        /// Refresh all photos select-mode.
        /// </summary>
        private void RefreshPhotosSelectMode(SelectMode selectMode)
        {
            foreach (ProjectViewItem item in this.ProjectViewItems)
            {
                item.SelectMode = selectMode;
            }
        }



        /// <summary>
        /// New from size.
        /// </summary>
        /// <param name="pixels"> The bitmap size. </param>
        private void NewFromSize(BitmapSize pixels)
        {
            {
                //Transition
                this.ViewModel.IsTransition = false;
                this.ViewModel.CanvasTransformer.Transition(0.0f);
            }

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
                this.Frame.Navigate(typeof(DrawPage));//Navigate   
            }
        }

        /// <summary>
        /// Open from ProjectViewItem.
        /// </summary>
        /// <param name="projectViewItem"> The ProjectViewItem. </param>
        private async void OpenFromProjectViewItem(ProjectViewItem projectViewItem)
        {
            //Transition
            {
                //Get the position of the image element relative to the screen.   
                FrameworkElement image = projectViewItem.ImageEx;
                float imageWidth = (float)image.ActualWidth;
                float imageHeight = (float)image.ActualHeight;
                Point postion = Retouch_Photo2.Menus.MenuHelper.GetVisualPostion(image);

                this.ViewModel.IsTransition = true;
                this.ViewModel.SetCanvasTransformerRadian(0.0f);
                this.ViewModel.CanvasTransformer.Transition(0.0f);
                this.ViewModel.CanvasTransformer.TransitionSource(postion, imageWidth, imageHeight);
                this.ViewModel.CanvasTransformer.Size = new Size(this.ActualWidth, this.ActualHeight - 50);
            }

            //FileUtil
            {
                if (projectViewItem.ZipFilePath == null) return;

                await FileUtil.DeleteCacheAsync();

                await FileUtil.ExtractToDirectory(projectViewItem.ZipFilePath);
                await FileUtil.LoadImageRes(this.ViewModel.CanvasDevice);
            }

            //Project
            {
                Project project = FileUtil.LoadProject(this.ViewModel.CanvasDevice);
                this.ViewModel.LoadFromProject(project);
                this.Frame.Navigate(typeof(DrawPage));//Navigate   
            }
        }

        /// <summary>
        /// New from Picture.
        /// </summary>
        /// <param name="pixels"> The picker locationId. </param>
        private async Task NewFromPicture(PickerLocationId location)
        {
            //Transition
            {
                this.ViewModel.IsTransition = false;
                this.ViewModel.CanvasTransformer.Transition(0.0f);
            }


            //ImageRe
            ImageRe imageRe = await FileUtil.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, location);
            if (imageRe == null) return;

            //Images            
            ImageRe.DuplicateChecking(imageRe);
            ImageStr imageStr = imageRe.ToImageStr();

            //Transformer
            string name = this.RenameByRecursive($"{imageRe.Name}");
            int width = (int)imageRe.Width;
            int height = (int)imageRe.Height;
            Transformer transformerSource = new Transformer(width, height, Vector2.Zero);

            //ImageLayer 
            ImageLayer imageLayer = new ImageLayer
            {
                TransformManager = new TransformManager(transformerSource),
                StyleManager = new StyleManager(transformerSource, transformerSource, imageStr)
            };


            //Project
            {                
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
                this.Frame.Navigate(typeof(DrawPage));//Navigate  
            }
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
            while (this._renamed(newName) == false);

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