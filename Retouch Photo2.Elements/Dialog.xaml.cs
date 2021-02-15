// Core:              ★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents a dialog box that contains check boxes, hyperlinks, buttons, and other XAML content that you can customize.
    /// </summary>
    public sealed partial class Dialog : UserControl
    {

        //@Content
        /// <summary> <see cref = "Dialog" /> 's Content.</summary>
        public object CenterContent
        {
            get => this.ContentPresenter.Content;
            set => this.ContentPresenter.Content = value;
        }
        /// <summary> <see cref = "Dialog" /> 's Title.</summary>
        public object Title
        {
            get => this.ContentControl.Content;
            set => this.ContentControl.Content = value;
        }

        /// <summary> <see cref = "Dialog" /> 's CloseButton.</summary>
        public Button CloseButton => this._CloseButton;
        /// <summary> <see cref = "Dialog" /> 's PrimaryButton.</summary>
        public Button PrimaryButton => this._PrimaryButton;




        //@VisualState
        bool _vsIsShow;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get => this._vsIsShow ? this.Show2 : this.Hide2;
            set => VisualStateManager.GoToState(this, value.Name, true);
        }


        //@Construct
        /// <summary>
        /// Initializes a DialogBase. 
        /// </summary>
        public Dialog()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.LayoutRoot.Tapped += (s, e) => this.Hide();
        }

        /// <summary>
        /// Show the dialog.
        /// </summary>
        public void Show()
        {
            this._vsIsShow = true;
            this.VisualState = this.VisualState;//State
        }

        /// <summary>
        /// Hide the dialog.
        /// </summary>
        public void Hide()
        {
            this._vsIsShow = false;
            this.VisualState = this.VisualState;//State
        }
    }
}