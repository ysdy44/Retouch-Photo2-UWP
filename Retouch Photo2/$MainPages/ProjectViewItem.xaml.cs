// Core:              ★★
// Referenced:   ★
// Difficult:         
// Only:              ★★★
// Complete:      
using Retouch_Photo2.ViewModels;
using System;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Item of <see cref="Project"/>. 
    /// </summary>
    public sealed partial class ProjectViewItem : UserControl, IProjectViewItem
    {

        //@Content
        /// <summary> Gets or sets the name. </summary>
        //public string Name { get; set; } 
        /// <summary> Gets or sets the thumbnail path. </summary>
        public Uri ImageSource { get => this.BitmapImage.UriSource; set => this.BitmapImage.UriSource = value; }
        /// <summary> Gets or sets the rect of image visual area. </summary>
        public Rect ImageVisualRect { get; private set; } = Rect.Empty;
        /// <summary> Gets or sets the project. </summary>
        public Project Project { get; set; }


        //@VisualState
        bool _vsIsMultiple = false;
        bool _vsIsSelected = false;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsMultiple)
                {
                    if (this._vsIsSelected) 
                        return this.Selected;
                    else 
                        return this.UnSelected;
                }
                else 
                    return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        /// <summary> Gets or sets the state of select-mode. </summary>
        public bool IsMultiple
        {
            get => this._vsIsMultiple;
            set
            {
                this._vsIsMultiple = value;
                this.VisualState = this.VisualState;//State
            }
        }
        /// <summary> Gets or sets the weather is selected. </summary>
        public bool IsSelected
        {
            get => this._vsIsSelected;
            set
            {
                this._vsIsSelected = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a ProjectViewItem from <see cref = "StorageFolder" />.
        /// </summary>
        public ProjectViewItem()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Set the position and size of the image element relative to the visual element for <see cref="IProjectViewItem.ImageVisualRect"/>
        /// </summary>
        /// <returns> The calculated widnows. </returns>
        public void RenderImageVisualRect(UIElement widnows)
        {
            //Gets visual-postion in visual.
            Point sourcePostion = this.ImageBorder.TransformToVisual(widnows).TransformPoint(new Point());//@VisualPostion

            this.ImageVisualRect = new Rect
            {
                X = sourcePostion.X,
                Y = sourcePostion.Y,
                Width = this.ImageEx.ActualWidth,
                Height = this.ImageEx.ActualHeight
            };
        }

    }
}