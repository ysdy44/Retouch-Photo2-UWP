using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Pages.MainPages;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Retouch_Photo2.Pages
{
    /// <summary> 
    /// State of <see cref="MainPage"/>. 
    /// </summary>
    public enum MainPageState
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Main. </summary>
        Main,
        /// <summary> Loading. </summary>
        //Loading,

        /// <summary> Add a blank project. </summary>
        //Add,
        /// <summary> Add a pictures project. </summary>
        Pictures,

        /// <summary> Save project(s). </summary>
        Save,
        /// <summary> Share project(s). </summary>
        Share,

        /// <summary> Delete project(s). </summary>
        Delete,
        /// <summary> Duplicate project(s). </summary>
        Duplicate,

        /// <summary> Create a new Folder. </summary>
        //Folder,
        /// <summary> Move a project into a folder. </summary>
        Move,
    }


    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainPage" />. 
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        ObservableCollection<Photo> PhotoFileList = new ObservableCollection<Photo>();
        

        //Add
        private async void AddShow() => await this.AddDialog.ShowAsync(ContentDialogPlacement.InPlace);
        private void AddHide() => this.AddDialog.Hide();

        private async void FolderShow() => await this.FolderDialog.ShowAsync(ContentDialogPlacement.InPlace);
        private void FolderHide() => this.FolderDialog.Hide();
        

        //Loading
        private bool IsLoading { set => this.LoadingControl.IsActive = value; }
        /// <summary> State of <see cref="MainPage"/>. </summary>
        public MainPageState State
        {
            get => this.state;
            set
            {
                switch (value)
                {
                    case MainPageState.None:
                        this.RadiusAnimaPanel.CenterContent = null;
                        break;

                    case MainPageState.Main:
                        this.RadiusAnimaPanel.CenterContent = this.MainControl;
                        break;
                    //case MainPageState.Loading:
                    //break;

                    //case MainPageState.Add:
                    //break;
                    case MainPageState.Pictures:
                        this.RadiusAnimaPanel.CenterContent = this.PicturesControl;
                        break;

                    case MainPageState.Save:
                        this.RadiusAnimaPanel.CenterContent = this.SaveControl;
                        break;
                    case MainPageState.Share:
                        this.RadiusAnimaPanel.CenterContent = this.ShareControl;
                        break;

                    case MainPageState.Delete:
                        this.RadiusAnimaPanel.CenterContent = this.DeleteControl;
                        break;
                    case MainPageState.Duplicate:
                        this.RadiusAnimaPanel.CenterContent = this.DuplicateControl;
                        break;

                    //case MainPageState.Folder:
                    //break;
                    //case MainPageState.Move:
                    //break;

                    default:
                        break;
                }

                this.state = value;
            }
        }
        private MainPageState state;
        

        MainControl MainControl = new MainControl();

        AddDialog AddDialog = new AddDialog();
        PicturesControl PicturesControl = new PicturesControl();

        SaveControl SaveControl = new SaveControl();
        ShareControl ShareControl = new ShareControl();

        DeleteControl DeleteControl = new DeleteControl();
        DuplicateControl DuplicateControl = new DuplicateControl();

        FolderDialog FolderDialog = new FolderDialog();


        //@Construct
        public MainPage()
        {
            this.InitializeComponent();
            this.State = MainPageState.Main;//State   

            this.ViewModel.CanvasTheme = (App.Current.RequestedTheme == ApplicationTheme.Dark) ? ElementTheme.Dark : ElementTheme.Light;
            this.ThemeControl.Loaded += (s, e) => this.ThemeControl.ApplicationTheme = App.Current.RequestedTheme;

            this.Loaded += (s, e) =>
            {
                Project project = new Project(1024, 1024);//Project
                this.Frame.Navigate(typeof(DrawPage), project);//Navigate    
            };

            this.GridView.ItemClick += (s, e) =>
            {
                if (this.State != MainPageState.None) return;

                if (e.ClickedItem is Photo photo)
                {
                    this.IsLoading = true;//Loading

                    Project project = new Project(1024, 1024);
                    this.Frame.Navigate(typeof(DrawPage), project);//Navigate     

                    this.IsLoading = false;//Loading
                }
            };

            //Main
            {
                this.MainControl.AddButton.Tapped += (s, e) => this.AddShow();
                this.MainControl.PicturesButton.Tapped += (s, e) => this.State = MainPageState.Pictures;

                this.MainControl.SaveButton.Tapped += (s, e) => this.State = MainPageState.Save;
                this.MainControl.ShareButton.Tapped += (s, e) => this.State = MainPageState.Share;

                this.MainControl.DeleteButton.Tapped += (s, e) => this.State = MainPageState.Delete;
                this.MainControl.DuplicateButton.Tapped += (s, e) => this.State = MainPageState.Duplicate;

                this.MainControl.FolderButton.Tapped += (s, e) => this.FolderShow();
                this.MainControl.MoveButton.Tapped += (s, e) => this.State = MainPageState.Move;
            }

            //Second
            {
                this.MainControl.SecondAddButton.Tapped += (s, e) => this.AddShow();
                this.MainControl.SecondPicturesButton.Tapped += (s, e) => this.State = MainPageState.Pictures;

                this.MainControl.SecondSaveButton.Tapped += (s, e) => this.State = MainPageState.Save;
                this.MainControl.SecondShareButton.Tapped += (s, e) => this.State = MainPageState.Share;

                this.MainControl.SecondDeleteButton.Tapped += (s, e) => this.State = MainPageState.Delete;
                this.MainControl.SecondDuplicateButton.Tapped += (s, e) => this.State = MainPageState.Duplicate;

                this.MainControl.SecondFolderButton.Tapped += (s, e) => this.FolderShow();
                this.MainControl.SecondMoveButton.Tapped += (s, e) => this.State = MainPageState.Move;
            }

            //Add
            {
                this.AddDialog.SecondaryButtonClick += (sender, args) => this.AddHide();

                this.AddDialog.PrimaryButtonClick += (sender, args) =>
                {
                    this.AddHide();

                    this.IsLoading = true;//Loading

                    BitmapSize pixels = this.AddDialog.Size;
                    Project project = new Project((int)pixels.Width, (int)pixels.Height);//Project
                    this.Frame.Navigate(typeof(DrawPage), project);//Navigate    

                    this.IsLoading = false;//Loading
                };
            }

            //Pictures
            {
                this.PicturesControl.PhotoButton.Tapped += async (s, e) =>
                {
                    //File
                    StorageFile file = await this.ViewModel.PickSingleFileAsync(PickerLocationId.PicturesLibrary);
                    if (file == null) return;

                    //ImageKey
                    string imageKey = file.Name;

                    //CanvasBitmap
                    CanvasBitmap bitmap = await this.ViewModel.GetCanvasBitmap(file);
                    if (bitmap == null) return;

                    //Layer
                    ImageLayer imageLayer = new ImageLayer(imageKey, this.ViewModel.GetImage);
                    if (imageLayer == null) return;

                    //Project
                    Project project = new Project(imageLayer);
                    this.Frame.Navigate(typeof(DrawPage), project);//Navigate       
                };

                this.PicturesControl.DestopButton.Tapped += async (s, e) =>
                {
                    //File
                    StorageFile file = await this.ViewModel.PickSingleFileAsync(PickerLocationId.Desktop);
                    if (file == null) return;

                    //ImageKey
                    string imageKey = file.Name;

                    //CanvasBitmap
                    CanvasBitmap bitmap = await this.ViewModel.GetCanvasBitmap(file);
                    if (bitmap == null) return;

                    //Layer
                    ImageLayer imageLayer = new ImageLayer(imageKey, this.ViewModel.GetImage);
                    if (imageLayer == null) return;

                    //Project
                    Project project = new Project(imageLayer);
                    this.Frame.Navigate(typeof(DrawPage), project);//Navigate
                };

                this.PicturesControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;
            }

            //Save
            {
                this.SaveControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;
            }

            //Share
            {
                this.ShareControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;
            }

            //Delete
            {
                this.DeleteControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;
            }

            //Duplicate
            {
                this.DuplicateControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;
            }

            //Folder
            {
                this.FolderDialog.SecondaryButtonClick += (sender, args) => this.FolderHide();

                this.FolderDialog.PrimaryButtonClick += (sender, args) =>
                {
                    this.FolderHide();

                    this.IsLoading = true;//Loading

                    //......

                    this.IsLoading = false;//Loading
                };
            }
        }

        private async void Refresh()
        {
            this.PhotoFileList.Clear(); //Notify

            // Get files from the destination folder.
            IOrderedEnumerable<StorageFile> storageFiles = await Photo.CreatePhotoFilesFromStorageFolder(ApplicationData.Current.LocalFolder).ConfigureAwait(false);

            foreach (StorageFile storageFile in storageFiles)
            {
                // [StorageFile] --> [Photo]
                Photo photo = Photo.CreatePhoto(storageFile, ApplicationData.Current.LocalFolder.Path);

                if (photo == null) break;

                this.PhotoFileList.Add(photo); //Notify
            }
        }
    }
}