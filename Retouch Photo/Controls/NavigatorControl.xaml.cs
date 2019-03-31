using Retouch_Photo.Element;
using Retouch_Photo.Library;
using Retouch_Photo.ViewModels;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo.Controls
{
    public sealed partial class NavigatorControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        #region DependencyProperty

        /// <summary>
        /// Brush of <see cref="ApplicationViewTitleBar"/>.
        /// </summary>
        public SolidColorBrush Brush
        {
            get { return (SolidColorBrush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Brush), typeof(SolidColorBrush), typeof(NavigatorControl), new PropertyMetadata(new SolidColorBrush(Colors.White), (sender, e) =>
        {
            NavigatorControl con = (NavigatorControl)sender;

            if (e.NewValue is SolidColorBrush value)
            {
                ThemeControl.TitleBarColor = value.Color;
            }
        }));

        #endregion

        public NavigatorControl()
        {
            this.InitializeComponent();

            //Navigator
            this.FiftyPercent.Tapped += (sender, e) => this.Navigator((m) => m.Fit(0.5f));
            this.HundredPercent.Tapped += (sender, e) => this.Navigator((m) => m.Fit(1f));
            this.TwoHundredPercent.Tapped += (sender, e) => this.Navigator((m) => m.Fit(2f));
            this.AutoPercent.Tapped += (sender, e) => this.Navigator((m) => m.Fit());


            //Theme
            this.ThemeSwitch.Toggled += (sender, e) => ThemeControl.Theme = (this.ThemeSwitch.IsOn ? ElementTheme.Dark : ElementTheme.Light);
            this.ThemeSwitch.Loaded += (sender, e) =>
            {
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    this.ThemeSwitch.IsOn = (frameworkElement.RequestedTheme != ElementTheme.Light);
                }
            };


            //Ruler
            this.RulerSwitch.Toggled += (sender, e) =>
            {
                this.ViewModel.MatrixTransformer.IsRuler = this.RulerSwitch.IsOn;
                this.ViewModel.Invalidate();
            };

            
        }

        private void Navigator(Action<MatrixTransformer> action)
        {
            action(this.ViewModel.MatrixTransformer);

            App.ViewModel.Invalidate();
        }
    }
}
