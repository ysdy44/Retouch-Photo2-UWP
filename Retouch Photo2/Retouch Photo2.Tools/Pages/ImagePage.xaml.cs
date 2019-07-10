using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Numerics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary> 
    /// Page of <see cref = "ImagePage"/>.
    /// </summary>
    public sealed partial class ImagePage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Converter
        private string ImageReStoryboardConverter(ImageRe imageRe)
        {
            if (imageRe == null) return null;

            if (imageRe.IsStoryboardNotify == true)
            {
                this.EaseStoryboard.Begin();//Storyboard
                return null;
            }

            return imageRe.ToString();
        }
        private bool ImageReToIsEnabledConverter(ImageRe imageRe)
        {
            if (imageRe == null) return false;
            if (imageRe.IsStoryboardNotify == true) return false;

            return true;
        }


        //@Construct
        public ImagePage()
        {
            this.InitializeComponent();
            this.SelectButton.Tapped += async (s, e) =>
            {
                //File
                StorageFile file = await this.ViewModel.PickSingleFileAsync(PickerLocationId.PicturesLibrary);
                if (file == null) return;

                //imageRe
                ImageRe imageRe = await ImageRe.CreateFromStorageFile(this.ViewModel.CanvasDevice, file);
                if (imageRe == null) return;

                //Contains
                bool isContains = this.ViewModel.ContainsImage(imageRe.Key);
                if (isContains) imageRe = this.ViewModel.GetImage(imageRe.Key);

                this.ViewModel.Images.Push(imageRe);//Images

                this.SelectionViewModel.ImageRe = imageRe;//ImageRe
            };
            this.ReplaceButton.Tapped += async (s, e) =>
            {
                //File
                StorageFile file = await this.ViewModel.PickSingleFileAsync(PickerLocationId.PicturesLibrary);
                if (file == null) return;

                //imageRe
                ImageRe imageRe = await ImageRe.CreateFromStorageFile(this.ViewModel.CanvasDevice, file);
                if (imageRe == null) return;

                //Contains
                bool isContains = this.ViewModel.ContainsImage(imageRe.Key);
                if (isContains) imageRe = this.ViewModel.GetImage(imageRe.Key);

                //Transformer
                Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is ImageLayer imageLayer)
                    {
                        imageLayer.ImageRe = imageRe;
                        imageLayer.Source = transformerSource;
                    }
                }, true);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }
}