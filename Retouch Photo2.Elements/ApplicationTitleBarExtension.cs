// Core:              ★★★★★
// Referenced:   ★★
// Difficult:         
// Only:              
// Complete:      ★
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

        private static readonly ApplicationView View = ApplicationView.GetForCurrentView();
        private Color Color
        {
            set
            {
                ApplicationTitleBarExtension.View.TitleBar.BackgroundColor = value;
                ApplicationTitleBarExtension.View.TitleBar.InactiveBackgroundColor = value;
                ApplicationTitleBarExtension.View.TitleBar.ButtonBackgroundColor = value;
                ApplicationTitleBarExtension.View.TitleBar.ButtonInactiveBackgroundColor = value;
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref="ApplicationTitleBarExtension"/>'s title. </summary>
        public string Title { get => ApplicationTitleBarExtension.View.Title; set => ApplicationTitleBarExtension.View.Title = value; }


        /// <summary> Gets or set the color for <see cref="ApplicationViewTitleBar"/>. </summary>
        public Color TitleBarColor
        {
            get => (Color)base.GetValue(TitleBarColorProperty);
            set => SetValue(TitleBarColorProperty, value);
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
            set => SetValue(AccentTitleBarColorProperty, value);
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
            get => (bool)base.GetValue(IsAccentProperty);
            set => SetValue(IsAccentProperty, value);
        }
        /// <summary> Identifies the <see cref = "ApplicationTitleBarExtension.IsAccent" /> dependency property. </summary>
        public static readonly DependencyProperty IsAccentProperty = DependencyProperty.Register(nameof(IsAccent), typeof(bool), typeof(ApplicationTitleBarExtension), new PropertyMetadata(false, (sender, e) =>
        {
            ApplicationTitleBarExtension control = (ApplicationTitleBarExtension)sender;

            if (e.NewValue is bool value)
            {
                if (value) control.Color = control.AccentTitleBarColor;
                else control.Color = control.TitleBarColor;
            }
        }));


        #endregion


    }
}