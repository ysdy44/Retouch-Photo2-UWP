using Retouch_Photo2.Elements;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.ViewModels
{
    public partial class MenuViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the type. </summary>
        public string Type { get; set; }


        /// <summary> Gets or sets the title. </summary>
        public string Title
        {
            get => this.title;
            set
            {
                if (this.title == value) return;
                this.title = value;
                this.OnPropertyChanged(nameof(Title));//Notify 
            }
        }
        private string title;


        /// <summary> Gets or sets the <see cref="ToolTip.IsOpen"/>. </summary>
        public bool IsOpen
        {
            get => this.isOpen;
            set
            {
                if (this.isOpen == value) return;
                this.isOpen = value;
                this.OnPropertyChanged(nameof(IsOpen));//Notify 
            }
        }
        private bool isOpen;


        /// <summary> Gets or sets the <see cref="Windows.UI.Xaml.Visibility"/> for button. </summary>
        public Visibility ButtonVisibility
        {
            get => this.buttonVisibility;
            set
            {
                if (this.buttonVisibility == value) return;
                this.buttonVisibility = value;
                this.OnPropertyChanged(nameof(ButtonVisibility));//Notify 
            }
        }
        private Visibility buttonVisibility;


        /// <summary> Gets or sets the <see cref="Windows.UI.Xaml.Visibility"/>. </summary>
        public Visibility Visibility
        {
            get => this.visibility;
            set
            {
                if (this.visibility == value) return;
                this.visibility = value;
                this.OnPropertyChanged(nameof(Visibility));//Notify 
            }
        }
        private Visibility visibility = Visibility.Collapsed;


        /// <summary> Gets or sets the <see cref="Canvas.LeftProperty"/>. </summary>
        public double Left
        {
            get => this.left;
            set
            {
                if (this.left == value) return;
                this.left = value;
                this.OnPropertyChanged(nameof(Left));//Notify 
            }
        }
        private double left;

        /// <summary> Gets or sets the <see cref="Canvas.TopProperty"/>. </summary>
        public double Top
        {
            get => this.top;
            set
            {
                if (this.top == value) return;
                this.top = value;
                this.OnPropertyChanged(nameof(Top));//Notify 
            }
        }
        private double top;


        /// <summary> Gets or sets the <see cref="FrameworkElement.Width"/>. </summary>
        public double Width
        {
            get => this.width;
            set
            {
                if (this.width == value) return;
                this.width = value;
                this.OnPropertyChanged(nameof(Width));//Notify 
            }
        }
        private double width = 200.0d;

        /// <summary> Gets or sets the <see cref="FrameworkElement.Height"/>. </summary>
        public double Height
        {
            get => this.height;
            set
            {
                if (this.height == value) return;
                this.height = value;
                this.OnPropertyChanged(nameof(Height));//Notify 
            }
        }
        private double height = 200.0d;


        /// <summary>
        /// Show by placement target.
        /// </summary>  
        public void Show(FrameworkElement placementTarget, double windowWidth, double windowHeight, FlyoutPlacementMode placementMod)
        {
            this.Left = ExpanderButton.CalculatePostionX(placementTarget, this.Width, windowWidth, placementMod);
            this.Top = ExpanderButton.CalculatePostionY(placementTarget, this.Height, windowHeight, placementMod);
            this.Visibility = Visibility.Visible;
        }


        //@Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName"> Name of the property used to notify listeners. </param>
        protected void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}