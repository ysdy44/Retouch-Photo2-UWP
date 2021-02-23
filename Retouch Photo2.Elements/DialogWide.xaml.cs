// Core:              ★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents a dialog box that contains grid view that you can customize.
    /// </summary>
    public sealed partial class DialogWide : UserControl
    {

        //@Content
        /// <summary> <see cref = "DialogWide" /> 's GridView.</summary>
        public GridView GridView => this._GridView;
        /// <summary> <see cref = "Dialog" /> 's Title.</summary>
        public object Title { get => this.ContentControl.Content; set => this.ContentControl.Content = value; }

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
        /// Initializes a DialogWide. 
        /// </summary>
        public DialogWide()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.LayoutRoot.Tapped += (s, e) => this.Hide();
            this.RootGrid.Tapped += (s, e) => e.Handled = true;
        }

    }
}