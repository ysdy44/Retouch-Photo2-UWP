using System;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary> 
    /// Item of <see cref="MainPage"/>. 
    /// </summary>
    public sealed partial class ProjectViewItem : UserControl
    {

        //@Static
        /// <summary> Occurs when tapped the project-control. </summary>
        public static Action<ProjectViewItem> ItemClick;


        //@VisualState
        SelectMode _vsSelectMode = SelectMode.None;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsSelectMode)
                {
                    case SelectMode.UnSelected: return this.UnSelected;
                    case SelectMode.Selected: return this.Selected;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        /// <summary>
        /// Gets or sets the select-mode.
        /// </summary>
        public SelectMode SelectMode
        {
            get => this._vsSelectMode;
            set
            {
                this._vsSelectMode = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@Content
        /// <summary> ImageEx. </summary>
        public FrameworkElement ImageEx => this._ImageEx;
        /// <summary> Gets the name. </summary>
        //public string Name { get; private set; }
        /// <summary> Gets the zip file path. </summary>
        public string Photo2pkFilePath { get; private set; }
        /// <summary> Gets the thumbnail path. </summary>
        public string ThumbnailPath { get; private set; }


        //@Construct
        /// <summary>
        /// Construct a ProjectControl from <see cref = "StorageFile" />.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="zipFile"> The zip file name. </param>
        /// <param name="thumbnail"> The thumbnail name. </param>
        public ProjectViewItem(string name, string zipFile, string thumbnail)
        {
            this.InitializeComponent();

            //Content    
            this.Name = name;
            this.Photo2pkFilePath = zipFile;
            this.ThumbnailPath = thumbnail;

            this._ImageEx.Source = new Uri(thumbnail, UriKind.Relative);

            //Static
            this._RootGrid.Tapped += (s, e) => ProjectViewItem.ItemClick?.Invoke(this);//Delegate
        }

        /// <summary>
        /// Rename image source.
        /// </summary>
        public void Rename(string name, string zipFile, string thumbnail)
        {
            this.Name = name;
            this.Photo2pkFilePath = zipFile;
            this.ThumbnailPath = thumbnail;

            this._ImageEx.Source = new Uri(thumbnail, UriKind.Relative);
        }

        /// <summary>
        /// Switch the state.
        /// </summary>
        public void SwitchState()
        {
            switch (this.SelectMode)
            {
                case SelectMode.UnSelected:
                    this.SelectMode = SelectMode.Selected;
                    break;
                case SelectMode.Selected:
                    this.SelectMode = SelectMode.UnSelected;
                    break;
            }
        }

        /// <summary>
        /// Refresh image source.
        /// </summary>
        public void RefreshImageSource()
        {
            object url = this._ImageEx.Source;
            this._ImageEx.Source = null;
            this._ImageEx.Source = url;
        }


        /// <summary>
        /// Get the position and size of the image element relative to the visual element. 
        /// </summary>
        /// <returns> The calculated rect. </returns>
        public Rect GetVisualRect(UIElement visual)
        {
            GeneralTransform transform = this.ImageEx.TransformToVisual(visual);
            Point sourcePostion = transform.TransformPoint(new Point());

            return new Rect
            {
                X= sourcePostion.X,
                Y= sourcePostion.Y,
                Width= this.ImageEx.ActualWidth,
                Height= this.ImageEx.ActualHeight
            };
        }

    }
}