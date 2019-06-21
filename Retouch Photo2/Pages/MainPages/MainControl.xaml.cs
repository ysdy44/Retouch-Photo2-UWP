using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Pages.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "MainControl" />.
    /// </summary>
    public sealed partial class MainControl : Page
    {

        /// <summary> <see cref = "MainControl" />'s AddButton. </summary>
        public Windows.UI.Xaml.Controls.Button AddButton { get => this._AddButton.RootButton; set => this._AddButton.RootButton = value; }
        /// <summary> <see cref = "MainControl" />'s PicturesButton. </summary>
        public Windows.UI.Xaml.Controls.Button PicturesButton { get => this._PicturesButton.RootButton; set => this._PicturesButton.RootButton = value; }
     
        /// <summary> <see cref = "MainControl" />'s SaveButton. </summary>
        public Windows.UI.Xaml.Controls.Button SaveButton { get => this._SaveButton.RootButton; set => this._SaveButton.RootButton = value; }
        /// <summary> <see cref = "MainControl" />'s ShareButton. </summary>
        public Windows.UI.Xaml.Controls.Button ShareButton { get => this._ShareButton.RootButton; set => this._ShareButton.RootButton = value; }
     
        /// <summary> <see cref = "MainControl" />'s DeleteButton. </summary>
        public Windows.UI.Xaml.Controls.Button DeleteButton { get => this._DeleteButton.RootButton; set => this._DeleteButton.RootButton = value; }
        /// <summary> <see cref = "MainControl" />'s DuplicateButton. </summary>
        public Windows.UI.Xaml.Controls.Button DuplicateButton { get => this._DuplicateButton.RootButton; set => this._DuplicateButton.RootButton = value; }
     
        /// <summary> <see cref = "MainControl" />'s FolderButton. </summary>
        public Windows.UI.Xaml.Controls.Button FolderButton { get => this._FolderButton.RootButton; set => this._FolderButton.RootButton = value; }
        /// <summary> <see cref = "MainControl" />'s MoveButton. </summary>
        public Windows.UI.Xaml.Controls.Button MoveButton { get => this._MoveButton.RootButton; set => this._MoveButton.RootButton = value; }


        //Second
        /// <summary> <see cref = "MainControl" />'s SecondAddButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondAddButton { get => this._SecondAddButton.RootButton; set => this._SecondAddButton.RootButton = value; }
        /// <summary> <see cref = "MainControl" />'s SecondPicturesButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondPicturesButton { get => this._SecondPicturesButton.RootButton; set => this._SecondPicturesButton.RootButton = value; }

        /// <summary> <see cref = "MainControl" />'s SecondSaveButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondSaveButton { get => this._SecondSaveButton.RootButton; set => this._SecondSaveButton.RootButton = value; }
        /// <summary> <see cref = "MainControl" />'s SecondShareButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondShareButton { get => this._SecondShareButton.RootButton; set => this._SecondShareButton.RootButton = value; }

        /// <summary> <see cref = "MainControl" />'s SecondDeleteButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondDeleteButton { get => this._SecondDeleteButton.RootButton; set => this._SecondDeleteButton.RootButton = value; }
        /// <summary> <see cref = "MainControl" />'s SecondDuplicateButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondDuplicateButton { get => this._SecondDuplicateButton.RootButton; set => this._SecondDuplicateButton.RootButton = value; }

        /// <summary> <see cref = "MainControl" />'s SecondFolderButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondFolderButton { get => this._SecondFolderButton.RootButton; set => this._SecondFolderButton.RootButton = value; }
        /// <summary> <see cref = "MainControl" />'s SecondMoveButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondMoveButton { get => this._SecondMoveButton.RootButton; set => this._SecondMoveButton.RootButton = value; }


        public MainControl()
        {
            this.InitializeComponent();
             
            /*
            this._SaveButton.RootButton.IsEnabled = false;
            this._ShareButton.RootButton.IsEnabled = false;

            this._DeleteButton.RootButton.IsEnabled = false;
            this._DuplicateButton.RootButton.IsEnabled = false;

            this._FolderButton.RootButton.IsEnabled = false;
            this._MoveButton.RootButton.IsEnabled = false;
             */


            this._MoreButton.RootButton.Tapped += (s, e) => this.Flyout.ShowAt(this._MoreButton);


            //Second
            this._SecondAddButton.RootButton.Tapped += (s, e) => this.Flyout.Hide();
            this._SecondPicturesButton.RootButton.Tapped += (s, e) => this.Flyout.Hide();

            this._SecondSaveButton.RootButton.Tapped += (s, e) => this.Flyout.Hide();
            this._SecondShareButton.RootButton.Tapped += (s, e) => this.Flyout.Hide();

            this._SecondDeleteButton.RootButton.Tapped += (s, e) => this.Flyout.Hide();
            this._SecondDuplicateButton.RootButton.Tapped += (s, e) => this.Flyout.Hide();

            this._SecondFolderButton.RootButton.Tapped += (s, e) => this.Flyout.Hide();
            this._SecondMoveButton.RootButton.Tapped += (s, e) => this.Flyout.Hide();
        }
    }
}
