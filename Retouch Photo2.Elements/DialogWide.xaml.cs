// Core:              ★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.System;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents a dialog box that contains check boxes, hyperlinks, buttons, and other XAML content that you can customize.
    /// </summary>
    public sealed partial class DialogWide : UserControl
    {

        //@Content
        /// <summary> <see cref = "DialogWide" /> 's GridView.</summary>
        public GridView GridView => this._GridView;
        /// <summary> <see cref = "DialogWide" /> 's Title.</summary>
        public object Title
        {
            get => this.ContentControl.Content;
            set => this.ContentControl.Content = value;
        }

        /// <summary> <see cref = "DialogWide" /> 's CloseButton.</summary>
        public Button CloseButton => this._CloseButton;
        /// <summary> <see cref = "DialogWide" /> 's PrimaryButton.</summary>
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

        /// <summary>
        /// Show the dialog.
        /// </summary>
        public void Show()
        {
            this._vsIsShow = true;
            this.VisualState = this.VisualState;//State

            this._PrimaryButton.Focus(FocusState.Pointer);
            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;
        }

        /// <summary>
        /// Hide the dialog.
        /// </summary>
        public void Hide()
        {
            this._vsIsShow = false;
            this.VisualState = this.VisualState;//State

            Window.Current.CoreWindow.KeyDown -= this.CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Escape:
                    this.Hide();
                    break;
                default:
                    break;
            }
        }

    }
}