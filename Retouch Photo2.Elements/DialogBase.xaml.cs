// Core:              ★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements.Dialogs
{
    public sealed partial class DialogBase : UserControl
    {

        //@Content
        /// <summary> <see cref = "DialogBase" /> 's Content.</summary>
        public object CenterContent
        {
            get => this.ContentPresenter.Content;
            set => this.ContentPresenter.Content = value;
        }
        /// <summary> <see cref = "DialogBase" /> 's Title.</summary>
        public object Title
        {
            get => this.ContentControl.Content;
            set => this.ContentControl.Content = value;
        }

        /// <summary> <see cref = "DialogBase" /> 's CloseButton.</summary>
        public Button CloseButton => this._CloseButton;
        /// <summary> <see cref = "DialogBase" /> 's PrimaryButton.</summary>
        public Button PrimaryButton => this._PrimaryButton;

        //@Construct
        /// <summary>
        /// Initializes a DialogBase. 
        /// </summary>
        public DialogBase()
        {
            this.InitializeComponent();
            this.LayoutRoot.Tapped += (s, e) => this.Hide();
            this.HideStoryboard.Completed += (s, e) =>
            {
                this.LayoutRoot.Visibility = Visibility.Collapsed;
            };
        }

        /// <summary>
        /// Show the dialog.
        /// </summary>
        public void Show()
        {
            this.ShowStoryboard.Begin();
        }

        /// <summary>
        /// Hide the dialog.
        /// </summary>
        public void Hide()
        {
            this.HideStoryboard.Begin();
        }
    }
}