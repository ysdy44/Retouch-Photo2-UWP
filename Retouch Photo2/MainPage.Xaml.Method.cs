using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
using Retouch_Photo2.Brushs;

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
            //Transition
            this.ViewModel.IsTransition = false;
            this.ViewModel.CanvasTransformer.Transition(0.0f);

            Project project = new Project((int)pixels.Width, (int)pixels.Height);//Project
            this.ViewModel.LoadFromProject(project);

            this.Frame.Navigate(typeof(DrawPage));//Navigate   
        }
        private async void NewProjectFromPhoto(object sender, Photo photo)
        {
            if (this.State != MainPageState.Main) return;

            if (sender is FrameworkElement element)
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
                     


        private void ConstructAddDialog()
        {
            this.AddDialog.CloseButton.Click += (sender, args) =>
            {
                this.AddDialog.Hide();
            };
            this.AddDialog.PrimaryButton.Click += (sender, args) =>
            {
                this.AddDialog.Hide();

                BitmapSize size = this.AddSizePicker.Size;

                this.NewProjectFromSize(size);
            };
        }

        private void ShowAddDialog()
        {
            this.AddDialog.Show();
        }
        

        private void ConstructFolderDialog()
        {
        }

        private void ShowFolderDialog()
        {
        }



        private async void NavigatedTo()
        {
            await this.Refresh();
        }
        private void NavigatedFrom(FrameworkElement element)
        {
        }

    }
}