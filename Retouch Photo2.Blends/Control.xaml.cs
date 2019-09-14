using Retouch_Photo2.Blends.Icons;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Control of <see cref="Blend">.
    /// </summary>
    public sealed partial class Control : UserControl
    {
        //@Content
        /// <summary> RightBorder's Child. </summary>
        public ComboBox ComboBox => this._ComboBox;

        //@Converter
        private int BlendTypeToIntConverter(BlendType type) => (int)type;

        #region DependencyProperty

        /// <summary> Gets or sets type. </summary>
        public BlendType BlendType
        {
            get { return (BlendType)GetValue(BlendTypeProperty); }
            set { SetValue(BlendTypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "Control.BlendType"/> dependency property. </summary>
        public static readonly DependencyProperty BlendTypeProperty = DependencyProperty.Register(nameof(BlendType), typeof(BlendType), typeof(Control), new PropertyMetadata(BlendType.Normal));

        #endregion

        //@Construct
        public Control()
        {
            this.InitializeComponent();
        }
    }
}