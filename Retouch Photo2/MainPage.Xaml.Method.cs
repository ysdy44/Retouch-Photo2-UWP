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
            IEnumerable<StorageFile> orderedPhotos = await FileUtil.FIndFilesInLocalFolder();

            //Refresh, when the count is not equal.
            if (orderedPhotos.Count() != this.ProjectViewItems.Count)
            {
                this.ProjectViewItems.Clear(); //Notify

                foreach (StorageFile storageFile in orderedPhotos)
                {
                    // [StorageFile] --> [projectViewItem]
                    string name = storageFile.DisplayName;
                    string zipFile = storageFile.Path;
                    string thumbnail = $"{ApplicationData.Current.LocalFolder.Path}\\{name}.png";
                    ProjectViewItem item = new ProjectViewItem(name, zipFile, thumbnail);

                    if (item != null) this.ProjectViewItems.Add(item); //Notify
                }
            }

            if (this.ProjectViewItems.Count == 0)
                this.MainLayout.MainPageState = MainPageState.Initial;
            else
                this.MainLayout.MainPageState = MainPageState.Main;
        }

        /// <summary>
        /// Refresh the selected count.
        /// </summary>
        private void RefreshSelectCount()
        {
            int count = this.ProjectViewItems.Count(p => p.SelectMode == SelectMode.Selected);

            this.MainLayout.SelectText = count.ToString();

            bool isEnable = (count != 0);
            this.DeleteOKButton.IsEnabled = isEnable;
            this.DuplicateOKButton.IsEnabled = isEnable;
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
            this.LoadingControl.IsActive = true;
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
                if (projectViewItem.Photo2pkFilePath == null) return;

                await FileUtil.DeleteAllInTemporaryFolder();

                await FileUtil.ExtractZipFile(projectViewItem.Photo2pkFilePath);
                await FileUtil.LoadImageRes(this.ViewModel.CanvasDevice);
            }

            //Project
            {
                string name = projectViewItem.Name;
                Project project = FileUtil.LoadProject(this.ViewModel.CanvasDevice, name);
                this.ViewModel.LoadFromProject(project);
            }

            this.LoadingControl.IsActive = false;
            this.Frame.Navigate(typeof(DrawPage));//Navigate   
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
        /// Rename the ProjectViewItem.
        /// </summary>
        /// <param name="item"> The ProjectViewItem. </param>
        private async Task RenameProjectViewItem(ProjectViewItem item)
        {
            string oldName = item.Name;
            string newName = this.RenameTextBox.Text;
            if (oldName == newName)
            {
                this.RenameTipTextBlock.Visibility = Visibility.Visible;
                return;
            }

            ProjectViewItem hasRenamed = this.ProjectViewItems.FirstOrDefault(p => p.Name == newName);
            if (hasRenamed != null)
            {
                this.RenameTipTextBlock.Visibility = Visibility.Visible;
                return;
            }

            //Rename
            await FileUtil.RenameZipFileAndThumbnail(item, newName);
            this.HideRenameDialog();
        }

        /// <summary>
        /// Delete all selected ProjectViewItem(s).
        /// </summary>
        private async Task DeleteProjectViewItems(IList<ProjectViewItem> items)
        {
            foreach (ProjectViewItem item in items)
            {
                await FileUtil.DeleteZipFileAndThumbnail(item.Name);

                item.Visibility = Visibility.Collapsed;
                this.ProjectViewItems.Remove(item);//Notify

                await Task.Delay(300);
            }
        }

        /// <summary>
        /// Duplicate all selected ProjectViewItem(s).
        /// </summary>     
        private async Task DuplicateProjectViewItems(IList<ProjectViewItem> items)
        {
            foreach (ProjectViewItem item in items)
            {
                string oldName = item.Name;
                string newName = this.RenameByRecursive(oldName);
                StorageFile storageFile = await FileUtil.DuplicateZipFileAndThumbnail(oldName, newName);

                string zipFile = storageFile.Path;
                string thumbnail = $"{ApplicationData.Current.LocalFolder.Path}\\{newName}.png";
                ProjectViewItem newItem = new ProjectViewItem(newName, zipFile, thumbnail);

                this.ProjectViewItems.Add(newItem);//Notify
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