using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Button that's been held down.
    /// </summary>
    public sealed partial class RepeatControl : UserControl
    {

        #region DependencyProperty

        /// <summary> Checked of <see cref="RepeatControl"/>. </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(RepeatControl), new PropertyMetadata(false, (sender, e) =>
        {
            RepeatControl con = (RepeatControl)sender;

            if (e.NewValue is bool value)
            {
                switch (value)
                {
                    case true:
                        con.ShowStoryboard.Begin();//Storyboard
                        break;

                    case false:
                        con.HideStoryboard.Begin();//Storyboard
                        break;
                }
            }
        }));

        #endregion
         

        //@Construct
        public RepeatControl()
        {
            this.InitializeComponent();

            this.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                    this.IsChecked = true;
            };
            this.PointerPressed += (s, e) => this.IsChecked = true;
            this.PointerReleased += (s, e) => this.IsChecked = false;
            this.PointerExited += (s, e) =>  this.IsChecked = false;
        }
    }
}