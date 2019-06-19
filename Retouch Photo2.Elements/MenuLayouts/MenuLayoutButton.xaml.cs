using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Button of <see cref="MenuLayout"/>..
    /// </summary>
    public sealed partial class MenuLayoutButton : UserControl
    {               
        //@Converter
        private SolidColorBrush StateToBackgroundConverter(MenuLayoutState state) => (state ==  MenuLayoutState.FlyoutShow) ? this.AccentColor : this.UnAccentColor;
        private SolidColorBrush StateToForegroundConverter(MenuLayoutState state) => (state != MenuLayoutState.FlyoutHide) ? this.CheckColor : this.UnCheckColor;

        //@Content
        /// <summary> Content of <see cref="MenuLayout"/>.. </summary>
        public object CenterContent { set => this.RootButton.Content = value; get => this.RootButton.Content; }
        
        #region DependencyProperty
        
        /// <summary> State of <see cref="MenuLayoutButton"/>. </summary>
        public MenuLayoutState State
        {
            get { return (MenuLayoutState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }
        /// <summary> Identifies the <see cref = "MenuLayoutButton.State" /> dependency property. </summary>
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof(State), typeof(MenuLayoutState), typeof(MenuLayoutButton), new PropertyMetadata(MenuLayoutState.FlyoutHide));

        #endregion

        //@Construct
        public MenuLayoutButton()
        {
            this.InitializeComponent();
            this.RootButton.Tapped += (s, e) => this.State = MenuLayoutButton.GetState(this.State);
        }

        //@Static
        /// <summary>
        /// Get the corresponding status. 
        /// </summary>
        /// <param name="state"> The source state. </param>
        /// <returns></returns>
        public static MenuLayoutState GetState(MenuLayoutState state)
        {
            switch (state)
            {
                case MenuLayoutState.FlyoutHide:
                    return MenuLayoutState.FlyoutShow;
                case MenuLayoutState.FlyoutShow:
                    return MenuLayoutState.FlyoutHide;

                case MenuLayoutState.RootExpanded:
                    return MenuLayoutState.RootNotExpanded;
                case MenuLayoutState.RootNotExpanded:
                    return MenuLayoutState.RootExpanded;
            }
            return MenuLayoutState.FlyoutShow;
        }

    }
}