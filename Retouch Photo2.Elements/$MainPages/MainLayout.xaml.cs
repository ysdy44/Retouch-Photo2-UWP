// Core:              ★★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★★★
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// <see cref = "MainPage" />'s layout. 
    /// </summary>
    public sealed partial class MainLayout : UserControl
    {

        //@Content     
        /// <summary> GridView. </summary>
        public GridView GridView => this._GridView;
        /// <summary> SelectCheckBox. </summary>
        public CheckBox SelectCheckBox => this._SelectCheckBox;


        /// <summary> InitialBorder's Child. </summary>
        public UIElement InitialChild { get => this.InitialBorder.Child; set => this.InitialBorder.Child = value; }

        /// <summary> HeadBorder's Child. </summary>
        public UIElement HeadChild { get => this.HeadBorder.Child; set => this.HeadBorder.Child = value; }
        /// <summary> SelectBorder's Child. </summary>
        public UIElement SelectChild { get => this.SelectBorder.Child; set => this.SelectBorder.Child = value; }

        /// <summary> MainBorder's Child. </summary>
        public UIElement MainChild { get => this.MainBorder.Child; set => this.MainBorder.Child = value; }
        /// <summary> PicturesBorder's Child. </summary>
        public UIElement PicturesChild { get => this.PicturesBorder.Child; set => this.PicturesBorder.Child = value; }
        /// <summary> RenameBorder's Child. </summary>
        public UIElement RenameChild { get => this.RenameBorder.Child; set => this.RenameBorder.Child = value; }
        /// <summary> SaveBorder's Child. </summary>
        public UIElement SaveChild { get => this.SaveBorder.Child; set => this.SaveBorder.Child = value; }
        /// <summary> ShareBorder's Child. </summary>
        public UIElement ShareChild { get => this.ShareBorder.Child; set => this.ShareBorder.Child = value; }
        /// <summary> DeleteBorder's Child. </summary>
        public UIElement DeleteChild { get => this.DeleteBorder.Child; set => this.DeleteBorder.Child = value; }
        /// <summary> DuplicateBorder's Child. </summary>
        public UIElement DuplicateChild { get => this.DuplicateBorder.Child; set => this.DuplicateBorder.Child = value; }



        //@Construct
        /// <summary>
        /// Initializes a MainLayout.
        /// </summary>
        public MainLayout()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
        }

    }
}