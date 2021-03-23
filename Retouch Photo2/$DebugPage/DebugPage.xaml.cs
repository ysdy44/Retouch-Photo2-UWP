using Retouch_Photo2.Elements;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    internal class UIElementGroupingList : List<UIElementGrouping> { }
    internal class UIElementGrouping : IGrouping<string, UIElement>
    {
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


        //@Construct
        /// <summary>
        /// Initializes a DebugPage. 
        /// </summary>
        public DebugPage()
        {
            this.InitializeComponent();
            this.ConstructFlowDirection();
            this.Head.LeftButtonClick += (s, e) => this.Frame.GoBack();
            this.ScrollViewer.ViewChanged += (s, e) => this.Head.Move(this.ScrollViewer.VerticalOffset);
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