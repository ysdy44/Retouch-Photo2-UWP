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

using System.Collections.ObjectModel;
using Windows.Storage;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {

        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.IProjectViewItem" />. </summary>
        public static ObservableCollection<IProjectViewItem> Projects { get; } = new ObservableCollection<IProjectViewItem>();

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.ViewModel" />. </summary>
        public static ViewModel ViewModel { get; } = new ViewModel
        {
            //Tool
            MoveTool = new MoveTool(),
            TransformerTool = new TransformerTool(),
            CreateTool = new CreateTool(),
            ClickeTool = new ClickeTool(),
        };

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


        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    //FileUtil
                    await FileUtil.DeleteAllInTemporaryFolder();

                    //Projects 
                    foreach (StorageFolder zipFolder in await FileUtil.FIndAllZipFolders())
                    {
                        // [StorageFolder] --> [projectViewItem]
                        IProjectViewItem item = await FileUtil.ConstructProjectViewItem(zipFolder);
                        App.Projects.Add(item);
                    }

                    //Setting
                    Setting setting = await XML.ConstructSettingFile();
                    App.SettingViewModel.ConstructSetting(setting);

                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                    // Ensure the current window is active
                    Window.Current.Activate();
                }
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }
    }
}
