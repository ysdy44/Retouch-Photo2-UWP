using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary>
    /// Mode of <see cref="PhotosPage"/>
    /// </summary>
    public enum PhotosPageMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Add a <see cref="ImageLayer"/>. </summary>
        AddImager,

        /// <summary> Make <see cref="Brushs.Style.Fill"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        FillImage,
        /// <summary> Make <see cref="Brushs.Style.Stroke"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        StrokeImage,

        /// <summary> Select a image in <see cref= "ImageTool" />. </summary>
        SelectImage,
        /// <summary> Replace a image in <see cref= "ImageTool" />. </summary>
        ReplaceImage
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "PhotosPage" />. 
    /// </summary>
    public sealed partial class PhotosPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Static
        public static Action<Photo> AddCallBack;

        public static Action<Photo> FillImageCallBack;
        public static Action<Photo> StrokeImageCallBack;

        public static Action<Photo> SelectCallBack;
        public static Action<Photo> ReplaceCallBack;


        //@VisualState
        Photo _vsPhoto = null;
        PhotosPageMode _vsMode = PhotosPageMode.None;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsMode)
                {
                    case PhotosPageMode.None: return this.Normal;

                    case PhotosPageMode.AddImager: return this.AddImageLayer;

                    case PhotosPageMode.FillImage: return this.FillImage;
                    case PhotosPageMode.StrokeImage: return this.StrokeImage;

                    case PhotosPageMode.SelectImage: return this.SelectImage;
                    case PhotosPageMode.ReplaceImage: return this.ReplaceImage;
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        public PhotosPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructGridView();

            this.BackButton.Tapped += (s, e) => this.Frame.GoBack();
            this.AddButton.Tapped += async (s, e) => await this.Pick();


            this.AddImageLayerButton.Tapped += (s, e) =>
            {
                //Photo
                Photo photo = this._vsPhoto;
                Retouch_Photo2.PhotosPage.AddCallBack?.Invoke(photo);

                this.Frame.GoBack();
            };

            this.FillImageButton.Tapped += (s, e) =>
            {
                //Photo
                Photo photo = this._vsPhoto;
                Retouch_Photo2.PhotosPage.FillImageCallBack?.Invoke(photo);

                this.Frame.GoBack();
            };
            this.StrokeImageButton.Tapped += (s, e) =>
            {
                //Photo
                Photo photo = this._vsPhoto;
                Retouch_Photo2.PhotosPage.StrokeImageCallBack?.Invoke(photo);

                this.Frame.GoBack();
            };

            this.SelectImageButton.Tapped += (s, e) =>
            {
                //Photo
                Photo photo = this._vsPhoto;
                Retouch_Photo2.PhotosPage.SelectCallBack?.Invoke(photo);

                this.Frame.GoBack();
            };
            this.ReplaceImageButton.Tapped += (s, e) =>
            {
                //Photo
                Photo photo = this._vsPhoto;
                Retouch_Photo2.PhotosPage.ReplaceCallBack?.Invoke(photo);

                this.Frame.GoBack();
            };
        }

        //The current page becomes the active page
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PhotosPageMode mode)
            {
                this._vsMode = mode;
                this.VisualState = this.VisualState;//State

                if (Photo.Instances.Count == 0)
                {
                    await this.Pick();
                }
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this._vsMode = PhotosPageMode.None;
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "PhotosPage" />. 
    /// </summary>
    public sealed partial class PhotosPage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TitleTextBlock.Text = resource.GetString("/$PhotosPage/Title");

            this.AddImageLayerButton.Content = resource.GetString("/$PhotosPage/AddImage");

            this.FillImageButton.Content = resource.GetString("/$PhotosPage/FillImage");
            this.StrokeImageButton.Content = resource.GetString("/$PhotosPage/StrokeImage");

            this.SelectImageButton.Content = resource.GetString("/$PhotosPage/SelectImage");
            this.ReplaceImageButton.Content = resource.GetString("/$PhotosPage/ReplaceImage");
        }

        
        private void ConstructGridView()
        {
            this.GridView.ItemsSource = Photo.Instances;
            this.GridView.ItemClick += async (s, e) =>
            {
                if (e.ClickedItem is Photo photo)
                {
                    if (this._vsPhoto == photo)
                    {
                        this._vsPhoto = null;
                        this.RadiusAnimaPanel.Visibility = Visibility.Collapsed;
                        this.GridView.SelectionMode = ListViewSelectionMode.None;
                        await Task.Delay(100);
                        this.GridView.SelectionMode = ListViewSelectionMode.Single;
                    }
                    else
                    {
                        this._vsPhoto = photo;
                        this.TextBlock.Text = $"{photo.Name}{photo.FileType}";
                        this.RadiusAnimaPanel.Visibility = Visibility.Visible;
                    }
                }
            };
        }


        private async Task Pick()
        {
            //Photo
            StorageFile copyFile = await FileUtil.PickAndCopySingleImageFileAsync(PickerLocationId.Desktop);
            if (copyFile == null) return;
            Photo photo = await Photo.CreatePhotoFromCopyFileAsync(this.ViewModel.CanvasDevice, copyFile);
            Photo.DuplicateChecking(photo);
        }

    }
}