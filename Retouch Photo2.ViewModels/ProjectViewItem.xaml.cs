using Retouch_Photo2.Layers;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// MainPagr's the only <see cref = "ProjectViewItem" />. 
    /// </summary>
    public sealed partial class ProjectViewItem : UserControl
    {

        //@Static
        /// <summary> Occurs when tapped the project-control. </summary>
        public static Action<ProjectViewItem> ItemClick;
        /// <summary> Occurs when right-click input a project-control. </summary>
        public static Action<ProjectViewItem> RightTapped;


        //@VisualState
        SelectMode _vsSelectMode = SelectMode.None;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsSelectMode)
                {
                    case SelectMode.None: return this.Normal;
                    case SelectMode.UnSelected: return this.UnSelected;
                    case SelectMode.Selected: return this.Selected;
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Content
        /// <summary> Tittle. </summary>
        public string Tittle { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        /// <summary> ImageEx. </summary>
        public FrameworkElement ImageEx => this._ImageEx;

        /// <summary> Photo's describe. </summary>
        public string Describe { get; private set; }
        /// <summary> Photo's zip file path. </summary>
        public string ZipFilePath { get; private set; }
        /// <summary> Photo's file date. </summary>
        public DateTimeOffset Time { get; private set; }

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
        /// Construct a ProjectControl from <see cref = "StorageFile" />.
        /// </summary>
        /// <param name="flie">Photo's File</param>
        /// <param name="folderPath">Path</param>
        /// <returns> The project-control. </returns>
        public ProjectViewItem(StorageFile flie, string folderPath)
        {
            this.InitializeComponent();

            //Content    
            this.Tittle = flie.DisplayName;
            this._ImageEx.Source = new Uri($"{folderPath}/{flie.DisplayName}.png", UriKind.Relative);
            
            DateTimeOffset time = flie.DateCreated;
            this.Describe = $"{time.Year}.{time.Month}.{time.Day}";
            this.ZipFilePath = flie.Path;
            this.Time = time;

            //Static
            this._RootGrid.Tapped += (s, e) => ProjectViewItem.ItemClick?.Invoke(this);//Delegate
            this._RootGrid.RightTapped += (s, e) => ProjectViewItem.RightTapped?.Invoke(this);//Delegate
            this._RootGrid.Holding += (s, e) => ProjectViewItem.RightTapped?.Invoke(this);//Delegate
        }

    }
}