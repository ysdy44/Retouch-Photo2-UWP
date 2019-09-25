using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary> 
    /// Page of <see cref = "ImagePage"/>.
    /// </summary>
    public sealed partial class ImagePage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        
        //@Content
        public Storyboard EaseStoryboard => this._EaseStoryboard;
        
        //@Converter
        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }
        
        //@Construct
        public ImagePage()
        {
            this.InitializeComponent();
            this.ClearButton.Tapped += (s, e) => this.SelectionViewModel.ImageRe =null;//ImageRe
            this.SelectButton.Tapped += async (s, e) =>
            {
                //imageRe
                ImageRe imageRe = await ImageRe.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, PickerLocationId.PicturesLibrary);
                if (imageRe == null) return;
                
                //Images
                this.ViewModel.DuplicateChecking(imageRe);

                this.SelectionViewModel.ImageRe = imageRe;//ImageRe
            };
            this.ReplaceButton.Tapped += async (s, e) =>
            {
                //imageRe
                ImageRe imageRe = await ImageRe.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, PickerLocationId.PicturesLibrary);
                if (imageRe == null) return;
                
                //Images
                this.ViewModel.DuplicateChecking(imageRe);

                //Transformer
                Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is ImageLayer imageLayer)
                    {
                        imageLayer.ImageRe = imageRe;
                        imageLayer.TransformManager.Source = transformerSource;
                    }
                }, true);

                this.ViewModel.Invalidate();//Invalidate
            };
            this.StackButton.Tapped +=   (s, e) => 
            {
                this.ListView.ItemsSource = null;
                this.ListView.ItemsSource = this.ViewModel.Images;
                this.Flyout.ShowAt(this);//this.StackButton
            };
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ImageRe imageRe)
                {
                    this.SelectionViewModel.ImageRe = imageRe;//ImageRe
                }
            };
        }
    }
}