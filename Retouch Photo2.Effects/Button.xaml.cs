using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Retouch_Photo2 Effects 's Button.
    /// </summary>
    public sealed partial class Button : UserControl
    {
        /// <summary> <see cref = "Button" />'s RootButton. </summary>
        public Windows.UI.Xaml.Controls.Button RootButton => this._RootButton;
        /// <summary> <see cref = "Button" />'s ToggleSwitch. </summary>
        public Windows.UI.Xaml.Controls.ToggleSwitch ToggleSwitch => this._ToggleSwitch;


        #region DependencyProperty

        /// <summary> 
        /// Gets or sets whether the status of the <see cref = "Button" /> is "on". 
        /// 
        /// Null: RootButton.IsEnabled = false, ToggleSwitch.IsEnabled = false, ToggleSwitch.IsOn = false, 
        /// False: RootButton.IsEnabled = false, ToggleSwitch.IsEnabled = true, ToggleSwitch.IsOn = false, 
        /// True: RootButton.IsEnabled = true, ToggleSwitch.IsEnabled = true, ToggleSwitch.IsOn = true, 
        /// 
        /// </summary>
        public bool? IsOn
        {
            get { return (bool?)GetValue(IsOnProperty); }
            set { SetValue(IsOnProperty, value); }
        }
        /// <summary> Identifies the <see cref = "Button.IsOn" /> dependency property. </summary>
        public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(nameof(IsOn), typeof(bool?), typeof(Button), new PropertyMetadata(null, (sender, e) =>
        {
            Button con = (Button)sender;

            if (e.NewValue is bool value)
            {
                if (value)
                {
                    //True
                    con._ToggleSwitch.IsOn = true;
                    con._ToggleSwitch.IsEnabled = true;
                    con._RootButton.IsEnabled = true;
                }
                else
                {
                    //False
                    con._ToggleSwitch.IsOn = false;
                    con._ToggleSwitch.IsEnabled = true;
                    con._RootButton.IsEnabled = false;
                }
            }
            else
            {
                //Null
                con._ToggleSwitch.IsOn = false;
                con._ToggleSwitch.IsEnabled = false;
                con._RootButton.IsEnabled = false;
            }
        }));

         
        #endregion

        //@Construct
        public Button(UIElement icon)
        {
            this.InitializeComponent();
            this._RootButton.Content = icon;
        }
    }
}