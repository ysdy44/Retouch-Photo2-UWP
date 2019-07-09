using Microsoft.Graphics.Canvas;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
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
        private string ImageSummaryStoryboardConverter(string imageSummary)
        {
            if (imageSummary == string.Empty)
            {
                this.EaseStoryboard.Begin();//Storyboard
            }

            return imageSummary;
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

                //ImageKey
                string imageKey = file.Name;
                this.SelectionViewModel.ImageKey = imageKey;

                //Bitmap
                CanvasBitmap bitmap = await this.ViewModel.GetCanvasBitmap(file);
                if (bitmap == null) return;

                //Images
                this.ViewModel.Images.Add(imageKey, bitmap);

                this.SelectionViewModel.ImageSummary = string.Format
                (
                    "{0} {1}x{2}pixels {3}Dpi",
                    imageKey,
                    bitmap.SizeInPixels.Width,
                    bitmap.SizeInPixels.Height,
                    bitmap.Dpi
                );
            };
        }
    }
}