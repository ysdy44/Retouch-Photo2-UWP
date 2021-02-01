// Core:              ★★
// Referenced:   ★
// Difficult:         
// Only:              ★★★
// Complete:      
using System;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Item of <see cref="Project"/>. 
    /// </summary>
    public sealed partial class ProjectViewItem : UserControl, IProjectViewItem
    {
        
        //@Static
        /// <summary> Occurs when tapped the project-control. </summary>
        public static Action<ProjectViewItem> ItemClick;


        //@VisualState
        SelectMode _vsSelectMode = SelectMode.None;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
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
        
        public void Rename(string name,  string thumbnail)
        {
            this.Name = name;
            this.ImageEx.Source = new Uri(thumbnail, UriKind.Relative);
        }
        
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
        
        public void RefreshImageSource()
        {
            object url = this.ImageEx.Source;
            this.ImageEx.Source = null;
            this.ImageEx.Source = url;
        }
                
        public Rect GetVisualRect(UIElement visual)
        {
            //Gets visual-postion in visual.
            Point sourcePostion = this.ImageEx.TransformToVisual(visual).TransformPoint(new Point());//@VisualPostion

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