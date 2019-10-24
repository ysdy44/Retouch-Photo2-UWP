using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainPage" />. 
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private async Task LoadSettingViewModel()
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


        #region MainPageState


        private void SetMainPageState(MainPageState state)
        {
            this.State = state;
            bool? selectMode = (state == MainPageState.Main) ? (bool?)null : (bool?)false;

            foreach (Photo item in this.PhotoFileList)
            {
                item.SelectMode = selectMode;
            }
        }


        private async void AddDialogShow()
        {
            AddDialog addDialog = new AddDialog();
            Grid.SetRow(addDialog, 1);
            Grid.SetRowSpan(addDialog, 2);

            this.RootGrid.Children.Add(addDialog);

            //Add
            addDialog.CloseButtonClick += (sender, args) =>
            {
                addDialog.Hide();
                this.RootGrid.Children.Remove(addDialog);
            };
            addDialog.PrimaryButtonClick += (sender, args) =>
            {
                addDialog.Hide();
                this.RootGrid.Children.Remove(addDialog);

                this.IsLoading = true;//Loading

                BitmapSize pixels = addDialog.Size;
                Project project = new Project((int)pixels.Width, (int)pixels.Height);//Project
                this.Frame.Navigate(typeof(DrawPage), project);//Navigate    

                this.IsLoading = false;//Loading
            };

            await addDialog.ShowAsync(ContentDialogPlacement.InPlace);
        }

        private async void FolderDialogShow()
        {
        }


        #endregion


        private void NavigatedTo()
        {
        }
        private void NavigatedFrom(FrameworkElement element)
        {
            this.ViewModel.CanvasTransformer.Size = new Size(this.ActualWidth, this.ActualHeight - 50);

            //Transition
            this.ViewModel.Transition(0.0f);

            Point postion = Retouch_Photo2.Menus.MenuHelper.GetVisualPostion(element);
            float width = (float)element.ActualWidth;
            float height = (float)element.ActualHeight;
            this.ViewModel.TransitionSource(postion, width, height);
        }

    }
}