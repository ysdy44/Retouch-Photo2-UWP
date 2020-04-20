using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
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

        /// <summary> Add a ImageLayer. </summary>
        AddImageLayer,

        /// <summary> To ImageBrush in BrushTool. </summary>
        BrushToolImage,

        /// <summary> Select in ImageTool. </summary>
        ImageToolSelect,
        /// <summary> Replace in ImageTool. </summary>
        ImageToolReplace
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "PhotosPage" />. 
    /// </summary>
    public sealed partial class PhotosPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


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
                    case PhotosPageMode.AddImageLayer: return this.AddImageLayer;
                    case PhotosPageMode.BrushToolImage: return this.BrushToolImage;
                    case PhotosPageMode.ImageToolSelect: return this.ImageToolSelect;
                    case PhotosPageMode.ImageToolReplace: return this.ImageToolReplace;
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

            this.BackButton.Tapped += (s, e) => this.Frame.GoBack();
            this.AddButton.Tapped += async (s, e) => await this.Pick();


            this.AddImageLayerButton.Tapped += (s, e) => this.Add();
            this.BrushToolImageButton.Tapped += (s, e) => this.Image();
            this.ImageToolSelectButton.Tapped += (s, e) => this.Select();
            this.ImageToolReplaceButton.Tapped += (s, e) => this.Replace();
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


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TitleTextBlock.Text = resource.GetString("/$PhotosPage/Title");

            this.AddImageLayerButton.Content = resource.GetString("/$PhotosPage/AddImageLayer");
            this.BrushToolImageButton.Content = resource.GetString("/$PhotosPage/BrushToolImage");
            this.ImageToolSelectButton.Content = resource.GetString("/$PhotosPage/ImageToolSelect");
            this.ImageToolReplaceButton.Content = resource.GetString("/$PhotosPage/ImageToolReplace");
        }

    }
}