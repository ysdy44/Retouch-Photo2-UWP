using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary> 
    /// Page of <see cref = "ImagePage"/>.
    /// </summary>
    public sealed partial class ImagePage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        /// <summary> Tip. </summary>
        public void TipSelect() => this.EaseStoryboard.Begin();//Storyboard

        //@Construct
        public ImagePage()
        {
            this.InitializeComponent();

            this.ClearButton.Tapped += (s, e) => this.SelectionViewModel.ImageStr = new ImageStr();//ImageRe
            this.SelectButton.Tapped += async (s, e) =>
            {
                //imageRe
                ImageRe imageRe = await FileUtil.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, PickerLocationId.PicturesLibrary);
                if (imageRe == null) return;

                //Images
                ImageRe.DuplicateChecking(imageRe);

                this.SelectionViewModel.ImageStr = imageRe.ToImageStr();//ImageRe
            };

            this.ReplaceButton.Tapped += async (s, e) =>
            {
                //imageRe
                ImageRe imageRe = await FileUtil.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, PickerLocationId.PicturesLibrary);
                if (imageRe == null) return;

                //Images
                ImageRe.DuplicateChecking(imageRe);

                //Transformer
                Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is ImageLayer imageLayer)
                    {
                        imageLayer.TransformManager.Source = transformerSource;
                        imageLayer.StyleManager.FillBrush.ImageStr = imageRe.ToImageStr();
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.StackButton.Tapped += (s, e) =>
            {
                this.ListView.ItemsSource = null;
                this.ListView.ItemsSource = ImageRe.Instances;
                this.Flyout.ShowAt(this);//this.StackButton
            };

            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ImageRe imageRe)
                {
                    this.SelectionViewModel.ImageStr = imageRe.ToImageStr();//ImageRe
                }
            };
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}