using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using System;
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

        /// <summary> Make <see cref="Retouch_Photo2.Styles.Style.Fill"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        FillImage,
        /// <summary> Make <see cref="Retouch_Photo2.Styles.Style.Stroke"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        StrokeImage,

        /// <summary> Select a image in <see cref= "ImageTool" />. </summary>
        SelectImage,
        /// <summary> Replace a image in <see cref= "ImageTool" />. </summary>
        ReplaceImage
    }

    /// <summary> 
    /// Represents a page to select a photo.
    /// </summary>
    public sealed partial class PhotosPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Static       
        /// <summary> Add a <see cref="ImageLayer"/>. </summary>
        public static Action<Photo> AddCallBack;

        /// <summary> Make <see cref="Retouch_Photo2.Styles.Style.Fill"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        public static Action<Photo> FillImageCallBack;
        /// <summary> Make <see cref="Retouch_Photo2.Styles.Style.Stroke"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        public static Action<Photo> StrokeImageCallBack;

        /// <summary> Select a image in <see cref= "ImageTool" />. </summary>
        public static Action<Photo> SelectCallBack;
        /// <summary> Replace a image in <see cref= "ImageTool" />. </summary>
        public static Action<Photo> ReplaceCallBack;


        //@VisualState
        int _vsPhotosCount = 0;
        Photo _vsPhoto = null;
        PhotosPageMode _vsMode = PhotosPageMode.None;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsPhotosCount ==0) return this.ZeroPhotos;

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
        /// <summary>
        /// Initializes a PhotosPage. 
        /// </summary>
        public PhotosPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructGridView();
            this.ConstructDragAndDrop();


            this.BackButton.Click += (s, e) => this.Frame.GoBack();
            this.AddButton.Click += async (s, e) => await this.PickAndCopySingleImageFileAsync();
            this.ZeroAddButton.Click += async (s, e) =>
            {
                await this.PickAndCopySingleImageFileAsync();

                this._vsPhotosCount = Photo.Instances.Count;
                this.VisualState = this.VisualState;//State
            };


            this.AddImageLayerButton.Click += (s, e) => this.ButtonClick(PhotosPageMode.AddImager);

            this.FillImageButton.Click += (s, e) => this.ButtonClick(PhotosPageMode.FillImage);
            this.StrokeImageButton.Click += (s, e) => this.ButtonClick(PhotosPageMode.StrokeImage);

            this.SelectImageButton.Click += (s, e) => this.ButtonClick(PhotosPageMode.SelectImage);
            this.ReplaceImageButton.Click += (s, e) => this.ButtonClick(PhotosPageMode.ReplaceImage);
        }

        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Extension
            this.AVTBBE.Invalidate();

            if (e.Parameter is PhotosPageMode mode)
            {                
                this._vsPhotosCount = Photo.Instances.Count;
                this._vsMode = mode;
                this.VisualState = this.VisualState;//State
            }
        }
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this._vsMode = PhotosPageMode.None;
        }
    }
}