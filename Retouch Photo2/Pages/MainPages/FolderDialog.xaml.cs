using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Pages.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "FolderDialog" />.
    /// </summary>
    public sealed partial class FolderDialog : ContentDialog
    {
        /// <summary> <see cref = "FolderDialog" /> 's Text.</summary>
        public string Text 
        {
            get => this.TextBox.Text;
            set => this.TextBox.Text=value;
        }

        public FolderDialog()
        {
            this.InitializeComponent();
        }
    }
}