using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.DrawPages
{
    /// <summary>
    /// <see cref = "DrawPage" /> Appbar's <see cref = "RenameDialog" />.
    /// </summary>
    public sealed partial class RenameDialog : UserControl
    {

        //@Content
        /// <summary> <see cref = "RenameDialog" /> 's TextBox.</summary>
        public TextBox TextBox => this._TextBox;
        /// <summary> <see cref = "RenameDialog" /> 's CloseButton.</summary>
        public Button CloseButton => this._CloseButton;
        /// <summary> <see cref = "RenameDialog" /> 's PrimaryButton.</summary>
        public Button PrimaryButton => this._PrimaryButton;

        //@Construct
        public RenameDialog()
        {
            this.InitializeComponent();
        }
    }
}
