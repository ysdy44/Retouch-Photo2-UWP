using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "GrayAdjustment"/>.
    /// </summary>
    public sealed partial class GrayPage : IAdjustmentPage
    {
        public GrayAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.Gray;
        public FrameworkElement Icon { get; } = new GrayIcon();
        public FrameworkElement Self => null;
        public string Text { get; private set; }

        //@Construct
        public GrayPage()
        {
            this.ConstructStrings();
        }


        public IAdjustment GetNewAdjustment() => new GrayAdjustment();
        

        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Gray");
        }

    }
}