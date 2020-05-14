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


        //@Construct
        /// <summary>
        /// Initializes a ProjectViewItem from <see cref = "StorageFolder" />.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="thumbnail"> The thumbnail path. </param>
        public ProjectViewItem(string name, string thumbnail)
        {
            this.InitializeComponent();

            this.Name = name;
            this.ImageEx.Source = new Uri(thumbnail, UriKind.Relative);

            this._RootGrid.Tapped += (s, e) => ProjectViewItem.ItemClick?.Invoke(this);//Delegate
        }

        /// <summary>
        /// Rename.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="thumbnail"> The thumbnail path. </param>
        public void Rename(string name,  string thumbnail)
        {
            this.Name = name;
            this.ImageEx.Source = new Uri(thumbnail, UriKind.Relative);
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
            object url = this.ImageEx.Source;
            this.ImageEx.Source = null;
            this.ImageEx.Source = url;
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