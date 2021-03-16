// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         
// Only:              ★★★★★
// Complete:      ★★★★★

// Core:              ★★★★★   // This is a very core Class.
// Core:                               // This is a useless Class.

// Referenced:   ★★★★★   // This Class is referenced by many others.
// Referenced:                    // This Class is rarely referenced.

// Difficult:         ★★★★★   // This class is very complex, please don't change it.
// Difficult:                          // This class is very simple, feel free to change it.

// Only:              ★★★★★   // This class has many of the same types.
// Only:                               // There is only one class.

// Complete:      ★★★★★   // This class is complete.
// Complete:                       // This class is not completed.

using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Menus.Models;
using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    public sealed partial class App : Application
    {

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.ViewModel" />. </summary>
        public static ViewModel ViewModel { get; } = new ViewModel();

        /// <summary> Retouch_Photo2's the only Selection<see cref = "ViewModels.ViewModel" />. </summary>
        public static ViewModel SelectionViewModel => App.ViewModel;

        /// <summary> Retouch_Photo2's the only Method<see cref = "ViewModels.ViewModel" />. </summary>
        public static ViewModel MethodViewModel => App.ViewModel;

        /// <summary> Retouch_Photo2's the only Setting<see cref = "ViewModels.ViewModel" />. </summary>
        public static SettingViewModel SettingViewModel = new SettingViewModel
        {
            KeyboardAccelerators = new List<KeyboardAccelerator2>
            {                         
                new KeyboardAccelerator2
                {                           
                    TitleResource = ("$SettingPage_Key_Move_Left"),                              
                    Group = 1,                               
                    Key = VirtualKey.Left,                              
                    Invoked = () =>                              
                    {                             
                        App.ViewModel.CanvasTransformer.Position += new Vector2(50, 0);                                
                        App.ViewModel.CanvasTransformer.ReloadMatrix();                                
                        App.ViewModel.Invalidate();//Invalidate                          
                    }
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("$SettingPage_Key_Move_Top"),
                    Group = 1,
                    Key = VirtualKey.Up,
                    Invoked = () =>
                    {
                        App.ViewModel.CanvasTransformer.Position += new Vector2(0, 50);
                        App.ViewModel.CanvasTransformer.ReloadMatrix();
                        App.ViewModel.Invalidate();//Invalidate
                    }                
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("$SettingPage_Key_Move_Right"),
                    Group = 1,
                    Key = VirtualKey.Right,
                    Invoked = () =>
                    {
                        App.ViewModel.CanvasTransformer.Position -= new Vector2(50, 0);
                        App.ViewModel.CanvasTransformer.ReloadMatrix();
                        App.ViewModel.Invalidate();//Invalidate
                    }
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("$SettingPage_Key_Move_Bottom"),
                    Group = 1,
                    Key = VirtualKey.Down,
                    Invoked = () =>
                    {                               
                        App.ViewModel.CanvasTransformer.Position -= new Vector2(50, 0);
                        App.ViewModel.CanvasTransformer.ReloadMatrix();
                        App.ViewModel.Invalidate();//Invalidate
                    }
                },


                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Edit_Cut"),
                    Group = 2,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.X,
                    Invoked = App.MethodViewModel.MethodEditCut,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Edit_Duplicate"),
                    Group = 2,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.J,
                    Invoked = App.MethodViewModel.MethodEditDuplicate,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Edit_Copy"),
                    Group = 2,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.C,
                    Invoked = App.MethodViewModel.MethodEditCopy,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Edit_Paste"),
                    Group = 2,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.V,
                    Invoked = App.MethodViewModel.MethodEditPaste,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Edit_Clear"),
                    Group = 2,
                    Key = VirtualKey.Delete,
                    Invoked = App.MethodViewModel.MethodEditClear,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Select_All"),
                    Group = 2,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.A,
                    Invoked = App.MethodViewModel.MethodSelectAll,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Select_Deselect"),
                    Group = 2,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.D,
                    Invoked = App.MethodViewModel.MethodSelectDeselect,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Select_Invert"),
                    Group = 2,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.I,
                    Invoked = App.MethodViewModel.MethodSelectInvert,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Group_Group"),
                    Group = 2,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.G,
                    Invoked = App.MethodViewModel.MethodGroupGroup,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Group_Ungroup"),
                    Group = 2,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.U,
                    Invoked = App.MethodViewModel.MethodGroupUngroup,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("Edits_Group_Release"),
                    Group = 2,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.R,
                    Invoked = App.MethodViewModel.MethodGroupRelease,
                },


                new KeyboardAccelerator2
                {
                    TitleResource = ("$DrawPage_Export"),
                    Group = 3,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.E,
                    Invoked = () => Retouch_Photo2.DrawPage.ShowExport?.Invoke()
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("$DrawPage_Undo"),
                    Group = 3,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.Z,
                    Invoked = App.MethodViewModel.MethodEditUndo,
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("$DrawPage_FullScreen"),
                    Group = 3,
                    Key = VirtualKey.Escape,
                    Invoked = () => Retouch_Photo2.DrawPage.FullScreen?.Invoke()
                },
                new KeyboardAccelerator2
                {
                    TitleResource = ("$DrawPage_Gallery"),
                    Group = 3,
                    Modifiers = VirtualKeyModifiers2.Control,
                    Key = VirtualKey.P,
                    Invoked = () => Retouch_Photo2.DrawPage.ShowGallery?.Invoke()
                },
            }
        };
                  
        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.TipViewModel" />. </summary>
        public static TipViewModel TipViewModel = new TipViewModel
        {
            //Tool
            MoveTool = new MoveTool(),
            TransformerTool = new TransformerTool(),
            CreateTool = new CreateTool(),
            Tools = new List<ITool>
            {
                new CursorTool(),
                null,

                new ViewTool(),
                new BrushTool(),
                new TransparencyTool(),
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

                new PatternGridTool(),
                new PatternDiagonalTool(),
                new PatternSpottedTool(),
                null,

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
                //new DebugMenu(),

                new EditMenu(),
                new OperateMenu(),

                new AdjustmentMenu(),
                new EffectMenu(),

                new TextMenu(),
                //new ParagraphMenu(),

                new StrokeMenu(),
                new StyleMenu(),

                new HistoryMenu(),
                new TransformerMenu(),

                new LayerMenu(),
                new ColorMenu(),
                //new KeyboardMenu(),
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
