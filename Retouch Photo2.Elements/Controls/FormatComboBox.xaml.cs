using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Image save format ComboBox.
    /// </summary>
    public sealed partial class FormatComboBox : UserControl
    {
        //@Content
        /// <summary> Format. </summary>
        public FormatType Format { get => (FormatType)this.ComboBox.SelectedIndex; set => this.ComboBox.SelectedIndex = (int)value; }

        //@Construct
        public FormatComboBox()
        {
            this.InitializeComponent();
        }
    }
}