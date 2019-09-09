using Retouch_Photo2.Controls;
using Retouch_Photo2.Elements.DrawPages;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2.Pages
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Converter
        private FrameworkElement IconConverter(ITool tool) => tool.ShowIcon;
        private Page PageConverter(ITool tool) => tool.Page;

        //@Construct
        public DrawPage()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) =>
            {
                this.ThemeControl.ApplicationTheme = App.Current.RequestedTheme;
            };

            this.BackButton.Tapped += (s, e) => this.Frame.GoBack();
            this.SaveButton.Tapped += (s, e) => this.Frame.GoBack();
                        

            //Menu
            DrawPage.AddMenu(this, this.TipViewModel.DebugMenu);
            DrawPage.AddMenu(this, this.TipViewModel.SelectionMenu);
            DrawPage.AddMenu(this, this.TipViewModel.OperateMenu);
            DrawPage.AddMenu(this, this.TipViewModel.AdjustmentMenu);
            DrawPage.AddMenu(this, this.TipViewModel.EffectMenu);
            DrawPage.AddMenu(this, this.TipViewModel.TransformerMenu);
            DrawPage.AddMenu(this, this.TipViewModel.ColorMenu);
            DrawPage.AddMenu(this, this.TipViewModel.ToolMenu);
            DrawPage.AddMenu(this, this.TipViewModel.LayerMenu);
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


        //@Static
        private static void AddMenu(DrawPage drawPage, IMenu menu)
        {
            //MenuOverlay
            UIElement overlay = menu.MenuOverlay;
            drawPage.OverlayCanvas.Children.Add(overlay);


            //MenuButton
            IMenuButton menuButton = menu.MenuButton;
            switch (menuButton.Type)
            {
                case MenuButtonType.None:
                    FrameworkElement button = menuButton.Self;
                    drawPage.DrawLayout.TopRightPanelChildren.Add(button);
                    break;
                case MenuButtonType.ToolButton:
                    FrameworkElement toolButton = menuButton.Self;
                    drawPage.ToolsControl.MoreBorderChild = toolButton;
                    break;
                case MenuButtonType.LayersControl:
                    FrameworkElement layersControl = menuButton.Self;
                    drawPage.DrawLayout.RightPane = layersControl;
                    break;
            }
        }
    }
}
