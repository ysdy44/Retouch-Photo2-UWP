using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2.TestApp
{
    sealed partial class App : Application
    {
        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.ViewModel" />. </summary>
        public static ViewModel ViewModel = new ViewModel();

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.Keyboards.KeyboardViewModel" />. </summary>
        public static KeyboardViewModel KeyboardViewModel = new KeyboardViewModel();

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.Selections.SelectionViewModel" />. </summary>
        public static SelectionViewModel SelectionViewModel = new SelectionViewModel();

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.MezzanineViewModel" />. </summary>
        public static MezzanineViewModel MezzanineViewModel = new MezzanineViewModel();

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModels.Tips.TipViewModel" />. </summary>
        public static TipViewModel TipViewModel = new TipViewModel(new NoneTool())
        {
            TransformerToolBase=new TransformerToolBase(),

            ViewTool = new ViewTool(),
            RectangleTool = new RectangleTool(),
            EllipseTool = new EllipseTool(),
            CursorTool = new CursorTool(),
        };


        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }
        
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            
            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(Pages.MainPage), e.Arguments);
                }
                Window.Current.Activate();
            }
        }
        
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
