using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary>
    /// Mode of <see cref="ImageResPage"/>
    /// </summary>
    public enum ImageResPageMode
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
    /// Retouch_Photo2's the only <see cref = "ImageResPage" />. 
    /// </summary>
    public sealed partial class ImageResPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@VisualState
        ImageRe _vsImageRe = null;
        ImageResPageMode _vsMode = ImageResPageMode.None;

        public VisualState VisualState
        {
            get
            {
                switch (this._vsMode)
                {
                    case ImageResPageMode.None: return this.Normal;
                    case ImageResPageMode.AddImageLayer: return this.AddImageLayer;
                    case ImageResPageMode.BrushToolImage: return this.BrushToolImage;
                    case ImageResPageMode.ImageToolSelect: return this.ImageToolSelect;
                    case ImageResPageMode.ImageToolReplace: return this.ImageToolReplace;
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Construct
        public ImageResPage()
        {
            this.InitializeComponent();

            this.GridView.ItemsSource = ImageRe.Instances;
            this.GridView.ItemClick += async (s, e) =>
            {
                if (e.ClickedItem is ImageRe imageRe)
                {
                    if (this._vsImageRe == imageRe)
                    {
                        this._vsImageRe = null;
                        this.RadiusAnimaPanel.Visibility = Visibility.Collapsed;
                        this.GridView.SelectionMode = ListViewSelectionMode.None;
                        await Task.Delay(100);
                        this.GridView.SelectionMode = ListViewSelectionMode.Single;
                    }
                    else
                    {
                        this._vsImageRe = imageRe;
                        this.TextBlock.Text = $"{imageRe.Name}{imageRe.FileType}";
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
            if (e.Parameter is ImageResPageMode mode)
            {
                this._vsMode = mode;
                this.VisualState = this.VisualState;//State

                if (ImageRe.Instances.Count == 0)
                {
                    await this.Pick();
                }
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this._vsMode = ImageResPageMode.None;
        }

    }
}