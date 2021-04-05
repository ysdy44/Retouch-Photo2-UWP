using Retouch_Photo2.Elements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    internal class UIElementGroupingList : List<UIElementGrouping> { }
    internal class UIElementGrouping : IGrouping<string, UIElement>
    {
        //@String
        static readonly ResourceLoader resource = ResourceLoader.GetForCurrentView();
        public string Title => string.IsNullOrEmpty(Key) ? string.Empty : UIElementGrouping.resource.GetString($"{Key}");

        public string Key { set; get; }
        public List<UIElement> Items { get; set; } = new List<UIElement>();

        public IEnumerator<UIElement> GetEnumerator() => this.Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.Items.GetEnumerator();
    }


    /// <summary> 
    /// Represents a page used to debug.
    /// </summary>
    public sealed partial class DebugPage : Page
    {

        //FlowDirection
        private void ConstructFlowDirection()
        {
            bool isRightToLeft = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;

            base.FlowDirection = isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        private float SizeWidth = 600;
        private float SizeHeight = 400;
        readonly List<Bug> Bugs = new List<Bug>();

        //@Construct
        /// <summary>
        /// Initializes a DebugPage. 
        /// </summary>
        public DebugPage()
        {
            this.InitializeComponent();
            this.ConstructFlowDirection();
            this.Head.LeftButtonClick += (s, e) => this.Frame.GoBack();
            this.Head.RightButtonClick += (s, e) => this.CanvasAnimatedControl.Paused = !this.CanvasAnimatedControl.Paused;
            this.ScrollViewer.ViewChanged += (s, e) => this.Head.Move(this.ScrollViewer.VerticalOffset);

            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.SizeWidth = (float)e.NewSize.Width;
                this.SizeHeight = (float)e.NewSize.Height;
            };

            this.CanvasAnimatedControl.Paused = true;
            this.CanvasAnimatedControl.CreateResources += (sender, args) =>
            {
                int width = (int)this.ActualWidth;
                int height = (int)this.ActualHeight;

                for (int i = 0; i < 64; i++)
                {
                    this.Bugs.Add(new Bug(width, height));
                }
            };
            this.CanvasAnimatedControl.Draw += (sender, args) =>
            {
                foreach (Bug bug in this.Bugs)
                {
                    args.DrawingSession.FillCircle(bug.Position, 32, bug.Color);
                }
            };
            this.CanvasAnimatedControl.Update += (sender, args) =>
            {
                foreach (Bug bug in this.Bugs)
                {
                    bug.UpdatePosition(this.SizeWidth, this.SizeHeight);
                }
            };
        }
    }


    public sealed partial class DebugPage : Page
    {

        //@BackRequested
        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
        }
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= BackRequested;
        }
        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (BackRequestedExtension.DialogIsShow) return;
            if (BackRequestedExtension.LayoutIsShow) return;

            e.Handled = true;
            this.Frame.GoBack();
        }

    }
}