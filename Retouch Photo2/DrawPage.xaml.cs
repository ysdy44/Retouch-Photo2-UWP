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
        private FrameworkElement IconConverter(ITool tool) => tool.Icon;
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


            //Tool
            this.ConstructTool(this.TipViewModel.CursorTool);
            this.ConstructTool(this.TipViewModel.ViewTool);
            this.ConstructTool(this.TipViewModel.BrushTool);
            this.ConstructTool(this.TipViewModel.RectangleTool);
            this.ConstructTool(this.TipViewModel.EllipseTool);
            this.ConstructTool(this.TipViewModel.PenTool);
            this.ConstructTool(this.TipViewModel.ImageTool);
            this.ConstructTool(this.TipViewModel.AcrylicTool);


            //Menu
            this.ConstructMenu(this.TipViewModel.ToolMenu);
            this.ConstructMenu(this.TipViewModel.LayerMenu);
            this.ConstructMenu(this.TipViewModel.DebugMenu);
            this.ConstructMenu(this.TipViewModel.SelectionMenu);
            this.ConstructMenu(this.TipViewModel.OperateMenu);
            this.ConstructMenu(this.TipViewModel.AdjustmentMenu);
            this.ConstructMenu(this.TipViewModel.EffectMenu);
            this.ConstructMenu(this.TipViewModel.TransformerMenu);
            this.ConstructMenu(this.TipViewModel.ColorMenu);
        }

        #region Tool


        List<ITool> Tools = new List<ITool>();
        UIElementCollection TooLeft => this.DrawLayout.LeftPaneChildren;


        private void ConstructTool(ITool tool)
        {
            if (tool == null) return;

            this.Tools.Add(tool);

            IToolButton button = tool.Button;

            button.Self.Tapped += (s, e) =>
            {
                this.ToolGroupType(tool.Type);
                this.TipViewModel.Tool = tool;
            };

            this.TooLeft.Add(button.Self);
        }

        private void ToolGroupType(ToolType groupType)
        {
            foreach (ITool tool in this.Tools)
            {
                if (tool == null) break;

                tool.IsSelected = (tool.Type == groupType);
            }

            this.ViewModel.Invalidate();//Invalidate

            this.TipViewModel.SetTouchbar(TouchbarType.None);//Touchbar
        }


        #endregion

        #region Menu


        UIElementCollection MenuOverlay => this.OverlayCanvas.Children;
        UIElementCollection MenuHead => this.DrawLayout.HeadRightChildren;
        UIElement MenuLayersControl { set => this.DrawLayout.RightPane = value; }


        private void ConstructMenu(IMenu menu)
        {
            if (menu == null) return;
            
            //MenuOverlay
            UIElement overlay = menu.Overlay;
            this.MenuOverlay.Add(overlay);

            menu.Move += (s, e) =>
            {
                int index = this.MenuOverlay.IndexOf(menu.Overlay);
                int count = this.MenuOverlay.Count;
                this.MenuOverlay.Move((uint)index, (uint)count - 1);
            };

            //MenuButton
            IMenuButton menuButton = menu.Button;
            switch (menuButton.Type)
            {
                case MenuButtonType.None: this.MenuHead.Add(menuButton.Self); break;
                case MenuButtonType.ToolButton: this.TooLeft.Add(menuButton.Self); break;
                case MenuButtonType.LayersControl: this.MenuLayersControl = menuButton.Self; break;
            }
        }


        #endregion

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
