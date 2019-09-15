using Retouch_Photo2.Controls;
using Retouch_Photo2.Elements.DrawPages;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Converter
        private FrameworkElement IconConverter(ITool tool) => tool.ShowIcon;
        private Page PageConverter(ITool tool) => tool.Page;
        public Visibility BoolToVisibilityConverter(bool isChecked) => isChecked ? Visibility.Visible : Visibility.Collapsed;

        //@Construct
        public DrawPage()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.ThemeControl.ApplicationTheme = App.Current.RequestedTheme;

            //Appbar
            this.ExpandAppbar.ChildrenWidth = new List<double> { 40, 4, 40, 4, 40, 32, 40, 4, 40, 4, 40, 4, 40, };
            this.DrawLayout.BackButton.Tapped += (s, e) => this.Frame.GoBack();
            this.SaveButton.Tapped += (s, e) => this.Frame.GoBack();

            //FullScreen
            this.UnFullScreenButton.Tapped += (s, e) => this.DrawLayout.IsFullScreen = !DrawLayout.IsFullScreen;
            this.FullScreenButton.Tapped += (s, e) => DrawLayout.IsFullScreen = !DrawLayout.IsFullScreen;
                                   
            //Menu
            this.Construct(this.TipViewModel.DebugMenu);
            this.Construct(this.TipViewModel.SelectionMenu);
            this.Construct(this.TipViewModel.OperateMenu);
            this.Construct(this.TipViewModel.AdjustmentMenu);
            this.Construct(this.TipViewModel.EffectMenu);
            this.Construct(this.TipViewModel.TransformerMenu);
            this.Construct(this.TipViewModel.ColorMenu);
            this.Construct(this.TipViewModel.ToolMenu);
            this.Construct(this.TipViewModel.LayerMenu);
        }


        private void Construct(IMenu menu)
        {
            UIElementCollection uIElements = this.OverlayCanvas.Children;

            //MenuOverlay
            UIElement overlay = menu.Overlay;
            uIElements.Add(overlay);

            menu.Move += (s, e) =>
            {
                int index = uIElements.IndexOf(menu.Overlay);
                int count = uIElements.Count;
                uIElements.Move((uint)index, (uint)count - 1);
            };

            //MenuButton
            IMenuButton menuButton = menu.Button;
            switch (menuButton.Type)
            {
                case MenuButtonType.None:
                    {
                        FrameworkElement button = menuButton.Self;
                        this.DrawLayout.HeadRightStackPane.Children.Add(button);
                    }
                    break;
                case MenuButtonType.ToolButton:
                    {
                        FrameworkElement toolButton = menuButton.Self;
                        this.ToolsControl.MoreBorderChild = toolButton;
                    }
                    break;
                case MenuButtonType.LayersControl:
                    {
                        FrameworkElement layersControl = menuButton.Self;
                        this.DrawLayout.RightPane = layersControl;
                    }
                    break;
            }
        }


        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            return;
            if (e.Parameter is Project project)
            {
                if (project == null)
                {
                    base.Frame.GoBack();
                    return;
                }

             //   this.Loaded += (sender, e2) =>
                //{

            this.LoadingControl.Visibility = Visibility.Visible;//Loading
            this.ViewModel.LoadFromProject(project);//Project
            this.LoadingControl.Visibility = Visibility.Collapsed;//Loading   

            this.ViewModel.Invalidate();
               // };
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }
    }
}
