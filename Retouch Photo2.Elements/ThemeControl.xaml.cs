using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Toggle sensitive themes for your application. 
    /// </summary>
    public sealed partial class ThemeControl : UserControl
    {

        /// <summary> Theme of current <see cref="Application"/>. </summary>
        public ApplicationTheme ApplicationTheme { set { this.Theme = (value == ApplicationTheme.Dark) ? ElementTheme.Dark : ElementTheme.Light; } }
        
        #region DependencyProperty

        /// <summary> Theme of current <see cref="Window"/>. </summary>
        public ElementTheme Theme
        {
            get { return (ElementTheme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ThemeControl.Theme" /> dependency property. </summary>
        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(nameof(Theme), typeof(ElementTheme), typeof(ThemeControl), new PropertyMetadata(ElementTheme.Default, (sender, e) =>
        {
            ThemeControl con = (ThemeControl)sender;

            if (e.NewValue is ElementTheme value)
            {
                if (value == ElementTheme.Dark)
                    con.DarkStoryboard.Begin();//Storyboard
                else
                    con.LightStoryboard.Begin();//Storyboard      
            }
        }));

        #endregion
        
        //@Construct
        public ThemeControl()
        {
            this.InitializeComponent();
        }
    }
}