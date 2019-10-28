using FanKit.Transformers;
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
using System.Xml.Linq;
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
        
        private async Task ConstructSettingViewModel()
        {
            //Setting
            SettingViewModel setting = null;

            try
            {
                setting = await SettingViewModel.CreateFromLocalFile();
            }
            catch (Exception)
            {
            }

            if (setting != null)
            {
                this.SettingViewModel = setting;
            }

            ElementTheme theme = this.SettingViewModel.ElementTheme;
            ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);
        }


        private void NewProjectFromSize(BitmapSize pixels)
        {
            Project project = new Project((int)pixels.Width, (int)pixels.Height);//Project
            this.ViewModel.LoadFromProject(project);

            this.Frame.Navigate(typeof(DrawPage));//Navigate   
        }
        private void NewProjectFromPhoto(object sender, Photo photo)
        {
            if (this.State != MainPageState.Main) return;

            if (sender is FrameworkElement element)
            {
                this.NavigatedFrom(element);
            }

            if (photo.Path != null)
            {
                //Create an XDocument object.
                string path = photo.Path;
                XDocument document = XDocument.Load(path);

                Project project = Retouch_Photo2.ViewModels.XML.LoadProject(document);
                this.ViewModel.LoadFromProject(project);
            }

            this.Frame.Navigate(typeof(DrawPage));//Navigate     
        }
        private async Task NewProjectFromPictures(PickerLocationId location)
        {
            //ImageRe
            ImageRe imageRe = await ImageRe.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, location);
            if (imageRe == null) return;

            //Images
            this.ViewModel.DuplicateChecking(imageRe);

            //Transformer
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

            //Layer
            ImageLayer imageLayer = new ImageLayer
            {
                TransformManager = new TransformManager(transformerSource),
                ImageRe = imageRe,
            };

            //Project
            Project project = new Project(imageLayer);
            this.Frame.Navigate(typeof(DrawPage), project);//Navigate       
        }
        

        private async Task Refresh()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            //get all file.
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();

            //Sort by Time
            IOrderedEnumerable<StorageFile> orderedFiles = files.OrderByDescending(file => file.DateCreated);

            this.PhotoFileList.Clear(); //Notify
            this.GridView.Children.Clear();

            foreach (StorageFile storageFile in orderedFiles)
            {
                // [StorageFile] --> [Photo]
                Photo photo = Photo.CreatePhoto(storageFile, ApplicationData.Current.LocalFolder.Path);

                if (photo != null)
                {
                    this.PhotoFileList.Add(photo); //Notify
                    this.GridView.Children.Add(photo.Instance);
                }
            }

            bool isZero = (this.PhotoFileList.Count == 0);
            this.State = isZero ?
                MainPageState.None :
                MainPageState.Main;//State
        }


        private void NavigatedTo()
        {
        }
        private void NavigatedFrom(FrameworkElement element)
        {
            this.ViewModel.CanvasTransformer.Size = new Size(this.ActualWidth, this.ActualHeight - 50);

            //Transition
            this.ViewModel.CanvasTransformer.Transition(0.0f);

            Point postion = Retouch_Photo2.Menus.MenuHelper.GetVisualPostion(element);
            float width = (float)element.ActualWidth;
            float height = (float)element.ActualHeight;
            this.ViewModel.CanvasTransformer.TransitionSource(postion, width, height);
        }

    }
}