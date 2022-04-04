// Core:              ★★★★★
// Referenced:   ★★
// Difficult:         
// Only:              
// Complete:      ★
using System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provides constant and static methods 
    /// for gets and sets to <see cref="ApplicationViewTitleBar"/>'s background and title.
    /// </summary>
    public class ApplicationTitleBarExtension : FrameworkElement
    {

        private readonly Lazy<ApplicationView> ViewLazy = new Lazy<ApplicationView>(() => ApplicationView.GetForCurrentView());
        private ApplicationView View => this.ViewLazy.Value;

        private Color Color
        {
            set
            {
                this.View.TitleBar.BackgroundColor = value;
                this.View.TitleBar.InactiveBackgroundColor = value;
                this.View.TitleBar.ButtonBackgroundColor = value;
                this.View.TitleBar.ButtonInactiveBackgroundColor = value;
            }
        }


        #region DependencyProperty


        /// <summary> Gets or set the color for <see cref="ApplicationViewTitleBar"/>. </summary>
        public Color TitleBarColor
        {
            get => (Color)base.GetValue(TitleBarColorProperty);
            set => base.SetValue(TitleBarColorProperty, value);
        }
        /// <summary> Identifies the <see cref = "ApplicationTitleBarExtension.TitleBarColor" /> dependency property. </summary>
        public static readonly DependencyProperty TitleBarColorProperty = DependencyProperty.Register(nameof(TitleBarColor), typeof(Color), typeof(ApplicationTitleBarExtension), new PropertyMetadata(Colors.Gray, (sender, e) =>
        {
            ApplicationTitleBarExtension control = (ApplicationTitleBarExtension)sender;

            if (e.NewValue is Color value)
            {
                if (control.IsAccent == false) control.Color = value;
            }
        }));


        /// <summary> Gets or set the accent color for <see cref="ApplicationViewTitleBar"/>. </summary>
        public Color AccentTitleBarColor
        {
            get => (Color)base.GetValue(AccentTitleBarColorProperty);
            set => base.SetValue(AccentTitleBarColorProperty, value);
        }
        /// <summary> Identifies the <see cref = "ApplicationTitleBarExtension.AccentTitleBarColor" /> dependency property. </summary>
        public static readonly DependencyProperty AccentTitleBarColorProperty = DependencyProperty.Register(nameof(AccentTitleBarColor), typeof(Color), typeof(ApplicationTitleBarExtension), new PropertyMetadata(Colors.DodgerBlue, (sender, e) =>
        {
            ApplicationTitleBarExtension control = (ApplicationTitleBarExtension)sender;

            if (e.NewValue is Color value)
            {
                if (control.IsAccent == true) control.Color = value;
            }
        }));


        /// <summary> Gets or set the state for <see cref="ApplicationViewTitleBar"/>. </summary>
        public bool IsAccent
        {
            get => this.isAccent;
            set
            {
                this.Color = value ? this.AccentTitleBarColor : this.TitleBarColor;
                this.isAccent = value;
            }
        }
        private bool isAccent;

        #endregion


        /// <summary> <see cref="ApplicationView.Title"/> </summary>
        public string Title { get => this.View.Title; set => this.View.Title = value; }


        /// <summary> <see cref="ApplicationView.IsFullScreenMode"/> </summary>
        public bool IsFullScreenMode => this.View.IsFullScreenMode;
        /// <summary> <see cref="ApplicationView.ExitFullScreenMode"/> </summary>
        public void ExitFullScreenMode() => this.View.ExitFullScreenMode();
        /// <summary> <see cref="ApplicationView.TryEnterFullScreenMode"/> </summary>
        public bool TryEnterFullScreenMode() => this.View.TryEnterFullScreenMode();

    }
}