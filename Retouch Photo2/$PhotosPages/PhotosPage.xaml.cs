// Core:              ★★★★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★★★★
// Complete:      ★★★
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Photos;
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
        AddImage,

        /// <summary> Make <see cref="Retouch_Photo2.Styles.IStyle.Fill"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        FillImage,
        /// <summary> Make <see cref="Retouch_Photo2.Styles.IStyle.Stroke"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
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


        //@Static       
        /// <summary> Add a <see cref="ImageLayer"/>. </summary>
        public static Action<Photo> AddImageCallBack;

        /// <summary> Make <see cref="Retouch_Photo2.Styles.IStyle.Fill"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        public static Action<Photo> FillImageCallBack;
        /// <summary> Make <see cref="Retouch_Photo2.Styles.IStyle.Stroke"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        public static Action<Photo> StrokeImageCallBack;

        /// <summary> Select a image in <see cref= "ImageTool" />. </summary>
        public static Action<Photo> SelectImageCallBack;
        /// <summary> Replace a image in <see cref= "ImageTool" />. </summary>
        public static Action<Photo> ReplaceImageCallBack;


        //@VisualState
        int _vsPhotosCount = 0;
        PhotosPageMode _vsMode = PhotosPageMode.None;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsPhotosCount ==0) return this.ZeroPhotos;
                             
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, true);
        }

        
        //@Construct
        /// <summary>
        /// Initializes a PhotosPage. 
        /// </summary>
        public PhotosPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructDragAndDrop();
            this.GridView.ItemsSource = Photo.Instances;


            this.BackButton.Click += (s, e) => this.Frame.GoBack();
            this.AddButton.Click += async (s, e) => await this.PickAndCopySingleImageFileAsync();
            this.ZeroAddButton.Click += async (s, e) =>
            {
                await this.PickAndCopySingleImageFileAsync();

                this._vsPhotosCount = Photo.Instances.Count;
                this.VisualState = this.VisualState;//State
            };


            #region Photo

            Photo.ItemClick += (sender, photo) =>
            {
                this.ButtonClick(photo, this._vsMode);
            };

            Photo.FlyoutShow += (sender, photo) =>
            {
                FrameworkElement element = (FrameworkElement)sender;
                this.Billboard.CalculatePostion(element);
                this.Billboard.Photo = photo;

                this.Billboard.IsShow = this.BillboardCanvas.IsHitTestVisible = true;
            };
            this.BillboardCanvas.Tapped += (s, e) =>
            {
                this.Billboard.IsShow = this.BillboardCanvas.IsHitTestVisible = false;
            };

            #endregion
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