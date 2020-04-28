using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Loading animation controls.
    /// </summary>
    public sealed partial class LoadingControl : UserControl
    {
        //@Content
        /// <summary> TextBlock's Text. </summary>
        public string Text { set => this.TextBlock.Text = value; get => this.TextBlock.Text; }
        
        #region DependencyProperty

        /// <summary> Gets or sets whether the <see cref = "LoadingControl" /> Visibility. </summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }       
        /// <summary> Identifies the <see cref = "LoadingControl.IsActive" /> dependency property. </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(LoadingControl), new PropertyMetadata(false, (sender, e) =>
        {
            LoadingControl con = (LoadingControl)sender;

            if (e.NewValue is bool value)
            {
                con.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                con.ProgressRing.IsActive = value;
            }
        }));

        #endregion

        //@Construct
        public LoadingControl()
        {
            this.InitializeComponent();
        }
    }
}