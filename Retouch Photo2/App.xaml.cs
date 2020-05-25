using Retouch_Photo2.Menus;
using Retouch_Photo2.Menus.Models;
using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    public sealed partial class App : Application
    {

        static ViewModel viewModel = new ViewModel();

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.ViewModel" />. </summary>
        public static ViewModel ViewModel => App.viewModel;

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.SelectionViewModel" />. </summary>
        public static ViewModel SelectionViewModel => App.viewModel;

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.MethodViewModel" />. </summary>
        public static ViewModel MethodViewModel => App.viewModel;

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.SettingViewModel" />. </summary>
        public static SettingViewModel SettingViewModel = new SettingViewModel();

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.TipViewModel" />. </summary>
        public static TipViewModel TipViewModel = new TipViewModel(new NoneTool(), new MoveTool(), new TransformerTool(), new CreateTool())
        {
            //Tool
            Tools = new List<ITool>
            {               
                
                /*                
                 new CursorTool(),                 
                 new ViewTool(),
                 new GeometryRectangleTool(),
                 */

                /*
                 */
                 new CursorTool(),
                 null,

                 new ViewTool(),
                 new BrushTool(),
                 //new TransparencyTool(),
                 null,

                 //Geometry0
                 new GeometryRectangleTool(),
                 new GeometryEllipseTool(),
                 new PenTool(),
                 new NodeTool(),
                 null,

                 new TextArtisticTool(),
                 new TextFrameTool(),
                 null,

                 new ImageTool(),
                 new CropTool(),
                 

                 new MoreTool(),
                 

                 //Geometry1
                 new GeometryRoundRectTool(),
                 new GeometryTriangleTool(),
                 new GeometryDiamondTool(),
                 null,

                 //Geometry2
                 new GeometryPentagonTool(),
                 new GeometryStarTool(),
                 new GeometryCogTool(),
                 null,

                 //Geometry3
                 new GeometryDountTool(),
                 new GeometryPieTool(),
                 new GeometryCookieTool(),
                 null,

                 //Geometry4
                 new GeometryArrowTool(),
                 new GeometryCapsuleTool(),
                 new GeometryHeartTool(),
                 
            },

            //Menu
            Menus = new List<IMenu>
            {
                new KeyboardMenu(),

                new SelectionMenu(),
                new OperateMenu(),

                new AdjustmentMenu(),
                new EffectMenu(),

                new CharacterMenu(),
                new ParagraphMenu(),

                new StrokeMenu(),
                new StyleMenu(),

                new HistoryMenu(),
                new TransformerMenu(),

                new LayerMenu(),
                new ColorMenu(),
                /*
                 */
            },
        };


        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }


        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
