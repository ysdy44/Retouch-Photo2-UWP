using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "MainControl" />.
    /// </summary>
    public sealed partial class MainControl : Page
    {

        //@Content
        /// <summary> <see cref = "MainControl" />'s AddButton. </summary>
        public Windows.UI.Xaml.Controls.Button AddButton   => this._AddButton.RootButton;  
        /// <summary> <see cref = "MainControl" />'s PicturesButton. </summary>
        public Windows.UI.Xaml.Controls.Button PicturesButton   => this._PicturesButton.RootButton;  
     
        /// <summary> <see cref = "MainControl" />'s SaveButton. </summary>
        public Windows.UI.Xaml.Controls.Button SaveButton   => this._SaveButton.RootButton;  
        /// <summary> <see cref = "MainControl" />'s ShareButton. </summary>
        public Windows.UI.Xaml.Controls.Button ShareButton   => this._ShareButton.RootButton;  
     
        /// <summary> <see cref = "MainControl" />'s DeleteButton. </summary>
        public Windows.UI.Xaml.Controls.Button DeleteButton   => this._DeleteButton.RootButton;  
        /// <summary> <see cref = "MainControl" />'s DuplicateButton. </summary>
        public Windows.UI.Xaml.Controls.Button DuplicateButton   => this._DuplicateButton.RootButton;  
     
        /// <summary> <see cref = "MainControl" />'s FolderButton. </summary>
        public Windows.UI.Xaml.Controls.Button FolderButton   => this._FolderButton.RootButton;  
        /// <summary> <see cref = "MainControl" />'s MoveButton. </summary>
        public Windows.UI.Xaml.Controls.Button MoveButton   => this._MoveButton.RootButton; 


        //Second
        /// <summary> <see cref = "MainControl" />'s SecondAddButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondAddButton   => this._SecondAddButton.RootButton;  
        /// <summary> <see cref = "MainControl" />'s SecondPicturesButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondPicturesButton   => this._SecondPicturesButton.RootButton;  

        /// <summary> <see cref = "MainControl" />'s SecondSaveButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondSaveButton   => this._SecondSaveButton.RootButton;  
        /// <summary> <see cref = "MainControl" />'s SecondShareButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondShareButton   => this._SecondShareButton.RootButton;  

        /// <summary> <see cref = "MainControl" />'s SecondDeleteButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondDeleteButton   => this._SecondDeleteButton.RootButton;  
        /// <summary> <see cref = "MainControl" />'s SecondDuplicateButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondDuplicateButton   => this._SecondDuplicateButton.RootButton;  

        /// <summary> <see cref = "MainControl" />'s SecondFolderButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondFolderButton   => this._SecondFolderButton.RootButton;  
        /// <summary> <see cref = "MainControl" />'s SecondMoveButton. </summary>
        public Windows.UI.Xaml.Controls.Button SecondMoveButton   => this._SecondMoveButton.RootButton;


        //@Construct
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
