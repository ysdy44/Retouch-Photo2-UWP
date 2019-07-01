using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{ 
    /// <summary> 
    /// Click the button to toggle sensitive themes for your application. 
    /// </summary>
    public sealed partial class ThemeControl : UserControl
    {
        
        /// <summary> Theme of current <see cref="Application"/>. </summary>
        public ApplicationTheme ApplicationTheme
        {
            set
            {
                this.Theme = (value == ApplicationTheme.Dark) ? ElementTheme.Dark : ElementTheme.Light;
            }
        }

        private ElementTheme _Theme
        {
            get => this.theme;
            set
            {
                if (value == ElementTheme.Dark)
                    this.DarkStoryboard.Begin();//Storyboard                
                else
                    this.LightStoryboard.Begin();//Storyboard                

                this.theme = value;
            }
        }

        private ElementTheme theme;

        private ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;

        #region DependencyProperty


        /// <summary> Color of <see cref="ApplicationViewTitleBar"/>. </summary>
        public Color TitleBarColor
        {
            get { return (Color)GetValue(TitleBarColorProperty); }
            set { SetValue(TitleBarColorProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ThemeControl.TitleBarColor" /> dependency property. </summary>
        public static readonly DependencyProperty TitleBarColorProperty = DependencyProperty.Register(nameof(TitleBarColor), typeof(Color), typeof(ThemeControl), new PropertyMetadata(Colors.White, (sender, e) =>
        {
            ThemeControl con = (ThemeControl)sender;

            if (e.NewValue is Color value)
            {
                con.TitleBar.BackgroundColor =
                con.TitleBar.InactiveBackgroundColor =
                con.TitleBar.ButtonBackgroundColor =
                con.TitleBar.ButtonInactiveBackgroundColor = value;
            }
        }));


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
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    frameworkElement.RequestedTheme = value;
                }

                con._Theme = value;
            }
        }));


        #endregion

        //@Construct
        public ThemeControl()
        {
            this.InitializeComponent();
            this.Button.Tapped += (s, e) => this.Theme = (this.Theme == ElementTheme.Dark) ? ElementTheme.Light : ElementTheme.Dark;
            //this.Loaded += (s, e) => this.Theme = (App.Current.RequestedTheme == ApplicationTheme.Dark) ? ElementTheme.Dark : ElementTheme.Light;
        }
    }
}