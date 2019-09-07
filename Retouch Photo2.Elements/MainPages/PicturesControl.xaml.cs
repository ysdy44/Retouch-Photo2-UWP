using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "PicturesControl" />.
    /// </summary>
    public sealed partial class PicturesControl : UserControl
    {
        //@Content
        /// <summary> <see cref = "PicturesControl" />'s PhotoButton. </summary>
        public Windows.UI.Xaml.Controls.Button PhotoButton  => this._PhotoButton.RootButton;  
        /// <summary> <see cref = "PicturesControl" />'s DestopButton. </summary>
        public Windows.UI.Xaml.Controls.Button DestopButton  => this._DestopButton.RootButton;  
        /// <summary> <see cref = "PicturesControl" />'s CancelButton. </summary>
        public Windows.UI.Xaml.Controls.Button CancelButton => this._CancelButton.RootButton;

        //@Construct
        public PicturesControl()
        {
            this.InitializeComponent();
        }
    }
}
